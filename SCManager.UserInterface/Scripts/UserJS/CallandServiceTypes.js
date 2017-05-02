//---------------------------------------Docuement Ready--------------------------------------------------//
$(document).ready(function () {
    try
    {
        FillCallTypes()
        FillServiceTypes()
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
function FillCallTypes() {
    debugger;
   
    var thisType = GetCallTypes(); //Binding Data
    //Hidden

        $("#MajorCommission").val(thisType[1].MajorCommission);
        $("#MinorCommission").val(thisType[3].MinorCommission);
        $("#MandatoryCommission").val(thisType[2].MandatoryCommission);
        $("#RepeatCommission").val(thisType[4].RepeatCommission);
        $("#DemoCommission").val(thisType[0].DemoCommission);
    
}
function FillServiceTypes()
{
    debugger;
    var thisType = GetServiceTypes(); //Binding Data
    $("#AMC1Commission").val(thisType[0].AMC1Commission);
    $("#AMC2Commission").val(thisType[1].AMC2Commission);
}
//---------------------------------------Get Call and service Details-------------------------------------//
function GetCallTypes() {
    try {
        var data = { };
        var ds = {};
        ds = GetDataFromServer("CallandServiceTypes/GetCallTypes/", data);
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        if (ds.Result == "OK") {
            return ds.Record;
        }
        if (ds.Result == "ERROR") {
            alert(ds.Message);
        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

function GetServiceTypes() {
    try {
        var data = {};
        var ds = {};
        ds = GetDataFromServer("CallandServiceTypes/GetServiceTypes/", data);
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        if (ds.Result == "OK") {
            return ds.Record;
        }
        if (ds.Result == "ERROR") {
            alert(ds.Message);
        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}