var DataTables = {};
$(document).ready(function () {
    try {
        debugger;
        $("#prelaoder").show();
        DataTables.form8DetailTable = $('#form8DetailTable').DataTable({
            dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
            order: [],
            fixedHeader: {
                header: true
            },
            "scrollX": true,
            "scrollY": "150px",
            "scrollCollapse": true,
            searching: true,
            paging: true,
            data: null,
            pageLength: 3,
            columns: [
              {
                  "data": "ErrorRow",
                  render: function (data, type, row) {
                      return ((data !== null) || (data !== undefined)) ?
                               '<div style="color:Red">[' + data + ']</div>' : '<i>-</i>';
                  },
                  "defaultContent": "<i>-</i>"
              },
              {
                  "data": "Error",
                  render: function (data, type, row) {
                      return ((data !== null) || (data !== undefined)) ?
                          '<div style="color:Red">' + data + '</div>' : '<i>-</i>';
                  },
                  "defaultContent": "<i>-</i>"
              },
              { "data": "InvoiceNo", "defaultContent": "<i>-</i>" },//2
              { "data": "InvoiceDate", "defaultContent": "<i>-</i>" },
              { "data": "SalesOrderNo", "defaultContent": "<i>-</i>" },
              { "data": "ChallanNo", "defaultContent": "<i>-</i>" },
              { "data": "ChallanDate", "defaultContent": "<i>-</i>" },
              { "data": "PONo", "defaultContent": "<i>-</i>" },
              { "data": "PODate", "defaultContent": "<i>-</i>" },
              { "data": "Discount", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },//9
              { "data": "Remarks", "defaultContent": "<i>-</i>" },//10
              { "data": "Material", "defaultContent": "<i>-</i>" },//11
              { "data": "Quantity", "defaultContent": "<i>-</i>" },//12
              { "data": "Rate", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },//13
              { "data": "CGSTPercentage", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },//14
              { "data": "SGSTPercentage", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },//15
              { "data": "TradeDiscount", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" }//16
            ],
            columnDefs: [{ className: "text-left", "targets": [0, 1, 2, 4, 5, 7] },
                           { "width": "10%", "targets": [1] },
                           { className: "text-right", "targets": [9, 12, 13, 14, 15, 16] },
                          { className: "text-center", "targets": [0, 3, 6] }]
        });

        DataTables.form8ImportTable = $('#form8ImportTable').DataTable({
            dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
            order: [],
            fixedHeader: {
                header: true
            },
            
            "scrollX": true,
            "scrollY": "150px",
            "scrollCollapse": true,
            searching: true,
            paging: true,
            data: null,
            pageLength: 3,
            columns: [
              { "data": "InvoiceNo", "defaultContent": "<i>-</i>" },
              { "data": "InvoiceDate", "defaultContent": "<i>-</i>" },
              { "data": "SalesOrderNo", "defaultContent": "<i>-</i>" },
              { "data": "ChallanNo", "defaultContent": "<i>-</i>" },
              { "data": "ChallanDate", "defaultContent": "<i>-</i>" },
              { "data": "PONo", "defaultContent": "<i>-</i>" },
              { "data": "PODate", "defaultContent": "<i>-</i>" },//6
              { "data": "Discount", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },//7
              { "data": "Remarks", "defaultContent": "<i>-</i>" },//8
              { "data": "Material", "defaultContent": "<i>-</i>" },//9
              { "data": "Quantity", "defaultContent": "<i>-</i>" },//10
              { "data": "Rate", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },//11
              { "data": "CGSTPercentage", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },//12
              { "data": "SGSTPercentage", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },//13
              { "data": "TradeDiscount", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },//14
            ],
            columnDefs: [{ className: "text-left", "targets": [2, 3, 5, 8, 9, 0] },
                           { className: "text-right", "targets": [7, 10, 11, 12, 13, 14] },
                           { className: "text-center", "targets": [1, 4, 6] }]
        });

        DataTables.UploadedFilesHistoryTable = $('#tblHistoryList').DataTable({
            dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
            order: [],
            searching: true,
            paging: true,
            data: null,
            pageLength: 8,
            language: {
                search: "_INPUT_",
                searchPlaceholder: "Search"
            },
            columns: [
              { "data": "CreatedDate", "defaultContent": "<i>-</i>" },
              { "data": "FilePath", "defaultContent": "<i>-</i>" },
              { "data": "FileType", "defaultContent": "<i>-</i>" },
              { "data": "RecordCount", "defaultContent": "<i>-</i>" },
              {
                  "data": "FileStatus", render(data, type, row) {
                      if (data === "Successfully Imported") {
                          return '<span style="color:green">'+data+'</span>'
                      }
                      else
                      {
                          return '<span style="color:red">' + data + '</span>'
                      }
                  }
                  , "defaultContent": "<i>-</i>" }
            ],
            columnDefs: [
               { className: "text-center", "targets": [0] },
               { className: "text-right", "targets": [3] },
               { className: "text-left", "targets": [1, 2, 4] }]
        });
        Cancel()//Cancel does the all the reset options needed in the initial page loading
    }
    catch (ex) {
        notyAlert('error', ex.message);
        throw ex;
    }
});


function UploadFile(FileObject) {
    try {
        debugger;
        // Checking whether FormData is available in browser  
        if (window.FormData !== undefined) {
            debugger;
            var fileUpload = $("#FileUpload1").get(0);
            var files = fileUpload.files;
            if (files.length > 0) {
                // Create FormData object  
                var fileData = new FormData();

                // Looping over all files and add it to FormData object  
                for (var i = 0; i < files.length; i++) {
                    debugger;
                    fileData.append(files[i].name, files[i]);
                }
                $.ajax({
                    url: FileObject.Controller,
                    type: "POST",
                    contentType: false, // Not to set any content header  
                    processData: false, // Not to process data  
                    data: fileData,
                    success: function (result) {
                        if (result.Result === "OK") {
                            debugger;
                            if (FileObject.Controller !== "/Import/UploadFile") {
                                BindTableForm8(JSON.parse(result.ImportList));
                            }
                            $("#prelaoder").hide();
                            if (result.RemovedCount === 0) {
                                $("#btnUpload").prop('disabled', false);
                                $("#AHContainer").hide();
                                $("#ErrorBar").hide();
                                $("#noError").show();
                                BindImportForm8Table(JSON.parse(result.ImportList), result.TotalCount, result.TotalCount, result.RemovedCount, result.FileName);
                            }
                            else {
                                $("#ErrorData").show();
                                $("#dataErrorList").text('');
                                $("#dataErrorList").append('<b> Data Error ! </b>');
                                $('#dataError').show();
                            }
                        }
                        else {
                            $("#prelaoder").hide();
                            $("#ErrorBar").show();
                            $("#ErrorList").text('');
                            $("#ErrorList").append('<b>  ' + result.Message + '</b>');

                            if (result.Result === "WARNING") {
                                $("#fileError").show();
                            }
                            else if (result.Result === "EXCEPTION") {
                                $("#sysError").show();
                            }
                        }
                    },
                    error: function (err) {
                        $("#prelaoder").hide();
                        notyAlert('error', err.statusText);
                    }

                });
            }
            else {
                Cancel();
                $("#noFile").show();
            }

        } else {
            $("#prelaoder").hide();
            notyAlert('error', 'FormData is not supported.');
        }
    } catch (ex) {
        $("#prelaoder").hide();
        notyAlert('error', ex.message);
    }
}

function BindTableForm8(Form8List) {
    try {
        debugger;
        $("#prelaoder").show();
        $("#AHContainer").show();
        $("#SideBar").show();
        $("#finalRow").hide();
        DataTables.form8DetailTable.clear().rows.add(Form8List).draw(true);
        $("#prelaoder").hide();
    }
    catch (ex) {
        $("#prelaoder").hide();
        notyAlert("error", ex.message);
    }
}

function BindFileName() {
    try {
        debugger;
        var fileUpload = $("#FileUpload1").get(0);
        var files = fileUpload.files;

        // Looping over all files and add it to FormData object  
        $('#UploadPreview').empty();
        for (var i = 0; i < files.length; i++) {
            var html = '<br/><br/><div class="col-md-12 list" style="font-size:large;"><i class="fa fa-file-excel-o" aria-hidden="true"></i><span> ' + (files[i].name).substring(0, (files[i].name).lastIndexOf('.')) + '</span><a href="javascript:void(0)" fileguid="' + files[i].lastModified + '" onclick="Attachment_Remove(this)" style="float:right;"></a></div>'
            $('#UploadPreview').append(html);
        }
        if (files.length === 0) {
            $('#UploadPreview').append('');
        }
        $("#noFile").hide();
    }
    catch (ex) {
        notyAlert('error', ex.message);
    }
}

function BindImportForm8Table(ImportForm8List, Count, TotalCount, RemovedCount, files) {
    $("#AHContainer").hide();
    $("#TableContainer").show();
    $('#fileName').text('');
    $('#fileName').append('<span><b> ' + files + '</b></span>');
    $('#TotalRows').text('');
    $('#TotalRows').append('<span><b> ' + TotalCount + '</b></span>');
    $('#InsertedRowCount').text('');
    $('#InsertedRowCount').append('<span><b> ' + Count + '</b></span>');
    $('#RemovedRowCount').text('');
    $('#RemovedRowCount').append('<span><b> ' + RemovedCount + '</b></span>');
    DataTables.form8ImportTable.clear().rows.add(ImportForm8List).draw(true);
    $("#prelaoder").hide();
}

function FetchHistory() {
    try {
        debugger;
        $("#HistoryModel").modal('show');
        $('a[href="#HistoryList"]').click();
        var ds = {};
        ds = GetDataFromServer("Import/GetAllUploadedFile/");
        debugger;
        if (ds !== '') {
            ds = JSON.parse(ds);
        }
        if (ds.Result === "OK") {
            DataTables.UploadedFilesHistoryTable.clear().rows.add(ds.Records).draw(true);
        }
        if (ds.Result === "ERROR") {
            alert(ds.Message);
        }
    }
    catch (ex) {
        notyAlert('error', ex.message);
    }
}

function Validate() {
    try {
        debugger;
        $("#fileError").hide();
        $("#noError").hide();
        $('#dataError').hide();
        $("#sysError").hide();
        $("#AHContainer").hide();
        $("#SideBar").hide();
        $("#btnUpload").prop('disabled', true);
        $("#prelaoder").show();
        $("#ErrorBar").hide();
        $("#ErrorData").hide();
        $("#importRow").show();
        $("#uploadRow").hide();
        $("#finalRow").hide();
        $("#li2").addClass("active");
        $("#li1").removeClass("active");
        $("#li3").removeClass("active");
        var FileObject = new Object;
        FileObject.Controller = "/Import/ValidateUploadFile";
        UploadFile(FileObject);
    }
    catch (ex) {
        notyAlert('error', ex.message);
    }
}

function DownloadTemplate() {
    debugger;
    try {
        debugger;
        $.ajax({

            url: '/Import/DownloadTemplate',
            type: "GET",
            contentType: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',// content type for excel file of office 2007 or more
            success: function (returnValue) {
                debugger;
                window.location = '/Import/DownloadTemplate?file=' + returnValue.filename;

            },
            error: function (Error) {
                notyAlert('error', Error);
            }
        });

    } catch (ex) {
        notyAlert('error', ex.message);
    }
}

//----------On click of Cancel Button-----------//
function Cancel() {
    debugger;
    $("#noFile").hide();
    $("#FileUpload1").val('');
    $("#prelaoder").hide();
    $("#AHContainer").hide();
    $("#TableContainer").hide();
    $("#SideBar").hide();
    $("#btnUpload").prop('disabled', true);
    $('#UploadPreview').empty();
    //$("#uploadRow").hide(); //given for testing requirement (not necessarily needed)
    $("#uploadRow").show();
    $("#importRow").hide();
    $("#finalRow").hide();
    $("#ErrorBar").hide();
    $("#ErrorData").hide();
    $("#li1").addClass("active");
    $("#li2").removeClass("active");
    $("#li3").removeClass("active");
    $("fileError").hide();
    $("noError").hide();
    $('#dataError').hide();
    $("sysError").hide();
}

function SaveSuccess() {
    try {
        debugger;
        $("#prelaoder").show();
        $("#uploadRow").hide();
        $("#importRow").hide();
        $("#finalRow").show();
        var FileObject = new Object;
        FileObject.Controller = "/Import/UploadFile";
        UploadFile(FileObject);
        $("#li3").addClass("active");
        $("#li1").removeClass("active");
        $("#li2").removeClass("active");
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}
