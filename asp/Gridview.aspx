<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Gridview.aspx.cs" Inherits="asp_Gridview" %>


<asp:Content ID="Content5" ContentPlaceHolderID="cpMainContent" Runat="Server">
    <div id="MainContent">
        <table style="width:100%">
            <tr>
                <td></td>
                <td style="font-size:1em;color:gray;text-align:right;padding:5px;"><asp:Label ID="lblTitle" runat="server" Text=""></asp:Label>&nbsp;
                    <asp:ImageButton ID="imgExcelDownload" ImageUrl="~/img/excel_thumb.jpg" runat="server" AlternateText="Download to Excel" OnClick="imgExcelDownload_Click" /></td>
                <td></td>
            </tr>
            <tr>
                <td style="width:10%"></td>
                <td style="width:80%">
    <asp:GridView ID="GridView1" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None" Width="100%">
        <AlternatingRowStyle BackColor="White" />
        <EditRowStyle BackColor="#7C6F57" />
        <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#003399" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
        <RowStyle BackColor="#E3EAEB" />
        <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
        <SortedAscendingCellStyle BackColor="#F8FAFA" />
        <SortedAscendingHeaderStyle BackColor="#246B61" />
        <SortedDescendingCellStyle BackColor="#D4DFE1" />
        <SortedDescendingHeaderStyle BackColor="#15524A" />
                    </asp:GridView>
                </td>
                <td style="width:10%"></td>
            </tr>
        </table>
    <asp:Label ID="lblStoreNo" runat="server" Text=""></asp:Label>
    <asp:Label ID="lblSalesmanNo" runat="server" Text=""></asp:Label>
    <asp:Label ID="lblUsername" runat="server" Text=""></asp:Label></div>
</asp:Content>

