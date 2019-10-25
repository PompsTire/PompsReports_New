using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Microsoft.Reporting.WebForms;

public partial class asp_ReportViewer : System.Web.UI.Page
{
    string sqlConnection = ConfigurationManager.ConnectionStrings["Pomps_ReportsConnectionString"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        lblYear2.Text = DateTime.Now.Year.ToString();
        lblReportName.Visible = false;
        lblUserID.Visible = false;

        if (!IsPostBack)
        {
            string currentYear = DateTime.Now.AddDays(-1).Year.ToString();
            string currentMonth, currentDay;
            if (Request.QueryString.Get("reportName") != null)
            {
                lblReportName.Text = Request.QueryString.Get("reportName").ToString();
            }
            else
            {
                lblReportName.Text = "No report name found";
            }

            if (Request.QueryString.Get("userID") != null)
            {
                lblUserID.Text = Request.QueryString.Get("userID").ToString();
                string username = FindUsername(lblUserID.Text);
            }
            else
            {
                lblUserID.Text = "No User ID found";
            }
            

            if (DateTime.Now.AddDays(-1).Month < 10)
            {
                currentMonth = "0" + DateTime.Now.AddDays(-1).Month.ToString();
            }
            else
            {
                currentMonth = DateTime.Now.AddDays(-1).Month.ToString();
            }

            if (DateTime.Now.AddDays(-1).Day < 10)
            {
                currentDay = "0" + DateTime.Now.AddDays(-1).Day.ToString();
            }
            else
            {
                currentDay = DateTime.Now.AddDays(-1).Day.ToString();
            }

            //UserAccess(userID);
            RenderReport(lblUserID.Text,lblReportName.Text, currentYear, currentMonth, currentDay);
        }
    }

    private void RenderReport(string UserID,string ReportName, string currentYear,string currentMonth,string currentDay)
    {
        try
        {
            var listOfRegions = new List<string>();
            listOfRegions = RetrieveUserRegionAccess().Split(',').ToList();
            var listOfStores = new List<string>();
            listOfStores = RetrieveUserStoreAccess().Split(',').ToList();
            var listOfSalesman = RetrieveUserSalesmanAccess().Split(',');
            //int regionCount = RetrieveUserRegionAccess(UserID).Count();
            string[] region = listOfRegions.ToArray();
            string[] store = listOfStores.ToArray();
            string[] salesman = listOfSalesman.ToArray();

            ServerReport serverReport = ReportViewer1.ServerReport;
            ReportViewer1.ProcessingMode = ProcessingMode.Remote;
            ReportViewer1.ShowToolBar = false;
            ReportViewer1.ShowParameterPrompts = false;
            ReportViewer1.SizeToReportContent = true;
            ReportViewer1.Width = Unit.Percentage(100);
            ReportViewer1.Width = Unit.Percentage(100);
            serverReport.ReportServerUrl = new Uri("http://gbsql01/reportserver");

            switch (ReportName)
            {
                case ("All_YTD_Sales"):
                    lblTitle.Text = "YTD Item Class Sales";
                    serverReport.ReportPath = "/PompsReports/New/Sales/YTD Item Class Sales";
                    ReportParameter[] filters = new ReportParameter[1];
                    filters[0] = new ReportParameter("UserID", lblUserID.Text);
                    /*filters[0] = new ReportParameter("ProductClassCategory", lblProductClassCategory.Text);
                    filters[1] = new ReportParameter("RegionNo");
                    filters[1].Values.AddRange(region);
                    filters[2] = new ReportParameter("StoreNo");
                    filters[2].Values.AddRange(store);
                    filters[3] = new ReportParameter("enddate", currentYear + currentMonth + currentDay);
                    filters[4] = new ReportParameter("clscst", "X");
                    filters[5] = new ReportParameter("tihlslmsel");
                    filters[5].Values.AddRange(salesman);*/
                    ReportViewer1.ServerReport.SetParameters(filters);
                    ReportViewer1.ServerReport.Refresh();
                    string username = FindUsername(lblUserID.Text);
                    ActivityLog(username, "YTD Item Class Sales");
                    break;
                case ("All_YTD_Sales_NO_INTR"):
                    lblTitle.Text = "YTD Item Class Sales";
                    serverReport.ReportPath = "/PompsReports/New/Sales/YTD Item Class Sales";
                    ReportParameter[] filters1 = new ReportParameter[1];
                    filters1[0] = new ReportParameter("UserID", lblUserID.Text);
                    /*filters1[0] = new ReportParameter("ProductClassCategory", lblProductClassCategory.Text);
                    filters1[1] = new ReportParameter("RegionNo");
                    filters1[1].Values.AddRange(region);
                    filters1[2] = new ReportParameter("StoreNo");
                    filters1[2].Values.AddRange(store);
                    filters1[3] = new ReportParameter("enddate", currentYear + currentMonth + currentDay);
                    filters1[4] = new ReportParameter("clscst", "I");
                    filters1[5] = new ReportParameter("tihlslmsel");
                    filters1[5].Values.AddRange(salesman);*/
                    ReportViewer1.ServerReport.SetParameters(filters1);
                    ReportViewer1.ServerReport.Refresh();
                    string username1 = FindUsername(lblUserID.Text);
                    ActivityLog(username1, "All_YTD_Sales_NO_INTR");
                    break;
                case ("YTD_Sales_NO_INTR"):
                    lblTitle.Text = "Sales Analysis";
                    serverReport.ReportPath = "/PompsReports/New/YTD_Sales_NO_INTR_" + lblUserID.Text;
                    ReportViewer1.ServerReport.Refresh();
                    string username2 = FindUsername(lblUserID.Text);
                    ActivityLog(username2, "YTD_Sales_NO_INTR");
                    break;
                case ("YTD_GP_NO_INTR"):
                    lblTitle.Text = "Gross Profit Analysis";
                    serverReport.ReportPath = "/PompsReports/New/YTD_GP_NO_INTR_" + lblUserID.Text;
                    ReportViewer1.ServerReport.Refresh();
                    string username3 = FindUsername(lblUserID.Text);
                    ActivityLog(username3, "YTD_GP_NO_INTR");
                    break;
                case ("YTD_Sales"):
                    lblTitle.Text = "Sales Analysis";
                    serverReport.ReportPath = "/PompsReports/New/YTD_Sales_" + lblUserID.Text;
                    ReportViewer1.ServerReport.Refresh();
                    string username4 = FindUsername(lblUserID.Text);
                    ActivityLog(username4, "YTD_Sales");
                    break;
                case ("TireSales_NO_INTR"):
                    lblTitle.Text = "Sales Analysis";
                    serverReport.ReportPath = "/PompsReports/New/TireSales_NO_INTR_" + lblUserID.Text;
                    ReportViewer1.ServerReport.Refresh();
                    string username5 = FindUsername(lblUserID.Text);
                    ActivityLog(username5, "TireSales_NO_INTR");
                    break;
                case ("TireGP_NO_INTR"):
                    lblTitle.Text = "Sales Analysis";
                    serverReport.ReportPath = "/PompsReports/New/TireGP_NO_INTR_" + lblUserID.Text;
                    ReportViewer1.ServerReport.Refresh();
                    string username6 = FindUsername(lblUserID.Text);
                    ActivityLog(username6, "TireGP_NO_INTR");
                    break;
                case ("YTD_GP"):
                    lblTitle.Text = "Gross Profit Analysis";
                    serverReport.ReportPath = "/PompsReports/New/YTD_GP_" + lblUserID.Text;
                    ReportViewer1.ServerReport.Refresh();
                    string username7 = FindUsername(lblUserID.Text);
                    ActivityLog(username7, "YTD_GP");
                    break;
                case ("InventoryAnalysis_NO_INTR"):
                    lblTitle.Text = "Inventory Analysis";
                    serverReport.ReportPath = "/PompsReports/New/InventoryAnalysis_NO_INTR_" + lblUserID.Text;
                    ReportViewer1.ServerReport.Refresh();
                    string username8 = FindUsername(lblUserID.Text);
                    ActivityLog(username8, "InventoryAnalysis_NO_INTR");
                    break;
                case ("InventoryAnalysis"):
                    lblTitle.Text = "Inventory Analysis";
                    serverReport.ReportPath = "/PompsReports/New/InventoryAnalysis_" + lblUserID.Text;
                    ReportViewer1.ServerReport.Refresh();
                    string username9 = FindUsername(lblUserID.Text);
                    ActivityLog(username9, "InventoryAnalysis");
                    break;
                case ("InventoryAgingReport"):
                    lblTitle.Text = "Inventory Analysis";
                    string Age = Request.QueryString.Get("Age").ToString();
                    serverReport.ReportPath = "/PompsReports/New/Inventory/InventoryAgingReport";
                    ReportViewer1.ShowToolBar = true;
                    ReportViewer1.ShowPrintButton = false;
                    ReportParameter[] filters2 = new ReportParameter[2];
                    filters2[0] = new ReportParameter("UserID", lblUserID.Text);
                    filters2[1] = new ReportParameter("Age", Age);
                    ReportViewer1.ServerReport.SetParameters(filters2);
                    ReportViewer1.ServerReport.Refresh();
                    string username10 = FindUsername(lblUserID.Text);
                    ActivityLog(username10, "InventoryAgingReport");
                    break;
                case ("RoadServiceTechnician"):
                    lblTitle.Text = "Road Service Analysis";
                    serverReport.ReportPath = "/PompsReports/New/Road Service/Road Service Technicians Report";
                    ReportViewer1.ShowToolBar = true;
                    ReportViewer1.ShowPrintButton = false;
                    ReportParameter[] filters3 = new ReportParameter[1];
                    filters3[0] = new ReportParameter("UserID", lblUserID.Text);
                    ReportViewer1.ServerReport.SetParameters(filters3);
                    ReportViewer1.ServerReport.Refresh();
                    string username11 = FindUsername(lblUserID.Text);
                    ActivityLog(username11, "RoadServiceTechnician");
                    break;
                case ("RoadServiceCustomer"):
                    lblTitle.Text = "Road Service Analysis";
                    serverReport.ReportPath = "/PompsReports/New/Road Service/Road Service Customers Report";
                    ReportViewer1.ShowToolBar = true;
                    ReportViewer1.ShowPrintButton = false;
                    ReportParameter[] filters4 = new ReportParameter[1];
                    filters4[0] = new ReportParameter("UserID", lblUserID.Text);
                    ReportViewer1.ServerReport.SetParameters(filters4);
                    ReportViewer1.ServerReport.Refresh();
                    string username12 = FindUsername(lblUserID.Text);
                    ActivityLog(username12, "RoadServiceTechnician");
                    break;
                case ("YTDItemClassSales"):
                    lblTitle.Text = "Sales Analysis";
                    serverReport.ReportPath = "/PompsReports/New/Sales/YTD Item Class Sales";
                    ReportViewer1.ShowToolBar = true;
                    ReportViewer1.ShowPrintButton = false;
                    ReportParameter[] filters5 = new ReportParameter[1];
                    filters5[0] = new ReportParameter("UserID", lblUserID.Text);
                    ReportViewer1.ServerReport.SetParameters(filters5);
                    ReportViewer1.ServerReport.Refresh();
                    string username13 = FindUsername(lblUserID.Text);
                    ActivityLog(username13, "YTDItemClassSales");
                    break;
                case ("YTDVehicleRoadService"):
                    lblTitle.Text = "Sales Analysis";
                    serverReport.ReportPath = "/PompsReports/New/Road Service/YTD Road Service Calls by Vehicle";
                    ReportViewer1.ShowToolBar = true;
                    ReportViewer1.ShowPrintButton = false;
                    ReportParameter[] filters6 = new ReportParameter[1];
                    filters6[0] = new ReportParameter("UserID", lblUserID.Text);
                    ReportViewer1.ServerReport.SetParameters(filters6);
                    ReportViewer1.ServerReport.Refresh();
                    string username14 = FindUsername(lblUserID.Text);
                    ActivityLog(username14, "YTDVehicleRoadService");
                    break;
                case ("YTDRSCalls"):
                    string customerAccount, carMake, carModel, technician, period, itemNumber;
                    if (Request.QueryString.Get("customer_account") != null)
                    {
                        customerAccount = Request.QueryString.Get("customer_account").ToString();
                    }
                    else
                    {
                        customerAccount = "0";
                    }
                    if (Request.QueryString.Get("car_make") != null)
                    {
                        carMake = Request.QueryString.Get("car_make").ToString();
                    }
                    else
                    {
                        carMake = "All";
                    }
                    if (Request.QueryString.Get("car_model") != null)
                    {
                        carModel = Request.QueryString.Get("car_model").ToString();
                    }
                    else
                    {
                        carModel = "All";
                    }
                    if (Request.QueryString.Get("technician") != null)
                    {
                        technician = Request.QueryString.Get("technician").ToString();
                    }
                    else
                    {
                        technician = "0";
                    }
                    if (Request.QueryString.Get("period") != null)
                    {
                        period = Request.QueryString.Get("period").ToString();
                    }
                    else
                    {
                        period = "0";
                    }
                    if (Request.QueryString.Get("item_number") != null)
                    {
                        itemNumber = Request.QueryString.Get("item_number").ToString();
                    }
                    else
                    {
                        itemNumber = "All";
                    }

                    lblTitle.Text = "Road Service Analysis";
                    serverReport.ReportPath = "/PompsReports/New/Road Service/YTD Road Service Calls";
                    ReportViewer1.ShowToolBar = true;
                    ReportViewer1.ShowPrintButton = false;
                    ReportParameter[] filters7 = new ReportParameter[7];
                    filters7[0] = new ReportParameter("item_number", itemNumber);
                    filters7[1] = new ReportParameter("UserID", lblUserID.Text);
                    filters7[2] = new ReportParameter("customer_account", customerAccount);
                    filters7[3] = new ReportParameter("car_make", carMake);
                    filters7[4] = new ReportParameter("car_model", carModel);
                    filters7[5] = new ReportParameter("technician", technician);
                    filters7[6] = new ReportParameter("period", period);
                    ReportViewer1.ServerReport.SetParameters(filters7);
                    ReportViewer1.ServerReport.Refresh();
                    string username15 = FindUsername(lblUserID.Text);
                    ActivityLog(username15, "Road Service Analysis");
                    break;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    /*private void UserAccess(string UserID)
    {
        if (Request.QueryString.Get("ProductClassCategory") != null)
        {
            lblProductClassCategory.Text = Request.QueryString.Get("ProductClassCategory");
        }
        else
        {
            lblProductClassCategory.Text = Request.QueryString.Get("All");
        }

        //lblRegion.Text = RetrieveUserRegionAccess(UserID);
        //lblSalesman.Text = RetrieveUserSalesmanAccess(UserID);
        //lblStore.Text = RetrieveUserStoreAccess(UserID);
        //lblProductClassCategory.Visible = false;
    }*/

    private string RetrieveUserRegionAccess()
    {
        using (SqlConnection connection = new SqlConnection(sqlConnection))
        {
                string regionNo;
            string query = @"SELECT RegionNo FROM dbo.Database_User_Region WHERE UserID = @UserID";

            connection.Open();

            SqlCommand cmd2 = new SqlCommand("SELECT rtrim(UserRegionAccess) as UserRegionAccess FROM dbo.Database_User WHERE UserID = @UserID",connection);
            try {
                cmd2.Parameters.AddWithValue("@UserID", lblUserID.Text);

                if (cmd2.ExecuteScalar().ToString() == "All Regions")
                {
                    regionNo = "99";
                }
                else
                {
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@UserID", lblUserID.Text);

                        if (cmd.ExecuteScalar() != null)
                        {
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                var myList = new List<string>();
                                while (reader.Read())
                                {
                                    myList.Add(reader["RegionNo"].ToString());
                                }
                                var rows = myList.ToArray();
                                regionNo = (string.Join(",", rows));
                                connection.Close();
                            }
                        }
                        else
                        {
                            regionNo = "";
                            connection.Close();
                        }
                    }
                }

                return regionNo;
            }
            catch (Exception ex)
                {
                    throw ex;
                }
        }

    }

    private string RetrieveUserStoreAccess()
    {
        using (SqlConnection connection = new SqlConnection(sqlConnection))
        {
            string storeNo;
            string query = @"SELECT StoreNo FROM dbo.Database_User_Store WHERE UserID = @UserID";

            connection.Open();

            SqlCommand cmd2 = new SqlCommand("SELECT rtrim(UserStoreAccess) as UserStoreAccess FROM dbo.Database_User WHERE UserID = @UserID",connection);
            try
            {
                cmd2.Parameters.AddWithValue("@UserID", lblUserID.Text);

                if (cmd2.ExecuteScalar().ToString() == "All Stores")
                {
                    storeNo = "0";
                }
                else
                {
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@UserID", lblUserID.Text);

                        if (cmd.ExecuteScalar() != null)
                        {
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                var myList = new List<string>();
                                while (reader.Read())
                                {
                                    myList.Add(reader["storeNo"].ToString());
                                }
                                var rows = myList.ToArray();
                                storeNo = (string.Join(",", rows));
                                connection.Close();
                            }
                        }
                        else
                        {
                            storeNo = "";
                            connection.Close();
                        }
                    }
                }
                    return storeNo;
            }
            catch (Exception ex)
            {
                    throw ex;
             }
        }
    }

    private string RetrieveUserSalesmanAccess()
    {
        using (SqlConnection connection = new SqlConnection(sqlConnection))
        {
            string SalesmanNo;
            string query = @"SELECT SalesmanNo FROM dbo.Database_User_Salesman WHERE UserID = @UserID";

            connection.Open();

            SqlCommand cmd2 = new SqlCommand("SELECT rtrim(UserSalesmanAccess) as UserSalesmanAccess FROM dbo.Database_User WHERE UserID = @UserID", connection);
            try
            {
                cmd2.Parameters.AddWithValue("@UserID", lblUserID.Text);

                if (cmd2.ExecuteScalar().ToString() == "All Salesman Acct")
                {
                    SalesmanNo = "0";
                }
                else
                {
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@UserID", lblUserID.Text);

                        if (cmd.ExecuteScalar() != null)
                        {
                            using (SqlDataReader reader = cmd.ExecuteReader())
                             {
                                    var myList = new List<string>();
                                    while (reader.Read())
                                {
                                    myList.Add(reader["SalesmanNo"].ToString());
                                }
                                var rows = myList.ToArray();
                                SalesmanNo = (string.Join(",", rows));
                                connection.Close();
                            }
                        }
                        else
                        {
                            SalesmanNo = "";
                            connection.Close();
                        }
                    }
                }
                    return SalesmanNo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
    /*private string FindUserID(string username)
    {
        using (SqlConnection connection = new SqlConnection(sqlConnection))
        {
            string query = @"select UserID as UserID FROM Pomps_Reports.dbo.Database_User WHERE rtrim(UserLogin) = @UserLogin";

            connection.Open();

            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                try
                {
                    string userID;
                    cmd.Parameters.AddWithValue("@UserLogin", lblUsername.Text);

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
    }*/
    private string FindUsername(string UserID)
    {
        using (SqlConnection connection = new SqlConnection(sqlConnection))
        {
            string query = @"select UserLogin as Username FROM Pomps_Reports.dbo.Database_User WHERE UserID = @UserID";

            connection.Open();

            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                try
                {
                    string username;
                    cmd.Parameters.AddWithValue("@UserID", UserID);

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

    protected void ReportViewer1_ReportError(object sender, ReportErrorEventArgs e)
    {
        throw e.Exception;
    }
    private void ActivityLog(string username, string msg)
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
                    throw ex;
                }

            }
        }
    }
}