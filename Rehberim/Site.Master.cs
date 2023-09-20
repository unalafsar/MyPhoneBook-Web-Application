using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;
using System;

namespace Rehberim

{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Kullanıcı oturumu varsa navbar öğelerini etkinleştir
                if (Session["UserID"] != null)
                {
                    navAnaSayfa.Visible = true;
                    navKisileriGoruntule.Visible = true;
                    navKisiEkle.Visible = true;
                    navLogKayıtları.Visible = true;
                    navRolAyarlama.Visible = true;
                    navKullaniciEkle.Visible = true; ;
                    btnGiris.Visible = false; // Giriş butonunu gizle
                    btnCikis.Visible = true; // Çıkış butonunu göster
                }
                else
                {
                    navAnaSayfa.Visible = false;
                    navKisileriGoruntule.Visible = false;
                    navKisiEkle.Visible = false;
                    navLogKayıtları.Visible = false;
                    navRolAyarlama.Visible = false;
                    navKullaniciEkle.Visible = false; ;
                    btnGiris.Visible = true; // Giriş butonunu göster
                    btnCikis.Visible = false; // Çıkış butonunu gizle
                }
            }
        }

        protected void btnGiris_Click(object sender, EventArgs e)
        {
            
        }

        protected void btnCikis_Click(object sender, EventArgs e)
        {
            string kullaniciAdi = Session["UserID"] as string;

            Session.Clear(); 

            if (!string.IsNullOrEmpty(kullaniciAdi))
            {
                string connectionString = ConfigurationManager.ConnectionStrings["RehberConnectionString"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string logQuery = "INSERT INTO LogKayitlari (KullaniciAdi, IslemTuru, IslemDetayi, IslemZamani) VALUES (@KullaniciAdi, @IslemTuru, @IslemDetayi, @IslemZamani)";
                    using (SqlCommand logCommand = new SqlCommand(logQuery, connection))
                    {
                        logCommand.Parameters.AddWithValue("@KullaniciAdi", kullaniciAdi);
                        logCommand.Parameters.AddWithValue("@IslemTuru", "Çıkış");
                        logCommand.Parameters.AddWithValue("@IslemDetayi", "Oturum Sonlandırıldı");
                        logCommand.Parameters.AddWithValue("@IslemZamani", DateTime.Now);

                        logCommand.ExecuteNonQuery();
                    }
                }
            }

            Response.Redirect("~/Anasayfa.aspx");
        }
    }
}
