<%@ Page Title="" Language="C#" MasterPageFile="~/AccountingMasterPage.master" AutoEventWireup="true" CodeBehind="ConfigUsers.aspx.cs" Inherits="WebAccounting.ConfigUsers" %>

<asp:Content ID="headContent" ContentPlaceHolderID="headPlaceHolder" Runat="Server">
</asp:Content>

<asp:Content ID="bodyContent" ContentPlaceHolderID="bodyPlaceHolder" Runat="Server">
    <asp:Panel ID="configurationArea" runat="server" HorizontalAlign="Center">
        <br/>
        <br/>
        <asp:Label ID="lblTitle" runat="server" Text="Configurar Usu�rios" CssClass="titleStyle"></asp:Label>
        <br/>
        <br/>
        <div style="width: 70%; margin-left: auto; margin-right: auto;">
            <asp:Label ID="lblPageInfo" runat="server" Text="Os usu�rios identificados pelo sistema est�o listados abaixo.
            Novos usu�rios s�o reconhecidos automaticamente pelo sistema quando trabalhos de impress�o s�o realizados." CssClass="pageInfoStyle"></asp:Label>
        </div>
        <br/>
        <br/>
    </asp:Panel>
    <asp:Panel ID="controlArea" runat="server" HorizontalAlign="Center">
        <br/>
        <br/>
        <br/>
    </asp:Panel>
</asp:Content>
