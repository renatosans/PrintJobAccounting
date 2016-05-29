<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoginPage.aspx.cs" Inherits="WebAdministrator.LoginPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-type" content="text/html; charset=UTF-8" />
    <meta http-equiv="Content-Language" content="pt-br" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" >
    <title>Datacopy Trade</title>
    <link href="StyleSheets/LoginStyles.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <div id="horizont">
        <form id="loginArea" runat="server">
            <asp:Table ID="loginBox" runat="server" CssClass="loginBoxStyle" Width="380px" Height="240px">
                <asp:TableRow ID="TableRow1" runat="server" HorizontalAlign="Center">
                    <asp:TableCell ID="TableCell1A" runat="server" ColumnSpan="2"><img alt="Datacopy Trade" src="images/login.png" width="250" height="80" /></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow ID="TableRow2" runat="server">
                    <asp:TableCell ID="TableCell2A" runat="server"><asp:Label ID="lblLoginName" runat="server" Text="Usuário"></asp:Label></asp:TableCell>
                    <asp:TableCell ID="TableCell2B" runat="server"><asp:TextBox ID="txtLoginName" runat="server" Width="205px" Text=""></asp:TextBox></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow ID="TableRow3" runat="server">
                    <asp:TableCell ID="TableCell3A" runat="server"><asp:Label ID="lblPassword" runat="server" Text="Senha"></asp:Label></asp:TableCell>
                    <asp:TableCell ID="TableCell3B" runat="server"><asp:TextBox ID="txtPassword" TextMode="Password" runat="server" Width="205px" Text=""></asp:TextBox></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow ID="TableRow4" runat="server" HorizontalAlign="Center">
                    <asp:TableCell ID="TableCell4A" runat="server" ColumnSpan="2"></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow ID="TableRow5" runat="server" HorizontalAlign="Center">
                    <asp:TableCell ID="TableCell5A" runat="server" ColumnSpan="2"><asp:Button ID="btnLogin" runat="server" Text="Login" Width="120px" onclick="btnLogin_Click" /></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow ID="TableRow6" runat="server" HorizontalAlign="Center">
                    <asp:TableCell ID="TableCell6A" runat="server" ColumnSpan="2"><asp:Label ID="lblErrorMessages" runat="server" Text="Mensagens de erro" CssClass="errorMessagesStyle" /></asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </form>
        <asp:Panel ID="controlArea" runat="server" CssClass="controlAreaStyle">
        </asp:Panel>
    </div>
</body>
</html>
