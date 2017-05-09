var DataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";

//---------------------------------------Docuement Ready--------------------------------------------------//

$(document).ready(function () {
    try
    {
        DataTables.OtherIncomeTable = $('#tblOtherIncomeList').DataTable(
     {
         dom: '<"pull-left"f>rt<"bottom"ip><"clear">',
         order: [],
         searching: true,
         paging: true,
         data: GetAllOtherIncome(false),
         columns: [
                { "data": "ID", "defaultContent": "<i>-</i>" },
              { "data": "IncomeTypeCode", "defaultContent": "<i>-</i>" },
               { "data": "RefDateFormatted", "defaultContent": "<i>-</i>" },
                { "data": "RefNo", "defaultContent": "<i>-</i>" },
              { "data": "Amount", "defaultContent": "<i>-</i>" },
               { "data": "Description", "defaultContent": "<i>-</i>" },

           { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink" onclick="Edit(this)"><i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>' }
         ],
         columnDefs: [{ "targets": [0], "visible": false, "searchable": false },
             { className: "text-right", "targets": [4] },
             { className: "text-center", "targets": [1, 2, 3,5] },
         ]
     });

        $('#tblOtherIncomeList tbody').on('dblclick', 'td', function () {

            Edit(this);
        });
        var $datepicker = $('#RefDate');
        $datepicker.datepicker('setDate', null);
    }
    catch (e) {
        notyAlert('error', e.message);
    }
});
//------------------------------- CreditNotes Save-----------------------------//
function save() {
    debugger;
    $("#btnInsertUpdateOtherIncome").trigger('click');

}
function Delete() {

    notyConfirm('Are you sure to delete?', 'DeleteOtherIncome()');

}
//---------------------------------------Delete-------------------------------------------------------//
function DeleteOtherIncome() {
    debugger;
    var id = $("#ID").val();
    if (id != EmptyGuid) {
        $("#btnFormDelete").click();
    }
    else {
        notyAlert('error', 'Error');
    }
}
function OtherIncomeSaveSuccess(data, status) {
    debugger;
    var JsonResult = JSON.parse(data)
    switch (JsonResult.Result) {
        case "OK":
            //if ($("#ID").val() == EmptyGuid) {
            //    fillCreditNotes(JsonResult.Records.ID);
            //}
            //else {
            //    fillCreditNotes($("#ID").val());
            //}
            BindAllOtherIncome();
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

//---------------------------------------Bind All OtherIncome----------------------------------------------//
function BindAllOtherIncome() {
    try {
        debugger;
        DataTables.OtherIncomeTable.clear().rows.add(GetAllOtherIncome(false)).draw(false);
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}
function clearfields() {
    $("#ID").val(EmptyGuid);
    $("#IncomeTypeCode").val("")
    $("#Amount").val("");
    $("#Description").val("")
    $("#RefNo").val("");
    var $datepicker = $('#RefDate');
    $datepicker.datepicker('setDate', null);
    $("#PaymentMode").prop('disabled', false);
    $("#deleteId").val("0")
    ResetForm();
}
function goBack() {
    $('#OtherIncomeTab').trigger('click');
    clearfields();
}
//---------------get grid fill result-------------------
function GetAllOtherIncome(showAllYN) {
    try {
        debugger;
        var data = { "showAllYN": showAllYN };
        var ds = {};
        ds = GetDataFromServer("OtherIncome/GetAllOtherIncome/", data);
        debugger;
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        if (ds.Result == "OK") {
            // debugger;
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

//--------------------button actions ----------------------
function List() {
    try {

        ChangeButtonPatchView('OtherIncome', 'btnPatchOtherIncomeSettab', 'List');

    } catch (x) {
        alert(x);
    }

}

function Add(id) {
    debugger;
    if (id != 1) {
        $('#AddTab').trigger('click');
    }
    //clearfields();
    ChangeButtonPatchView('OtherIncome', 'btnPatchOtherIncomeSettab', 'Add');
}
