<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Reports.aspx.cs" Inherits="asp_Reports" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>


<asp:Content ID="Content5" ContentPlaceHolderID="cpMainContent" Runat="Server">
    <div id="MainContent">
        <table>
            <tr>
                <th style="width:15%;text-align:left">
        <asp:Label ID="lblTable" runat="server" Text="" Font-Size="2.1em"></asp:Label></th>
                <th style="width:70%"></th>
                <th style="width:15%"></th>
            </tr>
            <tr>
                <td style="width:15%">
                </td>
                <td style="max-width:850px;margin:auto">
                        <rsweb:ReportViewer ID="ReportViewer1" runat="server" SizeToReportContent="True" ShowParameterPrompts="false" ShowPrintButton="True"></rsweb:ReportViewer></td>
                <td style="width:15%"></td>
            </tr>
        </table>
        <div id="Login">
            <asp:Table ID="Table1" runat="server">
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Table ID="tblFinancials" Width="1050px" runat="server">
                <asp:TableRow>
                    <asp:TableCell Width="165px">REPORT<br />
                        <asp:DropDownList ID="ddFinancials" Font-Size="14px" Width="150px" runat="server" AutoPostBack="True" OnTextChanged="ddFinancials_TextChanged" DataSourceID="FinancialReportsSqlDataSource" DataTextField="RptName" DataValueField="RptID">
                        </asp:DropDownList></asp:TableCell>
                    <asp:TableCell Width="165px">&nbsp;</asp:TableCell>
                    <asp:TableCell Width="165px">&nbsp;</asp:TableCell>
                    <asp:TableCell>&nbsp;</asp:TableCell>
                    <asp:TableCell>&nbsp;</asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>PERIOD<br /><asp:DropDownList ID="ddFinancialPeriod" Width="150px" Font-Size="14px"  runat="server" DataSourceID="FinancialPeriodSqlDataSource" DataTextField="Period" DataValueField="Period"></asp:DropDownList></asp:TableCell>
                    <asp:TableCell>REGION<br />
                        <asp:DropDownList ID="ddRegion" Font-Size="14px" Width="150px" runat="server" DataSourceID="RegionSqlDataSource" DataTextField="RegionName" DataValueField="RegionNo" AutoPostBack="True">
                        </asp:DropDownList></asp:TableCell>
                    <asp:TableCell>STORE<br />
            <asp:DropDownList ID="ddStore" Font-Size="14px"  runat="server" DataSourceID="StoreSqlDataSource" DataTextField="StoreName" DataValueField="StoreNo">
            </asp:DropDownList></asp:TableCell>
                    <asp:TableCell>REPORT FORMAT<br />
                                    <asp:DropDownList ID="ddReportType" Width="150px" Font-Size="14px"  runat="server">
                                        <asp:ListItem></asp:ListItem>
                                        <asp:ListItem>CONSOLIDATED</asp:ListItem>
                                        <asp:ListItem>STORES LIST</asp:ListItem>
                                    </asp:DropDownList></asp:TableCell>
                    <asp:TableCell>&nbsp;</asp:TableCell>
                </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell>&nbsp;</asp:TableCell>
                                <asp:TableCell>&nbsp;</asp:TableCell>
                                <asp:TableCell>&nbsp;</asp:TableCell>
                                <asp:TableCell>&nbsp;</asp:TableCell>
                                <asp:TableCell>&nbsp;</asp:TableCell>
                            </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell ColumnSpan="5">
                        <asp:Image ID="Image3" ImageUrl="~/asp/img/Under Construction.png" Width="550px" Height="450px"  runat="server" Visible="false" /></asp:TableCell>
                </asp:TableRow>
            </asp:Table></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell ColumnSpan="5">
                        <asp:Table ID="tblSales" runat="server">
                            <asp:TableRow>
                                <asp:TableCell>
                                    REPORT NAME:<br /><asp:DropDownList ID="ddSalesReport" runat="server" AppendDataBoundItems="true" OnSelectedIndexChanged="ddSalesReport_SelectedIndexChanged" DataSourceID="UserReportsSqlDataSource" DataTextField="RptName" DataValueField="RptID">
                                </asp:DropDownList></asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell VerticalAlign="Top">
                                    YEAR:<br />
                                    <asp:DropDownList ID="ddSalesYear" runat="server" AppendDataBoundItems="True">
                                        <asp:ListItem Value="0">ANY YEAR</asp:ListItem>
                                    </asp:DropDownList></asp:TableCell>
                                </asp:TableRow><asp:TableRow>
                                <asp:TableCell>
                                    <asp:CheckBoxList ID="cblSalesPeriod" runat="server"></asp:CheckBoxList>
                                    PERIOD:<br />
                                    <asp:ListBox ID="lbSalesPeriod" runat="server" AppendDataBoundItems="True"  SelectionMode="Multiple">
                                        <asp:ListItem Value="0">ALL PERIODS</asp:ListItem></asp:ListBox></asp:TableCell>
                                <asp:TableCell>
                                        REGION<br />
                                    <asp:ListBox ID="lbSalesRegion" runat="server" AppendDataBoundItems="False" SelectionMode="Multiple">
                                    </asp:ListBox></asp:TableCell>
                                <asp:TableCell>
                                    STORE:<br />
                                    <asp:ListBox ID="lbSalesStore" runat="server" AppendDataBoundItems="False" SelectionMode="Multiple">
                                    </asp:ListBox></asp:TableCell>
                                <asp:TableCell>
                                    SALES ASSOCIATE:<br />
                                    <asp:ListBox ID="lbSalesman" runat="server" AppendDataBoundItems="True" SelectionMode="Multiple">
                                        <asp:ListItem Value="0">ALL SALESMAN NUMBERS</asp:ListItem>
                                    </asp:ListBox></asp:TableCell></asp:TableRow>
                            <asp:TableRow>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell>
                                    CUSTOMER CLASS:<br />
                                    <asp:ListBox ID="lbCustCls" runat="server" AppendDataBoundItems="True" SelectionMode="Multiple">
                                        <asp:ListItem Value="A">ALL CUST CLS</asp:ListItem>
                                    </asp:ListBox></asp:TableCell>
                                <asp:TableCell>
                                    PRODUCT CLASS:<br />
                                    <asp:ListBox ID="lbPrdCls" runat="server" AppendDataBoundItems="True" SelectionMode="Multiple">
                                        <asp:ListItem Value="AA">ALL PRD CLS</asp:ListItem>
                                    </asp:ListBox></asp:TableCell>
                                <asp:TableCell>
                                    PRODUCT VENDOR:<br />
                                    <asp:ListBox ID="lbPrdVnd" runat="server" AppendDataBoundItems="True" SelectionMode="Multiple">
                                        <asp:ListItem Value="All">ALL PRD VND</asp:ListItem>
                                    </asp:ListBox></asp:TableCell></asp:TableRow><asp:TableRow></asp:TableRow><asp:TableRow>
                                <asp:TableCell>
                                    PRODUCT NUMBER:<br />
                                    <asp:TextBox ID="txtPrdNo" runat="server"></asp:TextBox></asp:TableCell>
                            </asp:TableRow>
                        </asp:Table></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell ColumnSpan="5">
        <asp:Image ID="Image1" ImageUrl="~/asp/img/Under Construction.png" Width="550px" Height="450px" runat="server" Visible="false"/>
                    </asp:TableCell></asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Table ID="tblInventory" runat="server">
                <asp:TableRow>
                    <asp:TableCell>&nbsp;</asp:TableCell>
                    <asp:TableCell>&nbsp;</asp:TableCell>
                    <asp:TableCell>
                        <asp:Image ID="Image2" ImageUrl="~/asp/img/Under Construction.png" Width="550px" Height="450px"  runat="server" /></asp:TableCell>
                    <asp:TableCell>&nbsp;</asp:TableCell>
                    <asp:TableCell>&nbsp;</asp:TableCell>
                </asp:TableRow></asp:Table>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell ColumnSpan="5">
                        <asp:Button ID="btnPreview" runat="server" Text="" OnClick="btnPreview_Click" /></asp:TableCell>
                </asp:TableRow>
            </asp:Table></div>
        <asp:Label ID="lblUsername" runat="server" Text=""></asp:Label>
        <asp:Label ID="lblUserID" runat="server" Text=""></asp:Label>
        <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
         
            <asp:SqlDataSource ID="StoreSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:Pomps_ReportsConnectionString %>" SelectCommand="select Database_User_Store.StoreNo,convert(varchar(3),Database_User_Store.StoreNo) + ' - ' + Rtrim(StoreName) as StoreName
from Pomps_Reports.dbo.Database_User_Store inner join Pomps_Reports.dbo.Store on Database_User_Store.StoreID = Store.StoreID left outer join Pomps_Reports.dbo.Region on Store.StoreRegionID = Region.RegionID
where UserID = @UserID and StoreName &lt;&gt; 'CLOSED' and Database_User_Store.StoreNo &lt; 600 and (RegionNo = @RegionNo or IsNull(@RegionNo,'99')='99') and UserStoreActive = 1
order by Database_User_Store.StoreNo">
                <SelectParameters>
                    <asp:ControlParameter ControlID="lblUserID" Name="UserID" PropertyName="Text" />
                    <asp:ControlParameter ControlID="ddRegion" Name="RegionNo" PropertyName="Text" />
                </SelectParameters>
            </asp:SqlDataSource>
            <asp:SqlDataSource ID="RegionSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:Pomps_ReportsConnectionString %>" SelectCommand="select  Database_User_Region.RegionNo,case when Database_User_Region.RegionNo = 99 then -1 else  Database_User_Region.RegionNo end RegionNumber,case when Database_User_Region.RegionNo = 99 then RegionName else convert(varchar(2),Database_User_Region.RegionNo) + ' - ' + rtrim(RegionName) end as RegionName
from Pomps_Reports.dbo.Database_User_Region left outer join Pomps_Reports.dbo.Region on Database_User_Region.RegionID = Region.RegionID
where UserID = @UserID and UserRegionActive = 1
order by RegionNumber">
                <SelectParameters>
                    <asp:ControlParameter ControlID="lblUserID" Name="UserID" PropertyName="Text" />
                </SelectParameters>
            </asp:SqlDataSource>  
        <asp:SqlDataSource ID="UserReportsSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:Pomps_ReportsConnectionString %>" SelectCommand="select 'zzz' as RptID, '' as RptName union all select rtrim(RptID) as RptID
,rtrim(RptName) as RptName
from dbo.Database_User_Report 
where UserRptActive = 1 and rtrim(RptType) = 'Sales' and UserID = @UserID order by RptID">
            <SelectParameters>
                <asp:ControlParameter ControlID="lblUserID" Name="UserID" PropertyName="Text" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="FinancialPeriodSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:Pomps_ReportsConnectionString %>" SelectCommand="select Period from Pomps_Reports.dbo.Period where Period_Report = 'Financials' order by Period desc"></asp:SqlDataSource>
                        <asp:SqlDataSource ID="FinancialReportsSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:Pomps_ReportsConnectionString %>" SelectCommand="select rtrim(RptID) as RptID,rtrim(RptName) as RptName
from Pomps_Reports.dbo.Database_User_Report
where UserID = @UserID and UserRptActive = 1 and RptType = 'Financials'">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="lblUserID" Name="UserID" PropertyName="Text" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                              
    </div>
</asp:Content>

