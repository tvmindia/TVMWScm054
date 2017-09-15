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
               { "data": "SaleOrderNo", "defaultContent": "<i>-</i>" },
               { "data": "PONo", "defaultContent": "<i>-</i>" },

               { "data": "Subtotal", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
               { "data": "VATAmount", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
               { "data": "Discount", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
             

               { "data": "TotalTaxAmount", render: function (data, type, row) { debugger; return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
               { "data": "GrandTotal", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
                                        
               { "data": "Remarks", "defaultContent": "<i>-</i>" },
               { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink"  onclick="Edit(this)" ><i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>' }
             ],
             columnDefs: [{ "targets": [0], "visible": false, "searchable": false }, { "targets": [1], "visible": false, "searchable": false },{"targets": [6,7,8], "visible": false},
                { className: "text-left", "targets": [2,11] },
                { className: "text-right", "targets": [9,10] },
                { className: "text-center", "targets": [3, 4, 5] },
                  { "width": "15%", "targets": 4 },
                  { "width": "15%", "targets": 9 },
                  { "width": "15%", "targets": 10 },
                  { "width": "15%", "targets": 11 },

                { "orderable": false, "targets": [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11] }
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
                { "data": "CGSTAmount", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i></i>" },
                { "data": "SGSTPercentage", render: function (data, type, row) { return (EG_createTextBox(data, 'F', row, 'SGSTPercentage', 'CalculateSGST')); }, "defaultContent": "<i></i>" },
                { "data": "SGSTAmount", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i></i>" },
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
function GetAllForm8() {
    try {
        debugger;
        var data = {};
        var ds = {};
        ds = GetDataFromServer("Form8TaxInvoice/GetAllForm8/", data);
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

function BindForm8(id) {
    try {
        var data = { "ID": id };
        var ds = {};
        ds = GetDataFromServer("Form8TaxInvoice/GetForm8/", data);
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        if (ds.Result == "OK") {

            BindForm8Fields(ds.Records);
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

function BindForm8Fields(Records) {
    try { 
        $('#HeaderID').val(Records.ID);
        $('#InvNo').val(Records.InvoiceNo);        
        $('#Remarks').val(Records.Remarks);
        $('#CNo').val(Records.ChallanNo);        
        $('#PONo').val(Records.PONo);      
        $('#SONo').val(Records.SaleOrderNo);
        $('#subtotal').val(roundoff(Records.Subtotal));
        $('#vatamount').val(roundoff(Records.VATAmount));
        $('#discount').val(roundoff(Records.Discount));
        $('#totaltaxamount').val(roundoff(Records.TotalTaxAmount));
        $('#grandtotal').val(roundoff(Records.GrandTotal));       
        $('#Total').val(roundoff(Records.GrandTotal - Records.VATAmount));
        EG_Rebind_WithData(Records.Form8Detail,1);
        //$('#InvNo').attr('readonly', 'readonly');
        var $datepicker = $('#InvDate');      
        $datepicker.datepicker('setDate', new Date(Records.InvoiceDateFormatted));
        if (Records.ChallanDateFormatted != null) {
            var $datepicker = $('#CDate');           
            $datepicker.datepicker('setDate', new Date(Records.ChallanDateFormatted));
        } 
        if (Records.PODateFormatted!=null) {
            var $datepicker = $('#PODate');            
            $datepicker.datepicker('setDate', new Date(Records.PODateFormatted));
        } 
    } catch (e) {
        notyAlert('error', e.message);
    } 
}


//--------------------button actions ----------------------
function List() {
    try {
        showLoader();
        ChangeButtonPatchView('Form8TaxInvoice', 'btnPatchAttributeSettab', 'List');
        DataTables.eventTable.clear().rows.add(GetAllForm8()).draw(false);
          hideLoader();
    } catch (x) {
       // alert(x);
    }
}

function Add() {
    try {
        ResetForm()
    }
    catch(X)
    {}
    showLoader();
    ChangeButtonPatchView('Form8TaxInvoice', 'btnPatchAttributeSettab', 'Add');
    EG_ClearTable();
    RestForm8();
    EG_AddBlankRows(5)
    $('#InvNo').focus();
    hideLoader();
}

function DeleteClick() {    
   notyConfirm('Are you sure to delete?', 'Form8Delete()'); 
}

function Form8Delete() {    
   try {
        var id = $('#HeaderID').val();
        if (id != '' && id != null) {
            var data = { "ID": id };
            var ds = {};
            ds = GetDataFromServer("Form8TaxInvoice/DeleteForm8/", data);
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

function Form8DetailDelete(id,rw) {
    try { 
        var Hid = $('#HeaderID').val();
        if (id != '' && id != null && Hid != '' && Hid != null && Hid != emptyGUID) {
            var data = { "ID": id, "HeaderID": Hid };
            var ds = {};
            ds = GetDataFromServer("Form8TaxInvoice/DeleteForm8Detail/", data);
            if (ds != '') {
                ds = JSON.parse(ds);
            }
            if (ds.Result == "OK") {
                notyAlert('success', ds.Message);
                AmountSummary()
                BindForm8(Hid);
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
        notyConfirm('Are you sure to delete?', 'Form8DetailDelete("' + rowData.ID + '",' + rowData[EG_SlColumn] + ')');
    }
}

function RestForm8() {
    ClearFields();
    $('#HeaderID').val(emptyGUID);//clear field will make this field model invalid
   // $('#InvNo').removeAttr('readonly')
    var $datepicker = $('#InvDate');
    $datepicker.datepicker('setDate', null);
    var $datepicker = $('#CDate');
    $datepicker.datepicker('setDate', null);
    var $datepicker = $('#PODate');
    $datepicker.datepicker('setDate', null);
}

function Edit(currentObj) {
    showLoader();
    var rowData = DataTables.eventTable.row($(currentObj).parents('tr')).data();
    //Event Request Case
    if ((rowData != null) && (rowData.ID != null)) {

        EG_ClearTable();
        $('#AddTab').trigger('click');
        if (BindForm8(rowData.ID)) {
            ChangeButtonPatchView('Form8TaxInvoice', 'btnPatchAttributeSettab', 'Edit');            
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
    EG_AddBlankRows(5);
    $('#discount').val('');
    $('#Total').val('');
    $('#totaltaxamount').val('');
    $('#grandtotal').val('');
    $('#subtotal').val('');
}

//-----------------------------------------Reset Validation Messages--------------------------------------//
function ResetForm() { 
    var validator = $("#F8").validate();
    $('#F8').find('.field-validation-error span').each(function () {
        validator.settings.success($(this));
    });
    validator.resetForm();
}

function resetCurrent() {
    try { 
        var id = $('#HeaderID').val();
        BindForm8(id); 
    } catch (e) { }
}

function SaveSuccess(data, status, xhr) {
    var i = JSON.parse(data)
    switch (i.Result) {
        case "OK":
              notyAlert('success', i.Message);
           
            BindForm8Fields(i.Records)
            ChangeButtonPatchView('Form8TaxInvoice', 'btnPatchAttributeSettab', 'Edit');
           
            break;
        case "Error":
            notyAlert('error', i.Message);
            $('#InvNo').focus();
            break;
        case "ERROR":
            notyAlert('error', i.Message);
            $('#InvNo').focus();
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
    debugger; 
    var total = 0.00;
    var cgstamount = 0.00;   
    var sgstamount = 0.00;  
    var discount = 0.00;
    var quant = 0.00;
    var rate = 0.00;
    var taxtotal = 0.00;
    

    for (i = 0; i < EG_GridData.length; i++) {

        //total = total + (parseFloat(EG_GridData[i]['NetAmount']) || 0);
        debugger;
        quant = (parseFloat(EG_GridData[i]['Quantity']) || 0);
        rate = (parseFloat(EG_GridData[i]['Rate']) || 0);
        discount = (parseFloat(EG_GridData[i]['TradeDiscount']) || 0);
        cgstamount = (parseFloat(EG_GridData[i]['CGSTAmount']) || 0);
        sgstamount = (parseFloat(EG_GridData[i]['SGSTAmount']) || 0);
        total = total + ((quant * rate) - (discount) + (cgstamount + sgstamount)); // GrandTotal calculation
        taxtotal = taxtotal + ( cgstamount + sgstamount );  // TotalTaxAmount calculation
    }
    $('#grandtotal').val(roundoff(total));
    $('#totaltaxamount').val(roundoff(taxtotal));

    //$('#subtotal').val(roundoff(total));  
    //var subtotal = parseFloat($('#subtotal').val()) || 0;
    //var vatamount = parseFloat($('#vatamount').val()) || 0;
   // var cgstamount = parseFloat($('#totaltaxamount').val()) || 0;
   // var sgstamount = parseFloat($('#totaltaxamount').val()) || 0;
   // var discount = parseFloat($('#discount').val()) || 0;
       
    //var total = subtotal - discount;
    //$('#Total').val(roundoff(total));      
   
        //var vatp = (parseFloat($('#vatpercentage').val()) || 0);
   // if (vatp > 0) {
      //  vatamount = (total * vatp) / 100;
      //  $('#vatamount').val(roundoff(vatamount));
   // }

    //$('#grandtotal').val(roundoff(total + cgstamount + sgstamount));
}

var typingFlag = 0;
//function calculateVat() {
//    debugger;
//    if (typingFlag == 0) {
//        setTimeout(calculateVatPercentage, 2000);//done to wait till typing over
//        typingFlag = 1;
//    }
//}

function ClearVatPercent() {
   debugger;
   if ($('#vatamount').val()!= $('#VatAmountValue').val())
   $('#vatpercentage').val('');
   var total = parseFloat($('#Total').val()) || 0;
   var vatamount = parseFloat($('#vatamount').val()||0);
    var GTotal = total + vatamount;
    $('#grandtotal').val(roundoff(GTotal));
}

function calculateVatPercentage() {
    debugger;
   var vatp = parseFloat($('#vatpercentage').val()) || 0;
  var total = parseFloat($('#Total').val()) || 0;
   if (vatp > 100) {
        vatp = 100;
        $('#vatpercentage').val(vatp);
    }
    if (vatp < 0) {
        vatp = 0;
        $('#vatpercentage').val(vatp);
    }

  
    $('#vatamount').val(roundoff(total * vatp / 100));
    $('#VatAmountValue').val($('#vatamount').val());
    AmountSummary();
    typingFlag = 0;
}

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



//function validateForm() {

//    for (i = 0; i < EG_GridData.length; i++) {
//        if (EG_GridData[i]['Material'] != "") {
//            return ""
//        }
//    }

//    return "Minimum one detail is required to save";
//}



//-----------------------------------------------------------------------------EG_MandatoryFields



