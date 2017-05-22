$(document).ready(function () {

   // 
});

function AddTechnicanJob()
{
    $("#AddJobModel").modal('show');
    $(".calltypehidden").hide();
}

function SaveTechnicanJob()
{
    $('#btnJobSave').trigger('click');
    
}

function JobSaveSuccess(data, status, xhr)
{
    try {
        var JsonResult = JSON.parse(data)
        switch (JsonResult.Result) {
            case "OK":
                notyAlert('success', JsonResult.Record.Message);
                break;
            case "VALIDATION":
                notyAlert('error', JsonResult.Message);
                break;
            case "ERROR":
                notyAlert('error', JsonResult.Message);
                break;
            default:
                notyAlert('error', JsonResult.Message);
                break;
        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

function CallTypeOnChange(curobj)
{
    try
    {
        if (curobj.value != "Repeat")
        {
            $(".calltypehidden").hide();
        }
        else
        {
            $(".calltypehidden").show();
            BindJobNumberDropDown();
            BindTechnicianDropDown();
        }
        
    }
    catch(e)
    {
        notyAlert('error', e.Message);
    }
}

function TechnicianOnChange(i)
{
    var val=$(i).val();
    var a = $("#EmployeeList").find('option[value="' + val + '"]');
    var itemID = a.attr('id');
    $("#hdfEmpID").val(itemID);
 }

function BindTechnicianDropDown()
{
    try
    {
        var data = {};
        var ds = {};
        ds = GetDataFromServer("DailyServiceReport/GetAllTechnicianForServiceTypeDropDown/", data);
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        if (ds.Result == "OK") {
            var options = '';
            $.each(ds.Records, function (key, value) {
                 //$("#RepeatJobNo").append($("<option></option>").val(value.ID).html(value.JobNo));
                options += '<option id="' + value.ID + '" value="' + value.Name + '" >' + '</option>';
            });
            document.getElementById('EmployeeList').innerHTML = options;
        }
        if (ds.Result == "ERROR") {
            notyAlert('error', ds.Message);
        }
    }
    catch(e)
    {
        notyAlert('error', e.Message);
    }
}

function BindJobNumberDropDown() {
   
    try {
    
        var data = {};
        var ds = {};
        ds = GetDataFromServer("DailyServiceReport/GetJobNumbersForDropDown/", data);
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        if (ds.Result == "OK")
        {
            var options = '';
            $.each(ds.Records, function (key, value) {
             //$("#RepeatJobNo").append($("<option></option>").val(value.ID).html(value.JobNo));
                options += '<option id="' + value.ID + '" value="' + value.JobNo + '" >' + '</option>';
            });
            document.getElementById('jobnumberList').innerHTML = options;
        }
        if (ds.Result == "ERROR") {
            notyAlert('error', ds.Message);
        }

    }
    catch (e) {
        notyAlert('error', e.Message);
    }
}


