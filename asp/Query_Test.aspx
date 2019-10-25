<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" EnableEventValidation="false" AutoEventWireup="true" CodeFile="Query_Test.aspx.cs" Inherits="asp_Query" %>

<asp:Content ID="Content5" ContentPlaceHolderID="cpMainContent" Runat="Server">
    <div id="MainContent">
        <div id="Login">
    <asp:table ID="tblQuery" runat="server" Font-Size ="10px" Font-Names="Euphemia">
        <asp:TableRow>
            <asp:TableCell ColumnSpan="5">
                <asp:Label ID="lblError" runat="server" Text="">
                    <asp:Label ID="lblTxtFields" runat="server" Text=""></asp:Label>
                </asp:Label></asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>
                <asp:DropDownList ID="ddQueryOption" runat="server" Font-Size ="10px" Font-Names="Euphemia">
                    <asp:ListItem Value="det_qry">Query - Detail</asp:ListItem>
                    <asp:ListItem Value="sum_qry">Query - Summary</asp:ListItem>
                    <asp:ListItem Value="mat_qry">Query - Matrix</asp:ListItem>
                </asp:DropDownList></asp:TableCell>
            <asp:TableCell ColumnSpan="4">
                <asp:Label ID="lblQuery" runat="server" Text=""></asp:Label></asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell ColumnSpan="5">
                <asp:Label ID="lblQueryFilters" runat="server" Text=""></asp:Label></asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell ColumnSpan="5">
                <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label></asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell ColumnSpan="5">
                <asp:DropDownList ID="ddDataTable" runat="server" DataSourceID="tablesListSqlDataSource" DataTextField="tbl_Name" DataValueField="sql_tbl" AppendDataBoundItems="True"  Font-Size ="10px" Font-Names="Euphemia">
                    <asp:ListItem Value="">Select your table</asp:ListItem>
                </asp:DropDownList>
                <asp:ListBox ID="lbQueryFields" runat="server" SelectionMode="Multiple" Width="150px" Font-Size ="10px" Font-Names="Euphemia"></asp:ListBox>
                <asp:DropDownList ID="ddAddQueryFilters" runat="server" Font-Size ="10px" Font-Names="Euphemia">
                    <asp:ListItem Value="And">And</asp:ListItem>
                    <asp:ListItem Value="Or">Or</asp:ListItem>
                </asp:DropDownList>
                <asp:DropDownList ID="ddQueryFilters" runat="server" DataSourceID="dataTableFieldsSqlDataSource" DataTextField="field_Name" DataValueField="field_Name" AppendDataBoundItems="true" AutoPostBack="true" OnTextChanged="ddQueryFilters_TextChanged" Font-Size ="10px" Font-Names="Euphemia">
                    <asp:ListItem Value=""></asp:ListItem>
                </asp:DropDownList>
    <asp:DropDownList ID="ddQueryFiltersFactor" runat="server" Font-Names="Euphemia" AutoPostBack="True" OnTextChanged="ddQueryFiltersFactor_TextChanged" Font-Size ="10px">
        <asp:ListItem Value=""></asp:ListItem>
        <asp:ListItem Value="in">Equals</asp:ListItem>
        <asp:ListItem Value="not in">Not Equals</asp:ListItem>
        <asp:ListItem Value="<">Less Than</asp:ListItem>
        <asp:ListItem Value=">">Greater Than</asp:ListItem>
        <asp:ListItem Value="Like">Contains</asp:ListItem>
        <asp:ListItem Value="Like">Starts with</asp:ListItem>
        <asp:ListItem Value="Like">Ends with</asp:ListItem>
    </asp:DropDownList><asp:TextBox ID="txtQueryFiltersValue" runat="server"></asp:TextBox><br />
                <asp:ListBox ID="lbQueryFiltersValue" runat="server" SelectionMode="Multiple" Width="200px" Font-Size ="10px" Font-Names="Euphemia"></asp:ListBox><br />
                <asp:Button ID="btnAddQueryFilter" runat="server" Text="Add Filter" OnClick="btnAddQueryFilter_Click"  Font-Size ="10px" Font-Names="Euphemia"/>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell ColumnSpan="5">
                <asp:Button ID="btnGenerateQuery" runat="server" Text="" OnClick="btnGenerateQuery_Click"  Font-Size ="10px" Font-Names="Euphemia"/>&nbsp;
                <asp:Button ID="btnCancelQuery" runat="server" Text="Cancel" OnClick="btnCancelQuery_Click"  Font-Size ="10px" Font-Names="Euphemia"/>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell ColumnSpan="5">
                <asp:GridView ID="GridView1" runat="server" Font-Size ="10px" Font-Names="Euphemia"></asp:GridView>
            </asp:TableCell>
        </asp:TableRow>
    </asp:table>        
    <asp:SqlDataSource ID="dataTableFieldsSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:SQLMaddenCoConnectionString %>" SelectCommand="select rtrim(tbl_Field_Name) as field_Name from PompsTire_ERP.dbo.systemTableFields as tf inner join PompsTire_ERP.dbo.systemTables as t on tf.tbl_ID = t.tblID"></asp:SqlDataSource>
    <asp:SqlDataSource ID="tablesListSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:SQLMaddenCoConnectionString %>" SelectCommand="select rtrim(tbl_SQL_Name) as sql_tbl
,rtrim(tbl_Name) as tbl_Name
from PompsTire_ERP.dbo.systemTables
order by tbl_Name"></asp:SqlDataSource></div>
        <asp:Table ID="Table1" Width="100px" runat="server">
            <asp:TableRow>
                <asp:TableCell Width="25%"></asp:TableCell>
                <asp:TableCell Width="65%">
                    <asp:Image ID="Image1" ImageUrl="~/asp/img/Under Construction.png" Width="650px" Height="550px" runat="server" /></asp:TableCell>
                <asp:TableCell Width="10%"></asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        <asp:Label ID="lblUserID" runat="server" Text=""></asp:Label>
    </div>
</asp:Content>

