var DataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";

//---------------------------------------Docuement Ready--------------------------------------------------//

$(document).ready(function () {
    try
    {
        DataTables.CreditNotesTable = $('#tblCreditNotesList').DataTable(
       {
           dom: '<"pull-left"f>rt<"bottom"ip><"clear">',
           order: [],
           searching: true,
           paging: true,
           data: GetAllCreditNotes(false),
           columns: [
                  { "data": "ID", "defaultContent": "<i>-</i>" },
                { "data": "CreditNoteNo", "defaultContent": "<i>-</i>" },
                 { "data": "DateFormatted", "defaultContent": "<i>-</i>" },
                { "data": "Amount", "defaultContent": "<i>-</i>" },
                 { "data": "Description", "defaultContent": "<i>-</i>" },
                 
             { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink" onclick="Edit(this)"><i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>' }
           ],
           columnDefs: [{ "targets": [0], "visible": false, "searchable": false },
               { className: "text-right", "targets": [3] },
               { className: "text-center", "targets": [1, 2, 5] },
               { className: "text-left", "targets": [4] },
                           ]
       });

        $('#tblCreditNotesList tbody').on('dblclick', 'td', function () {

            Edit(this);
        });

        FillDates();

        $('#fromDate').change(function () {
            FromDateOnChange();
        });
        $('#toDate').change(function () {
            FromDateOnChange();
        });
    }
    catch (e) {
        notyAlert('error', e.message);
    }
});

function FillDates()
{
    debugger;
    var m_names = new Array("Jan", "Feb", "Mar",
 "Apr", "May", "Jun", "Jul", "Aug", "Sep",
 "Oct", "Nov", "Dec");

    var d = new Date();
    var curr_date = d.getDate();
    var curr_month = d.getMonth();
    var curr_year = d.getFullYear();
    var toDate = curr_date + "-" + m_names[curr_month]
    + "-" + curr_year;
    var $datepicker = $('#toDate');
    $datepicker.datepicker('setDate', new Date(toDate));
    var today = new Date()
    var pd = new Date();
   pd.setDate(pd.getDate() - 30);
   var  priorDate = pd.toLocaleString()
    priorDate = priorDate.split(' ')[0];
    var p_month = parseInt(priorDate.split('/')[0]) - 1;
    var p_date = priorDate.split('/')[1];
    var p_year = priorDate.split('/')[2];
    var fromDate = p_date + "-" + m_names[p_month]
    + "-" + p_year;
    var $datepicker = $('#fromDate');
    $datepicker.datepicker('setDate', new Date(fromDate));
}

//--------------------button actions ----------------------
function List() {
    try {

        ChangeButtonPatchView('CreditNotes', 'btnPatchCreditNotesSettab', 'List');
        DateClear();

    } catch (x) {
        alert(x);
    }

}
function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode;
    if (charCode != 46 && charCode > 31
      && (charCode < 48 || charCode > 57))
        return false;

    return true;
}
function DateClear()
{
    $('#fromDate').val("");
    $('#toDate').val("");
}

function clearfields()
{
    $("#ID").val(EmptyGuid);
    $("#CreditNoteNo").val("")
    $("#Amount").val("");
    $("#Description").val("")
    $("#HiddenCreditNoteNo").val("");
    var $datepicker = $('#Date');
    $datepicker.datepicker('setDate', null);   
    $("#CreditNoteNo").prop('disabled', false);
    $("#deleteId").val("0")
    ResetForm();
}
//---------------------------------------Edit CreditNotes--------------------------------------------------//
function Edit(currentObj) {
    //Tab Change on edit click
    debugger;

    $('#AddTab').trigger('click');
    ChangeButtonPatchView("CreditNotes", "btnPatchCreditNotesSettab", "Edit"); //ControllerName,id of the container div,Name of the action
    debugger;
    var rowData = DataTables.CreditNotesTable.row($(currentObj).parents('tr')).data();
    if ((rowData != null) && (rowData.ID != null)) {
        fillCreditNotes(rowData.ID);
      
    }

}
function Delete() {

    notyConfirm('Are you sure to delete?', 'DeleteCreditNote()');

}
//---------------------------------------Delete-------------------------------------------------------//
function DeleteCreditNote() {
    debugger;
    var id = $("#ID").val();
    if (id != EmptyGuid) {
        $("#btnFormDelete").click();
    }
    else {
        notyAlert('error', 'Error');
    }
}
//---------------------------------------Get CreditNotes Details By ID-------------------------------------//
function GetCreditNotesdByID(id) {
    try {
        var data = { "id": id };
        var ds = {};
        ds = GetDataFromServer("CreditNotes/GetCreditNotesByID/", data);
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
function goBack() {
    $('#CreditNotesTab').trigger('click');
    clearfields();
}
function DeleteSuccess(data, status) {
    var i = JSON.parse(data)
    debugger;

    switch (i.Result) {

        case "OK":
            BindAllCreditNotes();
            notyAlert('success', i.Message);
            clearfields();
            goBack();
            break;
        case "Error":
            notyAlert('error', "Error");
            break;
        case "ERROR":
            notyAlert('error', i.Message);
            break;
        default:
            break;
    }
}
//---------------------------------------Fill CreditNotes--------------------------------------------------//
function fillCreditNotes(ID) {
    debugger;
    ChangeButtonPatchView("CreditNotes", "btnPatchCreditNotesSettab", "Edit");
    var thisItem = GetCreditNotesdByID(ID); //Binding Data
    //Hidden
    $("#deleteId").val(thisItem[0].ID);
    $("#ID").val(thisItem[0].ID);
    $("#HiddenCreditNoteNo").val(thisItem[0].CreditNoteNo);
    $("#CreditNoteNo").val(thisItem[0].CreditNoteNo);
    $("#Amount").val(thisItem[0].Amount);
    $("#Description").val(thisItem[0].Description);
    $("#CreditNoteNo").prop('disabled', true);
    if (thisItem[0].Date != null) {
        var $datepicker = $('#Date');
        $datepicker.datepicker('setDate', new Date(thisItem[0].Date));
    }
}

//-----------------------------------------Reset Validation Messages--------------------------------------//
function ResetForm() {
    var validator = $("#formIns_Up").validate();
    $('#formIns_Up').find('.field-validation-error span').each(function () {
        validator.settings.success($(this));
    });
    validator.resetForm();
}

function Add(id) {
    debugger;
    if (id != 1) {
        $('#AddTab').trigger('click');
    }
    clearfields();
    ChangeButtonPatchView('CreditNotes', 'btnPatchCreditNotesSettab', 'Add');
}

function showAllYNCheckedOrNot(i)
{
    debugger;
    
    if(i.checked==true)
    {
        DataTables.CreditNotesTable.clear().rows.add(GetAllCreditNotes(true)).draw(false);
    }
    else
    {
        DataTables.CreditNotesTable.clear().rows.add(GetAllCreditNotes(false)).draw(false);
    }
    $('#fromDate').val("");
    $('#toDate').val("");
}

function FromDateOnChange()
{

    DataTables.CreditNotesTable.clear().rows.add(GetCreditNotesBetweenDates()).draw(false);
}
function GetCreditNotesBetweenDates()
{
    try {
        debugger;
        var fromDate = $("#fromDate").val();
        var toDate = $("#toDate").val();
        if (toDate == "" && fromDate == "")
        {
            //DataTables.CreditNotesTable.clear().rows.add(GetAllCreditNotes(false)).draw(false);
        }
        else
        {
            var data = { "fromDate": fromDate, "toDate": toDate };
            var ds = {};
            ds = GetDataFromServer("CreditNotes/GetCreditNotesBetweenDates/", data);
            debugger;
            if (ds != '') {
                ds = JSON.parse(ds);
            }
            if (ds.Result == "OK") {
                // debugger;
                return ds.Records;
            }
            if (ds.Result == "ERROR") {
                alert(ds.Message);
            }
        }
     
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}
//---------------get grid fill result-------------------
function GetAllCreditNotes(showAllYN) {
    try {
        debugger;
        var data = { "showAllYN": showAllYN };
        var ds = {};
        ds = GetDataFromServer("CreditNotes/GetAllCreditNotes/", data);
        debugger;
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        if (ds.Result == "OK") {
            // debugger;
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

//------------------------------- CreditNotes Save-----------------------------//
function save() {
    debugger;
    $("#btnInsertUpdateCreditNotes").trigger('click');

}

//---------------------------------------Bind All CreditNotes----------------------------------------------//
function BindAllCreditNotes() {
    try {
        debugger;
        DataTables.CreditNotesTable.clear().rows.add(GetAllCreditNotes(false)).draw(false);
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

function CreditNotesSaveSuccess(data, status) {
    debugger;
    var JsonResult = JSON.parse(data)
    switch (JsonResult.Result) {
        case "OK":
            if ($("#ID").val() == EmptyGuid) {
                fillCreditNotes(JsonResult.Records.ID);
            }
            else {
                fillCreditNotes($("#ID").val());
            }
            FillDates();
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