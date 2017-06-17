var DataTables = {};
var appAddress = window.location.protocol + "//" + window.location.host + "/";   //Retrieving browser Url 
$(document).ready(function () {
    try {

        DataTables.AMCBaseValueTable = $('#tblAMCBaseValue').DataTable(
         {
             //"dom": '<"top"i>rt<"bottom"flp><"clear">'
             dom: '<"pull-right"Bf><"pull-left"i>rt<"pull-left"p><"clear">',
             buttons: [{
                 extend: 'excel',
                 exportOptions:
                              {
                                 // columns: [2, 3, 4, 5, 6, 7, 8]
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
             data: GetAMCBaseValueSummary(),
             columns: [

               { "data": "ICRDate", "defaultContent": "<i>-</i>" },
               { "data": "ICRNo", "defaultContent": "<i>-</i>" },
               { "data": "AMCNo", "defaultContent": "<i>-</i>" },
               { "data": "AMCValidFromDate", "defaultContent": "<i>-</i>" },
               { "data": "AMCValidToDate", "defaultContent": "<i>-</i>" },
               { "data": "Technician", "defaultContent": "<i>-</i>" },
               { "data": "CustomerName", "defaultContent": "<i>-</i>" },
               { "data": "BaseAmount", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
               { "data": "ServiceCharge", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
               { "data": "Discount", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
               { "data": "Total", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
            
             ],
             columnDefs: [
                 
                 
                  { className: "text-right", "targets": [7,8,9,10] },
                    { className: "text-center", "targets": [0,3,4],  },
                    { className: "text-left", "targets": [1,2,5,6] },

             ]
         });

        //DataTables.itemTable.on('order.dt search.dt', function () {
        //    DataTables.itemTable.column(1, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
        //        cell.innerHTML = i + 1;
        //    });
        //}).draw();

        $(".buttons-print").hide();
        $(".buttons-excel").hide();
    }
    catch (e) {
        notyAlert('error', e.message);
    }
});


function GetAMCBaseValueSummary() {
    try {
        var frdate = $("#fromdate").val();
        var todate = $("#todate").val();
        var data = {};
        if ((frdate) && (todate)) {
            data = { "fromdate": frdate, "todate": todate };
        }
        var EmptyArr = [];
        var ds = {};
        ds = GetDataFromServer("Report/GetAMCBaseValueSummaryReport/", data);
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        if (ds.Result == "OK") {
            if (ds.Record)
            {
                $("#lblsum").text(ds.Record.TotalSum);
            }
            return (ds.Records == null ? EmptyArr : ds.Records);
        }
        if (ds.Result == "ERROR") {
            
            notyAlert('error', ds.Message);
            return EmptyArr;
        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}


function IsValidDate(dateString) {
    debugger;
    // First check for the pattern
    if (!/^\d{1,2}\/\d{1,2}\/\d{4}$/.test(dateString))
        return false;

    // Parse the date parts to integers
    var parts = dateString.split("/");
    var day = parseInt(parts[1], 10);
    var month = parseInt(parts[0], 10);
    var year = parseInt(parts[2], 10);

    // Check the ranges of month and year
    if (year < 1000 || year > 3000 || month == 0 || month > 12)
        return false;
    var monthLength = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];
    // Adjust for leap years
    if (year % 400 == 0 || (year % 100 != 0 && year % 4 == 0))
        monthLength[1] = 29;
    // Check the range of the day
    return day > 0 && day <= monthLength[month - 1];
}
function RefreshAMCBaseValueSummaryTable() {
    try {
        var fromdate = $("#fromdate").val();
        var todate = $("#todate").val();
        if (fromdate&&todate&&DataTables.AMCBaseValueTable) {
            DataTables.AMCBaseValueTable.clear().rows.add(GetAMCBaseValueSummary()).draw(false);
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
function PrintTableToDoc() {
    try {

        $(".buttons-excel").trigger('click');
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

