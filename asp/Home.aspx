<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Home.aspx.cs" Inherits="asp_Home" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>


<asp:Content ID="Content5" ContentPlaceHolderID="cpMainContent" Runat="Server">
    <div id="MainContent">
        <table style="width:100%;height:100%">
            <tr>
                <td colspan="3" style="font-size:1.2em;color:gray;text-align:right;padding:5px;"><asp:Label ID="lblTitle" runat="server" Text=""></asp:Label>&nbsp;|&nbsp;
                    <asp:Label ID="lblYear2" runat="server" Text=""></asp:Label></td>
            </tr>
            <tr>
                <td style="width:15%">&nbsp;&nbsp;</td>
                <td style="width:70%">
                    <rsweb:ReportViewer ID="ReportViewer4" runat="server"></rsweb:ReportViewer></td>
                <td style="width:15%">&nbsp;&nbsp;</td>
            </tr>
            <tr>
                <td colspan="3" style="padding:10px;">
                    <asp:CheckBox ID="cbExcludeIntracompany" runat="server" AutoPostBack="true" OnCheckedChanged="cbExcludeIntracompany_CheckedChanged" />&nbsp;<asp:Label ID="lblIntracompany" runat="server" Text="Exclude Intracompany Sales"></asp:Label></td>
            </tr>
        </table>
        <asp:Label ID="lblUserAccess" runat="server" Text=""></asp:Label><asp:Label ID="lblReportName" runat="server" Text=""></asp:Label>
            </div>
</asp:Content>

