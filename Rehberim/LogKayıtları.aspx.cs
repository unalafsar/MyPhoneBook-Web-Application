using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Data;
using System.Web.UI;

namespace Rehberim
{
    public partial class LogKayıtları : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                if (Session["UserID"] != null)
                {
                    string kullaniciAdi = Session["UserID"].ToString();

                    string userRole = GetUserRole(kullaniciAdi);

                    if (SayfaIzinliMi("LogKayıtları", userRole))
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


        protected void DropDownListSiralama_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedValue = DropDownListSiralama.SelectedValue;

            if (selectedValue == "Artan")
            {
                SortGridView("IslemZamani ASC");
            }
            else if (selectedValue == "Azalan")
            {
                SortGridView("IslemZamani DESC");
            }
        }

        private void SortGridView(string sortOrder)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["RehberConnectionString"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = $"SELECT KullaniciAdi, IslemTuru, IslemDetayi, IslemZamani FROM LogKayitlari ORDER BY {sortOrder}";
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                GridViewLog.DataSource = reader;
                GridViewLog.DataBind();
                reader.Close();
            }
        }


        protected void ButtonFiltrele_Click(object sender, EventArgs e)
        {
            string kullaniciAdi = TextBoxKullaniciAdi.Text.Trim();

            string filterQuery = "SELECT KullaniciAdi, IslemTuru, IslemDetayi, IslemZamani FROM LogKayitlari WHERE KullaniciAdi LIKE '%' + @KullaniciAdi + '%'";

            string connectionString = ConfigurationManager.ConnectionStrings["RehberConnectionString"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(filterQuery, connection);
                command.Parameters.AddWithValue("@KullaniciAdi", kullaniciAdi);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                GridViewLog.DataSource = reader;
                GridViewLog.DataBind();
                reader.Close();
            }
        }


        private void BindGridView()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["RehberConnectionString"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT KullaniciAdi, IslemTuru, IslemDetayi, IslemZamani FROM LogKayitlari";
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                GridViewLog.DataSource = reader;
                GridViewLog.DataBind();
                reader.Close();
            }
        }
    }
}
