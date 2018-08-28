var DataTables = {};
var emptyGUID = '00000000-0000-0000-0000-000000000000'
var _Materials = [];
//---------------------------------------Docuement Ready--------------------------------------------------//

$(document).ready(function () {
    try {


        $('[data-toggle="popover"]').popover();
        //$(document).on("click", ".popover", function () {
        //    $(this).popover('hide');
        //});

        DataTables.customerTaxBillsTable = $('#tblCustomerTaxBills').DataTable(
        {
            dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
            buttons: [{
                extend: 'excel',
                exportOptions:
                             {
                                 columns: [1, 3, 4, 5, 6, 7,8,9,10,11,12]
                             }
            }],
            order: [],
            searching: true,
            paging: true,
            data: GetAllTaxBill(),
            columns: [
              { "data": "ID", "defaultContent": "<i>-</i>" },
              { "data": "Technician", "defaultContent": "<i>-</i>" },
              { "data": "JobNo", "defaultContent": "<i>-</i>" },
              { "data": "BillDateFormatted", "defaultContent": "<i>-</i>" },
              { "data": "BillNo", "defaultContent": "<i>-</i>" },
              { "data": "Subtotal", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
              { "data": "Discount", "defaultContent": "<i>-</i>" },
              { "data": "TotalTaxAmount", "defaultContent": "<i>-</i>" },
              { "data": "GrandTotal", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
              { "data": "CustomerName", "defaultContent": "<i>-</i>" },
              { "data": "CustomerContactNo", "defaultContent": "<i>-</i>" },
              { "data": "CustomerLocation", "defaultContent": "<i>-</i>" },
              
              { "data": "Remarks", "defaultContent": "<i>-</i>" },
              { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink"  onclick="Edit(this)" ><i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>' }
            ],
            columnDefs: [{ "targets": [0], "visible": false, "searchable": false }, { "targets": [2], "visible": false, "searchable": false },
                 { className: "text-right", "targets": [5,6,7,8] },
            { className: "text-center", "targets": [2, 3, 4, 5, 6, 7, 8, 10, 13] },
            { className: "text-left", "targets": [1,9,11,12] },
            { "width": "8%", "targets": 4 },
            { "width": "8%", "targets": 5 },
              { "width": "8%", "targets": 6 },
            { "width": "10%", "targets": 7 },
             { "width": "8%", "targets": 3 },
              { "width": "9%", "targets": 8 },
              { "width": "9%", "targets": 9 },
            ]
        });       
        //BindJobNumberDropDown();
      
        DataTables.TaxBillDetail = $('#tblTaxBillDetails').DataTable(
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
        
        debugger;
        $('#tblCustomerTaxBills tbody').on('dblclick', 'td', function () {
            debugger;
            Edit(this);
        });

        getMaterials();
        EG_ComboSource('Materials', _Materials, 'ItemCode', 'Description')
        EG_GridDataTable = DataTables.TaxBillDetail;
        List();      
        var $datepicker = $('#BillDate');
        $datepicker.datepicker('setDate', null);
        $(".buttons-excel").hide();
        $("#btnDownloadBillToPDF").hide();
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
    tempObj.Description = "";
    tempObj.Quantity = "";
    tempObj.UOM = "";
    tempObj.Rate = "";
    tempObj.TradeDiscount = "";
    tempObj.CgstPercentage = "";
    tempObj.CGSTAmount = "";
    tempObj.SgstTPercentage = "";
    tempObj.SGSTAmount = "";
    tempObj.NetAmount = "";

    return tempObj
}
function EG_Columns() {

    var obj = [

                { "data": "ID", "defaultContent": "<i>0</i>" },
                { "data": "MaterialID", "defaultContent": "<i></i>" },
                { "data": "SlNo", "defaultContent": "<i></i>" },
                { "data": "Material"},// render: function (data, type, row) { return (EG_createCombo(data, 'S', row, 'Material', 'Materials', 'FillUOM')); } },
                { "data": "Description", "defaultContent": "<i></i>" },
                { "data": "Quantity"},//, render: function (data, type, row) { return (EG_createTextBox(data, 'N', row, 'Quantity', 'CalculateAmount')); }, "defaultContent": "<i></i>" },
                { "data": "UOM", "defaultContent": "<i></i>" },
                {"data":"ReferralRate","defaultContent": "<i></i>" },
                { "data": "Rate", render: function (data, type, row) { return (EG_createTextBox(data, 'F', row, 'Rate', 'CalculateAmount',true)); }, "defaultContent": "<i></i>" },
                { "data": "TradeDiscount", render: function (data, type, row) { return (EG_createTextBox(data, 'F', row, 'TradeDiscount', 'CalculateAmount',true)); }, "defaultContent": "<i></i>" },
                { "data": "CgstPercentage", render: function (data, type, row) { return (EG_createTextBox(data, 'F', row, 'CgstPercentage', 'CalculateCGST',true)); }, "defaultContent": "<i></i>" },
                {
                    "data": "CGSTAmount", render: function (data, type, row) {
                       
                        //var CGST = parseFloat(row.CgstPercentage != "" ? row.CgstPercentage : 0);
                        //var Rate = parseFloat(row.Rate != "" ? row.Rate : 0);
                        //var Qty = parseFloat(row.Quantity != "" ? row.Quantity : 0);
                        //var Disc = parseFloat(row.TradeDiscount != "" ? row.TradeDiscount : 0);
                        //var Taxable = parseFloat((Qty * Rate) - Disc);
                        //var CGSTAmt = parseFloat(Taxable * CGST / 100);
                        //if ((row.CGSTAmount == null) || (row.CGSTAmount == ""))
                        //    return roundoff(parseFloat(CGSTAmt))
                        //else 
                            return roundoff(data, 1);
                        
                    }, "defaultContent": "<i></i>"
                },
                { "data": "SgstPercentage", render: function (data, type, row) { return (EG_createTextBox(data, 'F', row, 'SgstPercentage', 'CalculateSGST',true)); }, "defaultContent": "<i></i>" },
                {
                    "data": "SGSTAmount", render: function (data, type, row) {

                        //var SGST = parseFloat(row.SgstPercentage != "" ? row.SgstPercentage : 0);
                        //var Rate = parseFloat(row.Rate != "" ? row.Rate : 0);
                        //var Qty = parseFloat(row.Quantity != "" ? row.Quantity : 0);
                        //var Disc = parseFloat(row.TradeDiscount != "" ? row.TradeDiscount : 0);
                        //var Taxable = parseFloat((Qty * Rate) - Disc);
                        //var SGSTAmt = parseFloat(Taxable * SGST / 100);
                        //if ((row.SGSTAmount == null) || (row.SGSTAmount == ""))
                        //    return roundoff(parseFloat(SGSTAmt))
                        //else
                           return roundoff(data, 1);
                        s
                       
                    }, "defaultContent": "<i></i>"
                },

                { "data": "NetAmount", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i></i>" },
                { "data": null, "orderable": false, "defaultContent": '<a href="#" class="DeleteLink"  onclick="DeleteItem(this)" ><i class="glyphicon glyphicon-trash" aria-hidden="true"></i></a>' }

               
    ]
  
 
    return obj
    
  
}
function EG_Columns_Settings() {

    var obj = [
        { "targets": [0], "visible": false, "searchable": false }, { "targets": [1], "visible": false, "searchable": false },
         { "width": "5%", "targets": 2 },
         { "width": "15%", "targets": 3 },
        { "width": "15%", "targets": 4 },
         { "width": "6%", "targets": 5 },
        { "width": "8%", "targets": 6 },
         { "width": "8%", "targets": 7 },
          { "width": "8%", "targets": 8 },
          { "width": "8%", "targets": 9 },
            { "width": "8%", "targets": 10 },
          { "width": "8%", "targets": 11 },
          { "width": "8%", "targets": 12 },
          { "width": "8%", "targets": 13 },
          { "width": "18%", "targets": 14 },
            { "width": "5%", "targets": 15 },
        { className: "text-right", "targets": [8] },
        { className: "text-center", "targets": [9, 10, 12] },
        { className: "text-right disabled", "targets": [14,9] },
        { className: "text-center disabled", "targets": [2,5,6,15,7,11,13] },
         { className: "text-left disabled", "targets": [3,4] },
        { "orderable": false, "targets": [0, 1, 2, 3, 4, 5, 6, 7, 8,9,10,11,12,13,14,15] },
    
    ]

    return obj;

}
//------------------------------------------------------------------

//--------------------button actions ----------------------
function List() {
    try {
        
        ChangeButtonPatchView('TaxBillEntry', 'btnPatchTaxBillEntrySettab', 'List');
        $("#HeaderID").val("");
        reset();
      

     BindAllCustomerBill();
    } catch (x) {
        // alert(x);
    }

}
function goBack() {
    $('#AddTab').trigger('click');
    $("#HeaderID").val("");
    reset();
}

function Add()
{   
    debugger;
    ChangeButtonPatchView('TaxBillEntry', 'btnPatchTaxBillEntrySettab', 'Add');
    EG_ClearTable();
    EG_AddBlankRows(0);
    $("#HeaderID").val("");    
       reset();
   
}

//function Add() {
//    debugger;
//    ChangeButtonPatchView('TaxBillEntry', 'btnPatchTaxBillEntrySettab', 'Add');
//    EG_ClearTable();   
//    EG_AddBlankRows(5)
//    $("#HeaderID").val("");
//    //reset();
//}

function reset()
{
    if (($("#HeaderID").val() == "") || ($("#HeaderID").val() == 'undefined') || ($("#HeaderID").val() == "0")) {
        // $("#HeaderID").val(emptyGUID);
        $("#EmpID").val("");
        $("#ModelTechEmpID").val("");
        $("#JobNo").val("");
      
        $("#BillDate").val("");
        $("#BillNo").val("");
        $("#CustomerName").val("");
        $("#PaymentRefNo").val("");
        $("#CustomerContactNo").val("");
        $("#CustomerLocation").val("");
        $("#PaymentMode").val("");
        $("#Remarks").val("");
        $("#subtotal").val("");
        $("#discount").val("");       
        $("#total").val("");
        
        $("#SCAmount").val("");
        $("#VATAmount").val("");
        $("#VATPercentageAmount").val("");
        $("#CGSTAmount").val("");       
        $("#SGSTAmount").val("");        
        $("#grandtotal").val("");
        $("#ServiceChargeComm").val("");
        $("#SCCommAmount").val("");
        $("#SpecialComm").val("");       
        $("#SGSTAmount").val("");
        $("#CGSTAmount").val("");
        ResetTaxForm();
    }
    else {
        BindTaxBillEntry($("#HeaderID").val());
    }

}

function ResetTaxForm() {

    var validator = $("#TaxBill").validate();
    $('#TaxBill').find('.field-validation-error span').each(function () {
        validator.settings.success($(this));
    });
    validator.resetForm();
}

function FillUOM(row) {

    for (i = 0; i < _Materials.length; i++) {
        if (_Materials[i].ItemCode == EG_GridData[row - 1]['Material']) {
            EG_GridData[row - 1]['UOM'] = _Materials[i].UOM;
            EG_GridData[row - 1]['MaterialID'] = _Materials[i].ID;
            EG_GridData[row - 1]['Description'] = _Materials[i].Description;
            EG_GridData[row - 1]['Rate'] = _Materials[i].SellingRate;
            EG_GridData[row - 1]['ReferralRate'] = _Materials[i].SellingRate;
           
            //----for calculating amount on changing item(if already quantity exists)
         //  CalculateAmount(row)
            EG_Rebind();
            //----------------------------------------------------------------
            break;
        }
    }
}
//---------------------------------------Edit Item--------------------------------------------------//
function Edit(currentObj) {
    debugger;
    var rowData = DataTables.customerTaxBillsTable.row($(currentObj).parents('tr')).data();
    //Event Request Case
    if ((rowData != null) && (rowData.ID != null))
    {
        EG_ClearTable();
        debugger;
        $('#EditTab').trigger('click');
        $("#HeaderID").val(rowData.ID);
        if (BindTaxBillEntry(rowData.ID))
        {
            ChangeButtonPatchView('TaxBillEntry', 'btnPatchTaxBillEntrySettab', 'Edit');
            DisableFields();
            debugger;
          AmountSummary();
        }
        else
        {
            $('#ListTab').trigger('click');
        }
     
    }
}

//---------------get grid fill result-------------------
function GetAllTaxBill() {
    try {

        var data = {};
        var ds = {};
        ds = GetDataFromServer("TaxBillEntry/GetAllTaxBillEntry/", data);

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

function getMaterials()
{
    try
    {
        var filter = 1;
        var data = { "filter": filter };
        var ds = {};
        ds = GetDataFromServer("Item/ItemsForDropdown/", data);
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        if (ds.Result == "OK")
        {
            _Materials = ds.Records;
        }
        if (ds.Result == "ERROR")
        {
            alert(ds.Message);
        }
    }
    catch (e)
    {
        notyAlert('error', e.message);
    }
}

function getMaterialsByTechnician(empID)
{
    try
    {
        var data = { "empID": empID };
        var ds = {};
        ds = GetDataFromServer("Item/ItemsForDropdownByTechnician/", data);
        if (ds != '')
        {
            ds = JSON.parse(ds);
        }
        if (ds.Result == "OK")
        {
            _Materials = [];
            _Materials = ds.Records;
            EG_ComboSource('Materials', _Materials, 'ItemCode', 'Description')
        }
        if (ds.Result == "ERROR")
        {
            alert(ds.Message);
        }
    }
    catch (e)
    {
        notyAlert('error', e.message);
    }
}


function BindAllCustomerBill() {
    try {

        DataTables.customerTaxBillsTable.clear().rows.add(GetAllTaxBill()).draw(false);
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}


function BindTaxBillEntry(id) {

    try {
        debugger;
        var data = { "ID": id };
        var ds = {};
        ds = GetDataFromServer("TaxBillEntry/GetTaxBillHeaderByID/", data);
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        if (ds.Result == "OK") {

            BindTaxBillEntryFields(ds.Records);

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

function BindTaxBillEntryFields(Records) {
    try {
        debugger;
        ChangeButtonPatchView('TaxBillEntry', 'btnPatchTaxBillEntrySettab', 'Edit');
        $('#HeaderID').val(Records.ID);
        $("#EmpID").val(Records.EmpID);
        $("#ModelTechEmpID").val(Records.EmpID);
        $("#JobNo").val(Records.JobNo);
        debugger;
        $("#BillDate").val(Records.BillDateFormatted);
        $("#BillNo").val(Records.BillNo);
        $("#CustomerName").val(Records.CustomerName);
        $("#PaymentRefNo").val(Records.PaymentRefNo);
        $("#CustomerContactNo").val(Records.CustomerContactNo);
        $("#CustomerLocation").val(Records.CustomerLocation);
        $("#PaymentMode").val(Records.PaymentMode);
        $("#Remarks").val(Records.Remarks);
        $("#subtotal").val(roundoff(Records.Subtotal));
        $("#discount").val(roundoff(Records.Discount));
        // $("#total").val(roundoff(Records.Subtotal - Records.Discount));
        $("#total").val(roundoff(Records.TotalAmount));
        
        $("#SCAmount").val(roundoff(Records.ServiceCharge));
        $("#VATAmount").val(roundoff(Records.VATAmount));
        $("#VATPercentageAmount").val(Records.VATAmount);


        $("#CGSTAmount").val(roundoff(Records.CGSTAmount));
      //  $("#CGSTPercentageAmount").val(Records.CGSTAmount);
        $("#SGSTAmount").val(roundoff(Records.SGSTAmount));
      /// $("#SGSTPercentageAmount").val(Records.SGSTAmount);

        $("#grandtotal").val(roundoff(Records.GrandTotal));
        $("#ServiceChargeComm").val(roundoff(Records.ServiceCharge / 100));
        $("#SCCommAmount").val(roundoff(Records.SCCommAmount));
        $("#SpecialComm").val(roundoff(Records.SpecialComm));
      //  $("#cgstpercentage").val(roundoff(Records.CgstPercentage));
       // $("#sgstpercentage").val(roundoff(Records.SgstPercentage));
        $("#SGSTAmount").val(roundoff(Records.SGSTAmount));
        $("#CGSTAmount").val(roundoff(Records.CGSTAmount));
        debugger;
        EG_Rebind_WithData(Records.TaxBillEntryDetail);
       
        
        //$('#tblTaxBillDetails input.gridTextbox').attr('onblur', 'CalculateAmount(this)');
    }
    catch (e)
    {
        notyAlert('error', e.message);
    }
}

function DisableFields()
{
    $('#HeaderID').prop('readonly', true);
    $("#EmpID").prop('disabled', true);
    $("#ModelTechEmpID").prop('readonly', true);
    $("#JobNo").prop('readonly', true);
    $("#BillDate").prop('readonly', true);
    $("#BillNo").prop('readonly', true);
    $("#CustomerName").prop('readonly', true);
    $("#PaymentRefNo").prop('readonly', true);
    $("#CustomerContactNo").prop('readonly', true);
    $("#CustomerLocation").prop('readonly', true);
    $("#PaymentMode").prop('disabled', true);
    $("#Remarks").prop('readonly', true);
    $("#subtotal").prop('readonly', true);
    $("#discount").prop('readonly', false);
    $("#total").prop('readonly', true);
    $("#SCAmount").prop('readonly', true);
    $("#VATAmount").prop('readonly', true);
    $("#CGSTAmount").prop('readonly', true);
    $("#SGSTAmount").prop('readonly', true);
    $("#vatpercentage").prop('readonly', true);
    $("#VATPercentageAmount").prop('readonly', true);
    $("#cgstpercentage").prop('readonly', true);
    $("#CGSTPercentageAmount").prop('readonly', true);
    $("#sgstpercentage").prop('readonly', true);
    $("#SGSTPercentageAmount").prop('readonly', true);
    $("#grandtotal").prop('readonly', true);
    $("#ServiceChargeComm").prop('readonly', true);
    $("#SCCommAmount").prop('readonly', true);
    $("#SpecialComm").prop('readonly', true);
    $("#discount").prop('readonly', true);
}


function save()
{
    debugger;
    $("#ID").val(emptyGUID);  
    $("#hdnIsActive").val('1');   
    var result = JSON.stringify(EG_GridData);
    $("#DetailJSON").val(result);
    $("#btnSave").trigger('click');    
}

function SaveSuccess(data, status) {
        var JsonResult = JSON.parse(data)
        switch (JsonResult.Result) {
            case "OK":
                if ($("#HeaderID").val() == emptyGUID || $("#HeaderID").val() == "") {
                    BindTaxBillEntry(JsonResult.Records.ID); 
                    
                }
                else {
                    BindTaxBillEntry($("#HeaderID").val());
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

function CalculateAmount(row)  
{
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
       // EGdic = EG_GridData[row - 1]['Discount'];
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
     //   EG_GridData[row - 1]['NetAmount'] = roundoff(qty * rate);
       // EG_GridData[row - 1]['Discount'] = roundoff(dic);
        EG_GridData[row - 1]['TradeDiscount'] = roundoff(dic);
        EG_GridData[row - 1]['NetAmount'] = roundoff(qty * rate - dic);
        CalculateCGST(row, true);
        CalculateSGST(row, true);
        EG_Rebind();
    
    ///**************//
        //var total = 0.00;
        //for (i = 0; i < EG_GridData.length; i++) {
        //    total = total + (parseFloat(EG_GridData[i]['NetAmount']) || 0);
        //}
        //$('#subtotal').val(roundoff(total));

        AmountSummary();
}

//--------------------Finding CGST Amount---------------------//
   function CalculateCGST(row, avoidSummary) {    
       debugger;
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
       EGcgst = EG_GridData[row - 1]["CgstPercentage"];

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
     //  EG_GridData[row - 1]['NetAmount'] = roundoff(qty * rate - dic);
       //EG_GridData[row - 1]['NetAmount'] = roundoff(qty * rate);
       EG_GridData[row - 1]['TradeDiscount'] = roundoff(dic);
       EG_GridData[row - 1]["CgstPercentage"] = roundoff(cgst);
       EG_GridData[row - 1]['CGSTAmount'] = roundoff(((qty * rate) - (dic)) * (cgst / 100));
       EG_GridData[row - 1]['SgstPercentage'] = roundoff(cgst);
       EG_GridData[row - 1]['SGSTAmount'] = roundoff(((qty * rate) - (dic)) * (cgst / 100));

       if (avoidSummary == undefined || avoidSummary == false) {
           EG_Rebind();
           AmountSummary();
       }
   }

//--------------------------Finding SGST Amount--------------------//

function CalculateSGST(row, avoidSummary) {
    // function CalculateSGST() {

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
    EGsgst = EG_GridData[row - 1]["SgstPercentage"];


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
    // EG_GridData[row - 1]['NetAmount'] = roundoff(qty * rate);
    //EG_GridData[row - 1]['NetAmount'] = roundoff(qty * rate - dic);
    EG_GridData[row - 1]['TradeDiscount'] = roundoff(dic);
    EG_GridData[row - 1]["SgstPercentage"] = roundoff(sgst);
    EG_GridData[row - 1]['SGSTAmount'] = roundoff(((qty * rate) - (dic)) * (sgst / 100));


    if (avoidSummary == undefined || avoidSummary == false) {
        EG_Rebind();
        AmountSummary();
    }


}

////----------------------Finding TotalTaxAmount and  GrandTotal------------------//
function AmountSummary() {
    debugger;
    var t1 = 0.00;
    var t2 = 0.00;
    var tot = 0.00;
    var total = 0.00;
    var cgstamount = 0.00;
    var sgstamount = 0.00;
    var discount = 0.00;
    var quant = 0.00;
    var rate = 0.00;
    var taxtotal = 0.00;
    var netamount = 0.00;
    var subTot = 0.00;
    var discAmount = 0.00
    var disPercent = 0.00;
    var serviceamount = 0.00;
    var total1 = 0.00;
   var disc = 0.00;
    serviceamount = parseFloat($("#SCAmount").val());

    for (i = 0; i < EG_GridData.length; i++) {

        //total = total + (parseFloat(EG_GridData[i]['NetAmount']) || 0);
        debugger;

        quant = (parseFloat(EG_GridData[i]['Quantity']) || 0);
        rate = (parseFloat(EG_GridData[i]['Rate']) || 0);
        discount =(parseFloat(EG_GridData[i]['TradeDiscount']) || 0);
        disc =disc+(parseFloat(EG_GridData[i]['TradeDiscount']) || 0);
        cgstamount = cgstamount+(parseFloat(EG_GridData[i]['CGSTAmount']) || 0);
        sgstamount = sgstamount + (parseFloat(EG_GridData[i]['SGSTAmount']) || 0);
        netamount = netamount + (parseFloat(EG_GridData[i]['NetAmount']) || 0);
     
        t1 = t1 + ((quant * rate));
        //tot = tot + ((quant * rate) - (discount));
        //total = total + ((quant * rate) - (discount) + (cgstamount + sgstamount)); // GrandTotal calculation
        //taxtotal = taxtotal + (cgstamount + sgstamount);  // TotalTaxAmount calculation

        tot = tot + ((quant * rate) - (discount));
        //total = total + ((quant * rate) - (discount) + (cgstamount + sgstamount)); // GrandTotal calculation
        total =tot+ (cgstamount + sgstamount); // GrandTotal calculation
        taxtotal = taxtotal + (cgstamount + sgstamount);  // TotalTaxAmount calculation      
           
        discAmount = disc;       
    }
    
    total1 = total + serviceamount;

    $('#total').val(roundoff(tot));
    $('#grandtotal').val(roundoff(total1));
    $('#totaltaxamount').val(roundoff(taxtotal));
    $('#subtotal').val(roundoff(t1));
    $('#discount').val(roundoff(discAmount));
    $('#CGSTAmount').val(roundoff(cgstamount));
    $('#SGSTAmount').val(roundoff(sgstamount));




    //$('#total').val(roundoff(t2));
    //$('#grandtotal').val(roundoff(total1));
    //$('#totaltaxamount').val(roundoff(taxtotal));
    //$('#subtotal').val(roundoff(tot));
    //$('#discount').val(roundoff(discAmount));
    //$('#CGSTAmount').val(roundoff(cgstamount));
    //$('#SGSTAmount').val(roundoff(sgstamount));

        /**********************/
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





/** CGSTAmount and SGSTAmount old calculation  **/



    //function AmountSummary() {
    //    //debugger;
    //    var Total = 0.00;
    //    for (i = 0; i < EG_GridData.length; i++) {
    //        Total = Total + (parseFloat(EG_GridData[i]['NetAmount']) || 0);
    //    }
    //    $('#subtotal').val(roundoff(Total));
    //    var discount = parseFloat($('#discount').val()) || 0;

    //    $('#total').val(roundoff(Total - discount));

    //    var total = parseFloat($('#total').val()) || 0;

    //    if ($("#vatpercentage").val() != "")
    //        CalculateVAT();

    //    var vatamount = parseFloat($('#VATAmount').val()) || 0;
    //    var SCAmount = parseFloat($('#SCAmount').val()) || 0;
    //    $('#grandtotal').val(roundoff(total + vatamount + SCAmount));
    //}





    //function AmountSummary() {
    //    //debugger;
    //    var Total = 0.00;
    //    for (i = 0; i < EG_GridData.length; i++) {
    //        Total = Total + (parseFloat(EG_GridData[i]['NetAmount']) || 0);
    //    }
    //    $('#subtotal').val(roundoff(Total));
    //    var discount = parseFloat($('#discount').val()) || 0;

    //    $('#total').val(roundoff(Total - discount));

    //    var total = parseFloat($('#total').val()) || 0;

    //    if (($("#cgstpercentage").val() != "") && ($("#sgstpercentage").val() != ""))
    //    {
    //        //CalculateCGST();
    //        //CalculateSGST();
    //        CalculateGSTOld();
    //    }
    //    //if (($("#cgstpercentage").val() != ""))
    //    //{ 
    //    //    CalculateCGST();
    //    //}
    //    //if(($("#sgstpercentage").val() != "")){
    //    //    CalculateSGST();
    //    //}

    //    var vatamount = parseFloat($('#VATAmount').val()) || 0;
    //    var cgstamount = parseFloat($('#CGSTAmount').val()) || 0;
    //    var sgstamount = parseFloat($('#SGSTAmount').val()) || 0;
    //    var SCAmount = parseFloat($('#SCAmount').val()) || 0;
    //    $('#grandtotal').val(roundoff(total + (cgstamount+sgstamount) + SCAmount));
    //}

    //function CalculateVAT() {
    //   // debugger;
    //    var vatpercent = $("#vatpercentage").val();
    //    var Total = $("#total").val();
    //    vatpercent = parseFloat(vatpercent);
    //    if (vatpercent > 100) {
    //        vatpercent = 100
    //        $("#vatpercentage").val(vatpercent);
    //    }
    //    if (vatpercent < 0) {
    //        vatpercent = 0
    //        $("#vatpercentage").val(vatpercent);
    //    }
    //    Total = parseFloat(Total);
    //    var vatamt = (Total * vatpercent / 100)
    //    if (isNaN(vatamt))
    //    { vatamt = 0.00 }
    //    $("#VATAmount").val(roundoff(vatamt));
    //    $('#VATPercentageAmount').val(roundoff(vatamt));
    //    $('#grandtotal').val(roundoff(Total + vatamt));
//}


    //function CalculateCGST() {
    //    // debugger;
    //    var cgstpercent = $("#cgstpercentage").val();
    //    var Total = $("#total").val();
    //    cgstpercent = parseFloat(cgstpercent);
    //    if (cgstpercent > 100) {
    //        cgstpercent = 100
    //        $("#cgstpercentage").val(cgstpercent);
    //    }
    //    if (cgstpercent < 0) {
    //        cgstpercent = 0
    //        $("#cgstpercentage").val(cgstpercent);
    //    }
    //    Total = parseFloat(Total);
    //    var cgstamt = (Total * cgstpercent / 100)
    //    if (isNaN(cgstamt))
    //    { cgstamt = 0.00 }
    //    $("#CGSTAmount").val(roundoff(cgstamt));
    //    $('#CGSTPercentageAmount').val(roundoff(cgstamt));
    //    $('#grandtotal').val(roundoff(Total + cgstamt));
    //}

    //function CalculateGSTOld() {
    //    // debugger;
    //    var cgstpercent = $("#cgstpercentage").val();
    //    var sgstpercent = $("#sgstpercentage").val();
    //    var Total = $("#total").val();
    //    cgstpercent = parseFloat(cgstpercent);
    //    if (cgstpercent > 100) {
    //        cgstpercent = 100
    //        $("#cgstpercentage").val(cgstpercent);
    //    }
    //    if (cgstpercent < 0) {
    //        cgstpercent = 0
    //        $("#cgstpercentage").val(cgstpercent);
    //    }

    //    sgstpercent = parseFloat(sgstpercent);
    //    if (sgstpercent > 100) {
    //        sgstpercent = 100
    //        $("#sgstpercentage").val(sgstpercent);
    //    }
    //    if (sgstpercent < 0) {
    //        sgstpercent = 0
    //        $("#sgstpercentage").val(sgstpercent);
    //    }
    //    Total = parseFloat(Total);
    //    var cgstamt = (Total * cgstpercent / 100)
    //    if (isNaN(cgstamt))
    //    { cgstamt = 0.00 }
    //    $("#CGSTAmount").val(roundoff(cgstamt));
    //    $('#CGSTPercentageAmount').val(roundoff(cgstamt));      
    //    var sgstamt = (Total * sgstpercent / 100)
    //    if (isNaN(sgstamt))
    //    { sgstamt = 0.00 }
    //    $("#SGSTAmount").val(roundoff(sgstamt));
    //    $('#SGSTPercentageAmount').val(roundoff(sgstamt));
    //    $('#grandtotal').val(roundoff(Total + sgstamt+cgstamt));
    //}


    //function CalculateSGST() {
    //    // debugger;
    //    var sgstpercent = $("#sgstpercentage").val();
    //    var Total = $("#total").val();
    //    sgstpercent = parseFloat(sgstpercent);
    //    if (sgstpercent > 100) {
    //        sgstpercent = 100
    //        $("#sgstpercentage").val(sgstpercent);
    //    }
    //    if (sgstpercent < 0) {
    //        sgstpercent = 0
    //        $("#sgstpercentage").val(sgstpercent);
    //    }
    //    Total = parseFloat(Total);
    //    var sgstamt = (Total * sgstpercent / 100)
    //    if (isNaN(sgstamt))
    //    { sgstamt = 0.00 }
    //    $("#SGSTAmount").val(roundoff(sgstamt));
    //    $('#SGSTPercentageAmount').val(roundoff(sgstamt));
    //    $('#grandtotal').val(roundoff(Total + sgstamt));       
    //}



    //function DiscountChange() {
    //    debugger;
    //    //var subtotal = parseFloat($('#subtotal').val()) || 0;
    //    //var discount = parseFloat($('#discount').val()) || 0;
    //    //$('#total').val(roundoff(subtotal - discount));
    //    AmountSummary()
    //}

    function PrintTableToDoc() {
        debugger;
        try {

            $(".buttons-excel").trigger('click');
        }
        catch (e) {
            //this will show the error msg in the browser console(F12) 
            console.log(e.message);
        }
    }

//Trigger button to fill Tax Bill download form
    function DownloadTaxBill()
    {
        debugger;
        FillDownloadForm();
        DownloadForm(); 
    }

//To fill tax form
    function FillDownloadForm()
    {
        debugger;
        DrawTable({
            Action: "TaxBillEntry/GetTaxBill/",
            data: { "ID": $('#HeaderID').val() },
            Exclude_column: ["SCCode", "ID", "HeaderID", "MaterialID", "ReferralRate", "TotalTaxAmount", "GrandTotal", "ItemNo", "SubTotalAmount"
               ],
            Header_column_style: {
                "SlNo": { "style": "font-size:11px;border-bottom:1px solid grey;font-weight: 500;", "custom_name": "Sl No" },
                "Material": { "style": "font-size:11px;border-bottom:1px solid grey;font-weight: 500;", "custom_name": "Item" },
                "Description": { "style": "width:70px;font-size:11px;border-bottom:1px solid grey;font-weight: 500;" },
                "Quantity": { "style": "text-align: center;font-size:11px;border-bottom:1px solid grey;font-weight: 500;" },
                "UOM": { "style": "text-align:center;font-size:11px;border-bottom:1px solid grey;font-weight: 500;" },
                "Rate": { "style": "text-align: right;font-size:11px;border-bottom:1px solid grey;font-weight: 500;", "custom_name": "Rate" },
                "TradeDiscount": { "style": "text-align: right;font-size:11px;border-bottom:1px solid grey;font-weight: 500;", "custom_name": "Discount" },
                "CgstPercentage":{ "style": "text-align: right;font-size:11px;border-bottom:1px solid grey;font-weight: 500;", "custom_name": "CGST %" },
                "CGSTAmount": { "style": "text-align: right;font-size:11px;border-bottom:1px solid grey;font-weight: 500;", "custom_name": "CGST Amt" },
                "SgstPercentage": { "style": "text-align: right;font-size:11px;border-bottom:1px solid grey;font-weight: 500;", "custom_name": "SGST %" },
                "SGSTAmount": { "style": "text-align: right;font-size:11px;border-bottom:1px solid grey;font-weight: 500;", "custom_name": "SGST Amt" },
                "NetAmount": { "style": "text-align: right;font-size:11px;border-bottom:1px solid grey;font-weight: 500;", "custom_name": "Total Amt" },
                
            },
            Row_color: { "Odd": "White", "Even": "white" },
            Body_Column_style: {
                "SlNo": "font-size:9px;font-weight: 100;",
                "Material": "font-size:9px;font-weight: 100;",
                "Description": "font-size:9px;font-weight: 100;width:70px;",
                "Quantity": "text-align:center;font-size:9px;font-weight: 100;",
                "UOM": "text-align:center;font-size:9px;font-weight: 100;",
                "Rate": "text-align:right;font-size:9px;font-weight: 100;",
                "TradeDiscount": "text-align:right;font-size:9px;font-weight: 100;",
                "CgstPercentage": "text-align:right;font-size:9px;font-weight: 100;",
                "CGSTAmount": "text-align:right;font-size:9px;font-weight: 100;",
                "SgstPercentage": "text-align:right;font-size:9px;font-weight: 100;",
                "SGSTAmount": "text-align:right;font-size:9px;font-weight: 100;",
                "NetAmount": "text-align:right;font-size:9px;font-weight: 100;",
            }

        });

        var empID = $("#EmpID :selected").text();       
        $("#lblTechnician").text(empID);
        var billNo = $('#BillNo').val();
        $("#lblBillNo").text(billNo);
        var billDate = $('#BillDate').val();
        $("#lblBillDate").text(billDate);
        var paymentMode = $('#PaymentMode').val();
        $("#lblModeOfPayment").text(paymentMode);
        var paymentRefNo = $('#PaymentRefNo').val();
        $("#lblPaymentReferenceNo").text(paymentRefNo);
        var customerName = $('#CustomerName').val();
        $("#lblCustomerName").text(customerName);
        var customerContactNo = $('#CustomerContactNo').val();
        $("#lblCustomerContact").text(customerContactNo);
        var customerLocation = $('#CustomerLocation').val();
        $('#lblCustomerLocation').text(customerLocation);
        var remarks = $('#Remarks').val();
        $('#lblRemarks').text(remarks);
        var subTotal = $('#subtotal').val();
        $("#lblSubTotal").text(subTotal);
        var Discount = $('#discount').val();
        $("#lblDiscount").text(Discount);
        var Total = $('#total').val();
        $("#lblTotal").text(Total);
        var vatPercentage = $('#vatpercentage').val();
        $("#lblTax").text(vatPercentage);
        var vatAmount = $('#VATAmount').val();
        $("#lblTaxPercentage").text(vatAmount);

        //var cgstPercentage = $('#cgstpercentage').val();
        //$("#lblCGST").text(cgstPercentage).append("<b>%</b>");
        var CGSTAmount = $('#CGSTAmount').val();
        $("#lblCGSTPercentage").text(CGSTAmount);

        //var sgstPercentage = $('#sgstpercentage').val();
        //$("#lblSGST").text(sgstPercentage).append("<b>%</b>");
        var SGSTAmount = $('#SGSTAmount').val();
        $("#lblSGSTPercentage").text(SGSTAmount);



        var serviceChargeAmount = $('#SCAmount').val();
        $("#lblServiceCharge").text(serviceChargeAmount);
        var grandTotal = $('#grandtotal').val();
        $("#lblGrandtotal").text(grandTotal);

        var thisItem = GetAllFranchiseeDetail();
    
        $('#FName').text(thisItem[0].ServiceCenterDescription);
        $('#Address1').text(thisItem[0].ServiceCenterAddress);
        $('#email').text(thisItem[0].ServiceCenterEmail);
        $('#contactNo').text(thisItem[0].ServiceCenterContactNo);
        $('#GSTIN').text(thisItem[0].ServiceCenterGstIn);
        $('#panNo').text(thisItem[0].ServiceCenterPanNo);
        $('#placeOfSupply').text(thisItem[0].ServiceCenterPlace);


    }

//to download Form
    function DownloadForm()
    {
        debugger;
        var BodyContent = $('#divTaxBillDetails').html();
        var HeaderContent = $('#divTaxBillHeader').html();
        $('#hdnContent').val(BodyContent);
        $('#hdnHeadContent').val(HeaderContent);
        //var customerName = $("#CustomerName").val();
        //$('#hdnCustomerName').val(customerName);
        var billNo = $("#BillNo").val();
        $('#hdnBillNo').val(billNo);
        $('#btnDownloadBillToPDF').trigger('click');
    }

    function GetAllFranchiseeDetail() {
        try {
            debugger;
            var data = {};
            var ds = {};
            ds = GetDataFromServer("TaxBillEntry/GetAllFranchiseeDetail/", data);
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


    