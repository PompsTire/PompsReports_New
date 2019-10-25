<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Product_Cross_Ref_Look_Up.aspx.cs" Inherits="asp_Intranet_Product_Cross_Ref_Look_Up" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table style="width:100%">
                <tr style="background-color:whitesmoke">
                    <td colspan="2" style="width:850px;font-weight:bold;font-size:1.2em;">Please type the Vendor or Pomp's Item Number or part of the Item Description (All upper Case)
                    <asp:Label ID="lblError" runat="server" Text=""></asp:Label></td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:TextBox ID="txtNAVendorItemNo" runat="server" Width="991px" Height="25px"></asp:TextBox>&nbsp;&nbsp;
                        <asp:ImageButton ID="imgBtnSearch" runat="server" ImageUrl="~/asp/img/magnifying-glass_thumbnail.png" OnClick="imgBtnSearch_Click" Height="19px" Width="16px" ToolTip="search" />Search</td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None" Width="100%" BorderColor="Silver" BorderStyle="Solid" BorderWidth="2px">
                            <AlternatingRowStyle BackColor="White" />
                            <Columns>
                                <asp:BoundField DataField="CUSTOMER_NAME" HeaderText="CUSTOMER NAME" SortExpression="CUSTOMER_NAME" />
                                <asp:BoundField DataField="NA_VENDOR_NUMBER" HeaderText="NA_VENDOR_NUMBER" SortExpression="NA_VENDOR_NUMBER" Visible="False" />
                                <asp:BoundField DataField="NA_VENDOR_NAME" HeaderText="NA VENDOR NAME" SortExpression="NA_VENDOR_NAME" />
                                <asp:BoundField DataField="NA_VENDOR_ITEM_NO" HeaderText="NA VENDOR ITEM NO" SortExpression="NA_VENDOR_ITEM_NO" >
                                <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="POMPS_ITEM_NO" HeaderText="POMP'S ITEM NO" SortExpression="POMPS_ITEM_NO" >
                                <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="ITEM_DESCRIPTION" HeaderText="ITEM DESCRIPTION" SortExpression="ITEM_DESCRIPTION" />
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
        </div>
    </form>
</body>
</html>
