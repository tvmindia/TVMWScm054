﻿var DataTables = {};
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
               { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink" onclick="JobEdit(this)"><i class="fa fa-pencil-square-o" aria-hidden="true"></i>Edit</a><span> | </span><a href="#" class="actionLink" onclick="JobDelete(this)"><i class="fa fa-trash-o" aria-hidden="true"></i>Remove</a>' }
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




