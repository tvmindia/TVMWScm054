var DataTables = {};
var emptyGUID = '00000000-0000-0000-0000-000000000000'
var _Materials = [];

//---------------------------------------Docuement Ready--------------------------------------------------//
$(document).ready(function () {
    try {

        var EventRequestsViewModel = new Object();
        DataTables.eventTable = $('#tblInvoices').DataTable(
         {
             dom: '<"pull-left"f>rt<"bottom"ip><"clear">',
             order: [],
             searching: true,
             paging: true,
             data: null,
             columns: [
               { "data": "SCCode" },
               { "data": "ID" },
               { "data": "InvoiceNo" },
               { "data": "InvoiceDateFormatted" },
               { "data": "Customer", "defaultContent": "<i>-</i>" },
               { "data": "SPUNo" },
               { "data": "TicketNo" },
               { "data": "SaleOrderNo", "defaultContent": "<i>-</i>" },
               { "data": "TotalBaseValue", render: function (data, type, row) { return roundoff(data); }, "defaultContent": "<i>-</i>" },
               { "data": "VATAmount", render: function (data, type, row) { return roundoff(data); }, "defaultContent": "<i>-</i>" }, 
               { "data": "Remarks", "defaultContent": "<i>-</i>" },
               { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink"  onclick="Edit(this)" ><i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>' }
             ],
             columnDefs: [{ "targets": [0], "visible": false, "searchable": false }, { "targets": [1], "visible": false, "searchable": false },
                  { className: "text-right", "targets": [7, 8, 9] },
                    { className: "text-left", "targets": [4] },
             { className: "text-center", "targets": [2, 3,5,6,10] }

             ]
         });

        $('#tblInvoices tbody').on('dblclick', 'td', function () {

            Edit(this);
        });
      
        DataTables.DetailTable = $('#tblInvDetails').DataTable(
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

        getMaterials();
        EG_ComboSource('Materials', _Materials, 'ItemCode', 'Description')
        EG_GridDataTable = DataTables.DetailTable;
        List();



    } catch (x) {

        notyAlert('error', e.message);

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
                { "data": "SCCode", "defaultContent": "<i></i>" },
                { "data": "ID", "defaultContent": "<i>0</i>" },
                 { "data": "MaterialID", "defaultContent": "<i></i>" },
                { "data": "SlNo", "defaultContent": "<i></i>" },
                { "data": "Material", render: function (data, type, row) { return (EG_createCombo(data, 'S', row, 'Material', 'Materials', 'FillUOM')); } },
                { "data": "Description", "defaultContent": "<i></i>" },
                { "data": "Quantity", render: function (data, type, row) { return (EG_createTextBox(data, 'N', row, 'Quantity', 'CalculateAmount')); }, "defaultContent": "<i></i>" },
                { "data": "UOM", "defaultContent": "<i></i>" },
                { "data": "Rate", render: function (data, type, row) { return (EG_createTextBox(data, 'F', row, 'Rate', 'CalculateAmount')); }, "defaultContent": "<i></i>" },
                { "data": "BasicAmount", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i></i>" },
                { "data": "TradeDiscount", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i></i>" },
                { "data": "NetAmount", render: function (data, type, row) {return roundoff(data); }, "defaultContent": "<i></i>" },
                { "data": null, "orderable": false, "defaultContent": '<a href="#" class="DeleteLink"  onclick="DeleteItem(this)" ><i class="glyphicon glyphicon-trash" aria-hidden="true"></i></a>' }

    ]

    return obj

}

function EG_Columns_Settings() {

    var obj = [
        { "targets": [0], "visible": false, "searchable": false }, { "targets": [1], "visible": false, "searchable": false }, { "targets": [2], "visible": false, "searchable": false },
            { "width": "5%", "targets": 3 },
        { "width": "15%", "targets": 4 },
         { "width": "20%", "targets": 5 },
           { "width": "8%", "targets": 6 },
        { "width": "8%", "targets": 7 },
         { "width": "8%", "targets": 8 },
          { "width": "10%", "targets": 9 },
           { "width": "12%", "targets": 10 },
            { "width": "10%", "targets": 11 },
        { className: "text-right", "targets": [8] },
          { className: "text-left disabled", "targets": [5] },
        { className: "text-center", "targets": [3, 4, 6, 12] },
          { className: "text-center disabled", "targets": [7] },
        { className: "text-right disabled", "targets": [ 9, 10,11] },
        { "orderable": false, "targets": [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10,11] }

    ]

    return obj;

}

//------------------------------------------------------------------


//---------------Bind logics-------------------
function GetAllForm8B() {
    try {

        var data = {};
        var ds = {};
        ds = GetDataFromServer("Form8BRetailInvoice/GetAllForm8B/", data);
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

function BindForm8B(id) {
    try {
        var data = { "ID": id };
        var ds = {};
        ds = GetDataFromServer("Form8BRetailInvoice/GetForm8B/", data);
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        if (ds.Result == "OK") {

            BindForm8BFields(ds.Records);
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

function  BindForm8BFields(Records) {
    try {
        $('#HeaderID').val(Records.ID);
        $('#InvNo').val(Records.InvoiceNo);
        $('#Remarks').val(Records.Remarks);
        $('#CNo').val(Records.ChallanNo);
        $('#PONo').val(Records.PONo);
        $('#SONo').val(Records.SaleOrderNo);
        $('#subtotal').val(roundoff(Records.Subtotal));
        $('#vatamount').val(roundoff(Records.VATAmount));
        $('#discount').val(roundoff(Records.VATExpense));
        $('#grandtotal').val(roundoff(Records.GrandTotal));
     
       // $('#InvNo').attr('readonly', 'readonly');
        $('#SPUNo').val(Records.SPUNo);
        $('#TicketNo').val(Records.TicketNo);
        if ((Records.SPUNo) && (Records.TicketNo))
        {
            $("#divCustomer").show();
            $("#Customer").val(Records.Customer);
        }
        $('#CustDel').val(Records.CustomerDelvAddrs);
        $('#CustBill').val(Records.CustomerBillAddrs);


        var $datepicker = $('#InvDate');
        $datepicker.datepicker('setDate', new Date(Records.InvoiceDateFormatted));

        if (Records.ChallanDateFormatted != null) {
            var $datepicker = $('#CDate');
            $datepicker.datepicker('setDate', new Date(Records.ChallanDateFormatted));
        }

        if (Records.PODateFormatted != null) {
            var $datepicker = $('#PODate');
            $datepicker.datepicker('setDate', new Date(Records.PODateFormatted));
        }


        EG_Rebind_WithData(Records.Form8BDetail, 1);

    } catch (e) {
        notyAlert('error', e.message);
    }
}




//--------------------button actions ----------------------
function List() {
    try {
        showLoader();
        ChangeButtonPatchView('Form8BRetailInvoice', 'btnPatchAttributeSettab', 'List');
        DataTables.eventTable.clear().rows.add(GetAllForm8B()).draw(false);
        hideLoader();
    } catch (x) {
        // alert(x);
    }

}

function Add() {
    ResetForm();
    showLoader();
    ChangeButtonPatchView('Form8BRetailInvoice', 'btnPatchAttributeSettab', 'Add');
    EG_ClearTable();
    RestForm8B();
    EG_AddBlankRows(5)

    hideLoader();
}

function DeleteClick() {

    notyConfirm('Are you sure to delete?', 'Form8BDelete()');

}

function Form8BDelete() {
    try { 
        var id = $('#HeaderID').val();
        if (id != '' && id != null) {
            var data = { "ID": id };
            var ds = {};
            ds = GetDataFromServer("Form8BRetailInvoice/DeleteForm8B/", data);
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

function Form8BDetailDelete(id, rw) {
    try {
        var Hid = $('#HeaderID').val();
        if (id != '' && id != null && Hid != '' && Hid != null && Hid != emptyGUID) {
            var data = { "ID": id, "HeaderID": Hid };
            var ds = {};
            ds = GetDataFromServer("Form8BRetailInvoice/DeleteForm8BDetail/", data);
            if (ds != '') {
                ds = JSON.parse(ds);
            }
            if (ds.Result == "OK") {
                notyAlert('success', ds.Message);
                BindForm8B(Hid);
                AmountSummary();
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
            AmountSummary();
            notyAlert('success', 'Deleted Successfully');

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
        notyConfirm('Are you sure to delete?', 'Form8BDetailDelete("' + rowData.ID + '",' + rowData[EG_SlColumn] + ')');
    }
}

function RestForm8B() {
    ClearFields();
    $('#HeaderID').val(emptyGUID);//clear field will make this field model invalid
   // $('#InvNo').removeAttr('readonly')
    var $datepicker = $('#InvDate');
    $datepicker.datepicker('setDate', null);
    var $datepicker = $('#CDate');
    $datepicker.datepicker('setDate', null);
    var $datepicker = $('#PODate');
    $datepicker.datepicker('setDate', null);
}


function Edit(currentObj) {
    showLoader();
    var rowData = DataTables.eventTable.row($(currentObj).parents('tr')).data();
    //Event Request Case
    if ((rowData != null) && (rowData.ID != null)) {

        EG_ClearTable();
        $('#AddTab').trigger('click');
        if (BindForm8B(rowData.ID)) {
            ChangeButtonPatchView('Form8BRetailInvoice', 'btnPatchAttributeSettab', 'Edit');

        }
        else {
            $('#ListTab').trigger('click');
        }

    }
    hideLoader();

}

function save() {
    var validation = EG_Validate();
    if (validation == "") {

        var result = JSON.stringify(EG_GridData);
        $("#DetailJSON").val(result);
        $("#savebutton").trigger('click');
    }
    else {
        notyAlert('error', validation);
    }

}

function reset() {
    EG_ClearTable();
    EG_AddBlankRows(5)
}

//-----------------------------------------Reset Validation Messages--------------------------------------//
function ResetForm() {
    var validator = $("#F8").validate();
    $('#F8').find('.field-validation-error span').each(function () {
        validator.settings.success($(this));
    });
    validator.resetForm();
    $("#divCustomer").hide();
    $("#Customer").val('');
}

function resetCurrent() {
    try {
        var id = $('#HeaderID').val();
        BindForm8B(id);


    } catch (e) {

    }
}

function SaveSuccess(data, status, xhr) {
    var i = JSON.parse(data)
    switch (i.Result) {
        case "OK":
            notyAlert('success', i.Message);

            BindForm8BFields(i.Records)
            ChangeButtonPatchView('Form8BRetailInvoice', 'btnPatchAttributeSettab', 'Edit');

            break;
        case "Error":
            notyAlert('error', i.Message);
            break;
        case "ERROR":
            notyAlert('error', i.Message);
            break;
        default:
            break;
    }
}



//---------------------page related logics----------------------------------- 

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
    dic = qty * rate; ///form8b is for material return from IFB , so value of doc wil be zero, it is done by giving total amount as trade discount.

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

function AmountSummary() {
    var BATotal = 0.00;
    for (i = 0; i < EG_GridData.length; i++) { 
        BATotal = BATotal + (parseFloat(EG_GridData[i]['BasicAmount']) || 0);
    }

    var subtotal = parseFloat($('#subtotal').val()) || 0;
    var vatamount = parseFloat($('#vatamount').val()) || 0;   
    var vatp = (parseFloat($('#vatpercentage').val()) || 0);
    if (vatp > 0) {
        debugger;
        vatamount = (BATotal * vatp) / 100;
        $('#vatamount').val(roundoff(vatamount));
    }

    $('#discount').val(roundoff(vatamount));
    var discount = parseFloat($('#discount').val()) || 0;
    $('#grandtotal').val(roundoff(subtotal + vatamount - discount));
}

var typingFlag = 0;
//function calculateVat() {
//    if (typingFlag == 0) {
//        setTimeout(calculateVatPercentage, 2000);//done to wait till typing over
//        typingFlag = 1;
//    }
//}
function ClearVatPercent()
{
    debugger;
    if ($('#vatamount').val() != $('#VatAmountValue').val())
    $('#vatpercentage').val('');
    $('#discount').val($('#vatamount').val());
}
function calculateVatPercentage() {
    debugger;
    var BATotal = 0.00;
    for (i = 0; i < EG_GridData.length; i++) {
        BATotal = BATotal + (parseFloat(EG_GridData[i]['BasicAmount']) || 0);
    }
    var vatp = parseFloat($('#vatpercentage').val()) || 0;
    var subtotal = parseFloat($('#subtotal').val()) || 0;
    if (vatp > 100) {
        vatp = 100;
        $('#vatpercentage').val(vatp);
    }
    if (vatp < 0) {
        vatp = 0;
        $('#vatpercentage').val(vatp);
    }

    
    $('#vatamount').val(roundoff(BATotal * vatp / 100));
    $('#VatAmountValue').val($('#vatamount').val());
    AmountSummary();
    typingFlag = 0;
}

function FillUOM(row) {

    for (i = 0; i < _Materials.length; i++) {
        if (_Materials[i].ItemCode == EG_GridData[row - 1]['Material']) {
            EG_GridData[row - 1]['UOM'] = _Materials[i].UOM;
            EG_GridData[row - 1]['MaterialID'] = _Materials[i].ID;
            EG_GridData[row - 1]['Description'] = _Materials[i].Description;
            EG_Rebind();
            break;
        }
    }

}



//-----------------------------------------------------------------------------EG_MandatoryFields



