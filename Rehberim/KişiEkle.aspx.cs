using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace Rehberim
{
    public partial class KisiEkle : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                if (Session["UserID"] != null)
                {
                    string kullaniciAdi = Session["UserID"].ToString();

                    string userRole = GetUserRole(kullaniciAdi);

                    if (SayfaIzinliMi("KişiEkle", userRole))
                    {

                        
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

        protected void btnAddPerson_Click(object sender, EventArgs e)
        {

            string kullaniciAdi = Session["UserID"] as string; 

            string ad = txtAd.Text;
            string soyad = txtSoyad.Text;
            string telNo = txtTelNo.Text;
            string mail = txtMail.Text;
            string calistigiYer = txtCalistigiYer.Text;
            string KimlikNo = txtKimlikNo.Text;



            
            string connectionString = ConfigurationManager.ConnectionStrings["RehberConnectionString"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string insertQuery = "INSERT INTO Kisiler (Ad, Soyad, TelNo, Mail, CalistigiYer, KimlikNo) VALUES (@Ad, @Soyad, @TelNo, @Mail, @CalistigiYer, @KimlikNo)";
                SqlCommand command = new SqlCommand(insertQuery, connection);
                command.Parameters.AddWithValue("@Ad", ad);
                command.Parameters.AddWithValue("@Soyad", soyad);
                command.Parameters.AddWithValue("@TelNo", telNo);
                command.Parameters.AddWithValue("@Mail", mail);
                command.Parameters.AddWithValue("@CalistigiYer", calistigiYer);
                command.Parameters.AddWithValue("@KimlikNo", KimlikNo);

                command.ExecuteNonQuery();

                LogTransaction(connection, kullaniciAdi, "Kişi Ekleme", $"Kişi Eklendi - Ad: {ad}, Soyad: {soyad}");

                lblMessage.Text = "Kişi başarıyla eklendi.";
                lblMessage.Visible = true;

                ClearPersonFields();

            }

           
        }
        private void LogTransaction(SqlConnection connection, string kullaniciAdi, string islemTuru, string islemDetay)
        {
            string logQuery = "INSERT INTO LogKayitlari (KullaniciAdi, IslemTuru, IslemDetayi, IslemZamani) VALUES (@KullaniciAdi, @IslemTuru, @IslemDetayi, GETDATE())";
            SqlCommand logCommand = new SqlCommand(logQuery, connection);
            logCommand.Parameters.AddWithValue("@KullaniciAdi", kullaniciAdi);
            logCommand.Parameters.AddWithValue("@IslemTuru", islemTuru);
            logCommand.Parameters.AddWithValue("@IslemDetayi", islemDetay);
            logCommand.Parameters.AddWithValue("@IslemZamani", DateTime.Now); 

            logCommand.ExecuteNonQuery();
        }

        private void ClearPersonFields()
        {
            txtAd.Text = "";
            txtSoyad.Text = "";
            txtTelNo.Text = "";
            txtMail.Text = "";
            txtCalistigiYer.Text = "";
            txtKimlikNo.Text = "";
        }
    }
}
