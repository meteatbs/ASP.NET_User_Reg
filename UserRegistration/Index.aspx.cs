using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

namespace UserRegistration
{
    public partial class Index : System.Web.UI.Page
    {
        SqlConnection sqlCon = new SqlConnection(@"Data Source=DESKTOP-I43AV94;Initial Catalog=UserRegistrationDB;Integrated Security=True");
        

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Clear();
                if (!string.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    int userID = Convert.ToInt32(Request.QueryString["id"]);
                    sqlCon.Open();
                    {
                        
                        SqlDataAdapter sda = new SqlDataAdapter("UserViewByID", sqlCon);
                        sda.SelectCommand.CommandType = CommandType.StoredProcedure;
                        sda.SelectCommand.Parameters.AddWithValue("@UserID",userID);
                        DataTable dtbl = new DataTable();
                        sda.Fill(dtbl);
                        hfUserID.Value = userID.ToString();
                        txtFirstName.Text = dtbl.Rows[0][1].ToString();
                        txtLastName.Text = dtbl.Rows[0][2].ToString();
                        txtContact.Text = dtbl.Rows[0][3].ToString();
                        ddlGender.Items.FindByValue(dtbl.Rows[0][4].ToString()).Selected = true;
                        txtAddress.Text = dtbl.Rows[0][5].ToString();
                        txtUserName.Text = dtbl.Rows[0][6].ToString();
                        txtPassword.Text = dtbl.Rows[0][7].ToString();
                        txtPassword.Attributes.Add("value",dtbl.Rows[0][7].ToString());
                        txtConfirmPassword.Text = dtbl.Rows[0][7].ToString();
                        txtConfirmPassword.Attributes.Add("value", dtbl.Rows[0][7].ToString());




                    }
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {

            if (txtUserName.Text == "" || txtPassword.Text == "")
                lblErrorMessage.Text = "Please Fill Mandatory Fields";
            else if (txtPassword.Text != txtConfirmPassword.Text)
                lblErrorMessage.Text = "Passwords Doesn't Match";
            else
            {
                sqlCon.Open();
                
                {
                    SqlCommand sqlCmd = new SqlCommand("UserAddOrEdit", sqlCon);
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.Parameters.AddWithValue("@UserID", Convert.ToInt32(hfUserID.Value == "" ? "0" : hfUserID.Value)); //we have to use hidden id to pass values
                    sqlCmd.Parameters.AddWithValue("@FirstName", txtFirstName.Text.Trim());
                    sqlCmd.Parameters.AddWithValue("@LastName", txtLastName.Text.Trim());
                    sqlCmd.Parameters.AddWithValue("@Contact", txtContact.Text.Trim());
                    sqlCmd.Parameters.AddWithValue("@Gender", ddlGender.SelectedValue);
                    sqlCmd.Parameters.AddWithValue("@Address", txtAddress.Text.Trim());
                    sqlCmd.Parameters.AddWithValue("@Username", txtUserName.Text.Trim());
                    sqlCmd.Parameters.AddWithValue("@Password", txtPassword.Text.Trim());
                    sqlCmd.ExecuteNonQuery();
                    Clear();
                    lblSuccesMessage.Text = "Submitted Succesfully";

                    
                }
            }
        }

        void Clear()
        {
            txtFirstName.Text = txtLastName.Text = txtContact.Text = txtAddress.Text = txtPassword.Text = txtConfirmPassword.Text = "";
            txtFirstName.Focus();
            hfUserID.Value = "";
            lblErrorMessage.Text =lblSuccesMessage.Text = "";
        }
    }
}