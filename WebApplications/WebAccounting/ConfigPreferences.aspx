<%@ Page Title="" Language="C#" MasterPageFile="~/AccountingMasterPage.master" AutoEventWireup="true" CodeBehind="ConfigPreferences.aspx.cs" Inherits="WebAccounting.ConfigPreferences" %>

<asp:Content ID="headContent" ContentPlaceHolderID="headPlaceHolder" Runat="Server">
</asp:Content>

<asp:Content ID="bodyContent" ContentPlaceHolderID="bodyPlaceHolder" Runat="Server">
    <asp:Panel ID="configurationArea" runat="server" HorizontalAlign="Center">
        <br/>
        <br/>
        <asp:Label ID="lblTitle" runat="server" Text="Preferências" CssClass="titleStyle"></asp:Label>
        <br/>
        <br/>
        <div style="width: 70%; margin-left: auto; margin-right: auto;">
            <asp:Label ID="lblPageInfo" runat="server" Text="Configure os itens abaixo para definir o comportamento do sistema.
            Preencha o nome da empresa assim como deverá aparecer nos relatórios e defina o remetente que o sistema deverá usar para e-mails periódicos de relatório." CssClass="pageInfoStyle"></asp:Label>
        </div>
        <br/>
        <br/>
        <asp:Panel ID="pnlTenant" runat="server" align="center" style="width: 50%; margin-left: auto; margin-right: auto;">
            <asp:Label ID="lbltenant" runat="server" Text="Dados da empresa" CssClass="pageInfoStyle"></asp:Label>
            <br/>
        </asp:Panel>
        <asp:Panel ID="pnlTenantPreferences" runat="server" align="center" style="width: 50%; margin-left: auto; margin-right: auto;">
            <asp:Label ID="lbltenantPreferences" runat="server" Text="Opções" CssClass="pageInfoStyle"></asp:Label>
            <br/>
        </asp:Panel>
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
