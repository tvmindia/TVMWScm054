var DataTables = {};
var ToDate, FromDate,CurrentDate;
var appAddress = window.location.protocol + "//" + window.location.host + "/";   //Retrieving browser Url 
$(document).ready(function () {
    try {
        debugger;

        DataTables.itemTable = $('#tblAMCReportList').DataTable(
         {
             dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
             buttons: [{
                 extend: 'excel',
                 exportOptions:
                              {
                                  columns: [0,1,2, 3, 4, 5, 6, 7, 8,9,10]
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
             data: GetAmcReportTable(),
             columns: [

               { "data": "CustomerName", "defaultContent": "<i>-</i>" },
               { "data": "Location", "defaultContent": "<i>-</i>" },
               { "data": "ContactNo", "defaultContent": "<i>-</i>" },
               { "data": "Model", "defaultContent": "<i>-</i>" },
               { "data": "SerialNo", "defaultContent": "<i>-</i>" },
                 { "data": "AmcNo", "defaultContent": "<i>-</i>" },
               { "data": "AmcStartDate", "defaultContent": "<i>-</i>" },
               { "data": "AmcEndDate", "defaultContent": "<i>-</i>" },
                 { "data": "Remarks", "defaultContent": "<i>-</i>" },
               { "data": "DueDays", "defaultContent": "<i>-</i>" },
               {
                   "data": "DueDays", render: function (data, type, row) {

                       if (data < 0)
                           return "Expired"
                       else if (data >0 && data <= 10)
                           return "Expiring Soon"
                       else if (data ==0 )
                           return "Expiring Today"
                       else
                           return "Active ";
                   }, "defaultContent": "<i>-</i>"
               }

             ],
             columnDefs: [
                    { className: "text-center", "targets": [5,6,7,8,9,10], "searchable": true },
                    { className: "text-left", "targets": [0,1,2,3,4], "searchable": true },

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


function GetAmcReportTable() {
    try {
        debugger;
        var frdate = $("#fromdate").val();
        var todate = $("#todate").val();
        var data = {};
        if ((frdate) && (todate)) {
            data = { "fromdate": frdate, "todate": todate };
        }

        var ds = {};
        ds = GetDataFromServer("Report/GetAmcReportTable/", data);
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



function RefreshAmcReportTable() {
    try {
        debugger;
        var fromdate = $("#fromdate").val();
        var todate = $("#todate").val();
        if (fromdate != "" && todate != "") {
            if (DataTables.itemTable!=undefined)
            DataTables.itemTable.clear().rows.add(GetAmcReportTable()).draw(false);
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

