<%@ Page Title="" Language="C#" MasterPageFile="~/SettingsMasterPage.master" AutoEventWireup="true" CodeBehind="AssociateSettings.aspx.cs" Inherits="WebAccounting.AssociateSettings" %>

<asp:Content ID="headContent" ContentPlaceHolderID="headPlaceHolder" runat="server">
    <script type="text/javascript" src="Scripts/jquery-1.4.2.min.js"></script>
</asp:Content>

<asp:Content ID="bodyContent" ContentPlaceHolderID="bodyPlaceHolder" runat="server">
    <asp:Panel ID="settingsArea" runat="server" HorizontalAlign="Center">
        <br/>
        <br/>
        <asp:Label ID="lblTitle" runat="server" Text="Associação" CssClass="titleStyle"></asp:Label>
        <br/>
        <br/>
        <div style="width: 70%; margin-left: auto; margin-right: auto;">
            <asp:Label ID="lblPageInfo" runat="server" Text="Escolha os usuários na caixa de seleção abaixo." CssClass="pageInfoStyle"></asp:Label>
        </div>
        <br/>
        <br/>
        <div id="transferArea" runat="server" style="width: 80%; margin-left: auto; margin-right: auto;">
            <div id="divAvailable" style="float:left; width: 40%;">
                <select id="listAvailable" runat="server" style="width: 100%; height: 200px; background: WhiteSmoke;" multiple="true" />
            &nbsp;</div>
            <div id="transferButtons" style="float:left; width: 18%; height: 150px; margin-top: 50px;">
                <input type="button" id="add" value=">>" />
                <br/><br/>
                <input type="button" id="remove" value="<<" />
            </div>            
            <div id="divTransfered" style="float:right; width: 40%;">
                <select id="listTransfered" runat="server" style="width: 100%; height: 200px; background: WhiteSmoke;" multiple="true" />
            &nbsp;</div>
        </div>
        <br/>
        <br/>        
    </asp:Panel>
    <asp:Panel ID="controlArea" runat="server" HorizontalAlign="Center">
        <input type="button" id="btnSubmit" style="width:125px; height:30px;" onclick="javascript:btnSubmitClick();" value="Enviar" />
        <br/>
        <br/>
        <br/>
    </asp:Panel>
    <script type='text/javascript'>
        // Define as ações dos botões "add" e "remove"
        var available = GetFirstElementContainingSubstring('listAvailable');
        var transfered = GetFirstElementContainingSubstring('listTransfered');
        $().ready(function() {
            $('#add').click(function() {
                return !$('#' + available + ' option:selected').remove().appendTo('#' + transfered);
            });
            $('#remove').click(function() {
                return !$('#' + transfered + ' option:selected').remove().appendTo('#' + available);
            });
        });
        
        // Define a ação do botão enviar
        function btnSubmitClick() {
            var listTransfered = document.getElementById(GetFirstElementContainingSubstring('listTransfered'));
            for (i = 0; i < listTransfered.length; i++) {
                var input = document.createElement('input');
                input.setAttribute('type', 'hidden');
                input.setAttribute('name', 'transferedItem' + listTransfered.options[i].value);
                input.setAttribute('value', listTransfered.options[i].text);
                document.forms[0].appendChild(input);
            }
            document.forms[0].submit();
        }
    </script>
</asp:Content>
