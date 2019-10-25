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
using System.Net;
using System.Net.Mail;

public partial class Administrator : System.Web.UI.Page
{
    string sqlConnection = ConfigurationManager.ConnectionStrings["Pomps_ReportsConnectionString"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string username = Request.QueryString.Get("Username").ToString();
            lblUsername.Text = username;
            lblUserAccess.Text = RetrieveUserAccess(lblUsername.Text);
            ClearScreen();
        }
    }

    protected void cbStore_CheckedChanged(object sender, EventArgs e)
    {
        if (cbStore.Checked == true)
        {
            ClearScreen();
            lblInstructions.Text = "Select Store";
            panelUserAccess.Visible = true;
            tblAdminMenu.Visible = false;
            tblEmail.Visible = false;
            tblFinancialPeriod.Visible = false;
            cbStore.Checked = true;
            cblStore.Visible = true;
            UserStoreAccess();
            btnSubmit.Visible = true;
            btnSubmit.Text = "Save";
        }
    }

    protected void cbRegion_CheckedChanged(object sender, EventArgs e)
    {
        ClearScreen();
        lblInstructions.Text = "Select Region";
        panelUserAccess.Visible = true;
        tblAdminMenu.Visible = false;
        tblEmail.Visible = false;
        tblFinancialPeriod.Visible = false;
        cbRegion.Checked = true;
        cblRegion.Visible = true;
        UserRegionAccess();
        btnSubmit.Visible = true;
        btnSubmit.Text = "Save";
    }

    protected void cbSalesClass_CheckedChanged(object sender, EventArgs e)
    {
        ClearScreen();
        lblInstructions.Text = "Select Sales Class";
        panelUserAccess.Visible = true;
        tblAdminMenu.Visible = false;
        tblEmail.Visible = false;
        tblFinancialPeriod.Visible = false;
        cblSalesClass.Visible = true;
        cbStore.Checked = false;
        cbRegion.Checked = false;
        cbSalesClass.Checked = true;
        cbSalesman.Checked = false;
        UserSalesClassAccess();
        btnSubmit.Visible = true;
        btnSubmit.Text = "Save";
    }

    protected void cbSalesman_CheckedChanged(object sender, EventArgs e)
    {
        ClearScreen();
        lblInstructions.Text = "Select Salesman Accounts to Access";
        panelUserAccess.Visible = true;
        tblAdminMenu.Visible = false;
        tblEmail.Visible = false;
        tblFinancialPeriod.Visible = false;
        cbSalesman.Checked = true;
        cblSalesman.Visible = true;
        UserSalesmanAccess();
        btnSubmit.Visible = true;
        btnSubmit.Text = "Save";
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        switch (lblInstructions.Text)
        {
            case ("Select Store"):
                foreach (ListItem store in cblStore.Items)
                {
                    if (store.Selected == true)
                    {
                        SaveUserStoreAccess();
                        store.Selected = false;
                    }
                }
                break;
            case ("Select Region"):
                foreach (ListItem region in cblRegion.Items)
                {
                    if (region.Selected == true)
                    {
                        SaveUserRegionAccess();
                        region.Selected = false;
                    }
                }
                break;
            case ("Select Salesman Accounts to Access"):
                foreach (ListItem Salesman in cblSalesman.Items)
                {
                    if (Salesman.Selected == true)
                    {
                        UserSalesmanAccess();
                        Salesman.Selected = false;
                    }
                }
                break;
            case ("Select Sales Class"):
                foreach (ListItem SalesClass in cblSalesClass.Items)
                {
                    if (SalesClass.Selected == true)
                    {
                        SaveUserSalesClassAccess();
                        SalesClass.Selected = false;
                    }
                }
                break;
            default:
                ClearScreen();
                break;
        }
    }

    protected void lnkBtnUserAccess_Click(object sender, EventArgs e)
    {
        ClearScreen();
        panelUserAccess.Visible = true;
        tblAdminMenu.Visible = false;
        tblEmail.Visible = false;
    }

    protected void lnkBtnEmailNotification_Click(object sender, EventArgs e)
    {
        ClearScreen();
    }

    protected void lnkBtnCloseFinancialPeriod_Click(object sender, EventArgs e)
    {
        ClearScreen();
        lblInstructions.Text = "Enter Financial Period:";
        panelUserAccess.Visible = false;
        txtFinancialPeriod.Height = 18;
        tblFinancialPeriod.Visible = true;
        tblAdminMenu.Visible = false;
        tblEmail.Visible = false;
    }

    protected void btnCloseFinancialPeriod_Click(object sender, EventArgs e)
    {
        using (SqlConnection connection = new SqlConnection(sqlConnection))
        {
            string month;
            if(Convert.ToInt16(DateTime.Now.Month.ToString()) > 9 ) { month = DateTime.Now.Month.ToString(); } else { month = "0" + DateTime.Now.Month.ToString(); }
            if(Convert.ToInt16(txtFinancialPeriod.Text.Substring(4,2).ToString()) > 13 || Convert.ToInt16(txtFinancialPeriod.Text.Substring(4, 2).ToString()) < 1 || Convert.ToInt32(txtFinancialPeriod.Text.TrimEnd()) > Convert.ToInt32(DateTime.Now.Year.ToString() + month))
            {
                lblInstructions.Text = "Financial Period seems to be incorrect, please reenter in format (YYYYMM)";
            }
            else
            {
                string query = @"select Period_ID from Pomps_Reports.dbo.Period where Period = @Period";
                connection.Open();
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    try
                    {
                        cmd.Parameters.AddWithValue("@Period", txtFinancialPeriod.Text);
                        if (cmd.ExecuteScalar() != null)
                        {
                            lblInstructions.Text = "This Financial Period has previously been entered, try again";
                        }
                        else
                        {
                            SaveFinancialPeriod(txtFinancialPeriod.Text);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    connection.Close();
                }
            }
        }
    }

    protected void btnSendEmail_Click(object sender, EventArgs e)
    {
        using (SqlConnection connection = new SqlConnection(sqlConnection))
        {
            string query = @"select RTrim(UserLogin) + '@pompstire.com' as UserEmail from Pomps_Reports.dbo.Database_User
where RTrim(UserFrontPage) in('ADMIN','MGR') and UserLoginActive = 1 and EmailUser = 1";

            connection.Open();

            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                try
                {
                    string emailTo;
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while(sdr.Read())
                        {
                            emailTo = sdr["UserEmail"].ToString();
                            if (emailTo.Length > 2 )
                            {
                                try {
                                Email_Notification(txtEmailFrom.Text, emailTo, txtEmailSubject.Text, txtEmailMsg.Text);
                                }
                                catch (Exception ex)
                                {
                                    throw ex;
                                }
                            }
                            else {
                                lblInstructions.Text = "Could not send e-mail.";
                            }
                        }
                    }
                    ClearScreen();
                    lblInstructions.Text = "Your e-mail has been sent successfully.";
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                connection.Close();
            }
        }
    }

    protected void btnCancelEmail_Click(object sender, EventArgs e)
    {
        ClearScreen();
    }
    protected void ddUsername_TextChanged(object sender, EventArgs e)
    {
        //Response.Redirect(Request.Url.ToString());
        ClearScreen();
        //panelUserAccess.Visible = true;
        //tblAdminMenu.Visible = false;
        //tblEmail.Visible = false;
        //tblFinancialPeriod.Visible = false;
    }

    private void UserStoreAccess()
    {
        using (SqlConnection connection = new SqlConnection(sqlConnection))
        {
            string query = @"SELECT Store.StoreID AS StoreID,CONVERT(VARCHAR(3),Store.StoreNo) + ' ' + RTRIM(StoreName) AS StoreName,coalesce(UserStoreActive,0) as UserStoreActive
FROM Pomps_Reports.dbo.Store
LEFT OUTER JOIN Pomps_Reports.dbo.Database_User_Store ON Store.StoreID = Database_User_Store.StoreID AND UserID = @UserID
WHERE StoreName not like 'CLOSED%' and StoreName not like 'TEST%'
ORDER BY Store.StoreNo";

            connection.Open();

            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                try
                {
                    cmd.Parameters.AddWithValue("@UserID", ddUsername.SelectedValue.ToString());
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            ListItem item = new ListItem();
                            item.Text = sdr["StoreName"].ToString();
                            item.Value = sdr["StoreID"].ToString();
                            if (sdr["UserStoreActive"].ToString() == "1")
                            {
                                item.Selected = true;
                            }
                            else
                            {
                                item.Selected = false;
                            }
                            cblStore.Items.Add(item);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
        }
    }

    private void UserRegionAccess()
    {
        using (SqlConnection connection = new SqlConnection(sqlConnection))
        {
            string query = @"SELECT Region.RegionID, Convert(varchar(2),Region.RegionNo) + ' ' + RTrim(Region.RegionName) as Region,coalesce(Database_User_Region.UserRegionActive,0) as UserRegionActive
FROM Pomps_Reports.dbo.Region 
LEFT OUTER JOIN Pomps_Reports.dbo.Database_User_Region ON Region.RegionID = Database_User_Region.RegionID AND UserID = @UserID
ORDER BY Region.RegionName";

            connection.Open();

            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                try
                {
                    cmd.Parameters.AddWithValue("@UserID", ddUsername.SelectedValue.ToString());
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            ListItem item = new ListItem();
                            item.Text = sdr["Region"].ToString();
                            item.Value = sdr["RegionID"].ToString();
                            if (sdr["UserRegionActive"].ToString() == "1")
                            {
                                item.Selected = true;
                            }
                            else
                            {
                                item.Selected = false;
                            }
                            cblRegion.Items.Add(item);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
        }
    }

    private void UserSalesmanAccess()
    {
        using (SqlConnection connection = new SqlConnection(sqlConnection))
        {
            string query = @"SELECT s.SalesmanID as SalesmanID,convert(varchar(5),s.SalesmanNo) + ' ' + rtrim(SalesmanName) as Salesman
,SalesmanName,coalesce(UserSalesmanActive,0) as UserSalesmanActive
FROM Pomps_Reports.dbo.Salesman as s
LEFT OUTER JOIN Pomps_Reports.dbo.Database_User_Salesman as us ON s.SalesmanID = us.SalesmanID AND UserID = @UserID
ORDER BY SalesmanName";

            connection.Open();

            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                try
                {
                    cmd.Parameters.AddWithValue("@UserID", ddUsername.SelectedValue.ToString());
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            ListItem item = new ListItem();
                            item.Text = sdr["Salesman"].ToString();
                            item.Value = sdr["SalesmanID"].ToString();
                            if (sdr["UserSalesmanActive"].ToString() == "1")
                            {
                                item.Selected = true;
                            }
                            else
                            {
                                item.Selected = false;
                            }
                            cblSalesman.Items.Add(item);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
        }
    }

    private void UserSalesClassAccess()
    {
        using (SqlConnection connection = new SqlConnection(sqlConnection))
        {
            string query = @"SELECT SalesClass.SalesClassID as SalesClassID, convert(varchar(1),SalesClass.SalesClassCode) + ' ' + rtrim(SalesClassDescription) as SalesClass
,coalesce(SalesClassActive,0) as SalesClassActive FROM Pomps_Reports.dbo.SalesClass
LEFT OUTER JOIN Pomps_Reports.dbo.Database_User_Salesclass ON SalesClass.SalesClassID = Database_User_Salesclass.SalesClassID AND Database_User_Salesclass.UserID = @UserID
ORDER BY SalesClass.SalesClassCode";

            connection.Open();

            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                try
                {
                    cmd.Parameters.AddWithValue("@UserID", ddUsername.SelectedValue.ToString());
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            ListItem item = new ListItem();
                            item.Text = sdr["SalesClass"].ToString();
                            item.Value = sdr["SalesClassID"].ToString();
                            if (sdr["SalesClassActive"].ToString() == "1")
                            {
                                item.Selected = true;
                            }
                            else
                            {
                                item.Selected = false;
                            }
                            cblSalesClass.Items.Add(item);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
        }
    }
    private void SaveUserStoreAccess()
    {
        using (SqlConnection connection = new SqlConnection(sqlConnection))
        {
            string query = @"INSERT INTO Pomps_Reports.dbo.Database_User_Store (UserID,UserStoreActive,StoreID,StoreNo)
(SELECT @UserID as UserID,1 as UserStoreActive,@StoreID as StoreID,StoreNo
FROM Pomps_Reports.dbo.Store
WHERE StoreID = @StoreID)";

            connection.Open();

            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                try
                {
                    cmd.Parameters.AddWithValue("@UserID", ddUsername.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@StoreID", cblStore.SelectedValue.ToString());
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
        }
    }

    private void SaveUserRegionAccess()
    {
        using (SqlConnection connection = new SqlConnection(sqlConnection))
        {
            string query = @"INSERT INTO Pomps_Reports.dbo.Database_User_Region (UserID,RegionID,RegionNo,UserRegionActive)
(SELECT @UserID as UserID,@RegionID as RegionID,RegionNo,1 as UserRegionActive
FROM Pomps_Reports.dbo.Region
WHERE RegionID = @RegionID)";

            connection.Open();

            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                try
                {
                    cmd.Parameters.AddWithValue("@UserID", ddUsername.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@RegionID", cblRegion.SelectedValue.ToString());
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
        }
    }

    private void SaveUserSalesClassAccess()
    {
        using (SqlConnection connection = new SqlConnection(sqlConnection))
        {
            string query = @"INSERT INTO Pomps_Reports.dbo.Database_User_Salesclass (UserID,SalesClassID,SalesClassCode,SalesClassActive)
(SELECT @UserID as UserID,@SalesClassID,SalesClassCode,1 as SalesClassActive FROM Pomps_Reports.dbo.SalesClass Where SalesClassID = @SalesClassID)";

            connection.Open();

            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                try
                {
                    cmd.Parameters.AddWithValue("@UserID", ddUsername.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@SalesClassID", cblSalesClass.SelectedValue.ToString());
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                connection.Close();

            }
        }
    }

    private void SaveFinancialPeriod (string period)
    {
        using (SqlConnection connection = new SqlConnection(sqlConnection))
        {
            string query = @"INSERT INTO Pomps_Reports.dbo.Period (Period,Period_Report) VALUES (@Period,'Financials')";
            connection.Open();
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    try
                    {
                        cmd.Parameters.AddWithValue("@Period", period);
                        if (cmd.ExecuteScalar() != null)
                        {
                            lblInstructions.Text = "New Financial Period could not be saved.";
                        }
                        else {
                            lblInstructions.Text = "New Financial Period has been saved, click on send to send notifications to all Managers";
                            tblFinancialPeriod.Visible = false;
                            txtEmailFrom.Text = lblUsername.Text + "@pompstire.com";
                            txtEmailTo.Text = "Store Managers";
                            txtEmailSubject.Text = txtFinancialPeriod.Text +  " Financials";
                            txtEmailMsg.Text = "Financial Reports for Period " + txtFinancialPeriod.Text + " are now available at http://reports.pompstire.com.\n\nFor questions about your reports please reply to this message.\n\nFor questions about website access please contact Ofilia at oreifschneider@pompstire.com";
                            txtFinancialPeriod.Text = string.Empty;
                            tblEmail.Visible = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                connection.Close();
                }
        }
    }
    private string RetrieveUserAccess(string username)
    {
        using (SqlConnection connection = new SqlConnection(sqlConnection))
        {
            string query = @"SELECT UserID FROM Pomps_Reports.dbo.Database_User WHERE UserLogin = @UserLogin";

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
    private static void Email_Notification(string from,string to,string subject,string msg)
    {
        MailMessage Adj_Notification = new MailMessage();
        SmtpClient client = new SmtpClient();
        client.Port = 25;
        client.Host = "mail.pompstire.com";
        client.Timeout = 100000;
        client.DeliveryMethod = SmtpDeliveryMethod.Network;
        client.UseDefaultCredentials = false;
        client.Credentials = new System.Net.NetworkCredential("anonymous", "");
        Adj_Notification.From = new MailAddress(from);
        Adj_Notification.To.Add(new MailAddress(to));
        Adj_Notification.Bcc.Add(new MailAddress("oreifschneider@pompstire.com"));
        Adj_Notification.Subject = subject;
        Adj_Notification.IsBodyHtml = true;
        Adj_Notification.Body = msg;
        client.Send(Adj_Notification);
    }

    private void ClearScreen()
    {
        lblUserAccess.Visible = false;
        lblUsername.Visible = false;
        lblInstructions.Text = "";
        btnSubmit.Visible = false;
        cblStore.Items.Clear();
        cblStore.Visible = false;
        cblRegion.Items.Clear();
        cblRegion.Visible = false;
        cblSalesman.Items.Clear();
        cblSalesman.Visible = false;
        cblSalesClass.Items.Clear();
        cblSalesClass.Visible = false;
        cbSalesClass.Checked = false;
        cbRegion.Checked = false;
        cbSalesman.Checked = false;
        cbStore.Checked = false;
        txtEmailFrom.Text = String.Empty;
        txtEmailTo.Text = string.Empty;
        txtEmailSubject.Text = string.Empty;
        txtEmailMsg.Text = String.Empty;
        txtFinancialPeriod.Text = String.Empty;
        tblAdminMenu.Visible = true;
        panelUserAccess.Visible = false;
        tblEmail.Visible = false;
        tblFinancialPeriod.Visible = false;
    }
}