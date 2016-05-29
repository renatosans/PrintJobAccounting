<%@ Page Title="" Language="C#" MasterPageFile="~/SettingsMasterPage.master" AutoEventWireup="true" CodeBehind="SmtpServerSettings.aspx.cs" Inherits="WebAccounting.SmtpServerSettings" %>

<asp:Content ID="headContent" ContentPlaceHolderID="headPlaceHolder" Runat="Server">
</asp:Content>

<asp:Content ID="bodyContent" ContentPlaceHolderID="bodyPlaceHolder" Runat="Server">
    <asp:Panel ID="settingsArea" runat="server" HorizontalAlign="Center">
        <br/>
        <br/>
        <asp:Label ID="lblTitle" runat="server" Text="Dados do servidor de Smtp" CssClass="titleStyle"></asp:Label>
        <br/>
        <br/>
        <div style="width: 70%; margin-left: auto; margin-right: auto;">
            <asp:Label ID="lblPageInfo" runat="server" Text="Informe o endereço e a porta do servidor.
            Caso necessite de autenticação no servidor informe usuário e senha." CssClass="pageInfoStyle"></asp:Label>
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
