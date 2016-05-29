<%@ Page Title="" Language="C#" MasterPageFile="~/AccountingMasterPage.master" AutoEventWireup="true" CodeBehind="DeviceCostDetails.aspx.cs" Inherits="WebAccounting.DeviceCostDetails" %>

<asp:Content ID="bodyContent" ContentPlaceHolderID="bodyPlaceHolder" Runat="Server">
    <asp:Panel ID="reportSurface" runat="server" HorizontalAlign="Center">
        <br/>
        <br/>
        <asp:Label ID="lblTitle" runat="server" Text="Relatório de custos de impressão/cópia" CssClass="titleStyle"></asp:Label>
        <br/>
        <br/>
        <asp:Label ID="lblDeviceName" runat="server" Text="Impressora: " CssClass="titleStyle"></asp:Label>
        <br/>
        <br/>
    </asp:Panel>
</asp:Content>
