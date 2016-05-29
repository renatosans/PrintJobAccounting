
// Função que verifica se uma string está vazia ou é nula
String.IsNullOrEmpty = function(value) {
    var isNullOrEmpty = true;

    if (value) {
        if (typeof (value) == 'string') {
            if (value.length > 0)
                isNullOrEmpty = false;
        }
    }

    return isNullOrEmpty;
}

// Função que retorna o ID(Elemento) que contenha a sequência de caracteres fornecida
function GetFirstElementContainingSubstring(substring) {
    var allElements = document.all || document.getElementsByTagName('*');

    for (var ndx = 0; ndx < allElements.length; ndx++) {
        var elementId = allElements[ndx].id;
        if (elementId) {
            if (elementId.indexOf(substring) != -1) {
                return elementId;
            }
        }
    }
}
