using DocumentFormat.OpenXml.Math;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Rehberim
{
    public partial class KullaniciEkle : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UserID"] != null)
                {
                    string kullaniciAdi = Session["UserID"].ToString();

                    string userRole = GetUserRole(kullaniciAdi);

                    if (SayfaIzinliMi("KullaniciEkle", userRole))
                    {
                        BindGridView();

                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "showError", "alert('Bu sayfaya erişim izniniz yok.'); window.location.href = 'AnaSayfa.aspx';", true);
                    }



                }
            }
        }


        protected string GetUserRole(string kullaniciAdi)
        {
            string userRole = "kullanıcı"; 

            string connectionString = ConfigurationManager.ConnectionStrings["RehberConnectionString"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string selectQuery = "SELECT Rol FROM Kullanicilar WHERE KullaniciAdi = @KullaniciAdi";
                SqlCommand command = new SqlCommand(selectQuery, connection);
                command.Parameters.AddWithValue("@KullaniciAdi", kullaniciAdi);
                connection.Open();
                object roleObj = command.ExecuteScalar();

                if (roleObj != null)
                {
                    userRole = roleObj.ToString();
                }
            }

            return userRole;
        }
        private bool SayfaIzinliMi(string sayfaAdi, string userRole)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["RehberConnectionString"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string selectQuery = "SELECT COUNT(*) FROM Rol_Sayfalar RS " +
                                     "INNER JOIN Sayfalar S ON RS.SayfaID = S.SayfaID " +
                                     "INNER JOIN Roller R ON RS.RolID = R.RolID " +
                                     "WHERE S.SayfaAdi = @SayfaAdi AND R.RolAdi = @UserRole";

                SqlCommand command = new SqlCommand(selectQuery, connection);
                command.Parameters.AddWithValue("@SayfaAdi", sayfaAdi);
                command.Parameters.AddWithValue("@UserRole", userRole);
                connection.Open();

                int rowCount = (int)command.ExecuteScalar();
                return rowCount > 0;
            }
        }

        protected void btnKaydet_Click(object sender, EventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["RehberConnectionString"].ConnectionString;

            string kullaniciAdi = TextKullaniciAdi.Text;
            string sifre = txtSifre.Text;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string insertQuery = "INSERT INTO Kullanicilar (KullaniciAdi, Sifre) VALUES (@KullaniciAdi, @Sifre)";
                SqlCommand cmd = new SqlCommand(insertQuery, connection);

                cmd.Parameters.AddWithValue("@KullaniciAdi", kullaniciAdi);
                cmd.Parameters.AddWithValue("@Sifre", sifre);

                cmd.ExecuteNonQuery();

                connection.Close();
            }
            BindGridView();
            lblMesaj.Text = "Kullanıcı başarıyla eklendi.";
            lblMesaj.ForeColor = System.Drawing.Color.Green;
            lblMesaj.Visible = true;
        }


        private DataTable GetKullanicilar()
        {
            DataTable dt = new DataTable();
            string connectionString = ConfigurationManager.ConnectionStrings["RehberConnectionString"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string selectQuery = "SELECT KullaniciAdi FROM Kullanicilar";

                SqlCommand cmd = new SqlCommand(selectQuery, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                adapter.Fill(dt);
            }

            return dt;
        }

        private void BindGridView()
        {
            DataTable dt = GetKullanicilar();
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Sil")
            {
                string kullaniciAdi = e.CommandArgument.ToString();

                bool isDeleted = SilKullanici(kullaniciAdi);

                if (isDeleted)
                {
                    lblMesaj2.Text = kullaniciAdi + " adlı kullanıcı başarıyla silindi.";
                    lblMesaj2.ForeColor = System.Drawing.Color.Green;
                    lblMesaj2.Visible = true;

                    BindGridView();
                }
                else
                {
                    lblMesaj.Text = "Kullanıcı silme işlemi sırasında bir hata oluştu.";
                    lblMesaj.ForeColor = System.Drawing.Color.Red;
                    lblMesaj.Visible = true;
                }
            }
        }

        private bool SilKullanici(string kullaniciAdi)
        {
            

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["RehberConnectionString"].ConnectionString;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string deleteQuery = "DELETE FROM Kullanicilar WHERE KullaniciAdi = @KullaniciAdi";
                    SqlCommand cmd = new SqlCommand(deleteQuery, connection);
                    cmd.Parameters.AddWithValue("@KullaniciAdi", kullaniciAdi);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    return rowsAffected > 0;
                }
            }
            catch (Exception)
            {
                return false; 
            }
        }

        



    }
}
