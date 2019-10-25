using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Data.Odbc;
using System.Configuration;
using System.Text;
using Microsoft.Reporting.WebForms;

public partial class asp_Default : System.Web.UI.Page
{
    string sqlConnection = ConfigurationManager.ConnectionStrings["Pomps_ReportsConnectionString"].ConnectionString;
    string serverReportUri = "http://gbsql01/reportserver";
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lblUsername.Text = Request.QueryString.Get("Username").ToString();
            //string userAccess = Request.QueryString.Get("userAccess").ToString();
            //lblUsername.Text = "oreifschneider";
            cbExcludeIntercompany.Checked = true;
            //lblUsername.Visible = false;
            Generate_Sales_Comparison2(lblUsername.Text);
            FrontPage(User_Access(lblUsername.Text));
            CustClass_DropDownList.Visible = false;
        }
    }

    /*protected void CustClass_DropDownList_TextChanged(object sender, EventArgs e)
    {
        if (cbExcludeIntercompany.Checked == true)
        {
            Sales_Reports_No_INTR(lblUsername.Text);
            Generate_Sales_Comparison2(lblUsername.Text);
        }
        else
        {
            Generate_Sales_Comparison();
            Sales_Reports();
        }
    }*/

    protected void cbExcludeIntercompany_CheckedChanged(object sender, EventArgs e)
    {
        if (cbExcludeIntercompany.Checked == true)
        {
            Generate_Sales_Comparison2(lblUsername.Text);
            FrontPage(User_Access(lblUsername.Text));
        }
        else
        {
            Generate_Sales_Comparison(lblUsername.Text);
            FrontPage_2(User_Access(lblUsername.Text));
        }
    }

    private void FrontPage(string userAccess)
    {
        try
        {

            ServerReport serverReport = ReportViewer1.ServerReport;
            ReportViewer1.ProcessingMode = ProcessingMode.Remote;
            serverReport.ReportServerUrl = new Uri(serverReportUri);
            serverReport.ReportPath = "/PompsReports/New/YTD_Sales_NO_INTR-" + userAccess;

            ServerReport serverReport2 = ReportViewer2.ServerReport;
            ReportViewer2.ProcessingMode = ProcessingMode.Remote;
            serverReport2.ReportServerUrl = new Uri(serverReportUri);
            serverReport2.ReportPath = "/PompsReports/New/YTD_Sales_NO_INTR_b-" + userAccess;

            ReportViewer1.ServerReport.Refresh();

            ReportViewer2.ServerReport.Refresh();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void FrontPage_2(string userAccess)
    {
        try
        {

            ServerReport serverReport = ReportViewer1.ServerReport;
            ReportViewer1.ProcessingMode = ProcessingMode.Remote;
            serverReport.ReportServerUrl = new Uri(serverReportUri);
            serverReport.ReportPath = "/PompsReports/New/YTD_Sales-" + userAccess;

            ServerReport serverReport2 = ReportViewer2.ServerReport;
            ReportViewer2.ProcessingMode = ProcessingMode.Remote;
            serverReport2.ReportServerUrl = new Uri(serverReportUri);
            serverReport2.ReportPath = "/PompsReports/New/YTD_Sales_b-" + userAccess;

            ReportViewer1.ServerReport.Refresh();

            ReportViewer2.ServerReport.Refresh();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private void Sales_Reports()
    {
        ServerReport serverReport = ReportViewer1.ServerReport;
        ReportViewer1.ProcessingMode = ProcessingMode.Remote;
        serverReport.ReportServerUrl = new Uri(serverReportUri);
        serverReport.ReportPath = "/PompsReports/New/YTD_Sales";

        ServerReport serverReport2 = ReportViewer2.ServerReport;
        ReportViewer2.ProcessingMode = ProcessingMode.Remote;
        serverReport2.ReportServerUrl = new Uri(serverReportUri);
        serverReport2.ReportPath = "/PompsReports/New/YTD_Sales_b";
        try
        {
            ReportParameter[] filters = new ReportParameter[2];
            filters[0] = new ReportParameter("UserLogin", lblUsername.Text);
            filters[1] = new ReportParameter("Cust_Cls", "All");

            ReportViewer1.ServerReport.SetParameters(filters);
            ReportViewer1.ServerReport.Refresh();

            ReportViewer2.ServerReport.SetParameters(filters);
            ReportViewer2.ServerReport.Refresh();
        }
        catch (Exception ex)
        {
            throw ex;
        } 
    }

    private void Sales_Reports_No_INTR(string username)
    {
        ServerReport serverReport = ReportViewer1.ServerReport;
        ReportViewer1.ProcessingMode = ProcessingMode.Remote;
        serverReport.ReportServerUrl = new Uri(serverReportUri);
        serverReport.ReportPath = "/PompsReports/New/YTD_Sales_NO_INTR";

        ServerReport serverReport2 = ReportViewer2.ServerReport;
        ReportViewer2.ProcessingMode = ProcessingMode.Remote;
        serverReport2.ReportServerUrl = new Uri(serverReportUri);
        serverReport2.ReportPath = "/PompsReports/New/YTD_Sales_NO_INTR_b";

        try
        {
            ReportParameter[] filters = new ReportParameter[2];
            filters[0] = new ReportParameter("UserLogin", username);
            filters[1] = new ReportParameter("Cust_Cls", "All");

            ReportViewer1.ServerReport.SetParameters(filters);
            ReportViewer1.ServerReport.Refresh();

            ReportViewer2.ServerReport.SetParameters(filters);
            ReportViewer2.ServerReport.Refresh();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void Generate_Sales_Comparison2(string username)
    {
        using (SqlConnection connection = new SqlConnection(sqlConnection))
        {
            connection.Open();

            string SalesComparison_1 = @"SELECT SUM(Invoice_Product.ProductQuantity * Invoice_Product.ProductPrice) AS Total_Sales
FROM Invoice_Product INNER JOIN ProductClass ON Invoice_Product.Product_Cls = ProductClass.ProductClassCode
INNER JOIN Invoice on InvoiceNumber = Invoice_Number
INNER JOIN SalesClass on Invoice_Customer_Cls = SalesClassCode
WHERE   Year(Sale_Date) = Year(DateAdd(day,-1,GetDate())) AND  (Invoice_Store in(select UserStoreNo from dbo.Database_User_Store as us inner join dbo.Database_User as u on us.UserID = u.UserID
where u.UserLogin = @UserLogin)) and Invoice_Salesman in(select UserSalesmanNo from dbo.Database_User_Salesman as us
inner join dbo.Database_User as u on us.UserID = u.UserID where u.UserLogin = @UserLogin)
and Invoice_Customer_Cls in(select UserSalesClassCode from dbo.Database_User_Salesclass as usc
inner join dbo.Database_User as u on usc.UserID = u.UserID where u.UserLogin  = @UserLogin)
and Invoice_Customer_Cls not in('I','7')
and Product_Cls not in('45','50','84','91','97','98','99')";

            using (SqlCommand cmd = new SqlCommand(SalesComparison_1, connection))
            {
                try { 
                cmd.Parameters.AddWithValue("@UserLogin", username);
                //cmd.Parameters.AddWithValue("@Cust_Cls",CustClass_DropDownList.SelectedItem.Text.Trim());

                    if (cmd.ExecuteScalar() != null)
                    {
                        string sales = cmd.ExecuteScalar().ToString();
                        lblTotalSales.Text = String.Format("{0:c}",Convert.ToDecimal(sales));
                        //lblTotalSales.Text = sales;
                    }
                    else
                    {
                        //lblTotalSales.Text = String.Format("{0:c}","0");
                        lblTotalSales.Text = "$0.00";
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                } 
            }

            connection.Close();
        }
    }


    private void Generate_Sales_Comparison(string Username)
    {
        using (SqlConnection connection = new SqlConnection(sqlConnection))
        {
            connection.Open();

            string SalesComparison_1 = @"SELECT SUM(Invoice_Product.ProductQuantity * Invoice_Product.ProductPrice) AS Total_Sales
FROM         Invoice_Product INNER JOIN
                      ProductClass ON Invoice_Product.Product_Cls = ProductClass.ProductClassCode
INNER JOIN Invoice on InvoiceNumber = Invoice_Number
INNER JOIN SalesClass on Invoice_Customer_Cls = SalesClassCode
WHERE   Year(Sale_Date) = Year(DateAdd(day,-1,GetDate())) AND  (Invoice_Store in(select UserStoreNo from dbo.Database_User_Store as us inner join dbo.Database_User as u on us.UserID = u.UserID
where u.UserLogin = @UserLogin)) and Invoice_Salesman in(select UserSalesmanNo from dbo.Database_User_Salesman as us
inner join dbo.Database_User as u on us.UserID = u.UserID where u.UserLogin = @UserLogin)
and Invoice_Customer_Cls in(select UserSalesClassCode from dbo.Database_User_Salesclass as usc
inner join dbo.Database_User as u on usc.UserID = u.UserID where u.UserLogin  = @UserLogin)";

            using (SqlCommand cmd = new SqlCommand(SalesComparison_1, connection))
            {
                try
                {
                    cmd.Parameters.AddWithValue("@UserLogin", Username);
                    //cmd.Parameters.AddWithValue("@Cust_Cls", CustClass_DropDownList.SelectedItem.Text.Trim());
                    if (cmd.ExecuteScalar() != null)
                    {
                        string sales = cmd.ExecuteScalar().ToString();
                        lblTotalSales.Text = String.Format("{0:c}", Convert.ToDecimal(sales));
                    }
                    else
                    {
                        //lblTotalSales.Text = String.Format("{0:c}", "0");
                        lblTotalSales.Text = "$0.00";
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            connection.Close();
        }

    }

    private string User_Access(string username)
    {
        using (SqlConnection connection = new SqlConnection(sqlConnection))
        {
            connection.Open();
            try
            {
                /*string custClass = @"select 'All' as SalesClassDescription
UNION ALL
select distinct rtrim(SalesClassDescription) as SalesClassDescription from dbo.SalesClass as pd
inner join dbo.Database_User_Salesclass as usc on pd.SalesClassCode = usc.UserSalesClassCode
inner join dbo.Database_User as u on usc.UserID = u.UserID where u.UserLogin  = @UserLogin
ORDER BY SalesClassDescription";

                SqlCommand cmd1 = new SqlCommand(custClass, connection);
                cmd1.Parameters.AddWithValue("@UserLogin",username);
                if (cmd1.ExecuteScalar() != null)
                {
                    List<string> CustClass = new List<string>();
                    using (SqlDataReader reader = cmd1.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CustClass.Add(reader["SalesClassDescription"].ToString());
                        }
                    }

                    CustClass_DropDownList.DataSource = CustClass;
                    CustClass_DropDownList.DataBind();
                }
                else
                {
                    lblMessage.Text = "Please contact your Administrator to get access to Customer Class";
                }*/

                string sqlQuery = @"select UserID from Pomps_Reports.dbo.Database_User
where UserLogin = @UserLogin";

                string userAccess;

                SqlCommand cmd1 = new SqlCommand(sqlQuery, connection);
                cmd1.Parameters.AddWithValue("@UserLogin", username);
                if (cmd1.ExecuteScalar() != null)
                {
                    userAccess = cmd1.ExecuteScalar().ToString();
                }
                else {
                    userAccess = "0";
                }

                return userAccess;
                lblMessage.Text = userAccess.ToString();
                connection.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    private void Generate_Sales()
    {

    }
}