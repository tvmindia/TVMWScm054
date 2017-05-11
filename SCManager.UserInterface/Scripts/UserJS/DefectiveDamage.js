var DataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";
var _Materials = [];
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
                  { "data": "OpenDateFormatted", "defaultContent": "<i>-</i>" },
                   { "data": "RefNo", "defaultContent": "<i>-</i>" },
                    { "data": "ItemCode", "defaultContent": "<i>-</i>" },        
                      
               { "data": "Description", "defaultContent": "<i>-</i>" },
                 { "data": "Qty", "defaultContent": "<i>-</i>" },
                 { "data": "ReturnStatusYN", "defaultContent": "<i>-</i>" },
                 { "data": "Remarks", "defaultContent": "<i>-</i>" },
               { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink" onclick="Edit(this)"><i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>' }
             ],
             columnDefs: [{ "targets": [0], "visible": false, "searchable": false },
                 { className: "text-right", "targets": [6] },
                 { className: "text-center", "targets": [1, 2, 3, 4, 7] },
                 { className: "text-left", "targets": [5,8] },
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

        GetAllItemCode();
        EG_ComboSource('Materials', _Materials, 'ItemCode', 'ID', 'Description')
    }
    catch (e) {
        notyAlert('error', e.message);
    }
   
});
//--3-----------combo source binding----------------------------
function EG_ComboSource(id, values, valueCol, textCol,textDesc) {
    debugger;
    if (document.getElementById(id) == null || document.getElementById(id) == 'undefined') {
        alert("combo source element is not defined in cshtml");
    }

    var options = '';
    for (var i = 0; i < values.length; i++)
        options += '<option id="' + values[i][textCol] + '" value="' + values[i][valueCol] + '" >' + values[i][textDesc] + '</option>';

    document.getElementById(id).innerHTML = options;

    //
}
function ItemCodeOnChange(i)
{
    debugger;
    var val=$(i).val();
    var a = $("#Materials").find('option[value="' + val + '"]');
    var itemID = a.attr('id');
    $("#ItemID").val(itemID);
    var desc = a.text();
    $("#Description").val(desc);
    
}
function ReturnItemCode(itemID)
{
    debugger;
    var a = $("#Materials").find('option[id="' + itemID + '"]');
    $("#ItemCode").val(a.val());
}
function Delete() {

    notyConfirm('Are you sure to delete?', 'DeleteDefectiveDamaged()');

}
//---------------------------------------Delete-------------------------------------------------------//
function DeleteDefectiveDamaged() {
    debugger;
    var id = $("#ID").val();
    if (id != EmptyGuid) {
        $("#btnFormDelete").click();
    }
    else {
        notyAlert('error', 'Error');
    }
}
function ReturnToCompany() {
    debugger;
    var ReturnID = $("#ID").val();
    if (ReturnID != EmptyGuid) {
        notyReturnConfirm('Are you sure to Return?', 'ReturnDefectiveDamaged()');
       
    }
    else {
        notyAlert('error', 'Error');
    }
}
function ReturnDefectiveDamaged()
{
    $("#btnFormReturn").click();
}
function ReturnSuccess(data, status) {
    var i = JSON.parse(data)
    debugger;

    switch (i.Result) {

        case "OK":
            BindAllDefectiveDamaged();
            ReturnedFields();
            notyAlert('success', i.Message);
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
function ReturnedFields()
{
    $("#EmpID").prop('disabled', true);
    $("#Type").prop('disabled', true);
    $("#RefNo").prop('disabled', true);
    $("#ItemCode").prop('disabled', true);
    $("#Qty").prop('disabled', true);
    $("#Remarks").prop('disabled', true);
    $('#OpenDate').prop('disabled', true);
    ChangeButtonPatchView("DefectiveorDamaged", "btnPatchDefectiveorDamagedSettab", "Return");
}
function DeleteSuccess(data, status) {
    var i = JSON.parse(data)
    debugger;

    switch (i.Result) {

        case "OK":
            BindAllDefectiveDamaged();
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
    $("#ReturnID").val(EmptyGuid);
    $("#EmpID").val("")
    $("#HiddenEmpID").val("");
    $("#Type").val("")
    $("#HiddenType").val("")
    $("#RefNo").val("")
    $("#ItemCode").val("")
    $("#ItemID").val("");
    $("#Description").val("");
    $("#Qty").val("")
    $("#HiddenQty").val("");
    $("#Remarks").val("")
    var $datepicker = $('#OpenDate');
    $datepicker.datepicker('setDate', null);
    $("#deleteId").val("0")
    $("#returnId").val("0");
    $("#EmpID").prop('disabled', false);
    $("#Type").prop('disabled', false);
    $("#RefNo").prop('disabled', false);
    $("#ItemCode").prop('disabled', false);
    $("#Qty").prop('disabled', false);
    $("#Remarks").prop('disabled', false);
    $('#OpenDate').prop('disabled', false);
    ResetForm();
}
//---------------------------------------Fill DefectiveDamaged--------------------------------------------------//
function fillDefectiveDamaged(ID) {
    debugger;
    ChangeButtonPatchView("DefectiveorDamaged", "btnPatchDefectiveorDamagedSettab", "Edit");
    var thisItem = GetDefectiveDamagedByID(ID); //Binding Data
    //Hidden
   
    $("#ID").val(thisItem[0].ID);
    $("#ReturnID").val(thisItem[0].ID);
    $("#Type").val(thisItem[0].Type);
    $("#HiddenType").val(thisItem[0].Type);
    $("#EmpID").val(thisItem[0].EmpID);
    $("#HiddenEmpID").val(thisItem[0].EmpID);
    if (thisItem[0].OpenDate != null) {
        var $datepicker = $('#OpenDate');
        $datepicker.datepicker('setDate', new Date(thisItem[0].OpenDate));
    }
    $("#RefNo").val(thisItem[0].RefNo)
    //$("#ItemID").val(thisItem[0].ItemID)
    $("#ItemID").val(thisItem[0].ItemID)
    $("#Description").val(thisItem[0].Description)
    $("#Qty").val(thisItem[0].Qty)
    $("#HiddenQty").val(thisItem[0].Qty)
    $("#Remarks").val(thisItem[0].Remarks)
    $("#deleteId").val(thisItem[0].ID);
    $("#returnId").val(thisItem[0].ID);
    $("#EmpID").prop('disabled', true);
    $("#Type").prop('disabled', true);
    ReturnItemCode(thisItem[0].ItemID);
}
//---------------------------------------Get DefectiveDamaged Details By ID-------------------------------------//
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
    debugger;
    var rowData = DataTables.DefectiveDamagedTable.row($(currentObj).parents('tr')).data();
    if ((rowData != null) && (rowData.ID != null)) {
        fillDefectiveDamaged(rowData.ID);
        if(rowData.ReturnStatusYN==true)
        {
            ReturnedFields();
        }
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

function TypeOnChange(curObj)
{
    try
    {
        if($("#Type").val()=="Damaged")
        {
            $("#EmpID").prop('disabled', true);
        }
        else
        {
            $("#EmpID").prop('disabled', false);
        }
    }
    catch(e)
    {

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
    var type = $("#Type").val();
   
    if ( ($("#Type").val() != "") && ($("#ItemID").val() != "") && ($("#Qty").val() != "") && ($("#OpenDate").val() != ""))
    {
        var qty = DefectiveDamagedValidation();
        var enteredQty = $("#Qty").val();
        var hdfQty = $("#HiddenQty").val();
        if (hdfQty == "") {
            hdfQty = "0";
        }
        debugger;
        if (qty == "0" ||qty=="No items")
        {           
            if (type == "Defective")
            {
                notyAlert('error', "Technician is not having enough stock of the selected item. Defective entry cannot be done!");
            }
            else
            {
                notyAlert('error', "Office is not having enough stock of the selected item. Damage entry cannot be done!");
            }
                
            
            
        }
        else
        {
            enteredQty = parseInt(enteredQty);
            qty = parseInt(qty);
            hdfQty = parseInt(hdfQty);
           
            if (qty > enteredQty)
            {
                if (hdfQty != enteredQty)
                {
                    var totalQty = qty + hdfQty - enteredQty;
                    if (type == "Defective")
                    {
                        notySaveConfirm("Do you want to continue ?", 'SaveClick()', "The technician's stock for selected item will reduce to " + totalQty + ".");
                    }
                    else
                    {
                        notySaveConfirm("Do you want to continue ?", 'SaveClick()', "The Office's stock for selected item will reduce to " + totalQty + ".");
                    }
                    
                }
                else
                {
                    SaveClick();
                    //var remainingQty = qty - enteredQty;

                    //notySaveConfirm("The technician's stock for selected item will reduce to " + remainingQty + " .\n Do you want to continue ? ", 'SaveClick()');
                }
               
            }
            else
            {
               
                if (type == "Defective") {
                    notyAlert('error', "Technician is not having enough stock of the selected item. Defective entry cannot be done!");
                }
                else {
                    notyAlert('error', "Office is not having enough stock of the selected item. Damage entry cannot be done!");
                }
                              
            }
            
        }
        
    }
    else
    {
        $("#btnInsertUpdateDefectiveDamaged").trigger('click');
    }
   
}

function SaveClick()
{
    $("#btnInsertUpdateDefectiveDamaged").trigger('click');
}

function DefectiveDamagedValidation() {
    try {
        debugger;
        var ItemID = $("#ItemID").val();
        var EmpID = $("#EmpID").val();
        var type = $("#Type").val();
        var data = { "ItemID": ItemID, "EmpID": EmpID,"type":type };
        var ds = {};
        ds = GetDataFromServer("DefectiveorDamaged/DefectiveDamagedValidation/", data);
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        return ds.Message;
    }
    catch (e) {
   
    }
}

function GetAllItemCode() {


    try {

        var data = {};
        var ds = {};
        ds = GetDataFromServer("DefectiveorDamaged/GetAllItemCode/", data);
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        if (ds.Result == "OK") {

            _Materials = ds.Records;


        }
        if (ds.Result == "ERROR") {
            alert(ds.Message);
        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }



}