<%@ Page Title="" Language="C#" MasterPageFile="~/AdministratorMasterPage.Master" AutoEventWireup="true" CodeBehind="TenantSettings.aspx.cs" Inherits="WebAdministrator.TenantSettings" %>

<asp:Content ID="headContent" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>

<asp:Content ID="bodyContent" ContentPlaceHolderID="bodyPlaceHolder" runat="server">
    <asp:Panel ID="settingsArea" runat="server" HorizontalAlign="Center">
        <br/>
        <br/>
        <asp:Label ID="lblTitle" runat="server" Text="Dados da empresa" CssClass="titleStyle"></asp:Label>
        <br/>
        <br/>
        <div style="width: 70%; margin-left: auto; margin-right: auto;">
            <asp:Label ID="lblPageInfo" runat="server" Text="Informe o identificador da empresa e seu nome amigável." CssClass="pageInfoStyle"></asp:Label>
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
