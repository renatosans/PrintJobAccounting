<%@ Page Title="" Language="C#" MasterPageFile="~/SettingsMasterPage.master" AutoEventWireup="true" CodeBehind="PageCounterHistory.aspx.cs" Inherits="WebAccounting.PageCounterHistory" %>

<asp:Content ID="headContent" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>

<asp:Content ID="bodyContent" ContentPlaceHolderID="bodyPlaceHolder" runat="server">
    <asp:Panel ID="displayArea" runat="server" HorizontalAlign="Center">
        <br/>
        <br/>
        <asp:Label ID="lblTitle" runat="server" Text="Histórico do contador" CssClass="titleStyle"></asp:Label>
        <br/>
        <br/>
    </asp:Panel>
    <asp:Panel ID="controlArea" runat="server" HorizontalAlign="Center">
        <asp:Button ID="btnOK" runat="server" Width="125px" Height="30px" 
            Text="OK" onclick="btnOK_Click" />
        <br/>
        <br/>
        <br/>
    </asp:Panel>
</asp:Content>
