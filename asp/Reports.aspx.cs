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

public partial class asp_Reports : System.Web.UI.Page
{
    string sqlConnection = ConfigurationManager.ConnectionStrings["Pomps_ReportsConnectionString"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if(Request.QueryString.Get("Username") != null)
            { 
            string username = Request.QueryString.Get("Username").ToString();
                lblUsername.Text = username;
                lblUsername.Visible = false;
                lblUserID.Text = RetrieveUserAccess(username).ToString();
                lblUserID.Visible = false;
                if (lblUserID.Text == "0")
                {
                    lblTable.Text = "You must be logged in to the Pomp's domain to access the reports page";
                    tblFinancials.Visible = false;
                    tblInventory.Visible = false;
                    tblSales.Visible = false;
                    btnPreview.Visible = false;
                }
                else {
            lblTable.Text = Request.QueryString.Get("TableID").ToString();
                    ActivateTable(lblTable.Text);
                    btnPreview.Text = "Preview";
                }
        }
        else {
                Response.Redirect("http://reports.pompstire.com");
            }
        }
    }
    
    protected void ddFinancials_TextChanged(object sender, EventArgs e)
    {
        switch (ddFinancials.SelectedItem.Text)
        {
            case ("Profit & Loss"):
                ddFinancialPeriod.Visible = true;
                ddRegion.Visible = true;
                ddStore.Visible = true;
                ddReportType.Visible = true;
                break;
            case ("Profit & Loss (Excludes Dept. 110)"):
                ddFinancialPeriod.Visible = true;
                ddRegion.Visible = true;
                ddStore.Visible = true;
                ddReportType.Visible = true;
                break;
            case ("Historical Financials"):
                ddFinancialPeriod.Visible = true;
                ddRegion.Visible = true;
                ddStore.Visible = true;
                ddReportType.Visible = false;
                break;
            case ("GL Audit"):
                ddFinancialPeriod.Visible = true;
                ddRegion.Visible = true;
                ddStore.Visible = true;
                ddReportType.Visible = false;
                break;
            case ("Balance Sheet"):
                ddFinancialPeriod.Visible = true;
                ddRegion.Visible = true;
                ddStore.Visible = true;
                ddReportType.Visible = false;
                break;
            case ("YTD Recap Earnings Statement"):
                ddFinancialPeriod.Visible = true;
                ddRegion.Visible = false;
                ddStore.Visible = false;
                ddReportType.Visible = false;
                break;
            case ("MTD Recap Earnings Statement"):
                ddFinancialPeriod.Visible = true;
                ddRegion.Visible = false;
                ddStore.Visible = false;
                ddReportType.Visible = false;
                break;
            case ("Recap Shops Profit & Loss"):
                ddFinancialPeriod.Visible = true;
                ddRegion.Visible = true;
                ddStore.Visible = false;
                ddReportType.Visible = false;
                break;
            case ("Recap Shops Balance Sheet"):
                ddFinancialPeriod.Visible = true;
                ddRegion.Visible = true;
                ddStore.Visible = false;
                ddReportType.Visible = false;
                break;
        }
    }
    protected void btnPreview_Click(object sender, EventArgs e)
    {
        if (btnPreview.Text == "Reset")
        {
            Response.Redirect(Request.Url.ToString());
        }
        else
        {
            string reportPath = "/MaddenReports";
            string reportName;
            if (tblFinancials.Visible == true && tblInventory.Visible == false && tblSales.Visible == false)
            {
                reportPath = reportPath + "/General Ledger/";
                reportName = ddFinancials.SelectedValue;
                btnPreview.Visible = true;

            }
            else if (tblFinancials.Visible == false && tblInventory.Visible == true && tblSales.Visible == false)
            {
                reportPath = "";
                reportName = "";
                btnPreview.Visible = false;
            }
            else
            {
                switch (ddSalesReport.SelectedItem.Text)
                {
                    case ("Labor Pricing Vs Price1"):
                    reportPath = "/Paul W";
                    reportName = "/YTD Labor Pricing Data";
                    btnPreview.Visible = true;
                        break;
                    /*case ("Madden Labor Pricing Vs Price1"):
                        reportPath = "/Report Project";
                        reportName = "/Madden YTD Labor Pricing Data";
                        btnPreview.Visible = true;
                        break;*/
                    default:
                reportPath = "";
                reportName = "";
                btnPreview.Visible = false;
                        break;
                }
            }
            RenderReports(reportPath, reportName);
            ActivityLog(lblUsername.Text, "Report: " + reportName);
            btnPreview.Text = "Reset";
            tblFinancials.Visible = false;
            tblInventory.Visible = false;
            tblSales.Visible = false;
        }
    }
    protected void ddSalesReport_SelectedIndexChanged(object sender, EventArgs e)
    {
        SalesReportsFilters();
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
                        userID = "0";
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
    
    private void SalesReportsFilters()
    {
        using (SqlConnection c = new SqlConnection(sqlConnection))
        {
            c.Open();
            switch (ddSalesReport.SelectedItem.Text)
            {
                case ("Labor Pricing Vs Price1"):
                    string Query1 = @"SELECT DISTINCT LEFT(tihhyrprin,4) as tihhyrprin FROM SQLMaddenco.dbo.LaborVsPrice1 ORDER BY LEFT(tihhyrprin,4) DESC";
                    using (SqlDataAdapter y = new SqlDataAdapter(Query1, c))
                    {
                        // fill a data table
                        var yr = new DataTable();
                        y.Fill(yr);

                        // Bind the table to the list box
                        ddSalesYear.DataTextField = "tihhyrprin";
                        ddSalesYear.DataValueField = "tihhyrprin";
                        ddSalesYear.DataSource = yr;
                        ddSalesYear.DataBind();
                    }
                    using (SqlDataAdapter a = new SqlDataAdapter("SELECT DISTINCT tihhyrprin FROM SQLMaddenco.dbo.LaborVsPrice1 ORDER BY tihhyrprin DESC", c))
                    {
                        // fill a data table
                        var t = new DataTable();
                        a.Fill(t);

                        // Bind the table to the list box
                        lbSalesPeriod.DataTextField = "tihhyrprin";
                        lbSalesPeriod.DataValueField = "tihhyrprin";
                        lbSalesPeriod.DataSource = t;
                        lbSalesPeriod.DataBind();
                    }
            using (SqlDataAdapter e = new SqlDataAdapter("SELECT SUM(tihlqty) as UNITS,tihlclsprd AS ProductClassCode,Convert(Varchar(2),tihlclsprd) + ' ' + RTrim(ProductClassDescription) AS ProductClassDescription FROM SQLMaddenco.dbo.LaborVsPrice1 INNER JOIN Pomps_Reports.dbo.ProductClass ON tihlclsprd = ProductClassCode GROUP BY tihlclsprd,ProductClassDescription ORDER BY tihlclsprd", c))
            {
                // fill a data table
                var prdcls = new DataTable();
                e.Fill(prdcls);

                // Bind the table to the list box
                lbPrdCls.DataTextField = "ProductClassDescription";
                lbPrdCls.DataValueField = "ProductClassCode";
                lbPrdCls.DataSource = prdcls;
                lbPrdCls.DataBind();
            }
            using (SqlDataAdapter f = new SqlDataAdapter("SELECT SUM(tihlqty) AS Units,tihlvndprd AS ProductVendorCode,Convert(Varchar(3),tihlvndprd) + ' ' + RTrim(ProductVendorDescription) AS ProductVendorDescription FROM SQLMaddenco.dbo.LaborVsPrice1 INNER JOIN Pomps_Reports.dbo.ProductVendor ON tihlvndprd = ProductVendorCode GROUP BY tihlvndprd,ProductVendorDescription ORDER BY tihlvndprd", c))
            {
                // fill a data table
                var prdvnd = new DataTable();
                f.Fill(prdvnd);

                // Bind the table to the list box
                lbPrdVnd.DataTextField = "ProductVendorDescription";
                lbPrdVnd.DataValueField = "ProductVendorCode";
                lbPrdVnd.DataSource = prdvnd;
                lbPrdVnd.DataBind();
                    }

                    string salesClass = @"SELECT DISTINCT tihhclscst as SalesClassCode,tihhclscst + ' - ' + SalesClassDescription as SalesClassDescription
FROM SQLMaddenco.dbo.LaborVsPrice1 INNER JOIN Pomps_Reports.dbo.SalesClass
ON tihhclscst = SalesClassCode";
                    using (SqlDataAdapter g = new SqlDataAdapter(salesClass, c))
                    {
                        try
                        {
                            // fill a data table
                            var cstcls = new DataTable();
                            //g.SelectCommand.Parameters.AddWithValue("@UserID", lblUserID.Text);
                            g.Fill(cstcls);

                            // Bind the table to the list box
                            lbCustCls.DataTextField = "SalesClassDescription";
                            lbCustCls.DataValueField = "SalesClassCode";
                            lbCustCls.DataSource = cstcls;
                            lbCustCls.DataBind();
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }

                    break;
            }

            string salesman = @"SELECT uslm.SalesmanNo as tihhslmsel,Convert(Varchar(5),uslm.SalesmanNo) + ' ' + RTrim(slm.SalesmanName) AS SalesmanName,RTrim(slm.SalesmanName) as Name
FROM Pomps_Reports.dbo.Database_User_Salesman uslm INNER JOIN Pomps_Reports.dbo.Salesman slm ON uslm.SalesmanID = slm.SalesmanID WHERE uslm.SalesmanNo NOT LIKE('%01') AND UserID = @UserID ORDER BY Name";
            using (SqlDataAdapter h = new SqlDataAdapter(salesman, c))
            {
                try {
                // fill a data table
                var cstcls = new DataTable();
                    h.SelectCommand.Parameters.AddWithValue("@UserID", lblUserID.Text);
                h.Fill(cstcls);

                // Bind the table to the list box
                lbSalesman.DataTextField = "SalesmanName";
                lbSalesman.DataValueField = "tihhslmsel";
                lbSalesman.DataSource = cstcls;
                lbSalesman.DataBind();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            string region = @"SELECT ur.RegionNo as RegionNo,case when ur.RegionNo = 99 then RTrim(RegionName) else Convert(Varchar(2),ur.RegionNo) + ' ' + RTrim(RegionName) end AS RegionName
FROM Pomps_Reports.dbo.Database_User_Region ur INNER JOIN Pomps_Reports.dbo.Region r ON ur.RegionID = r.RegionID WHERE UserID = @UserID ORDER BY case when ur.RegionNo = 99 then -1 else ur.RegionNo end";
            using (SqlDataAdapter b = new SqlDataAdapter(region, c))
            {
                try {
                // fill a data table
                var r = new DataTable();
                b.SelectCommand.Parameters.AddWithValue("@UserID", lblUserID.Text);
                b.Fill(r);

                // Bind the table to the list box
                lbSalesRegion.DataTextField = "RegionName";
                lbSalesRegion.DataValueField = "RegionNo";
                lbSalesRegion.DataSource = r;
                lbSalesRegion.DataBind();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            string store = @"SELECT us.StoreNo,case when us.StoreNo = 0 then RTrim(StoreName) else Convert(Varchar(3),us.StoreNo) + ' ' + RTrim(StoreName) end AS StoreName FROM Pomps_Reports.dbo.Database_User_Store us
INNER JOIN Pomps_Reports.dbo.Store s ON us.StoreID = s.StoreID WHERE us.StoreNo < 600 AND UserID = @UserID ORDER BY us.StoreNo";

            using (SqlDataAdapter d = new SqlDataAdapter(store, c))
            {
                try {
                // fill a data table
                var s = new DataTable();
                d.SelectCommand.Parameters.AddWithValue("@UserID", lblUserID.Text);
                d.Fill(s);

                // Bind the table to the list box
                lbSalesStore.DataTextField = "StoreName";
                lbSalesStore.DataValueField = "StoreNo";
                lbSalesStore.DataSource = s;
                lbSalesStore.DataBind(); }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            // use a SqlAdapter to execute the query
            c.Close();
        }
    }
    private void RenderReports(string reportPath, string reportName)
    {
        try
        {
            ServerReport serverReport = ReportViewer1.ServerReport;
            ReportViewer1.ProcessingMode = ProcessingMode.Remote;
            serverReport.ReportServerUrl = new Uri("http://gbsql01/reportserver");
            serverReport.ReportPath = reportPath + reportName;

            string[] salesPeriod = ReportPeriod(lbSalesPeriod).Split(',');
            string[] salesRegion = ReportRegion(lbSalesRegion).Split(',');
            string[] salesStore = ReportStore(lbSalesStore).Split(',');
            string[] salesPrdCls = ReportPrdCls(lbPrdCls).Split(',');
            string[] salesPrdVnd = ReportPrdVnd(lbPrdVnd).Split(',');
            string[] salesSalesman = ReportSalesman(lbSalesman).Split(',');
            string[] salesCustClass = ReportCustClass(lbCustCls).Split(',');

            switch (reportName)
            {
                case ("PandLALL"):
                    ReportParameter[] filters = new ReportParameter[13];
                    filters[0] = new ReportParameter("Parameter1", ddFinancialPeriod.SelectedValue.Substring(4, 2));
                    filters[1] = new ReportParameter("Parameter2", ddFinancialPeriod.SelectedValue.Substring(4, 2));
                    filters[2] = new ReportParameter("Parameter3", ddFinancialPeriod.SelectedValue.Substring(0, 4));
                    filters[3] = new ReportParameter("Parameter4", ddFinancialPeriod.SelectedValue.Substring(0, 4));
                    filters[4] = new ReportParameter("Parameter5", ddFinancialPeriod.SelectedValue.Substring(4, 2));
                    filters[5] = new ReportParameter("Parameter6", "99");
                    filters[6] = new ReportParameter("Parameter7", ddRegion.SelectedValue);
                    filters[7] = new ReportParameter("Parameter8", ddRegion.SelectedValue);
                    filters[8] = new ReportParameter("Parameter9", ddStore.SelectedValue);
                    filters[9] = new ReportParameter("Parameter10", ddStore.SelectedValue);
                    filters[10] = new ReportParameter("Parameter11", "AA");
                    filters[11] = new ReportParameter("Parameter12", "AA");
                    if (ddStore.SelectedValue.ToString() != "0")
                    {
                        filters[12] = new ReportParameter("ReportParameter1","CONSOLIDATED");
                    }
                    else
                    {
                        filters[12] = new ReportParameter("ReportParameter1", ddReportType.SelectedItem.Text);
                    }

                    ReportViewer1.ServerReport.SetParameters(filters);
                    break;
                case ("GLAuditReport"):
                    ReportParameter[] filters1 = new ReportParameter[42];
                    filters1[0] = new ReportParameter("Parameter1", ddFinancialPeriod.SelectedValue.Substring(4, 2));
                    filters1[1] = new ReportParameter("Parameter2", ddFinancialPeriod.SelectedValue.Substring(4, 2));
                    filters1[2] = new ReportParameter("Parameter3", ddFinancialPeriod.SelectedValue.Substring(4, 2));
                    filters1[3] = new ReportParameter("Parameter4", ddFinancialPeriod.SelectedValue.Substring(4, 2));
                    filters1[4] = new ReportParameter("Parameter5", ddFinancialPeriod.SelectedValue.Substring(0, 4));
                    filters1[5] = new ReportParameter("Parameter6", ddFinancialPeriod.SelectedValue.Substring(4, 2));
                    filters1[6] = new ReportParameter("Parameter7", ddStore.SelectedValue);
                    filters1[7] = new ReportParameter("Parameter8", ddStore.SelectedValue);
                    filters1[8] = new ReportParameter("Parameter9", ddRegion.SelectedValue);
                    filters1[9] = new ReportParameter("Parameter10", ddRegion.SelectedValue);
                    filters1[10] = new ReportParameter("Parameter11", "A");
                    filters1[11] = new ReportParameter("Parameter12", "A");
                    filters1[12] = new ReportParameter("Parameter13", "All");
                    filters1[13] = new ReportParameter("Parameter14", "All");
                    filters1[14] = new ReportParameter("Parameter15", ddFinancialPeriod.SelectedValue.Substring(4, 2));
                    filters1[15] = new ReportParameter("Parameter16", ddFinancialPeriod.SelectedValue.Substring(4, 2));
                    filters1[16] = new ReportParameter("Parameter17", ddFinancialPeriod.SelectedValue.Substring(4, 2));
                    filters1[17] = new ReportParameter("Parameter18", ddFinancialPeriod.SelectedValue.Substring(4, 2));
                    filters1[18] = new ReportParameter("Parameter19", ddFinancialPeriod.SelectedValue.Substring(0, 4));
                    filters1[19] = new ReportParameter("Parameter20", ddFinancialPeriod.SelectedValue.Substring(4, 2));
                    filters1[20] = new ReportParameter("Parameter21", ddStore.SelectedValue);
                    filters1[21] = new ReportParameter("Parameter22", ddStore.SelectedValue);
                    filters1[22] = new ReportParameter("Parameter23", ddRegion.SelectedValue);
                    filters1[23] = new ReportParameter("Parameter24", ddRegion.SelectedValue);
                    filters1[24] = new ReportParameter("Parameter25", "A");
                    filters1[25] = new ReportParameter("Parameter26", "A");
                    filters1[26] = new ReportParameter("Parameter27", "All");
                    filters1[27] = new ReportParameter("Parameter28", "All");
                    filters1[28] = new ReportParameter("Parameter29", ddFinancialPeriod.SelectedValue.Substring(4, 2));
                    filters1[29] = new ReportParameter("Parameter30", ddFinancialPeriod.SelectedValue.Substring(4, 2));
                    filters1[30] = new ReportParameter("Parameter31", ddFinancialPeriod.SelectedValue.Substring(4, 2));
                    filters1[31] = new ReportParameter("Parameter32", ddFinancialPeriod.SelectedValue.Substring(4, 2));
                    filters1[32] = new ReportParameter("Parameter33", ddFinancialPeriod.SelectedValue.Substring(0, 4));
                    filters1[33] = new ReportParameter("Parameter34", ddFinancialPeriod.SelectedValue.Substring(4, 2));
                    filters1[34] = new ReportParameter("Parameter35", ddStore.SelectedValue);
                    filters1[35] = new ReportParameter("Parameter36", ddStore.SelectedValue);
                    filters1[36] = new ReportParameter("Parameter37", ddRegion.SelectedValue);
                    filters1[37] = new ReportParameter("Parameter38", ddRegion.SelectedValue);
                    filters1[38] = new ReportParameter("Parameter39", "A");
                    filters1[39] = new ReportParameter("Parameter40", "A");
                    filters1[40] = new ReportParameter("Parameter41", "All");
                    filters1[41] = new ReportParameter("Parameter42", "All");
                    ReportViewer1.ServerReport.SetParameters(filters1);
                    break;
                case ("StoresBalanceSheet"):
                    ReportParameter[] filters2 = new ReportParameter[9];
                    filters2[0] = new ReportParameter("Parameter1", ddFinancialPeriod.SelectedValue.Substring(4, 2));
                    filters2[1] = new ReportParameter("Parameter2", ddFinancialPeriod.SelectedValue.Substring(0, 4));
                    filters2[2] = new ReportParameter("Parameter3", ddFinancialPeriod.SelectedValue.Substring(4, 2));
                    filters2[3] = new ReportParameter("Parameter4", ddStore.SelectedValue);
                    filters2[4] = new ReportParameter("Parameter5", ddStore.SelectedValue);
                    filters2[5] = new ReportParameter("Parameter6", ddRegion.SelectedValue);
                    filters2[6] = new ReportParameter("Parameter7", ddRegion.SelectedValue);
                    filters2[7] = new ReportParameter("Parameter8", "10100");
                    filters2[8] = new ReportParameter("Parameter9", "39999");
                    ReportViewer1.ServerReport.SetParameters(filters2);
                    break;
                case ("Historical Financials"):
                    ReportParameter[] filters3 = new ReportParameter[7];
                    filters3[0] = new ReportParameter("Parameter1", ddFinancialPeriod.SelectedValue.Substring(0, 4));
                    filters3[1] = new ReportParameter("Parameter2", ddFinancialPeriod.SelectedValue.Substring(0, 4));
                    filters3[2] = new ReportParameter("Parameter3", ddStore.SelectedValue);
                    filters3[3] = new ReportParameter("Parameter4", ddStore.SelectedValue);
                    filters3[4] = new ReportParameter("Parameter5", ddRegion.SelectedValue);
                    filters3[5] = new ReportParameter("Parameter6", ddRegion.SelectedValue);
                    filters3[6] = new ReportParameter("Parameter7", ddFinancialPeriod.SelectedValue.Substring(4, 2));
                    ReportViewer1.ServerReport.SetParameters(filters3);
                    break;
                    case ("/YTD Labor Pricing Data"):
                    ReportParameter[] filters4 = new ReportParameter[10];

                    filters4[0] = new ReportParameter("period");
                    filters4[0].Values.AddRange(salesPeriod);
                    filters4[1] = new ReportParameter("Region");
                    filters4[1].Values.AddRange(salesRegion);
                    filters4[2] = new ReportParameter("Store");
                    filters4[2].Values.AddRange(salesStore);
                    filters4[3] = new ReportParameter("UserID", lblUserID.Text);
                    filters4[4] = new ReportParameter("PrdCls");
                    filters4[4].Values.AddRange(salesPrdCls);
                    filters4[5] = new ReportParameter("PrdVnd");
                    filters4[5].Values.AddRange(salesPrdVnd);
                    filters4[6] = new ReportParameter("Salesman");
                    filters4[6].Values.AddRange(salesSalesman);
                    if (txtPrdNo.Text == "PRODUCT NUMBER" || txtPrdNo.Text == String.Empty)
                    {
                        filters4[7] = new ReportParameter("prdnum", "All");
                    }
                    else
                    {
                        filters4[7] = new ReportParameter("prdnum", txtPrdNo.Text);
                    }
                    filters4[8] = new ReportParameter("cstcls");
                    filters4[8].Values.AddRange(salesCustClass);
                    filters4[9] = new ReportParameter("year", ddSalesYear.SelectedValue.ToString());
                    ReportViewer1.ServerReport.SetParameters(filters4);
                    break;
                case ("PandLALL-All Depts"):
                    ReportParameter[] filters5 = new ReportParameter[13];
                    filters5[0] = new ReportParameter("Parameter1", ddFinancialPeriod.SelectedValue.Substring(4, 2));
                    filters5[1] = new ReportParameter("Parameter2", ddFinancialPeriod.SelectedValue.Substring(4, 2));
                    filters5[2] = new ReportParameter("Parameter3", ddFinancialPeriod.SelectedValue.Substring(0, 4));
                    filters5[3] = new ReportParameter("Parameter4", ddFinancialPeriod.SelectedValue.Substring(0, 4));
                    filters5[4] = new ReportParameter("Parameter5", ddFinancialPeriod.SelectedValue.Substring(4, 2));
                    filters5[5] = new ReportParameter("Parameter6", "10");
                    filters5[6] = new ReportParameter("Parameter7", ddRegion.SelectedValue);
                    filters5[7] = new ReportParameter("Parameter8", ddRegion.SelectedValue);
                    filters5[8] = new ReportParameter("Parameter9", ddStore.SelectedValue);
                    filters5[9] = new ReportParameter("Parameter10", ddStore.SelectedValue);
                    filters5[10] = new ReportParameter("Parameter11", "AA");
                    filters5[11] = new ReportParameter("Parameter12", "AA");
                    if (ddStore.SelectedValue.ToString() != "0")
                    {
                        filters5[12] = new ReportParameter("ReportParameter1", "CONSOLIDATED");
                    }
                    else
                    {
                        filters5[12] = new ReportParameter("ReportParameter1", ddReportType.SelectedItem.Text);
                    }

                    ReportViewer1.ServerReport.SetParameters(filters5);
                    break;
                case ("Recap Earnings Statement"):

                    int pr = Convert.ToInt32(ddFinancialPeriod.SelectedValue.Substring(4, 2));

                    ReportParameter[] filters6 = new ReportParameter[3];
                        filters6[0] = new ReportParameter("Parameter1", ddFinancialPeriod.SelectedValue.Substring(0, 4));
                        filters6[1] = new ReportParameter("Parameter2", "1");
                        filters6[2] = new ReportParameter("Parameter3", pr.ToString());
                        ReportViewer1.ServerReport.SetParameters(filters6);
                    break;
                case ("MTD Recap Earnings Statement"):

                    int pr1 = Convert.ToInt32(ddFinancialPeriod.SelectedValue.Substring(4, 2));

                    ReportParameter[] filters7 = new ReportParameter[3];
                    filters7[0] = new ReportParameter("Parameter1", ddFinancialPeriod.SelectedValue.Substring(0, 4));
                    filters7[1] = new ReportParameter("Parameter2", pr1.ToString());
                    filters7[2] = new ReportParameter("Parameter3", pr1.ToString());
                    ReportViewer1.ServerReport.SetParameters(filters7);
                    break;
                case ("Recaps List GL Income Statement"):

                    int period = Convert.ToInt32(ddFinancialPeriod.SelectedValue.Substring(4, 2));

                    ReportParameter[] filters8 = new ReportParameter[5];
                    filters8[0] = new ReportParameter("Parameter1", period.ToString());
                    filters8[1] = new ReportParameter("Parameter2", period.ToString());
                    filters8[2] = new ReportParameter("Parameter3", ddFinancialPeriod.SelectedValue.Substring(0, 4));
                    filters8[3] = new ReportParameter("Parameter4", ddFinancialPeriod.SelectedValue.Substring(0, 4));
                    filters8[4] = new ReportParameter("Parameter5", period.ToString());
                    //filters8[5] = new ReportParameter("Parameter7", ddRegion.SelectedValue);

                    ReportViewer1.ServerReport.SetParameters(filters8);
                    break;
                case ("RecapShopsBalanceSheet"):

                    int period1 = Convert.ToInt32(ddFinancialPeriod.SelectedValue.Substring(4, 2));

                    ReportParameter[] filters9 = new ReportParameter[3];
                    filters9[0] = new ReportParameter("Parameter1", period1.ToString());
                    filters9[1] = new ReportParameter("Parameter2", ddFinancialPeriod.SelectedValue.Substring(0, 4));
                    filters9[2] = new ReportParameter("Parameter3", period1.ToString());
                    //filters8[5] = new ReportParameter("Parameter7", ddRegion.SelectedValue);

                    ReportViewer1.ServerReport.SetParameters(filters9);
                    break;
                case ("YTD_Sales"):
                    break;
            }
                    ReportViewer1.ServerReport.Refresh();

            /*byte[] data = ReportViewer1.ServerReport.Render("pdf");
            Response.Clear();
            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-Disposition", string.Format("attachment;filename=\"{0}\"", "P&L.pdf"));
            Response.BinaryWrite(data);
            
            FileStream stream = File.OpenWrite("c:\\My Destination Folder Name>\\P&L.pdf");
            stream.Write(results, 0, results.Length);
            stream.Close();*/
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    /*protected void save_pdf()
    {
        String path_name = "~/PDF/";
        var pdfPath = Path.Combine(Server.MapPath(path_name));
        var formFieldMap = PDFHelper.GetFormFieldNames(pdfPath);

        string username = "Test";
        string password = "12345";
        String file_name_pdf = "Test.pdf";

        var pdfContents = PDFHelper.GeneratePDF(pdfPath, formFieldMap);

        File.WriteAllBytes(Path.Combine(pdfPath, file_name_pdf), pdfContents);

        WebRequest request = WebRequest.Create(Server.MapPath("~/PDF/" + pdfContents));
        request.Method = WebRequestMethods.Ftp.UploadFile;

        request.Credentials = new NetworkCredential(username, password);
        Stream reqStream = request.GetRequestStream();
        reqStream.Close();
    }*/

    private string ReportPeriod(ListBox periodParameter)
    {
        string period;
        try
        {
            var periodList = new List<string>();
            int i = 0;
            foreach (ListItem li in periodParameter.Items)
            {
                if (li.Selected)
                {
                    periodList.Add(li.Value.ToString());
                    i++;
                }

            }
                if(i == 0) { periodList.Add("0"); }
            var periodRows = periodList.ToArray();
            period = (string.Join(",", periodRows));
        }
        catch (Exception ex)
        {
            period = (ex.Message);
        }

        return period;
    }
    private string ReportPeriod2(ListBox periodParameter)
    {
        string period;
        try
        {
            var periodList2 = new List<string>();
            int i = 0;
            foreach (ListItem li in periodParameter.Items)
            {
                if (li.Selected)
                {
                    cblSalesPeriod.Items.Add(li.Value.ToString());
                    i++;
                }

            }
            if (i == 0) { cblSalesPeriod.Items.Add("0"); }
            var periodRows = cblSalesPeriod.Items.ToString().ToArray();
            period = (string.Join(",", periodRows));
        }
        catch (Exception ex)
        {
            period = (ex.Message);
        }

        return period;
    }
    private string ReportCustClass(ListBox custClassParameter)
    {
        string custCls;
        try
        {
            var custClsList = new List<string>();
            int i = 0;
            foreach (ListItem li in custClassParameter.Items)
            {
                if (li.Selected)
                {
                    custClsList.Add(li.Value.ToString());
                    i++;
                }
            }
            if(i == 0) { custClsList.Add("A"); }
            var custClsRows = custClsList.ToArray();
            custCls = (string.Join(",", custClsRows));
        }
        catch (Exception ex)
        {
            custCls = (ex.Message);
        }

        return custCls;
    }
    private string ReportSalesman(ListBox salesmanParameter)
    {
        string salesman;
        try
        {
            var salesmanList = new List<string>();
            int i = 0;
            foreach (ListItem li in salesmanParameter.Items)
            {
                if (li.Selected)
                {
                    salesmanList.Add(li.Value.ToString());
                    i++;
                }
            }
                if (i < 1)
                { salesmanList.Add("0"); }
            var periodRows = salesmanList.ToArray();
            salesman = (string.Join(",", periodRows));
        }
        catch (Exception ex)
        {
            salesman = (ex.Message);
        }

        return salesman;
    }

    private string ReportStore(ListBox storeParameter)
    {
        string store;
        try
        {
            var storeList = new List<string>();
            int i = 0;
            foreach (ListItem li in storeParameter.Items)
            {
                if (li.Selected)
                {
                    storeList.Add(li.Value.ToString());
                    i++;
                }
            }
            if(i == 0) { storeList.Add("0"); }
            var storeRows = storeList.ToArray();
            store = (string.Join(",", storeRows));
        }
        catch (Exception ex)
        {
            store = (ex.Message);
        }

        return store;
    }
    private string ReportRegion(ListBox regionParameter)
    {
        string region;
        try
        {
            var regionList = new List<string>();
            int i = 0;
            foreach (ListItem li in regionParameter.Items)
            {
                if (li.Selected)
                {
                    regionList.Add(li.Value.ToString());
                    i++;
                }
            }
            if(i == 0) { regionList.Add("99"); }
            var regionRows = regionList.ToArray();
            region = (string.Join(",", regionRows));
        }
        catch (Exception ex)
        {
            region = (ex.Message);
        }

        return region;
    }
    private string ReportPrdCls(ListBox prdClsParameter)
    {
        string prdCls;
        try
        {
            var prdClsList = new List<string>();
            int i = 0;
            foreach (ListItem li in prdClsParameter.Items)
            {
                if (li.Selected)
                {
                    prdClsList.Add(li.Value.ToString());
                    i++;
                }
            }
            if(i == 0) { prdClsList.Add("AA"); }
            var prdClsRows = prdClsList.ToArray();
            prdCls = (string.Join(",", prdClsRows));
        }
        catch (Exception ex)
        {
            prdCls = (ex.Message);
        }

        return prdCls;
    }
    private string ReportPrdVnd(ListBox prdClsParameter)
    {
        string prdVnd;
        try
        {
            var prdVndList = new List<string>();
            int i = 0;
            foreach (ListItem li in prdClsParameter.Items)
            {
                if (li.Selected)
                {
                    prdVndList.Add(li.Value.ToString());
                    i++;
                }
            }
            if(i == 0) { prdVndList.Add("All"); }
            var prdVndRows = prdVndList.ToArray();
            prdVnd = (string.Join(",", prdVndRows));
        }
        catch (Exception ex)
        {
            prdVnd = (ex.Message);
        }

        return prdVnd;
    }
    private void ActivateTable(string TableID)
    {
        switch (TableID)
        {
            case ("Financials"):
                tblFinancials.Visible = true;
                tblInventory.Visible = false;
                tblSales.Visible = false;
                break;
            case ("Inventory"):
                tblFinancials.Visible = false;
                tblInventory.Visible = true;
                tblSales.Visible = false;
                btnPreview.Visible = false;
                break;
            case ("Sales"):
                tblFinancials.Visible = false;
                tblInventory.Visible = false;
                ddSalesReport.DataBind();
                if (ddSalesReport.SelectedItem.Text == "")
                {
                    tblSales.Visible = false;
                    btnPreview.Visible = false;
                    Image1.Visible = true;
                }
                else {
                    Image1.Visible = false;
                SalesReportsFilters();
                //ddSalesYear.Height = 10;
                ddSalesReport.Font.Size = 9;
                ddSalesReport.Width = 200;
                ddSalesYear.Width = 200;
                ddSalesYear.Font.Size = 9;
                lbSalesPeriod.Rows = 8;
                lbSalesPeriod.Width = 200;
                lbSalesPeriod.Font.Size = 9;
                lbCustCls.Rows = 8;
                lbCustCls.Width = 200;
                lbCustCls.Font.Size = 9;
                lbPrdCls.Rows = 8;
                lbPrdCls.Width = 200;
                lbPrdCls.Font.Size = 9;
                lbPrdVnd.Rows = 8;
                lbPrdVnd.Width = 200;
                lbPrdVnd.Font.Size = 9;
                lbSalesStore.Rows = 8;
                lbSalesStore.Width = 200;
                lbSalesStore.Font.Size = 9;
                lbSalesRegion.Rows = 8;
                lbSalesRegion.Width = 200;
                lbSalesRegion.Font.Size = 9;
                lbSalesman.Rows = 8;
                lbSalesman.Width = 200;
                lbSalesman.Font.Size = 9;
                txtPrdNo.Width = 200;
                txtPrdNo.Font.Size = 9;
                txtPrdNo.Text = String.Empty;
                tblSales.Visible = true;
                btnPreview.Visible = true;
                }
                break;
            case ("All_YTD_Sales"):
                tblFinancials.Visible = false;
                tblInventory.Visible = false;
                tblSales.Visible = false;
                btnPreview.Visible = false;
                break;
        }
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