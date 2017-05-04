var DataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";
//---------------------------------------Docuement Ready--------------------------------------------------//
$(document).ready(function () {
    try {
        DataTables.DefectiveDamagedTable = $('#tblDefectiveorDamagedList').DataTable(
         {
             dom: '<"pull-left"f>rt<"bottom"ip><"clear">',
             order: [],
             searching: true,
             paging: true,
             data: GetAllDefectiveDamaged(),
             columns: [
                    { "data": "ID", "defaultContent": "<i>-</i>" },
                  { "data": "Type", "defaultContent": "<i>-</i>" },
                  { "data": "OpenDate", "defaultContent": "<i>-</i>" },
                   { "data": "RefNo", "defaultContent": "<i>-</i>" },
                    { "data": "ItemCode", "defaultContent": "<i>-</i>" },        
                      
               { "data": "Description", "defaultContent": "<i>-</i>" },
                 { "data": "Qty", "defaultContent": "<i>-</i>" },
                 { "data": "ReturnStatusYN", "defaultContent": "<i>-</i>" },
                 { "data": "Remarks", "defaultContent": "<i>-</i>" },
               { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink" onclick="Edit(this)"><i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>' }
             ],
             columnDefs: [{ "targets": [0], "visible": false, "searchable": false },
                    {
                        "render": function (data, type, row) {
                            return (data == true ? "Returned" + '<i class="fa fa-check" style="color:green;" aria-hidden="true"></i>' : "Not Returned");
                        },
                        "targets": 7
                    }
             ]
         });

        $('#tblDefectiveorDamagedList tbody').on('dblclick', 'td', function () {

            Edit(this);
        });
    }
    catch (e) {
        notyAlert('error', e.message);
    }
   
});
function DefectiveDamagedSaveSuccess(data, status) {
    debugger;
    var JsonResult = JSON.parse(data)
    switch (JsonResult.Result) {
        case "OK":
            if ($("#ID").val() == EmptyGuid) {
                fillDefectiveDamaged(JsonResult.Records.ID);
            }
            else {
                fillDefectiveDamaged($("#ID").val());
            }
            BindAllDefectiveDamaged();
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
//-----------------------------------------Reset Validation Messages--------------------------------------//
function ResetForm() {
    var validator = $("#formIns_Up").validate();
    $('#formIns_Up').find('.field-validation-error span').each(function () {
        validator.settings.success($(this));
    });
    validator.resetForm();
}
//---------------------------------------Clear Fields-----------------------------------------------------//
function clearfields() {
    $("#ID").val(EmptyGuid);
    $("#EmpID").val("")
    $("#Type").val("")
    $("#RefNo").val("")
    $("#ItemID").val("")
    $("#Description").val("");
    $("#Qty").val("")
    $("#Remarks").val("")
    var $datepicker = $('#OpenDate');
    $datepicker.datepicker('setDate', null);
    ResetForm();
}
//---------------------------------------Fill DefectiveDamaged--------------------------------------------------//
function fillDefectiveDamaged(ID) {
    debugger;
    ChangeButtonPatchView("DefectiveorDamaged", "btnPatchDefectiveorDamagedSettab", "Edit");
    var thisItem = GetDefectiveDamagedByID(ID); //Binding Data
    //Hidden
    $("#ID").val(thisItem[0].ID);
    $("#Type").val(thisItem[0].Type);
    $("#EmpID").val(thisItem[0].EmpID);
    if (thisItem[0].OpenDate != null) {
        var $datepicker = $('#OpenDate');
        $datepicker.datepicker('setDate', new Date(thisItem[0].OpenDate));
    }
    $("#RefNo").val(thisItem[0].RefNo)
    $("#ItemID").val(thisItem[0].ItemID)
    $("#Description").val(thisItem[0].Description)
    $("#Qty").val(thisItem[0].Qty)
    $("#Remarks").val(thisItem[0].Remarks)
   // $("#deleteId").val(thisItem[0].ID);
}
//---------------------------------------Get Employee Details By ID-------------------------------------//
function GetDefectiveDamagedByID(id) {
    try {
        var data = { "id": id };
        var ds = {};
        ds = GetDataFromServer("DefectiveorDamaged/GetDefectiveDamagedByID/", data);
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
//---------------------------------------Edit DefectiveDamaged--------------------------------------------------//
function Edit(currentObj) {
    //Tab Change on edit click
    debugger;

    $('#AddTab').trigger('click');
    ChangeButtonPatchView("DefectiveorDamaged", "btnPatchDefectiveorDamagedSettab", "Edit"); //ControllerName,id of the container div,Name of the action
    var rowData = DataTables.DefectiveDamagedTable.row($(currentObj).parents('tr')).data();
    if ((rowData != null) && (rowData.ID != null)) {
        fillDefectiveDamaged(rowData.ID);
    }
}
//---------------------------------------Bind All DefectiveDamaged----------------------------------------------//
function BindAllDefectiveDamaged() {
    try {
        debugger;
        DataTables.DefectiveDamagedTable.clear().rows.add(GetAllDefectiveDamaged()).draw(false);
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

function Add(id) {
    debugger;
    if (id != 1) {
        $('#AddTab').trigger('click');
    }
    clearfields();
    ChangeButtonPatchView('DefectiveorDamaged', 'btnPatchDefectiveorDamagedSettab', 'Add');
}
function goBack() {
    $('#DefectiveorDamagedTab').trigger('click');
    clearfields();
}

function ItemCodeOnChange(curObj) {
    debugger;
    try {
        var ID = curObj.value;
        var data = { "ID": ID };
        var ds = {};
        ds = GetDataFromServer("DefectiveorDamaged/GetItemDescriptionByID/", data);
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        if (ds.Result == "OK") {
            $("#Description").val(ds.Records[0].Description);
            return ds.Records;
        }
        if (ds.Result == "ERROR") {
            notyAlert('error', ds.Message);
        }

    }
    catch (e) {

    }
}
//--------------------button actions ----------------------
function List() {
    try {

        ChangeButtonPatchView('DefectiveorDamaged', 'btnPatchDefectiveorDamagedSettab', 'List');

    } catch (x) {
        alert(x);
    }

}
//---------------get grid fill result-------------------
function GetAllDefectiveDamaged() {
    try {
        debugger;
        var data = {};
        var ds = {};
        ds = GetDataFromServer("DefectiveorDamaged/GetAllDefectiveDamaged/", data);
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
//------------------------------- Defective/Damaged Save-----------------------------//
function save() {
    debugger;

    $("#btnInsertUpdateDefectiveDamaged").trigger('click');
}