var DataTables = {};
$(document).ready(function () {

    try {

        DataTables.TechnicianStockTable = $('#tblTechnicianList').DataTable(
         {
             dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
             buttons: [
                 //{
                 //extend: 'print',
                 //exportOptions:
                 //             {
                 //                 columns: [0,1, 2, 3],
                 //                 page: 'current',
                 //                 //format: {
                 //                 //    body: function (data, row, column, node) {
                 //                 //        debugger;
                 //                 //        // Strip $ from salary column to make it numeric
                 //                 //        return column === 0 ?
                 //                 //            data.Name="787" :
                 //                 //            data;
                 //                 //    }
                 //                 //}
                 //             }
                                
                              
                 //},
                 {
                   extend: 'excel',
                   exportOptions:
                                {
                                    columns: [0,1,2,3,4,5]
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
             data: GetAllTechnicianStock(),
             columns: [

              
           
               { "data": "Name",visible:false, "defaultContent": "<i>-</i>" },
               { "data": "ItemCode", "defaultContent": "<i>-</i>" },
               { "data": "Description", "defaultContent": "<i>-</i>" },
               { "data": "Stock", "defaultContent": "<i>-</i>" },
               { "data": "Rate", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
               { "data": "Value", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" }

             ],
             columnDefs: [
                  { className: "text-right",orderable:false, "targets": [4,5] },
                    { className: "text-center", orderable: false, "targets": [0, 3] },
                    { className: "text-left", orderable: false, "targets": [ 1, 2] },

             ],
             drawCallback: function (settings) {
                 var api = this.api();
                 var rows = api.rows({ page: 'current' }).nodes();
                 var last = null;

                 api.column(0, { page: 'current' }).data().each(function (group, i) {
                     if (last !== group) {
                         $(rows).eq(i).before(
                             '<tr class="group "><td colspan="10" class="rptGrp">' + '<b>Name</b> : ' + group + '</td></tr>'
                         );

                         last = group;
                     }
                 });
             }
         });
        //hide button of jquery datatable
        $(".buttons-print").hide();
        $(".buttons-excel").hide();

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

function PrintTableToDoc()
{
    try
    {
       
        $(".buttons-excel").trigger('click');
    }
    catch(e)
    {
        notyAlert('error', e.message);
    }
}