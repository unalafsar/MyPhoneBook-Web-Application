using System;
using System.Web;
using System.Web.UI;
using System.Data.SqlClient;
using System.Configuration;

namespace Rehberim
{
    public partial class GirişYap : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                
                if (Session["UserID"] != null)
                {
                    
                }
                else
                {
                    
                }
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string KullaniciAdi = txtKullaniciAdi.Text;
            string Sifre = txtPassword.Text;

            string connectionString = ConfigurationManager.ConnectionStrings["RehberConnectionString"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT COUNT(*) FROM Kullanicilar WHERE KullaniciAdi = @KullaniciAdi AND Sifre = @Sifre";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@KullaniciAdi", KullaniciAdi);
                    command.Parameters.AddWithValue("@Sifre", Sifre);

                    int result = (int)command.ExecuteScalar();

                    if (result > 0)
                    {
                        
                        Session["UserID"] = KullaniciAdi;
                        string logQuery = "INSERT INTO LogKayitlari (KullaniciAdi, IslemTuru, IslemDetayi, IslemZamani)   VALUES (@KullaniciAdi, @IslemTuru, @IslemDetayi, @IslemZamani)";
                        using (SqlCommand logCommand = new SqlCommand(logQuery, connection))
                        {
                            logCommand.Parameters.AddWithValue("@KullaniciAdi", KullaniciAdi);
                            logCommand.Parameters.AddWithValue("@IslemTuru", "Giriş");
                            logCommand.Parameters.AddWithValue("@IslemDetayi", "Başarılı Giriş");
                            logCommand.Parameters.AddWithValue("@IslemZamani", DateTime.Now);

                            logCommand.ExecuteNonQuery();
                        }

                        Response.Redirect("~/Anasayfa.aspx");
                    }
                    else
                    {
                        lblMessage.ForeColor = System.Drawing.Color.Red;
                        lblMessage.Text = "Kullanıcı adı veya şifre hatalı!";

                        string logQuery = "INSERT INTO LogKayitlari (KullaniciAdi, IslemTuru, IslemDetayi, IslemZamani)   VALUES (@KullaniciAdi, @IslemTuru, @IslemDetayi, @IslemZamani)";
                        using (SqlCommand logCommand = new SqlCommand(logQuery, connection))
                        {
                            logCommand.Parameters.AddWithValue("@KullaniciAdi", KullaniciAdi);
                            logCommand.Parameters.AddWithValue("@IslemTuru", "Giriş");
                            logCommand.Parameters.AddWithValue("@IslemDetayi", "Başarısız Giriş Denemesi");
                            logCommand.Parameters.AddWithValue("@IslemZamani", DateTime.Now);

                            logCommand.ExecuteNonQuery();
                        }
                    }
                }
            }
        }








        protected void btnSifreDegistir_Click(object sender, EventArgs e)
        {
            string KullaniciAdi = txtKullaniciAdii.Text;
            string MevcutSifre = txtMevcutSifre.Text;
            string YeniSifre = txtYeniSifre.Text;
            string YeniSifreTekrar = txtYeniSifreTekrar.Text;

            if (KontrolMevcutSifre(KullaniciAdi, MevcutSifre))
            {
                if (YeniSifre == YeniSifreTekrar)
                {
                    if (SifreGuncelle(KullaniciAdi, MevcutSifre, YeniSifre ))
                    {
                        lblMessage2.ForeColor = System.Drawing.Color.Green;
                        lblMessage2.Text = "Şifre başarıyla değiştirildi.";
                    }
                    else
                    {
                        lblMessage2.ForeColor = System.Drawing.Color.Red;
                        lblMessage2.Text = "Şifre değiştirme sırasında bir hata oluştu.";
                    }
                }
                else
                {
                    lblMessage2.ForeColor = System.Drawing.Color.Red;
                    lblMessage2.Text = "Yeni şifreler uyuşmuyor.";
                }
            }
            else
            {
                lblMessage2.ForeColor = System.Drawing.Color.Red;
                lblMessage2.Text = "Mevcut şifre hatalı.";
            }
        }
        private bool KontrolMevcutSifre(string kullaniciAdi, string mevcutSifre)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["RehberConnectionString"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT COUNT(*) FROM Kullanicilar WHERE KullaniciAdi = @KullaniciAdi AND Sifre = @MevcutSifre";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@KullaniciAdi", kullaniciAdi);
                    command.Parameters.AddWithValue("@MevcutSifre", mevcutSifre);

                    int result = (int)command.ExecuteScalar();

                    return result > 0;
                }
            }
        }

        private bool SifreGuncelle(string kullaniciAdi, string mevcutSifre, string yeniSifre)
        {
            if (KontrolMevcutSifre(kullaniciAdi, mevcutSifre))
            {
                string connectionString = ConfigurationManager.ConnectionStrings["RehberConnectionString"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "UPDATE Kullanicilar SET Sifre = @YeniSifre WHERE KullaniciAdi = @KullaniciAdi";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@KullaniciAdi", kullaniciAdi);
                        command.Parameters.AddWithValue("@YeniSifre", yeniSifre);

                        int rowsAffected = command.ExecuteNonQuery();

                        return rowsAffected > 0;
                    }
                }
            }
            else
            {
                return false;
            }
        }




    }
}
