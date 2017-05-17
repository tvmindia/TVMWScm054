var DataTables = {};
var emptyGUID = '00000000-0000-0000-0000-000000000000'
var _Materials = [];

//---------------------------------------Docuement Ready--------------------------------------------------//
$(document).ready(function () {
    try {

        var EventRequestsViewModel = new Object();
        DataTables.eventTable = $('#tblInvoices').DataTable(
         {
             dom: '<"pull-left"f>rt<"bottom"ip><"clear">',
             order: [],
             searching: true,
             paging: true,
             data: null,
             columns: [
               { "data": "SCCode" },
               { "data": "ID" },
               { "data": "InvoiceNo" },
               { "data": "InvoiceDateFormatted" },
               { "data": "VendorName", "defaultContent": "<i>-</i>" },
               { "data": "Subtotal", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
               { "data": "VATAmount", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },           
               { "data": "GrandTotal", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
               { "data": "Remarks", "defaultContent": "<i>-</i>" },
               { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink"  onclick="Edit(this)" ><i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>' }
             ],
             columnDefs: [{ "targets": [0], "visible": false, "searchable": false }, { "targets": [1], "visible": false, "searchable": false },
                  { className: "text-right", "targets": [5, 6, 7] },
             { className: "text-center", "targets": [2, 3, 4, 9] }

             ]
         });

        $('#tblInvoices tbody').on('dblclick', 'td', function () {

            Edit(this);
        });

        DataTables.DetailTable = $('#tblInvDetails').DataTable(
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
        EG_GridDataTable = DataTables.DetailTable;
        List();



    } catch (x) {

        notyAlert('error', e.message);

    }

});






//-----------------------EDIT GRID DEFN-------------------------------------
var EG_totalDetailRows = 0;
var EG_GridData;//DATA SOURCE OBJ ARRAY
var EG_GridDataTable;//DATA TABLE ITSELF FOR REBIND PURPOSE
var EG_SlColumn = 'SlNo';
var EG_GridInputPerRow = 4;
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
    tempObj.BasicAmount = "";
    tempObj.TradeDiscount = "";
    tempObj.NetAmount = "";

    return tempObj
}

function EG_Columns() {
    var obj = [
                { "data": "SCCode", "defaultContent": "<i></i>" },
                { "data": "ID", "defaultContent": "<i>0</i>" },
                 { "data": "MaterialID", "defaultContent": "<i></i>" },
                { "data": "SlNo", "defaultContent": "<i></i>" },
                { "data": "Material", render: function (data, type, row) { return (EG_createCombo(data, 'S', row, 'Material', 'Materials', 'FillUOM')); } },
                { "data": "Quantity", render: function (data, type, row) { return (EG_createTextBox(data, 'N', row, 'Quantity', 'CalculateAmount')); }, "defaultContent": "<i></i>" },
                { "data": "UOM", "defaultContent": "<i></i>" },
                { "data": "Rate", render: function (data, type, row) { return (EG_createTextBox(data, 'F', row, 'Rate', 'CalculateAmount')); }, "defaultContent": "<i></i>" },
                { "data": "BasicAmount", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i></i>" },
                { "data": "TradeDiscount", render: function (data, type, row) { return (EG_createTextBox(data, 'F', row, 'TradeDiscount', 'CalculateAmount')); }, "defaultContent": "<i></i>" },
                { "data": "NetAmount", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i></i>" },
                { "data": null, "orderable": false, "defaultContent": '<a href="#" class="DeleteLink"  onclick="DeleteItem(this)" ><i class="glyphicon glyphicon-trash" aria-hidden="true"></i></a>' }

    ]

    return obj

}

function EG_Columns_Settings() {

    var obj = [
        { "targets": [0], "visible": false, "searchable": false }, { "targets": [1], "visible": false, "searchable": false }, { "targets": [2], "visible": false, "searchable": false },
        { "targets": [4], "width": "20%" },
        { className: "text-right", "targets": [7, 9] },
        { className: "text-center", "targets": [3, 4, 5, 11] },
        { className: "text-right disabled", "targets": [8, 10] },
        { className: "text-center disabled", "targets": [6] },
        { "orderable": false, "targets": [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10] }

    ]

    return obj;

}

//------------------------------------------------------------------


//---------------Bind logics-------------------
function GetAllLocalPurchase() {
    try {

        var data = {};
        var ds = {};
        ds = GetDataFromServer("LocalPurchase/GetAllLocalPurchase/", data);
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

function BindLocalPurchase(id) {
    try {
        var data = { "ID": id };
        var ds = {};
        ds = GetDataFromServer("LocalPurchase/GetLocalPurchase/", data);
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        if (ds.Result == "OK") {

            BindLocalPurchaseFields(ds.Records);
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

function BindLocalPurchaseFields(Records) {
    try {

        debugger;
        $('#HeaderID').val(Records.ID);
        $('#InvNo').val(Records.InvoiceNo);
        $('#Remarks').val(Records.Remarks);       
        $('#VNo').val(Records.VendorName);      
        $('#subtotal').val(roundoff(Records.Subtotal));
        $('#vatamount').val(roundoff(Records.VATAmount));     
        $('#grandtotal').val(roundoff(Records.GrandTotal));
        EG_Rebind_WithData(Records.LocalPurchaseDetail, 1);
        $('#InvNo').attr('readonly', 'readonly');

        var $datepicker = $('#InvDate');
        $datepicker.datepicker('setDate', new Date(Records.InvoiceDateFormatted));
 


    } catch (e) {
        notyAlert('error', e.message);
    }




}




//--------------------button actions ----------------------
function List() {
    try {
        showLoader();
        ChangeButtonPatchView('LocalPurchase', 'btnPatchAttributeSettab', 'List');
        DataTables.eventTable.clear().rows.add(GetAllLocalPurchase()).draw(false);
        hideLoader();
    } catch (x) {
        // alert(x);
    }

}

function Add() {

    showLoader();
    ChangeButtonPatchView('LocalPurchase', 'btnPatchAttributeSettab', 'Add');
    EG_ClearTable();
    RestLocalPurchase();
    EG_AddBlankRows(5)

    hideLoader();
}

function DeleteClick() {

    notyConfirm('Are you sure to delete?', 'LocalPurchaseDelete()');

}

function LocalPurchaseDelete() {
    try {
        var id = $('#HeaderID').val();
        if (id != '' && id != null) {
            var data = { "ID": id };
            var ds = {};
            ds = GetDataFromServer("LocalPurchase/DeleteLocalPurchase/", data);
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

function LocalPurchaseDetailDelete(id, rw) {
    try {
        var Hid = $('#HeaderID').val();
        if (id != '' && id != null && Hid != '' && Hid != null && Hid != emptyGUID) {
            var data = { "ID": id, "HeaderID": Hid };
            var ds = {};
            ds = GetDataFromServer("LocalPurchase/DeleteLocalPurchaseDetail/", data);
            if (ds != '') {
                ds = JSON.parse(ds);
            }
            if (ds.Result == "OK") {
                notyAlert('success', ds.Message);
                BindLocalPurchase(Hid);
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

function DeleteItem(currentObj) {
    var rowData = EG_GridDataTable.row($(currentObj).parents('tr')).data();

    if ((rowData != null) && (rowData.ID != null)) {
        notyConfirm('Are you sure to delete?', 'LocalPurchaseDetailDelete("' + rowData.ID + '",' + rowData[EG_SlColumn] + ')');
    }
}

function RestLocalPurchase() {
    ClearFields();
    $('#HeaderID').val(emptyGUID);//clear field will make this field model invalid
    $('#InvNo').removeAttr('readonly')
    var $datepicker = $('#InvDate');
    $datepicker.datepicker('setDate', null);
    
}


function Edit(currentObj) {
    showLoader();
    var rowData = DataTables.eventTable.row($(currentObj).parents('tr')).data();
    //Event Request Case
    if ((rowData != null) && (rowData.ID != null)) {

        EG_ClearTable();
        $('#AddTab').trigger('click');
        if (BindLocalPurchase(rowData.ID)) {
            ChangeButtonPatchView('LocalPurchase', 'btnPatchAttributeSettab', 'Edit');

        }
        else {
            $('#ListTab').trigger('click');
        }

    }
    hideLoader();

}

function save() {
    var validation = EG_Validate();
    if (validation == "") {

        var result = JSON.stringify(EG_GridData);
        $("#DetailJSON").val(result);
        $("#savebutton").trigger('click');
    }
    else {
        notyAlert('error', validation);
    }

}

function reset() {
    EG_ClearTable();
    EG_AddBlankRows(5)
}

function resetCurrent() {
    try {
        debugger;
        var id = $('#HeaderID').val();
        BindLocalPurchase(id);


    } catch (e) {

    }
}

function SaveSuccess(data, status, xhr) {
    var i = JSON.parse(data)
    switch (i.Result) {
        case "OK":
            notyAlert('success', i.Message);

            BindLocalPurchaseFields(i.Records)
            ChangeButtonPatchView('LocalPurchase', 'btnPatchAttributeSettab', 'Edit');

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



//---------------------page related logics----------------------------------- 



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
    EGdic = EG_GridData[row - 1]['TradeDiscount'];

    qty = parseFloat(EGqty) || 0;
    rate = parseFloat(EGrate) || 0;
    dic = parseFloat(EGdic) || 0;
    if (dic > (qty * rate)) {
        dic = (qty * rate);
    }
    else if (dic < 0) {
        dic=0
    }

    EG_GridData[row - 1]['Rate'] = roundoff(rate);
    EG_GridData[row - 1]['BasicAmount'] = roundoff(qty * rate);
    EG_GridData[row - 1]['TradeDiscount'] = roundoff(dic);
    EG_GridData[row - 1]['NetAmount'] = roundoff(qty * rate - dic);
    EG_Rebind();

    var total = 0.00;
    for (i = 0; i < EG_GridData.length; i++) {
        total = total + (parseFloat(EG_GridData[i]['NetAmount']) || 0);
    }

    $('#subtotal').val(roundoff(total));
    AmountSummary();

}

function AmountSummary() {

    var subtotal = parseFloat($('#subtotal').val()) || 0;
    var vatamount = parseFloat($('#vatamount').val()) || 0;     
    var vatp = (parseFloat($('#vatpercentage').val()) || 0);
    if (vatp > 0) {
        vatamount = (subtotal * vatp) / 100;
        $('#vatamount').val(roundoff(vatamount));
    }

    $('#grandtotal').val(roundoff(subtotal + vatamount ));
}

var typingFlag = 0;
function calculateVat() {
    if (typingFlag == 0) {
        setTimeout(calculateVatPercentage, 2000);//done to wait till typing over
        typingFlag = 1;
    }
}

function calculateVatPercentage() {
    var vatp = parseFloat($('#vatpercentage').val()) || 0;
    var subtotal = parseFloat($('#subtotal').val()) || 0;
    if (vatp > 100) {
        vatp = 100;
    }
    if (vatp < 0) {
        vatp = 0;
    }

    $('#vatpercentage').val(vatp);
    $('#vatamount').val(roundoff(subtotal * vatp / 100));
    AmountSummary();
    typingFlag = 0;
}

function FillUOM(row) {

    for (i = 0; i < _Materials.length; i++) {
        if (_Materials[i].ItemCode == EG_GridData[row - 1]['Material']) {
            EG_GridData[row - 1]['UOM'] = _Materials[i].UOM;
            EG_GridData[row - 1]['MaterialID'] = _Materials[i].ID;
            EG_Rebind();
            break;
        }
    }

}

 