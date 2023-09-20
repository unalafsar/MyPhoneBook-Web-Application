using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Rehberim
{
    public partial class RolAyarlama : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UserID"] != null)
                {
                    string kullaniciAdi = Session["UserID"].ToString();

                    string userRole = GetUserRole(kullaniciAdi);

                    if (SayfaIzinliMi("RolAyarlama", userRole))
                    {
                        BindGridViewKullanicilar();
                        BindGridViewRolSayfaAtama();
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "showError", "alert('Bu sayfaya erişim izniniz yok.'); window.location.href = 'AnaSayfa.aspx';", true);
                    }
                }
                else
                {
                    Response.Redirect("Giris.aspx"); 
                }
            }
        }

        protected string GetUserRole(string kullaniciAdi)
        {
            string userRole = "kullanici"; 

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


        protected void BindGridViewKullanicilar()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["RehberConnectionString"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string selectQuery = "SELECT ID, KullaniciAdi, Rol FROM Kullanicilar";
                SqlCommand command = new SqlCommand(selectQuery, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                GridViewKullanicilar.DataSource = reader;
                GridViewKullanicilar.DataBind();
                reader.Close();

                foreach (GridViewRow row in GridViewKullanicilar.Rows)
                {
                    DropDownList ddlRoles = (DropDownList)row.FindControl("DropDownList1");
                    if (ddlRoles != null)
                    {
                        ddlRoles.Items.Add(new ListItem("Rol Seçin", "0")); 
                        string rolesQuery = "SELECT RolAdi FROM Roller";
                        SqlCommand rolesCommand = new SqlCommand(rolesQuery, connection);
                        SqlDataReader rolesReader = rolesCommand.ExecuteReader();
                        while (rolesReader.Read())
                        {
                            ddlRoles.Items.Add(new ListItem(rolesReader["RolAdi"].ToString()));
                        }
                        rolesReader.Close();
                        ddlRoles.SelectedIndex = 0; 
                    }
                }
            }
        }

        protected void BindGridViewRolSayfaAtama()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["RehberConnectionString"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string selectRolesQuery = "SELECT RolID, RolAdi FROM Roller";
                SqlCommand rolesCommand = new SqlCommand(selectRolesQuery, connection);
                connection.Open();
                SqlDataReader rolesReader = rolesCommand.ExecuteReader();
                GridViewRolSayfaAtama.DataSource = rolesReader;
                GridViewRolSayfaAtama.DataBind();
                rolesReader.Close();

                foreach (GridViewRow row in GridViewRolSayfaAtama.Rows)
                {
                    DropDownList ddlSayfalar = (DropDownList)row.FindControl("DropDownListSayfalar");
                    if (ddlSayfalar != null)
                    {
                        ddlSayfalar.Items.Clear();

                        string selectSayfalarQuery = "SELECT SayfaID, SayfaAdi FROM Sayfalar";
                        SqlCommand sayfalarCommand = new SqlCommand(selectSayfalarQuery, connection);
                        SqlDataReader sayfalarReader = sayfalarCommand.ExecuteReader();
                        while (sayfalarReader.Read())
                        {
                            string sayfaID = sayfalarReader["SayfaID"].ToString();
                            string sayfaAdi = sayfalarReader["SayfaAdi"].ToString();

                            if (!SayfaVarMi(row.Cells[0].Text, sayfaID))
                            {
                                ddlSayfalar.Items.Add(new ListItem(sayfaAdi, sayfaID));
                            }
                        }
                        sayfalarReader.Close();
                    }
                }
            }
        }


        private bool SayfaVarMi(string rolAdi, string sayfaID)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["RehberConnectionString"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string selectQuery = "SELECT COUNT(*) FROM Rol_Sayfalar RS " +
                                     "INNER JOIN Sayfalar S ON RS.SayfaID = S.SayfaID " +
                                     "INNER JOIN Roller R ON RS.RolID = R.RolID " +
                                     "WHERE S.SayfaID = @SayfaID AND (R.RolAdi = @RolAdi OR R.RolAdi = 'Moderator')";

                SqlCommand command = new SqlCommand(selectQuery, connection);
                command.Parameters.AddWithValue("@SayfaID", sayfaID);
                command.Parameters.AddWithValue("@RolAdi", rolAdi);
                connection.Open();

                int rowCount = (int)command.ExecuteScalar();
                return rowCount > 0;
            }
        }



        protected string GetMevcutIzinliSayfalar(string rolAdi)
        {
            string mevcutIzinliSayfalar = string.Empty;

            string connectionString = ConfigurationManager.ConnectionStrings["RehberConnectionString"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string selectQuery = "SELECT DISTINCT S.SayfaAdi FROM Sayfalar S " +
                                     "INNER JOIN Rol_Sayfalar RS ON S.SayfaID = RS.SayfaID " +
                                     "INNER JOIN Roller R ON RS.RolID = R.RolID " +
                                     "WHERE R.RolAdi = @RolAdi";

                SqlCommand command = new SqlCommand(selectQuery, connection);
                command.Parameters.AddWithValue("@RolAdi", rolAdi);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string sayfaAdi = reader["SayfaAdi"].ToString();
                    mevcutIzinliSayfalar += sayfaAdi + ", ";
                }

                reader.Close();
            }

            if (!string.IsNullOrEmpty(mevcutIzinliSayfalar))
            {
                mevcutIzinliSayfalar = mevcutIzinliSayfalar.TrimEnd(',', ' ');
            }

            return mevcutIzinliSayfalar;
        }




        private int GetRolIDByRolAdi(string rolAdi)
        {
            int rolID = -1; 

            string connectionString = ConfigurationManager.ConnectionStrings["RehberConnectionString"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string selectQuery = "SELECT RolID FROM Roller WHERE RolAdi = @RolAdi";
                SqlCommand command = new SqlCommand(selectQuery, connection);
                command.Parameters.AddWithValue("@RolAdi", rolAdi);
                connection.Open();
                object result = command.ExecuteScalar();

                if (result != null && result != DBNull.Value)
                {
                    rolID = Convert.ToInt32(result);
                }
            }

            return rolID;
        }

        protected void btnKaydetRolSayfa_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            GridViewRow gridViewRow = (GridViewRow)btn.NamingContainer;

            DropDownList ddlSayfalar = (DropDownList)gridViewRow.FindControl("DropDownListSayfalar");
            string selectedSayfaID = ddlSayfalar.SelectedValue;
            string rolAdi = btn.CommandArgument;

            if (!string.IsNullOrEmpty(selectedSayfaID))
            {
                string connectionString = ConfigurationManager.ConnectionStrings["RehberConnectionString"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string checkExistenceQuery = "SELECT COUNT(*) FROM Rol_Sayfalar WHERE RolID = @RolID AND SayfaID = @SayfaID";
                    SqlCommand checkExistenceCommand = new SqlCommand(checkExistenceQuery, connection);
                    checkExistenceCommand.Parameters.AddWithValue("@RolID", GetRolIDByRolAdi(rolAdi));
                    checkExistenceCommand.Parameters.AddWithValue("@SayfaID", selectedSayfaID);
                    connection.Open();

                    int rowCount = (int)checkExistenceCommand.ExecuteScalar();

                    if (rowCount == 0)
                    {
                        string insertQuery = "INSERT INTO Rol_Sayfalar (RolID, SayfaID) VALUES (@RolID, @SayfaID)";
                        SqlCommand command = new SqlCommand(insertQuery, connection);
                        command.Parameters.AddWithValue("@RolID", GetRolIDByRolAdi(rolAdi));
                        command.Parameters.AddWithValue("@SayfaID", selectedSayfaID);
                        command.ExecuteNonQuery();

                       
                        BindGridViewRolSayfaAtama();
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alert('Bu sayfa zaten izinli!');", true);
                    }
                }
            }
        }







        protected void btnKaydet_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            GridViewRow gridViewRow = (GridViewRow)btn.NamingContainer; 

            DropDownList ddlRoles = (DropDownList)gridViewRow.FindControl("DropDownList1");
            string selectedRoleId = ddlRoles.SelectedValue;
            string userId = btn.CommandArgument;

            if (selectedRoleId != "0")
            {
                string connectionString = ConfigurationManager.ConnectionStrings["RehberConnectionString"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string updateQuery = "UPDATE Kullanicilar SET Rol = @Rol WHERE ID = @UserID";
                    SqlCommand command = new SqlCommand(updateQuery, connection);
                    command.Parameters.AddWithValue("@Rol", selectedRoleId);
                    command.Parameters.AddWithValue("@UserID", userId);
                    connection.Open();
                    command.ExecuteNonQuery();
                }

                BindGridViewKullanicilar(); 
            }
        }


        protected void btnGeriAlRol_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            string rolAdi = btn.CommandArgument;

            GeriAlRolIzinleri(rolAdi);

            BindGridViewRolSayfaAtama();
        }

        private void GeriAlRolIzinleri(string rolAdi)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["RehberConnectionString"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string deleteQuery = "DELETE FROM Rol_Sayfalar WHERE RolID = @RolID";
                SqlCommand command = new SqlCommand(deleteQuery, connection);
                command.Parameters.AddWithValue("@RolID", GetRolIDByRolAdi(rolAdi));
                connection.Open();
                command.ExecuteNonQuery();
            }
        }



    }


}
