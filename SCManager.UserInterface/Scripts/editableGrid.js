$(document).ready(function () {

    $('.gridTextbox').keydown(function (e) {
        try {
            if (e.which === 13) {
                var index = $('.gridTextbox').index(this) + 1;
                $('.gridTextbox').eq(index).focus();
            }
            else {

                switch (e.which) {
                    case 37: // left GridInputPerRow
                        var index = $('.gridTextbox').index(this) - 1;
                        $('.gridTextbox').eq(index).focus();
                        break;

                    case 38: // up
                        var index = $('.gridTextbox').index(this) - EG_GridInputPerRow;
                        $('.gridTextbox').eq(index).focus();
                        break;

                    case 39: // right
                        var index = $('.gridTextbox').index(this) + 1;
                        $('.gridTextbox').eq(index).focus();
                        break;

                    case 40: // down
                        var index = $('.gridTextbox').index(this) + EG_GridInputPerRow;
                        $('.gridTextbox').eq(index).focus();
                        break;

                    default: return; // exit this handler for other keys
                }

            }
        } catch (x) { }



    });

});



function EG_changeData(value, row, column) {
     debugger;
  //  EG_GridData[row-1][column] = value;
    for (i = 0; i < EG_GridData.length; i++) {
        if (EG_GridData[i][EG_SlColumn] == row) {
            EG_GridData[i][column] = value;
            break;
        }
    }

}

function datalistValidator(value,source) {
    var obj = $("#" +source).find("option[value='" + value + "']");
    if (obj != null && obj.length > 0) {
        
        return true
    }
     
    return false;
}

function EG_Validate_changeData(obj, row, column,source) {
    if (datalistValidator(obj.value, source))
    {
        obj.className = obj.className.replace(' EGDanger', '');
    }
    else {
        obj.value = "";
        obj.className = obj.className+ ' EGDanger'
    }

    EG_changeData(obj.value, row, column)
}

function EG_createTextBox(data, type, row, columnname) {

    var a = row[columnname];
    var b = row.SlNo;
    var c = "'";
    // debugger;
    if (data == "" || data == null) {

        return ('<input type="textbox" class="gridTextbox" value="" onblur="EG_changeData(this.value,' + b + ',' + c + columnname + c + ')" >  </input>');
    } else {
        return ('<input type="textbox" class="gridTextbox" value=' + data + ' onblur="EG_changeData(this.value,' + b + ',' + c + columnname + c + ')" >  </input>');
    }


}

function EG_ComboSource(id, values) {
    var options='';
    for (var i = 0; i < values.length; i++)
        options += '<option value="' + values[i] + '" />';
   
    document.getElementById(id).innerHTML = options;
    //
}

function EG_createCombo(data, type, row, columnname,Source) {
    var a = row[columnname];
    var b = row.SlNo;
    var c = "'";
    return ('<input class="gridTextbox" list="' + Source + '" name="' + columnname + '" onblur="EG_Validate_changeData(this,' + b + ',' + c + columnname + c + ','+c+Source+c+')" >');

 
}

function EG_blankRow(count) {

    var dataObj = [];
    for (i = 0; i < count; i++) {

        var tempObj = EG_TableDefn()
        tempObj[EG_SlColumn] = EG_totalDetailRows + i + 1;
        dataObj.push(tempObj);

    }

    EG_totalDetailRows = EG_totalDetailRows + count;
    return dataObj;

}