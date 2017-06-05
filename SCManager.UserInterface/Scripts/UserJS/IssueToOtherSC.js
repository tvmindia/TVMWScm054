var DataTables = {};
var emptyGUID = '00000000-0000-0000-0000-000000000000'
var _Materials = [];
//---------------------------------------Docuement Ready--------------------------------------------------//

$(document).ready(function () {
    try {
        DataTables.tblInvoicesList = $('#tblInvoicesList').DataTable(
       {
           dom: '<"pull-left"f>rt<"bottom"ip><"clear">',
           order: [],
           searching: true,
           paging: true,
           data: GetAllOtherSCReceipt(),
           columns: [
             { "data": "ID", "defaultContent": "<i>-</i>" },
             { "data": "InvoiceDateFormatted", "defaultContent": "<i>-</i>" },
             { "data": "InvoiceNo", "defaultContent": "<i>-</i>" },
             { "data": "ToSCName", "defaultContent": "<i>-</i>" },
               { "data": "GrandTotal", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },

             { "data": "Remarks", "defaultContent": "<i>-</i>" },
             { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink"  onclick="Edit(this)" ><i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>' }
           ],
           columnDefs: [{ "targets": [0], "visible": false, "searchable": false },
                { className: "text-right", "targets": [5] },
           { className: "text-center", "targets": [1, 2, 3, 4, 5, 6] }

           ]
       });

        DataTables.tblIssueToOtherSCDetails = $('#tblIssueToOtherSCDetails').DataTable(
      {

          dom: '<"pull-left"f>rt<"bottom"ip><"clear">',
          order: [],
          searching: false,
          paging: false,
          data: null,
          autoWidth: false,
          columns: EG_Columns(),
          columnDefs: EG_Columns_Settings()
      });

        $('#tblInvoicesList tbody').on('dblclick', 'td', function () {

            Edit(this);
        });
        getMaterials();
        EG_ComboSource('Materials', _Materials, 'ItemCode', 'Description')
        EG_GridDataTable = DataTables.tblIssueToOtherSCDetails;
        List();
    } catch (x) {

        notyAlert('error', x.message);

    }

});


//-----------------------EDIT GRID DEFN-------------------------------------
var EG_totalDetailRows = 0;
var EG_GridData;//DATA SOURCE OBJ ARRAY
var EG_GridDataTable;//DATA TABLE ITSELF FOR REBIND PURPOSE
var EG_SlColumn = 'SlNo';
var EG_GridInputPerRow = 3;
var EG_MandatoryFields = 'MaterialID,Quantity,Rate'


function EG_TableDefn() {

    var tempObj = new Object();
    tempObj.SCCode = "";
    tempObj.ID = "";
    tempObj.MaterialID = "";
    tempObj.SlNo = 0;
    tempObj.Material = "";
    tempObj.Description = "";
    tempObj.Quantity = "";
    tempObj.UOM = "";
    tempObj.Rate = "";
    tempObj.BasicAmount = "";
    tempObj.TradeDiscount = "";
    tempObj.NetAmount = "";

    return tempObj
}


function EG_Columns() {

    var obj = [

                { "data": "ID", "defaultContent": "<i>0</i>" },
                 { "data": "MaterialID", "defaultContent": "<i></i>" },
                { "data": "SlNo", "defaultContent": "<i></i>" },
                { "data": "Material", render: function (data, type, row) { return (EG_createCombo(data, 'S', row, 'Material', 'Materials', 'FillUOM')); } },
                 { "data": "Description", "defaultContent": "<i></i>" },
                { "data": "Quantity", render: function (data, type, row) { return (EG_createTextBox(data, 'N', row, 'Quantity', 'CalculateAmount')); }, "defaultContent": "<i></i>" },
                { "data": "UOM", "defaultContent": "<i></i>" },
                { "data": "Rate", render: function (data, type, row) { return (EG_createTextBox(data, 'F', row, 'Rate', 'CalculateAmount')); }, "defaultContent": "<i></i>" },
                { "data": "BasicAmount", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i></i>" },
                { "data": "TradeDiscount", render: function (data, type, row) { return (EG_createTextBox(data, 'F', row, 'TradeDiscount', 'CalculateAmount')); }, "defaultContent": "<i></i>" },
                { "data": "NetAmount", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i></i>" },
                { "data": null, "orderable": false, "defaultContent": '<a href="#" class="DeleteLink"  onclick="DeleteItem(this)" ><i class="glyphicon glyphicon-trash" aria-hidden="true"></i></a>' }

    ]

    return obj

}

function EG_Columns_Settings() {

    var obj = [
        { "targets": [0], "visible": false, "searchable": false }, { "targets": [1], "visible": false, "searchable": false },
        { "width": "5%", "targets": 2 },
        { "width": "15%", "targets": 3 },
         { "width": "20%", "targets": 4 },
           { "width": "8%", "targets": 5 },
        { "width": "8%", "targets": 6 },
         { "width": "8%", "targets": 7 },
          { "width": "10%", "targets": 8 },
           { "width": "12%", "targets": 9 },
            { "width": "10%", "targets": 10 },
             { "width": "10%", "targets": 11 },
        { className: "text-right", "targets": [7, 9] },
        { className: "text-center", "targets": [2, 3, 5, 11] },
        { className: "text-right disabled", "targets": [8, 10] },
        { className: "text-center disabled", "targets": [6] },
          { className: "text-left disabled", "targets": [4] },
        { "orderable": false, "targets": [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11] }

    ]

    return obj;

}

//------------------------------------------------------------------


function CalculateVAT() {

    var vatpercent = $("#vatpercentage").val();

    var subTotal = $("#subtotal").val();
    vatpercent = parseInt(vatpercent);
    if (vatpercent > 100) {
        vatpercent = 100
        $("#vatpercentage").val(vatpercent);
    }
    if (vatpercent < 0) {
        vatpercent = 0
        $("#vatpercentage").val(vatpercent);
    }
    subTotal = parseInt(subTotal);
    var vatamt = (subTotal * vatpercent / 100)
    if (isNaN(vatamt)) { vatamt = 0.00 }
    $("#vatamount").val(roundoff(vatamt));

    AmountSummary();
}

function CalculateAmount(row) {




    //EG_GridData[row-1][Quantity] = value
    var qty = 0.00;
    var rate = 0.00;
    var dic = 0.00;

    var EGqty = '';
    var EGrate = '';
    var EGdic = '';

    EGqty = EG_GridData[row - 1]["Quantity"];
    EGrate = EG_GridData[row - 1]['Rate'];
    EGdic = EG_GridData[row - 1]['TradeDiscount'];

    qty = parseFloat(EGqty) || 0;
    rate = parseFloat(EGrate) || 0;
    dic = parseFloat(EGdic) || 0;

    if (dic > (qty * rate)) {
        dic = (qty * rate);
    }
    else if (dic < 0) {
        dic = 0
    }

    EG_GridData[row - 1]['Rate'] = roundoff(rate);
    EG_GridData[row - 1]['BasicAmount'] = roundoff(qty * rate);
    EG_GridData[row - 1]['TradeDiscount'] = roundoff(dic);
    EG_GridData[row - 1]['NetAmount'] = roundoff(qty * rate - dic);
    EG_Rebind();

    var total = 0.00;
    for (i = 0; i < EG_GridData.length; i++) {
        total = total + (parseFloat(EG_GridData[i]['NetAmount']) || 0);
    }

    $('#subtotal').val(roundoff(total));
    AmountSummary();

}

function calculateVat() {
    debugger;
    var vatp = parseFloat($('#vatpercentage').val()) || 0;
    var subtotal = parseFloat($('#subtotal').val()) || 0;
    if (vatp > 100) {
        vatp = 100;
    }
    if (vatp < 0) {
        vatp = 0;
    }

    $('#vatpercentage').val(vatp);
    $('#vatamount').val(roundoff(subtotal * vatp / 100));
    AmountSummary();
    typingFlag = 0;
}

function AmountSummary() {

    var subtotal = parseFloat($('#subtotal').val()) || 0;
    var vatamount = parseFloat($('#vatamount').val()) || 0;
    var vatp = (parseFloat($('#vatpercentage').val()) || 0);
    if (vatp > 0) {
        vatamount = (subtotal * vatp) / 100;
        $('#vatamount').val(roundoff(vatamount));
    }
    $('#subtotal').val(roundoff(subtotal));
    $('#vatamount').val(roundoff(vatamount));
    $('#grandtotal').val(roundoff(subtotal + vatamount));
}

function FillUOM(row) {

    for (i = 0; i < _Materials.length; i++) {
        if (_Materials[i].ItemCode == EG_GridData[row - 1]['Material']) {
            EG_GridData[row - 1]['UOM'] = _Materials[i].UOM;
            EG_GridData[row - 1]['MaterialID'] = _Materials[i].ID;
            EG_GridData[row - 1]['Description'] = _Materials[i].Description;
            EG_GridData[row - 1]['Rate'] = _Materials[i].SellingRate;
            EG_Rebind();
            break;
        }
    }

}
//---------------Bind Invoices List-------------------
function GetAllOtherSCReceipt() {
    try {
        var data = {};
        var ds = {};
        ds = GetDataFromServer("IssueToOtherSC/GetAllIssueToOtherSC/", data);

        if (ds != '') {

            ds = JSON.parse(ds);
        }
        if (ds.Result == "OK") {
            //  
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

function getMaterials() {


    try {

        var data = {};
        var ds = {};
        ds = GetDataFromServer("Item/ItemsForDropdown/", data);
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


//---------------------------------------Edit Item--------------------------------------------------//
function Edit(currentObj) {

    var rowData = DataTables.tblInvoicesList.row($(currentObj).parents('tr')).data();
    //Event Request Case
    if ((rowData != null) && (rowData.ID != null)) {

        EG_ClearTable();
        $('#AddTab').trigger('click');
        $("#HeaderID").val(rowData.ID);
        if (BindOtherSCReceipt(rowData.ID)) {
            ChangeButtonPatchView('IssueToOtherSC', 'btnPatchIssueToOtherSCSettab', 'Edit');

        }
        else {
            $('#ListTab').trigger('click');
        }

    }


}

function reset() {
    if (($("#HeaderID").val() == "") || ($("#HeaderID").val() == 'undefined') || ($("#HeaderID").val() == "0")) {


        $("#InvoiceNo").val("");
        $("#ToSCName").val("");
        $("#Remarks").val("");
        $("#subtotal").val("");
        $("#vatamount").val("");
        $("#vatpercentage").val("");
        $("#grandtotal").val("");
        $('#InvoiceNo').attr('readonly', false);
        var $datepicker = $('#InvoiceDate');
        $datepicker.datepicker('setDate', null);
        EG_ClearTable();
        EG_AddBlankRows(5);
        ResetForm();
    }
    else {
        BindOtherSCReceipt($("#HeaderID").val());
    }
}

function BindOtherSCReceipt(id) {
    try {
        var data = { "ID": id };
        var ds = {};
        ds = GetDataFromServer("IssueToOtherSC/GetIssueToOtherSCByID/", data);
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        if (ds.Result == "OK") {

            BindOtherSCReceiptFields(ds.Records);
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

function BindOtherSCReceiptFields(Records) {
    try {

        ChangeButtonPatchView('IssueToOtherSC', 'btnPatchIssueToOtherSCSettab', 'Edit');
        $('#HeaderID').val(Records.ID);

        $("#InvoiceNo").val(Records.InvoiceNo);
        $("#ToSCName").val(Records.ToSCName);
        $("#Remarks").val(Records.Remarks);
        $("#subtotal").val(roundoff(Records.Subtotal));
        $("#vatamount").val(roundoff(Records.VATAmount));

        $("#grandtotal").val(roundoff(Records.GrandTotal));

        EG_Rebind_WithData(Records.IssueToOtherSCDetail, 1);
        $('#InvoiceNo').attr('readonly', 'readonly');

        var $datepicker = $('#InvoiceDate');
        $datepicker.datepicker('setDate', new Date(Records.InvoiceDate));

    } catch (e) {
        notyAlert('error', e.message);
    }




}
//-----------------------------------------Reset Validation Messages--------------------------------------//
function ResetForm() {
    var validator = $("#IssueToOtherScForm").validate();
    $('#IssueToOtherScForm').find('.field-validation-error span').each(function () {
        validator.settings.success($(this));
    });
    validator.resetForm();
}

function BindAllOtherSCReceipt() {
    try {

        DataTables.tblInvoicesList.clear().rows.add(GetAllOtherSCReceipt()).draw(false);
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

function SaveSuccess(data, status) {

    var JsonResult = JSON.parse(data)
    switch (JsonResult.Result) {
        case "OK":
            BindOtherSCReceipt(JsonResult.Records.ID);
            BindAllOtherSCReceipt();
            notyAlert('success', JsonResult.Message);
            break;
        case "ERROR":
            notyAlert('error', JsonResult.Message);
            break;
        default:
            notyAlert('error', JsonResult.Message);
            break;
    }
}
//--------------------button actions ----------------------
function List() {
    try {

        ChangeButtonPatchView('IssueToOtherSC', 'btnPatchIssueToOtherSCSettab', 'List');
        $("#HeaderID").val("");
        reset();

        BindAllCustomerBill();
    } catch (x) {
        // alert(x);
    }

}

function save() {
    debugger;
    $("#ID").val(emptyGUID);
    var validation = EG_Validate();
    if (validation == "") {

        var result = JSON.stringify(EG_GridData);
        $("#DetailJSON").val(result);
        $("#btnSave").trigger('click');
    }
    else {
        notyAlert('error', validation);
    }

}


function Add() {


    ChangeButtonPatchView('IssueToOtherSC', 'btnPatchIssueToOtherSCSettab', 'Add');
    EG_ClearTable();
    EG_AddBlankRows(5)
    $("#HeaderID").val("");
    reset();
}


function DeleteClick() {
    notyConfirm('Are you sure to delete?', 'OtherSCReceiptDelete()', '', "Yes, delete it!");
}

function OtherSCReceiptDelete() {
    try {
        var id = $('#HeaderID').val();
        if (id != '' && id != null) {
            var data = { "ID": id };
            var ds = {};
            ds = GetDataFromServer("IssueToOtherSC/DeleteIssueToOtherSC/", data);
            if (ds != '') {
                ds = JSON.parse(ds);
            }
            if (ds.Result == "OK") {
                notyAlert('success', ds.Message);
                BindAllOtherSCReceipt();
                $('#ListTab').trigger('click');
            }
            if (ds.Result == "ERROR") {
                notyAlert('error', ds.Message);
                return 0;
            }
            return 1;
        }

    }
    catch (e) {
        notyAlert('error', e.message);
        return 0;
    }

}


function DeleteItem(currentObj) {

    var rowData = EG_GridDataTable.row($(currentObj).parents('tr')).data();

    if ((rowData != null) && (rowData.ID != null)) {
        notyConfirm('Are you sure to delete?', 'OtherScReceiptDetailDelete("' + rowData.ID + '","' + rowData[EG_SlColumn] + '")', '', "Yes, delete it!");

    }
}

function OtherScReceiptDetailDelete(id, rw) {
    try {

        var Hid = $('#HeaderID').val();
        if (id != '' && id != null && Hid != '' && Hid != null && Hid != emptyGUID) {
            var data = { "ID": id, "HeaderID": Hid };
            var ds = {};
            ds = GetDataFromServer("IssueToOtherSC/DeleteIssueToOtherSCDetail/", data);
            if (ds != '') {
                ds = JSON.parse(ds);
            }
            if (ds.Result == "OK") {
                notyAlert('success', ds.Message);
                BindOtherSCReceipt(Hid);
            }
            if (ds.Result == "ERROR") {
                notyAlert('error', ds.Message);
                return 0;
            }
            return 1;
        }
        else {

            if (EG_GridData.length != 1) {
                EG_GridData.splice(rw - 1, 1);
                EG_Rebind_WithData(EG_GridData, 0);
            }
            else {
                reset();
                EG_Rebind();
            }
            notyAlert('success', 'Deleted Successfully');

        }

    }
    catch (e) {
        notyAlert('error', e.message);
        return 0;
    }

}