var DataTables = {};
 
var _Materials=[];
 
//---------------------------------------Docuement Ready--------------------------------------------------//
$(document).ready(function () {
    try {
     

        DataTables.DetailTable = $('#tblInvDetails').DataTable(
          {
              dom: '<"pull-left"f>rt<"bottom"ip><"clear">',
              order: [],
              searching: false,
              paging: false,
              data: GetGridData(),
              columns: EG_Columns(),
              columnDefs: EG_Columns_Settings()
             
          });
       
        getMaterials();
        EG_ComboSource('Materials', _Materials, 'ItemCode','Description')
        EG_GridDataTable = DataTables.DetailTable;
    
    }catch(x){}

});

//-----------------------EDIT GRID DEFN-------------------------------------
var EG_totalDetailRows = 0;
var EG_GridData;
var EG_GridDataTable;
var EG_SlColumn = 'SlNo';
var EG_GridInputPerRow = 4;

function EG_TableDefn() {

    var tempObj = new Object();
    tempObj.SCCode = "";
    tempObj.ID = "";
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
    var obj=[
                { "data": "SCCode", "defaultContent": "<i></i>" },
                { "data": "ID", "defaultContent": "<i>0</i>" },
                { "data": "SlNo", "defaultContent": "<i></i>" },
                { "data": "Material", render: function (data, type, row) { return (EG_createCombo(data, 'S', row, 'Material', 'Materials','FillUOM')); } },
                { "data": "Quantity", render: function (data, type, row) { return (EG_createTextBox(data, 'N', row, 'Quantity', 'CalculateAmount')); }, "defaultContent": "<i></i>" },
                { "data": "UOM", "defaultContent": "<i></i>" },
                { "data": "Rate", render: function (data, type, row) { return (EG_createTextBox(data, 'F', row, 'Rate', 'CalculateAmount')); }, "defaultContent": "<i></i>" },
                { "data": "BasicAmount", "defaultContent": "<i></i>" },
                { "data": "TradeDiscount", render: function (data, type, row) { return (EG_createTextBox(data, 'F', row, 'TradeDiscount', 'CalculateAmount')); }, "defaultContent": "<i></i>" },
                { "data": "NetAmount",  "defaultContent": "<i></i>" }
    ]

    return obj

}

function EG_Columns_Settings() {

    var obj = [
        { "targets": [0], "visible": false, "searchable": false }, { "targets": [1], "visible": false, "searchable": false },
        { "targets": [3], "width": "20%" },
        { className: "text-right", "targets": [6, 7, 8, 9] },
        { className: "text-center", "targets": [2, 3, 4, 5] }

    ]

    return obj;

}

//------------------------------------------------------------------


function GetGridData() {

    EG_GridData = EG_blankRow(20);
    return EG_GridData;
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


 



function CalculateAmount(row) {
  
    //EG_GridData[row-1][Quantity] = value
    var qty = 0.00;
    var rate = 0.00;
    var dic = 0.00;

    var EGqty = '';
    var EGrate ='';
    var EGdic = '';

    EGqty = EG_GridData[row - 1]["Quantity"];
    EGrate = EG_GridData[row - 1]['Rate'];
    EGdic = EG_GridData[row - 1]['TradeDiscount'];

    qty = parseFloat(EGqty)||0;
    rate = parseFloat(EGrate) || 0;
    dic = parseFloat(EGdic) || 0;

    EG_GridData[row - 1]['Rate'] = roundoff(rate);
    EG_GridData[row - 1]['BasicAmount'] = roundoff(qty * rate);   
    EG_GridData[row - 1]['TradeDiscount'] = roundoff(dic);
    EG_GridData[row - 1]['NetAmount'] = roundoff(qty * rate - dic);
    EG_Rebind();
    
}

function FillUOM(row) {
  
    for (i = 0; i < _Materials.length; i++) {
        if (_Materials[i].ItemCode == EG_GridData[row - 1]['Material']) {
            EG_GridData[row - 1]['UOM'] = _Materials[i].UOM;
                EG_Rebind();
                break;            
        }
    }

}

