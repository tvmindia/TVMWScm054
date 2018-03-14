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
                                 columns: [1, 2, 3, 4, 5, 6, 7,8,9]
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
              { "data": "GrandTotal", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
              { "data": "CustomerName", "defaultContent": "<i>-</i>" },
              { "data": "CustomerContactNo", "defaultContent": "<i>-</i>" },
              { "data": "CustomerLocation", "defaultContent": "<i>-</i>" },
              { "data": "Remarks", "defaultContent": "<i>-</i>" },
              { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink"  onclick="Edit(this)" ><i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>' }
            ],
            columnDefs: [{ "targets": [0], "visible": false, "searchable": false }, { "targets": [2], "visible": false, "searchable": false },
                 { className: "text-right", "targets": [5] },
            { className: "text-center", "targets": [1, 2, 3, 4, 9, 5, 6, 7, 8, 9, 10] }

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
        { "width": "20%", "targets": 4 },
         { "width": "8%", "targets": 5 },
        { "width": "8%", "targets": 6 },
         { "width": "8%", "targets": 7 },
          { "width": "10%", "targets": 8 },
          { "width": "10%", "targets": 9 },
          { "width": "5%", "targets": 10 },
        { className: "text-right", "targets": [8] },
        { className: "text-center", "targets": [] },
        { className: "text-right disabled", "targets": [7,9] },
        { className: "text-center disabled", "targets": [2,5,6,10] },
         { className: "text-left disabled", "targets": [3,4] },
        { "orderable": false, "targets": [0, 1, 2, 3, 4, 5, 6, 7, 8,9,10] }

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

function Add() {
    debugger;
    ChangeButtonPatchView('TaxBillEntry', 'btnPatchTaxBillEntrySettab', 'Add');
    EG_ClearTable();   
    EG_AddBlankRows(5)
    $("#HeaderID").val("");
    //reset();
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
            CalculateAmount(row)
            //EG_Rebind();
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
            //  
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
        var data = {};
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
        $("#total").val(roundoff(Records.Subtotal - Records.Discount));
        $("#SCAmount").val(roundoff(Records.ServiceCharge));
        $("#VATAmount").val(roundoff(Records.VATAmount));
        $("#VATPercentageAmount").val(Records.VATAmount);

        $("#grandtotal").val(roundoff(Records.GrandTotal));
        $("#ServiceChargeComm").val(roundoff(Records.ServiceCharge / 100));
        $("#SCCommAmount").val(roundoff(Records.SCCommAmount));
        $("#SpecialComm").val(roundoff(Records.SpecialComm));
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
    $("#discount").prop('readonly', true);
    $("#total").prop('readonly', true);
    $("#SCAmount").prop('readonly', true);
    $("#VATAmount").prop('readonly', true);
    $("#vatpercentage").prop('readonly', true);
    $("#VATPercentageAmount").prop('readonly', true);
    $("#grandtotal").prop('readonly', true);
    $("#ServiceChargeComm").prop('readonly', true);
    $("#SCCommAmount").prop('readonly', true);
    $("#SpecialComm").prop('readonly', true);   
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

    function AmountSummary() {
        //debugger;
        var Total = 0.00;
        for (i = 0; i < EG_GridData.length; i++) {
            Total = Total + (parseFloat(EG_GridData[i]['NetAmount']) || 0);
        }
        $('#subtotal').val(roundoff(Total));
        var discount = parseFloat($('#discount').val()) || 0;

        $('#total').val(roundoff(Total - discount));

        var total = parseFloat($('#total').val()) || 0;

        if ($("#vatpercentage").val() != "")
            CalculateVAT();

        var vatamount = parseFloat($('#VATAmount').val()) || 0;
        var SCAmount = parseFloat($('#SCAmount').val()) || 0;
        $('#grandtotal').val(roundoff(total + vatamount + SCAmount));
    }

    function CalculateVAT() {
       // debugger;
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
        if (isNaN(vatamt))
        { vatamt = 0.00 }
        $("#VATAmount").val(roundoff(vatamt));
        $('#VATPercentageAmount').val(roundoff(vatamt));
        $('#grandtotal').val(roundoff(Total + vatamt));
    }

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
            Exclude_column: ["SCCode", "ID", "HeaderID", "MaterialID","ReferralRate"
               ],
            Header_column_style: {
                "SlNo": { "style": "font-size:11px;border-bottom:1px solid grey;font-weight: 500;", "custom_name": "No" },
                "Material": { "style": "font-size:11px;border-bottom:1px solid grey;font-weight: 500;", "custom_name": "Item" },
                "Description": { "style": "width:70px;font-size:11px;border-bottom:1px solid grey;font-weight: 500;" },
                "Quantity": { "style": "text-align: center;font-size:11px;border-bottom:1px solid grey;font-weight: 500;" },
                "UOM": { "style": "text-align:center;font-size:11px;border-bottom:1px solid grey;font-weight: 500;" },
                "Rate": { "style": "text-align: right;font-size:11px;border-bottom:1px solid grey;font-weight: 500;", "custom_name": "Rate" },
                "NetAmount": { "style": "text-align: right;font-size:11px;border-bottom:1px solid grey;font-weight: 500;", "custom_name": "Total Amount" },
                
            },
            Row_color: { "Odd": "White", "Even": "white" },
            Body_Column_style: {
                "SlNo": "font-size:9px;font-weight: 100;",
                "Material": "font-size:9px;font-weight: 100;",
                "Description": "font-size:9px;font-weight: 100;width:70px;",
                "Quantity": "text-align:center;font-size:9px;font-weight: 100;",
                "UOM": "text-align:center;font-size:9px;font-weight: 100;",
                "Rate": "text-align:right;font-size:9px;font-weight: 100;",
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
        var serviceChargeAmount = $('#SCAmount').val();
        $("#lblServiceCharge").text(serviceChargeAmount);
        var grandTotal = $('#grandtotal').val();
        $("#lblGrandtotal").text(grandTotal);
    }

//to download Form
    function DownloadForm()
    {
        debugger;
        var BodyContent = $('#divTaxBillDetails').html();
        var HeaderContent = $('#divTaxBillHeader').html();
        $('#hdnContent').val(BodyContent);
        $('#hdnHeadContent').val(HeaderContent);
        var customerName = $("#CustomerName").val();
        $('#hdnCustomerName').val(customerName);
        $('#btnDownloadBillToPDF').trigger('click');
    }





