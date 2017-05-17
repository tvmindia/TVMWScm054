var DataTables = {};
var appAddress = window.location.protocol + "//" + window.location.host + "/";   //Retrieving browser Url 
$(document).ready(function () {
    try {

        DataTables.LedgerTable = $('#tblLedger').DataTable(
         {
             dom: '<"pull-left"f>rt<"bottom"ip><"clear">',
             order: [],
             searching: false,
             paging: true,
             data: GetAllStockLedger(),
             columns: [

               { "data": null, "defaultContent": "<i>-</i>" },
               { "data": "SCCode", "defaultContent": "<i>-</i>" },
               { "data": "ItemCode", "defaultContent": "<i>-</i>" },
               { "data": "Description", "defaultContent": "<i>-</i>" },
               { "data": "Type", "defaultContent": "<i>-</i>" },
               { "data": "RefNo", "defaultContent": "<i>-</i>" },
               { "data": "Qty", "defaultContent": "<i>-</i>" },
               { "data": "Location", "defaultContent": "<i>-</i>" },
               { "data": "logDetails", render: function (data, type, row) { return ConvertJsonToDate(data.CreatedDate) }, "defaultContent": "<i>-</i>" }

             ],
             columnDefs: [{ "targets": [0], "visible": true, "searchable": false },
                  { className: "text-right", "targets": [] },
                    { className: "text-center", "targets": [5, 6, 7, 8] },
                    { className: "text-left", "targets": [2, 3, 4] },

             ]
         });

        DataTables.LedgerTable.on('order.dt search.dt', function () {
            DataTables.LedgerTable.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
                cell.innerHTML = i + 1;
            });
        }).draw();

    }
    catch (e) {
        notyAlert('error', e.message);
    }
});
function GetAllStockLedger() {
    try {
        var frdate = $("#fromdate").val();
        var todate = $("#todate").val();
        var data = {};
        if ((frdate) && (todate)) {
            data = { "fromdate": frdate, "todate": todate };
        }

        var ds = {};
        ds = GetDataFromServer("Report/GetAllStockLedger/", data);
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

function UnderConstruction() {
    notyAlert('error', 'Under Construction');
}

function goBack() {
    window.location = appAddress + "Report/Index/";
}

function RefreshLedgerTable()
{
    try {
        var fromdate = $("#fromdate").val();
        var todate = $("#todate").val();
        if (DataTables.LedgerTable != undefined && fromdate != "" && todate != "") {
            
            DataTables.LedgerTable.clear().rows.add(GetAllStockLedger()).draw(false);
        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}