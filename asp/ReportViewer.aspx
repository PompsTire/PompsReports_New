<%@ Page Title="" Language="C#" Debug="true" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ReportViewer.aspx.cs" Inherits="asp_ReportViewer" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content5" ContentPlaceHolderID="cpMainContent" Runat="Server">
    <div id="MainContent">
        <table style="width:100%;height:100%">
            <tr>
                <td colspan="3" style="font-size:1.2em;color:gray;text-align:right;padding:5px;">
                <asp:Label ID="lblTitle" runat="server" Text=""></asp:Label>&nbsp;|&nbsp;
                    <asp:Label ID="lblYear2" runat="server" Text=""></asp:Label></td>
            </tr>
            <tr>
                <td style="width:15%">&nbsp;&nbsp;</td>
                <td style="width:70%"><rsweb:ReportViewer ID="ReportViewer1" runat="server" AsyncRendering="False" Width="1050px" Height="650px"></rsweb:ReportViewer></td>
                <td style="width:15%">&nbsp;&nbsp;</td>
            </tr>
        </table>
        <asp:Label ID="lblUserID" runat="server" Text="">
            <asp:Label ID="lblReportName" runat="server" Text=""></asp:Label></asp:Label>
    </div>
</asp:Content>

