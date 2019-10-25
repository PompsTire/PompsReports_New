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

public partial class asp_Gridview : System.Web.UI.Page
{
    string sqlConnection = ConfigurationManager.ConnectionStrings["Pomps_ReportsConnectionString"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            string userID, username, reportName;
            userID = Request.QueryString.Get("UserID").ToString();
            reportName = Request.QueryString.Get("reportQuery").ToString();
            username = "oreifschneider";
            //string username = Request.QueryString.Get("Username").ToString();
            //ReadUserAccess(userID);
            //reportName = "CustomerPurchases";
            RenderReport(reportName,userID);
        }
    }

    protected void imgExcelDownload_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Response.ClearContent();
            Response.AppendHeader("content-disposition", "attachment; filename=" + lblTitle.Text + ".xls");
            Response.ContentType = "application/excel";

            StringWriter stringWriter = new StringWriter();
            HtmlTextWriter htmlTextWriter = new HtmlTextWriter(stringWriter);

            GridView1.HeaderRow.Style.Add("background-color", "#FFFFFF");

            foreach (TableCell tableCell in GridView1.HeaderRow.Cells)
            {
                tableCell.Style["background-color"] = "seagreen";
            }

            foreach (GridViewRow gridViewRow in GridView1.Rows)
            {
                gridViewRow.BackColor = System.Drawing.Color.White;
                foreach (TableCell gridViewRowTableCell in gridViewRow.Cells)
                {
                    gridViewRowTableCell.Style["background-color"] = "#E3EAEB";
                }
            }

            foreach (GridViewRow gridViewRow in GridView1.Rows)
            {
                gridViewRow.BackColor = System.Drawing.Color.White;
                foreach (TableCell gridViewRowTableCell in gridViewRow.Cells)
                {
                    gridViewRowTableCell.Style["background-color"] = "white";
                }
            }

            //Response.Write("");
            Response.Write(lblTitle.Text.ToString());
            GridView1.RenderControl(htmlTextWriter);
            Response.Write(stringWriter.ToString());
            Response.End();
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    private void FillGridview(string userID,string parameter1, string parameter2, string parameter3, string parameter4,string myQuery)
    {
        SqlConnection sql = new SqlConnection(sqlConnection);
        lblSalesmanNo.Text = myQuery;
        sql.Open();
        try
        {
            using (SqlDataAdapter c = new SqlDataAdapter(myQuery, sql))
            {
                var custList = new DataTable();
                c.SelectCommand.Parameters.AddWithValue("@UserID1", userID);
                c.SelectCommand.Parameters.AddWithValue("@Parameter11", parameter1);
                c.SelectCommand.Parameters.AddWithValue("@Parameter22", parameter2);
                c.SelectCommand.Parameters.AddWithValue("@Parameter33", parameter3);
                c.SelectCommand.Parameters.AddWithValue("@Parameter44", parameter4);
                c.SelectCommand.CommandTimeout = 0;
                c.Fill(custList);
                GridView1.DataSource = custList;
                GridView1.DataBind();
                lblSalesmanNo.Visible = false;
                sql.Close();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public void RenderReport(string reportName,string userID)
    {
        string parameter1,parameter2,parameter3,parameter4;
        if(Request.QueryString.Get("reportName") != null)
        {
            lblTitle.Text = Request.QueryString.Get("reportName").ToString();
        }
        else
        {
            lblTitle.Text = String.Empty;
        }
        if(Request.QueryString.Get("Parameter1") != null)
        {
            parameter1 = Request.QueryString.Get("Parameter1").ToString();
        } else
        {
            parameter1 = "1";
        }
        if (Request.QueryString.Get("Parameter2") != null)
        {
            parameter2 = Request.QueryString.Get("Parameter2").ToString();
        }
        else
        {
            parameter2 = "1";
        }
        if (Request.QueryString.Get("Parameter3") != null)
        {
            parameter3 = Request.QueryString.Get("Parameter3").ToString();
        }
        else
        {
            parameter3 = "1";
        }
        if (Request.QueryString.Get("Parameter4") != null)
        {
            parameter4 = Request.QueryString.Get("Parameter4").ToString();
        }
        else
        {
            parameter4 = "1";
        }
        string myQuery = "exec Pomps_Reports.dbo.sp_" + reportName + " @UserID = @UserID1,@Parameter1=@Parameter11,@Parameter2=@Parameter22,@Parameter3=@Parameter33,@Parameter4=@Parameter44";
        FillGridview(userID,parameter1,parameter2,parameter3,parameter4,myQuery);
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
    }
    /*public Array storeAccess(string UserID)
    {
        SqlConnection sql = new SqlConnection(sqlConnection);
        sql.Open();
        try
        {
            SqlCommand cmd = new SqlCommand("exec Pomps_Reports.dbo.sp_GetUserStore @UserID = @UserID1", sql);
            cmd.Parameters.AddWithValue("@UserID1", UserID);
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                var myList = new List<string>();
                while (reader.Read())
                {
                    myList.Add(reader["StoreNo"].ToString());
                }
                var storeRows = myList.ToArray();
                Array str = storeRows;
                //string storeNo = ("'" + string.Join("','", storeRows) + "'");
                //lblStoreNo.Text = storeNo;
                sql.Close();
                return str;
            }
        }
        catch (Exception ex)
        {
            Array str = ex.Message.ToArray();
            return str;
        }
    }

    public Array salesmanAccess(string UserID)
    {
        SqlConnection sql = new SqlConnection(sqlConnection);
        sql.Open();
        try
        {

            SqlCommand cmd1 = new SqlCommand("exec Pomps_Reports.dbo.sp_GetUserSalesman @UserID = @UserID1", sql);
            cmd1.Parameters.AddWithValue("@UserID1", UserID);
            using (SqlDataReader reader = cmd1.ExecuteReader())
            {
                var myList = new List<string>();
                while (reader.Read())
                {
                    myList.Add(reader["SalesmanNo"].ToString());
                }
                var salesmanRows = myList.ToArray();
                Array slsm = salesmanRows;
                sql.Close();
                return slsm;
                //string salesmanNo = ("'" + string.Join("','", salesmanRows) + "'");
                //lblSalesmanNo.Text = salesmanNo;
            }

        }
        catch (Exception ex)
        {
            Array slsm = ex.Message.ToArray();
            return slsm;
        }
    }*/
}