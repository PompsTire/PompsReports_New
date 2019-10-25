<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="asp_Default" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>



<asp:Content ID="Content5" ContentPlaceHolderID="cpMainContent" Runat="Server">
    <div id="main">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:Table ID="Table1" runat="server" Width-="1050px">
            <asp:TableRow>
                <asp:TableCell Width="240px" ForeColor="SeaGreen"  ColumnSpan="4">Total Sales &nbsp;
                    <asp:Label ID="lblTotalSales" runat="server" Text="" Font-Size="1.2em" Font-Bold="true" Font-Names="Euphemia"></asp:Label></asp:TableCell>
                <asp:TableCell RowSpan="4">
                    <rsweb:ReportViewer ID="ReportViewer2" runat="server" Font-Names="Verdana" Font-Size="8pt" ProcessingMode="Remote" ShowToolBar="False" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" ShowParameterPrompts="False" AsyncRendering="False" SizeToReportContent="True" ZoomPercent="100"></rsweb:ReportViewer></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell ColumnSpan="4">
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana" Font-Size="8pt" ProcessingMode="Remote" ShowToolBar="False" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" ShowParameterPrompts="False" AsyncRendering="True" SizeToReportContent="True" ZoomPercent="95" Height="400px">
        </rsweb:ReportViewer></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell ColumnSpan="4" Font-Size=".65em">
        <asp:CheckBox ID="cbExcludeIntercompany" runat="server" AutoPostBack="true" OnCheckedChanged="cbExcludeIntercompany_CheckedChanged" />Exclude Intercompany & Non-Merchandise</asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell ColumnSpan="4">
                    <asp:DropDownList ID="CustClass_DropDownList" runat="server" Font-Names="Euphemia" AutoPostBack="true"></asp:DropDownList></asp:TableCell>
               
            </asp:TableRow>
        </asp:Table>
        <asp:Label ID="lblUsername" runat="server" Text=""></asp:Label><br />
        <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
    </div>
</asp:Content>

