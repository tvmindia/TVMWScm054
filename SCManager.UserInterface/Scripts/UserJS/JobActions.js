
function BindForm(ID)
{
    try
    {
        var result = GetServiceReportEntryByID(ID);
        if (result) {
            $("#ModelJobNo").val(result.JobNo);
            $("#ModelJobNo").attr({ 'disabled': 'disabled' });
            $("#ModelCustomerName").val(result.CustomerName);
            $("#ModelCustomerLocation").val(result.CustomerLocation);
            $("#ModelServiceType").val(result.ServiceType);
            $("#ModelCallType").val(result.CallType);
            $("#SCCommAmount").val(result.SCCommAmount);
            $("#ModelModelNo").val(result.ModelNo);
            $("#ModelSerialNo").val(result.SerialNo);
            $("#ModelICRNo").val(result.ICRNo);
            $("#ModelTechnicianRemark").val(result.TechnicianRemark);
            //Hiddenfields
            $("#ModelTechEmpID").val(result.Employee.ID);
            $("#ModelJobID").val(result.ID);
            $("#ModelServiceDate").val((result.ServiceDate));
            BindJobNumberDropDown();
            if (result.ServiceType == "RPT") {
                $(".calltypehidden").show();
                $("#ModelRepeat_JobNo").val(result.RepeatJobNo);
                $("#Repeat_EmpID").val(result.Repeat_EmpID);
            }
            else {
                $(".calltypehidden").hide();
            }
            $("#TechnicianLabel").text(result.Employee.Name);
            $("#ServiceDateLabel").text(ConvertJsonToDate(result.ServiceDate));
        }
    }
    catch(e)
    {
        notyAlert('error', e.message);
    }
}
function JobEdit(curobj) {
    try {
        ClearJobForm();
        var rowData = DataTables.DailyService.row($(curobj).parents('tr')).data();
        BindForm(rowData.ID);
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
    $("#ModelJobID").val('');
    $('#AddJobModel').on('shown.bs.modal', function () {
        $('#ModelJobNo').focus()
    })
}
function ResetForm() {
    var jobid = $("#ModelJobID").val();
    $('#jobform')[0].reset();
    if (jobid != '')
    {
        BindForm(jobid);
    }
}

function ValidateJobForm() {
    try {
        var fl = true;
        if (($("#ModelJobNo").val()) && ($("#ModelCustomerName").val()) && ($("#ModelCustomerLocation").val()) && ($("#ModelServiceType").val()) && ($("#ModelCallType").val())) {
            fl = true;
            if ($("#ModelServiceType").val() == "RPT") {
                if (($("#ModelRepeat_JobNo").val()) && ($("#Repeat_EmpID").val())) {
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
                //Duplicate job
                if (JsonResult.Record.Status == "2")
                {
                    notyAlert('error', JsonResult.Record.Message);
                }
                else
                {
                    notyAlert('success', JsonResult.Record.Message);
                    $("#AddJobModel").modal('hide');
                    debugger;
                    if ($("#txtServiceDate").val() == "") {
                        FilterServiceRecord(); //for binding table values (default filter)
                    }
                    else {
                        RefreshDailyServiceTable($("#ModelJobNo").val()); //for binding table values ehen having empid and service date.
                    }
                }               
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


function ServiceTypeOnChange(curobj) {
    try {
        if (curobj.value != "RPT") {
            $(".calltypehidden").hide();
            $("#ModelRepeat_EmpID").val('');
            $("#ModelRepeat_JobNo").val('');
        }
        else {
            $(".calltypehidden").show();
            BindJobNumberDropDown(); 
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

function RepeatJobNumberChange() {
    debugger;
    var jobno = $("#ModelRepeat_JobNo").val();
    var result = GetTechnicianByJobNo(jobno); 
    var empid = result[0].ID; 
    $("#Repeat_EmpID").val(empid); 
    }

function GetTechnicianByJobNo(jobno)
{
    try
    { 
        var data = { "JobNo": jobno };
        var ds = {};
        ds = GetDataFromServer("DailyServiceReport/GetTechnicianByJobNo/", data);
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        if (ds.Result == "OK") {
            return ds.Records;
        }
        if (ds.Result == "ERROR") {
            alert(ds.Message);
        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
} 

function ModelServiceDateOnChange(curobj)
{
    var val = $(curobj).val();
    $("#ServiceDateLabel").text(ConvertJsonToDate(val));
}


