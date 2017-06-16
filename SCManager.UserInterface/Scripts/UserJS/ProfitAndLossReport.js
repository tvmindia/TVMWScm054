var DataTables = {};
var appAddress = window.location.protocol + "//" + window.location.host + "/";   //Retrieving browser Url 
$(document).ready(function () {
    try {

        DataTables.ProfitAndLossTable = $('#tblProfitAndLoss').DataTable(
         {
             dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
             buttons: [{
                 extend: 'excel',
                 exportOptions:
                              {
                                   columns: [0,1,2]
                              }
             }],
            
             order: [],
             ordering:false,
             searching: false,
             paging: false,
             data: GetProfitAndLossSummary(),
             columns: [

              
               { "data": "Type", "defaultContent": "<i>-</i>" },
               { "data": "Description", "defaultContent": "<i>-</i>" },
               { "data": "Amount", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
               { "data": "BaseType", createdRow: function (row, data, dataIndex) { if (data == "T") { $(row).addClass('important'); } }, "defaultContent": "<i>-</i>" },
             

             ],
             
             columnDefs: [


                  { className: "text-right", "targets": [2] },
                    { "visible":false,"targets":[3] },
                    { className: "text-left", "targets": [0,1] },

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


function GetProfitAndLossSummary() {
    try {
        var frdate = $("#fromdate").val();
        var todate = $("#todate").val();
        var data = {};
        if ((frdate) && (todate)) {
            data = { "fromdate": frdate, "todate": todate };
        }
        var EmptyArr = [];
        var ds = {};
        ds = GetDataFromServer("Report/GetProfitAndLossReport/", data);
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        if (ds.Result == "OK") {
            
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
function RefreshProfitAndLossSummaryTable() {
    try {
        var fromdate = $("#fromdate").val();
        var todate = $("#todate").val();
        if (fromdate && todate && DataTables.ProfitAndLossTable) {
            DataTables.ProfitAndLossTable.clear().rows.add(GetProfitAndLossSummary()).draw(false);
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

