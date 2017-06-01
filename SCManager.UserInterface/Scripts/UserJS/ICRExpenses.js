var DataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";
var Bindflag = false;//for avoiding 3 times binding on pageload. 

//---------------------------------------Docuement Ready--------------------------------------------------//

$(document).ready(function () {
    try {
        debugger;
        ChangeButtonPatchView('ICRExpenses', 'btnPatchICRExpensesSettab', 'Add');
        DataTables.ICRExpensesTable = $('#tblICRExpensesList').DataTable(
       {
           dom: '<"pull-left"f>rt<"bottom"ip><"clear">',
           order: [],
           searching: true,
           paging: true,
           data: null,
           columns: [
                { "data": "ID", "defaultContent": "<i>-</i>" },
                { "data": "EntryNo", "defaultContent": "<i>-</i>" },
                { "data": "DateFormatted", "defaultContent": "<i>-</i>" },
                { "data": "RefNo", "defaultContent": "<i>-</i>" },
                { "data": "PaymentMode", "defaultContent": "<i>-</i>" }, 
                { "data": "Description", "defaultContent": "<i>-</i>" },
                { "data": "Amount", "defaultContent": "<i>-</i>" },
                { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink" onclick="Edit(this)"><i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>' }
           ],
           columnDefs: [{ "targets": [0], "visible": false, "searchable": false },
               { className: "text-right", "targets": [6] },
               { className: "text-center", "targets": [1, 2, 3, 4,7] },
               { className: "text-left", "targets": [5] },
           ]
       });

        $('#tblICRExpensesList tbody').on('dblclick', 'td', function () {
            Edit(this);
        });
        $('#fromDate').change(function () {
            if (Bindflag)
            BindAllICRExpenses();
        });
        $('#toDate').change(function () {
            if (Bindflag)
            BindAllICRExpenses();
        });

       
        debugger;
        var thisItem = GetOutStandingICRPayment();
        $("#OutStandingICRPayment").text(thisItem.OutStandingPaymentFormatted);
        clearfields();

    }
    catch (e) {
        notyAlert('error', e.message);
    }
});


function GetOutStandingICRPayment() {
    try {
        debugger;
        var data = {};
        var ds = {};
        ds = GetDataFromServer("ICRExpenses/GetOutStandingICRPayment/", data);
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

function FillDates() {  
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

    Bindflag = true;
}

//--------------------button actions ----------------------
function List() {
    try {
        ChangeButtonPatchView('ICRExpenses', 'btnPatchICRExpensesSettab', 'List');
        DateClear();
        Bindflag = false;//for avoiding 3 times binding on pageload. 
        FillDates()
        BindAllICRExpenses()

    } catch (x) {
        alert(x);
    }

}
 

 

function DateClear() {
    $('#fromDate').val("");
    $('#toDate').val("");
    $("#showAllYNCheckbox").prop('checked', false);
}

function clearfields() {
    $("#UpdateID").val(EmptyGuid);
    $("#ID").val(EmptyGuid);
    $("#EntryNo").prop('disabled', true);
    $("#EmpID").val("");
    $("#EmpID").prop('disabled', true);
    $("#EntryNo").val("<<Auto Generated>>");
    $("#PaymentMode").val("");
   
    $("#Amount").val("");
    $("#Description").val("");
    $("#RefNo").val("");
    var $datepicker = $('#Date');
    $datepicker.datepicker('setDate', null);
    $("#deleteId").val("0");
   
    ResetForm();
}
//---------------------------------------Edit ICRExpenses--------------------------------------------------//
function Edit(currentObj) {
    //Tab Change on edit click
    debugger;
    $('#AddTab').trigger('click');
    ChangeButtonPatchView("ICRExpenses", "btnPatchICRExpensesSettab", "Edit"); //ControllerName,id of the container div,Name of the action
    debugger;
    var rowData = DataTables.ICRExpensesTable.row($(currentObj).parents('tr')).data();
    if ((rowData != null) && (rowData.ID != null)) {
        fillICRExpenses(rowData.ID);
    }

}
//---------------------------------------Delete-------------------------------------------------------//
function Delete() {
    notyConfirm('Are you sure to delete?', 'DeleteICRExpenses()', '', "Yes, delete it!");
}

function DeleteICRExpenses() {
    debugger;
    var id = $("#UpdateID").val();
    if (id != EmptyGuid) {
        $("#btnFormDelete").click();
    }
    else {
        notyAlert('error', 'Error');
    }
}
//---------------------------------------Get ICRExpenses Details By ID-------------------------------------//
function GetICRExpensesByID(id) {
    debugger;
    try {
        var data = { "id": id };
        var ds = {};
        ds = GetDataFromServer("ICRExpenses/GetICRExpensesByID/", data);
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

//---------------------------------------Fill ICRExpenses--------------------------------------------------//
function fillICRExpenses(ID) {
    debugger;
    ChangeButtonPatchView("ICRExpenses", "btnPatchICRExpensesSettab", "Edit");
    var thisItem = GetICRExpensesByID(ID); //Binding Data
    //Hidden
    $("#UpdateID").val(thisItem.ID);
    $("#deleteId").val(thisItem.ID);
    $("#ID").val(thisItem.ID);
    $("#EntryNo").val(thisItem.EntryNo);
    $("#RefNo").val(thisItem.RefNo); 
    $("#PaymentMode").val(thisItem.PaymentMode);
    $("#Amount").val(thisItem.Amount);
    $("#Description").val(thisItem.Description);

    if (thisItem.RefDate != null) {
        var $datepicker = $('#Date');
        $datepicker.datepicker('setDate', new Date(thisItem.RefDate));
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

function Add(id) {
    debugger;
    if (id != 1) {
        $('#AddTab').trigger('click');
    }
    clearfields();
    ChangeButtonPatchView('ICRExpenses', 'btnPatchICRExpensesSettab', 'Add');
}

function showAllYNCheckedOrNot(i) {
    debugger;

    if (i.checked == true) {
        DataTables.ICRExpensesTable.clear().rows.add(GetAllICRExpenses(true)).draw(false);
        $('#fromDate').val("");
        $('#toDate').val("");
    }
    else {
        Bindflag = false;//for avoiding 3 times binding on pageload. 
        FillDates();
        DataTables.ICRExpensesTable.clear().rows.add(GetAllICRExpenses(false)).draw(false);
    }

}



//---------------get grid fill result-------------------
function GetAllICRExpenses(showAllYN) {
    try {
        debugger;
        var FromDate = $("#fromDate").val();
        var ToDate = $("#toDate").val();

        var data = { "showAllYN": showAllYN, "FromDate": FromDate, "ToDate": ToDate };
        var ds = {};
        ds = GetDataFromServer("ICRExpenses/GetAllICRExpenses/", data);
        debugger;
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

//------------------------------- ICRExpenses Save-----------------------------//
function save() {
    $("#btnInsertUpdateICRExpenses").trigger('click');
}

//---------------------------------------Bind All ICRExpenses----------------------------------------------//
function BindAllICRExpenses() {
    try {
        debugger;
        DataTables.ICRExpensesTable.clear().rows.add(GetAllICRExpenses(false)).draw(false);

    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

function SaveSuccess(data, status) {
    debugger;
    var JsonResult = JSON.parse(data)
    switch (JsonResult.Result) {
        case "OK":
            if ($("#UpdateID").val() == EmptyGuid) {
                $("#UpdateID").val(JsonResult.Records.ID);
                fillICRExpenses(JsonResult.Records.ID);
            }
            else {
                fillICRExpenses($("#UpdateID").val());
            }
            Bindflag = false;//for avoiding 3 times binding on pageload. 
            FillDates();
            BindAllICRExpenses();
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

function DeleteSuccess(data, status) {
    var i = JSON.parse(data)
    debugger;
    switch (i.Result) {
        case "OK":
            BindAllICRExpenses();
            $('#ListTab').trigger('click');
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