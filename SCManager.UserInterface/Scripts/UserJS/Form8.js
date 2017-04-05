var DataTables = {};

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