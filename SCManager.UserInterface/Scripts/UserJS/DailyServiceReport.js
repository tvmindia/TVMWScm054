var DataTables = {};
$(document).ready(function () {
    try
    {
      
        DataTables.DailyService = $('#tblDailyServiceReport').DataTable(
         {
             dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
             buttons: [{
                 extend: 'excel',
                 exportOptions:
                              {
                                  columns: [2, 3, 4, 5, 6, 7, 8, 9, 10, 12]
                              }
             }],
             order: [],
             searching: true,
             paging: true,
             pageLength: 50,
             language: {
                 search: "_INPUT_",
                 searchPlaceholder: "Search"
             },
             data: GetServicefilterbyDays(true),
             columns: [
               { "data": "ID", "defaultContent": "<i>-</i>" },
                  { "data": "Employee.Name", "defaultContent": "<i>-</i>" },
                   { "data": "ServiceDateformatted", "defaultContent": "<i>-</i>" },
               { "data": "JobNo", "defaultContent": "<i>-</i>" },
               { "data": "CustomerName", "defaultContent": "<i>-</i>" },
               { "data": "CustomerLocation", "defaultContent": "<i>-</i>" },
               { "data": "MobileNumber", "defaultContent": "<i>-</i>" },
               { "data": "ServiceTypeDescription", "defaultContent": "<i>-</i>" },
               { "data": "JobCallTypeDescription", "defaultContent": "<i>-</i>" },
               { "data": "ModelNo", "defaultContent": "<i>-</i>" },
               { "data": "SerialNo", "defaultContent": "<i>-</i>" },
               { "data": "CallStatusDescription", "defaultContent": "<i>-</i>" },
               { "data": "TechnicianRemark", "defaultContent": "<i>-</i>" },
               { "data": null, "orderable": false, "defaultContent": '<a data-toggle="tp" data-placement="top" data-delay={"show":2000, "hide":3000} title="Edit Job" href="#" class="actionLink" onclick="JobEdit(this)"><i class="glyphicon glyphicon-edit" aria-hidden="true"></i></a>' },
               { "data": null, "orderable": false, "defaultContent": '<a data-toggle="tp" data-placement="top" data-delay={"show":2000, "hide":3000} title="Delete Job" href="#" class="DeleteLink" onclick="JobDelete(this)"><i class="glyphicon glyphicon-trash" aria-hidden="true"></i></a>' }
             ],
             columnDefs: [{ "targets": [0,9], "visible": false, "searchable": false },
                  { className: "text-left", "targets": [1,3,4,5,6,7,8,10,12] },
                  { className: "text-center", "targets": [2,11,13,14] }
                
             ],   
         });
       // hide button of jquery datatable
         $(".buttons-print").hide();
         $(".buttons-excel").hide();
    
    
         $('#tblDailyServiceReport tbody').on('dblclick', 'td', function () {
            JobEdit(this)
        });
        $("#ModelJobNo").attr({ 'disabled': true });
    }
    catch(e)
    {
        notyAlert('error', e.message);
    }

    try {

        DataTables.DailyServiceReportSummary = $('#tblServiceReportSummary').DataTable(
         {
             dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
             order: [],
             searching: true,
             paging: true,
             data: GetServiceRegisterSummaryFilter(true),
             language: {
                 search: "_INPUT_",
                 searchPlaceholder: "Search"
             },
             columns: [
                 
               { "data": "EmpID", "defaultContent": "<i>-</i>" },
               { "data": "ServiceDate", "defaultContent": "<i>-</i>" },
               { "data": "Technician", "defaultContent": "<i>-</i>" },
               { "data": "TotalCalls", "defaultContent": "<i>-</i>" },
               { "data": "MinorCalls", "defaultContent": "<i>-</i>" },
               { "data": "MajorCalls", "defaultContent": "<i>-</i>" },
               { "data": "MandatoryCalls", "defaultContent": "<i>-</i>" },
               { "data": "DemoCalls", "defaultContent": "<i>-</i>" },
               { "data": "RepeatCalls", "defaultContent": "<i>-</i>" }
              
             ],
             columnDefs: [{ "targets": [0], "visible": false, "searchable": false },
                  { className: "text-left", "targets": [2] },
                  { className: "text-center", "targets": [1,3,4,5,6,7,8] }
                 
             ]
         });

        $('#tblServiceReportSummary tbody').on('dblclick', 'td', function () {
            ServiceRecordSummaryclick(this)
        });
    

    }
    catch (e) {

    }
});


function ServiceRecordSummaryclick(curobj) {
    debugger;
    try {
        var rowData = DataTables.DailyServiceReportSummary.row($(curobj).parents('tr')).data();
        debugger;
        $("#EmpSelector").val(rowData.EmpID);
        $("#txtServiceDate").val(rowData.ServiceDate);
        RefreshDailyServiceTable();
        goBack();

    }
    catch (e) {
        notyAlert('error', e.message);
    }

}



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
            $('#ModelCustomerName').focus()
        })
        $("#modelContextLabel").text('Add Job');
        //$("#ModelJobNo").removeAttr('disabled');
        $("#TechnicianLabel").text($("#EmpSelector option:selected").text());
        $("#ServiceDateLabel").text((serdat));
        ClearJobForm();
        $(".calltypehidden").hide();
    }
    else
    {
        notyAlert('error', 'Please Choose Technician and Service Date');
    }
   
}
function SummaryTabClick() { 
    ChangeButtonPatchView('DailyServiceReport', 'btnPatchDailyServiceReport', 'Back');
}
function DailyserviceTabClick() { 
    ChangeButtonPatchView('DailyServiceReport', 'btnPatchDailyServiceReport', 'Add');
} 
function goBack() {
    $('#DailyServiceReportTab').trigger('click'); 
} 
//-------------------------------------------------------FILTERS-----------------------------------------------------//
//TAB1
function FilterServiceRecord() { 
    $("#EmpSelector").val("");
    $("#txtServiceDate").val("");

    var checkedValue = $("input[name='filter']:checked").val()
    if (checkedValue != "") {
       
        if (checkedValue == 30)
        { 
            DataTables.DailyService.clear().rows.add(GetServicefilterbyDays(true)).draw(false);
        }
        else if (checkedValue == 60)
        { 
            DataTables.DailyService.clear().rows.add(GetServicefilterbyDays(false)).draw(false);
        }
    }
}
function GetServicefilterbyDays(Isdefault) {
    try { 
        var data = { "Isdefault": Isdefault };
        var ds = {};
        ds = GetDataFromServer("DailyServiceReport/GetServicefilterbyDays/", data);
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        if (ds.Result == "OK") {
            return ds.Records;
        }
        if (ds.Result == "ERROR") {
            notyAlert('error', ds.Message);
            var emptyarr = [];
            return emptyarr;
        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}
//TAB2
function FilterRecordSummary() { 
    $("#txtServiceDate2").val("");

    var checkedValue = $("input[name='filter2']:checked").val()
    if (checkedValue != "") { 
        if (checkedValue == 30) { 
            DataTables.DailyServiceReportSummary.clear().rows.add(GetServiceRegisterSummaryFilter(true)).draw(false);
        }
        else if (checkedValue == 60) { 
            DataTables.DailyServiceReportSummary.clear().rows.add(GetServiceRegisterSummaryFilter(false)).draw(false);
        }
    }
}
function GetServiceRegisterSummaryFilter(Isdefault) {
    try { 
        var data = { "Isdefault": Isdefault };
        var ds = {};
        ds = GetDataFromServer("DailyServiceReport/GetServiceRegisterSummaryFilter/", data);
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        if (ds.Result == "OK") {
            return ds.Records;
        }
        if (ds.Result == "ERROR") {
            notyAlert('error', ds.Message);
            var emptyarr = [];
            return emptyarr;
        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}
//-------------------------------------------------------FILTERS END-----------------------------------------------------//

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
        debugger;
        $("#married-true").prop('checked', false);
        $("#married-false").prop('checked', false);
        var v = $(curobj).val();
        $("#ModelTechEmpID").val(v);
        if ($("#txtServiceDate").val() == "")
        {
            $("#txtServiceDate").val($("#hdfcurrentdate").val());
            $("#ModelServiceDate").val($("#hdfcurrentdate").val());
        }
        $("#ModelServiceDate").val($("#txtServiceDate").val());
       
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
            notyAlert('error', ds.Message);
            var emptyarr = [];
            return emptyarr;
        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

function RefreshServiceReportSummaryTable(this_obj) {
    try {
        debugger;
        if (this_obj.value!="")
        {
            $("#married-true2").prop('checked', false);
            $("#married-false2").prop('checked', false);
            var serdate = $("#txtServiceDate2").val();
            if ((serdate) && (DataTables.DailyServiceReportSummary != undefined)) {
                DataTables.DailyServiceReportSummary.clear().rows.add(GetServiceRegisterSummary(serdate)).draw(false);
            }
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
            notyAlert('error', ds.Message);
            var emptyarr = [];
            return emptyarr;
        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

function PrintTableToDoc()
{
    debugger;

    try {

        $(".buttons-excel").trigger('click');
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}



