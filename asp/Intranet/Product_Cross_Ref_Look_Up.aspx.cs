using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Data.Odbc;
using System.Configuration;

public partial class asp_Intranet_Product_Cross_Ref_Look_Up : System.Web.UI.Page
{
    string madden = ConfigurationManager.ConnectionStrings["MaddenCoConnectionString"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {

        }
    }

    protected void imgBtnSearch_Click(object sender, ImageClickEventArgs e)
    {
        Fill_Gridview(txtNAVendorItemNo.Text.ToUpper());
    }
    
    private void Fill_Gridview(string Item)
    {
        string myQuery = @"select case when trim(pxcvnumcst) = '0' then 'ALL' else trim(cuname) end as Customer_Name
,trim(pxcvnumvnd) as NA_Vendor_Number
,case when pxcvnumvnd = '410' then 'BANDAG BASYS' when pxcvnumvnd = '31370' then 'B/S F/S BANDAG' when pxcvnumvnd = '21870' then 'CONTINENTAL GENERAL' when pxcvnumvnd = '51290' then 'GOODYEAR' when pxcvnumvnd = '41882' then 'HANKOOK TIRE' when pxcvnumvnd = '60390' then 'MICHELIN TIRE' when pxcvnumvnd = '98350' then 'YOKOHAMA TIRE' end as NA_Vendor_Name
,trim(pxcvprdcv) as NA_Vendor_Item_No
,trim(pxcvprdmc) as Pomps_Item_No
,trim(pddescrip) as Item_Description
from dta577.tmpxcv inner join dta577.tmprod on pxcvprdmc = pdnumber and pdstore = '001' left outer join dta577.tmcust on pxcvnumcst = cunumber
where (pxcvprdcv = ? or pxcvprdmc = ? or (pddescrip like '% ' || ? || ' %')) and pxcvnumvnd in('21870','31370','41882','51290','60390','98350') order by pxcvprdmc";
        OdbcConnection cn = new OdbcConnection(madden);
        cn.Open();
        try
        {
            using (OdbcDataAdapter c = new OdbcDataAdapter(myQuery, cn))
            {
                var crossReferenceList = new DataTable();
                c.SelectCommand.Parameters.AddWithValue("Parameter1", Item);
                c.SelectCommand.Parameters.AddWithValue("Parameter2", Item);
                c.SelectCommand.Parameters.AddWithValue("Parameter3", Item);
                c.SelectCommand.CommandTimeout = 0;
                c.Fill(crossReferenceList);
                GridView1.DataSource = crossReferenceList;
                GridView1.DataBind();
                cn.Close();
            }
        }
        catch (Exception ex)
        {
            string error = ex.Message;
            lblError.Text = error;
        }
    }
    /*private void export_Gridview_To_Excel()
    {
        try {
        {
            Response.ClearContent();
            Response.AppendHeader("content-disposition", "attachment; filename=Product_Cross_Reference.xls");
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
            Response.Write(lblError.Text.ToString());
            GridView1.RenderControl(htmlTextWriter);
            Response.Write(stringWriter.ToString());
            Response.End();
        }
        }
        catch (Exception ex)
        {
            string error = ex.Message;
            lblError.Text = error;
        }
    }
    public override void VerifyRenderingInServerForm(Control control)
    {

    }*/
}