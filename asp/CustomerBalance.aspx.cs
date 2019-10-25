using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

public partial class asp_CustomerBalance : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //lblUserID.Text = Request.QueryString.Get("UserID").ToString();
            lblCustNumber.Text = Request.QueryString.Get("CustNumber").ToString();
            lblUserID.Text = Request.QueryString.Get("UserID").ToString();
            lblCustNumber.Visible = false;
            lblUserID.Visible = false;
            //lblPeriod.Text = Request.QueryString.Get("Period").ToString();
            gvCustomerBalance.DataBind();
        }
    }
    
    protected void ImageButton1_Click(object sender, EventArgs e)
    {
        {
            Response.ClearContent();
            Response.AppendHeader("content-disposition", "attachment; filename=Customers_Balance.xls");
            Response.ContentType = "application/excel";

            StringWriter stringWriter = new StringWriter();
            HtmlTextWriter htmlTextWriter = new HtmlTextWriter(stringWriter);

            gvCustomerBalance.HeaderRow.Style.Add("background-color", "#FFFFFF");

            foreach (TableCell tableCell in gvCustomerBalance.HeaderRow.Cells)
            {
                tableCell.Style["background-color"] = "seagreen";
            }

            foreach (GridViewRow gridViewRow in gvCustomerBalance.Rows)
            {
                gridViewRow.BackColor = System.Drawing.Color.White;
                foreach (TableCell gridViewRowTableCell in gridViewRow.Cells)
                {
                    gridViewRowTableCell.Style["background-color"] = "#E3EAEB";
                }
            }

            foreach (GridViewRow gridViewRow in gvCustomerBalance.Rows)
            {
                gridViewRow.BackColor = System.Drawing.Color.White;
                foreach (TableCell gridViewRowTableCell in gridViewRow.Cells)
                {
                    gridViewRowTableCell.Style["background-color"] = "white";
                }
            }

            //Response.Write("");
            Response.Write(lblCustomerBalance.Text.ToString());
            gvCustomerBalance.RenderControl(htmlTextWriter);
            Response.Write(stringWriter.ToString());
            Response.End();
        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
    }
}