using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public partial class Login : System.Web.UI.Page
{
    string sqlConnection = ConfigurationManager.ConnectionStrings["Pomps_ReportsConnectionString"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetExpires(DateTime.Now);
        Response.Cache.SetNoStore();

        if (!IsPostBack)
        {
            lblYear.Text = DateTime.Now.Year.ToString();
            //string username = Request.LogonUserIdentity.ToString();
            lblMessage.ForeColor = Color.Black;
            lblMessage.Text = "Please enter your username and password";
            tblLogin.Visible = true;
            tblResetPassword.Visible = false;
            tblForgotPwd.Visible = false;
            //txtUsername.Text = username;
            ResetPwd.Visible = false;
            //Label1.Text = Page.User.Identity.Name.ToString();
        }

    }
    protected void btnLogin_Click(object sender, EventArgs e)
    {
        /*if (txtUsername.Text != Page.User.Identity.Name.Substring(6).TrimEnd())
        {
            lblMessage.Text = "You must be logged in to Pomp's domain to access the SQL Reporting website";
        }
        else
        {*/
            string access;

            access = RetrieveUserAccess(txtUsername.Text, txtPassword.Text);

            if (access != "")
            {
                if (txtPassword.Text == "password" || txtPassword.Text == " " || txtPassword.Text == String.Empty)
                {
                    lblMessage.ForeColor = Color.Black;
                    lblMessage.Text = "Your current password does not comply with security parameters, please reset your password";
                    tblLogin.Visible = false;
                    tblResetPassword.Visible = true;
                    ResetPwd.Visible = true;
                    ResetPwd.Text = "Submit";
                }
                else
                {
                    ActivityLog(txtUsername.Text, "Successfully logged in to SQL Reporting website");
                    Response.Redirect("~/asp/Home.aspx?&Username=" + txtUsername.Text + "&reportName=KPI");
                }
            }
            else
            {
                lblMessage.ForeColor = Color.Red;
                lblMessage.Text = "Your Username or Password are invalid, please re-enter login information or contact your Administrator for access to this website";
            }
        //}
    }

    protected void ResetPwd_Click(object sender, EventArgs e)
    {
        switch (ResetPwd.Text)
        {
            case ("Continue"):
                Response.Redirect(Request.Url.ToString());
                break;
            case ("Submit"):
                ResetPassword(txtUsername.Text, txtNewPwd.Text);
                break;
            case ("Reset Password"):
                ValidateUser(txtPompsEmail.Text, txtEmpNo.Text);
                break;
        }
    }
    protected void btnForgotPwd_Click(object sender, EventArgs e)
    {
        lblMessage.Text = "User Authentication";
        tblLogin.Visible = false;
        tblResetPassword.Visible = false;
        tblForgotPwd.Visible = true;
        ResetPwd.Visible = true;
        ResetPwd.Text = "Reset Password";
    }
    private string RetrieveUserAccess(string username, string password)
    {
        using (SqlConnection connection = new SqlConnection(sqlConnection))
        {
            string query = @"SELECT UserID FROM Pomps_Reports.dbo.Database_User WHERE UserLogin = @UserLogin and UserPassword = @UserPassword";

            connection.Open();

            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                try
                {
                    string userID;
                    cmd.Parameters.AddWithValue("@UserLogin", username);
                    cmd.Parameters.AddWithValue("@UserPassword", password);

                    if (cmd.ExecuteScalar() != null)
                    {
                        userID = cmd.ExecuteScalar().ToString();
                        connection.Close();
                    }
                    else
                    {
                        userID = "";
                        lblMessage.ForeColor = Color.Red;
                        connection.Close();
                    }

                    return userID;
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
        }

    }

    private void ResetPassword(string username, string password)
    {
        if (txtNewPwd.Text != txtConfirmPwd.Text)
        {
            lblMessage.ForeColor = Color.Red;
            lblMessage.Text = "Your password does not match with your confirmed password.";
        }
        else if (txtNewPwd.Text == "password")
        {
            lblMessage.ForeColor = Color.Red;
            lblMessage.Text = "Your password cannot be the same as the Default password generated by the system.";
        }
        else
        {
            using (SqlConnection connection = new SqlConnection(sqlConnection))
            {
                string query = @"UPDATE Pomps_Reports.dbo.Database_User SET UserPassword = @UserPassword WHERE UserLogin = @UserLogin";

                connection.Open();

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    try
                    {
                        cmd.Parameters.AddWithValue("@UserPassword", password);
                        cmd.Parameters.AddWithValue("@UserLogin", username);

                        cmd.ExecuteNonQuery();
                        connection.Close();
                        tblResetPassword.Visible = false;
                        lblMessage.ForeColor = Color.Black;
                        lblMessage.Text = "Your password has been updated";
                        ResetPwd.Text = "Continue";

                    }
                    catch (Exception ex)
                    {
                        lblMessage.ForeColor = Color.Red;
                        lblMessage.Text = ex.ToString();
                    }

                }
            }
        }
    }

    private void ValidateUser(string email,string empNo)
    {
        using (SqlConnection connection = new SqlConnection(sqlConnection))
        {
            try {
                string myQuery = @"exec Inventory.dbo.sp_Authentication @email = @email1,@employeeId = @employeeId1";
                connection.Open();
                SqlCommand command = new SqlCommand(myQuery, connection);
                command.Parameters.AddWithValue("@email1", email);
                command.Parameters.AddWithValue("@employeeId1", empNo);
                command.CommandTimeout = 0;
                if(command.ExecuteScalar() != null)
                {
                    lblMessage.Text = "Reset your Password";
                    tblForgotPwd.Visible = false;
                    tblResetPassword.Visible = true;
                    tblLogin.Visible = false;
                    ResetPwd.Text = "Submit";
                }
                else
                {
                    lblMessage.ForeColor = Color.Red;
                    lblMessage.Text = "Your E-mail or Employee Number are invalid, please try again or contact your Administrator for access to this website";
                }
            }
            catch (Exception ex)
            {
                lblMessage.ForeColor = Color.Red;
                lblMessage.Text = ex.ToString();
            }

        }
    }
    private void ActivityLog(string username,string msg)
    {
            using (SqlConnection connection = new SqlConnection(sqlConnection))
            {
                string query = @"INSERT INTO Pomps_Reports.dbo.Transaction_Log(Transaction_Date,Transaction_Description,Transaction_Message)
VALUES(SYSDATETIME(),@userLogin,@Message)";

                connection.Open();

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    try
                    {
                        cmd.Parameters.AddWithValue("@userLogin", username);
                        cmd.Parameters.AddWithValue("@Message", msg);

                        cmd.ExecuteNonQuery();
                        connection.Close();

                    }
                    catch (Exception ex)
                    {
                        lblMessage.ForeColor = Color.Red;
                        lblMessage.Text = ex.ToString();
                    }

                }
            }
    }

}