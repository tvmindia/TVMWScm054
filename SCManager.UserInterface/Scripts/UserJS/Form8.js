var DataTables = {};
var totalDetailRows = 0;
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
           columns: [
             { "data": "SCCode", "defaultContent": "<i></i>" },
             { "data": "ID", "defaultContent": "<i>0</i>" },
             { "data": "SlNo", "defaultContent": "<i></i>" },
             { "data": "Material", "defaultContent": "<i></i>" },
             { "data": "Quantity", "defaultContent": "<i></i>" },
             { "data": "UOM", "defaultContent": "<i></i>" },
             { "data": "Rate", "defaultContent": "<i></i>" },
             { "data": "BasicAmount", "defaultContent": "<i></i>" },
             { "data": "TradeDiscount", "defaultContent": "<i></i>" },
             { "data": "NetAmount", "defaultContent": "<i></i>" }
           ],
           columnDefs: [{ "targets": [0], "visible": false, "searchable": false }, { "targets": [1], "visible": false, "searchable": false },
               { "targets": [3], "width": "30%" },
                { className: "text-right", "targets": [6, 7, 8, 9] },
           { className: "text-center", "targets": [2, 3, 4, 5] }
           ]
       });

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
            debugger;
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


function List() {
    try {
       
        ChangeButtonPatchView('Form8TaxInvoice', 'btnPatchAttributeSettab', 'List');

    } catch (x) {
        alert(x);
    }
  
}

function Add() {
   
   
    ChangeButtonPatchView('Form8TaxInvoice', 'btnPatchAttributeSettab', 'Add');
    GetInvoiceDetailsGrid("0");

}

function reset() {
    GetInvoiceDetailsGrid("0");
}

function GetInvoiceDetailsGrid(id) {
    totalDetailRows = 0;
    if (id == "0") {
        var rows = blankRow(5);
        DataTables.DetailTable.clear().rows.add(rows).draw(false);
    }
  

}

function AddMoreRows() {
    var newrows = 5;
    DataTables.DetailTable.rows.add(blankRow(newrows)).draw(false);
 

}

function blankRow( count) {

    var dataObj = [];
    for (i = 0; i < count;i++)
    {
       
        var tempObj = new Object();
        tempObj.SCCode = "";
        tempObj.ID = "";
        tempObj.SlNo = totalDetailRows + i+1;
        tempObj.Material = "";
        tempObj.Quantity = "";
        tempObj.UOM = "";
        tempObj.Rate = "";
        tempObj.BasicAmount = "";
        tempObj.TradeDiscount = "";
        tempObj.NetAmount = "";      
        dataObj.push(tempObj);

    }
   
    totalDetailRows = totalDetailRows + count;
    return dataObj;

}

 