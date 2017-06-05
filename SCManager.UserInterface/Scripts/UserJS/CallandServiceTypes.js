//---------------------------------------Docuement Ready--------------------------------------------------//
$(document).ready(function () {
    try
    {
     
        FillServiceTypes()
    }
    catch (e) {
        notyAlert('error', e.message);
    }
});

//------------------------------- Employee Save-----------------------------//
function save() {
    

    $("#btnUpdateCallandServiceType").trigger('click');
}
function CallandServiceTypesSaveSuccess(data, status) {
   
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

function FillServiceTypes()
{
   
    var thisType = GetServiceTypes(); //Binding Data
    if (thisType)
    {
    $("#AMC1Commission").val(roundoff(thisType.AMC1Commission));
    $("#AMC2Commission").val(roundoff(thisType.AMC2Commission));
    $("#MajorCommission").val(roundoff(thisType.MajorCommission));
    $("#MinorCommission").val(roundoff(thisType.MinorCommission));
    $("#MandatoryCommission").val(roundoff(thisType.MandatoryCommission));
    $("#RepeatCommission").val(roundoff(thisType.RepeatCommission));
    $("#DemoCommission").val(roundoff(thisType.DemoCommission));
    }
  
}
//---------------------------------------Get Call and service Details-------------------------------------//


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
            notyAlert('error', ds.Message);
            
        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}