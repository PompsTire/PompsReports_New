<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="CustomerBalance.aspx.cs" Inherits="asp_CustomerBalance" %>


<asp:Content ID="Content5" ContentPlaceHolderID="cpMainContent" Runat="Server">
    <div id="MainContent">
        <div id="Login">
            <asp:Table ID="Table1" runat="server" Width="1050px">
                <asp:TableRow>
                    <asp:TableCell>
            <asp:Label ID="lblCustomerBalance" runat="server" Text="Customer Outstanding Balance" Font-Size="2.3em"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Right">
            <asp:ImageButton ID="ImageButton1" ImageUrl="~/asp/img/excel_thumb.jpg" OnClick="ImageButton1_Click" runat="server" /></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell ColumnSpan="2">
            <asp:GridView ID="gvCustomerBalance" runat="server" AutoGenerateColumns="False" CellPadding="4" DataSourceID="CustomerBalanceSqlDataSource" ForeColor="#333333" GridLines="None" Width="1050px" AllowPaging="True" PageSize="250">
                <AlternatingRowStyle BackColor="White" />
                <Columns>
                    <asp:BoundField DataField="Customer" HeaderText="Customer" SortExpression="Customer" />
                    <asp:BoundField DataField="Invoice_Number" HeaderText="Invoice Number" SortExpression="Invoice_Number" >
                    <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Invoice_Date" DataFormatString="{0:d}" HeaderText="Invoice Date" SortExpression="Invoice_Date" >
                    <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Invoice_Total" HeaderText="Invoice Total" SortExpression="Invoice_Total" DataFormatString="{0:c2}" >
                    <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                    <asp:BoundField DataField="YTD_Paid" HeaderText="YTD Paid" ReadOnly="True" SortExpression="YTD_Paid" DataFormatString="{0:c2}" >
                    <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Balance" DataFormatString="{0:c2}" HeaderText="Balance" SortExpression="Balance">
                    <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Period" HeaderText="Comments" ReadOnly="True" SortExpression="Period">
                    <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                </Columns>
                <EditRowStyle BackColor="#7C6F57" />
                <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#d8ecff" Font-Bold="True" ForeColor="#10331F" />
                <PagerStyle BackColor="#d8ecff" ForeColor="#10331F" HorizontalAlign="Center" />
                <RowStyle BackColor="#E3EAEB" />
                <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                <SortedAscendingCellStyle BackColor="#F8FAFA" />
                <SortedAscendingHeaderStyle BackColor="#246B61" />
                <SortedDescendingCellStyle BackColor="#D4DFE1" />
                <SortedDescendingHeaderStyle BackColor="#15524A" />
            </asp:GridView></asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            <asp:SqlDataSource ID="CustomerBalanceSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:Pomps_ReportsConnectionString %>" SelectCommand=" select tihhnumcst,tihhnamsrt as customer,tihhnuminv as Invoice_Number,tihhdteinv as Invoice_Date,tihhtotinv as Invoice_Total,coalesce(sum(ttaramtapl),0) as YTD_Paid,tihhtotinv - sum(ttaramtapl) as Balance,tihhyrprin as period
from SQLMaddenco.dbo.tmihsh as i left outer join SQLMaddenco.dbo.ttard as p on i.tihhnuminv = p.ttarinvapl
where (tihhnumcst = @CustNumber or IsNull(@CustNumber,1) = 1) and (tihhnumstr in(select StoreNo from Pomps_Reports.dbo.Database_User_Store where UserID = @UserID)) and (tihhslmsel in (Select SalesmanNo from Pomps_Reports.dbo.Database_User_Salesman where UserID = @UserID))
group by tihhnuminv,tihhyrprin,tihhdteinv,tihhnumcst,tihhnamsrt,tihhtotinv
having tihhtotinv - sum(ttaramtapl) &lt;&gt; 0">
                <SelectParameters>
                    <asp:ControlParameter ControlID="lblCustNumber" Name="CustNumber" PropertyName="Text" />
                    <asp:ControlParameter ControlID="lblUserID" Name="UserID" PropertyName="Text" />
                </SelectParameters>
            </asp:SqlDataSource>
        </div><asp:Label ID="lblCustNumber" runat="server" Text=""></asp:Label>
        <asp:Label ID="lblUserID" runat="server" Text=""></asp:Label>
    </div>
</asp:Content>

