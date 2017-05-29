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
             data: null,
             columns: [
               { "data": "ID", "defaultContent": "<i>-</i>" },
               { "data": "JobNo", "defaultContent": "<i>-</i>" },
               { "data": "CustomerName", "defaultContent": "<i>-</i>" },
               { "data": "CustomerLocation", "defaultContent": "<i>-</i>" },
               { "data": "ServiceType", "defaultContent": "<i>-</i>" },
               { "data": "CallType", "defaultContent": "<i>-</i>" },
               { "data": "ModelNo", "defaultContent": "<i>-</i>" },
               { "data": "SerialNo", "defaultContent": "<i>-</i>" },
               { "data": "CallStatusDescription", "defaultContent": "<i>-</i>" },
               { "data": "ICRNo","defaultContent": "<i>-</i>" },
               { "data": "TechnicianRemark", "defaultContent": "<i>-</i>" },
               { "data": null, "orderable": false, "defaultContent": '<a data-toggle="tp" data-placement="top" data-delay={"show":2000, "hide":3000} title="Edit Job" href="#" class="actionLink" onclick="JobEdit(this)"><i class="glyphicon glyphicon-edit" aria-hidden="true"></i></a>' },
               { "data": null, "orderable": false, "defaultContent": '<a data-toggle="tp" data-placement="top" data-delay={"show":2000, "hide":3000} title="Delete Job" href="#" class="DeleteLink" onclick="JobDelete(this)"><i class="glyphicon glyphicon-trash" aria-hidden="true"></i></a>' }
             ],
             columnDefs: [{ "targets": [0], "visible": false, "searchable": false },
                  { className: "text-left", "targets": [1,2,3,4,5,6,7,9,10] },
                  { className: "text-center", "targets": [8,11,12] },
                  { className: "text-right", "targets": [] }
           ]
         });
      
    }
    catch(e)
    {
     
    }

    try {

        DataTables.DailyServiceReportSummary = $('#tblServiceReportSummary').DataTable(
         {
             dom: '<"pull-left"f>rt<"bottom"ip><"clear">',
             order: [],
             searching: true,
             paging: true,
             data: null,
             columns: [
               { "data": "ServiceDate",render: function (data, type, row) { return ConvertJsonToDate(data); }, "defaultContent": "<i>-</i>" },
               { "data": "Technician", "defaultContent": "<i>-</i>" },
               { "data": "TotalCalls", "defaultContent": "<i>-</i>" },
               { "data": "MinorCalls", "defaultContent": "<i>-</i>" },
               { "data": "MajorCalls", "defaultContent": "<i>-</i>" },
               { "data": "MandatoryCalls", "defaultContent": "<i>-</i>" },
               { "data": "DemoCalls", "defaultContent": "<i>-</i>" },
               { "data": "RepeatCalls", "defaultContent": "<i>-</i>" }
              
             ],
             columnDefs: [
                  { className: "text-left", "targets": [1] },
                  { className: "text-center", "targets": [0,2,3,4,5,6,7] }
                 
             ]
         });

    }
    catch (e) {

    }
});



function JobDelete(curobj) {
    try
    {
    var rowData = DataTables.DailyService.row($(curobj).parents('tr')).data();
    notyConfirm('Are you sure to delete?', 'Delete("' + rowData.ID + '")', '', "Yes, delete it!");
    }
    catch (e)
    {
        notyAlert('error', e.message);
    }
}

function Delete(ID)
{
    try
    {
        var JobViewModel = new Object();
        JobViewModel.ID = ID;
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
    catch(e)
    {
        notyAlert('error', e.message);
    }
}


function RefreshDailyServiceTable(jobno) {
    try {
        var empid = $("#EmpSelector").val();
        var serdate = $("#txtServiceDate").val();
        if ((empid) && (serdate) && (DataTables.DailyService!=undefined))
        {
            DataTables.DailyService.clear().rows.add(GetAllServiceReportEntries(empid, serdate)).draw(false);
            $('[data-toggle="tp"]').tooltip({ container: 'body' });
        }
      
    }
    catch (e) {
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
        $('#AddJobModel').on('shown.bs.modal', function () {
            $('#ModelJobNo').focus()
        })
        $("#modelContextLabel").text('Add Job');
        $("#ModelJobNo").removeAttr('disabled');
     

        $("#TechnicianLabel").text($("#EmpSelector option:selected").text());
        $("#ServiceDateLabel").text(ConvertJsonToDate(serdat));
        ClearJobForm();
        $(".calltypehidden").hide();
    }
    else
    {
        notyAlert('error', 'Please Choose Technician and Service Date');
    }
   
}

function ServiceDateOnChange(curobj)
{
    try
    {
      
        $("#ModelServiceDate").val($(curobj).val())
        RefreshDailyServiceTable();
    }
    catch(e)
    {
     notyAlert('error', e.message);
    }
}

function TechnicianSelectOnChange(curobj)
{
    try
    {
        var v = $(curobj).val();
        $("#ModelTechEmpID").val(v);
        $("#txtServiceDate").val($("#hdfcurrentdate").val());
        $("#ModelServiceDate").val($("#hdfcurrentdate").val())
        RefreshDailyServiceTable();
    }
    catch(e)
    {
        notyAlert('error', e.Message);
    }
}


function GetAllServiceReportEntries(id,date) {
    try {

        var data = {"ID":id,"ServieDate":date};
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

function RefreshServiceReportSummaryTable() {
    try {
        var serdate = $("#txtServiceDate2").val();
        if ((serdate) && (DataTables.DailyServiceReportSummary != undefined)) {
            DataTables.DailyServiceReportSummary.clear().rows.add(GetServiceRegisterSummary(serdate)).draw(false);
        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}
function GetServiceRegisterSummary(date) {
    try
    {
        var data = { "ServiceDate": date };
        var ds = {};
        ds = GetDataFromServer("DailyServiceReport/GetServiceRegistrySummary/", data);
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




