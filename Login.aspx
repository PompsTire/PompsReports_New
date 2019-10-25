<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Pomp's SQL Reporting</title>
    <link rel="stylesheet" type="text/css" href="css/StyleSheet.css" media="screen" />
</head>
<body>
    <form id="form1" runat="server">
        <div style="max-width=560px">
            <div id="Top">
                <asp:Label ID="Label1" runat="server" Text=""></asp:Label></div>
            <div id="Header" style="align-items:center">
                <asp:Image ID="Logo" CssClass="Logo" ImageUrl="~/img/Pomp-s-Logo-2017-Clr.png" Width="169px" Height="76px" runat="server" />&nbsp;
                SQL Reporting Site
            </div>
            <div id="MainMenu"></div>
    <div  id="MainContent">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="Login">
        <blockquote>
        <asp:Label ID="lblMessage" runat="server" Text="" Font-Size="1.2em"></asp:Label></blockquote>
        <table>
            <tr>
                <td>
        <asp:Table ID="tblLogin" runat="server" Width="550px">
            <asp:TableRow><asp:TableCell>Username:</asp:TableCell></asp:TableRow>
            <asp:TableRow><asp:TableCell><asp:TextBox ID="txtUsername" runat="server" Width="100%" Font-Size="1.2em"></asp:TextBox></asp:TableCell></asp:TableRow>
            <asp:TableRow><asp:TableCell>Password:</asp:TableCell></asp:TableRow>
            <asp:TableRow><asp:TableCell><asp:TextBox ID="txtPassword" runat="server" Width="100%" Font-Size="1.2em" TextMode="Password"></asp:TextBox></asp:TableCell></asp:TableRow>
            <asp:TableRow>
                <asp:TableCell HorizontalAlign="Center">
        <asp:Button ID="btnLogin" runat="server" Text="Login" cssClass="btnCmd" OnClick="btnLogin_Click" /> &nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btnForgotPwd" runat="server" cssClass="btnCmd" Text="Forgot my Password" OnClick="btnForgotPwd_Click" /></asp:TableCell>
            </asp:TableRow>
        </asp:Table></td>
            </tr>
            <tr>
                <td>
        <asp:Table ID="tblResetPassword" runat="server" Width="550px">
            <asp:TableRow><asp:TableCell>New Password:</asp:TableCell></asp:TableRow>
            <asp:TableRow><asp:TableCell><asp:TextBox ID="txtNewPwd" runat="server" Width="100%" Font-Size="1.2em" TextMode="Password"></asp:TextBox></asp:TableCell></asp:TableRow>
            <asp:TableRow><asp:TableCell>Confirm Password:</asp:TableCell></asp:TableRow>
            <asp:TableRow><asp:TableCell><asp:TextBox ID="txtConfirmPwd" runat="server" Width="100%" Font-Size="1.2em" TextMode="Password"></asp:TextBox></asp:TableCell></asp:TableRow>
        </asp:Table>
                    <asp:Table ID="tblForgotPwd" runat="server" Width="550px">
                        <asp:TableRow><asp:TableCell>E-mail Address:</asp:TableCell></asp:TableRow>
                        <asp:TableRow><asp:TableCell><asp:TextBox ID="txtPompsEmail" runat="server" Font-Size="1.2em" Width="100%"></asp:TextBox></asp:TableCell></asp:TableRow>
                        <asp:TableRow><asp:TableCell>Employee Number:</asp:TableCell></asp:TableRow>
                        <asp:TableRow><asp:TableCell><asp:TextBox ID="txtEmpNo" runat="server" Font-Size="1.2em" Width="100%" TextMode="Password"></asp:TextBox></asp:TableCell></asp:TableRow>
                    </asp:Table>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="ResetPwd" runat="server" Width="150px" Height="50px" Text="Submit" OnClick="ResetPwd_Click" /></td>
            </tr>
        </table>
    </div></div>
            <div id="footer">
                &copy;<asp:Label ID="lblYear" runat="server" Text=""></asp:Label>&nbsp;Pomp's Tire Service, Inc.
            </div>
        </div>
    </form>
</body>
</html>
