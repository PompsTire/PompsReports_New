using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

public partial class asp_Inventory_Age : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lblClass.Text = Request.QueryString.Get("pdclass").ToString();
            lblVendor.Text = Request.QueryString.Get("pdvendor").ToString();
            lblUserID.Text = Request.QueryString.Get("UserID").ToString();
            gvInventory_Age.DataBind();
        }
    }

    protected void ImageButton1_Click(object sender, EventArgs e)
    {
        {
            Response.ClearContent();
            Response.AppendHeader("content-disposition", "attachment; filename=Inventory Cost By Age Range.xls");
            Response.ContentType = "application/excel";

            StringWriter stringWriter = new StringWriter();
            HtmlTextWriter htmlTextWriter = new HtmlTextWriter(stringWriter);

            gvInventory_Age.HeaderRow.Style.Add("background-color", "#FFFFFF");

            foreach (TableCell tableCell in gvInventory_Age.HeaderRow.Cells)
            {
                tableCell.Style["background-color"] = "seagreen";
            }

            foreach (GridViewRow gridViewRow in gvInventory_Age.Rows)
            {
                gridViewRow.BackColor = System.Drawing.Color.White;
                foreach (TableCell gridViewRowTableCell in gridViewRow.Cells)
                {
                    gridViewRowTableCell.Style["background-color"] = "#E3EAEB";
                }
            }

            foreach (GridViewRow gridViewRow in gvInventory_Age.Rows)
            {
                gridViewRow.BackColor = System.Drawing.Color.White;
                foreach (TableCell gridViewRowTableCell in gridViewRow.Cells)
                {
                    gridViewRowTableCell.Style["background-color"] = "white";
                }
            }

            //Response.Write("");
            Response.Write(lblInventoryAgeRange.Text.ToString());
            gvInventory_Age.RenderControl(htmlTextWriter);
            Response.Write(stringWriter.ToString());
            Response.End();
        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
    }
}