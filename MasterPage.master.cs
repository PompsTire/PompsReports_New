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

public partial class MasterPage : System.Web.UI.MasterPage
{
    string sqlConnection = ConfigurationManager.ConnectionStrings["Pomps_ReportsConnectionString"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            lblYear.Text = DateTime.Now.Year.ToString();

            if (Request.QueryString.Get("Username") != null)
            {
                //lblUsername.Text = Page.User.Identity.Name.Substring(6);
                string username = Request.QueryString.Get("Username").ToString();
                lblUsername.Text = username;
            }
            else
            {
                string userID = Request.QueryString.Get("userID").ToString();
                lblUsername.Text = FindUsername(userID).ToString();
                //Response.Redirect("http://reports.pompstire.com");
            }

            switch (ActivateLinks(lblUsername.Text))
            {
                case ("MGR"):
                    lnkbtnAdmin.Visible = false;
                    lnkbtnFinancials.Visible = true;
                    lnkbtnHome.Visible = true;
                    lnkbtnLogout.Visible = true;
                    lnkbtnQuery.Visible = true;
                    lnkbtnReports.Visible = true;
                    lnkbtnSales.Visible = true;
                    break;
                case ("SLS"):
                    lnkbtnAdmin.Visible = false;
                    lnkbtnFinancials.Visible = false;
                    lnkbtnHome.Visible = true;
                    lnkbtnLogout.Visible = true;
                    lnkbtnQuery.Visible = true;
                    lnkbtnReports.Visible = true;
                    lnkbtnSales.Visible = true;
                    break;
                case ("ADMIN"):
                    lnkbtnAdmin.Visible = true;
                    lnkbtnFinancials.Visible = true;
                    lnkbtnHome.Visible = true;
                    lnkbtnLogout.Visible = true;
                    lnkbtnQuery.Visible = true;
                    lnkbtnReports.Visible = true;
                    lnkbtnSales.Visible = true;
                    break;
                default:
                    lnkbtnAdmin.Visible = false;
                    lnkbtnFinancials.Visible = false;
                    lnkbtnHome.Visible = true;
                    lnkbtnLogout.Visible = true;
                    lnkbtnQuery.Visible = false;
                    lnkbtnReports.Visible = true;
                    lnkbtnSales.Visible = false;
                    break;
            }
        }

    }

    protected void lnkbtnFinancials_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/asp/Reports.aspx?TableID=Financials&Username=" + lblUsername.Text);
    }

    protected void lnkbtnInventory_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/asp/Reports.aspx?TableID=Inventory&Username=" + lblUsername.Text);
    }

    protected void lnkbtnSales_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/asp/Reports.aspx?TableID=Sales&Username=" + lblUsername.Text);
    }

    protected void lnkbtnHome_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/asp/Home.aspx?Username=" + lblUsername.Text + "&reportName=KPI");
    }

    protected void lnkbtnQuery_Click(object sender, EventArgs e)
    {
        if (lblUsername.Text == "gtester" || lblUsername.Text == "cwillborn" || lblUsername.Text == "rlawrence" || lblUsername.Text == "pwochinske" || lblUsername.Text == "jbroskovetz")
        {
            Response.Redirect("~/asp/Query.aspx?Username=" + lblUsername.Text);
        }
        else
        {
            Response.Redirect("~/asp/Construction.aspx?Username=" + lblUsername.Text);
        }
        
    }

    protected void lnkbtnAdmin_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/asp/Administrator.aspx?Username=" + lblUsername.Text);
    }


    protected void lnkbtnInventory_Click1(object sender, EventArgs e)
    {
        Response.Redirect("~/asp/Construction.aspx?Username=" + lblUsername.Text);
    }
    protected void lnkbtnRoadService_Click(object sender, EventArgs e)
    {
        //Response.Redirect("~/asp/Home.aspx?&Username=" + lblUsername.Text + "&reportName=RoadService_NO_INTR");
        Response.Redirect("~/asp/Home.aspx?&Username=" + lblUsername.Text + "&reportName=RoadService_NO_INTR_");
    }
    protected void lnkbtnSalesServices_Click(object sender, EventArgs e)
    {
        //Response.Redirect("~/asp/Construction.aspx?Username=" + lblUsername.Text);
    }
    protected void lnkbtnDealerPrograms_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/asp/DealerPrograms.aspx?Username=" + lblUsername.Text);
    }
    protected void lnkbtnLogout_Click(object sender, EventArgs e)
    {
        Session.Clear();
        Session.Abandon();
        Response.Cache.SetExpires(DateTime.Now);
        Response.Cache.SetNoStore();
        Response.Redirect("http://reports.pompstire.com");
    }

    private string ActivateLinks(string username)
    {
        using (SqlConnection connection = new SqlConnection(sqlConnection))
        {
            string query = @"SELECT rtrim(UserFrontPage) as UserFrontPage FROM Pomps_Reports.dbo.Database_User WHERE UserLogin = @UserLogin";

            connection.Open();

            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                try
                {
                    string Access;
                    cmd.Parameters.AddWithValue("@UserLogin", username);

                    if (cmd.ExecuteScalar() != null)
                    {
                        Access = cmd.ExecuteScalar().ToString();
                        connection.Close();
                    }
                    else
                    {
                        Access = "";
                        connection.Close();
                    }

                    return Access;
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
        }
    }

    private string FindUsername(string userID)
    {
        using (SqlConnection connection = new SqlConnection(sqlConnection))
        {
            string query = @"SELECT rtrim(UserLogin) as UserLogin FROM Pomps_Reports.dbo.Database_User WHERE UserID = @UserID";

            connection.Open();

            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                try
                {
                    string username;
                    cmd.Parameters.AddWithValue("@UserID", userID);

                    if (cmd.ExecuteScalar() != null)
                    {
                        username = cmd.ExecuteScalar().ToString();
                        connection.Close();
                    }
                    else
                    {
                        username = "";
                        connection.Close();
                    }

                    return username;
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
        }
    }
    private string FindUserID(string Username)
    {
        using (SqlConnection connection = new SqlConnection(sqlConnection))
        {
            string query = @"SELECT UserID as UserID FROM Pomps_Reports.dbo.Database_User WHERE UserLogin = @UserLogin";

            connection.Open();

            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                try
                {
                    string userID;

                    cmd.Parameters.AddWithValue("@UserLogin", Username);

                    if (cmd.ExecuteScalar() != null)
                    {
                        userID = cmd.ExecuteScalar().ToString();
                        connection.Close();
                    }
                    else
                    {
                        userID = "";
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



}
