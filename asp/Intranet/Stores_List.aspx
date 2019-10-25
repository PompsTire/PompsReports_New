<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Stores_List.aspx.cs" Inherits="asp_Intranet_Default" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1 {
            height: 60px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
           <div>
    <table style="width:100%;font-size:12pt;font-family:Euphemia;border:5px solid gray">
        <tr style="background-color:#CCCCCC;">
            <td colspan="6">Corporate Headquarters<br />1123 Cedar Street, Green Bay, WI 54301 - PO Box 1630, Green Bay, WI 54305</td>
            <td>&nbsp;
                <asp:ImageButton ID="imgBtnPdf" runat="server" ImageUrl="~/asp/img/thumb_pdf-icon.png" OnClick="btnPrint_Click" />&nbsp;&nbsp;
                <asp:ImageButton ID="ImgBtnExcel" runat="server" ImageUrl="~/asp/img/thumb_excel logo.png" OnClick="ImgBtnExcel_Click" />
            </td>
        </tr>
        <tr style="border-bottom:1px solid gray">
            <td>Local Number</td>
            <td>920-435-8301</td>
            <td></td>
            <td>Credit Dept.</td>
            <td></td>
            <td colspan="2">800-536-2940</td>
        </tr>
        <tr style="border-bottom:1px solid gray">
            <td>Toll Free</td>
            <td>800-236-8911</td>
            <td></td>
            <td>Credit Dept. Fax</td>
            <td></td>
            <td colspan="2">920-431-7615</td>
        </tr>
        <tr style="border-bottom:1px solid gray">
            <td>Office Fax 1 (General Office)</td>
            <td>920-431-7615</td>
            <td></td>
            <td>Wholesale</td>
            <td></td>
            <td colspan="2">800-236-8585</td>
        </tr>
        <tr style="border-bottom:1px solid gray">
            <td>Office Fax 2 (Corporate)</td>
            <td>920-431-7614</td>
            <td></td>
            <td>Wholesale Fax</td>
            <td></td>
            <td colspan="2">920-431-7620</td>
        </tr>
        <tr style="border-bottom:1px solid gray">
            <td>Computer Dept.</td>
            <td>800-207-8413</td>
            <td></td>
            <td>Warehouse</td>
            <td></td>
            <td colspan="2">800-584-5070</td>
        </tr>
        <tr style="border-bottom:1px solid gray">
            <td>Retail - Bandag - IT Dept. Fax</td>
            <td>920-431-7622</td>
            <td></td>
            <td>Warehouse Fax</td>
            <td></td>
            <td colspan="2">920-431-7618</td>
        </tr>
        <tr><td colspan="6">
            <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label></td></tr>
        <tr style="font-weight:bold;border-top:5px solid gray;border-bottom:5px solid gray;background-color:#CCCCCC">
            <td colspan="7" class="auto-style1">Region:
                <asp:DropDownList ID="ddRegion" runat="server" AutoPostBack="True" DataSourceID="RegionSqlDataSource" DataTextField="Region" DataValueField="RegionDivisionNo">
                </asp:DropDownList>
                &nbsp;
                Division:<asp:DropDownList ID="ddDivision" runat="server" DataSourceID="DivisionSqlDataSource" DataTextField="StrDivDesc" DataValueField="StrDivNum">
                </asp:DropDownList>
                &nbsp;
                <asp:Button ID="btnPreview" runat="server" Text="Preview" Font-Bold="true" Font-Names="Euphemia" Font-Size="12pt" Width="75px" OnClick="btnPreview_Click" />
            </td>
        </tr>
        <tr>
            <td colspan="7">
                
                <asp:GridView ID="gvStoresList" runat="server" AutoGenerateColumns="False" DataSourceID="StoresSqlDataSource" Width="100%" AllowSorting="True" CellPadding="4" ForeColor="#333333" GridLines="None" Height="269px">
                    <AlternatingRowStyle BackColor="White" />
                    <Columns>
                        <asp:TemplateField HeaderText="SERVICE LOCATION" SortExpression="STORE">
                            <ItemTemplate>
                                <%#Eval("STORE") %><br /><%#Eval("ADDRESS") %><br /><%#Eval("CITY") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField= "STORE" HeaderText="STORE" ReadOnly="True" SortExpression="STORE" Visible="False" />
                        <asp:BoundField DataField="ADDRESS" HeaderText="ADDRESS" SortExpression="ADDRESS" Visible="False" />
                        <asp:BoundField DataField="CITY" HeaderText="CITY" ReadOnly="True" SortExpression="CITY" Visible="False" />
                        <asp:BoundField DataField="SPEED_DIAL" HeaderText="SPEED_DIAL" ReadOnly="True" SortExpression="SPEED_DIAL" ItemStyle-HorizontalAlign="Center" >
<ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="PHONE" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <%#Eval("MANAGER_PHONE") %><br /><br /><%#Eval("STORE_PHONE") %>
                            </ItemTemplate>
                            <HeaderStyle Width="200px" />

<ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:TemplateField>
                        <asp:BoundField DataField="MANAGER_PHONE" HeaderText="MANAGER_PHONE" ReadOnly="True" SortExpression="MANAGER_PHONE" Visible="False" />
                        <asp:BoundField DataField="STORE_PHONE" HeaderText="STORE_PHONE" SortExpression="STORE_PHONE" ReadOnly="True" Visible="False" />
                        <asp:BoundField DataField="FAX" HeaderText="FAX" SortExpression="FAX" ReadOnly="True" />
                        <asp:BoundField DataField="STR" HeaderText="STR" SortExpression="STR" ReadOnly="True" ItemStyle-HorizontalAlign="Center">
                        <HeaderStyle Width="50px" />

<ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="CODE" HeaderText="CODE" SortExpression="CODE" >
                        <HeaderStyle Width="50px" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="MANAGER" SortExpression="MANAGER" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <%#Eval("MANAGER") %><br />
                                <a href="mailto:<%#Eval("EMAIL") %>"><%#Eval("EMAIL") %></a>
                            </ItemTemplate>

<ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:TemplateField>
                    </Columns>
                    <EditRowStyle BackColor="#7C6F57" />
                    <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                    <RowStyle BackColor="#E3EAEB" />
                    <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                    <SortedAscendingCellStyle BackColor="#F8FAFA" />
                    <SortedAscendingHeaderStyle BackColor="#246B61" />
                    <SortedDescendingCellStyle BackColor="#D4DFE1" />
                    <SortedDescendingHeaderStyle BackColor="#15524A" />
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td colspan="7">
            </td>
        </tr>
    </table>
                <asp:SqlDataSource ID="RegionSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:Pomps_ReportsConnectionString %>" SelectCommand="select  distinct RegionDivisionNo
, rtrim(RegionDivisionName)  as Region
from Pomps_Reports.dbo.Region
order by Region"></asp:SqlDataSource>
                <asp:SqlDataSource ID="DivisionSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:Pomps_ReportsConnectionString %>" SelectCommand="SELECT distinct RegionNo as StrDivNum,case when RegionNo = 99 then RTrim(RegionName) else convert(varchar(2),RegionNo) + ' ' + RTrim(RegionName) end as StrDivDesc,case when RegionNo = 99 then -1 else RegionNo end as RegionNo1 FROM [Region] WHERE ([RegionDivisionNo] = @RegionDivisionNo Or IsNull(@RegionDivisionNo,'99')='99') order by RegionNo1">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="ddRegion" Name="RegionDivisionNo" PropertyName="SelectedValue" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="StoresSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:Pomps_ReportsConnectionString %>" SelectCommand="SELECT RTRIM(SUBSTRING(StoreName,13,20)) AS STORE
, [StoreAddress] AS ADDRESS
, [StoreCity]  + ', ' + [StoreState] + '. ' + CONVERT(VARCHAR(10),StoreZip) AS CITY
,'1-' + CASE WHEN StoreNo &lt; 10 THEN '00' + CONVERT(VARCHAR(1),StoreNo) WHEN StoreNo &gt; 99 THEN CONVERT(VARCHAR(3),StoreNo) ELSE '0' + CONVERT(VARCHAR(2),StoreNo) END + '-600' AS SPEED_DIAL
,CONVERT(VARCHAR(3), LEFT([StorePhoneNo],3)) + '-' + SUBSTRING(CONVERT(VARCHAR(10),StorePhoneNo),4,3) + '-' + CONVERT(VARCHAR(4),RIGHT(StorePhoneNo,4)) AS MANAGER_PHONE
,CONVERT(VARCHAR(3), LEFT([Store800No],3)) + '-' + SUBSTRING(CONVERT(VARCHAR(10),Store800No),4,3) + '-' + CONVERT(VARCHAR(4),RIGHT(Store800No,4)) AS [STORE_PHONE]
,CONVERT(VARCHAR(3), LEFT([StoreFaxNo],3)) + '-' + SUBSTRING(CONVERT(VARCHAR(10),StoreFaxNo),4,3) + '-' + CONVERT(VARCHAR(4),RIGHT(StoreFaxNo,4)) AS FAX,CASE WHEN StoreNo &lt; 10 THEN '00' + CONVERT(VARCHAR(1),StoreNo) WHEN StoreNo &gt; 99 THEN CONVERT(VARCHAR(3),StoreNo) ELSE '0' + CONVERT(VARCHAR(2),StoreNo) END AS STR, [StoreCode] AS [CODE],[StoreManager] AS MANAGER,RTrim(StoreManagerEmail) AS [EMAIL] FROM Pomps_Reports.dbo.Store inner join Pomps_Reports.dbo.Region on StoreRegionID = RegionID where (RegionNo = @RegionNo OR ISNULL(@RegionNo,'99')='99') AND (RegionDivisionNo = @Region Or IsNull(@Region,'99')='99') AND StoreNo &lt;&gt; 0 and StoreNo &lt; 600 and StoreActive = 1 ORDER BY StoreName">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="ddDivision" Name="RegionNo" PropertyName="SelectedValue" />
                        <asp:ControlParameter ControlID="ddRegion" Name="Region" PropertyName="SelectedValue" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                <rsweb:ReportViewer ID="ReportViewer1" runat="server"></rsweb:ReportViewer>
    </div>
    </form>
</body>
</html>
