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
            data: GetAllTechniciansSalaryWithoutDate(),
            autoWidth: false,
            columns: [
             
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
            columnDefs: [
            { "width": "200px", "targets": 0 }, 
            { className: "text-left disabled", targets: [0] },
             { className: "text-right", targets: [1,2,5,7,9,11,13,15,16,17,18,19] },
            { className: "text-center", targets: [6, 8, 10, 12, 14] },
            { className: "text-right disabled", targets: [3]}

            ]
            ,
            scrollY: false,
            scrollX: true,
            //scrollCollapse: true,
            //fixedColumns: true
        });

       
    }
    catch(e)
    {
        notyAlert('error', e.message);
    }


});


function RefreshSalaryTable() {
    try {
        var mon = $("#Month").val();
        var yea = $("#Year").val();
        DataTables.SalaryTable.clear().rows.add(GetAllTechniciansSalary(mon,yea)).draw(false);
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

function GetAllTechniciansSalary(month,year) {
    try {

        var data = { "Month": month,"Year":year };
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

function GetAllTechniciansSalaryWithoutDate() {
    try {

        var data = { };
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



function SalaryCalculate()
{
   
    try {
        var mon = $("#Month option:selected").text();
        var yea = $("#Year").val();
        if (($("#Month").val() != '') && (yea != ''))
        {
            // mon = (mon != '' && mon != '--Select Month--') ? mon : ' - ';
            // yea = yea != '' ? yea : ' - ';
            $("#MsgCalcu").text("Calculated Salary for the month " + mon + "/" + yea);
            RefreshSalaryTable();
        }
        else
        {
            $("#MsgCalcu").text("Please Select Month and Year");
        }
      
       
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}
