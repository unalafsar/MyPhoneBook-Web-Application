using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace Rehberim
{
    public partial class KisiDetay : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                
                if (Session["UserID"] != null)
                {
                    string kullaniciAdi = Session["UserID"].ToString();

                    string userRole = GetUserRole(kullaniciAdi);

                    if (SayfaIzinliMi("KisiDetay", userRole))
                    {

                        if (Request.QueryString["ID"] != null)
                        {
                            int kisiID = Convert.ToInt32(Request.QueryString["ID"]);
                            GetAndDisplayKisiDetails(kisiID);
                        }
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

        

        private void GetAndDisplayKisiDetails(int kisiID)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["RehberConnectionString"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT Ad, Soyad, TelNo, Mail, CalistigiYer, KimlikNo, Aciklama, FotografYolu FROM Kisiler WHERE ID = @KisiID";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@KisiID", kisiID);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        lblAd.Text = reader["Ad"].ToString();
                        lblSoyad.Text = reader["Soyad"].ToString();
                        lblTelNo.Text = reader["TelNo"].ToString();
                        lblMail.Text = reader["Mail"].ToString();
                        lblCalistigiYer.Text = reader["CalistigiYer"].ToString();
                        lblKimlikNo.Text = reader["KimlikNo"].ToString();
                        lblAciklama.Text = reader["Aciklama"].ToString();


                        string fotografYolu = reader["FotografYolu"].ToString();
                        if (!string.IsNullOrEmpty(fotografYolu))
                        {
                            imgFoto.ImageUrl = fotografYolu + "?v=" + Guid.NewGuid();
                        }
                    }

                    reader.Close();
                }
            }
        }


    }
}
