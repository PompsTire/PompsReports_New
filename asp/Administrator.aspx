<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Administrator.aspx.cs" Inherits="Administrator" %>


<asp:Content ID="Content5" ContentPlaceHolderID="cpMainContent" Runat="Server">
    <div id="MainContent">
        <div id="Login">
            <blockquote>
            <asp:Label ID="lblInstructions" runat="server" Text=""></asp:Label></blockquote>
            <asp:Panel ID="panelUserAccess" runat="server">
            <table style="width:950px">
                <tr style="padding:25px">
                    <td colspan="4">Username:&nbsp;
                        <asp:DropDownList ID="ddUsername" runat="server" DataSourceID="UserNameSqlDataSource" DataTextField="UserLogin" DataValueField="UserID" Font-Size="Large" AutoPostBack="true" OnTextChanged="ddUsername_TextChanged"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th>
                        <asp:CheckBox ID="cbRegion" runat="server" AutoPostBack="true" OnCheckedChanged="cbRegion_CheckedChanged" />Region</th>
                    <th>
                        <asp:CheckBox ID="cbStore" runat="server" AutoPostBack="true" OnCheckedChanged="cbStore_CheckedChanged" />Store</th>
                    <th>
                        <asp:CheckBox ID="cbSalesClass" runat="server" AutoPostBack="true" OnCheckedChanged="cbSalesClass_CheckedChanged" />Sales Class</th>
                    <th>
                        <asp:CheckBox ID="cbSalesman" runat="server" AutoPostBack="true" OnCheckedChanged="cbSalesman_CheckedChanged" />Salesman</th>
                </tr>
                <tr>
                    <td colspan="4">
            <asp:CheckBoxList ID="cblStore" runat="server" DataTextField="StoreName" DataValueField="StoreID" Font-Size="Small" RepeatColumns="6" Width="950px"></asp:CheckBoxList>
                        <asp:CheckBoxList ID="cblRegion" runat="server" Font-Size="Medium" RepeatColumns="2" Width="450px"></asp:CheckBoxList>
                        <asp:CheckBoxList ID="cblSalesman" runat="server" Font-Size="XSmall" RepeatColumns="8" Width="1050px"></asp:CheckBoxList>
                        <asp:CheckBoxList ID="cblSalesClass" runat="server" Font-Size="Medium" RepeatColumns="2" Width="650px"></asp:CheckBoxList>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:Button ID="btnSubmit" runat="server" Text="" OnClick="btnSubmit_Click" /></td>
                </tr>
            </table></asp:Panel>
            <asp:Table ID="tblEmail" runat="server">
                <asp:TableRow>
                    <asp:TableCell>From</asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtEmailFrom" runat="server" Width="350px"></asp:TextBox></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow><asp:TableCell>To</asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtEmailTo" runat="server" Width="350px"></asp:TextBox></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>Subject</asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtEmailSubject" runat="server" Width="350px"></asp:TextBox></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell ColumnSpan="2">
                        <asp:TextBox ID="txtEmailMsg" runat="server" TextMode="MultiLine" Rows="10" Width="450px"></asp:TextBox></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell ColumnSpan="2">
                        <asp:Button ID="btnSendEmail" runat="server" Text="Send" OnClick="btnSendEmail_Click" />&nbsp;&nbsp;&nbsp;<asp:Button ID="btnCancelEmail" runat="server" Text="Cancel" OnClick="btnCancelEmail_Click" /></asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            <asp:Table ID="tblFinancialPeriod" runat="server">
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:TextBox ID="txtFinancialPeriod" runat="server" Width="450px"></asp:TextBox></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                    <asp:Button ID="btnCloseFinancialPeriod" runat="server" Text="Save and Send Notification" OnClick="btnCloseFinancialPeriod_Click"/></asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            <asp:Table ID="tblAdminMenu" runat="server" CellPadding="10">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell></asp:TableHeaderCell>
                </asp:TableHeaderRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <ul>
                            <li><asp:LinkButton ID="lnkBtnCloseFinancialPeriod" runat="server" OnClick="lnkBtnCloseFinancialPeriod_Click">Close Financial Period</asp:LinkButton></li>
                            <li>
                                <asp:LinkButton ID="lnkBtnEmailNotification" runat="server" OnClick="lnkBtnEmailNotification_Click">E-Mail Notification</asp:LinkButton></li>
                            <li>
                                <asp:LinkButton ID="lnkBtnUserAccess" runat="server" OnClick="lnkBtnUserAccess_Click">User Access</asp:LinkButton></li>
                        </ul>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
                        <asp:SqlDataSource ID="UserNameSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:Pomps_ReportsConnectionString %>" SelectCommand="SELECT [UserID], [UserLogin] FROM [Database_User] ORDER BY [UserLogin]"></asp:SqlDataSource>
            <asp:SqlDataSource ID="StoreSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:Pomps_ReportsConnectionString %>" SelectCommand="SELECT [StoreID], [StoreNo],convert(varchar(3),StoreNo) + ' ' +  Rtrim(StoreName) as StoreName FROM [Store]
WHERE StoreName not like 'CLOSED%' and StoreName not like 'TEST%' and (StoreNo = 0 or StoreNo between 2 and 600)
ORDER BY StoreNo"></asp:SqlDataSource>
        </div>
        <asp:Label ID="lblUserAccess" runat="server" Text=""></asp:Label>
        <asp:Label ID="lblUsername" runat="server" Text=""></asp:Label>
    </div>
</asp:Content>

