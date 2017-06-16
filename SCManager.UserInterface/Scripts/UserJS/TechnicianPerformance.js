var DataTables = {};
var appAddress = window.location.protocol + "//" + window.location.host + "/";   //Retrieving browser Url 
$(document).ready(function () {
    try
    {
        $('SELECT').on('change', function () {
            Rebind();
        });
        $("#DynamictblContainer").append('<table id="tblTechnicianPerformanceList" class="table compact dataTable no-footer" cellspacing="0" width="100%" role="grid" aria-describedby="tblTechnicianPerformanceList_info" style="width: 100%;"><thead><tr class="text-center" id="trTechPerform" role="row"><th class="sorting" tabindex="0" aria-controls="tblTechnicianPerformanceList" rowspan="1" colspan="1" aria-label="Date: activate to sort column ascending" style="width: 48px;">Date</th><th class="sorting" tabindex="0" aria-controls="tblTechnicianPerformanceList" rowspan="1" colspan="1" aria-label="Minor: activate to sort column ascending" style="width: 32px;">Minor</th><th class="sorting" tabindex="0" aria-controls="tblTechnicianPerformanceList" rowspan="1" colspan="1" aria-label="Major: activate to sort column ascending" style="width: 32px;">Major</th><th class="sorting" tabindex="0" aria-controls="tblTechnicianPerformanceList" rowspan="1" colspan="1" aria-label="Mndty: activate to sort column ascending" style="width: 36px;">Mndty</th><th class="sorting" tabindex="0" aria-controls="tblTechnicianPerformanceList" rowspan="1" colspan="1" aria-label="Install: activate to sort column ascending" style="width: 35px;">Install</th><th class="sorting" tabindex="0" aria-controls="tblTechnicianPerformanceList" rowspan="1" colspan="1" aria-label="Repeat: activate to sort column ascending" style="width: 42px;">Repeat</th><th class="sorting" tabindex="0" aria-controls="tblTechnicianPerformanceList" rowspan="1" colspan="1" aria-label="AMC1: activate to sort column ascending" style="width: 36px;">AMC1</th><th class="sorting" tabindex="0" aria-controls="tblTechnicianPerformanceList" rowspan="1" colspan="1" aria-label="AMC2: activate to sort column ascending" style="width: 37px;">AMC2</th></tr></thead><tbody id="tbodyPerform"></tbody></table>');
       
    }
    catch(e)
    {

    }
});
function DrawTable()
{
    var Records = GetItemsSummary();
    if (Records != null)
    {
        $("#DynamictblContainer").empty();
        $("#DynamictblContainer").append('<table id="tblTechnicianPerformanceList" class="table compact" cellspacing="0" width="100%">'
                            +'<thead><tr class="text-center" id="trTechPerform"></tr></thead>'
                            +'<tbody id="tbodyPerform"></tbody>'
                        +'</table>')
        var Header = [];
        $.each(Records, function (index, Records) {
            debugger;
            if (Header.length == 0) {
                debugger;
                $.each(Records, function (key, value) {
                    debugger;
                    Header.push(key);
                });

                for (var i = 0; i < Header.length; i++) {
                    $("#trTechPerform").append('<th>' + Header[i] + '</th>')
                }
            }
            var html = "";
            $.each(Records, function (key, value) {
                if (Records.Day == "Sunday") {
                    html = html + "<td style='background-color:#cef0a9;'>" + ((value != null && value != 0) ? value : "-") + "</td>"
                }
                else if(Records.Day=="Total")
                {
                    html = html + "<td style='background-color:#ADD8E6;'><b style='font-size:18px;color:#73411d'>" + ((value != null && value != 0) ? value : "-") + "</b></td>"
                }
                else
                {
                    html = html + "<td>" + ((value != null && value != 0) ? value : "-") + "</td>"
                }

            });
            $("#tbodyPerform").append('<tr>' + html + '</tr>')

        });
    }
   
}
function FireDatatable()
{
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
function GetItemsSummary() {
    debugger;
    try {
        var month = $("#Month").val();
        var year = $("#Year").val();
        var EmpID = $("#EmpID").val();
        if ((EmpID) && (year) && (month))
        {
            var data = {};
            if ((month) && (year)) {
                data = { "EmpID": EmpID, "month": month, "year": year };
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
        else
        {
            notyAlert('error', 'Select a technician');
        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}
function Rebind()
{
    debugger;
    DrawTable();
    FireDatatable();
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