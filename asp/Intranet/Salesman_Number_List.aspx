<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Salesman_Number_List.aspx.cs" Inherits="asp_Intranet_Salesman_Number_List" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div style="width:100%;margin:auto">
            <table style="width:100%;border:2px solid gray;">
                <tr>
                    <td>Region<br />
                        <asp:DropDownList ID="ddRegion" runat="server" DataSourceID="RegionSqlDataSource" DataTextField="RegionName" DataValueField="RegionNo" AppendDataBoundItems="True" AutoPostBack="True" Width="250px" Height="25px">
                            <asp:ListItem Value="99">ALL</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>Store<br />
                        <asp:DropDownList ID="ddStore" runat="server" DataSourceID="StoreSqlDataSource" DataTextField="StoreName" DataValueField="StoreNo" AppendDataBoundItems="True" Height="25px">
                            <asp:ListItem Value="0">ALL</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>Pomp's Salesman No<br />
                        <asp:TextBox ID="txtSlsmNo" runat="server" Height="25px"></asp:TextBox>
                    </td>
                    <td>Pomp's Employee No<br />
                        <asp:TextBox ID="txtEmpNo" runat="server" Height="25px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">First (or part of) Name<br />
                        <asp:TextBox ID="txtName" runat="server" Width="951px" Height="25px"></asp:TextBox>&nbsp;&nbsp;
                        <asp:ImageButton ID="imgBtnSearch" ImageUrl="~/asp/img/magnifying-glass_thumbnail.png" OnClick="imgBtnSearch_Click" runat="server" ToolTip="Search" />
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label></td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" Width="100%" DataSourceID="SalesmanSqlDataSource" CellPadding="4" ForeColor="#333333" GridLines="None" AllowSorting="True">
                            <AlternatingRowStyle BackColor="White" />
                            <Columns>
                                <asp:BoundField DataField="TSLMNUM" HeaderText="Salesman No" SortExpression="TSLMNUM" ItemStyle-HorizontalAlign="Center" >
                                <HeaderStyle Font-Underline="True" HorizontalAlign="Center" />
<ItemStyle HorizontalAlign="Center" Font-Underline="False"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="TSLMNUMEMP" HeaderText="Employee No" SortExpression="TSLMNUMEMP" ItemStyle-HorizontalAlign="Center" >
                                <HeaderStyle Font-Underline="True" HorizontalAlign="Center" />
<ItemStyle HorizontalAlign="Center" Font-Underline="False"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="TSLMNAM" HeaderText="Name" SortExpression="TSLMNAM" ItemStyle-HorizontalAlign="Center" >
<ItemStyle HorizontalAlign="Center" Font-Underline="False"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="TSLMSTR" HeaderText="Store No" SortExpression="TSLMSTR" ItemStyle-HorizontalAlign="Center" >
<ItemStyle HorizontalAlign="Center" Font-Underline="False"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="StoreName" HeaderText="Store Name" SortExpression="StoreName" ReadOnly="True" ItemStyle-HorizontalAlign="Center" >
<ItemStyle HorizontalAlign="Center" Font-Underline="False"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="RegionNo" HeaderText="Region No" SortExpression="RegionNo" ItemStyle-HorizontalAlign="Center" >
<ItemStyle HorizontalAlign="Center" Font-Underline="False"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="RegionName" HeaderText="Region Name" ReadOnly="True" SortExpression="RegionName" ItemStyle-HorizontalAlign="Center" >
<ItemStyle HorizontalAlign="Center" Font-Underline="False"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="mail" HeaderText="E-mail Address" SortExpression="mail" ItemStyle-HorizontalAlign="Center" Visible="False" >
<ItemStyle HorizontalAlign="Center" Font-Underline="False"></ItemStyle>
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="E-mail Address" SortExpression="mail">
                                    <ItemTemplate>
                                        <a href="mailto:<%#Eval("mail")%>"><%#Eval("mail") %></a>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
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
            </table>
                        <asp:SqlDataSource ID="RegionSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:Pomps_ReportsConnectionString %>" SelectCommand="select RegionNo
,convert(varchar(1),RegionNo) + '-' + rtrim(RegionName) as RegionName
from dbo.Region
where RegionNo not in('0','99')"></asp:SqlDataSource>
                        <asp:SqlDataSource ID="StoreSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:Pomps_ReportsConnectionString %>" SelectCommand="select StoreNo
,convert(varchar(3),StoreNo) + '-' + rtrim(StoreName) as StoreName
from dbo.Store inner join dbo.Region on StoreRegionID = RegionID
where StoreNo &lt; 500 and StoreNo not in('0','1') and (RegionNo = @RegionNo or IsNull(@RegionNo,'99')='99') and StoreName not like('%CLOSED%')
order by StoreNo">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="ddRegion" Name="RegionNo" PropertyName="SelectedValue" />
                            </SelectParameters>
            </asp:SqlDataSource>
                        <asp:SqlDataSource ID="SalesmanSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:Pomps_ReportsConnectionString %>" SelectCommand="SELECT SalesmanNo as [TSLMNUM],SalesmanEmpNo as [TSLMNUMEMP],RTRIM(SalesmanName) as [TSLMNAM],StoreNo as  [TSLMSTR],rtrim(StoreName) as StoreName,RegionNo,rtrim(RegionName) as RegionName, [mail] FROM [Salesman]
left outer join OPENQUERY(ADSI,'SELECT mail,EmployeeID FROM ''LDAP://gbdc01.pomps.local/OU=PTS Users,DC=pomps,DC=local'' WHERE objectClass = ''User''') AS ad ON CONVERT(integer, ad.EmployeeID) = SalesmanEmpNo inner join [Store] on SalesmanStrID = StoreID inner join [Region] on StoreRegionID = RegionID
where (SalesmanNo not like '%1') and (SalesmanEmpNo &lt;&gt; '0') and (SalesmanActive = 1) and (RegionNo = @RegionNo or IsNull(@RegionNo,'99')='99') and ((StoreNo = @StoreNo or IsNull(@StoreNo,'0')='0') and StoreName not like('%CLOSED%')) and (SalesmanName like ('%' + @Name + '%')) and (SalesmanNo = @SalesmanNo or IsNull(@SalesmanNo,'')='') and (SalesmanEmpNo = @SalesmanEmpNo or IsNull(@SalesmanEmpNo,'')='')">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="ddRegion" Name="RegionNo" PropertyName="SelectedValue" />
                                <asp:ControlParameter ControlID="ddStore" Name="StoreNo" PropertyName="SelectedValue" />
                                <asp:ControlParameter ControlID="txtName" Name="Name" PropertyName="Text" />
                                <asp:ControlParameter ControlID="txtSlsmNo" Name="SalesmanNo" PropertyName="Text" />
                                <asp:ControlParameter ControlID="txtEmpNo" Name="SalesmanEmpNo" PropertyName="Text" />
                            </SelectParameters>
                        </asp:SqlDataSource>
        </div>
    </form>
</body>
</html>
