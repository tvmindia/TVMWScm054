var DataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";
//---------------------------------------Docuement Ready--------------------------------------------------//
$(document).ready(function () {
    try
    {
       
        DataTables.itemTable = $('#tblItemList').DataTable(
         {
             dom: '<"pull-left"f>rt<"bottom"ip><"clear">',
             order: [],
             searching: true,
             paging: true,
             data: GetAllItems(),
             columns: [
               { "data": "ID", "defaultContent": "<i>-</i>" },
               { "data": "ItemCode", "defaultContent": "<i>-</i>" },
               { "data": "Description", "defaultContent": "<i>-</i>" },
               { "data": "Category", "defaultContent": "<i>-</i>" },
               { "data": "Subcategory", "defaultContent": "<i>-</i>" },
               { "data": "Stock", "defaultContent": "<i>-</i>" },
               { "data": "UOM", "defaultContent": "<i>-</i>" },
               { "data": "ReorderQty", "defaultContent": "<i>-</i>" },
               { "data": "ProductCommission", "defaultContent": "<i>-</i>" },
               { "data": "Remarks", "defaultContent": "<i>-</i>" },
               { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink" onclick="Edit(this)"><i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>' }
             ],
             columnDefs: [{ "targets": [0], "visible": false, "searchable": false }]
         });

        $('#tblItemList tbody').on('dblclick', 'td', function () {

            Edit(this);
        });
       
        //EG_GridDataTable = DataTables.DetailTable;
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

        ChangeButtonPatchView('Item', 'btnPatchItemSettab', 'List');

    } catch (x) {
        alert(x);
    }

}

//------------------------------- Item Save-----------------------------//


function save() {
    debugger;
      
        $("#btnInsertUpdateItem").trigger('click');
}

function ItemSaveSuccess(data, status) {
    debugger;
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
    debugger;
    ChangeButtonPatchView("Item", "btnPatchItemSettab", "Edit");
    var thisItem = GetItemDetailsByID(ID); //Binding Data
    //Hidden
    $("#ID").val(thisItem[0].ID);
    $("#ItemCode").val(thisItem[0].ItemCode);
    $("#Description").val(thisItem[0].Description)
    $("#Stock").val(thisItem[0].Stock)
    $("#UOM").val(thisItem[0].UOM)
    //Dropdown
    $("#ReorderQty").val(thisItem[0].ReorderQty)
    $("#ProductCommission").val(thisItem[0].ProductCommission)
    $("#Remarks").val(thisItem[0].Remarks)
    $("#deleteId").val(thisItem[0].ID);
    $("#CategoryID").val(thisItem[0].CategoryID);
    if (thisItem[0].SubCategoryID != null)
    {
        $("#Subcategory").val(thisItem[0].SubCategoryID);
    }
   
}

//---------------------------------------Clear Fields-----------------------------------------------------//
function clearfields() {
    $("#ID").val(EmptyGuid);
    $("#ItemCode").val("")
    $("#Description").val("")
    $("#Stock").val("")
    $("#UOM").val("")
    $("#ReorderQty").val("");
    $("#ProductCommission").val("");
    $("#Remarks").val("");
    $("#deleteId").val("0")
    $("#CategoryID").val("");
    $("#Subcategory").val("-1");
    ClearSubCategories();
    ResetForm();
}

function ClearSubCategories()
{
    $('#Subcategory').empty();
    $('#Subcategory').append("  <option value='-1'>-- Select SubCategory --</option>");
}

//---------------------------------------Delete-------------------------------------------------------//
function Delete() {
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
    debugger;

    switch (i.Result) {

        case "OK":
            BindAllItems();
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

//---------------------------------------Bind All Items----------------------------------------------//
function BindAllItems() {
    try {
        debugger;
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
    debugger;
    if (id != 1) {
        $('#AddTab').trigger('click');
    }
    clearfields();
    ChangeButtonPatchView('Item', 'btnPatchItemSettab', 'Add');
}

//---------------------------------------Edit Item--------------------------------------------------//
function Edit(currentObj) {
    //Tab Change on edit click
    debugger;
   
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
    debugger;
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