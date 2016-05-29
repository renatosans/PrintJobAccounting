<%@ Page Title="" Language="C#" MasterPageFile="~/AccountingMasterPage.master" AutoEventWireup="true" CodeBehind="GroupCostDetails.aspx.cs" Inherits="WebAccounting.GroupCostDetails" %>

<asp:Content ID="bodyContent" ContentPlaceHolderID="bodyPlaceHolder" runat="server">
    <asp:Panel ID="reportSurface" runat="server" HorizontalAlign="Center">
        <br/>
        <br/>
        <asp:Label ID="lblTitle" runat="server" Text="Relatório de custos de impressão/cópia" CssClass="titleStyle"></asp:Label>
        <br/>
        <br/>
        <asp:Label ID="lblCostCenter" runat="server" Text="Centro de Custo: " CssClass="titleStyle"></asp:Label>
        <br/>
        <br/>
    </asp:Panel>
</asp:Content>
