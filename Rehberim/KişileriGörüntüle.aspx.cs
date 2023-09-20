using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ClosedXML.Excel;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace Rehberim
{
    public partial class KisileriGoruntule : Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
               
                if (Session["UserID"] != null)
                {
                    string kullaniciAdi = Session["UserID"].ToString();

                    
                    string userRole = GetUserRole(kullaniciAdi);

                    
                    if (SayfaIzinliMi("KişileriGörüntüle", userRole))
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

        protected void BindGridView()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["RehberConnectionString"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string selectQuery = "SELECT ID, Ad, Soyad, TelNo, Mail, CalistigiYer, KimlikNo FROM Kisiler";
                SqlDataAdapter adapter = new SqlDataAdapter(selectQuery, connection);

                DataTable dt = new DataTable();
                adapter.Fill(dt);
                gvKisiler.DataSource = dt;
                gvKisiler.DataBind();
            }
        }


        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string searchText = txtSearch.Text.Trim();
            DataTable dt = new DataTable();

             
            string connectionString = ConfigurationManager.ConnectionStrings["RehberConnectionString"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string searchQuery = "SELECT ID, Ad, Soyad, TelNo, Mail, CalistigiYer, KimlikNo FROM Kisiler WHERE (Ad LIKE @SearchText + '%' OR Soyad LIKE @SearchText + '%' OR TelNo LIKE '%' + @SearchText + '%')";
                SqlDataAdapter adapter = new SqlDataAdapter(searchQuery, connection);
                adapter.SelectCommand.Parameters.AddWithValue("@SearchText", searchText);

                adapter.Fill(dt);
                gvKisiler.DataSource = dt;
                gvKisiler.DataBind();
            }
        }



        protected void btnShowAll_Click(object sender, EventArgs e)
        {
            BindGridView(); 
            txtSearch.Text = ""; 
        }

        protected void gvKisiler_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int rowIndex = e.RowIndex;
            int idToDelete = Convert.ToInt32(gvKisiler.DataKeys[rowIndex].Value);
            string kullaniciAdi = Session["UserID"] as string; 

            string connectionString = ConfigurationManager.ConnectionStrings["RehberConnectionString"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string deleteQuery = "DELETE FROM Kisiler WHERE ID = @Id";
                using (SqlCommand command = new SqlCommand(deleteQuery, connection))
                {
                    command.Parameters.AddWithValue("@Id", idToDelete);

                    Tuple<string, string> adSoyadTuple = GetAdSoyadById(idToDelete); 

                    string ad = adSoyadTuple.Item1;
                    string soyad = adSoyadTuple.Item2;

                    LogTransaction(connection, kullaniciAdi, "Silme", $"Kişi Silindi - Ad: {ad}, Soyad: {soyad}");


                    command.ExecuteNonQuery();                   


                    BindGridView();
                }
            }
        }

        private void LogTransaction(SqlConnection connection, string kullaniciAdi, string islemTuru, string islemDetay)
        {
            string logQuery = "INSERT INTO LogKayitlari (KullaniciAdi, IslemTuru, IslemDetayi, IslemZamani) VALUES (@KullaniciAdi, @IslemTuru, @IslemDetayi, GETDATE())";
            using (SqlCommand logCommand = new SqlCommand(logQuery, connection))
            {
                logCommand.Parameters.AddWithValue("@KullaniciAdi", kullaniciAdi);
                logCommand.Parameters.AddWithValue("@IslemTuru", islemTuru);
                logCommand.Parameters.AddWithValue("@IslemDetayi", islemDetay);
                logCommand.Parameters.AddWithValue("@IslemZamani", DateTime.Now);

                logCommand.ExecuteNonQuery();
            }
        }

        private Tuple<string, string> GetAdSoyadById(int id)
        {
            string ad = string.Empty;
            string soyad = string.Empty;

            string connectionString = ConfigurationManager.ConnectionStrings["RehberConnectionString"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string selectQuery = "SELECT Ad, Soyad FROM Kisiler WHERE ID = @Id";
                using (SqlCommand command = new SqlCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        reader.Read();
                        ad = reader["Ad"].ToString();
                        soyad = reader["Soyad"].ToString();
                    }

                    reader.Close();
                }
            }

            return Tuple.Create(ad, soyad);
        }



        protected void gvKisiler_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditRow")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                int id = Convert.ToInt32(gvKisiler.DataKeys[rowIndex].Value);
                Response.Redirect("Güncelle.aspx?ID=" + id); 
            }
            else if (e.CommandName == "Details")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                int id = Convert.ToInt32(gvKisiler.DataKeys[rowIndex].Value);
                Response.Redirect("KisiDetay.aspx?ID=" + id); 
            }

        }

        
        

        protected void btnSelectAll_Click(object sender, EventArgs e)
        {
            bool allSelected = true;

            foreach (GridViewRow row in gvKisiler.Rows)
            {
                CheckBox chkSelect = row.FindControl("chkSelect") as CheckBox;
                if (!chkSelect.Checked)
                {
                    allSelected = false;
                    break;
                }
            }

            foreach (GridViewRow row in gvKisiler.Rows)
            {
                CheckBox chkSelect = row.FindControl("chkSelect") as CheckBox;
                chkSelect.Checked = !allSelected;
            }
        }



        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {
            DataTable selectedData = new DataTable();
            selectedData.Columns.Add("Ad", typeof(string));
            selectedData.Columns.Add("Soyad", typeof(string));
            selectedData.Columns.Add("TelNo", typeof(string));
            selectedData.Columns.Add("Mail", typeof(string));
            selectedData.Columns.Add("CalistigiYer", typeof(string));
            selectedData.Columns.Add("KimlikNo", typeof(string));

            bool anySelected = false;

            foreach (GridViewRow row in gvKisiler.Rows)
            {
                CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");
                if (chkSelect.Checked)
                {
                    anySelected = true;

                    DataRow newRow = selectedData.NewRow();
                    newRow["Ad"] = HttpUtility.HtmlDecode(row.Cells[1].Text);
                    newRow["Soyad"] = HttpUtility.HtmlDecode(row.Cells[2].Text);
                    newRow["TelNo"] = HttpUtility.HtmlDecode(row.Cells[3].Text);
                    newRow["Mail"] = (row.FindControl("gvMailLink") as HyperLink).Text;
                    newRow["CalistigiYer"] = HttpUtility.HtmlDecode(row.Cells[5].Text);
                    newRow["KimlikNo"] = HttpUtility.HtmlDecode(row.Cells[6].Text);
                    selectedData.Rows.Add(newRow);
                }
            }

            if (!anySelected)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowAlert", "alert('Lütfen en az bir kişi seçin.');", true);
                return;
            }

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("SelectedPeople");

                int cellWidth = 20; 

                for (int i = 1; i <= selectedData.Columns.Count; i++)
                {
                    worksheet.Column(i).Width = cellWidth;
                }

                worksheet.Cells["A1"].LoadFromDataTable(selectedData, true);

                

                
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=Kisiler.xlsx");

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    excelPackage.SaveAs(memoryStream);
                    memoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
        }

    }
}
