using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using Microsoft.Reporting.WebForms;

public partial class asp_Intranet_Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lblMessage.Visible = false;
        }
    }
    protected void btnPreview_Click(object sender, EventArgs e)
    {
        gvStoresList.DataBind();
    }
    protected void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {
            ServerReport serverReport = ReportViewer1.ServerReport;
            ReportViewer1.ProcessingMode = ProcessingMode.Remote;
            serverReport.ReportServerUrl = new Uri("http://gbsql01/reportserver");
            serverReport.ReportPath = "/Intranet/StoreLocations2";

            ReportParameter[] filters = new ReportParameter[2];
            filters[0] = new ReportParameter("Region", ddRegion.SelectedValue);
            filters[1] = new ReportParameter("Division", ddDivision.SelectedValue);
            ReportViewer1.ServerReport.SetParameters(filters);
            //ReportViewer1.ServerReport.Refresh();

            byte[] data = ReportViewer1.ServerReport.Render("pdf");
            Response.Clear();
            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-Disposition", string.Format("attachment;filename=\"{0}\"", "Store_Locations.pdf"));
            Response.BinaryWrite(data);
        }
        catch
        {
            lblMessage.ForeColor = Color.Red;
            lblMessage.Text = "We are experiencing some issues with our server, please try again later";
        }
    }


    protected void ImgBtnExcel_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            ServerReport serverReport = ReportViewer1.ServerReport;
            ReportViewer1.ProcessingMode = ProcessingMode.Remote;
            serverReport.ReportServerUrl = new Uri("http://gbsql01/reportserver");
            serverReport.ReportPath = "/Intranet/Stores List";

            ReportParameter[] filters = new ReportParameter[2];
            filters[0] = new ReportParameter("Region", ddRegion.SelectedValue);
            filters[1] = new ReportParameter("Division", ddDivision.SelectedValue);
            ReportViewer1.ServerReport.SetParameters(filters);
            //ReportViewer1.ServerReport.Refresh();

            byte[] data = ReportViewer1.ServerReport.Render("excel");
            Response.Clear();
            Response.ContentType = "application/excel";
            Response.AddHeader("Content-Disposition", string.Format("attachment;filename=\"{0}\"", "Store_Locations.xls"));
            Response.BinaryWrite(data);
        }
        catch
        {
            lblMessage.ForeColor = Color.Red;
            lblMessage.Text = "We are experiencing some issues with our server, please try again later";
        }
    }
}