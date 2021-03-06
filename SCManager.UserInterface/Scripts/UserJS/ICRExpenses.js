﻿var DataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";
var ToDateVal, FromDateVal;
//---------------------------------------Docuement Ready--------------------------------------------------//

$(document).ready(function () {
    try { 
        ChangeButtonPatchView('ICRExpenses', 'btnPatchICRExpensesSettab', 'Add');
        DataTables.ICRExpensesTable = $('#tblICRExpensesList').DataTable(
       {
           dom: '<"pull-left"Bf>rt<"bottom"ip><"clear">',
           buttons: [{
               extend: 'excel',
               exportOptions:
                            {
                                columns: [1,2,3,4,5,6,7]
                            }
           }],
           order: [],
           searching: true,
           paging: true,
           data: null,
           columns: [
                { "data": "ID", "defaultContent": "<i>-</i>" },
                { "data": "EntryNo", "defaultContent": "<i>-</i>" },
                { "data": "DateFormatted", "defaultContent": "<i>-</i>" },
                { "data": "RefNo", "defaultContent": "<i>-</i>" },
                { "data": "PaymentMode", "defaultContent": "<i>-</i>" }, 
                { "data": "Description", "defaultContent": "<i>-</i>" },
                { "data": "Amount", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
                { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink" onclick="Edit(this)"><i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>' }
           ],
           columnDefs: [{ "targets": [0], "visible": false, "searchable": false },
               { className: "text-right", "targets": [6] },
               { className: "text-center", "targets": [1, 2, 3, 4,7] },
               { className: "text-left", "targets": [5] },
           ]
       });

        $('#tblICRExpensesList tbody').on('dblclick', 'td', function () {
            Edit(this);
        });
        FromDateVal = $("#fromDate").val();
        ToDateVal = $("#toDate").val();

        $('#fromDate').change(function () {
            $("#showAllYNCheckbox").prop('checked', false);
            BindAllICRExpenses();
        });
        $('#toDate').change(function () {
            $("#showAllYNCheckbox").prop('checked', false);
            BindAllICRExpenses();
        }); 
        BindOutStandingPayment();
        clearfields();
        $(".buttons-excel").hide();

    }
    catch (e) {
        notyAlert('error', e.message);
    }
});


function BindOutStandingPayment()
{
    var thisItem = GetOutStandingICRPayment();
    $("#OutStandingICRPayment").text(thisItem.OutStandingPaymentFormatted);
    $("#OutstandingCheque").text(thisItem.OutstandingChequeFormatted);
    $("#OutstandingCash").text(thisItem.OutstandingCashFormatted);

}


function GetOutStandingICRPayment() {
    try {
        var data = {};
        var ds = {};
        ds = GetDataFromServer("ICRExpenses/GetOutStandingICRPayment/", data);
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

//--------------------button actions ----------------------
function List() {
    try {
        ChangeButtonPatchView('ICRExpenses', 'btnPatchICRExpensesSettab', 'List');
        $("#showAllYNCheckbox").prop('checked', false);
        BindAllICRExpenses()

    } catch (x) {
        alert(x);
    }

}

function ChequeTypeDisplay() {
    debugger;

    if ($("#PaymentMode").val() == "CHEQUE") {
        $("#ChequeTypeDiv").show();
        $("#ChequeType").val("IFB");
    }
    else {
        $("#ChequeTypeDiv").hide();
        $("#ChequeType").val("");
    }
}

function clearfields() {
    $("#UpdateID").val(EmptyGuid);
    $("#ID").val(EmptyGuid);
    $("#ChequeType").val("");
    $("#ChequeTypeDiv").hide();
    $("#EntryNo").prop('disabled', true);
    $("#EmpID").val("");
    $("#EmpID").prop('disabled', true);
    $("#EntryNo").val("<<Auto Generated>>");
    $("#PaymentMode").val("");
    $("#Amount").val("");
    $("#Description").val("");
    $("#RefNo").val("");
    var $datepicker = $('#Date');
    $datepicker.datepicker('setDate', null);
    $("#deleteId").val("0");
   
    ResetForm();
}
//---------------------------------------Edit ICRExpenses--------------------------------------------------//
function Edit(currentObj) {
    //Tab Change on edit click
    $('#AddTab').trigger('click');
    ChangeButtonPatchView("ICRExpenses", "btnPatchICRExpensesSettab", "Edit"); //ControllerName,id of the container div,Name of the action
    var rowData = DataTables.ICRExpensesTable.row($(currentObj).parents('tr')).data();
    if ((rowData != null) && (rowData.ID != null)) {
        fillICRExpenses(rowData.ID);
    }

}
//---------------------------------------Delete-------------------------------------------------------//
function Delete() {
    notyConfirm('Are you sure to delete?', 'DeleteICRExpenses()', '', "Yes, delete it!");
}

function DeleteICRExpenses() {
    var id = $("#UpdateID").val();
    if (id != EmptyGuid) {
        $("#btnFormDelete").click();
    }
    else {
        notyAlert('error', 'Error');
    }
}
//---------------------------------------Get ICRExpenses Details By ID-------------------------------------//
function GetICRExpensesByID(id) {
    try {
        var data = { "id": id };
        var ds = {};
        ds = GetDataFromServer("ICRExpenses/GetICRExpensesByID/", data);
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

//---------------------------------------Fill ICRExpenses--------------------------------------------------//
function fillICRExpenses(ID) {
    ChangeButtonPatchView("ICRExpenses", "btnPatchICRExpensesSettab", "Edit");
    var thisItem = GetICRExpensesByID(ID); //Binding Data
    //Hidden
    $("#UpdateID").val(thisItem.ID);
    $("#deleteId").val(thisItem.ID);
    $("#ID").val(thisItem.ID);
    $("#EntryNo").val(thisItem.EntryNo);
    $("#RefNo").val(thisItem.RefNo); 
    $("#PaymentMode").val(thisItem.PaymentMode);
    $("#Amount").val(roundoff(thisItem.Amount));
    $("#Description").val(thisItem.Description);
    debugger;
    if (thisItem.PaymentMode == "CHEQUE") {
        $("#ChequeTypeDiv").show();
        $("#ChequeType").val(thisItem.ChequeType);
    }
    else {
        $("#ChequeTypeDiv").hide();
        $("#ChequeType").val("");
    }
    if (thisItem.RefDate != null) {
        var $datepicker = $('#Date');
        $datepicker.datepicker('setDate', new Date(thisItem.RefDate));
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
    if (id != 1) {
        $('#AddTab').trigger('click');
    }
    clearfields();
    ChangeButtonPatchView('ICRExpenses', 'btnPatchICRExpensesSettab', 'Add');
}

function showAllYNCheckedOrNot(i) {
    if (i.checked == true) {
        $('#fromDate').val("");
        $('#toDate').val("");
        DataTables.ICRExpensesTable.clear().rows.add(GetAllICRExpenses(true)).draw(false); 
    }
    else {
        $('#fromDate').val(FromDateVal);
        $('#toDate').val(ToDateVal);
        DataTables.ICRExpensesTable.clear().rows.add(GetAllICRExpenses(false)).draw(false);
    }

}



//---------------get grid fill result-------------------
function GetAllICRExpenses(showAllYN) {
    try {
        var FromDate = $("#fromDate").val();
        var ToDate = $("#toDate").val();

        var data = {"FromDate": FromDate, "ToDate": ToDate };
        var ds = {};
        ds = GetDataFromServer("ICRExpenses/GetAllICRExpenses/", data);
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

//------------------------------- ICRExpenses Save-----------------------------//
function save() {
    $("#btnInsertUpdateICRExpenses").trigger('click');
}

//---------------------------------------Bind All ICRExpenses----------------------------------------------//
function BindAllICRExpenses() {
    try {
        DataTables.ICRExpensesTable.clear().rows.add(GetAllICRExpenses(false)).draw(false);
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

function SaveSuccess(data, status) {
    var JsonResult = JSON.parse(data)
    switch (JsonResult.Result) {
        case "OK":
            if ($("#UpdateID").val() == EmptyGuid) {
                $("#UpdateID").val(JsonResult.Records.ID);
                fillICRExpenses(JsonResult.Records.ID);
            }
            else {
                fillICRExpenses($("#UpdateID").val());
            } 
            BindAllICRExpenses();
            BindOutStandingPayment();
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

function DeleteSuccess(data, status) {
    var i = JSON.parse(data)
    switch (i.Result) {
        case "OK":
            BindAllICRExpenses();
            $('#ListTab').trigger('click');
            notyAlert('success', i.Message);
            BindOutStandingPayment();
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

function ExportData() {
    try {

        $(".buttons-excel").trigger('click');
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}