var DataTables = {};
$(document).ready(function () {

    try {

        DataTables.TechnicianStockTable = $('#tblTechnicianList').DataTable(
         {
             dom: '<"pull-left"f>rt<"bottom"ip><"clear">',
             order: [],
             searching: false,
             paging: true,
             data: GetAllTechnicianStock(),
             columns: [

              
               { "data": null, "defaultContent": "<i>-</i>" },
               { "data": "Name", "defaultContent": "<i>-</i>" },
               { "data": "ItemCode", "defaultContent": "<i>-</i>" },
               { "data": "Description", "defaultContent": "<i>-</i>" },
               { "data": "Stock", "defaultContent": "<i>-</i>" },
               { "data": "Rate", "defaultContent": "<i>-</i>" },
               { "data": "Value", "defaultContent": "<i>-</i>" }

             ],
             columnDefs: [
                  { className: "text-right", "targets": [] },
                    { className: "text-center", "targets": [1,2,3,4,5,6] },
                    { className: "text-left", "targets": [0] },

             ],
             drawCallback: function (settings) {
                 var api = this.api();
                 var rows = api.rows({ page: 'current' }).nodes();
                 var last = null;

                 api.column(1, { page: 'current' }).data().each(function (group, i) {
                     if (last !== group) {
                         $(rows).eq(i).before(
                             '<tr class="group "><td colspan="10" class="rptGrp">' + '<b>Technician Name</b> : ' + group + '</td></tr>'
                         );

                         last = group;
                     }
                 });
             }
         });

        DataTables.TechnicianStockTable.on('order.dt search.dt', function () {
            DataTables.TechnicianStockTable.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
                cell.innerHTML = i + 1;
            });
        }).draw();

    }
    catch (e) {
        notyAlert('error', e.message);
    }

});


function GetAllTechnicianStock() {
    try {
        var frdate = $("#fromdate").val();
        var todate = $("#todate").val();
        var data = {};
        if ((frdate) && (todate)) {
            data = { "fromdate": frdate, "todate": todate };
        }

        var ds = {};
        ds = GetDataFromServer("Report/GetAllTechnicianStock/", data);
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

function RefreshTechnicianStockTable() {
    try {
        var fromdate = $("#fromdate").val();
        var todate = $("#todate").val();
        if (fromdate != "" && todate != "") {

            DataTables.TechnicianStockTable.clear().rows.add(GetAllTechnicianStock()).draw(false);
        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}