function msgbox(mensagem) {
    $('#conteudoMensagem').html(mensagem);
    $('#alertModal').modal('show');
}

function checkDate(field) {
    
    var allowBlank = true;
    var minYear = 1902;
    //var maxYear = (new Date()).getFullYear();
    var maxYear = 2060;

    var errorMsg = "";

    // regular expression to match required date format
    re = /^(\d{1,2})\/(\d{1,2})\/(\d{4})$/;

    if ($('#'+field).val() != '') {
        if (regs = $('#' + field).val().match(re)) {
            if (regs[1] < 1 || regs[1] > 31) {
                errorMsg = "Valor inválido para o dia: " + regs[1];
            } else if (regs[2] < 1 || regs[2] > 12) {
                errorMsg = "Valor inválido para o mês: " + regs[2];
            } else if (regs[3] < minYear || regs[3] > maxYear) {
                errorMsg = "Valor Inválido para o ano: " + regs[3] + " - deve estar entre " + minYear + " e " + maxYear;
            }
        } else {
            errorMsg = "Formato inválido de dado: " + field.value;
        }
    } else if (!allowBlank) {
        errorMsg = "Datas vazias não são permitidas!";
    }

    if (errorMsg != "") {
        msgbox(errorMsg);
        $('#' + field).focus();
        return false;
    }

    return true;
}