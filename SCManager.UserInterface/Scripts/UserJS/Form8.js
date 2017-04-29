var DataTables = {};

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
               { "data": "SaleOrderNo", "defaultContent": "<i>-</i>" },
               { "data": "Subtotal", "defaultContent": "<i>-</i>" },
               { "data": "VATAmount", "defaultContent": "<i>-</i>" },
               { "data": "Discount", "defaultContent": "<i>-</i>" },
               { "data": "GrandTotal", "defaultContent": "<i>-</i>" },
               { "data": "Remarks", "defaultContent": "<i>-</i>" },
               { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink"  onclick="Edit(this)" ><i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>' }
             ],
             columnDefs: [{ "targets": [0], "visible": false, "searchable": false }, { "targets": [1], "visible": false, "searchable": false },
                  { className: "text-right", "targets": [5, 6, 7, 8] },
             { className: "text-center", "targets": [2, 3, 4, 9, 10] }

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
var EG_GridInputPerRow = 4;

function EG_TableDefn() {

    var tempObj = new Object();
    tempObj.SCCode = "";
    tempObj.ID = "";
    tempObj.MaterialID = "";
    tempObj.SlNo = 0;
    tempObj.Material = "";
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
                { "data": "Quantity", render: function (data, type, row) { return (EG_createTextBox(data, 'N', row, 'Quantity', 'CalculateAmount')); }, "defaultContent": "<i></i>" },
                { "data": "UOM", "defaultContent": "<i></i>" },
                { "data": "Rate", render: function (data, type, row) { return (EG_createTextBox(data, 'F', row, 'Rate', 'CalculateAmount')); }, "defaultContent": "<i></i>" },
                { "data": "BasicAmount", render: function (data, type, row) { return roundoff(data,1); }, "defaultContent": "<i></i>" },
                { "data": "TradeDiscount", render: function (data, type, row) { return (EG_createTextBox(data, 'F', row, 'TradeDiscount', 'CalculateAmount')); }, "defaultContent": "<i></i>" },
                { "data": "NetAmount", "defaultContent": "<i></i>" }

    ]

    return obj

}

function EG_Columns_Settings() {

    var obj = [
        { "targets": [0], "visible": false, "searchable": false }, { "targets": [1], "visible": false, "searchable": false }, { "targets": [2], "visible": false, "searchable": false },
        { "targets": [4], "width": "20%" },
        { className: "text-right", "targets": [7, 8, 9, 10] },
        { className: "text-center", "targets": [3, 4, 5, 6] },
        { "orderable": false, "targets": [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10] }

    ]

    return obj;

}

//------------------------------------------------------------------


//---------------Bind logics-------------------
function GetAllForm8() {
    try {

        var data = {};
        var ds = {};
        ds = GetDataFromServer("Form8TaxInvoice/GetAllForm8/", data);
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

function BindForm8(id) {
    try {
        var data = { "ID": id };
        var ds = {};
        ds = GetDataFromServer("Form8TaxInvoice/GetForm8/", data);
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        if (ds.Result == "OK") {

            BindForm8Fields(ds.Records);
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

function BindForm8Fields(Records) {
    try {

        debugger;
        $('#HeaderID').val(Records.ID);
        $('#InvNo').val(Records.InvoiceNo);        
        $('#Remarks').val(Records.Remarks);
        $('#CNo').val(Records.ChallanNo);        
        $('#PONo').val(Records.PONo);      
        $('#SONo').val(Records.SaleOrderNo);
        $('#subtotal').val(roundoff(Records.Subtotal));
        $('#vatamount').val(roundoff(Records.VATAmount));
        $('#discount').val(roundoff(Records.Discount));
        $('#grandtotal').val(roundoff(Records.GrandTotal));
        EG_Rebind_WithData(Records.Form8Detail);
        $('#InvNo').attr('readonly', 'readonly');

        var $datepicker = $('#InvDate');      
        $datepicker.datepicker('setDate', new Date(Records.InvoiceDateFormatted));

        if (Records.ChallanDateFormatted != null) {
            var $datepicker = $('#CDate');           
            $datepicker.datepicker('setDate', new Date(Records.ChallanDateFormatted));
        }
      
        if (Records.PODateFormatted!=null) {
            var $datepicker = $('#PODate');            
            $datepicker.datepicker('setDate', new Date(Records.PODateFormatted));
        }
      

        

    } catch (e) {
        notyAlert('error', e.message);
    }
   



}




//--------------------button actions ----------------------
function List() {
    try {
        showLoader();
        ChangeButtonPatchView('Form8TaxInvoice', 'btnPatchAttributeSettab', 'List');
        DataTables.eventTable.clear().rows.add(GetAllForm8()).draw(false);
          hideLoader();
    } catch (x) {
       // alert(x);
    }

}

function Add() {

    showLoader();
    ChangeButtonPatchView('Form8TaxInvoice', 'btnPatchAttributeSettab', 'Add');
    EG_ClearTable();
    RestForm8();
    EG_AddBlankRows(5)
    EG_KeyDown();
    hideLoader();
}

function DeleteClick() {
    
   notyConfirm('Are you sure to delete?', 'Form8Delete');       
     
}

function Form8Delete() {    
   try {
        var id = $('#HeaderID').val();
        if (id != '' || id != null) {
            var data = { "ID": id };
            var ds = {};
            ds = GetDataFromServer("Form8TaxInvoice/DeleteForm8/", data);
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
function DeleteItem() {

}

function RestForm8() {
    ClearFields();
    $('#HeaderID').val('00000000-0000-0000-0000-000000000000');//clear field will make this field model invalid
    $('#InvNo').removeAttr('readonly')
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
        if (BindForm8(rowData.ID)) {
            ChangeButtonPatchView('Form8TaxInvoice', 'btnPatchAttributeSettab', 'Edit');
            EG_KeyDown();
        }
        else {
            $('#ListTab').trigger('click');
        }

    }
    hideLoader();
   
}

function save() {
    var validation = validateForm();
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

function resetCurrent() {
    try {
        debugger;
        var id = $('#HeaderID').val();
        BindForm8(id);


    } catch (e) {

    }
}

function SaveSuccess(data, status, xhr) {
    var i = JSON.parse(data)
    switch (i.Result) {
        case "OK":
            notyAlert('success', i.Message);
            BindForm8Fields(i.Records)
            ChangeButtonPatchView('Form8TaxInvoice', 'btnPatchAttributeSettab', 'Edit');
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

    var subtotal = parseFloat($('#subtotal').val()) || 0;
    var vatamount = parseFloat($('#vatamount').val()) || 0;
    var discount = parseFloat($('#discount').val()) || 0;
    var vatp = (parseFloat($('#vatpercentage').val()) || 0);
    if (vatp > 0) {
        vatamount = (subtotal * vatp) / 100;
        $('#vatamount').val(roundoff(vatamount));
    }

    $('#grandtotal').val(roundoff(subtotal + vatamount - discount));
}

var typingFlag = 0;
function calculateVat() {
    if (typingFlag == 0) {
        setTimeout(calculateVatPercentage, 2000);//done to wait till typing over
        typingFlag = 1;
    }
}

function calculateVatPercentage() {
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

function FillUOM(row) {

    for (i = 0; i < _Materials.length; i++) {
        if (_Materials[i].ItemCode == EG_GridData[row - 1]['Material']) {
            EG_GridData[row - 1]['UOM'] = _Materials[i].UOM;
            EG_GridData[row - 1]['MaterialID'] = _Materials[i].ID;
            EG_Rebind();
            break;
        }
    }

}

function validateForm() {

    for (i = 0; i < EG_GridData.length; i++) {
        if (EG_GridData[i]['Material'] != "") {
            return ""
        }
    }

    return "Minimum one detail is required to save";
}

//-----------------------------------------------------------------------------



