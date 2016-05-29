<%@ Page Title="" Language="C#" MasterPageFile="~/AdministratorMasterPage.Master" AutoEventWireup="true" CodeBehind="AddLicenses.aspx.cs" Inherits="WebAdministrator.AddLicenses" %>

<asp:Content ID="headContent" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>

<asp:Content ID="bodyContent" ContentPlaceHolderID="bodyPlaceHolder" runat="server">
    <asp:Panel ID="settingsArea" runat="server" HorizontalAlign="Center">
        <br/>
        <br/>
        <asp:Label ID="lblTitle" runat="server" Text="Adicionar Licenças" CssClass="titleStyle"></asp:Label>
        <br/>
        <br/>
        <div style="width: 70%; margin-left: auto; margin-right: auto;">
            <asp:Label ID="lblPageInfo" runat="server" Text="Informe a quantidade de licenças" CssClass="pageInfoStyle"></asp:Label>
        </div>
        <br/>
        <br/>
    </asp:Panel>
    <asp:Panel ID="controlArea" runat="server" HorizontalAlign="Center">
        <asp:Button ID="btnAdd" runat="server" Width="125px" Height="30px" 
            Text="Adicionar" onclick="btnAdd_Click" />
        <br/>
        <br/>
        <br/>
    </asp:Panel>
</asp:Content>
