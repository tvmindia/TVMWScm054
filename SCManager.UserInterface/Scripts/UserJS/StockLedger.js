var DataTables = {};
var appAddress = window.location.protocol + "//" + window.location.host + "/";   //Retrieving browser Url 
$(document).ready(function () {
    try {
       
        DataTables.LedgerTable = $('#tblLedger').DataTable(
         {
             dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
             buttons: [{
                 extend: 'excel',
                 exportOptions:
                              {
                                  columns: [2, 3, 4, 5, 6, 7, 8]
                              }
             }],
             order: [],
             searching: true,
             paging: true,
             pageLength: 50,
             language: {
                 search: "_INPUT_",
                 searchPlaceholder: "Search Items..."
             },
             //dom: '<lf<t>ip>',
             data: GetAllStockLedger(),
             columns: [

               { "data": "GroupCode", "defaultContent": "<i>-</i>" },
               { "data": "SCCode", "defaultContent": "<i>-</i>" },
               { "data": "ItemCode", "defaultContent": "<i>-</i>" },
               { "data": "Description", "defaultContent": "<i>-</i>" },
               { "data": "Type", "defaultContent": "<i>-</i>" },
               { "data": "RefNo", "defaultContent": "<i>-</i>" },
               { "data": "Qty", "defaultContent": "<i>-</i>" },
               { "data": "Location", "defaultContent": "<i>-</i>" },
               { "data": "logDetails", render: function (data, type, row) { return ConvertJsonToDate(data.CreatedDate) }, "defaultContent": "<i>-</i>" }

             ],
             columnDefs: [
                     { "targets": [0,1], "searchable": false },
                    { "targets": [0, 1], "visible": false },
                    { "targets": [4, 5, 6, 7, 8], "searchable": true },
                    { "targets": [2, 3], "visible": false, "searchable": true },
                    {orderable:false, className: "text-center", "targets": [6, 8] },
                    {orderable:false, className: "text-left", "targets": [4,5,7] }],
             drawCallback: function (settings) {
                 var api = this.api();
                 var rows = api.rows({ page: 'current' }).nodes();
                 var last = null;

                 api.column(0, { page: 'current' }).data().each(function (group, i) {
                     if (last !== group) {
                         $(rows).eq(i).before(
                             '<tr class="group "><td colspan="5" class="rptGrp">' + '<b>Item</b> : ' + group + '</td></tr>'
                         );

                         last = group;
                     }
                 });
             }
         });

      

        //hide button of jquery datatable
        $(".buttons-print").hide();
        $(".buttons-excel").hide();
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
function PrintTableToDoc() {
    try {
       $(".buttons-excel").trigger('click');
     }
    catch (e) {
        notyAlert('error', e.message);
    }
}


 