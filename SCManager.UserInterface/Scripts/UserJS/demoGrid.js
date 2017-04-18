var DataTables = {};
var editor;
var availableTags = [
     "ActionScript",
     "AppleScript",
     "Asp",
     "BASIC",
     "C",
     "C++",
     "Clojure",
     "COBOL",
     "ColdFusion",
     "Erlang",
     "Fortran",
     "Groovy",
     "Haskell",
     "Java",
     "JavaScript",
     "Lisp",
     "Perl",
     "PHP",
     "Python",
     "Ruby",
     "Scala",
     "Scheme"
];

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
              columnDefs: EG_Columns_Settings(),
              keys: {
                    columns: ':not(:first-child)',
                    editor:  editor
                    },
              select:{
                    style:    'os',
                    selector: 'td:first-child',
                    blurable: true
                    }
          });
       
        var a = getMaterials();        
        EG_ComboSource('Materials',a)
    
    }catch(x){}

});

//-----------------------EDIT GRID DEFN-------------------------------------
var EG_totalDetailRows = 0;
var EG_GridData;
var EG_SlColumn = 'SlNo';
var EG_GridInputPerRow = 7;

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
                { "data": "Material", render: function (data, type, row) { return (EG_createCombo(data, type, row, 'Material', 'Materials')); } },
                { "data": "Quantity", render: function (data, type, row) { return (EG_createTextBox(data, type, row, 'Quantity')); }, "defaultContent": "<i></i>" },
                { "data": "UOM", render: function (data, type, row) { return (EG_createTextBox(data, type, row, 'UOM')); }, "defaultContent": "<i></i>" },
                { "data": "Rate", render: function (data, type, row) { return (EG_createTextBox(data, type, row, 'Rate')); }, "defaultContent": "<i></i>" },
                { "data": "BasicAmount", render: function (data, type, row) { return (EG_createTextBox(data, type, row, 'BasicAmount')); }, "defaultContent": "<i></i>" },
                { "data": "TradeDiscount", render: function (data, type, row) { return (EG_createTextBox(data, type, row, 'TradeDiscount')); }, "defaultContent": "<i></i>" },
                { "data": "NetAmount", render: function (data, type, row) { return (EG_createTextBox(data, type, row, 'NetAmount')); }, "defaultContent": "<i></i>" }
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

    var mymats = new Array();
    mymats[0] = 'bush';
    mymats[1] = 'door';
    mymats[2] = 'clamp';
    mymats[3] = 'clip';
    mymats[4] = 'additive';
    mymats[5] = 'liquid';
    mymats[6] = 'drum';
    mymats[7] = 'lock';
    mymats[8] = 'mat';
    mymats[9] = 'footer';
    mymats[10] = 'badge';
    mymats[11] = 'plug';

    return mymats;
}

