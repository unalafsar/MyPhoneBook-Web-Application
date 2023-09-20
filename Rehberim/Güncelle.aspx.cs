using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Collections.Generic;

namespace Rehberim
{
    public partial class Guncelle : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                
                if (Session["UserID"] != null)
                {
                    string kullaniciAdi = Session["UserID"].ToString();

                    string userRole = GetUserRole(kullaniciAdi);

                    if (SayfaIzinliMi("Güncelle", userRole))
                    {
                        if (Request.QueryString["ID"] != null)
                        {
                            int id = Convert.ToInt32(Request.QueryString["ID"]);
                            PopulatePersonData(id);
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
        private void PopulatePersonData(int id)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["RehberConnectionString"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string selectQuery = "SELECT Ad, Soyad, TelNo, Mail, CalistigiYer, KimlikNo, Aciklama FROM Kisiler WHERE ID = @Id";
                SqlCommand command = new SqlCommand(selectQuery, connection);
                command.Parameters.AddWithValue("@Id", id);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    txtAd.Text = reader["Ad"].ToString();
                    txtSoyad.Text = reader["Soyad"].ToString();
                    txtTelNo.Text = reader["TelNo"].ToString();
                    txtMail.Text = reader["Mail"].ToString();
                    txtCalistigiYer.Text = reader["CalistigiYer"].ToString();
                    txtKimlikNo.Text = reader["KimlikNo"].ToString();
                    txtAciklama.Text = reader["Aciklama"].ToString();
                }
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(Request.QueryString["ID"]);
            string ad = txtAd.Text.Trim();
            string soyad = txtSoyad.Text.Trim();
            string telNo = txtTelNo.Text.Trim();
            string mail = txtMail.Text.Trim();
            string calistigiYer = txtCalistigiYer.Text.Trim();
            string kimlikNo = txtKimlikNo.Text.Trim();
            string Aciklama = txtAciklama.Text.Trim();
            string kullaniciAdi = Session["UserID"] as string; 

            string connectionString = ConfigurationManager.ConnectionStrings["RehberConnectionString"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string oldDataQuery = "SELECT Ad, Soyad, TelNo, Mail, CalistigiYer, KimlikNo, Aciklama FROM Kisiler WHERE ID = @Id";
                using (SqlCommand oldDataCommand = new SqlCommand(oldDataQuery, connection))
                {
                    oldDataCommand.Parameters.AddWithValue("@Id", id);
                    using (SqlDataReader reader = oldDataCommand.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string oldAd = reader["Ad"].ToString();
                            string oldSoyad = reader["Soyad"].ToString();
                            string oldTelNo = reader["TelNo"].ToString();
                            string oldMail = reader["Mail"].ToString();
                            string oldCalistigiYer = reader["CalistigiYer"].ToString();
                            string oldKimlikNo = reader["KimlikNo"].ToString();
                            string oldAciklama = reader["Aciklama"].ToString();
                            reader.Close(); 

                            string updateQuery = "UPDATE Kisiler SET Ad = @Ad, Soyad = @Soyad, TelNo = @TelNo, Mail = @Mail, CalistigiYer = @CalistigiYer, KimlikNo = @KimlikNo, Aciklama = @Aciklama WHERE ID = @Id";
                            using (SqlCommand command = new SqlCommand(updateQuery, connection))
                            {
                                command.Parameters.AddWithValue("@Ad", ad);
                                command.Parameters.AddWithValue("@Soyad", soyad);
                                command.Parameters.AddWithValue("@TelNo", telNo);
                                command.Parameters.AddWithValue("@Mail", mail);
                                command.Parameters.AddWithValue("@CalistigiYer", calistigiYer);
                                command.Parameters.AddWithValue("@KimlikNo", kimlikNo);
                                command.Parameters.AddWithValue("@Id", id);
                                command.Parameters.AddWithValue("@Aciklama", Aciklama);
                                command.ExecuteNonQuery();

                                List<string> changedFields = new List<string>();
                                if (ad != oldAd) changedFields.Add("Ad: " + oldAd + "->"+ ad);
                                if (soyad != oldSoyad) changedFields.Add("Soyad: " + oldSoyad + "->"+ soyad);
                                if (telNo != oldTelNo) changedFields.Add("Tel No: " + oldTelNo + "->" + telNo);
                                if (mail != oldMail) changedFields.Add("Mail: " + oldMail + "->"+ mail);
                                if (calistigiYer != oldCalistigiYer) changedFields.Add("Çalıştığı Yer: " + oldCalistigiYer + "->"+ calistigiYer);
                                if (kimlikNo != oldKimlikNo) changedFields.Add("Kimlik No: " + oldKimlikNo + "->" + kimlikNo);
                                if (Aciklama != oldAciklama) changedFields.Add("Açıklama: " + oldAciklama + "->" + Aciklama);

                                string changedFieldsString = string.Join(", ", changedFields);
                                string changeDetail = $"Değişen Alanlar => {changedFieldsString}";
                                LogTransaction(connection, kullaniciAdi, "Güncelleme", $"Kişi Güncellendi -  Ad: {ad}, Soyad: {soyad}.   {changeDetail}");
                            }
                        }
                    }
                }
            }

            
            lblUpdateMessage.Visible = true; 
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "HideMessage", "setTimeout(function(){ document.getElementById('" + lblUpdateMessage.ClientID + "').style.display = 'none'; }, 3000);", true);


           


            lblUpdateMessage.Visible = true; 

            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "HideMessage", "setTimeout(function(){ document.getElementById('" + lblUpdateMessage.ClientID + "').style.display = 'none'; }, 3000);", true);
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

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            string kullaniciAdi = Session["UserID"] as string; 


            if (fileUpload.HasFile)
            {
                string kişiAdı = txtAd.Text;
                string soyad = txtSoyad.Text.Trim();
                string klasörYolu = Server.MapPath("~/KisiFotograf/" + kişiAdı);

                if (!Directory.Exists(klasörYolu))
                {
                    Directory.CreateDirectory(klasörYolu);
                }

                int fotoSayısı = Directory.GetFiles(klasörYolu, "*.*", SearchOption.TopDirectoryOnly).Length;
                string dosyaAdı = kişiAdı + fotoSayısı + Path.GetExtension(fileUpload.FileName);
                string dosyaYolu = Path.Combine(klasörYolu, dosyaAdı);
                fileUpload.SaveAs(dosyaYolu);

                ImageResizer.ImageBuilder.Current.Build(dosyaYolu, dosyaYolu, new ImageResizer.ResizeSettings("width=400&height=400&crop=auto"));

                uploadedImage.ImageUrl = "~/KisiFotograf/" + kişiAdı + "/" + dosyaAdı + "?v=" + Guid.NewGuid();

                string connectionString = ConfigurationManager.ConnectionStrings["RehberConnectionString"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string updateQuery = "UPDATE Kisiler SET FotografYolu = @FotografYolu WHERE ID = @Id";
                    SqlCommand command = new SqlCommand(updateQuery, connection);
                    command.Parameters.AddWithValue("@FotografYolu", "~/KisiFotograf/" + kişiAdı + "/" + dosyaAdı);
                    command.Parameters.AddWithValue("@Id", Convert.ToInt32(Request.QueryString["ID"]));

                    command.ExecuteNonQuery();

                    string logMessage = $"Yeni Fotoğraf Eklendi - Ad: {kişiAdı}, Soyad: {soyad} Fotoğraf Yolu: ~/KisiFotograf/{kişiAdı}/{dosyaAdı}";
                    LogTransaction(connection, kullaniciAdi, "Yeni Fotoğraf Ekleme", logMessage);
                }
            }
        }





    }
}
