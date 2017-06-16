var DataTables = {};
$(document).ready(function () {

    try
    {
        DataTables.SalaryTable = $('#tblSalaryCalculation').DataTable(
        {
            dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
            order: [],          
            paging: false,
            data: GetAllTechniciansSalaryWithoutDate(),
            language: {
                search: "_INPUT_",
                searchPlaceholder: "Search"
            },
            autoWidth: false,
            columns: [
             
              { "data": "Name", "defaultContent": "<i>-</i>" },
              { "data": "TotalCommission",render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
              { "data": "SalaryAdvance", render: function (data, type, row) { return roundoff(data, 1); },"defaultContent": "<i>-</i>" },
              { "data": "TotalPayable", render: function (data, type, row) { var respay = roundoff(data, 1); return "<a data-toggle='tp' data-placement='top' data-delay={'show':2000, 'hide':3000} title='View Details' href='#' class='actionLink' onclick='ViewMore(this)'>" + respay + "</a>"; }, "defaultContent": "<i>-</i>" },
              { "data": "MajorCalls", render: function (data, type, row) { return RemoveZero(data); }, "defaultContent": "<i>-</i>" },
              { "data": "MajorCommission", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
              { "data": "DemoCalls", render: function (data, type, row) { return RemoveZero(data); }, "defaultContent": "<i>-</i>" },
              { "data": "DemoCommission", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
              { "data": "MandatoryCalls", render: function (data, type, row) { return RemoveZero(data); }, "defaultContent": "<i>-</i>" },
              { "data": "MandatoryCommission", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
              { "data": "MinorCalls", render: function (data, type, row) { return RemoveZero(data); }, "defaultContent": "<i>-</i>" },
              { "data": "MinorCommission", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
              { "data": "RepeatCalls", render: function (data, type, row) { return RemoveZero(data); }, "defaultContent": "<i>-</i>" },
              { "data": "RepeatCommission", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
              { "data": "RepeatDeductCalls", render: function (data, type, row) { return RemoveZero(data); }, "defaultContent": "<i>-</i>" },
              { "data": "RepeatDeductCommission", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
         
              { "data": "SpecialCommission", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
              { "data": "ServiceChargeCommission", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
              { "data": "ProductCommission", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
              { "data": "AMCCommission", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
              { "data": "SCCode", "defaultContent": "<i>-</i>" },
              { "data": "EmpID", "defaultContent": "<i>-</i>" },
              { "data": "Month", "defaultContent": "<i>-</i>" },
              { "data": "Year", "defaultContent": "<i>-</i>" },
            ],
            columnDefs: [
            { "width": "200px", "targets": 0 }, 
            { className: "text-left disabled", targets: [0] },
             { className: "text-right", targets: [5,7,9,11,13,15,16,17,18,19] },
            { className: "text-center", targets: [4,6, 8, 10, 12, 14] },
            { className: "text-right disabled", targets: [1, 2, 3] },
            { "visible": false, targets: [20,21,22,23] },

            ]
            ,
            scrollY: false,
            scrollX: true,
            scrollCollapse: true,
            fixedColumns: {
                leftColumns: 4
                
            }

        });

       
    }
    catch(e)
    {
        notyAlert('error', e.message);
    }

    try {


        DataTables.JobCommissionDetails = $('#tblJobCommissionDetails').DataTable(
         {
             dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
             order: [],
             searching: true,
             paging: true,
             pageLength: 8,
             data: null,
             autoWidth:false,
             language: {
                 search: "_INPUT_",
                 searchPlaceholder: "Search"
             },
             columns: [
               { "data": "ServiceDate","defaultContent": "<i>-</i>" },
               { "data": "JobNo", "defaultContent": "<i>-</i>" },
               { "data": "Type", "defaultContent": "<i>-</i>" },
               { "data": "CustomerName", "defaultContent": "<i>-</i>" },
               { "data": "SpecialCommission",render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
               { "data": "Commission",render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
               { "data": "Total",render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
               

             ],
             columnDefs: [
                  { className: "text-left", "targets": [1,2,3] },
                  { className: "text-center", "targets": [0] },
                   { className: "text-right", "targets": [4, 5, 6] },
                    { width: "20%", "targets": [4] },
                    { width: "25%", "targets": [5] }

 



             ]
         });


    }
    catch (e) {
        notyAlert('error', e.message);
    }

    try {

        DataTables.TCRBillCommissionDetails = $('#tblTCRBillDetails').DataTable(
         {
             dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
             order: [],
             searching: true,
             paging: true,
             pageLength: 8,
             data: null,
             language: {
                 search: "_INPUT_",
                 searchPlaceholder: "Search"
             },
             columns: [
               { "data": "BillDate", "defaultContent": "<i>-</i>" },
               { "data": "BillNo", "defaultContent": "<i>-</i>" },
               { "data": "CustomerName", "defaultContent": "<i>-</i>" },
               { "data": "ProductCommission", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
               { "data": "ServiceChargeCommission", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
               { "data": "Total", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },


             ],
             columnDefs: [
                  { className: "text-left", "targets": [1,2] },
                  { className: "text-center", "targets": [0] },
                  { className: "text-right", "targets": [3,4,5] }
             ]
         });


    }
    catch (e) {
        notyAlert('error', e.message);
    }


    try {

        DataTables.AMCCommissionDetails = $('#tblAMCDetails').DataTable(
         {
             dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
             order: [],
             searching: true,
             paging: true,
             pageLength: 8,
             data: null,
             language: {
                 search: "_INPUT_",
                 searchPlaceholder: "Search"
             },
             columns: [
               { "data": "ICRDate", "defaultContent": "<i>-</i>" },
               { "data": "ICRNo", "defaultContent": "<i>-</i>" },
               { "data": "CustomerName", "defaultContent": "<i>-</i>" },
               { "data": "AMCCommission", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
             ],
             columnDefs: [
                  { className: "text-left", "targets": [1,2] },
                  { className: "text-center", "targets": [0] },
                  { className: "text-right", "targets": [3] }
             ]
         });
    }
    catch (e) {
        notyAlert('error', e.message);
    }


    try {

        DataTables.AdvanceDetails = $('#tblAdvanceDetails').DataTable(
         {
             dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
             order: [],
             searching: true,
             paging: true,
             pageLength: 8,
             data: null,
             language: {
                 search: "_INPUT_",
                 searchPlaceholder: "Search"
             },
             columns: [
               { "data": "RefDate", "defaultContent": "<i>-</i>" },
               { "data": "Note", "defaultContent": "<i>-</i>" },
               { "data": "Advance", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
               


             ],
             columnDefs: [
                  { className: "text-left", "targets": [1] },
                  { className: "text-center", "targets": [0, 2] }

             ]
         });


    }
    catch (e) {
        notyAlert('error', e.message);
    }


});

function ViewMore(curobj)
{
   
    try
    {
        var rowData = DataTables.SalaryTable.row($(curobj).parents('tr')).data();
        BindAllCommissionTables(rowData.SCCode, rowData.EmpID, rowData.Month, rowData.Year);
        $("#TechnicianLabel").text(rowData.Name);
        var monthNames = ["","Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
        $("#ServiceDateLabel").text(monthNames[rowData.Month] +'/'+ rowData.Year);
        $("#ModelSalaryDetails").modal('show');
        $("#jobcommis").trigger('click');
    }
    catch(e)
    {
        notyAlert('error', e.message);
    }
}


function RemoveZero(data) {

    if (data == 0) { return "";}
    else { return data;}
}













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
            $("#MsgTotalCommission").text(ds.Record.TotalCommission);
            $("#MsgTotalSalaryAdvance").text(ds.Record.TotalAdvanceSalary);
            $("#MsgTotalPayable").text(ds.Record.TotalPayable);
          
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
            $("#Msgtotalpayable").text(ds.Record);
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
        $(".well").show();
        if (($("#Month").val() != '') && (yea != ''))
        {
            // mon = (mon != '' && mon != '--Select Month--') ? mon : ' - ';
            // yea = yea != '' ? yea : ' - ';
            $("#MsgPeriod").text("" + mon + "/" + yea);
            RefreshSalaryTable();
        }
        else
        {
            $("#MsgPeriod").text("Please Select Month and Year");
        }
      
       
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}



function BindAllCommissionTables(SCCode, EmpID, Month, Year) {
    try {
        var emptyarr = [];
        var data = { "SCCode": SCCode, "EmpID": EmpID, "Month": Month, "Year": Year };
        var ds = {};
        ds = GetDataFromServer("TechnicianSalaryCalculation/GetAllBreakUpSalaryByTechnician/", data);
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        if (ds.Result == "OK") {
            if (ds.JobRecords)
            {
                DataTables.JobCommissionDetails.clear().rows.add(ds.JobRecords).draw(false);
                $("#lbljobsum").text(ds.JobRecord);
            }
            else
            {
               
                DataTables.JobCommissionDetails.clear().rows.add(emptyarr).draw(false);
                $("#lbljobsum").text('');
            }
            if (ds.TCRRecords)
            {
                DataTables.TCRBillCommissionDetails.clear().rows.add(ds.TCRRecords).draw(false);
                $("#lbltcrsum").text(ds.TCRRecord);
            }
            else
            {
               
                DataTables.TCRBillCommissionDetails.clear().rows.add(emptyarr).draw(false);
                $("#lbltcrsum").text('');
            }
            if (ds.AMCRecords) {
                DataTables.AMCCommissionDetails.clear().rows.add(ds.AMCRecords).draw(false);
                $("#lblamcsum").text(ds.AMCRecord);
             
            }
            else {

                DataTables.AMCCommissionDetails.clear().rows.add(emptyarr).draw(false);
                $("#lblamcsum").text('');
            }

            if (ds.SARecords) {
                DataTables.AdvanceDetails.clear().rows.add(ds.SARecords).draw(false);
                $("#lbladvancesum").text(ds.SARecord);

            }
            else {

                DataTables.AdvanceDetails.clear().rows.add(emptyarr).draw(false);
                $("#lbladvancesum").text('');
            }
            if (ds.TotalComm)
            {
                $("#lblTotalCommission").text(ds.TotalComm);
            }
            else
            {
                $("#lblTotalCommission").text('');
            }
            if (ds.SARecord) {
                $("#lblSalaryAdvance").text(ds.SARecord);
            }
            else {
                $("#lblSalaryAdvance").text('');
            }
            if (ds.NetPayable) {
                $("#lblNetPayable").text(ds.NetPayable);
            }
            else {
                $("#lblNetPayable").text('');
            }
           
        }
        if (ds.Result == "ERROR") {
            notyAlert('error', ds.Message);
           
        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}
