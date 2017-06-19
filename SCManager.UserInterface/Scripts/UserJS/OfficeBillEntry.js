var DataTables = {};
var emptyGUID = '00000000-0000-0000-0000-000000000000'
var _Materials = [];
//---------------------------------------Docuement Ready--------------------------------------------------//
$(document).ready(function () {
    try {
        DataTables.customerBillsTable = $('#tblCustomerBills').DataTable(
 {
     dom: '<"pull-left"f>rt<"bottom"ip><"clear">',
     order: [],
     searching: true,
     paging: true,
     data: GetAllOfficeBill(),
     columns: [
       { "data": "ID", "defaultContent": "<i>-</i>" },       
       { "data": "BillDateFormatted", "defaultContent": "<i>-</i>" },
       { "data": "BillNo", "defaultContent": "<i>-</i>" },
       { "data": "CustomerName", "defaultContent": "<i>-</i>" },
       { "data": "CustomerContactNo", "defaultContent": "<i>-</i>" },
        { "data": "CustomerLocation", "defaultContent": "<i>-</i>" },      
       
       { "data": "Remarks", "defaultContent": "<i>-</i>" },
       { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink"  onclick="Edit(this)" ><i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>' }
     ],
     columnDefs: [{ "targets": [0], "visible": false, "searchable": false },
          { className: "text-right", "targets": [7] },
     { className: "text-center", "targets": [1, 2, 3, 4,5, 6] }

     ]
 });
        DataTables.OfficeBillDetail = $('#tblOfficeDetails').DataTable(
     {

         dom: '<"pull-left"f>rt<"bottom"ip><"clear">',
         order: [],
         searching: false,
         paging: false,
         data: null,
         autoWidth: false,
         columns: EG_Columns(),
         columnDefs: EG_Columns_Settings()
     });
        $('#tblCustomerBills tbody').on('dblclick', 'td', function () {

            Edit(this);
        });
        getMaterials();
        EG_ComboSource('Materials', _Materials, 'ItemCode', 'Description')
        EG_GridDataTable = DataTables.OfficeBillDetail;
        List();
    } catch (x) {

        notyAlert('error', x.message);

    }

});

//-----------------------EDIT GRID DEFN-------------------------------------
var EG_totalDetailRows = 0;
var EG_GridData;//DATA SOURCE OBJ ARRAY
var EG_GridDataTable;//DATA TABLE ITSELF FOR REBIND PURPOSE
var EG_SlColumn = 'SlNo';
var EG_GridInputPerRow = 3;
var EG_MandatoryFields = 'Quantity,Rate'

function EG_TableDefn() {

    var tempObj = new Object();
    tempObj.SCCode = "";
    tempObj.ID = "";
    tempObj.SlNo = 0;
    tempObj.Material = "";
    tempObj.Description = "";
    tempObj.MaterialID = "";
    tempObj.Quantity = "";
    tempObj.UOM = "";
    tempObj.Rate = "";
    tempObj.NetAmount = "";

    return tempObj
}

function EG_Columns() {
   
    var obj = [

                { "data": "ID", "defaultContent": "<i>0</i>" },
                { "data": "SlNo", "defaultContent": "<i></i>" },
                { "data": "Material", render: function (data, type, row) { return (EG_createCombo(data, 'S', row, 'Material', 'Materials', 'FillUOM')); } },
                 { "data": "Description", "defaultContent": "<i></i>" },
                { "data": "Quantity", render: function (data, type, row) { return (EG_createTextBox(data, 'N', row, 'Quantity', 'CalculateAmount')); }, "defaultContent": "<i></i>" },
                { "data": "UOM", "defaultContent": "<i></i>" },
                { "data": "Rate", render: function (data, type, row) { return (EG_createTextBox(data, 'F', row, 'Rate', 'CalculateAmount')); }, "defaultContent": "<i></i>" },
                { "data": "NetAmount", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i></i>" },
                { "data": null, "orderable": false, "defaultContent": '<a href="#" class="DeleteLink"  onclick="DeleteItem(this)" ><i class="glyphicon glyphicon-trash" aria-hidden="true"></i></a>' }

    ]

    return obj

}

function EG_Columns_Settings() {

    var obj = [
        { "targets": [0], "visible": false, "searchable": false },
        { "width": "5%", "targets": 1 },
         { "width": "15%", "targets": 2 },
        { "width": "20%", "targets": 3 },
         { "width": "8%", "targets": 4 },
        { "width": "8%", "targets": 5 },
         { "width": "8%", "targets": 6 },
          { "width": "10%", "targets": 7 },
          { "width": "5%", "targets": 8 },
        { className: "text-right", "targets": [6,4] },
        { className: "text-center", "targets": [1,8] },
        { className: "text-right disabled", "targets": [7] },
        { className: "text-center disabled", "targets": [5] },
          { className: "text-left disabled", "targets": [3] },
        { "orderable": false, "targets": [0, 1, 2, 3, 4, 5, 6, 7] }

    ]

    return obj;

}

function FillUOM(row) {

    for (i = 0; i < _Materials.length; i++) {
        if (_Materials[i].ItemCode == EG_GridData[row - 1]['Material']) {
            EG_GridData[row - 1]['UOM'] = _Materials[i].UOM;
            EG_GridData[row - 1]['MaterialID'] = _Materials[i].ID;
            EG_GridData[row - 1]['Description'] = _Materials[i].Description;     
            EG_GridData[row - 1]['Rate'] = _Materials[i].SellingRate;
            EG_Rebind();
            break;
        }
    }

}
function DeleteItem(currentObj) {

    var rowData = EG_GridDataTable.row($(currentObj).parents('tr')).data();

    if ((rowData != null) && (rowData.ID != null)) {
        notyConfirm('Are you sure to delete?', 'OfficeBillDetailDelete("' + rowData.ID + '","' + rowData[EG_SlColumn] + '")', '', "Yes, delete it!");
        
    }
}
function BindAllCustomerBill() {
    try {

        DataTables.customerBillsTable.clear().rows.add(GetAllOfficeBill()).draw(false);
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

function OfficeBillDetailDelete(id, rw) {
    try {
        
        var Hid = $('#HeaderID').val();
        if (id != '' && id != null && Hid != '' && Hid != null && Hid != emptyGUID) {
            var data = { "ID": id, "HeaderID": Hid };
            var ds = {};
            ds = GetDataFromServer("OfficeBillEntry/DeleteOfficeBillDetail/", data);
            if (ds != '') {
                ds = JSON.parse(ds);
            }
            if (ds.Result == "OK") {
                notyAlert('success', ds.Message);
                BindOfficeBillEntry(Hid);
            }
            if (ds.Result == "ERROR") {
                notyAlert('error', ds.Message);
                return 0;
            }
            return 1;
        }
        else {

            if (EG_GridData.length != 1) {
                EG_GridData.splice(rw - 1, 1);
                EG_Rebind_WithData(EG_GridData, 0);
            }
            else {
                reset();
                EG_Rebind();
            }
            notyAlert('success', 'Deleted Successfully');

        }

    }
    catch (e) {
        notyAlert('error', e.message);
        return 0;
    }

}

//------------------------------------------------------------------

//---------------------------------------Edit Item--------------------------------------------------//
function Edit(currentObj) {

    var rowData = DataTables.customerBillsTable.row($(currentObj).parents('tr')).data();
    //Event Request Case
    if ((rowData != null) && (rowData.ID != null)) {

        EG_ClearTable();
        $('#AddTab').trigger('click');
        $("#HeaderID").val(rowData.ID);
        if (BindOfficeBillEntry(rowData.ID)) {
            ChangeButtonPatchView('OfficeBillEntry', 'btnPatchOfficeBillEntrySettab', 'Edit');

        }
        else {
            $('#ListTab').trigger('click');
        }

    }


}

//--------------------button actions ----------------------
function List() {
    try {
     
        ChangeButtonPatchView('OfficeBillEntry', 'btnPatchOfficeBillEntrySettab', 'List');      
        $("#HeaderID").val("");
        reset();
        BindAllCustomerBill();
    } catch (x) {
        // alert(x);
    }

}
function SaveSuccess(data, status) {

    var JsonResult = JSON.parse(data)
    switch (JsonResult.Result) {
        case "OK":
            BindOfficeBillEntry(JsonResult.Records.ID);
            BindAllCustomerBill();
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
function BindOfficeBillEntry(id) {
    try {
        var data = { "ID": id };
        var ds = {};
        ds = GetDataFromServer("OfficeBillEntry/GetOfficeBillHeaderByID/", data);
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        if (ds.Result == "OK") {

            BindOfficeBillEntryFields(ds.Records);
        }
        if (ds.Result == "ERROR") {
            notyAlert('error', ds.Message);
            return 0;
        }
        return 1;

    }
    catch (e) {
        notyAlert('error', e.message);
        return 0;
    }

}
function BindOfficeBillEntryFields(Records) {
    try {
        ChangeButtonPatchView('OfficeBillEntry', 'btnPatchOfficeBillEntrySettab', 'Edit');
        $('#HeaderID').val(Records.ID);      
        $("#BillNo").val(Records.BillNo);
        $("#CustomerName").val(Records.CustomerName);
        $("#CustomerContactNo").val(Records.CustomerContactNo);
        $("#CustomerLocation").val(Records.CustomerLocation);
        $("#PaymentMode").val(Records.PaymentMode);
        $("#PaymentRefNo").val(Records.PaymentRefNo);
        $("#Remarks").val(Records.Remarks);
        $("#subtotal").val(roundoff(Records.Subtotal));       
        $("#VATAmount").val(roundoff(Records.VATAmount));
        $("#VATPercentageAmount").val(roundoff(Records.VATAmount)); 
        $("#discount").val(roundoff(Records.Discount));
        $("#total").val(roundoff(Records.Subtotal - Records.Discount));
        $("#grandtotal").val(roundoff(Records.GrandTotal));       
        EG_Rebind_WithData(Records.OfficeBillEntryDetail, 1);
        //$('#BillNo').attr('readonly', 'readonly');
       
        var $datepicker = $('#BillDate');
        $datepicker.datepicker('setDate', new Date(Records.BillDate));

    } catch (e) {
        notyAlert('error', e.message);
    }




}
function goBack() {
    $('#ListTab').trigger('click');
    $("#HeaderID").val("");
    reset();
}
function Add() {
    ChangeButtonPatchView('OfficeBillEntry', 'btnPatchOfficeBillEntrySettab', 'Add');
    EG_ClearTable();
    EG_AddBlankRows(5)
    $("#HeaderID").val("");
    reset();
}
function DeleteClick() {
  
    notyConfirm('Are you sure to delete?', 'OfficeBillDelete()', '', "Yes, delete it!");
}
function OfficeBillDelete() {
    try {
       
        var id = $('#HeaderID').val();
        if (id != '' && id != null) {
            var data = { "ID": id };
            var ds = {};
            ds = GetDataFromServer("OfficeBillEntry/DeleteOfficeBillEntry/", data);
            if (ds != '') {
                ds = JSON.parse(ds);
            }
            if (ds.Result == "OK") {
                notyAlert('success', ds.Message);
               // goBack();
                $('#ListTab').trigger('click');
            }
            if (ds.Result == "ERROR") {
                notyAlert('error', ds.Message);
                return 0;
            }
            return 1;
        }

    }
    catch (e) {
        notyAlert('error', e.message);
        return 0;
    }

}
function GetAllOfficeBill() {
    try {
       
        var data = {};
        var ds = {};
        ds = GetDataFromServer("OfficeBillEntry/GetAllOfficeBillEntry/", data);
      
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
function CalculateAmount(row) {




    //EG_GridData[row-1][Quantity] = value
    var qty = 0.00;
    var rate = 0.00;
    var dic = 0.00;

    var EGqty = '';
    var EGrate = '';
    var EGdic = '';

    EGqty = EG_GridData[row - 1]["Quantity"];
    EGrate = EG_GridData[row - 1]['Rate'];
    EGdic = EG_GridData[row - 1]['Discount'];

    qty = parseFloat(EGqty) || 0;
    rate = parseFloat(EGrate) || 0;
    dic = parseFloat(EGdic) || 0;

    if (dic > (qty * rate)) {
        dic = (qty * rate);
    }
    else if (dic < 0) {
        dic = 0
    }

    EG_GridData[row - 1]['Rate'] = roundoff(rate);
    EG_GridData[row - 1]['NetAmount'] = roundoff(qty * rate);
    EG_GridData[row - 1]['Discount'] = roundoff(dic);
    EG_Rebind();
    
    AmountSummary();

}

function DiscountChange()
{
    var subtotal = parseFloat($('#subtotal').val()) || 0;
    var discount = parseFloat($('#discount').val()) || 0;
    $('#total').val(roundoff(subtotal - discount));
    AmountSummary()
}

function ClearDiscountPercentage() {
    debugger;
    if ($('#VATAmount').val() != $('#VATPercentageAmount').val())
        $("#vatpercentage").val("");

    var total = parseFloat($('#total').val()) || 0;
    var vatAmount = parseFloat($('#VATAmount').val()) || 0;
    $('#grandtotal').val(roundoff(total + vatAmount )); 
}

function AmountSummary() {
    debugger;
    var Total = 0.00;
    for (i = 0; i < EG_GridData.length; i++) {
        Total = Total + (parseFloat(EG_GridData[i]['NetAmount']) || 0);
    } 
    $('#subtotal').val(roundoff(Total));
    var discount = parseFloat($('#discount').val()) || 0;
    $('#total').val(roundoff(Total - discount));
    if ($("#vatpercentage").val()!="")
        CalculateVAT();

    var total = parseFloat($('#total').val()) || 0;
    var vatAmount = parseFloat($('#VATAmount').val()) || 0;
    var discount = parseFloat($('#discount').val()) || 0;   
    $('#grandtotal').val(roundoff(total + vatAmount )); 
    $('#total').val(roundoff(total)); 
}
function CalculateVAT() {
    debugger;
    var vatpercent = $("#vatpercentage").val();
    var Total = $("#total").val();

    vatpercent = parseFloat(vatpercent);

    if (vatpercent > 100) {
        vatpercent = 100
        $("#vatpercentage").val(vatpercent);
    }
    if (vatpercent < 0) {
        vatpercent = 0
        $("#vatpercentage").val(vatpercent);
    }
    Total = parseFloat(Total);
    var vatamt = (Total * vatpercent / 100)
    if (isNaN(vatamt)) { vatamt = 0.00 }
    $("#VATAmount").val(roundoff(vatamt));
    $('#VATPercentageAmount').val(roundoff(vatamt));

    
}
//-----------------------------------------Reset Validation Messages--------------------------------------//
function ResetForm() {
    var validator = $("#Office").validate();
    $('#Office').find('.field-validation-error span').each(function () {
        validator.settings.success($(this));
    });
    validator.resetForm();
}
function save() {
    var validation = EG_Validate();
    if (validation == "") {
        var result = JSON.stringify(EG_GridData);
        $("#DetailJSON").val(result);
        $("#btnSave").trigger('click');
    }
    else {
        notyAlert('error', validation);
    }
}

function reset()
{
    if (($("#HeaderID").val() == "") || ($("#HeaderID").val() == 'undefined') || ($("#HeaderID").val() == "0")) {
            
        
        $("#BillNo").val("");
        $("#CustomerName").val("");
        $("#CustomerContactNo").val("");
        $("#CustomerLocation").val("");
        $("#PaymentMode").val("");
        $("#PaymentRefNo").val("");
        $("#Remarks").val("");
        $("#subtotal").val("");       
        $("#VATAmount").val("");
        $("#vatpercentage").val("");
        $("#discount").val("");
        $("#grandtotal").val("");     
      //  $('#BillNo').attr('readonly', false);      
        var $datepicker = $('#BillDate');
        $datepicker.datepicker('setDate', null);
        EG_ClearTable();
        EG_AddBlankRows(5);
        ResetForm();
    }
    else {
        BindOfficeBillEntry($("#HeaderID").val());
        $("#vatpercentage").val("");
    }
}
function getMaterials() {


    try {

        var data = {};
        var ds = {};
        ds = GetDataFromServer("Item/ItemsForDropdown/", data);
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