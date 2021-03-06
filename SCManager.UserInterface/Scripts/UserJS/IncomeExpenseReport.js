﻿var DataTables = {};
var appAddress = window.location.protocol + "//" + window.location.host + "/";   //Retrieving browser Url 
$(document).ready(function () {
    try {

        DataTables.IncomeExpenseTable = $('#tblIncomeExpense').DataTable(
         {
           
             dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
             buttons: [{
                 extend: 'excel',
                 exportOptions:
                {
                    columns: [0,1, 2, 3, 4, 5, 6]
                }
             }],
             language: {
                 search: "_INPUT_",
                 searchPlaceholder: "Search..."
             },
             order: [],
             searching: true,
             paging: true,
             pageLength: 50,
             data: GetIncomeExpenseSummary(),
             columns: [

               { "data": "ReferenceNo", "defaultContent": "<i>-</i>" },
               { "data": "Description", "defaultContent": "<i>-</i>" },
               { "data": "AccountHead", "defaultContent": "<i>-</i>" },
               { "data": "Income",render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
               { "data": "Expense", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
               { "data": "Balance",render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
               { "data": "logDetails.CreatedDatestr", "defaultContent": "<i>-</i>" }

             ],
             columnDefs: [
                      {'searchable': true,'targets': [3, 4, 5, 6]},
                      { className: "text-right", "targets": [3, 4, 5] },
                      { className: "text-center", "targets": [6] },
                      { className: "text-left", "targets": [0, 1, 2], 'searchable': true },

             ]
         });

       

        $(".buttons-print").hide();
        $(".buttons-excel").hide();
    }
    catch (e) {
        notyAlert('error', e.message);
    }

});



function GetIncomeExpenseSummary() {
    try {
        var fromdate = $("#fromdate").val();
        var todate = $("#todate").val();
        var data = {};
        if (IsVaildDateFormat(fromdate) && IsVaildDateFormat(todate) ) {
            data = { "fromdate": fromdate, "todate": todate };
        }

        var ds = {};
        ds = GetDataFromServer("Report/GetMonthlyIncomeExpenseSummary/", data);
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


function RefreshIncomeExpenseSummaryTable()
{
    try {
        var fromdate = $("#fromdate").val();
        var todate = $("#todate").val();
        if (IsVaildDateFormat(fromdate) && IsVaildDateFormat(todate) && DataTables.IncomeExpenseTable != undefined)
        {
            DataTables.IncomeExpenseTable.clear().rows.add(GetIncomeExpenseSummary()).draw(false);
        }
    }
    catch (e) {
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
function PrintTableToDoc()
{
    try {

        $(".buttons-excel").trigger('click');
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}
