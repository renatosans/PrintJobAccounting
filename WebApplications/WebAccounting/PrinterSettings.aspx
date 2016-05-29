<%@ Page Title="" Language="C#" MasterPageFile="~/SettingsMasterPage.master" AutoEventWireup="true" CodeBehind="PrinterSettings.aspx.cs" Inherits="WebAccounting.PrinterSettings" %>

<asp:Content ID="headContent" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>

<asp:Content ID="bodyContent" ContentPlaceHolderID="bodyPlaceHolder" runat="server">
    <asp:Panel ID="settingsArea" runat="server" HorizontalAlign="Center">
        <br/>
        <br/>
        <asp:Label ID="lblTitle" runat="server" Text="Dados da impressora" CssClass="titleStyle"></asp:Label>
        <br/>
        <br/>
        <div style="width: 70%; margin-left: auto; margin-right: auto;">
            <asp:Label ID="lblPageInfo" runat="server" Text="Informe o custo página da impressora (Impressões em preto e branco).
            Opcionalmente, informe o preço de impressões coloridas." CssClass="pageInfoStyle"></asp:Label>
        </div>
        <br/>
        <br/>
    </asp:Panel>
    <asp:Panel ID="controlArea" runat="server" HorizontalAlign="Center">
        <asp:Button ID="btnSubmit" runat="server" Width="125px" Height="30px" 
            Text="Enviar" onclick="btnSubmit_Click" />
        <br/>
        <br/>
        <br/>
    </asp:Panel>
</asp:Content>
