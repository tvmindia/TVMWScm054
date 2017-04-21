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
             data: GetAllForm8(),
             columns: [
               { "data": "SCCode" },
               { "data": "ID" },
               { "data": "InvoiceNo" },
               { "data": "InvoiceDateFormatted" },
               { "data": "SaleOrderNo", "defaultContent": "<i>-</i>" },
               { "data": "TotalItemsValue", "defaultContent": "<i>-</i>" },
               { "data": "VATAmount", "defaultContent": "<i>-</i>" },
               { "data": "Discount", "defaultContent": "<i>-</i>" },
               { "data": "Total", "defaultContent": "<i>-</i>" },
               { "data": "Remarks", "defaultContent": "<i>-</i>" },
               { "data": null, "orderable": false, "defaultContent": '<a href="#" onclick="Edit(this)"<i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>' }
             ],
             columnDefs: [{ "targets": [0], "visible": false, "searchable": false }, { "targets": [1], "visible": false, "searchable": false },
                  { className: "text-right", "targets": [5,6,7,8] },
             { className: "text-center", "targets": [2,3,4,9,10] }     
                 
                ]
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
         

    } catch (x) {

        notyAlert('error', e.message);

    }

});


//---------------get grid fill result-------------------
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


//--------------------button actions ----------------------
function List() {
    try {
       
        ChangeButtonPatchView('Form8TaxInvoice', 'btnPatchAttributeSettab', 'List');

    } catch (x) {
        alert(x);
    }
  
}

function Add() {
   
   
    ChangeButtonPatchView('Form8TaxInvoice', 'btnPatchAttributeSettab', 'Add');
    EG_ClearTable();
    EG_AddBlankRows(5)

}

function Edit(obj){

}


function reset() {
    EG_ClearTable();
    EG_AddBlankRows(5)
}
//----------------------------------------------------------------
 

 

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
                { "data": "SlNo", "defaultContent": "<i></i>" },
                { "data": "Material", render: function (data, type, row) { return (EG_createCombo(data, 'S', row, 'Material', 'Materials', 'FillUOM')); } },
                { "data": "Quantity", render: function (data, type, row) { return (EG_createTextBox(data, 'N', row, 'Quantity', 'CalculateAmount')); }, "defaultContent": "<i></i>" },
                { "data": "UOM", "defaultContent": "<i></i>" },
                { "data": "Rate", render: function (data, type, row) { return (EG_createTextBox(data, 'F', row, 'Rate', 'CalculateAmount')); }, "defaultContent": "<i></i>" },
                { "data": "BasicAmount", "defaultContent": "<i></i>" },
                { "data": "TradeDiscount", render: function (data, type, row) { return (EG_createTextBox(data, 'F', row, 'TradeDiscount', 'CalculateAmount')); }, "defaultContent": "<i></i>" },
                { "data": "NetAmount", "defaultContent": "<i></i>" }
    ]

    return obj

}

function EG_Columns_Settings() {

    var obj = [
        { "targets": [0], "visible": false, "searchable": false }, { "targets": [1], "visible": false, "searchable": false },
        { "targets": [3], "width": "20%" },
        { className: "text-right", "targets": [6, 7, 8, 9] },
        { className: "text-center", "targets": [2, 3, 4, 5] },
        { "orderable": false, "targets": [0 ,1,2,3,4,5,6,7,8,9]}

    ]

    return obj;

}

//------------------------------------------------------------------


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
        total = total + ( parseFloat(EG_GridData[i]['NetAmount'])||0);
    }

    $('#subtotal').val(roundoff(total));
    AmountSummary();

}

function AmountSummary()
{
    
    var subtotal = parseFloat($('#subtotal').val())||0;
    var vatamount = parseFloat($('#vatamount').val())||0;
    var discount = parseFloat($('#discount').val()) || 0;
    var vatp = (parseFloat($('#vatpercentage').val()) || 0);
    if (vatp > 0) {
        vatamount = (subtotal * vatp) / 100;
        $('#vatamount').val(roundoff(vatamount));
    }
  
    $('#grandtotal').val(roundoff(subtotal+vatamount-discount));
}

function RoundSummary() {
    var vatamount = parseFloat($('#vatamount').val()) || 0;
    var discount = parseFloat($('#discount').val()) || 0;
    $('#vatamount').val(roundoff(vatamount));
    $('#discount').val(roundoff(discount));
}

function calculateVat() {
    var vatp = parseFloat($('#vatpercentage').val()) || 0;
    var subtotal = parseFloat($('#subtotal').val()) || 0;
    $('#vatamount').val(roundoff(subtotal * vatp / 100));
    AmountSummary();
}

function FillUOM(row) {

    for (i = 0; i < _Materials.length; i++) {
        if (_Materials[i].ItemCode == EG_GridData[row - 1]['Material']) {
            EG_GridData[row - 1]['UOM'] = _Materials[i].UOM;
            EG_Rebind();
            break;
        }
    }

}

//-----------------------------------------------------------------------------