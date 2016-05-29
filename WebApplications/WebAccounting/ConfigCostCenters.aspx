<%@ Page Title="" Language="C#" MasterPageFile="~/AccountingMasterPage.master" AutoEventWireup="true" CodeBehind="ConfigCostCenters.aspx.cs" Inherits="WebAccounting.ConfigCostCenters" %>

<asp:Content ID="headContent" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>

<asp:Content ID="bodyContent" ContentPlaceHolderID="bodyPlaceHolder" runat="server">
    <asp:Panel ID="configurationArea" runat="server" HorizontalAlign="Center">
        <br/>
        <br/>
        <asp:Label ID="lblTitle" runat="server" Text="Centros de Custo" CssClass="titleStyle"></asp:Label>
        <br/>
        <br/>
        <div style="width: 70%; margin-left: auto; margin-right: auto;">
            <asp:Label ID="lblPageInfo" runat="server" Text="Agrupe os usuários por centro de custo para tirar relatórios por centro de custo.
            Os usuários agrupados nos centros de custo nas extremidades da hierarquia também aparecem nos níveis superiores da hierarquia." CssClass="pageInfoStyle"></asp:Label>
        </div>
        <br/>
        <br/>
        <div runat="server" style="width: 80%; margin-left: auto; margin-right: auto;">
            <div id="divCostCenters" style="float:left; width: 48%; height: 300px;">
                <asp:Panel ID="pnlCostCenters" runat="server" scrollbars="both" height="200px" backcolor="WhiteSmoke" horizontalalign="Left">
                </asp:Panel>
                <br/>
                <div runat="server" style="width: 90%; height: 30px; margin-left: auto; margin-right: auto;">
                    <input type="button" id="btnCreate" runat="server" style="float:left; width:140px; height:30px" value="Criar um novo" />
                    <input type="button" id="btnRemove" runat="server" style="float:right; width:140px; height:30px" value="Remover" />
                </div>
            </div>
            <div id="divAssociates" style="float:right; width: 48%; height: 300px;">
                <asp:Panel ID="pnlAssociates" runat="server" scrollbars="Vertical" height="200px" backcolor="WhiteSmoke" horizontalalign="Left">
                </asp:Panel>
                <br/>
                <div runat="server" style="width: 90%; height: 30px; margin-left: auto; margin-right: auto;">
                    <input type="button" id="btnAssociate" runat="server" style="float:left; width:140px; height:30px" value="Associar usuários" />
                    <input type="button" id="btnDisassociate" runat="server" style="float:right; width:140px; height:30px" value="Desassociar usuários" />
                </div>
            </div>
        </div>
    </asp:Panel>
    <asp:Panel ID="controlArea" runat="server" HorizontalAlign="Center">
        <input type="hidden" id="txtSelectedNode" runat="server" />
        <input type="hidden" id="txtRootNode" runat="server" />
    </asp:Panel>
</asp:Content>
