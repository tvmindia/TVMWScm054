var DataTables = {};
var appAddress = window.location.protocol + "//" + window.location.host + "/";   //Retrieving browser Url 
$(document).ready(function () {
    try
    {
        debugger;
        var Header = [];
        var Records=GetItemsSummary();
        $.each(Records, function (index, Records) {
            debugger;
            if (Header.length == 0) {
                debugger;
                $.each(Records, function (key, value) {
                    debugger;
                    Header.push(key);
                });
                $("#trTechPerform").empty();
                for (var i = 0; i < Header.length; i++) {
                    $("#trTechPerform").append('<th>' + Header[i] + '</th>')
                }
            }
            var html = "";
            $.each(Records, function (key, value) {
                if (Records.Day == "Sunday")
                {
                    html = html + "<td style='background-color:#cef0a9;'>" + ((value != null && value != 0) ? value : "-") + "</td>"
                }
                else
                {
                    html = html + "<td>" + ((value != null && value != 0) ? value : "-") + "</td>"
                }
               
            });
            $("#tbodyPerform").append('<tr>' + html + '</tr>')

        });


        $('#tblTechnicianPerformanceList').DataTable({
            buttons: [
           {
            extend: 'excel',
            text: 'Save current page',
            exportOptions: {
                modifier: {
                    page: 'current'
                }
            }
            }],
            dom: '<"pull-left"f>rt<"bottom"ip><"clear">',            
            order: [],
            searching: true,
            paging: false,
            language: {
                search: "_INPUT_",
                searchPlaceholder: "Search Items..."
            },
            columnDefs: [
            {
                targets: [1],
                visible: false,
                searchable: true
            },
            {
                targets: [1],
                visible: false
            }
            ]
        });
        //$(".buttons-print").hide();
        //$(".buttons-excel").hide();
    }
    catch(e)
    {

    }
});

function GetItemsSummary() {
    debugger;
    try {
        var month = $("#Month").val();
        var year = $("#Year").val();
        var EmpID = '5ece3004-aaca-4231-b34e-bc8bb6951e06';
        var data = {};
        if ((month) && (year)) {
            data = { "EmpID":EmpID,"month": month, "year": year };
        }
        var ds = {};
        ds = GetDataFromServer("Report/GetTechnicianPerformance/", data);
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
function PrintTableToDoc() {
    debugger;
    try {
        $(".buttons-excel").trigger('click');
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}
function goBack() {
    window.location = appAddress + "Report/Index/";
}