<%@ Page Title="" Language="C#" MasterPageFile="~/AdministratorMasterPage.Master" AutoEventWireup="true" CodeBehind="ConfigTenants.aspx.cs" Inherits="WebAdministrator.ConfigTenants" %>

<asp:Content ID="headContent" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>

<asp:Content ID="bodyContent" ContentPlaceHolderID="bodyPlaceHolder" runat="server">
    <asp:Panel ID="configurationArea" runat="server" HorizontalAlign="Center">
        <br/>
        <br/>
        <asp:Label ID="lblTitle" runat="server" Text="Configurar Empresas" CssClass="titleStyle"></asp:Label>
        <br/>
        <br/>
        <div style="width: 70%; margin-left: auto; margin-right: auto;">
            <asp:Label ID="lblPageInfo" runat="server" Text="Cadastre as empresas clientes que tem acesso ao sistema." CssClass="pageInfoStyle"></asp:Label>
        </div>
        <br/>
        <br/>        
    </asp:Panel>
    <asp:Panel ID="controlArea" runat="server" HorizontalAlign="Center">
        <asp:Button ID="btnNovo" runat="server" Width="125px" Height="30px" Text="Criar uma nova" />
        <br/>
        <br/>
        <br/>
    </asp:Panel>
</asp:Content>
