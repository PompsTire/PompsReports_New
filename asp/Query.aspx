<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Query.aspx.cs" Inherits="asp_Query" %>


<asp:Content ID="Content5" ContentPlaceHolderID="cpMainContent" Runat="Server">
    <div id="MainContent">
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1"><ProgressTemplate>
            <!--<div class="PleaseWait"></div>-->
            <asp:Image ID="Image1" runat="server" ImageUrl="~/img/pleasewait.gif" Width="40px" Height="40px" /></ProgressTemplate></asp:UpdateProgress>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
    <table style="width:100%;background-color:ghostwhite;border:2px solid darkgray">
        <tr>
            <td style="text-align:left" colspan="2" rowspan="3">
                <div style="float:left;margin-top:0;width:400px">
        <asp:table ID="tblSelectQuery" runat="server" Width="100%" Height="100%" CellPadding="0">
            <asp:TableHeaderRow>
                <asp:TableHeaderCell ColumnSpan="4"><asp:Label ID="lblError" runat="server" Text=""></asp:Label></asp:TableHeaderCell>
            </asp:TableHeaderRow>
            <asp:tableRow>
                <asp:tableCell Width="100px" Font-Size=".75em">Query Name:</asp:tableCell>
                <asp:tableCell ColumnSpan="3">
                    <asp:dropdownlist ID="ddMyQueries" runat="server" AppendDataBoundItems="True" DataSourceID="SavedQueriesSqlDataSource" DataTextField="QueryName" DataValueField="QueryCommand" Width="100%">
                        <asp:ListItem></asp:ListItem></asp:dropdownlist>
                    <asp:TextBox ID="txtQueryName" runat="server" Width="100%"></asp:TextBox>
                </asp:tableCell>
            </asp:tableRow>
            <asp:TableRow>
                <asp:TableCell Font-Size=".75em">Year:</asp:TableCell>
                <asp:TableCell>
                    <asp:DropDownList ID="ddStartYear" runat="server" AppendDataBoundItems="true" Width="100%">
                    <asp:ListItem Value="0">Start Year</asp:ListItem>
                    <asp:ListItem>2019</asp:ListItem>
                    <asp:ListItem>2018</asp:ListItem>
                    <asp:ListItem>2017</asp:ListItem>
                               </asp:DropDownList></asp:TableCell>
                <asp:TableCell Font-Size=".75em" HorizontalAlign="Right">To</asp:TableCell>
                <asp:TableCell><asp:DropDownList ID="ddEndYear" runat="server" AppendDataBoundItems="true" Width="100%">
                    <asp:ListItem Value="0">End Year</asp:ListItem>
                    <asp:ListItem>2019</asp:ListItem>
                    <asp:ListItem>2018</asp:ListItem>
                    <asp:ListItem>2017</asp:ListItem>
                               </asp:DropDownList></asp:TableCell>
            </asp:tableRow>
            <asp:TableRow>
                <asp:TableCell Font-Size=".75em">Month:</asp:TableCell>
                <asp:TableCell>
                    <asp:DropDownList ID="ddStartMonth" runat="server" Width="100%">
                    <asp:ListItem Value="0">Start Month</asp:ListItem>
                    <asp:ListItem Value="1">January</asp:ListItem>
                    <asp:ListItem Value="2">February</asp:ListItem>
                    <asp:ListItem Value="3">March</asp:ListItem>
                    <asp:ListItem Value="4">April</asp:ListItem>
                    <asp:ListItem Value="5">May</asp:ListItem>
                    <asp:ListItem Value="6">June</asp:ListItem>
                    <asp:ListItem Value="7">July</asp:ListItem>
                    <asp:ListItem Value="8">August</asp:ListItem>
                    <asp:ListItem Value="9">September</asp:ListItem>
                    <asp:ListItem Value="10">October</asp:ListItem>
                    <asp:ListItem Value="11">November</asp:ListItem>
                    <asp:ListItem Value="12">December</asp:ListItem></asp:DropDownList></asp:TableCell>
                <asp:TableCell Font-Size=".75em" HorizontalAlign="Right">To</asp:TableCell>
                <asp:TableCell><asp:DropDownList ID="ddEndMonth" runat="server" Width="100%">
                    <asp:ListItem Value="0">End Month</asp:ListItem>
                    <asp:ListItem Value="1">January</asp:ListItem>
                    <asp:ListItem Value="2">February</asp:ListItem>
                    <asp:ListItem Value="3">March</asp:ListItem>
                    <asp:ListItem Value="4">April</asp:ListItem>
                    <asp:ListItem Value="5">May</asp:ListItem>
                    <asp:ListItem Value="6">June</asp:ListItem>
                    <asp:ListItem Value="7">July</asp:ListItem>
                    <asp:ListItem Value="8">August</asp:ListItem>
                    <asp:ListItem Value="9">September</asp:ListItem>
                    <asp:ListItem Value="10">October</asp:ListItem>
                    <asp:ListItem Value="11">November</asp:ListItem>
                    <asp:ListItem Value="12">December</asp:ListItem></asp:DropDownList></asp:TableCell>
            </asp:tableRow>
        </asp:table></div>
                <div style="margin-top:0;margin-left:25px;float:left;text-align:left">
        <asp:Table ID="tblNewQuery" runat="server" Width="100%" CellPadding="0">
            <asp:TableRow><asp:TableCell ColumnSpan="5"></asp:TableCell></asp:TableRow>
            <asp:TableRow>
                <asp:TableCell Font-Size=".75em">Data Table:</asp:TableCell>
                <asp:TableCell ColumnSpan="4" Width="400px">
                <asp:DropDownList ID="ddDataTable" runat="server" AppendDataBoundItems="true" DataSourceID="DataTableSqlDataSource" DataTextField="TableName" DataValueField="TableID" Width="100%">
                    <asp:ListItem></asp:ListItem>
                </asp:DropDownList></asp:TableCell>
            </asp:tableRow>
            <asp:TableRow>
                <asp:TableCell Width="100px" Font-Size=".75em"><asp:LinkButton ID="lnkBtnTextFields" runat="server" OnClick="lnkBtnTextFields_Click">Text Fields:</asp:LinkButton></asp:TableCell>
                <asp:TableCell><asp:ListBox ID="lbSelectedGroupFields" runat="server" SelectionMode="Multiple" Width="100%">
                               </asp:ListBox></asp:TableCell>
                <asp:TableCell Width="150px" HorizontalAlign="Center">
                    <asp:LinkButton ID="lnkBtnParameters" runat="server" OnClick="lnkBtnParameters_Click" Font-Size=".75em">Parameter Criteria:</asp:LinkButton></asp:TableCell>
                <asp:TableCell Width="100px" HorizontalAlign="Right"><asp:Label ID="Label3" runat="server" Text="" Font-Size=".75em"></asp:Label></asp:TableCell>
                <asp:TableCell><asp:DropDownList ID="ddColumnField" runat="server" Width="100%"></asp:DropDownList></asp:TableCell>
            </asp:tableRow>
            <asp:TableRow>
                <asp:TableCell><asp:LinkButton ID="lnkBtnValueFields" runat="server" OnClick="lnkBtnValueFields_Click" Font-Size=".75em">Value Fields:</asp:LinkButton></asp:TableCell>
                <asp:TableCell><asp:ListBox ID="lbSelectedValueFields" runat="server" SelectionMode="Multiple" Width="100%">
                               </asp:ListBox></asp:TableCell>
                <asp:TableCell></asp:TableCell>
                <asp:TableCell HorizontalAlign="Right"><asp:Label ID="Label4" runat="server" Text="" Font-Size=".75em"></asp:Label></asp:TableCell>
                <asp:TableCell><asp:DropDownList ID="ddColumnValue" runat="server" Width="100%"></asp:DropDownList></asp:TableCell>
            </asp:tableRow></asp:Table></div></td>
            <th style="font-size:1.6em;color:#10331F;text-align:right;vertical-align:top">Build your own Query</th>
        </tr>
        <tr><td></td></tr>
        <tr><td></td></tr>
        <tr><td colspan="3">
                <asp:Label ID="lblInstructions" runat="server" Text="" Font-Size=".75em"></asp:Label>&nbsp;
                <asp:Button ID="btnCmd" runat="server" Text="" OnClick="btnCmd_Click" />&nbsp;
                <asp:Button ID="btnProcess" runat="server" Text="" OnClick="btnProcess_Click" /></td></tr>
    </table>
        <div style="position:fixed;width:850px;margin-left:550px;margin-top:10px;">
        <asp:Table ID="tblSelectQueryOption" runat="server" Font-Size="Smaller">
            <asp:TableHeaderRow><asp:TableHeaderCell><asp:Label ID="Label5" runat="server" Text=""></asp:Label></asp:TableHeaderCell></asp:TableHeaderRow>
            <asp:TableRow>
                <asp:TableCell>
                    <asp:RadioButton ID="RadioButtonSavedQuery" runat="server" />
                    <asp:RadioButton ID="RadioButtonListQuery" runat="server" />
                    <asp:Label ID="Label1" runat="server" Text=""></asp:Label></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:tableCell>
                    <asp:RadioButton ID="RadioButtonNewQuery" runat="server" />
                    <asp:RadioButton ID="RadioButtonPivotQuery" runat="server" />
                    <asp:Label ID="Label2" runat="server" Text=""></asp:Label></asp:tableCell></asp:TableRow>
            <asp:TableRow>
                <asp:TableCell>
                    <asp:Button ID="btnQuerySelection" runat="server" Text="Continue" OnClick="btnQuerySelection_Click" /></asp:TableCell>
            </asp:TableRow>
        </asp:Table>
            <asp:Table ID="tblFieldSelection" runat="server" Width="100%" BackColor="Gainsboro">
                <asp:TableRow>
                    <asp:TableCell></asp:TableCell>
                    <asp:TableCell Width="300px"><asp:DropDownList ID="ddTableFields" runat="server" OnSelectedIndexChanged="ddTableFields_SelectedIndexChanged" AutoPostBack="true" Width="300" Font-Size="Large"></asp:DropDownList><br />
                        <asp:ListBox ID="lbDataTableFields" runat="server" SelectionMode="Multiple" Width="300" Font-Size="Large"></asp:ListBox></asp:TableCell>
                    <asp:TableCell Width="40px">
                            <asp:ImageButton ID="imgBtnUpArrow" runat="server" ImageUrl="~/asp/img/thumb_up-arrow-png-19.png" OnClick="imgBtnUpArrow_Click" /><br />
                            <asp:ImageButton ID="imgBtnRightArrow" runat="server" ImageUrl="~/asp/img/thumb_right-down-png-19.png" OnClick="imgBtnRightArrow_Click" /><br />
                            <asp:ImageButton ID="imgBtnLeftArrow" runat="server" ImageUrl="~/asp/img/thumb_left-down-png-19 .png" OnClick="imgBtnLeftArrow_Click" /><br />
                            <asp:ImageButton ID="imgBtnDownArrow" runat="server" ImageUrl="~/asp/img/thumb_down-arrow-png-19.png" OnClick="imgBtnDownArrow_Click" /></asp:TableCell>
                    <asp:TableCell Width="300px"><br /><asp:ListBox ID="lbSelectedTableFields" runat="server" SelectionMode="Multiple" Width="300" Font-Size="Large"></asp:ListBox></asp:TableCell>
                    <asp:TableCell></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell></asp:TableCell>
                    <asp:TableCell ColumnSpan="3"><asp:Button ID="btnAddPar" runat="server" Text="" OnClick="btnAddPar_Click" Font-Size="Large" />&nbsp;
                            <asp:Button ID="btnContinue" runat="server" Text="Continue" OnClick="btnContinue_Click" Font-Size="Large"  /></asp:TableCell>
                    <asp:TableCell></asp:TableCell>
                </asp:TableRow>
            </asp:Table></div>
        <table style="width:100%;z-index:-4">
        <tr style="background-color:ghostwhite">
            <td colspan="3"><asp:ImageButton ID="imgExcel" runat="server" ImageAlign="Right" ImageUrl="../img/excel_thumb.jpg" OnClick="imgExcel_Click" /></td>
        </tr>
            <tr style="text-align:justify">
                <td style="width:15%"></td>
                <td><asp:GridView ID="GridView1" runat="server" Width="100%"></asp:GridView></td>
                <td style="width:15%"></td>
            </tr>
        <tr>
            <td colspan="3"><asp:Label ID="lblUserID" runat="server" Text=""></asp:Label>&nbsp;<asp:Label ID="lblTextFields" runat="server" Text=""></asp:Label></td>
        </tr>
        <tr>
            <td colspan="3"><asp:Literal ID="queryCmd" runat="server"></asp:Literal></td>
        </tr>
            <tr style="background-color:ghostwhite">
                <td colspan="3"><asp:Label ID="lblParameterValues" runat="server" Text="" Font-Size=".65em"></asp:Label></td>
            </tr>
    </table></ContentTemplate>
        </asp:UpdatePanel>
        <asp:SqlDataSource ID="SavedQueriesSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:Pomps_ReportsConnectionString %>" SelectCommand="SELECT [QueryName], [QueryCommand] FROM [Database_User_Query] WHERE ([UserID] = @UserID)">
                    <SelectParameters><asp:ControlParameter ControlID="lblUserID" Name="UserID" PropertyName="Text" Type="Int32" /></SelectParameters></asp:SqlDataSource>
       <asp:SqlDataSource ID="DataTableSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:Pomps_ReportsConnectionString %>" SelectCommand="SELECT [QueryTableID], RTRIM([TableID]) AS TableID, [TableName] FROM [QueryTables] WHERE TableActive = 1"></asp:SqlDataSource></div>
</asp:Content>



