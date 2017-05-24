function JobEdit(curobj) {
    try {
        var rowData = DataTables.DailyService.row($(curobj).parents('tr')).data();
        var result = GetServiceReportEntryByID(rowData.ID);
        ClearJobForm();
        if (result) {
            $("#JobNo").val(result.JobNo);
            $("#CustomerName").val(result.CustomerName);
            $("#CustomerLocation").val(result.CustomerLocation);
            $("#ServiceType").val(result.ServiceType);
            $("#CallType").val(result.CallType);
            $("#Repeat_JobNo").val(result.Repeat_JobNo);
            $("#txtEmployee").val(result.Repeat_EmpID);
            $("#ModelNo").val(result.ModelNo);
            $("#SerialNo").val(result.SerialNo);
            $("#ICRNo").val(result.ICRNo);
            $("#TechnicianRemark").val(result.TechnicianRemark);

            // $("#TechnicianLabel").text(result.);
            // $("#ServiceDateLabel").text(serdat);
        }
        $("#AddJobModel").modal('show');


        // $(".calltypehidden").hide();
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
        if (($("#JobNo").val()) && ($("#CustomerName").val()) && ($("#CustomerLocation").val()) && ($("#ServiceType").val()) && ($("#CallType").val())) {
            fl = true;
            if ($("#CallType").val() == "Repeat") {
                if (($("#Repeat_JobNo").val()) && ($("#txtEmployee").val())) {
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
        $("#Repeat_EmpID").val(itemID);
    }
    catch(e)
    {

    }
    
}