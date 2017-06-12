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
            data: GetAllTCRBill(),
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
            { className: "text-center", "targets": [1,2, 3, 4, 9, 5, 6, 7, 8,9,10] }

            ]
        });
        BindJobNumberDropDown();
        DataTables.TCRBillDetail = $('#tblTCRBillDetails').DataTable(
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
        { "targets": [0], "visible": false, "searchable": false }, { "targets": [1], "visible": false, "searchable": false }, 
         { "width": "5%", "targets": 2 },
         { "width": "15%", "targets": 3 },
        { "width": "20%", "targets": 4 },
         { "width": "8%", "targets": 5 },
        { "width": "8%", "targets": 6 },
         { "width": "8%", "targets": 7 },
          { "width": "10%", "targets": 8 },      
          { "width": "5%", "targets": 9 },
        { className: "text-right", "targets": [7,5] },
        { className: "text-center", "targets": [2,9] },
        { className: "text-right disabled", "targets": [8] },
        { className: "text-center disabled", "targets": [6] },
         { className: "text-left disabled", "targets": [4] },
        { "orderable": false, "targets": [0, 1, 2, 3, 4, 5, 6, 7, 8] }

    ]

    return obj;

}

//------------------------------------------------------------------

//--------------------button actions ----------------------
function List() {
    try {
      
        ChangeButtonPatchView('TCRBillEntry', 'btnPatchTCRBillEntrySettab', 'List');
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

function CalculateSCCommissionAmt()
{
     
    var serviceCharge = $("#SCAmount").val();
    var SCcmmsn = $("#ServiceChargeComm").val();
    serviceCharge = parseInt(serviceCharge);
    SCcmmsn = parseInt(SCcmmsn);
    if (SCcmmsn < 0) {
        SCcmmsn = 0;
        $("#ServiceChargeComm").val(0.00)
    }

    if (SCcmmsn != undefined && !isNaN(SCcmmsn))
    {
        var a = serviceCharge * SCcmmsn / 100;
        if (isNaN(a)) {
            a = 0;
        }
        $("#SCCommAmount").val(roundoff(a));
    }
    AmountSummary();
   
}



//---------------------------------------Edit Item--------------------------------------------------//
function Edit(currentObj) {
     
    var rowData = DataTables.customerBillsTable.row($(currentObj).parents('tr')).data();
    //Event Request Case
    if ((rowData != null) && (rowData.ID != null)) {

        EG_ClearTable();
        $('#AddTab').trigger('click');
        $("#HeaderID").val(rowData.ID);
        if (BindTCRBillEntry(rowData.ID)) {
            ChangeButtonPatchView('TCRBillEntry', 'btnPatchTCRBillEntrySettab', 'Edit');

        }
        else {
            $('#ListTab').trigger('click');
        }

    }
    

}

function BindTCRBillEntry(id) {
    debugger;
    try {
        var data = { "ID": id };
        var ds = {};
        ds = GetDataFromServer("TCRBillEntry/GetTCRBillHeaderByID/", data);
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        if (ds.Result == "OK") {

            BindTCRBillEntryFields(ds.Records);
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
function DeleteItem(currentObj) {
     
    var rowData = EG_GridDataTable.row($(currentObj).parents('tr')).data();

    if ((rowData != null) && (rowData.ID != null)) {
        notyConfirm('Are you sure to delete?', 'TCRBillDetailDelete("' + rowData.ID + '","' + rowData[EG_SlColumn] + '")', '', "Yes, delete it!");
     
    }
}
function TCRBillDetailDelete(id, rw) {
    try {
         
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

        ChangeButtonPatchView('TCRBillEntry', 'btnPatchTCRBillEntrySettab', 'Edit');
        $('#HeaderID').val(Records.ID);
        $("#EmpID").val(Records.EmpID);
        $("#ModelTechEmpID").val(Records.EmpID);
        $("#JobNo").val(Records.JobNo);
        $("#BillNo").val(Records.BillNo);
        $("#CustomerName").val(Records.CustomerName);
        $("#PaymentRefNo").val(Records.PaymentRefNo);
        $("#CustomerContactNo").val(Records.CustomerContactNo);
        $("#CustomerLocation").val(Records.CustomerLocation);
        $("#PaymentMode").val(Records.PaymentMode);
        $("#Remarks").val(Records.Remarks);
        $("#subtotal").val(roundoff(Records.Subtotal));
        $("#SCAmount").val(roundoff(Records.ServiceCharge));
        $("#VATAmount").val(roundoff(Records.VATAmount));
        //$("#vatpercentage").val(Records.EmpID);
        $("#discount").val(roundoff(Records.Discount));
        $("#grandtotal").val(roundoff(Records.GrandTotal));
       // $("#ServiceChargeComm").val(Records.ServiceCharge/100);
        $("#SCCommAmount").val(roundoff(Records.SCCommAmount));
        $("#SpecialComm").val(roundoff(Records.SpecialComm));
        EG_Rebind_WithData(Records.TCRBillEntryDetail, 1);
        $('#BillNo').attr('readonly', 'readonly');
       // $('#EmpID').attr('disabled', 'true');
       // $("#EmpID").val(Records.EmpID);

        var $datepicker = $('#BillDate');
        $datepicker.datepicker('setDate', new Date(Records.BillDate));

    } catch (e) {
        notyAlert('error', e.message);
    }




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

function BillBookNumberValidation()
{
    debugger;
    try {
        var BillNo = $('#BillNo').val();
       
        var data = { "BillNo": BillNo ,"BillBookType":"TCR"};
            var ds = {};
            ds = GetDataFromServer("AssignBillBook/BillBookNumberValidation/", data);
            debugger;
            if (ds != '') {
                ds = JSON.parse(ds);
            }
            if (ds.Records !='') {
                return 1;
            }
            else
            {
                if ($(".fa-exclamation-triangle").length == 0) {
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

function DeleteClick()
{
    notyConfirm('Are you sure to delete?', 'TCRBillDelete()', '', "Yes, delete it!");
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
     
    var JsonResult = JSON.parse(data)
    switch (JsonResult.Result) {
        case "OK":
            BindTCRBillEntry(JsonResult.Records.ID);
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
         
        DataTables.customerBillsTable.clear().rows.add(GetAllTCRBill()).draw(false);
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}



//---------------get grid fill result-------------------
function GetAllTCRBill() {
    try {
         
        var data = {};
        var ds = {};
        ds = GetDataFromServer("TCRBillEntry/GetAllTCRBillEntry/", data);
         
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
     
   
    ChangeButtonPatchView('TCRBillEntry', 'btnPatchTCRBillEntrySettab', 'Add');
    EG_ClearTable();
   // RestForm8();
    EG_AddBlankRows(5)
    $("#HeaderID").val("");
    reset();
}

function CalculateVAT()
{
     
    var vatpercent = $("#vatpercentage").val(); 

    var subTotal = $("#subtotal").val();
    vatpercent = parseInt(vatpercent);
    if (vatpercent > 100) {
        vatpercent = 100
        $("#vatpercentage").val(vatpercent);
    }
    if (vatpercent < 0) {
        vatpercent = 0
        $("#vatpercentage").val(vatpercent);
    }
    subTotal = parseInt(subTotal);
    var vatamt = (subTotal * vatpercent / 100)
    if (isNaN(vatamt)) { vatamt=0.00}
    $("#VATAmount").val(roundoff(vatamt));

    AmountSummary();
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
     
    var subtotal = parseFloat($('#subtotal').val()) || 0;
    var vatamount = parseFloat($('#VATAmount').val()) || 0;
    var SCAmount = parseFloat($('#SCAmount').val()) || 0;
    var discount = parseFloat($('#discount').val()) || 0;
    var vatp = (parseFloat($('#vatpercentage').val()) || 0);
    if (vatp > 0) {
        vatamount = (subtotal * vatp) / 100;
        $('#VATAmount').val(roundoff(vatamount));
    }
    $('#subtotal').val(roundoff(subtotal));
    $('#VATAmount').val(roundoff(vatamount));
    $('#SCAmount').val(roundoff(SCAmount));
    $('#discount').val(roundoff(discount))
    $('#grandtotal').val(roundoff(subtotal + vatamount + SCAmount - discount));
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
    if (($("#HeaderID").val() == "") || ($("#HeaderID").val() == 'undefined') || ($("#HeaderID").val() == "0")) {
        // $("#HeaderID").val(emptyGUID);
        $("#EmpID").val("");
        $("#ModelTechEmpID").val("");
        $("#JobNo").val("");
        $("#jobnumberList").val("");
        $("#BillNo").val("");
        $("#CustomerName").val("");
        $("#PaymentRefNo").val("");
        $("#CustomerContactNo").val("");
        $("#CustomerLocation").val("");
        $("#PaymentMode").val("");
        $("#Remarks").val("");
        $("#subtotal").val("");
        $("#SCAmount").val("");
        $("#ServiceChargeComm").val("");
        $("#VATAmount").val("");
        $("#vatpercentage").val("");
        $("#discount").val("");
        $("#grandtotal").val("");
        $('#BillNoMandatory').find('i').remove()
        //$("#ServiceCharge").val("");
        $("#SCCommAmount").val("");
        $("#SpecialComm").val("");
        $('#BillNo').attr('readonly', false);
        $('#EmpID').attr('disabled', false);
        var $datepicker = $('#BillDate');
        $datepicker.datepicker('setDate', null);
        EG_ClearTable();
        EG_AddBlankRows(5);
        ResetForm();
    }
    else
    {
        BindTCRBillEntry($("#HeaderID").val());
    }
}

function FillJobRelatedFields() {
    var job = $("#JobNo").val();
    try {

        var data = {"JobNo":job};
        ds = GetDataFromServer("DailyServiceReport/GetDailyJobByJobNo/", data);
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        if (ds.Result == "OK") {
            try {
               
                $("#CustomerName").val(ds.Record.CustomerName);
                $("#CustomerLocation").val(ds.Record.CustomerLocation);
                var $datepicker = $('#BillDate');
                $datepicker.datepicker('setDate', new Date(ds.Record.ServiceDate));

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



//-----------------------------------------Reset Validation Messages--------------------------------------//
function ResetForm() {
    var validator = $("#F8").validate();
    $('#F8').find('.field-validation-error span').each(function () {
        validator.settings.success($(this));
    });
    validator.resetForm();
}



function AddTechnicanJob() {
    var techi = $("#ModelTechEmpID").val();
     
    if ((techi) ) {
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
    _JobNoValue = jobNo;
    //  $("#JobNo").val(jobNo);
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
            for (var i = 0; i < ds.Records.length - 1; i++) {
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

function JobSelect(obj) {
    FillJobRelatedFields();
    //var v = $(obj).val();
    //RefreshDailyServiceTable(v);
}