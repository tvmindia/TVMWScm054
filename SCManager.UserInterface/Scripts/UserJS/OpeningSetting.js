var DataTables = {};
var emptyGUID = '00000000-0000-0000-0000-000000000000'
var _Materials = [];
//tblopenEdit
//tblOpen

//---------------------------------------Docuement Ready--------------------------------------------------//
$(document).ready(function () {
    try {

        var EventRequestsViewModel = new Object();
        DataTables.ListTable = $('#tblOpen').DataTable(
         {
             dom: '<"pull-left"f>rt<"bottom"ip><"clear">',
             order: [],
             searching: true,
             paging: true,
             data: null,
             columns: [
               { "data": "SlNo" },
               { "data": "Material" },
                { "data": "MaterialDescription" },
               { "data": "Quantity" },
               { "data": "UOM" }           
             ],
             columnDefs: [                    
             { className: "text-center disabled", "targets": [0, 1, 3, 4] },
             { className: "disabled", "targets": [2] }
             ]
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

        notyAlert('error', x.message);

    }

});


//-----------------------EDIT GRID DEFN-------------------------------------
var EG_totalDetailRows = 0;
var EG_GridData;//DATA SOURCE OBJ ARRAY
var EG_GridDataTable;//DATA TABLE ITSELF FOR REBIND PURPOSE
var EG_SlColumn = 'SlNo';
var EG_GridInputPerRow = 2;
var EG_MandatoryFields = 'MaterialID,Quantity'

function EG_TableDefn() {

    var tempObj = new Object();
    tempObj.SCCode = "";
    tempObj.ID = "";
    tempObj.MaterialID = "";
    tempObj.SlNo = 0;
    tempObj.Material = "";
    tempObj.MaterialDescription = "";
    tempObj.Quantity = "";
    tempObj.UOM = "";   

    return tempObj;
}

function EG_Columns() {
    var obj = [
                { "data": "SCCode", "defaultContent": "<i></i>" },
                { "data": "ID", "defaultContent": "<i>0</i>" },
                { "data": "MaterialID", "defaultContent": "<i></i>" },
                { "data": "SlNo", "defaultContent": "<i></i>" },
                { "data": "Material", render: function (data, type, row) { return (EG_createCombo(data, 'S', row, 'Material', 'Materials', 'FillUOM')); } },
                { "data": "MaterialDescription", "defaultContent": "<i></i>" },
                { "data": "Quantity", render: function (data, type, row) { return (EG_createTextBox(data, 'N', row, 'Quantity', '')); }, "defaultContent": "<i></i>" },
                { "data": "UOM", "defaultContent": "<i></i>" },             
                { "data": null, "orderable": false, "defaultContent": '<a href="#" class="DeleteLink"  onclick="DeleteItem(this)" ><i class="glyphicon glyphicon-trash" aria-hidden="true"></i></a>' }

    ]

    return obj;

}

function EG_Columns_Settings() {

    var obj = [
         { "width": "5%", "targets": 3 },
        { "width": "50%", "targets": 5 },
         { "width": "10%", "targets": 6 },
        { "targets": [0], "visible": false, "searchable": false }, { "targets": [1], "visible": false, "searchable": false }, { "targets": [2], "visible": false, "searchable": false },            
        { className: "text-center", "targets": [3, 4,6,8] },      
        { className: "text-center disabled", "targets": [7] },
        { className: "disabled", "targets": [5] },
        { "orderable": false, "targets": [0, 1, 2, 3, 4, 5, 6, 7,8] }

    ]

    return obj;

}

//------------------------------------------------------------------

//---------------Bind logics-------------------
function GetAllOpening() {
    try {

        var data = {};
        var ds = {};
        ds = GetDataFromServer("OpeningSetting/GetOpenings/", data);
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



function BindOpeningFields(Records) {
    try {

        $('#HeaderID').val(Records.ID);
        $('#Cash').val(Records.Cash);
        $('#Bank').val(Records.Bank);

        $('#CashLbl').html(Records.CashFormatted);
        $('#BankLbl').html(Records.BankFormatted);


        if (Records.WithEffectDate != null) {
            $('#WithEffectDateLbl').html(Records.WithEffectDateFormatted);
            $('#WithEffectDate').val((Records.WithEffectDate));
            //$datepicker = $('#WithEffectDate').val((Records.WithEffectDate));
            //$datepicker.datepicker('setDate', new Date(Records.WithEffectDate));
        }




    } catch (e) {
        notyAlert('error', e.message);
    }
}
   
 


//--------------------button actions ----------------------
function List() {
    try {
         
        ChangeButtonPatchView('OpeningSetting', 'btnPatchAttributeSettab', 'List');
        var result =GetAllOpening();
        BindOpeningFields(result);
        DataTables.ListTable.clear().rows.add(result.OpeningDetails).draw(false);
        
    } catch (x) {
        // alert(x);
    }

}

function Add() {

     
    ChangeButtonPatchView('OpeningSetting', 'btnPatchAttributeSettab', 'Save');
    EG_ClearTable();
    BindOpening();
  

    
}

function BindOpening() {

    var result = GetAllOpening();
    if (result == null || result.length == 0) {
        $('#HeaderID').val(emptyGUID);
        EG_AddBlankRows(5)
    }
    else {
        BindOpeningFields(result);
        EG_Rebind_WithData(result.OpeningDetails, 1);

    }
}

function DeleteItem(currentObj) {
    var rowData = EG_GridDataTable.row($(currentObj).parents('tr')).data();

    if ((rowData != null) && (rowData.ID != null)) {
        notyConfirm('Are you sure to delete?', 'OpeningDetailDelete("' + rowData.ID + '",' + rowData[EG_SlColumn] + ')');
    }
}


function OpeningDetailDelete(id, rw) {
    try {
        var Hid = $('#HeaderID').val();
        if (id != '' && id != null && Hid != '' && Hid != null && Hid != emptyGUID) {
            var data = { "ID": id, "HeaderID": Hid };
            var ds = {};
            ds = GetDataFromServer("OpeningSetting/DeleteOpeningDetail/", data);
            if (ds != '') {
                ds = JSON.parse(ds);
            }
            if (ds.Result == "OK") {
                notyAlert('success', ds.Message);
                BindOpening();
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

function reset() {
    EG_ClearTable();
    EG_AddBlankRows(5)
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


function SaveSuccess(data, status, xhr) {
    var i = JSON.parse(data)
    switch (i.Result) {
        case "OK":
            notyAlert('success', i.Message);
            BindOpening();
            ChangeButtonPatchView('OpeningSetting', 'btnPatchAttributeSettab', 'Save');

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


function FillUOM(row) {

    for (i = 0; i < _Materials.length; i++) {
        if (_Materials[i].ItemCode == EG_GridData[row - 1]['Material']) {
            EG_GridData[row - 1]['UOM'] = _Materials[i].UOM;
            EG_GridData[row - 1]['MaterialID'] = _Materials[i].ID;
            EG_GridData[row - 1]['MaterialDescription'] = _Materials[i].Description;
            EG_Rebind();
            break;
        }
    }

}