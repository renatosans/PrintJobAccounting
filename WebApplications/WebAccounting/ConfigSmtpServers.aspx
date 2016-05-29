<%@ Page Title="" Language="C#" MasterPageFile="~/AccountingMasterPage.master" AutoEventWireup="true" CodeBehind="ConfigSmtpServers.aspx.cs" Inherits="WebAccounting.ConfigSmtpServers" %>

<asp:Content ID="headContent" ContentPlaceHolderID="headPlaceHolder" Runat="Server">
</asp:Content>

<asp:Content ID="bodyContent" ContentPlaceHolderID="bodyPlaceHolder" Runat="Server">
    <asp:Panel ID="configurationArea" runat="server" HorizontalAlign="Center">
        <br/>
        <br/>
        <asp:Label ID="lblTitle" runat="server" Text="Configurar Servidores SMTP" CssClass="titleStyle"></asp:Label>
        <br/>
        <br/>
        <div style="width: 70%; margin-left: auto; margin-right: auto;">
            <asp:Label ID="lblPageInfo" runat="server" Text="Cadastre os servidores de SMTP que o sistema pode utilizar ao enviar seus e-mails.
            Eles são necessários para o envio de relatórios periódicos (mailing)." CssClass="pageInfoStyle"></asp:Label>
        </div>
        <br/>
        <br/>
    </asp:Panel>
    <asp:Panel ID="controlArea" runat="server" HorizontalAlign="Center">
        <asp:Button ID="btnNovo" runat="server" Width="125px" Height="30px" Text="Criar um novo" />
        <br/>
        <br/>
        <br/>
    </asp:Panel>
</asp:Content>
