var DataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";
var ToDateVal, FromDateVal;
//---------------------------------------Docuement Ready--------------------------------------------------//

$(document).ready(function () {
    try {
        ChangeButtonPatchView('Expenses', 'btnPatchExpensesSettab', 'Add');
        DataTables.ExpensesTable = $('#tblExpensesList').DataTable(
       {
           dom: '<"pull-left"Bf>rt<"bottom"ip><"clear">',
           buttons: [{
               extend: 'excel',
               exportOptions:
                            {
                                columns: [1,2, 3, 4, 5,6,7,8]
                            }
           }],                  

           order: [],
           searching: true,
           paging: true,
           data: null,
           columns: [
                { "data": "ID", "defaultContent": "<i>-</i>" },
                { "data": "EntryNo", "defaultContent": "<i>-</i>" },
                { "data": "DateFormatted", "defaultContent": "<i>-</i>" },
                { "data": "RefNo", "defaultContent": "<i>-</i>" },
                { "data": "ExpenseType", "defaultContent": "<i>-</i>" },
                { "data": "PaymentMode", "defaultContent": "<i>-</i>" },
                { "data": "EmpName", "defaultContent": "<i>-</i>" },
                { "data": "Description", "defaultContent": "<i>-</i>" },
                { "data": "Amount", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
                { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink" onclick="Edit(this)"><i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>' }
                ],
           columnDefs: [{ "targets": [0], "visible": false, "searchable": false },
               { className: "text-right", "targets": [8] },
               { className: "text-center", "targets": [1,2,3,5,6,9] },
               { className: "text-left", "targets": [7] },
           ]
       });

        $('#tblExpensesList tbody').on('dblclick', 'td', function () {
            Edit(this);
        });
        FromDateVal = $("#fromDate").val();
        ToDateVal = $("#toDate").val();
        $('#fromDate').change(function () { 
            BindAllExpenses();
            $("#showAllYNCheckbox").prop('checked', false);
        });
        $('#toDate').change(function () { 
            BindAllExpenses();
            $("#showAllYNCheckbox").prop('checked', false);
        }); 
        clearfields();
        $(".buttons-excel").hide();
    }
    catch (e) {
        notyAlert('error', e.message);
    }
});

//--------------------button actions ----------------------
function List() {
    try {
        ChangeButtonPatchView('Expenses', 'btnPatchExpensesSettab', 'List');
        if ($("#showAllYNCheckbox").prop('checked'))
        {
            BindAllExpenses();
        }
      
      //  $("#showAllYNCheckbox").prop('checked', false);
     //   BindAllExpenses()

    } catch (x) {
        notyAlert('error',x.message);
    }
}
function ExpenseTypeChange() {
    if ($("#ExpenseTypeCode").val() == "IFBOT")
    {
        $("#OutStandingPaymentArea").show();
        var thisItem = GetOutStandingPayment();
        $("#OutStandingPayment").text(thisItem.OutStandingPaymentFormatted); 
    }
    else {
        $("#OutStandingPaymentArea").hide();
    }

    if ($("#ExpenseTypeCode").val() == "SADV" || $("#ExpenseTypeCode").val() == "SAL")
    {  
        $("#EmpID").prop('disabled', false);       
    }
    else{
        $("#EmpID").prop('disabled', true);
        $("#EmpID").val("");
    }

    GetTechnicianSalaryOnChange();
}

function GetOutStandingPayment() {
    try {
        var data = {};
        var ds = {};
        ds = GetDataFromServer("Expenses/GetOutStandingPayment/", data);
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        if (ds.Result == "OK") {
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

function clearfields() {
    $("#UpdateID").val(EmptyGuid);
    $("#ID").val(EmptyGuid);
    $("#EntryNo").prop('disabled', true);
    $("#EmpID").val("");
    $("#EmpID").prop('disabled', true);
    $("#EntryNo").val("<<Auto Generated>>");
    $("#PaymentMode").val("");
    $("#ExpenseTypeCode").val("");
    $("#ExpenseTypeCode").prop('disabled', false);
    $("#Amount").val("");
    $("#Description").val("");
    $("#RefNo").val("");
    var $datepicker = $('#Date');
    $datepicker.datepicker('setDate', null); 
    $("#deleteId").val("0");
    $("#hdfExpenseTypeCode").val("0");
    $("#OutStandingPaymentArea").hide();
    $("#SalaryCalculationArea").hide();
    ResetForm();
}
//---------------------------------------Edit Expenses--------------------------------------------------//
function Edit(currentObj) {
    debugger;
    //Tab Change on edit click
    $('#AddTab').trigger('click');
    ChangeButtonPatchView("CreditNotes", "btnPatchCreditNotesSettab", "Edit"); //ControllerName,id of the container div,Name of the action
    var rowData = DataTables.ExpensesTable.row($(currentObj).parents('tr')).data();
    if ((rowData != null) && (rowData.ID != null)) {
        fillExpenses(rowData.ID);
    }
}
//---------------------------------------Delete-------------------------------------------------------//
function Delete() {
    notyConfirm('Are you sure to delete?', 'DeleteExpenses()', '', "Yes, delete it!");
}

function DeleteExpenses() {
    var id = $("#UpdateID").val();
    if (id != EmptyGuid) {
        $("#btnFormDelete").click();
    }
    else {
        notyAlert('error', 'Error');
    }
}
//---------------------------------------Get Expenses Details By ID-------------------------------------//
function GetExpensesByID(id) {
    try { 
        var data = { "id": id };
        var ds = {};
        ds = GetDataFromServer("Expenses/GetExpensesByID/", data);
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        if (ds.Result == "OK") {
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

//---------------------------------------Fill Expenses--------------------------------------------------//
function fillExpenses(ID) {
    ChangeButtonPatchView("Expenses", "btnPatchExpensesSettab", "Edit");
    var thisItem = GetExpensesByID(ID); //Binding Data
    //Hidden
    $("#UpdateID").val(thisItem.ID);
    $("#deleteId").val(thisItem.ID);
    $("#ID").val(thisItem.ID);
    $("#EntryNo").val(thisItem.EntryNo);
    $("#RefNo").val(thisItem.RefNo);
    $("#hdfExpenseTypeCode").val(thisItem.ExpenseTypeCode);
    $("#ExpenseTypeCode").val(thisItem.ExpenseTypeCode);
    $("#ExpenseTypeCode").prop('disabled', true);
    $("#EmpID").val(thisItem.EmpID);
    $("#PaymentMode").val(thisItem.PaymentMode);
    $("#Amount").val(roundoff(thisItem.Amount));
    $("#Description").val(thisItem.Description);
  
    if (thisItem.RefDate != null) {
        var $datepicker = $('#Date');
        $datepicker.datepicker('setDate', new Date(thisItem.RefDate));
    }
    ExpenseTypeChange();
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
    if (id != 1) {
        $('#AddTab').trigger('click');
    }
    clearfields();
    ChangeButtonPatchView('Expenses', 'btnPatchExpensesSettab', 'Add');
}

function showAllYNCheckedOrNot(i) {
    if (i.checked == true) { 
        $('#fromDate').val("");
        $('#toDate').val("");
        DataTables.ExpensesTable.clear().rows.add(GetAllExpenses(true)).draw(false);
    }
    else {
        $('#fromDate').val(FromDateVal);
        $('#toDate').val(ToDateVal);
        DataTables.ExpensesTable.clear().rows.add(GetAllExpenses(false)).draw(false);      
    }

}



//---------------get grid fill result-------------------
function GetAllExpenses(showAllYN) {
    try {
        var FromDate = $("#fromDate").val();
        var ToDate = $("#toDate").val();

        var data = {"FromDate": FromDate, "ToDate": ToDate};
        var ds = {};
        ds = GetDataFromServer("Expenses/GetAllExpenses/", data);
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        if (ds.Result == "OK") {
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

//------------------------------- Expenses Save-----------------------------//
function save() {
    debugger;
    if ($("#Amount").val() != "") {
        if ($("#Amount").val() > 0) {
            $("#btnInsertUpdateExpenses").trigger('click');
        }
        else {
            if ($("#ExpenseTypeCode").val() == "SADV") {
                $("#btnInsertUpdateExpenses").trigger('click');
            }
            else {
                notyAlert('error', "Amount Cannot be Negative");
            }
        }
    }
    else
    {
        $("#btnInsertUpdateExpenses").trigger('click');
    }
}

//---------------------------------------Bind All Expenses----------------------------------------------//
function BindAllExpenses() {
    try {
        DataTables.ExpensesTable.clear().rows.add(GetAllExpenses(false)).draw(false);
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

function SaveSuccess(data, status) {
    var JsonResult = JSON.parse(data)
    switch (JsonResult.Result) {
        case "OK":
            if ($("#UpdateID").val() == EmptyGuid) {
                $("#UpdateID").val(JsonResult.Records.ID);
                fillExpenses(JsonResult.Records.ID);
            }
            else {
                fillExpenses($("#UpdateID").val());
            }
            BindAllExpenses();
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
    switch (i.Result) {
        case "OK":
            BindAllExpenses();
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

function GetTechnicianSalaryOnChange()
{
    try
    {
        $("#SalaryCalculationArea").hide(); 
        var expensetype = $("#ExpenseTypeCode").val();
        var empid = $("#EmpID").val();
        var dat = $("#Date").val();
        if ((expensetype == 'SAL') && (empid != ''))
        {
            var techsal = GetTechnicanSalary(empid, dat);
            if(techsal)
            {
                $("#lblmonthyear").text(techsal.Period);
                $("#lbltotalcommission").text(techsal.TotalCommissionRupee);
                $("#lbladvance").text(techsal.AdvanceRupee);
                $("#lblpayable").text(techsal.PayableRupee);
                $("#SalaryCalculationArea").show();
            }
        }
        
    }
    catch(e)
    {
        notyAlert('error', e.message);
    }
}

function GetTechnicanSalary(TechID,dat)
{
    try {
        var data = { "ID": TechID, "Date": dat };
        var ds = {};
        ds = GetDataFromServer("Expenses/GetTechnicianSalaryByTechnician/", data);
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        if (ds.Result == "OK") {
            return ds.Record;
        }
        if (ds.Result == "ERROR") {
            notyAlert('error', ds.Message);
          
        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

function ExportData() {
    try {

        $(".buttons-excel").trigger('click');
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}