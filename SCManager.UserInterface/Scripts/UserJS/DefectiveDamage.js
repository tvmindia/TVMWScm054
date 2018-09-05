var DataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";
var _Materials = [];
//---------------------------------------Docuement Ready--------------------------------------------------//
$(document).ready(function () {
    try {
        DataTables.DefectiveDamagedTable = $('#tblDefectiveorDamagedList').DataTable(
         {
             dom: '<"pull-left"Bf>rt<"bottom"ip><"clear">',
             buttons: [{
                 extend: 'excel',
                 exportOptions:
                              {
                                  columns: [1,2, 3, 4, 5, 6, 7, 8,9]
                              }
             }],
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
                { "data": "Customer", "defaultContent": "<i>-</i>" },
                { "data": "ReturnStatusYN", "defaultContent": "<i>-</i>" },
                { "data": "ReceiveStatus", "defaultContent": "<i>-</i>" },
                { "data": "Remarks", "defaultContent": "<i>-</i>" },
               { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink" onclick="Edit(this)"><i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>' }
             ],
             columnDefs: [{ "targets": [0], "visible": false, "searchable": false },
                 { className: "text-right", "targets": [6] },
                 { className: "text-center", "targets": [1, 2, 3, 4,8] },
                 { className: "text-left", "targets": [5,7,9] },
                    {
                        "render": function (data, type, row) {
                            return (data == true ? "Returned " + '<i class="fa fa-check-circle" style="color:green;" aria-hidden="true"></i>' : "Not Returned");
                        },
                        "targets": 8

                    },
                     {
                         "render": function (data, type, row) {
                             return (data == 'Received' ? "Received " + '<i class="fa fa-check-circle" style="color:green;" aria-hidden="true"></i><p>(INV No:' + row.InvoiceNo + ')</p>' : data);
                         },
                         "targets": 9

                     }
             ]
         });

        $('#tblDefectiveorDamagedList tbody').on('dblclick', 'td', function () {

            Edit(this);
        });

        GetAllItemCode();
        EG_ComboSource('Materials', _Materials, 'ItemCode', 'ID', 'Description')
        $(".buttons-excel").hide();
    }

    catch (e) {
        notyAlert('error', e.message);
    }
   
});
//--3-----------combo source binding----------------------------
function EG_ComboSource(id, values, valueCol, textCol,textDesc) {
   
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
   
    var val=$(i).val();
    var a = $("#Materials").find('option[value="' + val + '"]');
    var itemID = a.attr('id');
    $("#ItemID").val(itemID);
    var desc = a.text();
    $("#Description").val(desc);
    
}
function ReturnItemCode(itemID)
{
  
    var a = $("#Materials").find('option[id="' + itemID + '"]');
    $("#ItemCode").val(a.val());
}
function Delete() {

    notyConfirm('Are you sure to delete?', 'DeleteDefectiveDamaged()', '', "Yes, delete it!");

}
//---------------------------------------Delete-------------------------------------------------------//
function DeleteDefectiveDamaged() {
   
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
    var type = $("#Type").val();
    var ReturnID = $("#ID").val();
    var ticketNo = $("#TicketNo").val();
    ticketNo = ticketNo.trim();
    var spuNo = $("#RefNo").val();
    spuNo = spuNo.trim();
    if ((type == "Defective" && ticketNo == "") || (type == "Defective" && spuNo == "")) {
        
        if (ticketNo == "" && spuNo!="") {
            $("#TicketNo").css('border-color', 'red');
            notyAlert('error', 'Please fill Ticket No to continue');
        }
       else if (spuNo == "" && ticketNo!="") {
           $("#RefNo").css('border-color', 'red');
           notyAlert('error', 'Please fill SPU No to continue');
       }
       else
       {
           notyAlert('error', 'Please fill SPU No and Tocken No to continue');
       }

    }
    else
    {
        if (ReturnID != EmptyGuid) {
            var qty = DefectiveDamagedValidation();
            var enteredQty = $("#Qty").val();
            var hdfQty = $("#HiddenQty").val();
            if (hdfQty == "") {
                hdfQty = "0";
            }
            enteredQty = parseInt(enteredQty);
            qty = parseInt(qty);
            hdfQty = parseInt(hdfQty);

            if (qty + hdfQty >= enteredQty) {

                var totalQty = qty + hdfQty - enteredQty;
                if (type == "Defective") {
                    notyConfirm("Are you sure to Return?", 'ReturnDefectiveDamaged()', "The technician's stock for selected item will reduce to " + totalQty + ".", "Yes, Return it!");
                }
                else {
                    notyConfirm("Are you sure to Return?", 'ReturnDefectiveDamaged()', "The Office's stock for selected item will reduce to " + totalQty + ".", "Yes, Return it!");
                }



            }
            //notyConfirm('Are you sure to Return?', 'ReturnDefectiveDamaged()', '', "Yes, Return it!");

        }
        else {
            notyAlert('error', 'Error');
        }
    }
  
}
function ReturnDefectiveDamaged()
{
    $("#btnFormReturn").click();
}
function ReturnSuccess(data, status) {
    var i = JSON.parse(data)
   

    switch (i.Result) {

        case "OK":
            BindAllDefectiveDamaged();
            ReturnedFields();
            notyAlert('success', i.Message);
            $("#MsgReturn").hide();
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
    $("#TicketNo").prop('disabled',true);
    $("#ItemCode").prop('disabled', true);
    $("#Qty").prop('disabled', true);
    $("#Remarks").prop('disabled', true);
    $('#OpenDate').prop('disabled', true);
    $("#Customer").prop('disabled', true);
    ChangeButtonPatchView("DefectiveorDamaged", "btnPatchDefectiveorDamagedSettab", "Return");
    $("#MsgReturn").hide();
}
function DeleteSuccess(data, status) {
    var i = JSON.parse(data)
   

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
            $("#MsgReturn").show();
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

function RemoveCss()
{
    $("#TicketNo").css('border-color', '');
}
function RemoveRefCSS()
{
    $("#RefNo").css('border-color', '');
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
    $("#TicketNo").val("")
    $("#ItemCode").val("")
    $("#ItemID").val("");
    $("#Description").val("");
    $("#Qty").val("")
    $("#HiddenQty").val("");
    $("#Remarks").val("")
    $("#TicketNo").css('border-color', '');
    $("#Customer").val("");
    var $datepicker = $('#OpenDate');
    $datepicker.datepicker('setDate', null);
    $("#deleteId").val("0")
    $("#returnId").val("0");
    $("#EmpID").prop('disabled', false);
    $("#Type").prop('disabled', false);
    $("#RefNo").prop('disabled', false);
    $("#TicketNo").prop('disabled',false)
    $("#ItemCode").prop('disabled', false);
    $("#Qty").prop('disabled', false);
    $("#Remarks").prop('disabled', false);
    $('#OpenDate').prop('disabled', false);
    $('#Customer').prop('disabled', false);
    ResetForm();
    $("#MsgReturn").hide();
}
//---------------------------------------Fill DefectiveDamaged--------------------------------------------------//
function fillDefectiveDamaged(ID) {
    
    ChangeButtonPatchView("DefectiveorDamaged", "btnPatchDefectiveorDamagedSettab", "Edit");
    $("#MsgReturn").show();
    var thisItem = GetDefectiveDamagedByID(ID); //Binding Data
    if (thisItem) {
        //Hidden
        if (thisItem[0].Type == "Damaged") {
            $("#lblRefNo").show();
            $("#lblSPUNo").hide();
        }
        else {
            $("#lblRefNo").hide();
            $("#lblSPUNo").show();
        }
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
        $("#TicketNo").val(thisItem[0].TicketNo)
        $("#Customer").val(thisItem[0].Customer)

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
  

    $('#AddTab').trigger('click');
    ChangeButtonPatchView("DefectiveorDamaged", "btnPatchDefectiveorDamagedSettab", "Edit"); //ControllerName,id of the container div,Name of the action
    $("#MsgReturn").show();
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
      
        DataTables.DefectiveDamagedTable.clear().rows.add(GetAllDefectiveDamaged()).draw(false);
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
    ChangeButtonPatchView('DefectiveorDamaged', 'btnPatchDefectiveorDamagedSettab', 'Add');
    $("#MsgReturn").hide();
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
            $("#lblRefNo").show();
            $("#lblSPUNo").hide();
        }
        else
        {
            $("#EmpID").prop('disabled', false);
            $("#lblRefNo").hide();
            $("#lblSPUNo").show();
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
        $("#MsgReturn").hide();

    } catch (x) {
        alert(x);
    }

}
//---------------get grid fill result-------------------
function GetAllDefectiveDamaged() {
    try {
      
        var data = {};
        var ds = {};
        ds = GetDataFromServer("DefectiveorDamaged/GetAllDefectiveDamaged/", data);
       
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
//------------------------------- Defective/Damaged Save-----------------------------//
function save() {
    debugger;
    var type = $("#Type").val();
    
        if (($("#Type").val() != "") && ($("#ItemID").val() != "") && ($("#Qty").val() != "") && ($("#OpenDate").val() != "")) {
            var qty = DefectiveDamagedValidation();
            var enteredQty = $("#Qty").val();
            var hdfQty = $("#HiddenQty").val();
            if (hdfQty == "") {
                hdfQty = "0";
            }
         
            if (qty == "0" || qty == "No items") {
                if (type == "Defective") {
                    notyAlert('error', "Technician is not having enough stock of the selected item. Defective entry cannot be done! Availabe Stock : "+qty);
                }
                else {
                    notyAlert('error', "Office is not having enough stock of the selected item. Damage entry cannot be done! Availabe Stock : "+qty);
                }



            }
            else {
                enteredQty = parseInt(enteredQty);
                qty = parseInt(qty);
                hdfQty = parseInt(hdfQty);

                if (qty + hdfQty >= enteredQty) {
                    if (hdfQty != enteredQty) {
                        var totalQty = qty + hdfQty - enteredQty;
                        if (type == "Defective") {
                            notyConfirm("Do you want to continue ?", 'SaveClick()', "", "Yes, Save it!",1);
                        }
                        else {
                            notyConfirm("Do you want to continue ?", 'SaveClick()', "", "Yes, Save it!",1);
                        }

                    }
                    else {
                        SaveClick();
                        //var remainingQty = qty - enteredQty;

                        //notySaveConfirm("The technician's stock for selected item will reduce to " + remainingQty + " .\n Do you want to continue ? ", 'SaveClick()');
                    }

                }
                else {

                    if (type == "Defective") {
                        notyAlert('error', "Technician is not having enough stock of the selected item. Defective entry cannot be done! Availabe Stock : "+qty);
                    }
                    else {
                        notyAlert('error', "Office is not having enough stock of the selected item. Damage entry cannot be done! Availabe Stock : "+qty);
                    }

                }

            }

        }
        else {
            $("#btnInsertUpdateDefectiveDamaged").trigger('click');
        }
    
  
   
}

function SaveClick()
{
    $("#btnInsertUpdateDefectiveDamaged").trigger('click');
}

function DefectiveDamagedValidation() {
    try {
     
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

    debugger;
    try {       
        var data = {  };
        var ds = {};
        //ds = GetDataFromServer("Item/ItemsForDropdown/", data);
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


function ReturnBill(this_obj) {
    $(this_obj).attr("href", "ReturnBill/Index");
}



function PrintReport() {
    debugger;
    try {
        $(".buttons-excel").trigger('click');
    }
    catch (e) {
        console.log(e.message);
    }
}
