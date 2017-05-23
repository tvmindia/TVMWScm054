var DataTables = {};
var emptyGUID = '00000000-0000-0000-0000-000000000000'
var _Materials = [];
//---------------------------------------Docuement Ready--------------------------------------------------//

$(document).ready(function () {
    try {
        debugger;
        DataTables.customerBillsTable = $('#tblCustomerBills').DataTable(
        {
            dom: '<"pull-left"f>rt<"bottom"ip><"clear">',
            order: [],
            searching: true,
            paging: true,
            data: GetAllTCRBill(),
            columns: [             
              { "data": "ID", "defaultContent": "<i>-</i>" },          
              { "data": "Technician", "defaultContent": "<i>-</i>" },
              { "data": "JobNo", "defaultContent": "<i>-</i>" },
              { "data": "BillDateFormatted", "defaultContent": "<i>-</i>" },
              { "data": "BillNo", "defaultContent": "<i>-</i>" },
              { "data": "CustomerName", "defaultContent": "<i>-</i>" },
              { "data": "CustomerContactNo", "defaultContent": "<i>-</i>" },
               { "data": "CustomerLocation", "defaultContent": "<i>-</i>" },
              { "data": "Remarks", "defaultContent": "<i>-</i>" },
              { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink"  onclick="Edit(this)" ><i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>' }
            ],
            columnDefs: [{ "targets": [0], "visible": false, "searchable": false },
                 { className: "text-right", "targets": [] },
            { className: "text-center", "targets": [1,2, 3, 4, 9, 5, 6, 7, 8] }

            ]
        });
        debugger;
        DataTables.TCRBillDetail = $('#tblTCRBillDetails').DataTable(
       {
          
           dom: '<"pull-left"f>rt<"bottom"ip><"clear">',
           order: [],
           searching: false,
           paging: false,
           data: null,
           columns: EG_Columns(),
           columnDefs: EG_Columns_Settings()
       });

        getMaterials();
        EG_ComboSource('Materials', _Materials, 'ItemCode', 'Description')
        EG_GridDataTable = DataTables.TCRBillDetail;
        List();
        var $datepicker = $('#BillDate');
        $datepicker.datepicker('setDate', null);

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
var EG_MandatoryFields = 'MaterialID,Quantity,Rate'

function EG_TableDefn() {

    var tempObj = new Object();
    tempObj.SCCode = "";
    tempObj.ID = "";
    tempObj.MaterialID = "";
    tempObj.SlNo = 0;
    tempObj.Material = "";
    tempObj.Quantity = "";
    tempObj.UOM = "";
    tempObj.Rate = "";  
    tempObj.NetAmount = "";

    return tempObj
}

function EG_Columns() {
    debugger;
    var obj = [
            
                { "data": "ID", "defaultContent": "<i>0</i>" },
                 { "data": "MaterialID", "defaultContent": "<i></i>" },
                { "data": "SlNo", "defaultContent": "<i></i>" },
                { "data": "Material", render: function (data, type, row) { return (EG_createCombo(data, 'S', row, 'Material', 'Materials', 'FillUOM')); } },
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
        { "targets": [0], "visible": false, "searchable": false }, { "targets": [1], "visible": false, "searchable": false }, 
        { "targets": [4], "width": "20%" },
        { className: "text-right", "targets": [7] },
        { className: "text-center", "targets": [3, 4, 5] },
        { className: "text-right disabled", "targets": [7,8] },
        { className: "text-center disabled", "targets": [5] },
        { "orderable": false, "targets": [0, 1, 2, 3, 4, 5, 6, 7, 8] }

    ]

    return obj;

}

//------------------------------------------------------------------

//--------------------button actions ----------------------
function List() {
    try {
      
        ChangeButtonPatchView('TCRBillEntry', 'btnPatchTCRBillEntrySettab', 'List');
        // DataTables.customerBillsTable.clear().rows.add(GetAllForm8()).draw(false);
        reset();
        BindAllCustomerBill();
    } catch (x) {
        // alert(x);
    }

}

function goBack() {
    $('#AddTab').trigger('click');
    reset();
}

function CalculateSCCommissionAmt()
{
    debugger;
    var serviceCharge = $("#ServiceCharge").val();
    var SCcmmsn = $("#ServiceChargeComm").val();
    serviceCharge = parseInt(serviceCharge);
    SCcmmsn = parseInt(SCcmmsn);
    if (SCcmmsn != undefined && !isNaN(SCcmmsn))
    {
        $("#SCCommAmount").val(serviceCharge / SCcmmsn);
    }
   
}



//---------------------------------------Edit Item--------------------------------------------------//
function Edit(currentObj) {
    debugger;
    var rowData = DataTables.customerBillsTable.row($(currentObj).parents('tr')).data();
    //Event Request Case
    if ((rowData != null) && (rowData.ID != null)) {

        EG_ClearTable();
        $('#AddTab').trigger('click');
        if (BindTCRBillEntry(rowData.ID)) {
            ChangeButtonPatchView('TCRBillEntry', 'btnPatchTCRBillEntrySettab', 'Edit');

        }
        else {
            $('#ListTab').trigger('click');
        }

    }
    

}

function BindTCRBillEntry(id) {
    try {
        var data = { "ID": id };
        var ds = {};
        ds = GetDataFromServer("TCRBillEntry/GetTCRBillHeaderByID/", data);
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        if (ds.Result == "OK") {

            BindTCRBillEntryFields(ds.Records);
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
function DeleteItem(currentObj) {
    debugger;
    var rowData = EG_GridDataTable.row($(currentObj).parents('tr')).data();

    if ((rowData != null) && (rowData.ID != null)) {
        notyConfirm('Are you sure to delete?', 'TCRBillDetailDelete("' + rowData.ID + '","' + rowData[EG_SlColumn] + '")', '', "Yes, delete it!");
      //  notyConfirm('Are you sure to delete?', 'TCRBillDetailDelete("' + rowData.ID + '",' + rowData[EG_SlColumn] + ')','', "Yes, delete it!");
    }
}
function TCRBillDetailDelete(id, rw) {
    try {
        debugger;
        var Hid = $('#HeaderID').val();
        if (id != '' && id != null && Hid != '' && Hid != null && Hid != emptyGUID) {
            var data = { "ID": id, "HeaderID": Hid };
            var ds = {};
            ds = GetDataFromServer("TCRBillEntry/DeleteTCRBillDetail/", data);
            if (ds != '') {
                ds = JSON.parse(ds);
            }
            if (ds.Result == "OK") {
                notyAlert('success', ds.Message);
                BindTCRBillEntry(Hid);
            }
            if (ds.Result == "ERROR") {
                notyAlert('error', ds.Message);
                return 0;
            }
            return 1;
        }
        else {
            debugger;
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

function BindTCRBillEntryFields(Records) {
    try {

        debugger;
        $('#HeaderID').val(Records.ID);
        $("#EmpID").val(Records.EmpID);
        $("#JobNo").val(Records.JobNo);
        $("#BillNo").val(Records.BillNo);
        $("#CustomerName").val(Records.CustomerName);
        $("#CustomerContactNo").val(Records.CustomerContactNo);
        $("#CustomerLocation").val(Records.CustomerLocation);
        $("#PaymentMode").val(Records.PaymentMode);
        $("#Remarks").val(Records.Remarks);
        $("#subtotal").val(roundoff(Records.Subtotal));
        $("#ServiceCharge").val(roundoff(Records.ServiceCharge));
        $("#VATAmount").val(roundoff(Records.VATAmount));
        //$("#vatpercentage").val(Records.EmpID);
        $("#discount").val(roundoff(Records.Discount));
        $("#grandtotal").val(roundoff(Records.GrandTotal));
       // $("#ServiceChargeComm").val(Records.ServiceCharge/100);
        $("#SCCommAmount").val(roundoff(Records.SCCommAmount));
        $("#SpecialComm").val(roundoff(Records.SpecialComm));
        EG_Rebind_WithData(Records.TCRBillEntryDetail, 1);
        $('#BillNo').attr('readonly', 'readonly');

        var $datepicker = $('#BillDate');
        $datepicker.datepicker('setDate', new Date(Records.BillDate));

    } catch (e) {
        notyAlert('error', e.message);
    }




}


function FillUOM(row) {
    debugger;
    for (i = 0; i < _Materials.length; i++) {
        if (_Materials[i].ItemCode == EG_GridData[row - 1]['Material']) {
            EG_GridData[row - 1]['UOM'] = _Materials[i].UOM;
            EG_GridData[row - 1]['MaterialID'] = _Materials[i].ID;
            EG_Rebind();
            break;
        }
    }

}

function DeleteClick()
{
    notyConfirm('Are you sure to delete?', 'TCRBillDelete()');
}
function TCRBillDelete() {
    try {
        var id = $('#HeaderID').val();
        if (id != '' && id != null) {
            var data = { "ID": id };
            var ds = {};
            ds = GetDataFromServer("TCRBillEntry/DeleteTCRBillEntry/", data);
            if (ds != '') {
                ds = JSON.parse(ds);
            }
            if (ds.Result == "OK") {
                notyAlert('success', ds.Message);
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
function SaveSuccess(data, status) {
    debugger;
    var JsonResult = JSON.parse(data)
    switch (JsonResult.Result) {
        case "OK":
            if ($("#ID").val() == emptyGUID) {
                BindTCRBillEntryFields(JsonResult.Records.itemID);
            }
            else {
                BindTCRBillEntryFields($("#ID").val());
            }
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

function BindAllCustomerBill()
{
    try {
        debugger;
        DataTables.customerBillsTable.clear().rows.add(GetAllTCRBill()).draw(false);
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

//---------------get grid fill result-------------------
function GetAllTCRBill() {
    try {
        debugger;
        var data = {};
        var ds = {};
        ds = GetDataFromServer("TCRBillEntry/GetAllTCRBillEntry/", data);
        debugger;
        if (ds != '') {
            debugger;
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


function save()
{
    //$("#JobNo").val("123ERT32q");
    $("#ID").val(emptyGUID);
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

function Add() {
    debugger;
   
    ChangeButtonPatchView('TCRBillEntry', 'btnPatchTCRBillEntrySettab', 'Add');
    EG_ClearTable();
   // RestForm8();
    EG_AddBlankRows(5)
    reset();
}

function CalculateVAT()
{
    debugger;
    var vatpercent=$("#vatpercentage").val();
    var subTotal = $("#subtotal").val();
    vatpercent = parseInt(vatpercent);
    subTotal = parseInt(subTotal);
    $("#VATAmount").val(subTotal/vatpercent);
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

    var total = 0.00;
    for (i = 0; i < EG_GridData.length; i++) {
        total = total + (parseFloat(EG_GridData[i]['NetAmount']) || 0);
    }

    $('#subtotal').val(roundoff(total));
    AmountSummary();

}

function AmountSummary() {
    debugger;
    var subtotal = parseFloat($('#subtotal').val()) || 0;
    var vatamount = parseFloat($('#VATAmount').val()) || 0;
    var discount = parseFloat($('#discount').val()) || 0;
    var vatp = (parseFloat($('#vatpercentage').val()) || 0);
    if (vatp > 0) {
        vatamount = (subtotal * vatp) / 100;
        $('#VATAmount').val(roundoff(vatamount));
    }

    $('#grandtotal').val(roundoff(subtotal + vatamount - discount));
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

function reset()
{
    $("#EmpID").val("");
    $("#JobNo").val("");
    $("#BillNo").val("");
    $("#CustomerName").val("");
    $("#CustomerContactNo").val("");
    $("#CustomerLocation").val("");
    $("#PaymentMode").val("");
    $("#Remarks").val("");
    $("#subtotal").val("");
    $("#ServiceChargeComm").val("");
    $("#VATAmount").val("");
    $("#vatpercentage").val("");
    $("#discount").val("");
    $("#grandtotal").val("");
    $("#ServiceCharge").val("");
    $("#SCCommAmount").val("");
    $("#SpecialComm").val("");
    $('#BillNo').attr('readonly', false);
    var $datepicker = $('#BillDate');
    $datepicker.datepicker('setDate', null);
    ResetForm();
}
//-----------------------------------------Reset Validation Messages--------------------------------------//
function ResetForm() {
    var validator = $("#F8").validate();
    $('#F8').find('.field-validation-error span').each(function () {
        validator.settings.success($(this));
    });
    validator.resetForm();
}