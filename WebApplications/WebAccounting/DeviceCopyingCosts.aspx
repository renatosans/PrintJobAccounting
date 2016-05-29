<%@ Page Title="" Language="C#" MasterPageFile="~/AccountingMasterPage.master" AutoEventWireup="true" CodeBehind="DeviceCopyingCosts.aspx.cs" Inherits="WebAccounting.DeviceCopyingCosts" %>

<asp:Content ID="headContent" ContentPlaceHolderID="headPlaceHolder" Runat="Server">
    <script type="text/javascript" src="Scripts/DateRange.js"></script>
    <script type="text/javascript" src="Scripts/Calendar.js"></script>
    <script type="text/javascript" src="Scripts/CalendarLanguage.js"></script>
    <link type="text/css" href="StyleSheets/CalendarStyles.css" rel="stylesheet" />
</asp:Content>


<asp:Content ID="bodyContent" ContentPlaceHolderID="bodyPlaceHolder" Runat="Server">
    <asp:Panel ID="reportSurface" runat="server" HorizontalAlign="Center">
        <br/>
        <br/>
        <asp:Label ID="lblTitle" runat="server" Text="Relatório de custos de cópia por Equipamento" CssClass="titleStyle"></asp:Label>
        <br/>
        <br/>
        <div style="width: 320px; margin-left: auto; margin-right: auto;">
        <asp:Table ID="reportFilter" runat="server" CssClass="reportFilterStyle">
            <asp:TableRow ID="TableRow1" runat="server">
                <asp:TableCell ID="TableCell1A" runat="server"><asp:Label ID="lblPrinter" runat="server" Text="Copiadora"  Width="100"></asp:Label></asp:TableCell>
                <asp:TableCell ID="TableCell1B" runat="server" ColumnSpan="2"><asp:DropDownList ID="cmbPrinter" runat="server" Width="210px"></asp:DropDownList></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="TableRow2" runat="server">
                <asp:TableCell ID="TableCell2A" runat="server"><asp:Label ID="lblStartDate" runat="server" Text="Data Inicial"></asp:Label></asp:TableCell>
                <asp:TableCell ID="TableCell2B" runat="server" width="70%" HorizontalAlign="Left"><input type="text" id="txtStartDate" runat="server" style="width:75%" value="" disabled="disabled" /><input type="button" id="btnOpenCalendar1" style="width:15%" value="..." disabled="disabled" runat="server" /></asp:TableCell>
                <asp:TableCell ID="TableCell2C" runat="server"><input type="text" id="txtStartHour" runat="server" style="width:90%" value="" disabled="disabled" /></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="TableRow3" runat="server">
                <asp:TableCell ID="TableCell3A" runat="server"><asp:Label ID="lblEndDate" runat="server" Text="Data Final"></asp:Label></asp:TableCell>
                <asp:TableCell ID="TableCell3B" runat="server" width="70%" HorizontalAlign="Left"><input type="text" id="txtEndDate" runat="server" style="width:75%" value="" disabled="disabled" /><input type="button" id="btnOpenCalendar2" style="width:15%" value="..." disabled="disabled" runat="server" /></asp:TableCell>
                <asp:TableCell ID="TableCell3C" runat="server"><input type="text" id="txtEndHour" runat="server" style="width:90%" value="" disabled="disabled" /></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="TableRow4" runat="server" HorizontalAlign="Center">
                <asp:TableCell ID="TableCell4A" runat="server" ColumnSpan="3"><input type="checkbox" id="chkLastMonth" runat="server" checked="checked" onclick="LastMonthChanged();" />Último mês</asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="TableRow5" runat="server" HorizontalAlign="Center">
                <asp:TableCell ID="TableCell5A" runat="server" ColumnSpan="3"></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="TableRow6" runat="server" HorizontalAlign="Center">
                <asp:TableCell ID="TableCell6A" runat="server" ColumnSpan="3"><input type="button" id="btnGenerateReport" onclick="GenerateReportClick();" value="Gerar Relatório" class="buttonStyle"/></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="TableRow7" runat="server" HorizontalAlign="Center">
                <asp:TableCell ID="TableCell7A" runat="server" ColumnSpan="3"><asp:Label ID="lblErrorMessages" runat="server" Text="Mensagens de erro" CssClass="errorMessagesStyle"/></asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        </div>
        <br/>
        <br/>
    </asp:Panel>
    <input type="hidden" id="hiddenStartDate"  runat="server" />
    <input type="hidden" id="hiddenStartHour"  runat="server" />
    <input type="hidden" id="hiddenEndDate"  runat="server" />
    <input type="hidden" id="hiddenEndHour" runat="server" />
    <script type='text/javascript'>
        var txtStartDate = GetFirstElementContainingSubstring('txtStartDate');
        var btnOpenCalendar1 = GetFirstElementContainingSubstring("btnOpenCalendar1");
        var txtEndDate = GetFirstElementContainingSubstring('txtEndDate');
        var btnOpenCalendar2 = GetFirstElementContainingSubstring("btnOpenCalendar2");

        var cal = Calendar.setup({
            bottomBar: false,
            onSelect: function(cal) { cal.hide() }
        });
        cal.manageFields(btnOpenCalendar1, txtStartDate, "%Y-%m-%d");
        cal.manageFields(btnOpenCalendar2, txtEndDate, "%Y-%m-%d");
    </script>
</asp:Content>
