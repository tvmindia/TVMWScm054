var DataTables = {};
var emptyGUID = '00000000-0000-0000-0000-000000000000'
var _Materials = [];
var _JobNoValue = '';
//---------------------------------------Docuement Ready--------------------------------------------------//
$(document).ready(function () {
    try {
        DataTables.customerBillsTable = $('#tblCustomerBills').DataTable(
 {
     dom: '<"pull-left"f>rt<"bottom"ip><"clear">',
     order: [],
     searching: true,
     paging: true,
     data: GetAllICRBill(),
     columns: [
       { "data": "ID", "defaultContent": "<i>-</i>" },
       { "data": "Technician", "defaultContent": "<i>-</i>" },
       { "data": "JobNo", "defaultContent": "<i>-</i>" },
       { "data": "ICRDateFormatted", "defaultContent": "<i>-</i>" },
       { "data": "ICRNo", "defaultContent": "<i>-</i>" },
       { "data": "AMCNO", "defaultContent": "<i>-</i>" },
       { "data": "CustomerName", "defaultContent": "<i>-</i>" },
       { "data": "CustomerContactNo", "defaultContent": "<i>-</i>" },
        { "data": "ModelNo", "defaultContent": "<i>-</i>" },
          { "data": "SerialNo", "defaultContent": "<i>-</i>" },
       { "data": "GrandTotal", render: function (data, type, row) { return roundoff(data, 1); },"defaultContent": "<i>-</i>" },
       { "data": "Remarks", "defaultContent": "<i>-</i>" },
       { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink"  onclick="Edit(this)" ><i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>' }
     ],
     columnDefs: [{ "targets": [0], "visible": false, "searchable": false }, { "targets": [2], "visible": false, "searchable": false },
          { className: "text-right", "targets": [10] },
     { className: "text-center", "targets": [1, 2, 3, 4, 9, 5, 6, 7, 8, 9] },
     {
         "render": function (data, type, row) {
             var returnstring = '';
             debugger;
             if (data) {                
                 returnstring = returnstring + '<span>' + row.AMCNO + ' / (' + row.AMCFromDateFormatted + ' to ' + row.AMCToDateFormatted + ' )</span><br/>';
                 
             }
             else
             {
                 returnstring = returnstring + '<span>' + (row.AMCNO ? row.AMCNO : "-") + ' / (' + (row.AMCFromDateFormatted?row.AMCFromDateFormatted:"-") + ' to ' + (row.AMCToDateFormatted?row.AMCToDateFormatted:"-" )+ ' )</span><br/>';
             }
             if (row.AMCNO == null && row.AMCFromDateFormatted == null && row.AMCToDateFormatted == null)
             {
                 returnstring = "-";
             }
             
             return returnstring;
         },
         "targets": 5
     }

     ]
 });
     
        BindJobNumberDropDown();
        DataTables.ICRBillDetail = $('#tblICRDetails').DataTable(
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
        var $datepicker = $('#ICRDate');
        $datepicker.datepicker('setDate', null);
        var $datepicker = $('#AMCValidFromDate');
        $datepicker.datepicker('setDate', null);
        var $datepicker = $('#AMCValidtoDate');
        $datepicker.datepicker('setDate', null);
        getMaterials();
        EG_ComboSource('Materials', _Materials, 'ItemCode', 'Description')
        EG_GridDataTable = DataTables.ICRBillDetail;
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
        { className: "text-right", "targets": [4,6] },
        { className: "text-center", "targets": [1,8] },
        { className: "text-right disabled", "targets": [7] },
        { className: "text-center disabled", "targets": [5] },
         { className: "text-left disabled", "targets": [3] },
        { "orderable": false, "targets": [0, 1, 2, 3, 4,5,6, 7] }

    ]

    return obj;     

}

//------------------------------------------------------------------

//--------------------button actions ----------------------
function List() {
    try {

        ChangeButtonPatchView('ICRBillEntry', 'btnPatchICRBillEntrySettab', 'List');
        $("#HeaderID").val("");
         reset();
        BindAllCustomerBill();
    } catch (x) {
        // alert(x);
    }

}

function ClearDiscountPercentage()
{
    $("#discountpercentage").val("");
}

function ClearServiceTaxPercentage()
{
    $("#ServiceTaxpercentage").val("");
}

function goBack() {
    $('#AddTab').trigger('click');
    $("#HeaderID").val("");
    reset();
}

//---------------------------------------Edit Item--------------------------------------------------//
function Edit(currentObj) {
    var rowData = DataTables.customerBillsTable.row($(currentObj).parents('tr')).data();
    if ((rowData != null) && (rowData.ID != null)) {
        EG_ClearTable();
        $('#AddTab').trigger('click');      
        $("#HeaderID").val(rowData.ID);
        if (BindICRBillEntry(rowData.ID)) {
            ChangeButtonPatchView('ICRBillEntry', 'btnPatchICRBillEntrySettab', 'Edit');
        }
        else {
            $('#ListTab').trigger('click');
        }
    }
}

function JobSelect(obj) {
    _JobNoValue = $(obj).val();
    FillJobRelatedFields();
}

function FillJobRelatedFields() {
    var job = _JobNoValue; 
    try {
        var data = { "JobNo": job };
        ds = GetDataFromServer("DailyServiceReport/GetDailyJobByJobNo/", data);
      
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        if (ds.Result == "OK") { 
            try {
                $("#CustomerName").val(ds.Record.CustomerName);
                $("#CustomerLocation").val(ds.Record.CustomerLocation);
                var $datepicker = $('#ICRDate');
                $datepicker.datepicker('setDate', new Date(ds.Record.ServiceDate));                
                    $("#ModelNo").val(ds.Record.ModelNo);
                    $("#ICRNo").val(ds.Record.ICRNo);
                    $("#SerialNo").val(ds.Record.SerialNo);              

            } catch (x) {

            }
        }
        if (ds.Result == "ERROR") {
            alert(ds.Message);
        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}
function Add() {
    ChangeButtonPatchView('ICRBillEntry', 'btnPatchICRBillEntrySettab', 'Add');
    EG_ClearTable();
    EG_AddBlankRows(2)
    $("#HeaderID").val("");
    reset();
}
function BindAllCustomerBill() {
    try {
        DataTables.customerBillsTable.clear().rows.add(GetAllICRBill()).draw(false);
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}
function getMaterials() {
    try {
        var data = {};
        var ds = {};
        ds = GetDataFromServer("Item/ServiceTypesItemsForDropdown/", data);
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
    if (($("#HeaderID").val() == "") || ($("#HeaderID").val() == 'undefined') || ($("#HeaderID").val() == "0"))
    {
        $("#EmpID").val("");
        $("#ModelTechEmpID").val("");
        $("#JobNo").val("");
        $("#jobnumberList").val("");
        $("#ICRNo").val("");
        $("#AMCNO").val("");
        $("#CustomerName").val("");
        $("#CustomerContactNo").val("");
        $("#CustomerLocation").val("");
        $("#PaymentMode").val("");
        $("#Remarks").val("");
        $("#subtotal").val("");
        $("#STAmount").val("");
        $("#TotalServiceTaxAmt").val("");
        $("#discountpercentage").val("");
        $("#ServiceTaxpercentage").val("");
        $("#Discount").val("");
        $("#ModelNo").val("");
        $("#PaymentRefNo").val("");
        $("#SerialNo").val("");
        $("#grandtotal").val("");
        $("#total").val("");
        $('#BillNoMandatory').find('i').remove()
        //$('#ICRNo').attr('readonly', false);
        var $datepicker = $('#ICRDate');
        $datepicker.datepicker('setDate', null);
        var $datepicker = $('#AMCValidFromDate');
        $datepicker.datepicker('setDate', null);
        var $datepicker = $('#AMCValidtoDate');
        $datepicker.datepicker('setDate', null);
        EG_ClearTable();
        EG_AddBlankRows(5);
        ResetForm();
    }
    else
    {
        BindICRBillEntry($("#HeaderID").val());
        $("#discountpercentage").val("");
        $("#ServiceTaxpercentage").val("");
    }  
  
}
//-----------------------------------------Reset Validation Messages--------------------------------------//
function ResetForm() {
    var validator = $("#ICR").validate();
    $('#ICR').find('.field-validation-error span').each(function () {
        validator.settings.success($(this));
    });
    validator.resetForm();
}
function BillBookNumberValidation() {
    debugger;
    try {
        var BillNo = $('#ICRNo').val();

        var data = { "BillNo": BillNo, "BillBookType": "TCR" };
        var ds = {};
        ds = GetDataFromServer("AssignBillBook/BillBookNumberValidation/", data);
        debugger;
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        if (ds.Records != '') {
            return 1;
        }
        else {
            if ($(".fa-exclamation-triangle").length == 0)
            {
                $("#BillNoMandatory").append('<i class="fa fa-exclamation-triangle" title="Bill Book For This Entry does not exists!"></i>');
            }
            
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
   
    var rowData = EG_GridDataTable.row($(currentObj).parents('tr')).data();

    if ((rowData != null) && (rowData.ID != null)) {
        notyConfirm('Are you sure to delete?', 'ICRBillDetailDelete("' + rowData.ID + '","' + rowData[EG_SlColumn] + '")', '', "Yes, delete it!");       
    }
}

function BindICRBillEntryFields(Records) {
    try {
      
        ChangeButtonPatchView('ICRBillEntry', 'btnPatchICRBillEntrySettab', 'Edit');
        $('#HeaderID').val(Records.ID);
        $("#EmpID").val(Records.EmpID);
        $("#ModelTechEmpID").val(Records.EmpID);
        $("#JobNo").val(Records.JobNo);
        $("#ICRNo").val(Records.ICRNo);
        $("#AMCNO").val(Records.AMCNO);
        $("#CustomerName").val(Records.CustomerName);
        $("#CustomerContactNo").val(Records.CustomerContactNo);
        $("#CustomerLocation").val(Records.CustomerLocation);
        $("#PaymentMode").val(Records.PaymentMode);
        $("#Remarks").val(Records.Remarks);       
        $("#Discount").val(roundoff(Records.Discount));
        $("#ModelNo").val(Records.ModelNo);
        $("#PaymentRefNo").val(Records.PaymentRefNo);
        $("#SerialNo").val(Records.SerialNo);
        //$("#AMCValidFromDate").val(Records.AMCValidFromDate);
        //$("#AMCValidtoDate").val(Records.AMCValidToDate);
        $('#TotalServiceTaxAmt').val(roundoff(Records.TotalServiceTaxAmt));
        $("#subtotal").val(roundoff(Records.STAmount))
        $("#grandtotal").val(roundoff(Records.GrandTotal));
        $("#total").val(roundoff(Records.Total));
        EG_Rebind_WithData(Records.ICRBillEntryDetail, 1);
       // $('#ICRNo').attr('readonly', 'readonly');

       
        if (Records.ICRDate != null)
        {
            var $datepicker = $('#ICRDate');
            $datepicker.datepicker('setDate', new Date(Records.ICRDate));
        }            

        if (Records.AMCValidFromDate != null)
        {
            var $datepicker = $('#AMCValidFromDate');
            $datepicker.datepicker('setDate', new Date(Records.AMCValidFromDate));
        }
        
               
        if (Records.AMCValidToDate != null)
        {
            var $datepicker = $('#AMCValidtoDate');
            $datepicker.datepicker('setDate', new Date(Records.AMCValidToDate));
        }
       

    } catch (e) {
        notyAlert('error', e.message);
    }
}


function DeleteClick() {
    notyConfirm('Are you sure to delete?', 'ICRBillDelete()');
}
function FillUOM(row) {
   
    for (i = 0; i < _Materials.length; i++) {
        if (_Materials[i].ItemCode == EG_GridData[row - 1]['Material']) {
            EG_GridData[row - 1]['UOM'] = _Materials[i].UOM;
            EG_GridData[row - 1]['MaterialID'] = _Materials[i].ID;
            EG_GridData[row - 1]['Description'] = _Materials[i].Description;
            EG_GridData[row - 1]['Rate'] = _Materials[i].SellingRate;
            if (EG_GridData[row - 1]['Quantity'] == '') {
                EG_GridData[row - 1]['Quantity'] = 1;
            }
           
            EG_Rebind();
            break;
        }
    }

}
function ICRBillDelete() {
    try {
        var id = $('#HeaderID').val();
        if (id != '' && id != null) {
            var data = { "ID": id };
            var ds = {};
            ds = GetDataFromServer("ICRBillEntry/DeleteICRBillEntry/", data);
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
function ICRBillDetailDelete(id, rw) {
    try {
      
        var Hid = $('#HeaderID').val();
        if (id != '' && id != null && Hid != '' && Hid != null && Hid != emptyGUID) {
            var data = { "ID": id, "HeaderID": Hid };
            var ds = {};
            ds = GetDataFromServer("ICRBillEntry/DeleteICRBillDetail/", data);
            if (ds != '') {
                ds = JSON.parse(ds);
            }
            if (ds.Result == "OK") {
                notyAlert('success', ds.Message);
                BindICRBillEntry(Hid);
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

function BindICRBillEntry(id) {
    try {
        var data = { "ID": id };
        var ds = {};
        ds = GetDataFromServer("ICRBillEntry/GetICRBillHeaderByID/", data);
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        if (ds.Result == "OK") {

            BindICRBillEntryFields(ds.Records);
            BillBookNumberValidation();
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
function GetAllICRBill() {
    try {
      
        var data = {};
        var ds = {};
        ds = GetDataFromServer("ICRBillEntry/GetAllICRBillEntry/", data);
      
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

function CalculateServiceTaxPercentage(id) {
    debugger;
    var serviceTaxpercent = $("#ServiceTaxpercentage").val();
    var baseAmt = $("#total").val();
    if (serviceTaxpercent != "") {
        serviceTaxpercent = parseInt(serviceTaxpercent);
        if (serviceTaxpercent > 100) {
            serviceTaxpercent = 100
            $("#ServiceTaxpercentage").val(serviceTaxpercent);
        }
        if (serviceTaxpercent < 0) {
            serviceTaxpercent = 0
            $("#ServiceTaxpercentage").val(serviceTaxpercent);
        }
        baseAmt = parseInt(baseAmt);
        var vatamt = (baseAmt * serviceTaxpercent / 100)
        if (isNaN(vatamt)) { vatamt = 0.00 }
        $("#TotalServiceTaxAmt").val(roundoff(vatamt));

    }
    else {
        if(id==1)
        {
            var vatamt = 0.00;
            $("#TotalServiceTaxAmt").val(roundoff(vatamt));
        }
    }
       
  
    

    AmountSummary();
}

function CalculateDiscountPercentage(id) {
    debugger;
    var discountpercent = $("#discountpercentage").val();
    var baseAmt = $("#subtotal").val();
    if (discountpercent != "")
    {
       
        discountpercent = parseInt(discountpercent);
        if (discountpercent > 100) {
            discountpercent = 100
            $("#discountpercentage").val(discountpercent);
        }
        if (discountpercent < 0) {
            discountpercent = 0
            $("#discountpercentage").val(discountpercent);
        }
        baseAmt = parseInt(baseAmt);
        var Discount = (baseAmt * discountpercent / 100)
        if (isNaN(Discount)) { Discount = 0.00 }
        $("#Discount").val(roundoff(Discount));
    }
    else
    {
        if(id==1)
        {
            var Discount = 0.00
            $("#Discount").val(roundoff(Discount));
        }
    }
   

    AmountSummary();
}

function SaveSuccess(data, status) {
   
    var JsonResult = JSON.parse(data)
    switch (JsonResult.Result) {
        case "OK":
            if ($("#HeaderID").val() == emptyGUID || $("#HeaderID").val()=="") {
                BindICRBillEntry(JsonResult.Records.ID);
            }
            else {
                BindICRBillEntry($("#HeaderID").val());
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
    $("#STAmount").val(roundoff(total));
    AmountSummary();
    CalculateServiceTaxPercentage(0);
    CalculateDiscountPercentage(0);

}
function BindJobNumberDropDown() {

    try {

        var data = {};
        var ds = {};
        ds = GetDataFromServer("DailyServiceReport/GetJobNumbersForDropDown/", data);
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        if (ds.Result == "OK") {
            var options = '';
            $.each(ds.Records, function (key, value) {
                //$("#RepeatJobNo").append($("<option></option>").val(value.ID).html(value.JobNo));
                options += '<option id="' + value.ID + '" value="' + value.JobNo + '" >' + '</option>';
            });
            document.getElementById('jobnumberList').innerHTML = '';
            document.getElementById('jobnumberList').innerHTML = options;
        }
        if (ds.Result == "ERROR") {
            notyAlert('error', ds.Message);
        }

    }
    catch (e) {
        notyAlert('error', e.Message);
    }
}
function AmountSummary() {
    debugger;
    var subtotal = parseFloat($('#subtotal').val()) || 0;
    var totalServiceTaxAmt = parseFloat($('#TotalServiceTaxAmt').val()) || 0;
    var discount = parseFloat($('#Discount').val()) || 0;
    $('#total').val(roundoff(subtotal - discount));
    var total = parseFloat($('#total').val()) || 0;
    $('#grandtotal').val(roundoff(total + totalServiceTaxAmt));    
    $('#subtotal').val(roundoff(subtotal));
    $('#TotalServiceTaxAmt').val(roundoff(totalServiceTaxAmt));
    $('#Discount').val(roundoff(discount))
}





function AddTechnicanJob() {
    var techi = $("#ModelTechEmpID").val();

    if ((techi)) {
        $("#AddJobModel").modal('show');
        $("#TechnicianLabel").text($("#EmpID option:selected").text());
        $("#ServiceDateLabel").text('Date not selected');
        $("#modelContextLabel").text('Add Job');
        ClearJobForm();
        $(".calltypehidden").hide();
    }
    else {
        notyAlert('error', 'Please Choose Technician');
    }

}
function TechnicianSelectOnChange(curobj) {
    try {
        var v = $(curobj).val();
        $("#ModelTechEmpID").val(v);
    }
    catch (e) {
        notyAlert('error', e.Message);
    }
}
function RefreshDailyServiceTable(jobNo) {
    //need to write code to refresh combo
    // $("#JobNo").html($("#JobNo").html() + '<option value="' + jobNo + '">' + jobNo + '</option>')
   
    //   $("#JobNo").val(jobNo);
    _JobNoValue = jobNo;
    ReBindJobNoDropdown();  
    FillJobRelatedFields();
}
function ReBindJobNoDropdown() {
    try {
       
        var data = {};
        var ds = {};
        ds = GetDataFromServer("ICRBillEntry/RebindJobNo/", data);
       
        if (ds != '') {
           
            ds = JSON.parse(ds);
        }
        if (ds.Result == "OK") {
         
            $('#jobnumberList').html('');
            for (var i = 0; i < ds.Records.length-1; i++) {
                var opt = new Option(ds.Records[i].Value, ds.Records[i].Text);                
                $('#jobnumberList').append(opt);
                $("#JobNo").val(_JobNoValue);
            }
           
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