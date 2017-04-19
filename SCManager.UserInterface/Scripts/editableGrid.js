$(document).ready(function () {

    EG_KeyDown()

});

var currBoxIndx;


//--1----------------data validations-------------------------
function datalistValidator(value, source) {
    var obj = $("#" + source).find("option[value='" + value + "']");
    if (obj != null && obj.length > 0) {
        return true
    }
    return false;
}

//-----------------------------------------------------------






//--2----------data change source-------------------------------

function EG_Validate_changeData_Text(obj, row, column, relatedfn) {
    if (EG_changeData(obj.value, row, column)) {
        currBoxIndx = $('.gridTextbox').index(obj);;
        eval(relatedfn + "(" + row + ")");       
    }

}

function EG_Validate_changeData_Combo(obj, row, column, source, relatedfn) {
    if (datalistValidator(obj.value, source)) {
        obj.className = obj.className.replace(' EGDanger', '');
    }
    else {
        obj.value = "";
        obj.className = obj.className + ' EGDanger'
    }

    if (EG_changeData(obj.value, row, column)) {
        currBoxIndx = $('.gridTextbox').index(obj);;
        eval(relatedfn + "(" + row + ")");
    }
}

function EG_changeData(value, row, column) {
     
  //  EG_GridData[row-1][column] = value;
    for (i = 0; i < EG_GridData.length; i++) {
        if (EG_GridData[i][EG_SlColumn] == row) {
            if (EG_GridData[i][column] != value)
            {
                EG_GridData[i][column] = value;
                return true;
            }
            return false;
        }
    }

}
//------------------------------------------------------------------------------


//--3-----------combo source binding----------------------------
function EG_ComboSource(id, values) {
    var options = '';
    for (var i = 0; i < values.length; i++)
        options += '<option value="' + values[i] + '" />';

    document.getElementById(id).innerHTML = options;
    //
}
//---------------------------------------------------------------


//--4------------------create controlls------------------------------
function EG_createTextBox(data, type, row, columnname,relatedfn) {

    var a = row[columnname];
    var b = row.SlNo;
    var c = "'";
    // debugger;
    if (data == "" || data == null) {

        return ('<input type="textbox" class="gridTextbox" value="" onblur="EG_Validate_changeData_Text(this,' + b + ',' + c + columnname + c + ',' + c + relatedfn + c + ') " >  </input>');
    } else {
        return ('<input type="textbox" class="gridTextbox" value=' + data + ' onblur="EG_Validate_changeData_Text(this,' + b + ',' + c + columnname + c + ',' + c + relatedfn + c + ') " >  </input>');
    }


}



function EG_createCombo(data, type, row, columnname, Source, relatedfn) {
    var a = row[columnname];
    var b = row.SlNo;
    var c = "'";
    if (data == "" || data == null) {
        return ('<input class="gridTextbox" list="' + Source + '" name="' + columnname + '" onblur="EG_Validate_changeData_Combo(this,' + b + ',' + c + columnname + c + ',' + c + Source + c + ',' + c + relatedfn + c + ') " >');
    }
    else {
        return ('<input class="gridTextbox" list="' + Source + '" name="' + columnname + '" value=' + data + ' onblur="EG_Validate_changeData_Combo(this,' + b + ',' + c + columnname + c + ',' + c + Source + c + ',' + c + relatedfn + c + ') " >');

    }
 
}

//---------------------------------------------------------------------------------


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


function EG_SetFocus() {
    debugger;
    $('.gridTextbox').eq(currBoxIndx + 1).focus();
    EG_KeyDown();
}


function EG_KeyDown() {

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
}