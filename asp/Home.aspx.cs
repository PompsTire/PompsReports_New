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
using Microsoft.Reporting.WebForms;

public partial class asp_Home : System.Web.UI.Page
{
    string sqlConnection = ConfigurationManager.ConnectionStrings["Pomps_ReportsConnectionString"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString.Get("Username") != null)
            {
                string userID, Username,reportName;
                Username = Request.QueryString.Get("Username").ToString();
                reportName = Request.QueryString.Get("reportName").ToString();
                userID = RetrieveUserAccess(Username);
                lblYear2.Text = DateTime.Now.Year.ToString();
            
                lblUserAccess.Text = userID;
                lblReportName.Text = reportName;
                cbExcludeIntracompany.Checked = true;
                
                StartUpRenderReports(lblUserAccess.Text,lblReportName.Text);
            }
            else {
                Response.Redirect("http://reports.pompstire.com");
            }

            lblUserAccess.Visible = false;
            lblReportName.Visible = false;

        }
    }
    protected void cbExcludeIntracompany_CheckedChanged(object sender, EventArgs e)
    {
        //CalculateYTD(lblUserAccess.Text);
        StartUpRenderReports(lblUserAccess.Text,lblReportName.Text);
    }
    protected void lnkbtnCustomerBalance_Click(object sender, EventArgs e)
    {
        //Response.Redirect("~/asp/CustomerBalance.aspx?CustNumber=" + "1" + "&UserID=" + lblUserAccess.Text);
    }

    private void StartUpRenderReports(string userAccess,string reportName)
    {        
        ServerReport serverReport3 = ReportViewer4.ServerReport;
        ReportViewer4.ProcessingMode = ProcessingMode.Remote;
        ReportViewer4.ShowToolBar = false;
        ReportViewer4.ShowParameterPrompts = false;
        ReportViewer4.SizeToReportContent = true;
        ReportViewer4.Width = Unit.Percentage(100);
        ReportViewer4.Width = Unit.Percentage(100);
        serverReport3.ReportServerUrl = new Uri("http://gbsql01/reportserver");
        if(reportName == "KPI") {
            lblTitle.Text = "Key Performance Indicators";
            if (cbExcludeIntracompany.Checked == true)
                {
                    serverReport3.ReportPath = "/PompsReports/New/KPI_NO_INTR_" + userAccess;
                }
            else
                {
                    serverReport3.ReportPath = "/PompsReports/New/KPI_" + userAccess;
                }
        }
        else
        {
            lblTitle.Text = "Road Service";
            serverReport3.ReportPath = "/PompsReports/New/" + reportName + userAccess;
            cbExcludeIntracompany.Visible = false;
            lblIntracompany.Visible = false;
        }
        ReportViewer4.ServerReport.Refresh();
    }
    protected string RetrieveUserAccess(string username)
    {
        using (SqlConnection connection = new SqlConnection(sqlConnection))
        {
            string query = @"SELECT UserID FROM dbo.Database_User WHERE UserLogin = @UserLogin";

            connection.Open();

            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                try
                {
                    string userID;
                    cmd.Parameters.AddWithValue("@UserLogin", username);

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