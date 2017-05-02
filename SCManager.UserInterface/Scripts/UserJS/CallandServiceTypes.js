//---------------------------------------Docuement Ready--------------------------------------------------//
$(document).ready(function () {
    try
    {
        FillCallandServiceTypes()
    }
    catch (e) {
        notyAlert('error', e.message);
    }
});

//------------------------------- Employee Save-----------------------------//
function save() {
    debugger;

    $("#btnUpdateCallandServiceType").trigger('click');
}
function CallandServiceTypesSaveSuccess(data, status) {
    debugger;
    var JsonResult = JSON.parse(data)
    switch (JsonResult.Result) {
        case "OK":
          
              //  FillCallandServiceTypes();

            notyAlert('success', JsonResult.Records.Message);
            break;
        case "ERROR":
            notyAlert('error', "Error!");
            break;
        default:
            notyAlert('error', JsonResult.Message);
            break;
    }
}
//---------------------------------------Fill Employee--------------------------------------------------//
function FillCallandServiceTypes() {
    debugger;
   
    var thisType = GetCallandServiceTypes(); //Binding Data
    //Hidden

        $("#AMC1Commission").val(thisType[0].AMC1Commission);
        $("#AMC2Commission").val(thisType[0].AMC2Commission);
        $("#MajorCommission").val(thisType[1].MajorCommission);
        $("#MinorCommission").val(thisType[1].MinorCommission);
        $("#MandatoryCommission").val(thisType[1].MandatoryCommission);
        $("#RepeatCommission").val(thisType[1].RepeatCommission);
        $("#DemoCommission").val(thisType[1].DemoCommission);
    
}
//---------------------------------------Get Employee Details By ID-------------------------------------//
function GetCallandServiceTypes() {
    try {
        var data = { };
        var ds = {};
        ds = GetDataFromServer("CallandServiceTypes/GetCallandServiceTypes/", data);
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        if (ds.Result == "OK") {
            return ds.call,ds.service;
        }
        if (ds.Result == "ERROR") {
            alert(ds.Message);
        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}