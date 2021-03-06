﻿var DataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";
//---------------------------------------Docuement Ready--------------------------------------------------//
$(document).ready(function () {
    try
    {
       
        DataTables.itemTable = $('#tblItemList').DataTable(
         {
             dom: '<"pull-left"Bf>rt<"bottom"ip><"clear">',
             buttons: [{
                 extend: 'excel',
                 exportOptions:
                              {
                                  columns: [1,2,4,5,6,7,8,9,10,11,15 ]
                              }
             }],

             order: [],
             searching: true,
             paging: true,
             data: GetAllItems(),
             columns: [
               { "data": "ID", "defaultContent": "<i>-</i>" },
               { "data": "ItemCode", "defaultContent": "<i>-</i>" },
               { "data": "Description", "defaultContent": "<i>-</i>" },
               { "data": "HsnNo", "defaultContent": "<i>-</i>" },
               { "data": "Category", "defaultContent": "<i>-</i>" },
               { "data": "Subcategory", "defaultContent": "<i>-</i>" },
               { "data": "Stock", "defaultContent": "<i>-</i>" },
               { "data": "DefDamgStockQty", "defaultContent": "<i>-</i>" },
               { "data": "UOM", "defaultContent": "<i>-</i>" },
               { "data": "ReorderQty","defaultContent": "<i>-</i>" },
               { "data": "ProductCommission", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
               { "data": "SellingRate", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
               { "data": "CgstPercentage", "defaultContent": "<i>-</i>" },
               { "data": "SgstPercentage", "defaultContent": "<i>-</i>" },
                { "data": "IsActive", "defaultContent": "<i>-</i>" },
               { "data": "Remarks", "defaultContent": "<i>-</i>" },
               
               { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink" onclick="Edit(this)"><i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>' }
             ],
             columnDefs: [{ "targets": [0,5,8], "visible": false, "searchable": false },
                  { className: "text-right", "targets": [ 6, 8,10] },
                    { className: "text-center", "targets": [1,3, 4,5,7,9, 11,12,13,14,16] },
                    { className: "text-left", "targets": [2, 15] },
                       {
                           "render": function (data, type, row) {
                               return (data == false ? "No " : "Yes");
                           },
                           "targets": [14]

                       },
             ]
         });

        $('#tblItemList tbody').on('dblclick', 'td', function () {

            Edit(this);
        });
       
        //EG_GridDataTable = DataTables.DetailTable;
        $(".buttons-excel").hide();
    }
    catch(e)
    {
        notyAlert('error', e.message);
    }
});

//---------------get grid fill result-------------------
function GetAllItems() {
    try {
       
        var data = {};
        var ds = {};
        ds = GetDataFromServer("Item/GetAllItems/", data);
       
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

        ChangeButtonPatchView('Item', 'btnPatchItemSettab', 'List');

    } catch (x) {
        alert(x);
    }

}

//------------------------------- Item Save-----------------------------//

function save() {
   
      
        $("#btnInsertUpdateItem").trigger('click');
}

function ItemSaveSuccess(data, status) {
  
    var JsonResult = JSON.parse(data)
    switch (JsonResult.Result) {
        case "OK":
            if ($("#ID").val() == EmptyGuid)
            {
                fillItems(JsonResult.Records.itemID);
            }
            else
            {
                fillItems($("#ID").val());
            }
            BindAllItems();
            notyAlert('success', JsonResult.Message);
            break;
        case "ERROR":
            notyAlert('error', JsonResult.Message);
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

//---------------------------------------Get Item Details By ID-------------------------------------//
function GetItemDetailsByID(id) {
    try {
        var data = { "id": id };
        var ds = {};
        ds = GetDataFromServer("Item/GetItemByID/", data);
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

//---------------------------------------Fill Items--------------------------------------------------//
function fillItems(ID) {
      ChangeButtonPatchView("Item", "btnPatchItemSettab", "Edit");
    var thisItem = GetItemDetailsByID(ID); //Binding Data
    //Hidden
    $("#ID").val(thisItem[0].ID);
    $("#ItemCode").val(thisItem[0].ItemCode);
    $("#Description").val(thisItem[0].Description)
    $("#HsnNo").val(thisItem[0].HsnNo)
    $("#Stock").val(thisItem[0].Stock)
    $("#DefDamgStockQty").val(thisItem[0].DefDamgStockQty);
    $("#SCQty").val(thisItem[0].SCQty);
    $("#SalesReturnPendingQty").val(thisItem[0].SalesReturnPendingQty);
    $("#TechnicianQty").val(thisItem[0].TechnicianQty);
    $("#UOM").val(thisItem[0].UOM)
    //Dropdown
    $("#ReorderQty").val(thisItem[0].ReorderQty)
    $("#ProductCommission").val(roundoff(thisItem[0].ProductCommission))
    $("#SellingRate").val(roundoff(thisItem[0].SellingRate))
    $("#Remarks").val(thisItem[0].Remarks)
    $("#deleteId").val(thisItem[0].ID);
    $("#CategoryID").val(thisItem[0].CategoryID);
    $("#CgstPercentage").val(thisItem[0].CgstPercentage);
    $("#SgstPercentage").val(thisItem[0].SgstPercentage);
    if (thisItem[0].SubCategoryID != null)
    {
        $("#Subcategory").val(thisItem[0].SubCategoryID);
    }


    if (thisItem[0].IsActive == true) {
        $("#IsActive").prop('checked', true);
        // $('#IsActive').val(this.checked ? 1 : 0);

    }
    else {
        $("#IsActive").prop('checked', false);

    }
  
}

//---------------------------------------Clear Fields-----------------------------------------------------//
function clearfields() {
    $("#ID").val(EmptyGuid);
    $("#ItemCode").val("")
    $("#Description").val("")
    $("#HsnNo").val("")
    $("#Stock").val("")
    $("#DefDamgStockQty").val("")
    $("#SCQty").val("")
    $("#TechnicianQty").val("")
    $("#SalesReturnPendingQty").val("");
    $("#UOM").val("")
    $("#ReorderQty").val("");
    $("#ProductCommission").val("");
    $("#SellingRate").val("");
    $("#Remarks").val("");
    $("#deleteId").val("0")
    $("#CategoryID").val("");
    $("#Subcategory").val("-1");
    $("#CgstPercentage").val("");
    $("#SgstPercentage").val("");
    $("#IsActive").prop('checked', false);
    ClearSubCategories();
    ResetForm();
}

function ClearSubCategories()
{
    $('#Subcategory').empty();
    $('#Subcategory').append("  <option value='-1'>-- Select SubCategory --</option>");
}

function Delete() {

    notyConfirm('Are you sure to delete?', 'DeleteItem()', '', "Yes, delete it!");

}
//---------------------------------------Delete-------------------------------------------------------//
function DeleteItem() {
    var id = $("#ID").val();
    if (id != EmptyGuid) {
        $("#btnFormDelete").click();
    }
    else {
        notyAlert('error', 'Please Select Attributes');
    }
}

function DeleteSuccess(data, status) {
    var i = JSON.parse(data)
   

    switch (i.Result) {

        case "OK":
            BindAllItems();
            notyAlert('success', i.Message);
            clearfields();
            goBack();
            break;
        case "Error":
            notyAlert('error', i.Message);
            break;
        case "ERROR":
            notyAlert('error', i.Message);
            break;
        default:
            break;
    }
}

//---------------------------------------Bind All Items----------------------------------------------//
function BindAllItems() {
    try {
      
        DataTables.itemTable.clear().rows.add(GetAllItems()).draw(false);
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

function reset()
{
    if ($("#ID").val() == EmptyGuid) {
        clearfields();
    }
    else {
        fillItems($("#ID").val())
    }
    
}

function Add(id) {
    
    if (id != 1) {
        $('#AddTab').trigger('click');
    }
    clearfields();
    ChangeButtonPatchView('Item', 'btnPatchItemSettab', 'Add');
}

//---------------------------------------Edit Item--------------------------------------------------//
function Edit(currentObj) {
    //Tab Change on edit click
   
   
    $('#AddTab').trigger('click');
    ChangeButtonPatchView("Item", "btnPatchItemSettab", "Edit"); //ControllerName,id of the container div,Name of the action
    var rowData = DataTables.itemTable.row($(currentObj).parents('tr')).data();
    if ((rowData != null) && (rowData.ID != null)) {
        fillItems(rowData.ID);
    }
}



function goBack()
{
    $('#ListTab').trigger('click');
    clearfields();
}

function CategoryOnChange(curObj)
{
   
    try
    {
        var CategoryID = curObj.value;
        var data = { "CategoryID": CategoryID };
        var ds = {};
        ds = GetDataFromServer("Item/GetAllSubCategories/", data);
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        if (ds.Result == "OK") {
            ClearSubCategories();
            $.each(ds.Records, function (key, value) {
                $("#Subcategory").append($("<option></option>").val(value.ID).html(value.Description));
            });
            return ds.Records;
        }
        if (ds.Result == "ERROR") {
            notyAlert('error', ds.Message);
        }

    }
    catch(e)
    {

    }    
}


function ExportData() {
    try {

        $(".buttons-excel").trigger('click');
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}s