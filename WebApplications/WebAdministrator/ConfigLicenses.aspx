<%@ Page Title="" Language="C#" MasterPageFile="~/AdministratorMasterPage.Master" AutoEventWireup="true" CodeBehind="ConfigLicenses.aspx.cs" Inherits="WebAdministrator.ConfigLicenses" %>

<asp:Content ID="headContent" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>

<asp:Content ID="bodyContent" ContentPlaceHolderID="bodyPlaceHolder" runat="server">
    <asp:Panel ID="configurationArea" runat="server" HorizontalAlign="Center">
        <br/>
        <br/>
        <asp:Label ID="lblTitle" runat="server" Text="Configurar Licenças" CssClass="titleStyle"></asp:Label>
        <br/>
        <br/>
        <div style="width: 70%; margin-left: auto; margin-right: auto;">
            <asp:Label ID="lblPageInfo" runat="server" Text="Adicione licenças de acordo com a quantidade de estações de trabalho em cada empresa." CssClass="pageInfoStyle"></asp:Label>
        </div>
        <br/>
        <br/>        
    </asp:Panel>
</asp:Content>
