var DataTables = {};
var emptyGUID = '00000000-0000-0000-0000-000000000000'
var _Materials = [];
var ticketNo = "";


//---------------------------------------Docuement Ready--------------------------------------------------//
$(document).ready(function () {
    try {
        
        var EventRequestsViewModel = new Object();
        DataTables.returnBillTable = $('#tblReturnBill').DataTable(
         {
             dom: '<"pull-left"f>rt<"bottom"ip><"clear">',
             order: [],
             searching: true,
             paging: true,
             data: null,
             columns: [
               { "data": "SCCode", "defaultContent": "<i>-</i>" },
               { "data": "ID", "defaultContent": "<i>-</i>" },
               { "data": "InvoiceNo" ,"defaultContent": "<i>-</i>" },
               { "data": "InvoiceDateFormatted","defaultContent": "<i>-</i>" },
               { "data": "ShippingCustomerName", "defaultContent": "<i>-</i>" },
               { "data": "TotalValue", render: function (data, type, row) { return roundoff(data,1); }, "defaultContent": "<i>-</i>" },
               { "data": "TotalTaxAmount", render: function (data, type, row) { return roundoff(data); }, "defaultContent": "<i>-</i>" },
               { "data": "GrandTotal", render: function (data, type, row) { return roundoff(data,1); }, "defaultContent": "<i>-</i>" },                                       
               { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink"  onclick="Edit(this)" ><i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>' }
             ],
             columnDefs: [{ "targets": [0], "visible": false, "searchable": false }, { "targets": [1], "visible": false, "searchable": false },
                { className: "text-left", "targets": [2] },
                { className: "text-right", "targets": [5,6,7] },
                { className: "text-center", "targets": [3,4,8] },
                 { "width": "15%", "targets": [7] },
                 { "width": "15%", "targets": [1] },
                 { "width": "15%", "targets": [2] },
                 { "width": "15%", "targets": [3] },

                { "orderable": false, "targets": [0, 1, 2, 3, 4, 5, 6, 7, 8] }
              
             ]
             
         });
        ticketNo = $('#TicketNo').val();
        $('#tblReturnBill tbody').on('dblclick', 'td', function () {
           
            Edit(this);
        });

        DataTables.DetailTable = $('#tblInvDetails').DataTable(
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
    tempObj.Description = "";
    tempObj.Quantity = "";
    tempObj.UOM = "";
    tempObj.Rate = "";
    tempObj.BasicAmount = "";
    tempObj.TradeDiscount = "";
    tempObj.CGSTPercentage = "";
    tempObj.CGSTAmount = "";
    tempObj.SGSTPercentage = "";
    tempObj.SGSTAmount = "";
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
                { "data": "Description", "defaultContent": "<i></i>" },
                { "data": "Quantity", render: function (data, type, row) { return (EG_createTextBox(data, 'N', row, 'Quantity', 'CalculateAmount')); }, "defaultContent": "<i></i>" },
                { "data": "UOM", "defaultContent": "<i></i>" },
                { "data": "Rate", render: function (data, type, row) { return (EG_createTextBox(data, 'F', row, 'Rate', 'CalculateAmount')); }, "defaultContent": "<i></i>" },
                { "data": "BasicAmount", render: function (data, type, row) { return roundoff(data,1); }, "defaultContent": "<i></i>" },
                { "data": "TradeDiscount", render: function (data, type, row) { return (EG_createTextBox(data, 'F', row, 'TradeDiscount', 'CalculateAmount')); }, "defaultContent": "<i></i>" },
                { "data": "CGSTPercentage", render: function (data, type, row) { return (EG_createTextBox(data, 'F', row, 'CGSTPercentage', 'CalculateCGST')); }, "defaultContent": "<i></i>" },
                { "data": "CGSTAmount", render: function (data, type, row) { return roundoff(data,1); }, "defaultContent": "<i></i>" },
                { "data": "SGSTPercentage", render: function (data, type, row) { return (EG_createTextBox(data, 'F', row, 'SGSTPercentage', 'CalculateSGST')); }, "defaultContent": "<i></i>" },
                { "data": "SGSTAmount", render: function (data, type, row) { return roundoff(data,1); }, "defaultContent": "<i></i>" },
                { "data": "NetAmount", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i></i>" },
                { "data": null, "orderable": false, "defaultContent": '<a href="#" class="DeleteLink"  onclick="DeleteItem(this)" ><i class="glyphicon glyphicon-trash" aria-hidden="true"></i></a>' }

    ]

    return obj

}

function EG_Columns_Settings() {

    var obj = [
        { "targets": [0], "visible": false, "searchable": false }, { "targets": [1], "visible": false, "searchable": false }, { "targets": [2], "visible": false, "searchable": false }, { "targets": [15], "visible": false },
        { "width": "5%", "targets": 3 },
        { "width": "10%", "targets": 4 },
        { "width": "10%", "targets": 5 },
        { "width": "8%", "targets": 6 },
        { "width": "8%", "targets": 7 },
        { "width": "8%", "targets": 8 },
        { "width": "8%", "targets": 9 },
        { "width": "8%", "targets": 10 },
        { "width": "8%", "targets": 11 },
        { "width": "8%", "targets": 12 },
        { "width": "8%", "targets": 13 },
        { "width": "8%", "targets": 14 },
        //{ className: "text-right", "targets": [] },
        { className: "text-center", "targets": [3, 4,6,8,10,11,13] },
       // { className: "text-right disabled", "targets": [] },
        { className: "text-center disabled", "targets": [9,12,7,14] },
        { className: "text-left disabled", "targets": [5] },
        { "orderable": false, "targets": [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10,11,12,13,14] }

    ]

    return obj;

}

//------------------------------------------------------------------


//---------------Bind logics-------------------
function GetAllReturnBill() {
    try {
        debugger;
        var data = {};
        var ds = {};
        ds = GetDataFromServer("ReturnBill/GetAllReturnBill/", data);
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

function GetAllFranchiseeDetail() {
    try {
        debugger;
        var data = {};
        var ds = {};
        ds = GetDataFromServer("ReturnBill/GetAllFranchiseeDetail/", data);
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

function GetSupplierDetail() {
    try {
        debugger;
        var data = {};
        var ds = {};
        ds = GetDataFromServer("ReturnBill/GetSupplierDetail/", data);
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


function BindReturnBill(id) {
    debugger;
    try {
        var data = { "ID": id };
        var ds = {};
        ds = GetDataFromServer("ReturnBill/GetReturnBillHeaderByID/", data);
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        if (ds.Result == "OK") {            

            BindReturnBillFields(ds.Records);
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
//------..//
function BindReturnBillFields(Records) {
    debugger;
    try {

        $('#HeaderID').val(Records.ID);
        //toggle control for view purpose
        $('#TicketNo').hide();
        $('#TicketNo').prop('disabled', true);
        $('#txtTicketNo').show();
        $('#txtTicketNo').val(Records.TicketNo);
        $('#txtTicketNo').prop('disabled', true);
        $('#InvNo').val(Records.InvoiceNo);        
        $('#Remarks').val(Records.Remarks);
        $('#CName').val(Records.CustomerName);
        $('#CPhoneNo').val(Records.CustomerPhoneNo);
        $('#CAddress').val(Records.CustomerAddress);
        $('#CEmail').val(Records.CustomerEmail);
        $('#CGstIn').val(Records.CustomerGstIn);
        $('#CPanNo').val(Records.CustomerPanNo);
        $('#Place').val(Records.PlaceOfSupply);
        $('#SName').val(Records.ShippingCustomerName);
        $('#SAddress').val(Records.ShippingAddress);
        $('#SGstIn').val(Records.ShippingGstIn);
        $('#SPanNo').val(Records.ShippingPanNo);
        $('#SPhoneNo').val(Records.ShippingCustomerPhoneNo);
        $('#SEmail').val(Records.ShippingCustomerEmail);
        $('#Total').val(roundoff(Records.TotalValue));
        $('#totaltaxamount').val(roundoff(Records.TotalTaxAmount));
        $('#grandtotal').val(roundoff(Records.GrandTotal));        
        if (Records.ReturnStatusYN == true)
        {           
            ChangeButtonPatchView('ReturnBill', 'btnPatchAttributeSettab', 'EditReturn');          
            ReturnFields();            
        }
        else
        {
            ChangeButtonPatchView('ReturnBill', 'btnPatchAttributeSettab', 'Edit');
            
        }
        EG_Rebind_WithData(Records.ReturnBillDetail, 1);
        if (Records.ReturnStatusYN == true) {
            $(".DeleteLink").hide();

        }
        var $datepicker = $('#InvDate');      
        $datepicker.datepicker('setDate', new Date(Records.InvoiceDateFormatted));



    } catch (e) {
        notyAlert('error', e.message);
    } 
}

//Math.round($('#grandtotal').val());
//--------------------button actions ----------------------
function List() {
    try {
        showLoader();
        ChangeButtonPatchView('ReturnBill', 'btnPatchAttributeSettab', 'List');
        DataTables.returnBillTable.clear().rows.add(GetAllReturnBill()).draw(false);
          hideLoader();
    } catch (x) {
       // alert(x);
    }
}

function Add() {
    try {
        debugger;
        ResetForm();
        RestReturnBill();
        // ticketNo = $('#TicketNo').val();
        $('#TicketNo').show();
        $('#TicketNo').prop('disabled', false);
        $('#txtTicketNo').hide();
        var thisSupplierItem = GetSupplierDetail();
        $("#CName").val(thisSupplierItem[0].CompanyDescription);
        $("#CAddress").val(thisSupplierItem[0].CompanyAddress);
        $("#CEmail").val(thisSupplierItem[0].CompanyEmail);
        $("#CPhoneNo").val(thisSupplierItem[0].CompanyContactNo);
        $("#CGstIn").val(thisSupplierItem[0].CompanyGstIn);
        $("#CPanNo").val(thisSupplierItem[0].CompanyPanNo);
        $("#Place").val(thisSupplierItem[0].CompanyPlace);
        
      
    }
    catch(X)
    {}
    showLoader();
    ChangeButtonPatchView('ReturnBill', 'btnPatchAttributeSettab', 'Add');    
    EG_ClearTable();
    
    EG_AddBlankRows(5)
    $('#TicketNo').focus();  
    hideLoader();
}

function DeleteClick() {    
   notyConfirm('Are you sure to delete?', 'ReturnBillDelete()'); 
}

function ReturnBillDelete() {    
   try {
        var id = $('#HeaderID').val();
        if (id != '' && id != null) {
            var data = { "ID": id };
            var ds = {};
            ds = GetDataFromServer("ReturnBill/DeleteReturnBill/", data);
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

function ReturnBillDetailDelete(id,rw) {
    try { 
        var Hid = $('#HeaderID').val();
        if (id != '' && id != null && Hid != '' && Hid != null && Hid != emptyGUID) {
            var data = { "ID": id, "HeaderID": Hid };
            var ds = {};
            ds = GetDataFromServer("ReturnBill/DeleteReturnBillDetail/", data);
            if (ds != '') {
                ds = JSON.parse(ds);
            }
            if (ds.Result == "OK") {
                notyAlert('success', ds.Message);
                AmountSummary()
                BindReturnBill(Hid);
            }
            if (ds.Result == "ERROR") {
                notyAlert('error', ds.Message);
                return 0;
            }
            return 1;
        }
        else { 
            if (EG_GridData.length != 1) {
                EG_GridData.splice(rw-1,1); 
                EG_Rebind_WithData(EG_GridData,0);
            }
            else {
                reset();
                EG_Rebind();
            }
            notyAlert('success', 'Deleted Successfully');
            AmountSummary();
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
        notyConfirm('Are you sure to delete?', 'ReturnBillDetailDelete("' + rowData.ID + '",' + rowData[EG_SlColumn] + ')');
    }
}

function RestReturnBill() {
    ClearFields();
    $('#HeaderID').val(emptyGUID);//clear field will make this field model invalid
   // $('#InvNo').removeAttr('readonly')
    var $datepicker = $('#InvDate');
    $datepicker.datepicker('setDate', null);    
}

function Edit(currentObj) {
    showLoader();    
    var rowData = DataTables.returnBillTable.row($(currentObj).parents('tr')).data();       
    if ((rowData != null) && (rowData.ID != null)) {
        EG_ClearTable();       
        $('#AddTab').trigger('click');
        if (BindReturnBill(rowData.ID)) {       
       
        }
        else {
            $('#ListTab').trigger('click');
        }       
       
    }
    hideLoader();   
}


function ReturnFields()
{
    $("#InvNo").prop('disabled', true);
    $("#InvDate").prop('disabled', true);
    $("#Remarks").prop('disabled', true);
    $("#CName").prop('disabled', true);
    $("#CPhoneNo").prop('disabled', true);
    $("#CAddress").prop('disabled', true);
    $("#SName").prop('disabled', true);
    $("#SPhoneNo").prop('disabled', true);
    $("#SAddress").prop('disabled', true);
    $("#CEmail").prop('disabled', true);
    $("#SEmail").prop('disabled', true);
    $("#CGstIn").prop('disabled', true);
    $("#SGstIn").prop('disabled', true);
    $("#CPanNo").prop('disabled', true);
    $("#SPanNo").prop('disabled', true);
    $("#CGstIn").prop('disabled', true);
    $("#Place").prop('disabled', true);
}

function save() {
    debugger;
    var validation = EG_Validate();
    if (validation == "") {
    
        var result = JSON.stringify(EG_GridData);
        var ticketNo = $('#TicketNo').val();
        $("#DetailJSON").val(result);
        $("#savebutton").trigger('click');
    }
    else {
        notyAlert('error', validation);
    }

}

function reset() {

    EG_ClearTable();
    EG_AddBlankRows(5);
    $('#discount').val('');
    $('#Total').val('');
    $('#totaltaxamount').val('');
    $('#grandtotal').val('');
    $('#subtotal').val('');
    $('#CName').val('');
    $('#CAddress').val('');
    $('#CGstIn').val('');
    $('#CPanNo').val('');
    $('#CEmail').val('');
    $('#CPhoneNo').val('');
    $('#SName').val('');
    $('#SAddress').val('');
    $('#SGstIn').val('');
    $('#SPanNo').val('');
    $('#SEmail').val('');
    $('#SPhoneNo').val('');
    $('#Place').val('');
    $('#InvNo').val('');
    $('#InvDate').val('');
    $('#Remarks').val('');
 
}

//-----------------------------------------Reset Validation Messages--------------------------------------//
function ResetForm() {
    debugger;
    var validator = $("#ReturnBill").validate();
    $('ReturnBill').find('.field-validation-error span').each(function () {
        validator.settings.success($(this));
    });
    validator.resetForm();
}

function resetCurrent() {
    try { 
        var id = $('#HeaderID').val();
        BindReturnBill(id); 
    } catch (e) { }
}

function SaveSuccessReturnBill(data, status, xhr) {
    debugger;
    var i = JSON.parse(data)
    switch (i.Result) {
        case "OK":
              notyAlert('success', i.Message);
           
            BindReturnBillFields(i.Records)
            ChangeButtonPatchView('ReturnBill', 'btnPatchAttributeSettab', 'Edit');
           
            break;
        case "Error":
            notyAlert('error', i.Message);
            $('#TicketNo').focus();
            break;
        case "ERROR":
            notyAlert('error', i.Message);
            $('#TicketNo').focus();
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
    debugger;
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
        dic = 0
    }

    EG_GridData[row - 1]['Rate'] = roundoff(rate);
    EG_GridData[row - 1]['BasicAmount'] = roundoff(qty * rate);
    EG_GridData[row - 1]['TradeDiscount'] = roundoff(dic);
    EG_GridData[row - 1]['NetAmount'] = roundoff(qty * rate - dic);
    CalculateCGST(row,true);
    CalculateSGST(row, true);
    EG_Rebind();

    var total = 0.00;
    for (i = 0; i < EG_GridData.length; i++) {
        total = total + (parseFloat(EG_GridData[i]['NetAmount']) || 0);
    }
    $('#subtotal').val(roundoff(total));
    AmountSummary();
    

}
//---------------------Finding CGST Amount---------------------//
function CalculateCGST(row,avoidSummary) {
    var qty = 0.00;
    var rate = 0.00;
    var dic = 0.00;
   

    var EGqty = '';
    var EGrate = '';
    var EGdic = '';
    var EGcgst = '';
  
    EGqty = EG_GridData[row - 1]["Quantity"];
    EGrate = EG_GridData[row - 1]["Rate"];
    EGdic = EG_GridData[row - 1]["TradeDiscount"];
    EGcgst = EG_GridData[row - 1]["CGSTPercentage"];
    

    qty = parseFloat(EGqty) || 0;
    rate = parseFloat(EGrate) || 0;
    dic = parseFloat(EGdic) || 0;
    cgst = parseFloat(EGcgst) || 0;
   

    if (dic > (qty * rate)) {
        dic = (qty * rate);
    }
    else if (dic < 0) {
        dic = 0
    }

    EG_GridData[row - 1]['Rate'] = roundoff(rate);
    EG_GridData[row - 1]['BasicAmount'] = roundoff(qty * rate);
    EG_GridData[row - 1]['TradeDiscount'] = roundoff(dic);
    EG_GridData[row - 1]["CGSTPercentage"] = roundoff(cgst);
    EG_GridData[row - 1]['CGSTAmount'] = roundoff(((qty * rate) - (dic)) * (cgst / 100));

    if (avoidSummary == undefined || avoidSummary == false) {
        EG_Rebind();
        AmountSummary();
    }
}

//--------------------------Finding SGST Amount--------------------//

function CalculateSGST(row, avoidSummary) {
        var qty = 0.00;
        var rate = 0.00;
        var dic = 0.00;
        var sgst = 0.00;

        var EGqty = '';
        var EGrate = '';
        var EGdic = '';
        var EGsgst = '';      
       
        EGqty = EG_GridData[row - 1]["Quantity"];
        EGrate = EG_GridData[row - 1]["Rate"];
        EGdic = EG_GridData[row - 1]["TradeDiscount"];
        EGsgst = EG_GridData[row - 1]["SGSTPercentage"];
      

        qty = parseFloat(EGqty) || 0;
        rate = parseFloat(EGrate) || 0;
        dic = parseFloat(EGdic) || 0;
        sgst = parseFloat(EGsgst) || 0;
        

        if (dic > (qty * rate)) {
            dic = (qty * rate);
        }
        else if (dic < 0) {
            dic = 0
        }

        EG_GridData[row - 1]['Rate'] = roundoff(rate);
        EG_GridData[row - 1]['BasicAmount'] = roundoff(qty * rate);
        EG_GridData[row - 1]['TradeDiscount'] = roundoff(dic);
        EG_GridData[row - 1]["SGSTPercentage"] = roundoff(sgst);
        EG_GridData[row - 1]['SGSTAmount'] = roundoff(((qty * rate) - (dic)) * (sgst / 100));
      
       
        if (avoidSummary == undefined || avoidSummary == false) {
            EG_Rebind();
            AmountSummary();
        }      

    }

//----------------------Finding TotalTaxAmount and  GrandTotal------------------//
function AmountSummary() {
    var total = 0.00;
    var cgstamount = 0.00;   
    var sgstamount = 0.00;  
    var discount = 0.00;
    var quant = 0.00;
    var rate = 0.00;
    var taxtotal = 0.00;
    var t1 = 0.00;
    

    for (i = 0; i < EG_GridData.length; i++) {

        //total = total + (parseFloat(EG_GridData[i]['NetAmount']) || 0);
        quant = (parseFloat(EG_GridData[i]['Quantity']) || 0);
        rate = (parseFloat(EG_GridData[i]['Rate']) || 0);
        discount = (parseFloat(EG_GridData[i]['TradeDiscount']) || 0);
        cgstamount = (parseFloat(EG_GridData[i]['CGSTAmount']) || 0);
        sgstamount = (parseFloat(EG_GridData[i]['SGSTAmount']) || 0);
        t1 = t1 + ((quant * rate) - (discount));// Total calculation
        total = total + ((quant * rate) - (discount) + (cgstamount + sgstamount)); // GrandTotal calculation
        taxtotal = taxtotal + ( cgstamount + sgstamount );  // TotalTaxAmount calculation
    }
    $("#Total").val(roundoff(t1));
    //Math.round($('#grandtotal').val(total));
    $('#grandtotal').val(Math.round(total));
    $('#totaltaxamount').val(roundoff(taxtotal));       
}

var typingFlag = 0;

function FillUOM(row) {

    for (i = 0; i < _Materials.length; i++) {
        if (_Materials[i].ItemCode == EG_GridData[row - 1]['Material']) {
            EG_GridData[row - 1]['UOM'] = _Materials[i].UOM;
            EG_GridData[row - 1]['MaterialID'] = _Materials[i].ID;
            EG_GridData[row - 1]['Description'] = _Materials[i].Description;
            //Description
            EG_Rebind();
            break;
        }
    }

}


//-------------------dropdown action ----------------------

function TicketNoChange(ticket) {
    try {
        var ticket = $('#TicketNo').val();
       
            var data = { "TicketNo": ticket };
            var ds = {};
            ds = GetDataFromServer("ReturnBill/GetMaterialsFromDefectiveDamaged/", data);
            if (ds != '') {     
                ds = JSON.parse(ds);            
            }
            if (ds.Result == "OK") {              
                              
                BindReturnBillFieldsByTicketNo(ds.Records);                                       
                AmountSummary();             
                              
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

function BindReturnBillFieldsByTicketNo(Records) {
    try {
        $('#HeaderID').val(Records[0].ID);
        $('#TicketNo').val(Records[0].TicketNo)       
        $('#SName').val(Records[0].CustomerName);
        $('#Total').val(roundoff(Records[0].TotalValue));
        $('#totaltaxamount').val(roundoff(Records[0].TotalTaxAmount));
        $('#grandtotal').val(roundoff(Records[0].GrandTotal));
        EG_Rebind_WithData(Records, 1);
        var $datepicker = $('#InvDate');
        $datepicker.datepicker('setDate', new Date(Records.InvoiceDateFormatted));
    }
    catch (e) {
        notyAlert('error', e.message);
    }

}

//---------------------Return bill to company-----------------------------------------

function ReturnClick() {
    ChangeButtonPatchView('ReturnBill', 'btnPatchAttributeSettab', 'Return');
    notyConfirm('Are you sure to return?', 'ReturnToCompany()', '', 'Yes,return it!');
    $("#InvNo").attr("disabled", "disabled");
    $("#InvDate").attr("disabled", "disabled");
    $("#CName").attr("disabled", "disabled");
    $("#CAddress").attr("disabled", "disabled");
    $("#CPhoneNo").attr("disabled", "disabled");
    $("#SName").attr("disabled", "disabled");
    $("#SAddress").attr("disabled", "disabled");
    $("#SPhoneNo").attr("disabled", "disabled");
    $("#SEmail").attr("disabled", "disabled");
    $("#CEmail").attr("disabled", "disabled");
    $("#CGstIn").attr("disabled", "disabled");
    $("#SGstIn").attr("disabled", "disabled");
    $("#CPanNo").attr("disabled", "disabled");
    $("#SPanNo").attr("disabled", "disabled");
    $("#Remarks").attr("disabled", "disabled");
    $(".DeleteLink").hide();
}


function ReturnToCompany() {
    try {
        debugger;
        var ReturnID = $("#HeaderID").val();
        var ticket = $("#txtTicketNo").val();
        var data = { "HeaderID": ReturnID , "TicketNo": ticket };       
        var ds = {};
        ds = GetDataFromServer("ReturnBill/ReturnDefectiveDamaged/",data);
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        if (ds.Result == "OK") {
            notyAlert('success', ds.Message);
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

function PrintTableToPdf()
{
    debugger;
    //$("#PrintPreviewModel").modal('show');
    FillPrintForm();
    PrintForm();
}

function FillPrintForm()
{
    debugger;
    DrawTable({
        Action: "ReturnBill/GetReturnBill/",
        data: {"ID":$('#HeaderID').val()  },
        Exclude_column: ["SCCode", "ID", "HeaderID", "MaterialID",
            "TotalTaxAmount", "ReturnStatusYN", "TotalValue",
            "TicketNo", "CustomerName", "GrandTotal", "NetAmount"],
        Header_column_style: {
            "SlNo": {"style":"font-size:11px;border-bottom:1px solid grey;font-weight: 500;","custom_name":"No"},
            "Material": { "style":"font-size:11px;border-bottom:1px solid grey;font-weight: 500;", "custom_name": "Material" },
                "Quantity":  {"style":"width:70px;font-size:11px;border-bottom:1px solid grey;font-weight: 500;"},
                "UOM": { "style": "text-align: left;font-size:11px;border-bottom:1px solid grey;font-weight: 500;" },
                "Description":  {"style":"text-align: left;font-size:11px;border-bottom:1px solid grey;font-weight: 500;"},
                "Rate":  {"style":"text-align: center;font-size:11px;border-bottom:1px solid grey;font-weight: 500;"},
                "BasicAmount": { "style": "text-align: center;font-size:11px;border-bottom:1px solid grey;font-weight: 500;", "custom_name": "Basic Amt" },
                "TradeDiscount": { "style": "text-align: center;font-size:11px;border-bottom:1px solid grey;font-weight: 500;", "custom_name": "Trade Dis" },
                "CGSTPercentage": { "style": "text-align: center;font-size:11px;border-bottom:1px solid grey;font-weight: 500;", "custom_name": "CGST %" },
                "CGSTAmount": { "style": "text-align: center;font-size:11px;border-bottom:1px solid grey;font-weight: 500;", "custom_name": "CGST Amt" },
                "SGSTPercentage": { "style": "text-align: center;font-size:11px;border-bottom:1px solid grey;font-weight: 500;", "custom_name": "SGST %" },
                "SGSTAmount": { "style": "text-align: center;font-size:11px;border-bottom:1px solid grey;font-weight: 500;", "custom_name": "SGST Amt" }
        },
        Row_color: { "Odd": "White", "Even": "white" },
        Body_Column_style: {
            "SlNo": "font-size:9px;font-weight: 100;",
            "Material": "font-size:9px;font-weight: 100;",
            "Quantity": "font-size:9px;font-weight: 100;width:70px;",
            "UOM": "text-align:left;font-size:9px;font-weight: 100;",
            "Description": "text-align: left;font-size:9px;font-weight: 100;",
            "Rate": "text-align:right;font-size:9px;font-weight: 100;",
            "BasicAmount": "text-align:right;font-size:9px;font-weight: 100;",
            "TradeDiscount": "text-align:right;font-size:9px;font-weight: 100;",
            "CGSTPercentage": "text-align:right;font-size:9px;font-weight: 100;",
            "CGSTAmount": "text-align:right;font-size:9px;font-weight: 100;",
            "SGSTPercentage": "text-align:right;font-size:9px;font-weight: 100;",
            "SGSTAmount" : "text-align:right;font-size:9px;font-weight: 100;"
        }

    });
    //var contents = $("#textbox").val();
    // $("#container").val(contents);
    var invoiceno = $('#InvNo').val();
    $("#invoiceNo").text(invoiceno);
    var invoicedate = $("#InvDate").val();
    $("#invoiceDate").text(invoicedate);
    //var customername = $("#CName").val();
    //$("#customerName").text(customername);
    //var customeraddress = $("#CAddress").val().split(/\n/);
    //$("#add1").text(customeraddress);
    //var customeradd1 = $("#CAddress").val().split(/\n/);
    //$("#add2").text(customeradd1);
    var customername1 = $("#SName").val();
    $("#customerName1").text(customername1);
    var customeraddress1 =$("#SAddress").val().split(/\n/);
    $("#address1").text(customeraddress1);
    //var customeraddress2 =$("#CAddress").val().split(/\n/);
    //$("#address2").text(customeraddress2);
    //var mobileno = $("#CPhoneNo").val();
    //$("#customerMobileNo1").text(mobileno);
    var mobileno1 = $("#SPhoneNo").val();
    $("#customerMobileNo2").text(mobileno1);
    //var Email1 = $("#CEmail").val();
    //$("#customerEmail1").text(Email1);
    var Email2 = $("#SEmail").val();
    $("#customerEmail2").text(Email2);
    //var GstIn1 = $("#CGstIn").val();
    //$("#gstin1").text(GstIn1);
    var GstIn2 = $("#SGstIn").val();
    $("#gstin2").text(GstIn2);
    //var GstInNew1 = $("#CGstIn").val();
    //$("#gstinnew1").text(GstInNew1);
    var GstInNew2 = $("#SGstIn").val();
    $("#gstinnew2").text(GstInNew2);
    //var PanNo1 = $("#CPanNo").val();
    //$("#PanNo1").text(PanNo1);
    var PanNo2 = $("#SPanNo").val();
    $("#PanNo2").text(PanNo2);
    //var supplyPlace1 = $("#Place").val();
    //$("#PlaceofSupply1").text(supplyPlace1);
    var supplyPlace2 = $("#Place").val();
    $("#PlaceofSupply2").text(supplyPlace2);
   
    var grandTotal =Math.round( $("#grandtotal").val());
    //var grandTotal =Math.round( $("#grandtotal").val());   
    $("#receivedRupees").text(convertNumberToWords((grandTotal)));
    //Math.round($('#grandtotal').val(total));

    var Total =Math.round( $("#grandtotal").val());
    $("#grandTotal").text(Total);
    debugger;
    var thisItem = GetAllFranchiseeDetail();
    $('#FName').text(thisItem[0].ServiceCenterDescription);
    $('#Address1').text(thisItem[0].ServiceCenterAddress);
    $('#email').text(thisItem[0].ServiceCenterEmail);
    $('#contactNo').text(thisItem[0].ServiceCenterContactNo);
    $('#GSTIN').text(thisItem[0].ServiceCenterGstIn);
    $('#panNo').text(thisItem[0].ServiceCenterPanNo);
    $('#placeOfSupply').text(thisItem[0].ServiceCenterPlace);

    debugger;
    var thisSupplierItem = GetSupplierDetail();
    $('#customerName').text(thisSupplierItem[0].CompanyDescription);
    $('#add1').text(thisSupplierItem[0].CompanyAddress);
    $('#customerEmail1').text(thisSupplierItem[0].CompanyEmail);
    $('#customerMobileNo1').text(thisSupplierItem[0].CompanyContactNo);
    $('#gstinnew1').text(thisSupplierItem[0].CompanyGstIn);
    $('#PanNo1').text(thisSupplierItem[0].CompanyPanNo);
    $('#PlaceofSupply1').text(thisSupplierItem[0].CompanyPlace);

}




function convertNumberToWords(amount) {
    var words = new Array();
    words[0] = '';
    words[1] = 'One';
    words[2] = 'Two';
    words[3] = 'Three';
    words[4] = 'Four';
    words[5] = 'Five';
    words[6] = 'Six';
    words[7] = 'Seven';
    words[8] = 'Eight';
    words[9] = 'Nine';
    words[10] = 'Ten';
    words[11] = 'Eleven';
    words[12] = 'Twelve';
    words[13] = 'Thirteen';
    words[14] = 'Fourteen';
    words[15] = 'Fifteen';
    words[16] = 'Sixteen';
    words[17] = 'Seventeen';
    words[18] = 'Eighteen';
    words[19] = 'Nineteen';
    words[20] = 'Twenty';
    words[30] = 'Thirty';
    words[40] = 'Forty';
    words[50] = 'Fifty';
    words[60] = 'Sixty';
    words[70] = 'Seventy';
    words[80] = 'Eighty';
    words[90] = 'Ninety';
    amount = amount.toString();
    var atemp = amount.split(".");
    var number = atemp[0].split(",").join("");
    var n_length = number.length;
    var words_string = "";
    if (n_length <= 9) {
        var n_array = new Array(0, 0, 0, 0, 0, 0, 0, 0, 0);
        var received_n_array = new Array();
        for (var i = 0; i < n_length; i++) {
            received_n_array[i] = number.substr(i, 1);
        }
        for (var i = 9 - n_length, j = 0; i < 9; i++, j++) {
            n_array[i] = received_n_array[j];
        }
        for (var i = 0, j = 1; i < 9; i++, j++) {
            if (i == 0 || i == 2 || i == 4 || i == 7) {
                if (n_array[i] == 1) {
                    n_array[j] = 10 + parseInt(n_array[j]);
                    n_array[i] = 0;
                }
            }
        }
        value = "";
        for (var i = 0; i < 9; i++) {
            if (i == 0 || i == 2 || i == 4 || i == 7) {
                value = n_array[i] * 10;
            } else {
                value = n_array[i];
            }
            if (value != 0) {
                words_string += words[value] + " ";
            }
            if ((i == 1 && value != 0) || (i == 0 && value != 0 && n_array[i + 1] == 0)) {
                words_string += "Crores ";
            }
            if ((i == 3 && value != 0) || (i == 2 && value != 0 && n_array[i + 1] == 0)) {
                words_string += "Lakhs ";
            }
            if ((i == 5 && value != 0) || (i == 4 && value != 0 && n_array[i + 1] == 0)) {
                words_string += "Thousand ";
            }
            if (i == 6 && value != 0 && (n_array[i + 1] != 0 && n_array[i + 2] != 0)) {
                words_string += "Hundred and ";
            } else if (i == 6 && value != 0) {
                words_string += "Hundred and ";
            }
        }
        words_string = words_string.split("  ").join(" ");
    }
    return words_string;
}


function SameDataAsAbove()
{
    debugger;
    

        var customername = $("#CName").val();
        $("#SName").val(customername);
        var customeraddress = $("#CAddress").val();
        $("#SAddress").val(customeraddress);
        var GstIn1 = $("#CGstIn").val();
        $("#SGstIn").val(GstIn1);
        var PanNo1 = $("#CPanNo").val();
        $("#SPanNo").val(PanNo1);
        var mobileno = $("#CPhoneNo").val();
        $("#SPhoneNo").val(mobileno);
        var Email1 = $("#CEmail").val();
        $("#SEmail").val(Email1);
  

}

function PrintForm()
{
    debugger;
    var BodyContent = $('#divPrintPreview').html();
    var HeaderContent = $('#divHeader').html();
    $('#hdnContent').val(BodyContent);
    $('#hdnHeadContent').val(HeaderContent);
    $('#btnPrintBillToPDF').trigger('click');
}
function SaveSuccessPrint(data, status, xhr) {
    debugger;
    var i = JSON.parse(data)
    switch (i.Result) {
        case "OK":
            var W = window.open(i.URL);
             W.window.print(); 
            break;
        case "ERROR":
            notyAlert('error', "Sorry an error occur, "+i.Message);
            break;
        default:
            break;
    }
}

//function validateForm() {

//    for (i = 0; i < EG_GridData.length; i++) {
//        if (EG_GridData[i]['Material'] != "") {
//            return ""
//        }
//    }

//    return "Minimum one detail is required to save";
//}



//-----------------------------------------------------------------------------EG_MandatoryFields



