using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Drawing;

public partial class asp_Intranet_Salesman_Number_List : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        lblMessage.ForeColor = Color.Black;
        lblMessage.Text = "";
    }

    protected void imgBtnSearch_Click(object sender, ImageClickEventArgs e)
    {
        if (ddRegion.SelectedValue == "99" && ddStore.SelectedValue == "0" && txtName.Text == String.Empty && txtEmpNo.Text == String.Empty && txtSlsmNo.Text == String.Empty)
        {
            lblMessage.ForeColor = Color.Red;
            lblMessage.Text = "Please enter a value in at least one of the options given above.";
        }
        else
        {
            if (txtSlsmNo.Text == String.Empty) txtSlsmNo.Text = " ";

            if (txtEmpNo.Text == String.Empty) txtEmpNo.Text = " ";
            
            if (txtName.Text == String.Empty)
            {
                txtName.Text = " ";
            }
            else
            {
                txtName.Text = txtName.Text.ToUpper();
            }
            GridView1.DataBind();
        }
    }

    protected void imgBtnExcel_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
             Response.ClearContent();
             Response.AppendHeader("content-disposition", "attachment; filename=Employee_Sales_Numbers.xls");
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
            Response.Write("Employee Sales Numbers");
            GridView1.RenderControl(htmlTextWriter);
            Response.Write(stringWriter.ToString());
           Response.End();
        }
        catch (Exception ex)
        {
            lblMessage.ForeColor = Color.Red;
            lblMessage.Text = ex.Message;
        }
    }
    public override void VerifyRenderingInServerForm(Control control)
    {

    }
}