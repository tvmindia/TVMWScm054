﻿
var DataTables = {};
$(document).ready(function () {

    try {

        DataTables.DepositWithdrawalTable = $('#tblDepositwithdrawalList').DataTable(
         {
             dom: '<"pull-left"f>rt<"bottom"ip><"clear">',
             order: [],
             searching: true,
             paging: true,
             data: null,
             columns: [
               { "data": "ID", "defaultContent": "<i>-</i>" },
                { "data": "TransactionDescription", "defaultContent": "<i>-</i>" },
               { "data": "RefDate",render: function (data, type, row) { return ConvertJsonToDate(data); }, "defaultContent": "<i>-</i>" },
              
               { "data": "RefNo", "defaultContent": "<i>-</i>" },
               { "data": "Amount",render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
               { "data": "Description", "defaultContent": "<i>-</i>" },
               { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink"  onclick="EditDepositWithdrawal(this)" ><i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>' }
            ],
             columnDefs: [
                   { visible:false, "targets": [0] },
                  { className: "text-left", "targets": [1,3,5] },
                  { className: "text-center", "targets": [2,4,6] }

             ]
         });

        RefreshDepositsAndWithdrawalsTableBetweenDates();

    }
    catch (e) {

    }
   
  
});

function TransactionTypeOnChange(curobj)
{
    try {
        if (curobj.value != "DEPST") {
            $(".hdDepositMode").hide();
       
        }
        else {
            $(".hdDepositMode").show();
           
        }

    }
    catch (e) {
        notyAlert('error', e.Message);
    }
}

function DepositModeOnChange(curobj)
{
    try {
        if (curobj.value != "Cheque") {
            $(".hdChequeStatus").hide();

        }
        else {
            $(".hdChequeStatus").show();
            $("#ChequeStatus").val('NotCleared');
        }

    }
    catch (e) {
        notyAlert('error', e.Message);

    }
}

function Add()
{
    ChangeButtonPatchView('DepositAndWithdrawal', 'btnPatchDepositandwithdrawal', 'Save');
}

function EditDepositWithdrawal(curObj)
{
    try
    {
        $('#tabDepositwithdrawalEntry').trigger('click');
        ChangeButtonPatchView('DepositAndWithdrawal', 'btnPatchDepositandwithdrawal', 'EditSave');
        var rowData = DataTables.DepositWithdrawalTable.row($(curObj).parents('tr')).data();
        var result = GetDepositandwithdrawalEntryByID(rowData.ID);
        ClearForm();
        if (result) {
            $("#TransactionType").val(result.TransactionType);
            $("#RefNo").val(result.RefNo);
            $("#RefDate").val(ConvertJsonToDate(result.RefDate));
            $("#Amount").val(result.Amount);
            $("#Description").val(result.Description);
            $("#DepwithID").val(result.ID);
            $("#deleteId").val(result.ID);
        }

       
      
    }
    catch(e)
    {
        notyAlert('error', e.message);
    }
}

function ClearForm()
{
    $('#formdepositwithdrwal')[0].reset();
    $("#DepwithID").val('');
    $("#deleteId").val('');
}

function GetDepositandwithdrawalEntryByID(ID) {
    try {

        var data = { "ID": ID };
        var ds = {};
        ds = GetDataFromServer("DepositAndWithdrawal/GetDepositAndWithdrawalEntryByID/", data);
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


function ReferenceDateOnChange(curobj)
{
    $("#chkShowAll").prop('checked', false);
    RefreshDepositsAndWithdrawalsTableBetweenDates();
}

function RefreshDepositsAndWithdrawalsTableBetweenDates() {
    try {
        var ReferenceDateFrom = $("#txtReferenceDateFrom").val();
        var ReferenceDateTo = $("#txtReferenceDateTo").val();
        if ((ReferenceDateFrom) && (ReferenceDateTo) && (DataTables.DepositWithdrawalTable != undefined)) {
            DataTables.DepositWithdrawalTable.clear().rows.add(GetAllDepositsAndWithdrawalsBetweenDates(ReferenceDateFrom, ReferenceDateTo)).draw(false);
            $('[data-toggle="tp"]').tooltip({ container: 'body' });
        }

    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

function GetAllDepositsAndWithdrawalsBetweenDates(fromdate, todate) {
    try {

        var data = { "Fromdate": fromdate, "Todate": todate };
        var ds = {};
        ds = GetDataFromServer("DepositAndWithdrawal/GetAllDepositAndWithdrawalBetweenDates/", data);
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        if (ds.Result == "OK") {
            return ds.Records;
        }
        if (ds.Result == "ERROR") {
            notyAlert('error', ds.Message);
          
        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}




function RefreshDepositsAndWithdrawalsTable() {
    try {

        if (DataTables.DepositWithdrawalTable != undefined) {
            $("#chkShowAll").prop('checked', true);
            DataTables.DepositWithdrawalTable.clear().rows.add(GetAllDepositsAndWithdrawals()).draw(false);
            $('[data-toggle="tp"]').tooltip({ container: 'body' });
        }

    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

function GetAllDepositsAndWithdrawals() {
    try {

        var data = {};
        var ds = {};
        ds = GetDataFromServer("DepositAndWithdrawal/GetAllDepositAndWithdrawal/", data);
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

function ShowAll(curobj)
{
    if (curobj.checked == true)
    {
        RefreshDepositsAndWithdrawalsTable();

    }
    $("#txtReferenceDateFrom").val('');
    $("#txtReferenceDateTo").val('');
}



function SaveDepositandwithdrawal()
{
    try
    {
        $("#btnInsertUpdateDepositandwithdrawal").trigger('click');
    }
    catch(e)
    {
        notyAlert('error', e.message);
    }
}

function DepositAndWithdrawalSaveSuccess(data, status, xhr)
{
    try {
        var JsonResult = JSON.parse(data)
        switch (JsonResult.Result) {
            case "OK":
                    notyAlert('success', JsonResult.Record.Message);
                    $("#DepwithID").val(JsonResult.Record.ID);
                    $("#deleteId").val(JsonResult.Record.ID);
                    RefreshDepositsAndWithdrawalsTable();
                    $("#txtReferenceDateFrom").val('');
                    $("#txtReferenceDateTo").val('');
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

function DeleteSuccess(data, status, xhr)
{
    try {
        var JsonResult = JSON.parse(data)
        switch (JsonResult.Result) {
            case "OK":
                notyAlert('success', JsonResult.Record.Message);
                RefreshDepositsAndWithdrawalsTable();
                $("#txtReferenceDateFrom").val('');
                $("#txtReferenceDateTo").val('');
                goBack();
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

function DeleteDepositandwithdrawal()
{
    try {
        notyConfirm('Are you sure to delete?', 'DeleteDepositandwithdrawalConform()', '', "Yes, delete it!");
       
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

function DeleteDepositandwithdrawalConform()
{
    $("#btnFormDelete").trigger('click');
}

function goBack() {
    $('#tabDepositwithdrawalList').trigger('click');
    ClearForm();
}

function List() {
    try {

        ChangeButtonPatchView('DepositAndWithdrawal', 'btnPatchDepositandwithdrawal', 'Add');
       

    } catch (e) {
        notyAlert('error', e.message);
    }

}
function AddDepositandwithdrawal()
{
    $('#tabDepositwithdrawalEntry').trigger('click');
    ClearForm();
    ChangeButtonPatchView('DepositAndWithdrawal', 'btnPatchDepositandwithdrawal', 'Save');
}