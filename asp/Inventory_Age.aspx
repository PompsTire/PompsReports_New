<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Inventory_Age.aspx.cs" Inherits="asp_Inventory_Age" %>

<asp:Content ID="Content5" ContentPlaceHolderID="cpMainContent" Runat="Server">
    <div id="MainContent">
        <div id="Login">
            <asp:Table ID="Table1" Width="1050px" runat="server">
                <asp:TableRow>
                    <asp:TableCell ColumnSpan="2">
                    <asp:Table ID="Table3" runat="server" Width="540px" Font-Size=".65em" Height="5px" Font-Bold="true">
                        <asp:TableHeaderRow>
                            <asp:TableHeaderCell ColumnSpan="6">INVENTORY COST BY AGE RANGE (YEARS)</asp:TableHeaderCell>
                        </asp:TableHeaderRow>
                        <asp:TableRow BackColor="#336666" ForeColor="White" Height="25px">
                            <asp:TableCell Width="100PX">MANUFACTURER</asp:TableCell>
                            <asp:TableCell Width="145px">CLASS</asp:TableCell>
                            <asp:TableCell>0 - 1</asp:TableCell>
                            <asp:TableCell>1 - 3</asp:TableCell>
                            <asp:TableCell>3 - 5</asp:TableCell>
                            <asp:TableCell>OVER 5</asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                    <asp:Panel ID="Panel2" runat="server" ScrollBars="Vertical" Width="550px" Height="300px" BorderStyle="Solid" BorderColor="#999999">
                        <asp:GridView ID="gvInventory" Font-Size="0.55em" runat="server" BackColor="White" BorderColor="#336666" BorderStyle="Double" BorderWidth="3px" CellPadding="4" GridLines="Horizontal" ShowHeader="False" AutoGenerateColumns="False" DataSourceID="InventoryAgingSqlDataSource">
                            <Columns>
                                <asp:HyperLinkField DataNavigateUrlFields="pdvendor,pdclass,UserID" DataNavigateUrlFormatString="Inventory_Age.aspx?pdvendor={0}&amp;pdclass={1}&amp;UserID={2}" DataTextField="pdmanufacturer" HeaderText="MANUFACTURER-CLASS" />
                                <asp:BoundField DataField="pdvendor" HeaderText="pdvendor" SortExpression="pdvendor" Visible="False" />
                                <asp:BoundField DataField="pdmanufacturer" HeaderText="MANUFACTURER" SortExpression="pdmanufacturer" Visible="False" />
                                <asp:BoundField DataField="pdclass" HeaderText="pdclass" SortExpression="pdclass" Visible="False" />
                                <asp:BoundField DataField="class" HeaderText="CLASS" SortExpression="class" />
                                <asp:BoundField DataField="0 - 1 years" DataFormatString="{0:c2}" HeaderText="0 - 1 years" ReadOnly="True" SortExpression="0 - 1 years" />
                                <asp:BoundField DataField="1 - 3 years" DataFormatString="{0:c2}" HeaderText="1 - 3 years" ReadOnly="True" SortExpression="1 - 3 years" />
                                <asp:BoundField DataField="3 - 5 years" DataFormatString="{0:c2}" HeaderText="3 - 5 years" ReadOnly="True" SortExpression="3 - 5 years" />
                                <asp:BoundField DataField="Over 5 years" DataFormatString="{0:c2}" HeaderText="Over 5 years" ReadOnly="True" SortExpression="Over 5 years" />
                            </Columns>
                            <FooterStyle BackColor="White" ForeColor="#333333" />
                            <HeaderStyle BackColor="#336666" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#336666" ForeColor="White" HorizontalAlign="Center" />
                            <RowStyle BackColor="White" ForeColor="#333333" />
                            <SelectedRowStyle BackColor="#339966" Font-Bold="True" ForeColor="White" />
                            <SortedAscendingCellStyle BackColor="#F7F7F7" />
                            <SortedAscendingHeaderStyle BackColor="#487575" />
                            <SortedDescendingCellStyle BackColor="#E5E5E5" />
                            <SortedDescendingHeaderStyle BackColor="#275353" />
        </asp:GridView>
                    </asp:Panel>
                    
                <asp:SqlDataSource ID="InventoryAgingSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:Pomps_ReportsConnectionString %>" SelectCommand="select *
from (select @UserID as UserID,pdvendor,pdmanufacturer,pdclass,class,coalesce(sum(pdinvmemo * pdactcost),0) as on_hand
,case when DateDiff(year,case when pdrecvdate = '00000000' then convert(date,'01/01/2001') else convert(date,convert(varchar(2),substring(pdrecvdate,2,2)) + '/' + convert(varchar(2),right(pdrecvdate,2)) + '/' + convert(varchar(4),left(pdrecvdate,4))) end,GetDate()) &gt; 5 then 'Over 5 years'
when DateDiff(year,case when pdrecvdate = '00000000' then convert(date,'01/01/2001') else convert(date,convert(varchar(2),substring(pdrecvdate,2,2)) + '/' + convert(varchar(2),right(pdrecvdate,2)) + '/' + convert(varchar(4),left(pdrecvdate,4))) end,GetDate()) between 3 and 4.99999 then '3 - 5 years'
when DateDiff(year,case when pdrecvdate = '00000000' then convert(date,'01/01/2001') else convert(date,convert(varchar(2),substring(pdrecvdate,2,2)) + '/' + convert(varchar(2),right(pdrecvdate,2)) + '/' + convert(varchar(4),left(pdrecvdate,4))) end,GetDate()) between 1 and 2.99999 then '1 - 3 years'
when DateDiff(year,case when pdrecvdate = '00000000' then convert(date,'01/01/2001') else convert(date,convert(varchar(2),substring(pdrecvdate,2,2)) + '/' + convert(varchar(2),right(pdrecvdate,2)) + '/' + convert(varchar(4),left(pdrecvdate,4))) end,GetDate()) &lt; 1 then '0 - 1 years' end as Age
from SQLMaddenco.dbo.Inventory inner join SQLMaddenco.dbo.Product on rtrim(Inventory.PDNUMBER) = rtrim(Product.pdnumber)
where convert(integer,PDSTORE) in(select StoreNo from Pomps_Reports.dbo.Database_User_Store where UserID = @UserID) and PDINVMEMO &lt;&gt; 0
group by pdvendor,pdmanufacturer,pdclass,class,case when DateDiff(year,case when pdrecvdate = '00000000' then convert(date,'01/01/2001') else convert(date,convert(varchar(2),substring(pdrecvdate,2,2)) + '/' + convert(varchar(2),right(pdrecvdate,2)) + '/' + convert(varchar(4),left(pdrecvdate,4))) end,GetDate()) &gt; 5 then 'Over 5 years'
when DateDiff(year,case when pdrecvdate = '00000000' then convert(date,'01/01/2001') else convert(date,convert(varchar(2),substring(pdrecvdate,2,2)) + '/' + convert(varchar(2),right(pdrecvdate,2)) + '/' + convert(varchar(4),left(pdrecvdate,4))) end,GetDate()) between 3 and 4.99999 then '3 - 5 years'
when DateDiff(year,case when pdrecvdate = '00000000' then convert(date,'01/01/2001') else convert(date,convert(varchar(2),substring(pdrecvdate,2,2)) + '/' + convert(varchar(2),right(pdrecvdate,2)) + '/' + convert(varchar(4),left(pdrecvdate,4))) end,GetDate()) between 1 and 2.99999 then '1 - 3 years'
when DateDiff(year,case when pdrecvdate = '00000000' then convert(date,'01/01/2001') else convert(date,convert(varchar(2),substring(pdrecvdate,2,2)) + '/' + convert(varchar(2),right(pdrecvdate,2)) + '/' + convert(varchar(4),left(pdrecvdate,4))) end,GetDate()) &lt; 1 then '0 - 1 years' end) m
pivot (sum(on_hand) for Age in([0 - 1 years],[1 - 3 years],[3 - 5 years],[Over 5 years])) pvt">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="lblUserAccess" Name="UserID" PropertyName="Text" />
                    </SelectParameters>
                </asp:SqlDataSource></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
            <asp:Label ID="lblInventoryAgeRange" runat="server" Text="Inventory Aging Range" Font-Size="2.3em"></asp:Label></asp:TableCell>
                    <asp:TableCell HorizontalAlign="Right">
                        <asp:ImageButton ID="ImageButton1" ImageUrl="~/asp/img/excel_thumb.jpg" OnClick="ImageButton1_Click" runat="server" /></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell ColumnSpan="2">
            <asp:GridView ID="gvInventory_Age" runat="server" AutoGenerateColumns="False" DataSourceID="InventoryAgeSqlDataSource" CellPadding="4" ForeColor="#333333" GridLines="None">
                <AlternatingRowStyle BackColor="White" />
                <Columns>
                    <asp:BoundField DataField="store" HeaderText="STORE" SortExpression="store" />
                    <asp:BoundField DataField="pdvendor" HeaderText="pdvendor" SortExpression="pdvendor" Visible="False" />
                    <asp:BoundField DataField="Manufacturer" HeaderText="MANUFACTURER" SortExpression="Manufacturer" ItemStyle-HorizontalAlign="Center" >
<ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="pdclass" HeaderText="pdclass" SortExpression="pdclass" Visible="False" ItemStyle-HorizontalAlign="Center" >
<ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="Class" HeaderText="CLASS" SortExpression="Class" ItemStyle-HorizontalAlign="Center" >
<ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="Item" HeaderText="ITEM DESCRIPTION" ReadOnly="True" SortExpression="Item" ItemStyle-HorizontalAlign="Center" >
<ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="Age" HeaderText="AGE RANGE" ReadOnly="True" SortExpression="Age" ItemStyle-HorizontalAlign="Center" >
<ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="on_hand" DataFormatString="{0:N0}" HeaderText="QTY ON HAND" ReadOnly="True" SortExpression="on_hand" ItemStyle-HorizontalAlign="Right" >
<ItemStyle HorizontalAlign="Right"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="total_cost" DataFormatString="{0:C2}" HeaderText="TOTAL COST" SortExpression="total_cost">
                    <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
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
            </asp:GridView></asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            <asp:SqlDataSource ID="InventoryAgeSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:Pomps_ReportsConnectionString %>" SelectCommand="select Inventory.pdstore as store,pdvendor,pdmanufacturer as Manufacturer,pdclass,class as Class
,rtrim(convert(varchar(15),Inventory.pdnumber)) + ' ' + rtrim(pddescrip) as Item
,coalesce(sum(pdinvmemo),0) as on_hand
,case when DateDiff(year,case when pdrecvdate = '00000000' then convert(date,'01/01/2001') else convert(date,convert(varchar(2),substring(pdrecvdate,2,2)) + '/' + convert(varchar(2),right(pdrecvdate,2)) + '/' + convert(varchar(4),left(pdrecvdate,4))) end,GetDate()) &gt; 5 then 'Over 5 years'
when DateDiff(year,case when pdrecvdate = '00000000' then convert(date,'01/01/2001') else convert(date,convert(varchar(2),substring(pdrecvdate,2,2)) + '/' + convert(varchar(2),right(pdrecvdate,2)) + '/' + convert(varchar(4),left(pdrecvdate,4))) end,GetDate()) between 3 and 4.99999 then '3 - 5 years'
when DateDiff(year,case when pdrecvdate = '00000000' then convert(date,'01/01/2001') else convert(date,convert(varchar(2),substring(pdrecvdate,2,2)) + '/' + convert(varchar(2),right(pdrecvdate,2)) + '/' + convert(varchar(4),left(pdrecvdate,4))) end,GetDate()) between 1 and 2.99999 then '1 - 3 years'
when DateDiff(year,case when pdrecvdate = '00000000' then convert(date,'01/01/2001') else convert(date,convert(varchar(2),substring(pdrecvdate,2,2)) + '/' + convert(varchar(2),right(pdrecvdate,2)) + '/' + convert(varchar(4),left(pdrecvdate,4))) end,GetDate()) &lt; 1 then '0 - 1 years' end as Age
,sum(pdinvmemo * pdactcost) as total_cost
from SQLMaddenco.dbo.Inventory inner join SQLMaddenco.dbo.Product on rtrim(Inventory.PDNUMBER) = rtrim(Product.pdnumber)
where (pdstore in(select StoreNo from Pomps_Reports.dbo.Database_User_Store where UserID = @UserID)) and  PDINVMEMO &lt;&gt; 0 and (pdvendor = @pdvendor or IsNull(@pdvendor,'All')='All') and (pdclass = @pdclass or IsNull(@pdclass,'00')='00') and (pdstore in(select StoreNo from Pomps_Reports.dbo.Database_User_Store where UserID = @UserID))
group by pdstore,pdvendor,pdmanufacturer,pdclass,class,Inventory.pdnumber,pddescrip
,case when DateDiff(year,case when pdrecvdate = '00000000' then convert(date,'01/01/2001') else convert(date,convert(varchar(2),substring(pdrecvdate,2,2)) + '/' + convert(varchar(2),right(pdrecvdate,2)) + '/' + convert(varchar(4),left(pdrecvdate,4))) end,GetDate()) &gt; 5 then 'Over 5 years'
when DateDiff(year,case when pdrecvdate = '00000000' then convert(date,'01/01/2001') else convert(date,convert(varchar(2),substring(pdrecvdate,2,2)) + '/' + convert(varchar(2),right(pdrecvdate,2)) + '/' + convert(varchar(4),left(pdrecvdate,4))) end,GetDate()) between 3 and 4.99999 then '3 - 5 years'
when DateDiff(year,case when pdrecvdate = '00000000' then convert(date,'01/01/2001') else convert(date,convert(varchar(2),substring(pdrecvdate,2,2)) + '/' + convert(varchar(2),right(pdrecvdate,2)) + '/' + convert(varchar(4),left(pdrecvdate,4))) end,GetDate()) between 1 and 2.99999 then '1 - 3 years'
when DateDiff(year,case when pdrecvdate = '00000000' then convert(date,'01/01/2001') else convert(date,convert(varchar(2),substring(pdrecvdate,2,2)) + '/' + convert(varchar(2),right(pdrecvdate,2)) + '/' + convert(varchar(4),left(pdrecvdate,4))) end,GetDate()) &lt; 1 then '0 - 1 years' end">
                <SelectParameters>
                    <asp:ControlParameter ControlID="lblUserID" Name="UserID" PropertyName="Text" />
                    <asp:ControlParameter ControlID="lblVendor" Name="pdvendor" PropertyName="Text" />
                    <asp:ControlParameter ControlID="lblClass" Name="pdclass" PropertyName="Text" />
                </SelectParameters>
            </asp:SqlDataSource>
        </div>
        <asp:Label ID="lblVendor" runat="server" Text=""></asp:Label>
        <asp:Label ID="lblClass" runat="server" Text=""></asp:Label>
        <asp:Label ID="lblAgeRange" runat="server" Text=""></asp:Label>
        <asp:Label ID="lblUserID" runat="server" Text=""></asp:Label>
    </div>
</asp:Content>

