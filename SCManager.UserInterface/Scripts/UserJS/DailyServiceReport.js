var DataTables = {};
$(document).ready(function () {
    try
    {
        DataTables.DailyService = $('#tblDailyServiceReport').DataTable(
         {
             dom: '<"pull-left"f>rt<"bottom"ip><"clear">',
             order: [],
             searching: true,
             paging: true,
             data: GetAllServiceReportEntries(),
             columns: [
               { "data": "ID", "defaultContent": "<i>-</i>" },
               { "data": "JobNo", "defaultContent": "<i>-</i>" },
               { "data": "CustomerName", "defaultContent": "<i>-</i>" },
               { "data": "CustomerLocation", "defaultContent": "<i>-</i>" },
               { "data": "ServiceType", "defaultContent": "<i>-</i>" },
               { "data": "CallType", "defaultContent": "<i>-</i>" },
               { "data": "ModelNo", "defaultContent": "<i>-</i>" },
               { "data": "SerialNo", "defaultContent": "<i>-</i>" },
               { "data": "CallStatusCode", "defaultContent": "<i>-</i>" },
               { "data": "ICRNo","defaultContent": "<i>-</i>" },
               { "data": "TechnicianRemark", "defaultContent": "<i>-</i>" },
               { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink" onclick="Edit(this)"><i class="fa fa-pencil-square-o" aria-hidden="true"></i>Edit</a><span> | </span><a href="#" class="actionLink" onclick="Delete(this)"><i class="fa fa-trash-o" aria-hidden="true"></i>Remove</a>' }
             ],
             columnDefs: [{ "targets": [0], "visible": false, "searchable": false },
                  { className: "text-left", "targets": [1,2,3,4,5,6,7,8,9,10] },
                  { className: "text-center", "targets": [] },
                  { className: "text-right", "targets": [] }
                    
                   
             ]
         });
    }
    catch(e)
    {
     
    }
});


function Edit(curobj)
{
    try
    {
        var rowData = DataTables.DailyService.row($(curobj).parents('tr')).data();
        var result = GetServiceReportEntryByID(rowData.ID);
        ClearJobForm();
        if (result)
        {
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
    catch(e)
    {
        notyAlert('error', e.message);
    }
}

function Delete(curobj) {
    try
    {
        var r = confirm("Are you sure?");
        if (r == true)
        {
            var rowData = DataTables.DailyService.row($(curobj).parents('tr')).data();
            var JobViewModel = new Object();
            JobViewModel.ID = rowData.ID;
            var data = "{'job':" + JSON.stringify(JobViewModel) + "}";
            PostDataToServer('DailyServiceReport/DeleteTechnicianJob/', data, function (JsonResult) {
                if (JsonResult != '') {
                    switch (JsonResult.Result) {
                        case "OK":
                            notyAlert('success', JsonResult.Record.Message);
                            RefreshDailyServiceTable();
                            break;
                        case "ERROR":
                            notyAlert('error', JsonResult.Record.Message);
                            break;
                        default:
                            break;
                    }
                }
            })
        }
        else
        {
            
        }
    }
    catch (e)
    {
        notyAlert('error', e.message);
    }
}


function RefreshDailyServiceTable() {
    try {
     DataTables.DailyService.clear().rows.add(GetAllServiceReportEntries()).draw(false);
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

function GetAllServiceReportEntries() {
    try {
       
        var data = {};
        var ds = {};
        ds = GetDataFromServer("DailyServiceReport/GetAllServiceReports/", data);
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

function ClearJobForm()
{
    $('#jobform')[0].reset();
}

function ValidateJobForm()
{
    try
    {
        var fl = true;
        if (($("#JobNo").val()) && ($("#CustomerName").val()) && ($("#CustomerLocation").val()) && ($("#ServiceType").val()) && ($("#CallType").val()))
        {
            fl = true;
            if ($("#CallType").val() == "Repeat")
            {
                if (($("#Repeat_JobNo").val()) && ($("#txtEmployee").val())) {
                    fl = true;
                }
                else
                {
                    fl = false;
                    notyAlert('error', 'Mandatory fields are empty!');
                }
            }
            
        }
        else
        {
            fl = false;
            notyAlert('error', 'Mandatory fields are empty!');
        }
        
        return fl;
    }
    catch(e)
    {
        notyAlert('error', e.message);
    }
}


function AddTechnicanJob()
{
    var techi = $("#EmpSelector").val();
    var serdat = $("#txtServiceDate").val();
    if ((techi) && (serdat))
    {
        $("#AddJobModel").modal('show');
        $("#TechnicianLabel").text($("#EmpSelector option:selected").text());
        $("#ServiceDateLabel").text(serdat);
        ClearJobForm();
        $(".calltypehidden").hide();
    }
    else
    {
        notyAlert('error', 'Please Choose Technician and Service Date');
    }
   
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

function ServiceDateOnChange(curobj)
{
    try
    {
     $("#ServiceDate").val($(curobj).val())
    }
    catch(e)
    {
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
    $("#Repeat_EmpID").val(itemID);
}

function TechnicianSelectOnChange(curobj)
{
    try
    {
        var v = $(curobj).val();
        $("#EmpID").val(v);
    }
    catch(e)
    {
        notyAlert('error', e.Message);
    }
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
            document.getElementById('EmployeeList').innerHTML = '';
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


