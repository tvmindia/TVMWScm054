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

        DataTables.customerBillsTable = $('#tblCustomerBills').DataTable(
        {
            dom: '<"pull-left"f>rt<"bottom"ip><"clear">',
           
            order: [],
            searching: true,
            paging: true,
            //data: GetAllTCRBill(),
            data: null,
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
            { className: "text-center", "targets": [2, 3,   10,  13] },
             { className: "text-left", "targets": [1,9,11,12,4] },
            { "width": "8%", "targets": 1 },
            { "width": "5%", "targets": 2 },
            { "width": "8%", "targets": 3 },
            { "width": "8%", "targets": 4 },
            { "width": "8%", "targets": 5 },
            { "width": "8%", "targets": 6 },
            { "width": "8%", "targets": 7 },
            { "width": "8%", "targets": 8 },      
            { "width": "8%", "targets": 9 },
            { "width": "8%", "targets": 8 },
            { "width": "8%", "targets": 10 },
            { "width": "8%", "targets": 11 },
            { "width": "8%", "targets": 12 },
            { "width": "3%", "targets": 13 },

            ]
        });
     
        BindJobNumberDropDown();


        //**********************//


        DataTables.tcrBillsTable = $('#tblTcrBills').DataTable(
      {
          dom: '<"pull-left"Bf>rt<"bottom"ip><"clear">',
          buttons: [{
              extend: 'excel',
              exportOptions:
                           {
                               columns: [0,1,2,3,4,6,7,8,9,10,11,12,13,14,15]
                           }
          }],
          order: [],
          searching: true,
          paging: true,
          data: null,
          columns: [            
                            
            { "data": "BillNo", "defaultContent": "<i>-</i>" },
            { "data": "BillDate", "defaultContent": "<i>-</i>" },           
            { "data": "JobNo", "defaultContent": "<i>-</i>" },
            { "data": "CustomerName", "defaultContent": "<i>-</i>" },
            { "data": "CustomerContactNo", "defaultContent": "<i>-</i>" },
            { "data": "CustomerLocation", "defaultContent": "<i>-</i>" },
            { "data": "Technician", "defaultContent": "<i>-</i>" },
            { "data": "TCRBillDetail.Material", "defaultContent": "<i>-</i>" },
            { "data": "TCRBillDetail.Description", "defaultContent": "<i>-</i>" },
            { "data": "TCRBillDetail.Quantity", "defaultContent": "<i>-</i>" },
            { "data": "TCRBillDetail.Rate", "defaultContent": "<i>-</i>" },           
            { "data": "TotalAmount", "defaultContent": "<i>-</i>" },
            { "data": "Discount", "defaultContent": "<i>-</i>" },
            { "data": "TotalTaxAmount", "defaultContent": "<i>-</i>" },
            
           { "data": "ServiceCharge", "defaultContent": "<i>-</i>" },

            { "data": "GrandTotal", "defaultContent": "<i>-</i>" },
          ],
          columnDefs: [
          //     { className: "text-right", "targets": [5,6,7,8] },
          //{ className: "text-center", "targets": [2, 3,   10,  13] },
           { className: "text-left", "targets": [1,9,11,12,4] }

          ]
      });


        //*********************//




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
        $(".buttons-excel").hide();
        $("#hdnDiv").hide();
        var $datepicker = $('#BillDate');
        $datepicker.datepicker('setDate', null);

        DataTables.customerBillsTable.on('search.dt', function () {
           
            var searchTerm = $('.dataTables_filter input').val();
            //console.log(input.value)
        })
       
       
        $('#tblCustomerBills_filter').keyup(function () {
            DataTables.tcrBillsTable.search($('.dataTables_filter input').val()).draw();
        });

    }

    catch (x) {

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
    tempObj.CgstAmount = "";
    tempObj.SgstPercentage = "";
    tempObj.SgstAmount = "";
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
                { "data": "TradeDiscount", render: function (data, type, row) { return (EG_createTextBox(data, 'F', row, 'TradeDiscount', 'CalculateAmount')); }, "defaultContent": "<i></i>" },
                {
                    "data": "CgstPercentage", render: function (data, type, row) {
                      
                        return (EG_createTextBox(data, 'F', row, 'CgstPercentage', 'CalculateCGST'));
                    }, "defaultContent": "<i></i>"
                },
                { "data": "CgstAmount", render: function (data, type, row) {return roundoff(data, 1); }, "defaultContent": "<i></i>" },
                {
                    "data": "SgstPercentage", render: function (data, type, row) {
                      
                       // if(data!=null)
                        return (EG_createTextBox(data, 'F', row, 'SgstPercentage', 'CalculateSGST'
                            )) ;
                    }, "defaultContent": "<i></i>"
                },
                { "data": "SgstAmount", render: function (data, type, row) {    
                 return roundoff(data, 1);}, "defaultContent": "<i></i>"},
               { "data": "NetAmount", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i></i>" },
                { "data": null, "orderable": false, "defaultContent": '<a href="#" class="DeleteLink"  onclick="DeleteItem(this)" ><i class="glyphicon glyphicon-trash" aria-hidden="true"></i></a>' }

    ]

    return obj

}
function EG_Columns_Settings() {

    var obj = [
        { "targets": [0], "visible": false, "searchable": false }, { "targets": [1], "visible": false, "searchable": false }, 
         { "width": "5%", "targets": 2 },
         { "width": "10%", "targets": 3 },
        { "width": "15%", "targets": 4 },
         { "width": "8%", "targets": 5 },
        { "width": "8%", "targets": 6 },
         { "width": "8%", "targets": 7 },
          { "width": "8%", "targets": 8 },      
          { "width": "8%", "targets": 9 },
          { "width": "8%", "targets": 8 },
          { "width": "8%", "targets": 10 },
          { "width": "8%", "targets": 11 },
          { "width": "8%", "targets": 12 },
          { "width": "10%", "targets": 13 },
          { "width": "3%", "targets": 14 },
        { className: "text-right", "targets": [7,5,9,11,8] },
        { className: "text-center", "targets": [2,9,14] },
        { className: "text-right disabled", "targets": [] },
        { className: "text-center disabled", "targets": [6, 10, 12] },
         { className: "text-left disabled", "targets": [4] },
        { "orderable": false, "targets": [0, 1, 2, 3, 4, 5, 6, 7, 8,9,10,11,12,13,14] }

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
    debugger;
    $("#SCAmount").val(roundoff($("#SCAmount").val()));
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
    
   
}
//---------------------------------------Edit Item--------------------------------------------------//
function Edit(currentObj) {
    debugger;
    var rowData = DataTables.customerBillsTable.row($(currentObj).parents('tr')).data();
    //Event Request Case
    if ((rowData != null) && (rowData.ID != null)) {

        EG_ClearTable();
        $('#AddTab').trigger('click');
        $("#HeaderID").val(rowData.ID);
        if (BindTCRBillEntry(rowData.ID)) {
            ChangeButtonPatchView('TCRBillEntry', 'btnPatchTCRBillEntrySettab', 'Edit');
            debugger;
          // AmountSummary();
        }
        else {
            $('#ListTab').trigger('click');
        }

    }
    

}
function BindTCRBillEntry(id) {
  
    try {
        debugger;
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
                AmountSummary();
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
            AmountSummary();
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
        ChangeButtonPatchView('TCRBillEntry', 'btnPatchTCRBillEntrySettab', 'Edit');
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
        //$("#CGSTPercentageAmount").val(Records.CGSTAmount);
        $("#SGSTAmount").val(roundoff(Records.SGSTAmount));
       // $("#SGSTPercentageAmount").val(Records.SGSTAmount);
        $("#cgstpercentage").val(roundoff(Records.CgstPercentage));
        $("#sgstpercentage").val(roundoff(Records.SgstPercentage));
        $("#grandtotal").val(roundoff(Records.GrandTotal));
        $("#ServiceChargeComm").val(roundoff(Records.ServiceCharge/100));
        $("#SCCommAmount").val(roundoff(Records.SCCommAmount));
        $("#SpecialComm").val(roundoff(Records.SpecialComm));
       // $('#BillNo').attr('readonly', 'readonly');
       // $('#EmpID').attr('disabled', 'true');
       // $("#EmpID").val(Records.EmpID);

        //var $datepicker = $('#BillDate');
        //$datepicker.datepicker('setDate', new Date(Records.BillDate));

        if (Records.IsDisabled) {
            DisableFields();
            EG_Rebind_WithData(Records.TCRBillEntryDetail, 0);
            $("#tblTCRBillDetails th:last-child").hide();
            $("#tblTCRBillDetails td:last-child").remove();
            $('#tblTCRBillDetails .DeleteLink').attr('onclick', '');
            $('#tblTCRBillDetails .DeleteLink i').css({ "color": "grey", "cursor": "default" });
            $('#tblTCRBillDetails .DeleteLink i').addClass('disabled');
        } else {
            EnableFields();
            EG_Rebind_WithData(Records.TCRBillEntryDetail, 1);
        }
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
            EG_GridData[row - 1]['CgstPercentage'] = _Materials[i].CgstPercentage;
            EG_GridData[row - 1]['SgstPercentage'] = _Materials[i].SgstPercentage;
            //----for calculating amount on changing item(if already quantity exists)
            //CalculateAmount(row);
            EG_Rebind();
            //----------------------------------------------------------------
            break;
        }
    }

}
function BillBookNumberValidation()
{
    debugger;
    try {
      
        var empID = $("#EmpID").val();
        if (empID == "")
        {
            notyAlert('error', "Technician is missing");
        }
        else
        {
            if ($('.popover:visible').length > 0) {
                $("#ahlinkMandatory").click();
            }
            var BillNo = $('#BillNo').val();
            if (BillNo != "" && BillNo != null) {

                var data = { "BillNo": BillNo, "BillBookType": "TCR", "EmpID": empID };
                var ds = {};
                ds = GetDataFromServer("AssignBillBook/BillBookNumberValidation/", data);
               
                if (ds != '') {
                    ds = JSON.parse(ds);
                }
                if (ds.Records == '') {
                    return 0;
                }
                else {
                    var msg = '';
                    debugger;
                    switch (ds.Records.Status) {
                        case "BLB02":
                            if (ds.Records.Name == '') {
                                msg = Messages.BLB02 + "(Bill Book not defined)"
                            } else {
                                msg = Messages.BLB02 + " - (Bill No belongs to " + ds.Records.Name + ')';
                            }
                            break;
                        case "BLB03":
                            msg = Messages.BLB03;
                            break;
                        case "BLB04":
                            msg = Messages.BLB04;
                            break;
                    }
                    if (ds.Records.Status != "BLB01" && ds.Records.Status != "BLB02") {
                        //if ($(".fa-exclamation-triangle").length == 0) {
                        $("#MandatoryStar").hide();
                        $("#BillNoMandatory").show()//.append('<i class="fa fa-exclamation-triangle" data-toggle="popover" data-placement="left" data-content="Content" title="' + msg + "( " + ds.Records.BookNo + " )" + '"></i>');
                        //$("#ahlinkMandatory").attr('data-content', ds.Records.BookNo);
                        $('#ahlinkMandatory').attr('data-content', msg + "( " + ds.Records.BookNo + " )");
                        $("#ahlinkMandatory").click();
                        //$(".popover-content").text("");
                        //$(".popover-content").text(msg + "( " + ds.Records.BookNo + " )");
                        //}
                    }
                    else if (ds.Records.Status == "BLB02") {
                        //if ($(".fa-exclamation-triangle").length == 0) {
                        $("#MandatoryStar").hide();
                        $("#BillNoMandatory").show();//.append('<i class="fa fa-exclamation-triangle" data-toggle="popover" data-placement="left" data-content="Content" title="' + msg + '"></i>');
                        //$("#ahlinkMandatory").attr('data-content', ds.Records.BookNo);                       
                        $('#ahlinkMandatory').attr('data-content', msg);
                        $("#ahlinkMandatory").click();
                        //$(".popover-content").text("");
                        //$(".popover-content").text(msg);
                        //}
                    }
                    else if (ds.Records.Status == "BLB01") {
                        $("#MandatoryStar").show();
                        $("#BillNoMandatory").hide();
                    }


                }
                if (ds.Result == "ERROR") {
                    notyAlert('error', ds.Message);
                    return 0;
                }
                return 1;
            }
        }

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
    debugger;
    var JsonResult = JSON.parse(data)
    switch (JsonResult.Result) {
        case "OK":
            if ($("#HeaderID").val() == emptyGUID || $("#HeaderID").val() == "") {
                BindTCRBillEntry(JsonResult.Records.ID);
                BillBookNumberValidation();
            }
            else
            {
                BindTCRBillEntry($("#HeaderID").val());
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
         
        DataTables.customerBillsTable.clear().rows.add(GetAllTCRBill()).draw(false);
        DataTables.tcrBillsTable.clear().rows.add(GetAllTCRBillForExport()).draw(false);
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
    debugger;
    //$("#JobNo").val("123ERT32q");
    $("#ID").val(emptyGUID);
    var validation = EG_Validate(false);
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


//function CalculateAmount(row) { 
//    debugger;
//    //EG_GridData[row-1][Quantity] = value
//    var qty = 0.00;
//    var rate = 0.00;
//    var dic = 0.00;

//    var EGqty = '';
//    var EGrate = '';
//    var EGdic = '';

//    EGqty = EG_GridData[row - 1]["Quantity"];
//    EGrate = EG_GridData[row - 1]['Rate'];
//    //EGdic = EG_GridData[row - 1]['Discount'];
//    EGdic = EG_GridData[row - 1]['TradeDiscount'];


//    qty = parseFloat(EGqty) || 0;
//    rate = parseFloat(EGrate) || 0;
//    dic = parseFloat(EGdic) || 0;

//    if (dic > (qty * rate)) {
//        dic = (qty * rate);
//    }
//    else if (dic < 0) {
//        dic = 0
//    }

//    EG_GridData[row - 1]['Rate'] = roundoff(rate);
//    EG_GridData[row - 1]['NetAmount'] = roundoff(qty * rate);
//    //EG_GridData[row - 1]['Discount'] = roundoff(dic);
//    EG_GridData[row - 1]['TradeDiscount'] = roundoff(dic);

//    EG_Rebind();
     
//    AmountSummary();

//}

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
    EG_GridData[row - 1]['NetAmount'] = roundoff((qty * rate) - dic);
    CalculateCGST(row, true);
    CalculateSGST(row, true);
    EG_Rebind();

    var total = 0.00;
    for (i = 0; i < EG_GridData.length; i++) {
        total = total + (parseFloat(EG_GridData[i]['NetAmount']) || 0);
    }
    $('#subtotal').val(roundoff(total));
    debugger;
    AmountSummary();


}

//---------------------Finding CGST Amount---------------------//
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
    //EG_GridData[row - 1]['BasicAmount'] = roundoff(qty * rate);
    EG_GridData[row - 1]['TradeDiscount'] = roundoff(dic);
    EG_GridData[row - 1]["CgstPercentage"] = roundoff(cgst);
    EG_GridData[row - 1]['CgstAmount'] = roundoff(((qty * rate) - (dic)) * (cgst / 100));
    EG_GridData[row - 1]['SgstPercentage'] = roundoff(cgst);
    EG_GridData[row - 1]['SgstAmount'] = roundoff(((qty * rate) - (dic)) * (cgst / 100));
   EG_GridData[row - 1]['NetAmount'] = roundoff((qty * rate) - dic);
    if (avoidSummary == undefined || avoidSummary == false) {
        EG_Rebind();
        AmountSummary();
    }
}

//--------------------------Finding SGST Amount--------------------//


function CalculateSGST(row, avoidSummary) {
    debugger;
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
    EGsgst = EG_GridData[row - 1]['SgstPercentage'];
   
   


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
    
    EG_GridData[row - 1]['TradeDiscount'] = roundoff(dic);
    EG_GridData[row - 1]['SgstPercentage'] = roundoff(sgst);
    EG_GridData[row - 1]['SgstAmount'] = roundoff(((qty * rate) - (dic)) * (sgst / 100));
    EG_GridData[row - 1]['NetAmount'] = roundoff((qty * rate)-dic);

    if (avoidSummary == undefined || avoidSummary == false) {
        EG_Rebind();
        AmountSummary();
    }


}

//----------------------Finding TotalTaxAmount and  GrandTotal------------------//
function AmountSummary() {
    debugger;
    var t1 = 0.00;
    var total = 0.00;
    var cgstamount = 0.00;
    var sgstamount = 0.00;
    var discount = 0.00;
    var quant = 0.00;
    var rate = 0.00;
    var taxtotal = 0.00;
    var disc  = 0.00;
    var serviceamount = 0.00;
    var total1 = 0.00;
    var cgstamt = 0.00;
    var sgstamt = 0.00;
    var net = 0.00;
   
    serviceamount = parseFloat($("#SCAmount").val() == "" ? 0 : $("#SCAmount").val());

       for (i = 0; i < EG_GridData.length; i++) {

        //total = total + (parseFloat(EG_GridData[i]['NetAmount']) || 0);
        debugger;
        quant = (parseFloat(EG_GridData[i]['Quantity']) || 0);
        rate = (parseFloat(EG_GridData[i]['Rate']) || 0);
        discount = (parseFloat(EG_GridData[i]['TradeDiscount']) || 0);
        disc = disc+(parseFloat(EG_GridData[i]['TradeDiscount']) || 0);
        cgstamount = (parseFloat(EG_GridData[i]['CgstAmount']) || 0);
        sgstamount = (parseFloat(EG_GridData[i]['SgstAmount']) || 0);
        cgstamt = cgstamt + (parseFloat(EG_GridData[i]['CgstAmount']) || 0);
        sgstamt = sgstamt + (parseFloat(EG_GridData[i]['SgstAmount']) || 0);
        net = net + (parseFloat(EG_GridData[i]['NetAmount']) || 0);
        t1 = t1 + ((quant * rate));
        total = total + ((quant * rate) - (discount) + (cgstamount + sgstamount)); // GrandTotal calculation
        taxtotal = taxtotal + (cgstamount + sgstamount);  // TotalTaxAmount calculation
    }
    total1 = total + serviceamount;
    $('#discount').val(roundoff(disc));
    //$('#total').val(roundoff(t1));
    $('#subtotal').val(roundoff(t1));
    $('#total').val(roundoff(net));
    $('#grandtotal').val(roundoff(total1));
    $('#totaltaxamount').val(roundoff(taxtotal));
    $('#CGSTAmount').val(roundoff(cgstamt));
    $('#SGSTAmount').val(roundoff(sgstamt));
   
}






function ServiceAmountchange()
{
    debugger;
 
    AmountSummary();
    //var total = parseFloat($('#total').val()) || 0;
    //var vatamount = parseFloat($('#VATAmount').val()) || 0;
    //var SCAmount = parseFloat($('#SCAmount').val()) || 0;
    //$('#grandtotal').val(roundoff(total + vatamount + SCAmount));
  
}

function DiscountChange() {
    debugger;
    //var subtotal = parseFloat($('#subtotal').val()) || 0;
    //var discount = parseFloat($('#discount').val()) || 0;
    //$('#total').val(roundoff(subtotal - discount));
    AmountSummary()
}

//function ClearDiscountPercentage() {
//    debugger;
//    if ($('#VATAmount').val() != $('#VATPercentageAmount').val())
//        $("#vatpercentage").val("");

//    var total = parseFloat($('#total').val()) || 0;
//    var vatAmount = parseFloat($('#VATAmount').val()) || 0;
//    var SCAmount = parseFloat($('#SCAmount').val()) || 0;
//    $('#grandtotal').val(roundoff(total + vatAmount + SCAmount));
//}
////To clear Gstdiscountpercentage
//function ClearGSTDiscountPercentage() {
//    debugger;
//    if (($('#CGSTAmount').val() != $('#CGSTPercentageAmount').val()) && ($('#SGSTAmount').val() != $('#SGSTPercentageAmount').val()))
//    {
//        $("#cgstpercentage").val("");
//        $("#sgstpercentage").val("");
//    }
       

//    var total = parseFloat($('#total').val()) || 0;
//    var cgstAmount = parseFloat($('#CGSTAmount').val()) || 0;
//    var sgstAmount = parseFloat($('#SGSTAmount').val()) || 0;
//    var SCAmount = parseFloat($('#SCAmount').val()) || 0;
//    $('#grandtotal').val(roundoff(total + (cgstAmount+sgstAmount) + SCAmount));
//}


//function CalculateVAT() {
//    debugger;
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


//To calculate CGST

//function CalculateCGST() {
//    debugger;
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


////To calculate SGST

//function CalculateSGST() {
//    debugger;
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


//function AmountSummary() {
//    debugger;
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
//    debugger;
//    var Total = 0.00;
//    for (i = 0; i < EG_GridData.length; i++) {
//        Total = Total + (parseFloat(EG_GridData[i]['NetAmount']) || 0);
//    }
//    $('#subtotal').val(roundoff(Total));
//    var discount = parseFloat($('#discount').val()) || 0;

//    $('#total').val(roundoff(Total - discount));

//    var total = parseFloat($('#total').val()) || 0;

//    if (($("#cgstpercentage").val() != "") && ($("#sgstpercentage").val() != "")) {
//        CalculateCGST();
//        CalculateSGST();

//    }

//        var cgstamount = parseFloat($('#CGSTAmount').val()) || 0;
//        var sgstamount = parseFloat($('#SGSTAmount').val()) || 0;
//        var SCAmount = parseFloat($('#SCAmount').val()) || 0;
//        $('#grandtotal').val(roundoff(total + (cgstamount + sgstamount) + SCAmount));
   
//}

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
function getMaterials() {


    try {

        var filter = 1;
        var data = { "filter": filter };
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
        $("#CGSTAmount").val("");
        $("#cgstpercentage").val("");
        $("#SGSTAmount").val("");
        $("#sgstpercentage").val("");

        $("#vatpercentage").val("");
        $("#discount").val("");
        $("#total").val("");
        $("#grandtotal").val("");
        $('#BillNoMandatory').hide();//.find('i').remove()
        //$("#ServiceCharge").val("");
        $("#SCCommAmount").val("");
        $("#SpecialComm").val("");
       // $('#BillNo').attr('readonly', false);
        $('#EmpID').attr('disabled', false);
        var $datepicker = $('#BillDate');
        $datepicker.datepicker('setDate', null);
        EG_ClearTable();
        EG_AddBlankRows(5);

        EnableFields();
        ResetTCRForm();
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
function ResetTCRForm() {
  
    var validator = $("#TCR").validate();
    $('#TCR').find('.field-validation-error span').each(function () {
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
        BillBookNumberValidation();
        getMaterialsByTechnician(v);

    }
    catch (e)
    {
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

function getMaterialsByTechnician(empID) {

    try {

        var data = { "empID": empID };
        var ds = {};
        ds = GetDataFromServer("Item/ItemsForDropdownByTechnician/", data);
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        if (ds.Result == "OK") {
            _Materials = [];
            _Materials = ds.Records;
            EG_ComboSource('Materials', _Materials, 'ItemCode', 'Description')

        }
        if (ds.Result == "ERROR") {
            alert(ds.Message);
        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}



function PrintReport() {
    debugger;
    try {
        GetAllTCRBillForExport();
        $(".buttons-excel").trigger('click');
    }
    catch (e) {
        console.log(e.message);
    }
}

function GetAllTCRBillForExport()
{
    try {

        var data = {};
        var ds = {};
        ds = GetDataFromServer("TCRBillEntry/GetAllTCRBillEntryForExport/", data);

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

function DisableFields() {
    $('#HeaderID').prop('readonly', true);
    $("#EmpID").prop('disabled', true);
    //$("#ModelTechEmpID").prop('readonly', true);
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

function EnableFields() {
    $('#HeaderID').prop('readonly', false);
    $("#EmpID").prop('disabled', false);
    //$("#ModelTechEmpID").prop('readonly', false);
    $("#JobNo").prop('readonly', false);
    $("#BillDate").prop('readonly', false);
    $("#BillNo").prop('readonly', false);
    $("#CustomerName").prop('readonly', false);
    $("#PaymentRefNo").prop('readonly', false);
    $("#CustomerContactNo").prop('readonly', false);
    $("#CustomerLocation").prop('readonly', false);
    $("#PaymentMode").prop('disabled', false);
    $("#Remarks").prop('readonly', false);
    $("#subtotal").prop('readonly', false);
    $("#discount").prop('readonly', true);
    $("#total").prop('readonly', false);
    $("#SCAmount").prop('readonly', false);
    $("#VATAmount").prop('readonly', false);
    $("#CGSTAmount").prop('readonly', false);
    $("#SGSTAmount").prop('readonly', false);
    $("#vatpercentage").prop('readonly', false);
    $("#VATPercentageAmount").prop('readonly', false);
    $("#cgstpercentage").prop('readonly', false);
    $("#CGSTPercentageAmount").prop('readonly', false);
    $("#sgstpercentage").prop('readonly', false);
    $("#SGSTPercentageAmount").prop('readonly', false);
    $("#grandtotal").prop('readonly', false);
    $("#ServiceChargeComm").prop('readonly', false);
    $("#SCCommAmount").prop('readonly', false);
    $("#SpecialComm").prop('readonly', false);
    $("#discount").prop('readonly', false);


    $("#tblTCRBillDetails th:last-child").show();
}