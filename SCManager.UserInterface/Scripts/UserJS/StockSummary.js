var DataTables = {};
var appAddress = window.location.protocol + "//" + window.location.host + "/";   //Retrieving browser Url 
$(document).ready(function () {
    try {

        DataTables.itemTable = $('#tblItemList').DataTable(
         {
             dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
             buttons: [{
                 extend: 'excel',
                 exportOptions:
                              {
                                  columns: [2, 3, 4, 5,6,7,8]
                              }
             }],
             language: {
                 search: "_INPUT_",
                 searchPlaceholder: "Search Items..."
             },
             order: [],
             searching: true,
             paging: true,
             pageLength: 50,
             data: GetItemsSummary(),
             columns: [
              
               { "data": "ID", "defaultContent": "<i>-</i>" },
               { "data": null, "defaultContent": "<i>-</i>" },
               { "data": "ItemCode", "defaultContent": "<i>-</i>" },
               { "data": "Description", "defaultContent": "<i>-</i>" },              
               { "data": "Category", "defaultContent": "<i>-</i>" },
               { "data": "Stock", "defaultContent": "<i>-</i>" },
               { "data": "UOM", "defaultContent": "<i>-</i>" },
               { "data": "SellingRate",render: function (data, type, row) { return roundoff(data, 1); },"defaultContent": "<i>-</i>" },
               { "data": "Value",render: function (data, type, row) { return roundoff(data, 1); },"defaultContent": "<i>-</i>" },
              { "data": "Remarks", "defaultContent": "<i>-</i>" },
             ],
             columnDefs: [{ "width": "20%", "targets": 3 },
                 { "width": "20%", "targets": 9 },
                 { "targets": [0], "visible": false, "searchable": false },
                  { className: "text-right", "targets": [7, 8], "searchable": true },
                    { className: "text-center", "targets": [5, 6], "searchable": true },
                    { className: "text-left", "targets": [2, 3, 4,9], "searchable": true },

             ]
         });

        DataTables.itemTable.on('order.dt search.dt', function () {
            DataTables.itemTable.column(1, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
                cell.innerHTML = i + 1;
            });
        }).draw();

        $(".buttons-print").hide();
        $(".buttons-excel").hide();
    }
    catch (e) {
        notyAlert('error', e.message);
    }
});


function GetItemsSummary() {
    try {
        var fromdate = $("#fromdate").val();
        var todate = $("#todate").val();
        var data = {};
        if (IsVaildDateFormat(fromdate) && IsVaildDateFormat(todate))
        {
            data = { "fromdate": fromdate, "todate": todate };
        }
        
        var ds = {};
        ds = GetDataFromServer("Report/GetItemsSummary/", data);
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



function RefreshItemSummaryTable()
{
    try
    {
        var fromdate = $("#fromdate").val();
        var todate = $("#todate").val(); 
        if (IsVaildDateFormat(fromdate) && IsVaildDateFormat(todate) &&  DataTables.itemTable!=undefined) {
            DataTables.itemTable.clear().rows.add(GetItemsSummary()).draw(false);
        }
    }
    catch(e)
    {
        notyAlert('error', e.message);
    }
}

function UnderConstruction()
{
    notyAlert('error', 'Under Construction');
}

function goBack()
{
    window.location = appAddress + "Report/Index/";
}
function PrintTableToDoc() {  
    try {

        $(".buttons-excel").trigger('click');
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

