var DataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";
//---------------------------------------Docuement Ready--------------------------------------------------//
$(document).ready(function () {
    try {

        DataTables.employeeTable = $('#tblEmployeesList').DataTable(
         {
             dom: '<"pull-left"f>rt<"bottom"ip><"clear">',
             order: [],
             searching: true,
             paging: true,
             data: GetAllEmployees(),
             columns: [
               { "data": "ID", "defaultContent": "<i>-</i>" },
               { "data": "Name", "defaultContent": "<i>-</i>" },
               { "data": "Type", "defaultContent": "<i>-</i>" },
               { "data": "MobileNo", "defaultContent": "<i>-</i>" },
               { "data": "Address", "defaultContent": "<i>-</i>" },
               { "data": "Remarks", "defaultContent": "<i>-</i>" },
                 { "data": "IsActive", "defaultContent": "<i>-</i>" },
               { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink" onclick="Edit(this)"><i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>' }
             ],
             columnDefs: [{ "targets": [0], "visible": false, "searchable": false },
                 { className: "text-center", "targets": [1, 2, 3, 4, 6,7] },
                 { className: "text-left", "targets": [5] },
                {
                    "render": function (data, type, row) {
                        return (data == false ? "No " : "Yes");
                    },
                    "targets": [6]

                },
             ]
         });

        $('#tblEmployeesList tbody').on('dblclick', 'td', function () {

            Edit(this);
        });
    }
    catch (e) {
        notyAlert('error', e.message);
    }
});
//---------------------------------------Edit Employee--------------------------------------------------//
function Edit(currentObj) {
    //Tab Change on edit click
    debugger;

    $('#AddTab').trigger('click');
    ChangeButtonPatchView("Employees", "btnPatchEmployeesSettab", "Edit"); //ControllerName,id of the container div,Name of the action
    var rowData = DataTables.employeeTable.row($(currentObj).parents('tr')).data();
    if ((rowData != null) && (rowData.ID != null)) {
        fillEmployee(rowData.ID);
    }
}
//------------------------------- Employee Save-----------------------------//
function save() {
   
    debugger;
    $("#btnInsertUpdateEmployee").trigger('click');
}
//---------------get grid fill result-------------------
function GetAllEmployees() {
    try {

        var data = {};
        var ds = {};
        ds = GetDataFromServer("Employees/GetAllEmployees/", data);
      
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
//--------------------button actions ----------------------
function List() {
    try {

        ChangeButtonPatchView('Employees', 'btnPatchEmployeesSettab', 'List');

    } catch (x) {
        alert(x);
    }

}

//---------------------------------------Bind All Employees----------------------------------------------//
function BindAllEmployees() {
    try {
       
        DataTables.employeeTable.clear().rows.add(GetAllEmployees()).draw(false);
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

function EmployeeSaveSuccess(data, status) {
  
    var JsonResult = JSON.parse(data)
    switch (JsonResult.Result) {
        case "OK":
            if ($("#ID").val() == EmptyGuid) {
                fillEmployee(JsonResult.Records.employeeID);
            }
            else {
                fillEmployee($("#ID").val());
            }
            BindAllEmployees();
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

//---------------------------------------Fill Employee--------------------------------------------------//
function fillEmployee(ID) {
    debugger;
    ChangeButtonPatchView("Employees", "btnPatchEmployeesSettab", "Edit");
    var thisItem = GetEmployeeDetailsByID(ID); //Binding Data
    //Hidden
    $("#ID").val(thisItem[0].ID);
    $("#Name").val(thisItem[0].Name);
    $("#Type").val(thisItem[0].Type)
    $("#MobileNo").val(thisItem[0].MobileNo)
    $("#Address").val(thisItem[0].Address)
    $("#Remarks").val(thisItem[0].Remarks)
    $("#deleteId").val(thisItem[0].ID);
    if (thisItem[0].IsActive == true) {
        $("#IsActive").prop('checked', true);
       // $('#IsActive').val(this.checked ? 1 : 0);

    }
    else {
        $("#IsActive").prop('checked', false);

    }
}

//---------------------------------------Get Employee Details By ID-------------------------------------//
function GetEmployeeDetailsByID(id) {
    try {
        var data = { "id": id };
        var ds = {};
        ds = GetDataFromServer("Employees/GetEmployeeByID/", data);
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
function Add(id) {
   
    if (id != 1) {
        $('#AddTab').trigger('click');
    }
    clearfields();
    ChangeButtonPatchView('Employees', 'btnPatchEmployeesSettab', 'Add');
}

//---------------------------------------Clear Fields-----------------------------------------------------//
function clearfields() {
    $("#ID").val(EmptyGuid);
    $("#Name").val("")
    $("#Type").val("")
    $("#MobileNo").val("")
    $("#Address").val("")
    $("#Remarks").val("");
    $("#deleteId").val("0")
    $("#IsActive").prop('checked', false);
    ResetForm();
}
function reset() {
    if ($("#ID").val() == EmptyGuid) {
        clearfields();
    }
    else {
        fillEmployee($("#ID").val())
    }

}

function goBack() {
    $('#EmployeesTab').trigger('click');
    clearfields();
}
//-----------------------------------------Reset Validation Messages--------------------------------------//
function ResetForm() {
    var validator = $("#formIns_Up").validate();
    $('#formIns_Up').find('.field-validation-error span').each(function () {
        validator.settings.success($(this));
    });
    validator.resetForm();
}
function Delete() {

    notyConfirm('Are you sure to delete?', 'DeleteEmployee()', '', "Yes, delete it!");

}
//---------------------------------------Delete-------------------------------------------------------//
function DeleteEmployee() {
    
    var id = $("#ID").val();
    if (id != EmptyGuid) {
        $("#btnFormDelete").click();
    }
    else {
        notyAlert('error', 'Please Select Employee');
    }
}

function DeleteSuccess(data, status) {
    var i = JSON.parse(data)
   

    switch (i.Result) {

        case "OK":
            BindAllEmployees();
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
