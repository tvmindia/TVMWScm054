﻿var DataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";
var _Materials = [];
//---------------------------------------Docuement Ready--------------------------------------------------//
$(document).ready(function () {
    try
    {
        DataTables.SalesReturnTable = $('#tblSalesReturnList').DataTable(
        {
            dom: '<"pull-left"f>rt<"bottom"ip><"clear">',
            order: [],
            searching: true,
            paging: true,
            data: GetAllSalesReturn(),
            columns: [
                   { "data": "ID", "defaultContent": "<i>-</i>" },
               
                 { "data": "OpenDateFormatted", "defaultContent": "<i>-</i>" },
                  { "data": "RefNo", "defaultContent": "<i>-</i>" },
                   { "data": "ItemCode", "defaultContent": "<i>-</i>" },

              { "data": "Description", "defaultContent": "<i>-</i>" },
                { "data": "Qty", "defaultContent": "<i>-</i>" },
               { "data": "ReturnStatusYN", "defaultContent": "<i>-</i>" },
                { "data": "Remarks", "defaultContent": "<i>-</i>" },
              { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink" onclick="Edit(this)"><i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>' }
            ],
            columnDefs: [{ "targets": [0], "visible": false, "searchable": false },
              
                { className: "text-right", "targets": [5] },
                { className: "text-center", "targets": [1, 2, 3, 4,6, 7, 8] },
                  {
                      "render": function (data, type, row) {
                          return (data == "True" ? "Returned" + '<i class="fa fa-check" style="color:green;" aria-hidden="true"></i>' : "Not Returned");
                      },
                      "targets": 6
                  }
            ]
        });

        $('#tblSalesReturnList tbody').on('dblclick', 'td', function () {

            Edit(this);
        });
        GetAllItemCode();
        EG_ComboSource('Materials', _Materials, 'ItemCode', 'ID', 'Description')
    }
    catch(e)
    {
        notyAlert('error', e.message);
    }
});

//---------------------------------------Edit DefectiveDamaged--------------------------------------------------//
function Edit(currentObj) {
    //Tab Change on edit click
    debugger;

    $('#AddTab').trigger('click');
    ChangeButtonPatchView("SalesReturn", "btnPatchSalesReturnSettab", "Edit"); //ControllerName,id of the container div,Name of the action
    debugger;
    var rowData = DataTables.SalesReturnTable.row($(currentObj).parents('tr')).data();
    if ((rowData != null) && (rowData.ID != null)) {
        fillSalesReturn(rowData.ID);
        if (rowData.ReturnStatusYN == "True") {
            ReturnedFields();
        }
    }

}

function ReturnedFields() {
    $("#RefNo").prop('disabled', true);
    $("#ItemCode").prop('disabled', true);
    $("#Qty").prop('disabled', true);
    $("#Remarks").prop('disabled', true);
    $('#OpenDate').prop('disabled', true);
    ChangeButtonPatchView("SalesReturn", "btnPatchSalesReturnSettab", "Return");
}

//-----------------------------------------Reset Validation Messages--------------------------------------//
function ResetForm() {
    var validator = $("#formIns_Up").validate();
    $('#formIns_Up').find('.field-validation-error span').each(function () {
        validator.settings.success($(this));
    });
    validator.resetForm();
}

//--3-----------combo source binding----------------------------
function EG_ComboSource(id, values, valueCol, textCol, textDesc) {
    debugger;
    if (document.getElementById(id) == null || document.getElementById(id) == 'undefined') {
        alert("combo source element is not defined in cshtml");
    }

    var options = '';
    for (var i = 0; i < values.length; i++)
        options += '<option id="' + values[i][textCol] + '" value="' + values[i][valueCol] + '" >' + values[i][textDesc] + '</option>';

    document.getElementById(id).innerHTML = options;

    //
}

//------------------------------- SalesReturn Save-----------------------------//
function save() {
    debugger;
    var qty = SalesReturnValidation();
    debugger;
    notySaveConfirm('Total quantities=' + qty + '\nAre you sure to Save?', 'SaveClick()');

}

function Delete() {

    notyConfirm('Are you sure to delete?', 'DeleteSalesReturn()');

}

//---------------------------------------Delete-------------------------------------------------------//
function DeleteSalesReturn() {
    debugger;
    var id = $("#ID").val();
    if (id != EmptyGuid) {
        $("#btnFormDelete").click();
    }
    else {
        notyAlert('error', 'Error');
    }
}

function DeleteSuccess(data, status) {
    var i = JSON.parse(data)
    debugger;

    switch (i.Result) {

        case "OK":
            BindAllSalesReturn();
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

function ReturnToCompany() {
    debugger;
    var ReturnID = $("#ID").val();
    if (ReturnID != EmptyGuid) {
        $("#btnFormReturn").click();
    }
    else {
        notyAlert('error', 'Error');
    }
}

function ReturnSuccess(data, status) {
    var i = JSON.parse(data)
    debugger;

    switch (i.Result) {

        case "OK":
            BindAllSalesReturn();
            ReturnedFields();
            notyAlert('success', i.Message);
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

//---------------------------------------Fill DefectiveDamaged--------------------------------------------------//
function fillSalesReturn(ID) {
    debugger;
    ChangeButtonPatchView("SalesReturn", "btnPatchSalesReturnSettab", "Edit");
    var thisItem = GetSalesReturnByID(ID); //Binding Data
    //Hidden
    $("#deleteId").val(thisItem[0].ID);
    $("#returnId").val(thisItem[0].ID);
    $("#ID").val(thisItem[0].ID);
    if (thisItem[0].OpenDate != null) {
        var $datepicker = $('#OpenDate');
        $datepicker.datepicker('setDate', new Date(thisItem[0].OpenDate));
    }
    $("#RefNo").val(thisItem[0].RefNo)
    $("#ItemID").val(thisItem[0].ItemID)
    $("#Description").val(thisItem[0].Description)
    $("#Qty").val(thisItem[0].Qty)
    $("#Remarks").val(thisItem[0].Remarks)
    $("#deleteId").val(thisItem[0].ID);
    $("#returnId").val(thisItem[0].ID);
    $("#EmpID").prop('disabled', true);
    $("#Type").prop('disabled', true);
    ReturnItemCode(thisItem[0].ItemID);
}

function ReturnItemCode(itemID) {
    debugger;
    var a = $("#Materials").find('option[id="' + itemID + '"]');
    $("#ItemCode").val(a.val());
}
//---------------------------------------Get DefectiveDamaged Details By ID-------------------------------------//
function GetSalesReturnByID(id) {
    try {
        var data = { "id": id };
        var ds = {};
        ds = GetDataFromServer("SalesReturn/GetSalesReturnByID/", data);
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

function SalesReturnSaveSuccess(data, status) {
    debugger;
    var JsonResult = JSON.parse(data)
    switch (JsonResult.Result) {
        case "OK":
            if ($("#ID").val() == EmptyGuid) {
                fillSalesReturn(JsonResult.Records.ID);
            }
            else {
                fillSalesReturn($("#ID").val());
            }
            BindAllSalesReturn();
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

//---------------------------------------Clear Fields-----------------------------------------------------//
function clearfields() {
    $("#ID").val(EmptyGuid);
    $("#RefNo").val("")
    $("#ItemCode").val("")
    $("#ItemID").val("");
    $("#Description").val("");
    $("#Qty").val("")
    $("#Remarks").val("")
    $("#deleteId").val("0")
    $("#returnId").val("0");
    var $datepicker = $('#OpenDate');
    $datepicker.datepicker('setDate', null);
    $("#RefNo").prop('disabled', false);
    $("#ItemCode").prop('disabled', false);
    $("#Qty").prop('disabled', false);
    $("#Remarks").prop('disabled', false);
    $('#OpenDate').prop('disabled', false);
    ResetForm();
}

//---------------------------------------Bind All DefectiveDamaged----------------------------------------------//
function BindAllSalesReturn() {
    try {
        debugger;
        DataTables.SalesReturnTable.clear().rows.add(GetAllSalesReturn()).draw(false);
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}


function SaveClick() {
    $("#btnInsertUpdateSalesReturn").trigger('click');
}

function SalesReturnValidation() {
    debugger;
    try {
        var ItemID = $("#ItemID").val();
        var data = { "ItemID": ItemID };
        var ds = {};
        ds = GetDataFromServer("SalesReturn/SalesReturnValidation/", data);
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        return ds.Message;
    }
    catch (e) {

    }
}

function ItemCodeOnChange(i) {
    debugger;
    var val = $(i).val();
    var a = $("#Materials").find('option[value="' + val + '"]');
    var itemID = a.attr('id');
    $("#ItemID").val(itemID);
    var desc = a.text();
    $("#Description").val(desc);

}

function GetAllItemCode() {


    try {

        var data = {};
        var ds = {};
        ds = GetDataFromServer("SalesReturn/GetAllItemCode/", data);
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        if (ds.Result == "OK") {

            _Materials = ds.Records;


        }
        if (ds.Result == "ERROR") {
            alert(ds.Message);
        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }



}

//---------------get grid fill result-------------------
function GetAllSalesReturn() {
    try {
        debugger;
        var data = {};
        var ds = {};
        ds = GetDataFromServer("SalesReturn/GetAllSalesReturn/", data);
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

//--------------------button actions ----------------------
function List() {
    try {

        ChangeButtonPatchView('SalesReturn', 'btnPatchSalesReturnSettab', 'List');

    } catch (x) {
        alert(x);
    }

}

function Add(id) {
    debugger;
    if (id != 1) {
        $('#AddTab').trigger('click');
    }
    clearfields();
    ChangeButtonPatchView('SalesReturn', 'btnPatchSalesReturnSettab', 'Add');
}

function goBack() {
    $('#SalesReturnTab').trigger('click');
    clearfields();
}