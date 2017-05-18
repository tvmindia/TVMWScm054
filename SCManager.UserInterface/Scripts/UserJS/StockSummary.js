var DataTables = {};
var appAddress = window.location.protocol + "//" + window.location.host + "/";   //Retrieving browser Url 
$(document).ready(function () {
    try {

        DataTables.itemTable = $('#tblItemList').DataTable(
         {
             dom: '<"pull-left"f>rt<"bottom"ip><"clear">',
             order: [],
             searching: false,
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
               { "data": "Value",render: function (data, type, row) { return roundoff(data, 1); },"defaultContent": "<i>-</i>" }
             
             ],
             columnDefs: [{ "targets": [0], "visible": false, "searchable": false },
                  { className: "text-right", "targets": [] },
                    { className: "text-center", "targets": [ 5,6,7,8] },
                    { className: "text-left", "targets": [2,3,4] },

             ]
         });

        DataTables.itemTable.on('order.dt search.dt', function () {
            DataTables.itemTable.column(1, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
                cell.innerHTML = i + 1;
            });
        }).draw();

    }
    catch (e) {
        notyAlert('error', e.message);
    }
});


function GetItemsSummary() {
    try {
        var frdate = $("#fromdate").val();
        var todate = $("#todate").val();
        var data = {};
        if ((frdate) && (todate))
        {
            data = { "fromdate": frdate, "todate": todate };
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

//function countDays() {
//    var fromdate = $("#fromdate").val();
//    var todate = $("#todate").val();
//    if (fromdate != "" && todate != "") {
//        fromdate = ConvertDateFormats(fromdate);
//        todate = ConvertDateFormats(todate);
//        var date1 = new Date(fromdate);
//        var date2 = new Date(todate);
//        var diff = date2.getTime() - date1.getTime();
//        if (diff >= 0) {
//            var ONE_DAY = 1000 * 60 * 60 * 24;
//            $("#dayscount").text((Math.round(diff / ONE_DAY) + 1) + ' Days');
//            RefreshItemSummaryTable();
//        }
//        else {
//            notyAlert('error', 'Please check the dates entered');
//            $("#dayscount").text('');
//            return false;
//        }
//    }
//    else {
//        $("#dayscount").text('');
//    }
//    return true;
//}

function RefreshItemSummaryTable()
{
    try
    {
        var fromdate = $("#fromdate").val();
        var todate = $("#todate").val();
        if (fromdate != "" && todate != "") {
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

