var DataTables = {};
var editor;
var totalDetailRows = 0;
var GridData;
var identityColumn = 'SlNo';
var GridInputPerRow=7;
//---------------------------------------Docuement Ready--------------------------------------------------//
$(document).ready(function () {
    try {
     

        DataTables.DetailTable = $('#tblInvDetails').DataTable(
          {
              dom: '<"pull-left"f>rt<"bottom"ip><"clear">',
              order: [],
              searching: false,
              paging: false,
              data: GetGridDate(),
              columns: [
                { "data": "SCCode", "defaultContent": "<i></i>" },
                { "data": "ID", "defaultContent": "<i>0</i>" },
                { "data": "SlNo", "defaultContent": "<i></i>" },
                { "data": "Material", render: function (data, type, row) { return (createTextBox(data, type, row, 'Material')); } },
                { "data": "Quantity", render: function (data, type, row) { return (createTextBox(data, type, row, 'Quantity')); }, "defaultContent": "<i></i>" },
                { "data": "UOM", render: function (data, type, row) { return (createTextBox(data, type, row, 'UOM')); }, "defaultContent": "<i></i>" },
                { "data": "Rate", render: function (data, type, row) { return (createTextBox(data, type, row, 'Rate')); }, "defaultContent": "<i></i>" },
                { "data": "BasicAmount", render: function (data, type, row) { return (createTextBox(data, type, row, 'BasicAmount')); }, "defaultContent": "<i></i>" },
                { "data": "TradeDiscount", render: function (data, type, row) { return (createTextBox(data, type, row, 'TradeDiscount')); }, "defaultContent": "<i></i>" },
                { "data": "NetAmount", render: function (data, type, row) { return (createTextBox(data, type, row, 'NetAmount')); }, "defaultContent": "<i></i>" }
              ],
              columnDefs: [{ "targets": [0], "visible": false, "searchable": false }, { "targets": [1], "visible": false, "searchable": false },
                  { "targets": [3], "width": "20%" },
                   { className: "text-right", "targets": [6, 7, 8, 9] },
              { className: "text-center", "targets": [2, 3, 4, 5] },



              ],
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


       
      
    
    }catch(x){}

});



function blankRow(count) {

    var dataObj = [];
    for (i = 0; i < count; i++) {

        var tempObj = new Object();
        tempObj.SCCode = "";
        tempObj.ID = "";
        tempObj.SlNo = totalDetailRows + i + 1;
        tempObj.Material = "";
        tempObj.Quantity = "";
        tempObj.UOM = "";
        tempObj.Rate = "";
        tempObj.BasicAmount = "";
        tempObj.TradeDiscount = "";
        tempObj.NetAmount = "";
        dataObj.push(tempObj);

    }

    totalDetailRows = totalDetailRows + count;
    return dataObj;

}

function GetGridDate() {

    GridData = blankRow(20);
    return GridData;
}

function changeData(value,row,column) {
   // debugger;
  
    for (i = 0; i < GridData.length; i++)
    {        
        if (GridData[i][identityColumn] == row)
        {
            GridData[i][column] = value;
            break;
        }
    }
   
}

function createTextBox(data, type, row,columnname) {
    
    var a = row[columnname];
    var b = row.SlNo;
    var c = "'";
   // debugger;
    if (data=="" || data == null) {
       
        return ('<input type="textbox" class="gridTextbox" value="" onblur="changeData(this.value,' + b + ',' + c + columnname + c + ')" >  </input>');
    } else {
        return ('<input type="textbox" class="gridTextbox" value=' + data + ' onblur="changeData(this.value,' + b + ',' + c + columnname + c + ')" >  </input>');
    }
   

}

