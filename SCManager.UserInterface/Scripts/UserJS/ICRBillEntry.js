var DataTables = {};
var emptyGUID = '00000000-0000-0000-0000-000000000000'
var _Materials = [];
//---------------------------------------Docuement Ready--------------------------------------------------//
$(document).ready(function () {
    try {
        DataTables.customerBillsTable = $('#tblCustomerBills').DataTable(
 {
     dom: '<"pull-left"f>rt<"bottom"ip><"clear">',
     order: [],
     searching: true,
     paging: true,
     data: GetAllICRBill(),
     columns: [
       { "data": "ID", "defaultContent": "<i>-</i>" },
       { "data": "Technician", "defaultContent": "<i>-</i>" },
       { "data": "JobNo", "defaultContent": "<i>-</i>" },
       { "data": "ICRDateFormatted", "defaultContent": "<i>-</i>" },
       { "data": "ICRNo", "defaultContent": "<i>-</i>" },
       { "data": "CustomerName", "defaultContent": "<i>-</i>" },
       { "data": "CustomerContactNo", "defaultContent": "<i>-</i>" },
        { "data": "ModelNo", "defaultContent": "<i>-</i>" },
          { "data": "SerialNo", "defaultContent": "<i>-</i>" },
       { "data": "STAmount", "defaultContent": "<i>-</i>" },
       { "data": "Remarks", "defaultContent": "<i>-</i>" },
       { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink"  onclick="Edit(this)" ><i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>' }
     ],
     columnDefs: [{ "targets": [0], "visible": false, "searchable": false },
          { className: "text-right", "targets": [9] },
     { className: "text-center", "targets": [1, 2, 3, 4, 9, 5, 6, 7, 8] }

     ]
 });
        debugger;
        DataTables.ICRBillDetail = $('#tblICRDetails').DataTable(
       {

           dom: '<"pull-left"f>rt<"bottom"ip><"clear">',
           order: [],
           searching: false,
           paging: false,
           data: null,
           columns: EG_Columns(),
           columnDefs: EG_Columns_Settings()
       });
        var $datepicker = $('#ICRDate');
        $datepicker.datepicker('setDate', null);
        var $datepicker = $('#AMCValidFromDate');
        $datepicker.datepicker('setDate', null);
        var $datepicker = $('#AMCValidtoDate');
        $datepicker.datepicker('setDate', null);
        getMaterials();
        EG_ComboSource('Materials', _Materials, 'ItemCode', 'Description')
        EG_GridDataTable = DataTables.ICRBillDetail;
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
var EG_MandatoryFields = 'Quantity,Rate'

function EG_TableDefn() {

    var tempObj = new Object();
    tempObj.SCCode = "";
    tempObj.ID = "";
    tempObj.SlNo = 0;
    tempObj.Material = "";
    tempObj.Quantity = "";
    tempObj.UOM = "";
    tempObj.Rate = "";
    tempObj.NetAmount = "";

    return tempObj
}

function EG_Columns() {
    debugger;
    var obj = [

                { "data": "ID", "defaultContent": "<i>0</i>" },               
                { "data": "SlNo", "defaultContent": "<i></i>" },
                { "data": "Material", render: function (data, type, row) { return (EG_createCombo(data, 'S', row, 'Material', 'Materials', 'FillUOM')); } },
                { "data": "Quantity", render: function (data, type, row) { return (EG_createTextBox(data, 'N', row, 'Quantity', 'CalculateAmount')); }, "defaultContent": "<i></i>" },
                { "data": "UOM", "defaultContent": "<i></i>" },
                { "data": "Rate", render: function (data, type, row) { return (EG_createTextBox(data, 'F', row, 'Rate', 'CalculateAmount')); }, "defaultContent": "<i></i>" },

                { "data": "NetAmount", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i></i>" },
                { "data": null, "orderable": false, "defaultContent": '<a href="#" class="DeleteLink"  onclick="DeleteItem(this)" ><i class="glyphicon glyphicon-trash" aria-hidden="true"></i></a>' }

    ]

    return obj

}

function EG_Columns_Settings() {

    var obj = [
        { "targets": [0], "visible": false, "searchable": false }, 
        
        { className: "text-right", "targets": [5,6] },
        { className: "text-center", "targets": [1,2,3, 4] },
        { className: "text-right disabled", "targets": [5,6] },
        { className: "text-center disabled", "targets": [7] },
        { "orderable": false, "targets": [0, 1, 2, 3, 4, 7] }

    ]

    return obj;

}

//------------------------------------------------------------------

//--------------------button actions ----------------------
function List() {
    try {

        ChangeButtonPatchView('ICRBillEntry', 'btnPatchICRBillEntrySettab', 'List');
        // DataTables.customerBillsTable.clear().rows.add(GetAllForm8()).draw(false);
        reset();
        BindAllCustomerBill();
    } catch (x) {
        // alert(x);
    }

}

function goBack() {
    $('#AddTab').trigger('click');
    $("#ID").val("");
    reset();
}

//---------------------------------------Edit Item--------------------------------------------------//
function Edit(currentObj) {
    debugger;
    var rowData = DataTables.customerBillsTable.row($(currentObj).parents('tr')).data();
    //Event Request Case
    if ((rowData != null) && (rowData.ID != null)) {

        EG_ClearTable();
        $('#AddTab').trigger('click');
        debugger;
        if (BindICRBillEntry(rowData.ID)) {
            ChangeButtonPatchView('ICRBillEntry', 'btnPatchICRBillEntrySettab', 'Edit');

        }
        else {
            $('#ListTab').trigger('click');
        }

    }


}

function Add() {
    debugger;

    ChangeButtonPatchView('ICRBillEntry', 'btnPatchICRBillEntrySettab', 'Add');
    EG_ClearTable();
    // RestForm8();
    EG_AddBlankRows(5)
    $("#ID").val("");
    reset();
}
function BindAllCustomerBill() {
    try {
        debugger;
        DataTables.customerBillsTable.clear().rows.add(GetAllICRBill()).draw(false);
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}
function getMaterials() {


    try {

        var data = {};
        var ds = {};
        ds = GetDataFromServer("Item/ServiceTypesItemsForDropdown/", data);
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
function save() {
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
function reset()
{
    if ($("#ID").val() == "")
    {
        $('#HeaderID').val("");
        $("#EmpID").val("");
        $("#TechEmpID").val("");
        $("#JobNo").val("");
        $("#ICRNo").val("");
        $("#CustomerName").val("");
        $("#CustomerContactNo").val("");
        $("#CustomerLocation").val("");
        $("#PaymentMode").val("");
        $("#Remarks").val("");
        $("#subtotal").val("");
        $("#STAmount").val("");
        $("#TotalServiceTaxAmt").val("");
        $("#Discount").val("");
        $("#ModelNo").val("");
        $("#SerialNo").val("");
        $("#grandtotal").val("");
        $('#ICRNo').attr('readonly', false);
        var $datepicker = $('#ICRDate');
        $datepicker.datepicker('setDate', null);
        var $datepicker = $('#AMCValidFromDate');
        $datepicker.datepicker('setDate', null);
        var $datepicker = $('#AMCValidtoDate');
        $datepicker.datepicker('setDate', null);
        EG_ClearTable();
        EG_AddBlankRows(5);
        ResetForm();
    }
  
}
//-----------------------------------------Reset Validation Messages--------------------------------------//
function ResetForm() {
    var validator = $("#ICR").validate();
    $('#ICR').find('.field-validation-error span').each(function () {
        validator.settings.success($(this));
    });
    validator.resetForm();
}
function DeleteItem(currentObj) {
    debugger;
    var rowData = EG_GridDataTable.row($(currentObj).parents('tr')).data();

    if ((rowData != null) && (rowData.ID != null)) {
        notyConfirm('Are you sure to delete?', 'ICRBillDetailDelete("' + rowData.ID + '","' + rowData[EG_SlColumn] + '")', '', "Yes, delete it!");
        //  notyConfirm('Are you sure to delete?', 'TCRBillDetailDelete("' + rowData.ID + '",' + rowData[EG_SlColumn] + ')','', "Yes, delete it!");
    }
}

function AddJobNo()
{

}

function BindICRBillEntryFields(Records) {
    try {

        debugger;
        $('#HeaderID').val(Records.ID);
        $("#EmpID").val(Records.EmpID);
        $("#TechEmpID").val(Records.EmpID);
        $("#JobNo").val(Records.JobNo);
        $("#ICRNo").val(Records.ICRNo);
        $("#CustomerName").val(Records.CustomerName);
        $("#CustomerContactNo").val(Records.CustomerContactNo);
        $("#CustomerLocation").val(Records.CustomerLocation);
        $("#PaymentMode").val(Records.PaymentMode);
        $("#Remarks").val(Records.Remarks);       
        $("#Discount").val(roundoff(Records.Discount));
        $("#ModelNo").val(Records.ModelNo);
        $("#SerialNo").val(Records.SerialNo);
        $("#AMCValidFromDate").val(Records.AMCValidFromDate);
        $("#AMCValidtoDate").val(Records.AMCValidToDate);
        $('#TotalServiceTaxAmt').val(roundoff(Records.TotalServiceTaxAmt));
        $("#subtotal").val(roundoff(Records.STAmount))
        $("#grandtotal").val(roundoff(Records.GrandTotal));
        EG_Rebind_WithData(Records.ICRBillEntryDetail, 1);
        $('#ICRNo').attr('readonly', 'readonly');

        var $datepicker = $('#ICRDate');
        $datepicker.datepicker('setDate', new Date(Records.ICRDate));
        var $datepicker = $('#AMCValidFromDate');
        $datepicker.datepicker('setDate', new Date(Records.AMCValidFromDate));
        var $datepicker = $('#AMCValidtoDate');
        $datepicker.datepicker('setDate', new Date(Records.AMCValidToDate));

    } catch (e) {
        notyAlert('error', e.message);
    }
}


function DeleteClick() {
    notyConfirm('Are you sure to delete?', 'ICRBillDelete()');
}
function FillUOM(row) {
    debugger;
    for (i = 0; i < _Materials.length; i++) {
        if (_Materials[i].ItemCode == EG_GridData[row - 1]['Material']) {
            EG_GridData[row - 1]['UOM'] = _Materials[i].UOM;
            EG_GridData[row - 1]['MaterialID'] = _Materials[i].ID;
            EG_Rebind();
            break;
        }
    }

}
function ICRBillDelete() {
    try {
        var id = $('#HeaderID').val();
        if (id != '' && id != null) {
            var data = { "ID": id };
            var ds = {};
            ds = GetDataFromServer("ICRBillEntry/DeleteICRBillEntry/", data);
            if (ds != '') {
                ds = JSON.parse(ds);
            }
            if (ds.Result == "OK") {
                notyAlert('success', ds.Message);
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
function ICRBillDetailDelete(id, rw) {
    try {
        debugger;
        var Hid = $('#HeaderID').val();
        if (id != '' && id != null && Hid != '' && Hid != null && Hid != emptyGUID) {
            var data = { "ID": id, "HeaderID": Hid };
            var ds = {};
            ds = GetDataFromServer("ICRBillEntry/DeleteICRBillDetail/", data);
            if (ds != '') {
                ds = JSON.parse(ds);
            }
            if (ds.Result == "OK") {
                notyAlert('success', ds.Message);
                BindICRBillEntry(Hid);
            }
            if (ds.Result == "ERROR") {
                notyAlert('error', ds.Message);
                return 0;
            }
            return 1;
        }
        else {
            debugger;
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
function BindICRBillEntry(id) {
    try {
        var data = { "ID": id };
        var ds = {};
        ds = GetDataFromServer("ICRBillEntry/GetICRBillHeaderByID/", data);
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        if (ds.Result == "OK") {

            BindICRBillEntryFields(ds.Records);
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
function GetAllICRBill() {
    try {
        debugger;
        var data = {};
        var ds = {};
        ds = GetDataFromServer("ICRBillEntry/GetAllICRBillEntry/", data);
        debugger;
        if (ds != '') {
            debugger;
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
function SaveSuccess(data, status) {
    debugger;
    var JsonResult = JSON.parse(data)
    switch (JsonResult.Result) {
        case "OK":
            if ($("#ID").val() == emptyGUID) {
                BindICRBillEntryFields(JsonResult.Records.itemID);
            }
            else {
                BindICRBillEntryFields($("#ID").val());
            }
            BindAllCustomerBill();
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
    EGdic = EG_GridData[row - 1]['Discount'];

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
    EG_GridData[row - 1]['NetAmount'] = roundoff(qty * rate);
    EG_GridData[row - 1]['Discount'] = roundoff(dic);
    EG_Rebind();

    var total = 0.00;
    for (i = 0; i < EG_GridData.length; i++) {
        total = total + (parseFloat(EG_GridData[i]['NetAmount']) || 0);
    }

    $('#subtotal').val(roundoff(total));
    $("#STAmount").val(roundoff(total));
    AmountSummary();

}

function AmountSummary() {
    debugger;
    var subtotal = parseFloat($('#subtotal').val()) || 0;
    var vatamount = parseFloat($('#TotalServiceTaxAmt').val()) || 0;
    var discount = parseFloat($('#Discount').val()) || 0;
    $('#grandtotal').val(roundoff(subtotal + vatamount - discount));
}





function AddTechnicanJob() {
    var techi = $("#TechEmpID").val();

    if ((techi)) {
        $("#AddJobModel").modal('show');
        $("#TechnicianLabel").text($("#EmpID option:selected").text());
        $("#ServiceDateLabel").text('Date not selected');
        ClearJobForm();
        $(".calltypehidden").hide();
    }
    else {
        notyAlert('error', 'Please Choose Technician and Service Date');
    }

}
function TechnicianSelectOnChange(curobj) {
    try {
        var v = $(curobj).val();
        $("#TechEmpID").val(v);
    }
    catch (e) {
        notyAlert('error', e.Message);
    }
}
