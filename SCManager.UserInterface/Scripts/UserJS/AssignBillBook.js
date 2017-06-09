var DataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";
var seriesTyping = 0;

//---------------------------------------Docuement Ready--------------------------------------------------//

$(document).ready(function () {
    try
    {
        DataTables.BillListTable = $('#tblBillList').DataTable(
     {
         dom: '<"pull-left"f>rt<"bottom"ip><"clear">',
         order: [],
         searching: true,
         paging: true,
         data: GetAllBillBookList(),
         columns: [
                { "data": "ID", "defaultContent": "<i>-</i>" },
              { "data": "BillNo", "defaultContent": "<i>-</i>" },
               { "data": "SeriesStart", "defaultContent": "<i>-</i>" },
                { "data": "SeriesEnd", "defaultContent": "<i>-</i>" },
               { "data": "LastUsed", "defaultContent": "<i>-</i>" },
               { "data": "Technician", "defaultContent": "<i>-</i>" },
                 { "data": "Status", "defaultContent": "<i>-</i>" },
                 { "data": "BillBookType", "defaultContent": "<i>-</i>" },
                   { "data": "Remarks", "defaultContent": "<i>-</i>" },
           { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink" onclick="Edit(this)"><i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>' }
         ],
         columnDefs: [{ "targets": [0], "visible": false, "searchable": false },            
             { className: "text-center", "targets": [1, 2, 3, 4, 5, 6, 7, 8,9] },
              {
                  "render": function (data, type, row) {
                      return (data == "True" ? "Open "  : "Closed");
                  },
                  "targets": 6

              },
            
         ]
     });
        $('#tblBillList tbody').on('dblclick', 'td', function () {

            Edit(this);
        });
    }
    catch (e) {
        notyAlert('error', e.message);
    }
});


//--------------------button actions ----------------------
function List() {
    try {
        $("#ID").val(EmptyGuid);
        ChangeButtonPatchView('AssignBillBook', 'btnPatchAssignBillBookSettab', 'List');
        BindAllBillBooks()

    } catch (x) {
        alert(x);
    }

}

//-----------------------------------------Reset Validation Messages--------------------------------------//
function ResetForm() {
    var validator = $("#AssignBillBook").validate();
    $('#AssignBillBook').find('.field-validation-error span').each(function () {
        validator.settings.success($(this));
    });
    validator.resetForm();
}

function SeriesChange()
{
    debugger;
    seriesTyping = 1;
}

function SerialStartandEndValidation()
{
    debugger;
    var seriesStart = $("#SeriesStart").val();
    var seriesEnd = $("#SeriesEnd").val();
    var LastUsed = $("#LastUsed").val();
    if(seriesEnd!="" && seriesEnd!="")
    {
        seriesStart = parseInt(seriesStart);
        seriesEnd = parseInt(seriesEnd);
        if(seriesStart>seriesEnd)
        {
            notyAlert('error', "Series End Should Be Greater Than Series Start");
            $("#SeriesEnd").css('border-color', 'red');
            return false;
        }
        else
        {
            $("#SeriesEnd").css('border-color', '');
            return true;
        }
    }
}


function SerialStartandEndRangeValidation(seriesStart,seriesEnd)
{
    debugger;
    try {
       
        if (seriesTyping == 1)
        {
            var BillNo = $("#BillNo").val();
            var BillBookType=$("#BillBookType").val();
            var data = { "SeriesStart": seriesStart, "SeriesEnd": seriesEnd,"BillNo":BillNo,"BillBookType":BillBookType };
            var ds = {};
            ds = GetDataFromServer("AssignBillBook/BillBookRangeValidation/", data);
            if (ds != '') {
                ds = JSON.parse(ds);
            }
            if (ds.Result == "OK") {
                if (ds.Message == "3") {
                    return true;
                }
                else {
                    notyAlert('error', ds.Message);
                    return false;
                }

            }
            if (ds.Result == "ERROR") {
                notyAlert('error', ds.Message);
                return false;
            }
        }
        else
        {
            return true;
        }
                   

    }
    catch (e) {
        notyAlert('error', e.message);
        return 0;
    }
}

function GetMissingSerials(seriesStart,seriesEnd,BillBookType)
{
    try {

        var data = {"SeriesStart": seriesStart,"SeriesEnd":seriesEnd,"BillBookType":BillBookType};
        var ds = {};
        ds = GetDataFromServer("AssignBillBook/GetMissingSerials/", data);

        if (ds != '') {
            ds = JSON.parse(ds);
        }
        if (ds.Result == "OK") {

            return ds.Records;
        }
        if (ds.Result == "ERROR") {
            alert(ds.Message);
        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}




//------------------------------- Bill Book Save-----------------------------//
function save() {
    debugger;
    try
    {
        var seriesStart = $("#SeriesStart").val();
        var seriesEnd = $("#SeriesEnd").val();
        var LastUsed = $("#LastUsed").val();
        if (seriesStart != "" && seriesEnd != "")
        {
            if (SerialStartandEndValidation() == true) {
                if (SerialStartandEndRangeValidation(seriesStart, LastUsed) == true)
                {
                    $("#btnSave").trigger('click');
                }
                
            }
        }
        else
        {
            $("#btnSave").trigger('click');
        }
        
        
    }
    catch (e)
    {
        notyAlert('error', e.message);
    }
   

}

function BindBillBookFields(Records)
{
    debugger;
    try {
        if (Records[0].Status == "True")
        {
            ChangeButtonPatchView('AssignBillBook', 'btnPatchAssignBillBookSettab', 'Edit');
        }
        else
        {
            ChangeButtonPatchView('AssignBillBook', 'btnPatchAssignBillBookSettab', 'Closed');
        }
        $("#ID").val(Records[0].ID);
    $("#BillNo").val(Records[0].BillNo);
    $("#SeriesStart").val(Records[0].SeriesStart);
    $("#SeriesEnd").val(Records[0].SeriesEnd);
    $("#LastUsed").val(Records[0].LastUsed);
    $("#EmpID").val(Records[0].EmpID);
    $("#Status").val(Records[0].Status);
    $("#BillBookType").val(Records[0].BillBookType);
    $("#Remarks").val(Records[0].Remarks);
    $(".masterinfoDet").html('');
    var result = GetMissingSerials(Records[0].SeriesStart, Records[0].LastUsed, Records[0].BillBookType);
    if (result != "" && result != undefined) {
        if (result.Table.length > 0) {
            $(".masterinfoDet").append('<div>Book Series ' + Records[0].SeriesStart + " - " + Records[0].SeriesEnd + '</div>');
            for (var i = 0; i < result.Table.length; i++) {
                $(".masterinfoDet").append('<label for="name" style="font-weight:normal;">' + result.Table[i].id + '</label>');
            }
        }
        else {
            $(".masterinfoDet").append('<div>Book Series ' + Records[0].SeriesStart + " - " + Records[0].SeriesEnd + '</div><br>');
            $(".masterinfoDet").append('<label for="name" style="font-weight:normal;">No Missing Numbers!</label>');
        }
    }
    
    } catch (e) {
        notyAlert('error', e.message);
    }
}

function fillBillBook(id)
{
    try {
        var data = { "ID": id };
        var ds = {};
        ds = GetDataFromServer("AssignBillBook/GeBillBookByID/", data);
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        if (ds.Result == "OK") {

            BindBillBookFields(ds.Records);
        }
        if (ds.Result == "ERROR") {
            notyAlert('error', ds.Message);
            return 0;
        }
        return 1;

    }
    catch (e) {
        notyAlert('error', e.message);
        return 0;
    }
}

function SaveSuccess(data, status) {
    debugger;
    var JsonResult = JSON.parse(data)
    switch (JsonResult.Result) {
        case "OK":
            if ($("#ID").val() == EmptyGuid) {
                if (JsonResult.Records.Status == "True")
                {
                    ChangeButtonPatchView('AssignBillBook', 'btnPatchAssignBillBookSettab', 'Edit');
                }
                else
                {
                    ChangeButtonPatchView('AssignBillBook', 'btnPatchAssignBillBookSettab', 'Closed');
                }
               
                fillBillBook(JsonResult.Records.ID);
            }
            else {
                ChangeButtonPatchView('AssignBillBook', 'btnPatchAssignBillBookSettab', 'Edit');
                fillBillBook($("#ID").val());
            }

            BindAllBillBooks();
            notyAlert('success', JsonResult.Records.Message);
            break;
        case "ERROR":
            notyAlert('error', "Error!");
            break;
        default:
            notyAlert('error', JsonResult.Message);
            break;
    }
}
function goBack() {
    $('#AddTab').trigger('click');
    Reset();
}
function DeleteClick() {
    notyConfirm('Are you sure to delete?', 'BillBookDelete()');
}
function BillBookDelete()
{
    debugger;
    try {
        var id = $('#ID').val();
        if (id != '' && id != null) {
            var data = { "ID": id };
            var ds = {};
            ds = GetDataFromServer("AssignBillBook/DeleteBillBook/", data);
            if (ds != '') {
                ds = JSON.parse(ds);
            }
            if (ds.Result == "OK") {
                notyAlert('success', ds.Message);
                $('#ListTab').trigger('click');
            }
            if (ds.Result == "ERROR") {
                notyAlert('error', ds.Message);
                return 0;
            }
            return 1;
        }

    }
    catch (e) {
        notyAlert('error', e.message);
        return 0;
    }
}
//---------------------------------------Edit Item--------------------------------------------------//
function Edit(currentObj) {
    debugger;
    var rowData = DataTables.BillListTable.row($(currentObj).parents('tr')).data();
    if ((rowData != null) && (rowData.ID != null)) {       
        $('#AddTab').trigger('click');
        if ((rowData != null) && (rowData.ID != null)) {
            $("#ID").val(rowData.ID);
            fillBillBook(rowData.ID);
            if (rowData.Status == "False") {
                ChangeButtonPatchView('AssignBillBook', 'btnPatchAssignBillBookSettab', 'Closed');
            }
            else {
                ChangeButtonPatchView('AssignBillBook', 'btnPatchAssignBillBookSettab', 'Edit');
            }
            debugger;
            $(".masterinfoDet").html('');
            var result = GetMissingSerials(rowData.SeriesStart, rowData.LastUsed, rowData.BillBookType);
            if(result!="" && result!=undefined)
            {
                if (result.Table.length > 0)
                {
                    $(".masterinfoDet").append('<div>Book Series ' + rowData.SeriesStart + " - " + rowData.SeriesEnd + '</div><br>');
                    for (var i = 0; i < result.Table.length; i++) {
                        if (i != result.Table.length - 1) {
                            $(".masterinfoDet").append('<label for="name" style="font-weight:normal;">' + result.Table[i].id + '</label>' + ',');
                        }
                        else {
                            $(".masterinfoDet").append('<label for="name" style="font-weight:normal;">' + result.Table[i].id + '</label>');
                        }

                    }
                }
                else {
                    $(".masterinfoDet").append('<div>Book Series ' + rowData.SeriesStart + " - " + rowData.SeriesEnd + '</div><br>');
                    $(".masterinfoDet").append('<label for="name" style="font-weight:normal;">No Missing Numbers!</label>');
                }
                
            }
            
        }       
        else {
            $('#ListTab').trigger('click');
        }
    }
}
function Add(id) {
    debugger;
    if (id != 1) {
        $('#AddTab').trigger('click');      
    }
    $("#ID").val(EmptyGuid);   
    ChangeButtonPatchView('AssignBillBook', 'btnPatchAssignBillBookSettab', 'Add');
    Reset();
    $("#Status").val("True");
}
function Reset()
{
    debugger;
    if (($("#ID").val() == EmptyGuid) || ($("#ID").val() == 'undefined') || ($("#ID").val() == "0"))
        {
    $("#BillNo").val("");
    $("#SeriesStart").val("");
    $("#SeriesEnd").val("");
    $("#LastUsed").val("");
    $("#EmpID").val("");
    $("#Status").val("");
    $("#BillBookType").val("");
    $("#Remarks").val("");
    $("#SeriesEnd").css('border-color', '');
    $(".masterinfoDet").html('');
    ResetForm();
    }
    else {
        ChangeButtonPatchView('AssignBillBook', 'btnPatchAssignBillBookSettab', 'Edit');
        fillBillBook($("#ID").val());
    }
}
//---------------------------------------Bind All Bill Book----------------------------------------------//
function BindAllBillBooks() {
    try {

        DataTables.BillListTable.clear().rows.add(GetAllBillBookList()).draw(false);

    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

function GetAllBillBookList()
{
    try {

        var data = { };
        var ds = {};
        ds = GetDataFromServer("AssignBillBook/GetAllBillBook/", data);

        if (ds != '') {
            ds = JSON.parse(ds);
        }
        if (ds.Result == "OK") {

            return ds.Records;
        }
        if (ds.Result == "ERROR") {
            alert(ds.Message);
        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}