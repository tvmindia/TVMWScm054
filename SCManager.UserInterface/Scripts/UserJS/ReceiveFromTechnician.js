﻿var DataTables = {};
var emptyGUID = '00000000-0000-0000-0000-000000000000'
var _Materials = [];
var typingStarted = 0;

//---------------------------------------Docuement Ready--------------------------------------------------//
$(document).ready(function () {
    try {


        DataTables.ReceiveItems = $('#tblReceiptItemsList').DataTable(
       {
           dom: '<"pull-left"Bf>rt<"bottom"ip><"clear">',
           buttons: [{
               extend: 'excel',
               exportOptions:
                            {
                                columns: [4,3,5,6,7,8]
                            }
           }],

           order: [],
           searching: true,
           paging: true,
           data: null,
           columns: [
             { "data": "SCCode" },
             { "data": "ID" },
             { "data": "MaterialID", "defaultContent": "<i></i>" },
              { "data": "empName", "defaultContent": "<i></i>" },
              { "data": "DateFormatted", "defaultContent": "<i></i>" },
             { "data": "Material", "defaultContent": "<i></i>" },
             { "data": "Description", "defaultContent": "<i></i>" },
             { "data": "Qty", "defaultContent": "<i></i>" },
             { "data": "UOM", "defaultContent": "<i></i>" }
           ],
           columnDefs: [{ "targets": [0], "visible": false, "searchable": false }, { "targets": [1], "visible": false, "searchable": false },
               { "targets": [2], "visible": false, "searchable": false },
                { className: "text-right disabled", "targets": [7] },
           { className: "text-center disabled", "targets": [3, 4, 5, 8] },
           { className: "text-left disabled", "targets": [6] }

           ]
       });

        $('#tblReceiptItemsList tbody').on('dblclick', 'td', function () {

            Edit(this);
        });


        DataTables.ReceiptSheetTable = $('#tblReceiptSheet').DataTable(
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
        EG_GridDataTable = DataTables.ReceiptSheetTable;
        Add();
        var $datepicker = $('#ReceiveDate');
        $datepicker.datepicker('setDate', null);
        // List();

        $('#fromDate').change(function () {
            BindAllReceiveList();
        });
        $('#toDate').change(function () {
            BindAllReceiveList();
        });
        $('#ReceiveDate').change(function () {
            GetReceiveSheetsByTechnician();
        });
        fillTechnicians();
        $(".buttons-excel").hide();
    } catch (x) {

        notyAlert('error', x.message);

    }

});


//-----------------------EDIT GRID DEFN-------------------------------------
var EG_totalDetailRows = 0;
var EG_GridData;//DATA SOURCE OBJ ARRAY
var EG_GridDataTable;//DATA TABLE ITSELF FOR REBIND PURPOSE
var EG_SlColumn = 'SlNo';
var EG_GridInputPerRow = 2;
var EG_MandatoryFields = 'MaterialID,Material,Qty'

function EG_TableDefn() {

    var tempObj = new Object();
    tempObj.SCCode = "";
    tempObj.ID = "";
    tempObj.MaterialID = "";
    tempObj.SlNo = 0;
    tempObj.Material = "";
    tempObj.Qty = "";
    tempObj.UOM = "";

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
                { "data": "Qty", render: function (data, type, row) { return (EG_createTextBox(data, 'N', row, 'Qty', '')); }, "defaultContent": "<i></i>" },
                { "data": "UOM", "defaultContent": "<i></i>" },
                { "data": null, "orderable": false, "defaultContent": '<a href="#" class="DeleteLink"  onclick="Delete(this)" ><i class="glyphicon glyphicon-trash" aria-hidden="true"></i></a>' }

    ]

    return obj

}

function EG_Columns_Settings() {
  
    var obj = [
        { "targets": [0], "visible": false, "searchable": false }, { "targets": [1], "visible": false, "searchable": false }, { "targets": [2], "visible": false, "searchable": false },
        { "targets": [4], "width": "20%" },
        { className: "text-right", "targets": [6] },
        { className: "text-center disabled", "targets": [7] },
        { className: "text-left disabled", "targets": [5] },
        { className: "text-center", "targets": [3, 4, 8] },
        { "orderable": false, "targets": [0, 1, 2, 3, 4, 5, 6, 7] }

    ]

    return obj;

}

//------------------------------------------------------------------

//---------------------page related logics----------------------------------- 


function fillTechnicians() {
    
    var ddlSelect = document.getElementById("ddlReceiveListTech");
    var option = document.createElement('option');
    option.text = "All Technicians", option.value = "All";
    ddlSelect.appendChild(option);
    $("#ddlReceiveListTech").val("All");
    BindAllReceiveList()
}

function getMaterialsByTechnician(empID)
{

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
            _Materials = [];
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

function FillUOM(row) {

    for (i = 0; i < _Materials.length; i++) {
        if (_Materials[i].ItemCode == EG_GridData[row - 1]['Material']) {
            EG_GridData[row - 1]['UOM'] = _Materials[i].UOM;
            EG_GridData[row - 1]['MaterialID'] = _Materials[i].ID;
            EG_GridData[row - 1]['Description'] = _Materials[i].Description;
            EG_Rebind();
            typingStarted = 1;
            break;
        }
    }

} function save() {

    var EmpID = $("#HiddenEmpID").val();
    var ReceiveDate = $("#ReceiveDate").val();
    if (EmpID != "" && ReceiveDate != "") {
       
        var validation = EG_Validate();
        if (validation == "") {

            var result = JSON.stringify(EG_GridData);
            $("#DetailJSON").val(result);
            $("#btnInsertUpdateReceiveSheets").trigger('click');
        }
        else {
            notyAlert('error', validation);
        }
    }
    else {
        $("#btnInsertUpdateReceiveSheets").trigger('click');
    }
}

function ReceiptSheetsSaveSuccess(data, status) {
   
    var i = JSON.parse(data)
   
    switch (i.Result) {
     
        case "OK":
            notyAlert('success', i.Message);
           
            BindReceiveSheetFields(i.Records)
            ChangeButtonPatchView('ReceiveFromTechnician', 'btnPatchReceiveFromTechnicianSettab', 'Edit');

            break;
        case "Error":
            notyAlert('error', i.Message);
            break;
        case "ERROR":
            notyAlert('error', i.Message);
            GetReceiveSheetsByTechnician();
            break;
        default:
            break;
    }
}
function BindAllReceiveList() {
    DataTables.ReceiveItems.clear().rows.add(GetReceiveList()).draw(false);
}
function GetReceiveList() {
    try {
       
        var empID = $("#ddlReceiveListTech").val();
        var fromDate = $("#fromDate").val();
        var toDate = $("#toDate").val();
        if (toDate == "" && fromDate == "" && empID == "") {
            //DataTables.CreditNotesTable.clear().rows.add(GetAllCreditNotes(false)).draw(false);
        }
        else {
            var data = { "fromDate": fromDate, "toDate": toDate, "empID": empID };
            var ds = {};
            ds = GetDataFromServer("ReceiveFromTechnician/GetAllReceiptsFromTechnician/", data);
           
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

    }
    catch (e) {
        notyAlert('error', e.message);
    }
}
function BindReceiveSheetFields(Records) {
    
    EG_Rebind_WithData(Records, 1)

}
function TechnicianChange(curObj) {
    
    $("#ddlReceiveListTech").val(curObj.value);
    if (curObj.value != undefined && curObj.value != null && curObj.value!="")
    {
        getMaterialsByTechnician(curObj.value);
    }
    else
    {
        getMaterials();
        EG_ComboSource('Materials', _Materials, 'ItemCode', 'Description');
    }
    GetReceiveSheetsByTechnician();
}
function Delete(currentObj) {
    var rowData = DataTables.ReceiptSheetTable.row($(currentObj).parents('tr')).data();
    if ((rowData != null) && (rowData.ID != null)) {
        notyConfirm('Are you sure to delete?', 'DeleteReceiveFromTech("' + rowData.ID + '","' + rowData[EG_SlColumn] + '")', '', "Yes, delete it!");
    }
    
}
function DeleteReceiveFromTech(id, rw) {
   
    var data = { "ID": id };
    var ds = {};
    if (id != '' && id != null) {
        ds = GetDataFromServer("ReceiveFromTechnician/DeleteReceiveFromTechnician/", data);
      
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        if (ds.Result == "OK") {
            notyAlert('success', i.Message);
            typingStarted = 0;
            GetReceiveSheetsByTechnician();
            return ds.Records;
        }
        if (ds.Result == "ERROR") {
            notyAlert('error', ds.Message);
            return 0;
        }
    }
    else {
       
        if (EG_GridData.length != 1) {
            EG_GridData.splice(rw - 1, 1);
            EG_Rebind_WithData(EG_GridData, 0);
        }
        else {
            //reset();
            RebindGrid();
            EG_Rebind();
        }
        notyAlert('success', 'Deleted Successfully');
    }

}
function GetReceiveSheetsByTechnician() {
    try {
      
        var empID = $("#ddlReceiveListTech").val();
        var transferDate = $("#ReceiveDate").val();

        if (transferDate == "" && empID == "") {
            //DataTables.CreditNotesTable.clear().rows.add(GetAllCreditNotes(false)).draw(false);
        }
        else if (transferDate != "" && empID == "All") {

        }
        else {
            var data = { "transferDate": transferDate, "empID": empID };
            var ds = {};
            ds = GetDataFromServer("ReceiveFromTechnician/GetReceiptsSheet/", data);
          
            if (ds != '') {
                ds = JSON.parse(ds);
                if (ds.Records != undefined && ds.Records != "" && ds.Records != null) {
                    BindReceiveSheetFields(ds.Records);
                }
                else {
                    if (typingStarted == 0)
                    {
                       // reset();
                        RebindGrid();
                    }
                                       
                }

            }
            if (ds.Result == "OK") {
                
                return ds.Records;
            }
            if (ds.Result == "ERROR") {
                alert(ds.Message);
            }
        }

    }
    catch (e) {
        notyAlert('error', e.message);
    }
}
//--------------------button actions ----------------------
function List() {
    try {
      
        ChangeButtonPatchView('ReceiveFromTechnician', 'btnPatchReceiveFromTechnicianSettab', 'List');
        var empID = $("#ddlReceiveListTech").val();
        var fromDate = $("#fromDate").val();
        var toDate = $("#toDate").val();
        if (empID != "") {
            if (toDate != "" || fromDate != "") {
                BindAllReceiveList();
            }
           
        }

        // DataTables.eventTable.clear().rows.add(GetAllForm8()).draw(false);
    } catch (x) {
        // alert(x);
    }

}

function RebindGrid() {
    EG_ClearTable();
    EG_AddBlankRows(5)
}

function reset() {
    ClearControls();
    RebindGrid();
    typingStarted = 0;
}
function ClearControls() {
   
  
        $("#HiddenEmpID").val("");
        var $datepicker = $('#ReceiveDate');
        $datepicker.datepicker('setDate', null);
        $("#ddlReceiveListTech").val("All");
    
  
}
function Add() {

    ChangeButtonPatchView('ReceiveFromTechnician', 'btnPatchReceiveFromTechnicianSettab', 'Add');
    var empID = $("#ddlReceiveListTech").val();
    var transferDate = $("#ReceiveDate").val();
    if (empID != "" && transferDate != "") {
        GetReceiveSheetsByTechnician();
    }
    else {
        EG_ClearTable();
        EG_AddBlankRows(5)
    }
}



function ExportData() {
    try {

        $(".buttons-excel").trigger('click');
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}
