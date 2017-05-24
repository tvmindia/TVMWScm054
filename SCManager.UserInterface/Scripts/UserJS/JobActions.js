function JobEdit(curobj) {
    try {
        var rowData = DataTables.DailyService.row($(curobj).parents('tr')).data();
        var result = GetServiceReportEntryByID(rowData.ID);
        ClearJobForm();
        if (result) {
            $("#ModelJobNo").val(result.JobNo);
            $("#ModelCustomerName").val(result.CustomerName);
            $("#ModelCustomerLocation").val(result.CustomerLocation);
            $("#ModelServiceType").val(result.ServiceType);
            $("#ModelCallType").val(result.CallType);
           
         
            $("#ModelModelNo").val(result.ModelNo);
            $("#ModelSerialNo").val(result.SerialNo);
            $("#ModelICRNo").val(result.ICRNo);
            $("#ModelTechnicianRemark").val(result.TechnicianRemark);
            //Hiddenfields
            $("#ModelTechEmpID").val(result.Employee.ID);
            $("#ModelJobID").val(result.ID);
            BindTechnicianDropDown();
            BindJobNumberDropDown();
            if(result.CallType=="Repeat")
            {
                $(".calltypehidden").show();
                $("#ModelRepeat_JobNo").val(result.RepeatJobNo);
                $("#ModelEmployee").val(result.RepeatEmpName);

            }
            else
            {
                $(".calltypehidden").hide();
            }
          
            $("#TechnicianLabel").text('Name: ' + result.Employee.Name);
            $("#ServiceDateLabel").text(result.ServiceDate);
        }
        $("#modelContextLabel").text('Edit Job');
        
        $("#AddJobModel").modal('show');


        
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

function GetServiceReportEntryByID(ID) {
    try {

        var data = { "ID": ID };
        var ds = {};
        ds = GetDataFromServer("DailyServiceReport/GetDailyJobByID/", data);
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


function ClearJobForm() {
    $('#jobform')[0].reset();

}

function ValidateJobForm() {

    try {
        var fl = true;
        if (($("#ModelJobNo").val()) && ($("#ModelCustomerName").val()) && ($("#ModelCustomerLocation").val()) && ($("#ModelServiceType").val()) && ($("#ModelCallType").val())) {
            fl = true;
            if ($("#ModelCallType").val() == "Repeat") {
                if (($("#ModelRepeat_JobNo").val()) && ($("#ModelEmployee").val())) {
                    fl = true;
                }
                else {
                    fl = false;
                    notyAlert('error', 'Mandatory fields are empty!');
                }
            }

        }
        else {
            fl = false;
            notyAlert('error', 'Mandatory fields are empty!');
        }

        return fl;
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

function SaveTechnicanJob() {
    $('#btnJobSave').trigger('click');

}

function JobSaveSuccess(data, status, xhr) {
    try {
        var JsonResult = JSON.parse(data)
        switch (JsonResult.Result) {
            case "OK":
                notyAlert('success', JsonResult.Record.Message);
                $("#AddJobModel").modal('hide');
                RefreshDailyServiceTable();
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


function CallTypeOnChange(curobj) {
    try {
        if (curobj.value != "Repeat") {
            $(".calltypehidden").hide();
        }
        else {
            $(".calltypehidden").show();
            BindJobNumberDropDown();
            BindTechnicianDropDown();
        }

    }
    catch (e) {
        notyAlert('error', e.Message);
    }
}


function BindTechnicianDropDown() {
    try {
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
            document.getElementById('EmployeeList').innerHTML = '';
            document.getElementById('EmployeeList').innerHTML = options;
        }
        if (ds.Result == "ERROR") {
            notyAlert('error', ds.Message);
        }
    }
    catch (e) {
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
        if (ds.Result == "OK") {
            var options = '';
            $.each(ds.Records, function (key, value) {
                //$("#RepeatJobNo").append($("<option></option>").val(value.ID).html(value.JobNo));
                options += '<option id="' + value.ID + '" value="' + value.JobNo + '" >' + '</option>';
            });
            document.getElementById('jobnumberList').innerHTML = '';
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

function TechnicianOnChange(i) {
    try
    {
        var val = $(i).val();
        var a = $("#EmployeeList").find('option[value="' + val + '"]');
        var itemID = a.attr('id');
        $("#ModelRepeat_EmpID").val(itemID);
    }
    catch(e)
    {

    }
    
}

function ModelServiceDateOnChange(curobj)
{
    var val = $(curobj).val();
    $("#ServiceDateLabel").text(val);
}