var DataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";

//---------------------------------------Docuement Ready--------------------------------------------------//

$(document).ready(function () {
    try
    {
        DataTables.BillListTable = $('#tblBillList').DataTable(
     {
         dom: '<"pull-left"f>rt<"bottom"ip><"clear">',
         order: [],
         searching: true,
         paging: true,
         data: GetAllBillBookList(),
         columns: [
                { "data": "ID", "defaultContent": "<i>-</i>" },
              { "data": "BillNo", "defaultContent": "<i>-</i>" },
               { "data": "SeriesStart", "defaultContent": "<i>-</i>" },
                { "data": "SeriesEnd", "defaultContent": "<i>-</i>" },
               { "data": "LastUsed", "defaultContent": "<i>-</i>" },
               { "data": "Technician", "defaultContent": "<i>-</i>" },
                 { "data": "Status", "defaultContent": "<i>-</i>" },
                   { "data": "Remarks", "defaultContent": "<i>-</i>" },
           { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink" onclick="Edit(this)"><i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>' }
         ],
         columnDefs: [{ "targets": [0], "visible": false, "searchable": false },            
             { className: "text-center", "targets": [1, 2, 3,4,5,6,7,8] },
            
         ]
     });
    }
    catch (e) {
        notyAlert('error', e.message);
    }
});


//--------------------button actions ----------------------
function List() {
    try {

        ChangeButtonPatchView('AssignBillBook', 'btnPatchAssignBillBookSettab', 'List');
        BindAllBillBooks()

    } catch (x) {
        alert(x);
    }

}

//------------------------------- Bill Book Save-----------------------------//
function save() {
    debugger;
    try
    {
        $("#btnSave").trigger('click');
    }
    catch (e)
    {
        notyAlert('error', e.message);
    }
   

}

function BindBillBookFields(Records)
{
    ChangeButtonPatchView('AssignBillBook', 'btnPatchAssignBillBookSettab', 'Edit');
}

function fillBillBook(id)
{
    try {
        var data = { "ID": id };
        var ds = {};
        ds = GetDataFromServer("AssignBillBook/GeBillBookByID/", data);
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        if (ds.Result == "OK") {

            BindBillBookFields(ds.Records);
        }
        if (ds.Result == "ERROR") {
            notyAlert('error', ds.Message);
            return 0;
        }
        return 1;

    }
    catch (e) {
        notyAlert('error', e.message);
        return 0;
    }
}

function SaveSuccess(data, status) {
    debugger;
    var JsonResult = JSON.parse(data)
    switch (JsonResult.Result) {
        case "OK":
            if ($("#ID").val() == EmptyGuid) {
                fillBillBook(JsonResult.Records.ID);
            }
            else {
                fillBillBook($("#ID").val());
            }

            BindAllCreditNotes();
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
function goBack() {
    $('#AddTab').trigger('click');
    Reset();
}
//---------------------------------------Edit Item--------------------------------------------------//
function Edit(currentObj) {
    var rowData = DataTables.customerBillsTable.row($(currentObj).parents('tr')).data();
    if ((rowData != null) && (rowData.ID != null)) {
        $('#AddTab').trigger('click');
        //if (BindICRBillEntry(rowData.ID)) {
        //    ChangeButtonPatchView('ICRBillEntry', 'btnPatchICRBillEntrySettab', 'Edit');
        //}
        //else {
        //    $('#ListTab').trigger('click');
        //}
    }
}
function Add() {
    ChangeButtonPatchView('AssignBillBook', 'btnPatchAssignBillBookSettab', 'Add');
    Reset();
}
function Reset()
{
    $("#BillNo").val("<<Auto Generated>>");
    $("#SeriesStart").val("");
    $("#SeriesEnd").val("");
    $("#LastUsed").val("");
}
//---------------------------------------Bind All Bill Book----------------------------------------------//
function BindAllBillBooks() {
    try {

        DataTables.BillListTable.clear().rows.add(GetAllBillBookList()).draw(false);

    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

function GetAllBillBookList()
{
    try {

        var data = { };
        var ds = {};
        ds = GetDataFromServer("AssignBillBook/GetAllBillBook/", data);

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