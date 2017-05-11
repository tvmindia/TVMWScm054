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
              { "data": "Amount", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
               { "data": "Description", "defaultContent": "<i>-</i>" },

           { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink" onclick="Edit(this)"><i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>' }
         ],
         columnDefs: [{ "targets": [0], "visible": false, "searchable": false },
             { className: "text-right", "targets": [4] },
             { className: "text-center", "targets": [1, 2, 3] },
              { className: "text-left", "targets": [5] },
         ]
     });

        $('#tblOtherIncomeList tbody').on('dblclick', 'td', function () {

            Edit(this);
        });
        FillDates();
        $('#fromDate').change(function () {
            FromDateOnChange();
        });
        $('#toDate').change(function () {
            FromDateOnChange();
        });
      
    }
    catch (e) {
        notyAlert('error', e.message);
    }
});

function FillDates() {
    debugger;
    var m_names = new Array("Jan", "Feb", "Mar",
 "Apr", "May", "Jun", "Jul", "Aug", "Sep",
 "Oct", "Nov", "Dec");

    var d = new Date();
    var curr_date = d.getDate();
    var curr_month = d.getMonth();
    var curr_year = d.getFullYear();
    var toDate = curr_date + "-" + m_names[curr_month]
    + "-" + curr_year;
    var $datepicker = $('#toDate');
    $datepicker.datepicker('setDate', new Date(toDate));
    var today = new Date()
    var pd = new Date();
    pd.setDate(pd.getDate() - 30);
    var priorDate = pd.toLocaleString()
    priorDate = priorDate.split(' ')[0];
    var p_month = parseInt(priorDate.split('/')[0]) - 1;
    var p_date = priorDate.split('/')[1];
    var p_year = priorDate.split('/')[2];
    var fromDate = p_date + "-" + m_names[p_month]
    + "-" + p_year;
    var $datepicker = $('#fromDate');
    $datepicker.datepicker('setDate', new Date(fromDate));
    FromDateOnChange();
}

function FromDateOnChange() {

    DataTables.OtherIncomeTable.clear().rows.add(GetOtherIncomeBetweenDates()).draw(false);
}

function DeleteSuccess(data, status) {
    var i = JSON.parse(data)
    debugger;

    switch (i.Result) {

        case "OK":
            BindAllOtherIncome();
            notyAlert('success', i.Message);
            clearfields();
            goBack();
            break;
        case "Error":
            notyAlert('error', "Error");
            break;
        case "ERROR":
            notyAlert('error', i.Message);
            break;
        default:
            break;
    }
}
function GetOtherIncomeBetweenDates() {
    try {
        debugger;
        var fromDate = $("#fromDate").val();
        var toDate = $("#toDate").val();
        if (toDate == "" && fromDate == "") {
            //DataTables.CreditNotesTable.clear().rows.add(GetAllCreditNotes(false)).draw(false);
        }
        else {
            var data = { "fromDate": fromDate, "toDate": toDate };
            var ds = {};
            ds = GetDataFromServer("OtherIncome/GetOtherIncomeBetweenDates/", data);
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

    }
    catch (e) {
        notyAlert('error', e.message);
    }
}
//------------------------------- CreditNotes Save-----------------------------//
function save() {
    debugger;
    $("#btnInsertUpdateOtherIncome").trigger('click');

}

//---------------------------------------Edit OtherIncome--------------------------------------------------//
function Edit(currentObj) {
    //Tab Change on edit click
    debugger;

    $('#AddTab').trigger('click');
    ChangeButtonPatchView("OtherIncome", "btnPatchOtherIncomeSettab", "Edit"); //ControllerName,id of the container div,Name of the action
    debugger;
    var rowData = DataTables.OtherIncomeTable.row($(currentObj).parents('tr')).data();
    if ((rowData != null) && (rowData.ID != null)) {
        fillOtherIncome(rowData.ID);

    }

}
//---------------------------------------Fill OtherIncome--------------------------------------------------//
function fillOtherIncome(ID) {
    debugger;
    ChangeButtonPatchView("OtherIncome", "btnPatchOtherIncomeSettab", "Edit");
    var thisItem = GetOtherIncomeByID(ID); //Binding Data
    //Hidden
    $("#deleteId").val(thisItem[0].ID);
    $("#ID").val(thisItem[0].ID);
    $("#IncomeTypeCode").val(thisItem[0].IncomeTypeCode);
    $("#RefNo").val(thisItem[0].RefNo);
    $("#HiddenRefNo").val(thisItem[0].RefNo);
    $("#Amount").val(roundoff(thisItem[0].Amount));
    $("#Description").val(thisItem[0].Description);
    $("#PaymentMode").val(thisItem[0].PaymentMode);
    $("#RefNo").prop('disabled', true);
    if (thisItem[0].RefDate != null) {
        var $datepicker = $('#RefDate');
        $datepicker.datepicker('setDate', new Date(thisItem[0].RefDate));
    }
}
//---------------------------------------Get OtherIncome Details By ID-------------------------------------//
function GetOtherIncomeByID(id) {
    try {
        var data = { "id": id };
        var ds = {};
        ds = GetDataFromServer("OtherIncome/GetOtherIncomeByID/", data);
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
            if ($("#ID").val() == EmptyGuid) {
                fillOtherIncome(JsonResult.Records.ID);
            }
            else {
                fillOtherIncome($("#ID").val());
            }
            FillDates();
            FromDateOnChange();
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
function showAllYNCheckedOrNot(i) {
    debugger;

    if (i.checked == true) {
        DataTables.OtherIncomeTable.clear().rows.add(GetAllOtherIncome(true)).draw(false);
        $('#fromDate').val("");
        $('#toDate').val("");
    }
    else {
        DataTables.OtherIncomeTable.clear().rows.add(GetAllOtherIncome(false)).draw(false);
        FillDates();
    }
    
}
//-----------------------------------------Reset Validation Messages--------------------------------------//
function ResetForm() {
    var validator = $("#formIns_Up").validate();
    $('#formIns_Up').find('.field-validation-error span').each(function () {
        validator.settings.success($(this));
    });
    validator.resetForm();
}
function clearfields() {
    $("#ID").val(EmptyGuid);
    $("#IncomeTypeCode").val("")
    $("#Amount").val("");
    $("#Description").val("")
    $("#RefNo").val("");
    var $datepicker = $('#RefDate');
    $datepicker.datepicker('setDate', null);
    $("#PaymentMode").val("");
    $("#deleteId").val("0")
    $("#RefNo").prop('disabled', false);
    var $datepicker = $('#RefDate');
    $datepicker.datepicker('setDate', null);
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

function DateClear() {
    $('#fromDate').val("");
    $('#toDate').val("");
}

//--------------------button actions ----------------------
function List() {
    try {

        ChangeButtonPatchView('OtherIncome', 'btnPatchOtherIncomeSettab', 'List');
        DateClear();
        FillDates()
        //BindAllOtherIncome()
    } catch (x) {
        alert(x);
    }

}

function Add(id) {
    debugger;
    if (id != 1) {
        $('#AddTab').trigger('click');
    }
    clearfields();
    ChangeButtonPatchView('OtherIncome', 'btnPatchOtherIncomeSettab', 'Add');
}
