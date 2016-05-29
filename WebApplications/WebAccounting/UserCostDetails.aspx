<%@ Page Title="" Language="C#" MasterPageFile="~/AccountingMasterPage.master" AutoEventWireup="true" CodeBehind="UserCostDetails.aspx.cs" Inherits="WebAccounting.UserCostDetails" %>

<asp:Content ID="bodyContent" ContentPlaceHolderID="bodyPlaceHolder" Runat="Server">
    <asp:Panel ID="reportSurface" runat="server" HorizontalAlign="Center">
        <br/>
        <br/>
        <asp:Label ID="lblTitle" runat="server" Text="Relatório de custos de impressão/cópia" CssClass="titleStyle"></asp:Label>
        <br/>
        <br/>
        <asp:Label ID="lblUsername" runat="server" Text="Usuário: " CssClass="titleStyle"></asp:Label>
        <br/>
        <br/>
    </asp:Panel>
</asp:Content>
