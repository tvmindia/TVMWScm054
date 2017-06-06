var DataTables = {};
$(document).ready(function () {

    try
    {
        DataTables.SalaryTable = $('#tblSalaryCalculation').DataTable(
        {
            dom: '<"pull-left"f>rt<"bottom"ip><"clear">',
            order: [],
            searching: true,
            paging: false,
            data: GetAllTechniciansSalary(),
            autoWidth: false,
            columns: [
              { "data": "EmpID", "defaultContent": "<i>-</i>" },
              { "data": "Name", "defaultContent": "<i>-</i>" },
              { "data": "TotalCommission",render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
              { "data": "SalaryAdvance", render: function (data, type, row) { return roundoff(data, 1); },"defaultContent": "<i>-</i>" },
              { "data": "TotalPayable", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
              { "data": "MajorCalls", "defaultContent": "<i>-</i>" },
              { "data": "MajorCommission", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
              { "data": "DemoCalls", "defaultContent": "<i>-</i>" },
              { "data": "DemoCommission", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
              { "data": "MandatoryCalls", "defaultContent": "<i>-</i>" },
              { "data": "MandatoryCommission", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
              { "data": "MinorCalls", "defaultContent": "<i>-</i>" },
              { "data": "MinorCommission", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
              { "data": "RepeatCalls", "defaultContent": "<i>-</i>" },
              { "data": "RepeatCommission", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
              { "data": "RepeatDeductCalls", "defaultContent": "<i>-</i>" },
              { "data": "RepeatDeductCommission", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
         
              { "data": "SpecialCommission", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
              { "data": "ServiceChargeCommission", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
              { "data": "ProductCommission", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
              { "data": "AMCCommission", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
            ],
            columnDefs: [{ "targets": [0], "visible": false, "searchable": false },
            { "width": "30%", "targets": 1 }, 
            { className: "text-left", targets: [1] },
            { className: "text-center", targets: [2, 3, 4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19]}

            ]
            ,
            scrollY: false,
            scrollX: true,
            scrollCollapse: true,
            fixedColumns: true
        });
    }
    catch(e)
    {
        notyAlert('error', e.message);
    }


});


function RefreshSalaryTable() {
    try {

    
        DataTables.SalaryTable.clear().rows.add(GetAllTechniciansSalary()).draw(false);
       

    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

function GetAllTechniciansSalary() {
    try {

        var data = {};
        var ds = {};
        ds = GetDataFromServer("TechnicianSalaryCalculation/GetTechniciansCalculatedSalary/", data);
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        if (ds.Result == "OK") {
            return ds.Records;
        }
        if (ds.Result == "ERROR") {
            notyAlert('error', ds.Message);
            var emptyarr = [];
            return emptyarr;

        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}
