var DataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";
var _Materials = [];
//---------------------------------------Docuement Ready--------------------------------------------------//
$(document).ready(function () {
    try
    {
        DataTables.SalesReturnTable = $('#tblSalesReturnList').DataTable(
        {
            dom: '<"pull-left"Bf>rt<"bottom"ip><"clear">',
            buttons: [{
                extend: 'excel',
                exportOptions:
                             {
                                 columns: [1,2, 3, 4, 5, 6,7]
                             }
            }],

            order: [],
            searching: true,
            paging: true,
            data: GetAllSalesReturn(),
            columns: [
                   { "data": "ID", "defaultContent": "<i>-</i>" },
               
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
              
                { className: "text-right", "targets": [5] },
                { className: "text-center", "targets": [1, 2, 3, 6, 8] },
                 { className: "text-left", "targets": [4,7] },
                  {
                      "render": function (data, type, row) {
                          return (data == "True" ? "Returned" + '<i class="fa fa-check" style="color:green;" aria-hidden="true"></i>' : "Not Returned");
                      },
                      "targets": 6
                  }
            ]
        });

        $('#tblSalesReturnList tbody').on('dblclick', 'td', function () {

            Edit(this);
        });
        GetAllItemCode();
        EG_ComboSource('Materials', _Materials, 'ItemCode', 'ID', 'Description')
        $(".buttons-excel").hide();
    }
    catch(e)
    {
        notyAlert('error', e.message);
    }
});



//---------------------------------------Edit DefectiveDamaged--------------------------------------------------//
function Edit(currentObj) {
    //Tab Change on edit click
   
    $('#AddTab').trigger('click');
    ChangeButtonPatchView("SalesReturn", "btnPatchSalesReturnSettab", "Edit"); //ControllerName,id of the container div,Name of the action
  
    var rowData = DataTables.SalesReturnTable.row($(currentObj).parents('tr')).data();
    if ((rowData != null) && (rowData.ID != null)) {
        fillSalesReturn(rowData.ID);
        if (rowData.ReturnStatusYN == "True") {
            ReturnedFields();
        }
    }

}

function ReturnedFields() {
    $("#RefNo").prop('disabled', true);
    $("#ItemCode").prop('disabled', true);
    $("#Qty").prop('disabled', true);
    $("#Remarks").prop('disabled', true);
    $('#OpenDate').prop('disabled', true);
    ChangeButtonPatchView("SalesReturn", "btnPatchSalesReturnSettab", "Return");
}

//-----------------------------------------Reset Validation Messages--------------------------------------//
function ResetForm() {
    var validator = $("#formIns_Up").validate();
    $('#formIns_Up').find('.field-validation-error span').each(function () {
        validator.settings.success($(this));
    });
    validator.resetForm();
}

//--3-----------combo source binding----------------------------
function EG_ComboSource(id, values, valueCol, textCol, textDesc) {
   
    if (document.getElementById(id) == null || document.getElementById(id) == 'undefined') {
        alert("combo source element is not defined in cshtml");
    }

    var options = '';
    for (var i = 0; i < values.length; i++)
        options += '<option id="' + values[i][textCol] + '" value="' + values[i][valueCol] + '" >' + values[i][textDesc] + '</option>';

    document.getElementById(id).innerHTML = options;

    //
}

//------------------------------- SalesReturn Save-----------------------------//
function save() {
   
    if (($('#OpenDate').val() != "") && ($("#ItemID").val() != "") && ($("#Qty").val() != "") && ($("#RefNo").val()!=""))
    {
        var qty = SalesReturnValidation();
     
        var enteredQty = $("#Qty").val();
        var hdfQty = $("#HiddenQty").val();
        if (hdfQty == "") {
            hdfQty = "0";
        }
        if (qty == "0") {
            notyAlert('error', "Selected Item does not have enough stock. This entry cannot be done!");

        }
        else
        {
            enteredQty = parseInt(enteredQty);
            qty = parseInt(qty);
            hdfQty = parseInt(hdfQty);
            if (qty > enteredQty) {
                if (hdfQty != enteredQty) {
                    var totalQty = qty + hdfQty - enteredQty;
                    notyConfirm('Do you want to continue ?', ' SaveClick()', "", "Yes, Save it!", 1); 
                }
                else {
                    SaveClick();
                    
                }

            }
            else {
                notyAlert('error', "Selected Item does not have enough stock. Entry cannot be done!");
            }
        }
       
    }
    else
    {
        $("#btnInsertUpdateSalesReturn").trigger('click');
    }
   

}

function Delete() {

    notyConfirm('Are you sure to delete?', 'DeleteSalesReturn()', '', "Yes, delete it!");

}

//---------------------------------------Delete-------------------------------------------------------//
function DeleteSalesReturn() {
  
    var id = $("#ID").val();
    if (id != EmptyGuid) {
        $("#btnFormDelete").click();
    }
    else {
        notyAlert('error', 'Error');
    }
}

function DeleteSuccess(data, status) {
    var i = JSON.parse(data)
   
    switch (i.Result) {

        case "OK":
            BindAllSalesReturn();
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

function ReturnToCompany() {
   
    var ReturnID = $("#ID").val();
    var txtQTY = $("#Qty").val();
    txtQTY = parseInt(txtQTY);
    if (ReturnID != EmptyGuid) {
        var qty = SalesReturnValidation();
        notyConfirm("Do you want to continue ?", 'ReturnDefectiveDamaged()', "Office stock for selected item will be reduced from "+ (txtQTY+parseInt(qty)) +" to " + qty + ".", "Yes, Save it!");
    }
    else {
        notyAlert('error', 'Error');
    }
}
function ReturnDefectiveDamaged() {
    $("#btnFormReturn").click();
}
function ReturnSuccess(data, status) {
    var i = JSON.parse(data)
   

    switch (i.Result) {

        case "OK":
            BindAllSalesReturn();
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

//---------------------------------------Fill DefectiveDamaged--------------------------------------------------//
function fillSalesReturn(ID) {
  
    ChangeButtonPatchView("SalesReturn", "btnPatchSalesReturnSettab", "Edit");
    var thisItem = GetSalesReturnByID(ID); //Binding Data
    //Hidden
    $("#deleteId").val(thisItem[0].ID);
    $("#returnId").val(thisItem[0].ID);
    $("#ID").val(thisItem[0].ID);
    if (thisItem[0].OpenDate != null) {
        var $datepicker = $('#OpenDate');
        $datepicker.datepicker('setDate', new Date(thisItem[0].OpenDate));
    }
    $("#RefNo").val(thisItem[0].RefNo)
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

function ReturnItemCode(itemID) {
    
    var a = $("#Materials").find('option[id="' + itemID + '"]');
    $("#ItemCode").val(a.val());
}
//---------------------------------------Get DefectiveDamaged Details By ID-------------------------------------//
function GetSalesReturnByID(id) {
    try {
        var data = { "id": id };
        var ds = {};
        ds = GetDataFromServer("SalesReturn/GetSalesReturnByID/", data);
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

function SalesReturnSaveSuccess(data, status) {
  
    var JsonResult = JSON.parse(data)
    switch (JsonResult.Result) {
        case "OK":
            if ($("#ID").val() == EmptyGuid) {
                fillSalesReturn(JsonResult.Records.ID);
            }
            else {
                fillSalesReturn($("#ID").val());
            }
            BindAllSalesReturn();
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

//---------------------------------------Clear Fields-----------------------------------------------------//
function clearfields() {
    $("#ID").val(EmptyGuid);
    $("#RefNo").val("")
    $("#ItemCode").val("")
    $("#ItemID").val("");
    $("#Description").val("");
    $("#Qty").val("")
    $("#Remarks").val("")
    $("#deleteId").val("0")
    $("#returnId").val("0");
    $("#HiddenQty").val("");
    var $datepicker = $('#OpenDate');
    $datepicker.datepicker('setDate', null);
    $("#RefNo").prop('disabled', false);
    $("#ItemCode").prop('disabled', false);
    $("#Qty").prop('disabled', false);
    $("#Remarks").prop('disabled', false);
    $('#OpenDate').prop('disabled', false);
    ResetForm();
}

//---------------------------------------Bind All DefectiveDamaged----------------------------------------------//
function BindAllSalesReturn() {
    try {
       
        DataTables.SalesReturnTable.clear().rows.add(GetAllSalesReturn()).draw(false);
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}


function SaveClick() {
    $("#btnInsertUpdateSalesReturn").trigger('click');
}

function SalesReturnValidation() {
   
    try {
        var ItemID = $("#ItemID").val();
        var data = { "ItemID": ItemID };
        var ds = {};
        ds = GetDataFromServer("SalesReturn/SalesReturnValidation/", data);
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        return ds.Message;
    }
    catch (e) {

    }
}

function ItemCodeOnChange(i) {
 
    var val = $(i).val();
    var a = $("#Materials").find('option[value="' + val + '"]');
    var itemID = a.attr('id');
    $("#ItemID").val(itemID);
    var desc = a.text();
    $("#Description").val(desc);

}

function GetAllItemCode() {

  
    try {
        var filter = 1;
        var data = {"filter":filter};
        var ds = {};
       // ds = GetDataFromServer("Item/ItemsForDropdown/", data);
        ds = GetDataFromServer("SalesReturn/GetAllItemCode/", data);
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

//---------------get grid fill result-------------------
function GetAllSalesReturn() {
    try {
       
        var data = {};
        var ds = {};
        ds = GetDataFromServer("SalesReturn/GetAllSalesReturn/", data);
       
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

        ChangeButtonPatchView('SalesReturn', 'btnPatchSalesReturnSettab', 'List');

    } catch (x) {
        alert(x);
    }

}

function Add(id) {
  
    if (id != 1) {
        $('#AddTab').trigger('click');
    }
    clearfields();
    ChangeButtonPatchView('SalesReturn', 'btnPatchSalesReturnSettab', 'Add');
}

function goBack() {
    $('#SalesReturnTab').trigger('click');
    clearfields();
}

function ExportData() {
    try {

        $(".buttons-excel").trigger('click');
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}