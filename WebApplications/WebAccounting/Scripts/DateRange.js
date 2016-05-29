
// Função disparada quando o checkbox de último mês foi marcado/desmarcado
function LastMonthChanged() {
    // não confundir o tipo dos objetos, neste escopo são elementos, no escopo acima são Strings
    var chkLastMonth = document.getElementById(GetFirstElementContainingSubstring('chkLastMonth'));

    var txtStartDate = document.getElementById(GetFirstElementContainingSubstring('txtStartDate'));
    var btnOpenCalendar1 = document.getElementById(GetFirstElementContainingSubstring("btnOpenCalendar1"));
    var txtStartHour = document.getElementById(GetFirstElementContainingSubstring('txtStartHour'));

    var txtEndDate = document.getElementById(GetFirstElementContainingSubstring('txtEndDate'));
    var btnOpenCalendar2 = document.getElementById(GetFirstElementContainingSubstring("btnOpenCalendar2"));
    var txtEndHour = document.getElementById(GetFirstElementContainingSubstring('txtEndHour'));

    txtStartDate.disabled = chkLastMonth.checked;
    btnOpenCalendar1.disabled = chkLastMonth.checked;
    txtStartHour.disabled = chkLastMonth.checked;

    txtEndDate.disabled = chkLastMonth.checked;
    btnOpenCalendar2.disabled = chkLastMonth.checked;
    txtEndHour.disabled = chkLastMonth.checked;

    if (chkLastMonth.checked) ReloadDateRange(txtStartDate, txtStartHour, txtEndDate, txtEndHour);
}

// Recarrega a faixa de datas com o período do último mês
function ReloadDateRange(txtStartDate, txtStartHour, txtEndDate, txtEndHour) {
    var hiddenStartDate = document.getElementById(GetFirstElementContainingSubstring("hiddenStartDate"));
    var hiddenStartHour = document.getElementById(GetFirstElementContainingSubstring("hiddenStartHour"));
    var hiddenEndDate = document.getElementById(GetFirstElementContainingSubstring("hiddenEndDate"));
    var hiddenEndHour = document.getElementById(GetFirstElementContainingSubstring("hiddenEndHour"));

    txtStartDate.value = hiddenStartDate.value;
    txtStartHour.value = hiddenStartHour.value;
    txtEndDate.value = hiddenEndDate.value;
    txtEndHour.value = hiddenEndHour.value;
}
