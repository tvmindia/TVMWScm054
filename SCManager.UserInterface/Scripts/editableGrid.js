$(document).ready(function () {

    EG_KeyDown();
   

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


function ValidateCombo(obj,source) {
    if (datalistValidator(obj.value, source)) {
        obj.className = obj.className.replace(' EGDanger', '');
        return true;
    }
    else {
        if (obj.value != "") {
            obj.value = "";
            obj.className = obj.className + ' EGDanger'
        }
       
        return false;
    }
}
 

function ValidateText(obj, type) {
  
    if (type == 'N') {
        if (!isNaN(parseInt(obj.value))) {
            obj.value = parseInt(obj.value);
            obj.className = obj.className.replace(' EGDanger', '');
            return true;
        }
        else {
            if (obj.value != "") {
                obj.value = "";
                obj.className = obj.className + ' EGDanger'
            }
            return false;
        }

    }
    else if (type == 'F') {
        if (!isNaN(parseFloat(obj.value)))
        {
            obj.value = parseFloat(obj.value);
            obj.className = obj.className.replace(' EGDanger', '');
            return true;
        }
        else {
            if (obj.value != "") {
                obj.value = "";
                obj.className = obj.className + ' EGDanger'
            }
            return false;
        }
    }
    else { }
}

//-----------------------------------------------------------






//--2----------data change source-------------------------------

function EG_Validate_changeData_Text(obj, type, row, column, relatedfn) {

   var flag= ValidateText(obj, type);

    if (EG_changeData(obj.value, row, column)) {
        currBoxIndx = $('.gridTextbox').index(obj);;
        eval(relatedfn + "(" + row + ")");
        if (flag) {
            EG_SetFocus_Next();
        }
        else {
            EG_SetFocus();
        }
    }

}

function EG_Validate_changeData_Combo(obj,type, row, column, source, relatedfn) {
     
    var flag = ValidateCombo(obj, source);

    if (EG_changeData(obj.value, row, column)) {
        currBoxIndx = $('.gridTextbox').index(obj);
        
        eval(relatedfn + "(" + row + ")");
        if (flag) {
            EG_SetFocus_Next();
        }
        else {
            EG_SetFocus();
        }
    }
}

function EG_changeData(value, row, column) {
 
  //  EG_GridData[row-1][column] = value;
    for (i = 0; i < EG_GridData.length; i++) {
        if (EG_GridData[i][EG_SlColumn] == row) {
            if (EG_GridData[i][column] != value)
            {
                EG_GridData[i][column] = value;

                if (i == EG_GridData.length-1) {
                    EG_AddBlankRowsWithoutRebind(1);
                }

                return true;
            }
            return false;
        }
    }

}
//------------------------------------------------------------------------------


//--3-----------combo source binding----------------------------
function EG_ComboSource(id, values,valueCol,textCol) {
    if (document.getElementById(id) == null || document.getElementById(id) == 'undefined') {
        alert("combo source element is not defined in cshtml");
    }

    var options = '';
    for (var i = 0; i < values.length; i++)
        options += '<option value="' + values[i][valueCol] + '" >' + values[i][textCol] + '</option>';

    document.getElementById(id).innerHTML = options;
   
    //
}
//---------------------------------------------------------------


//--4------------------create controlls------------------------------
function EG_createTextBox(data, type, row, columnname,relatedfn) {

    var a = row[columnname];
    var b = row.SlNo;
    var c = "'";
    var align = ''
    if (type == 'N' || type == 'F') {
        align='style="text-align:right"'
    }
    // debugger;
    if (data == "" || data == null) {

        return ('<input ' + align + ' onfocus="this.select();" type="textbox" class="gridTextbox" value="" onblur="EG_Validate_changeData_Text(this,' + c + type + c + ',' + b + ',' + c + columnname + c + ',' + c + relatedfn + c + ') " >  </input>');
    } else {
        return ('<input ' + align + ' onfocus="this.select();" type="textbox" class="gridTextbox" value=' + data + ' onblur="EG_Validate_changeData_Text(this,' + c + type + c + ',' + b + ',' + c + columnname + c + ',' + c + relatedfn + c + ') " >  </input>');
    }


}



function EG_createCombo(data, type, row, columnname, Source, relatedfn) {
    var a = row[columnname];
    var b = row.SlNo;
    var c = "'";
    if (data == "" || data == null) {
        return ('<input class="gridTextbox" list="' + Source + '" name="' + columnname + '" onblur="EG_Validate_changeData_Combo(this,'+ c + type + c +',' + b + ',' + c + columnname + c + ',' + c + Source + c + ',' + c + relatedfn + c + ') " >');
    }
    else {
        return ('<input class="gridTextbox" list="' + Source + '" name="' + columnname + '" value=' + data + ' onblur="EG_Validate_changeData_Combo(this,' + c + type + c + ',' + b + ',' + c + columnname + c + ',' + c + Source + c + ',' + c + relatedfn + c + ') " >');

    }
 
}

//---------------------------------------------------------------------------------

function EG_Rebind() {
    if (EG_GridDataTable == null) {
        alert("EG_GridDataTable not defined in document ready");
        return;
    }
    EG_GridDataTable.clear().rows.add(EG_GridData).draw(false);
   
}


function EG_ClearTable() {
    EG_totalDetailRows = 0;
    EG_GridDataTable.clear();
    EG_GridData = [];

}


//-------------------------blank rows-----------------------------
function EG_AddBlankRowsWithoutRebind(count) {

    for (i = 0; i < count; i++) {
        var tempObj = EG_TableDefn()
        tempObj[EG_SlColumn] = EG_totalDetailRows + i + 1;
        EG_GridData.push(tempObj);
    }

    EG_totalDetailRows = EG_totalDetailRows + count;
}


function EG_AddBlankRows(count) {

    for (i = 0; i < count; i++) {
        var tempObj = EG_TableDefn()
        tempObj[EG_SlColumn] = EG_totalDetailRows + i + 1;
        EG_GridData.push(tempObj);
    }

    EG_totalDetailRows = EG_totalDetailRows + count;
    EG_Rebind()
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

//--------------------------blank rows-----------------------------------




function EG_SetFocus_Next() {
  
    $('.gridTextbox').eq(currBoxIndx + 1).focus();
    EG_KeyDown();
}

function EG_SetFocus() {

    $('.gridTextbox').eq(currBoxIndx ).focus();
    EG_KeyDown();
}

function roundoff(num) {
    return (Math.round(num * 100) / 100).toFixed(2);
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