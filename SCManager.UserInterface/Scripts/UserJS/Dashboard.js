﻿var dataDonut;

$(document).ready(function () {
    //---------------------------------bar chart weekly summary -------------------------
    var data = {
        labels: ["Feb-W3", "Feb-W4", "Mar-W1", "Mar-W2", "Mar-W3", "Mar-W4", "Apr-W1"],
        datasets: [

            {
                fillColor: "rgba(151,187,205,0.5)",
                strokeColor: "rgba(151,187,205,1)",
                pointColor: "rgba(151,187,205,1)",
                pointStrokeColor: "#fff",
                data: [50, 30, 40, 19, 96, 27, 80]
            }
        ]
    }

    var options = {
        animation: true
    };

    //Get the context of the canvas element we want to select
    var c = $('#myChart');
    var ct = c.get(0).getContext('2d');
    var ctx = document.getElementById("myChart").getContext("2d");
    /*********************/
    new Chart(ctx).Bar(data, options);



//---------------------------------doughnut  stock value summary ------------------------
//    var dataDonut = [
//{
//    value: 38,
//    color: "rgba(151,250,205,0.6)",
//    label: "TL Spares"
//},
//{
//    value: 27,
//    color: "rgba(151,220,205,0.6)",
//    label: "FL Spares"
//},
//{
//    value: 10,
//    color: "rgba(151,180,205,0.6)",
//    label: "Additives"
//},
//{
//    value:25,
//    color: "rgba(151,160,205,0.6)",
//    label: "Accessories"
//},
  
//      ]
     dataDonut = GetStockValueSummary();
      var options1 =
      {
          
          animation: true,
          tooltipTemplate: function (V) { return getDonutAmount(V.label,V.value) },
          tooltipFillColor: "rgba(255,255,255,.89)",
          tooltipFontColor: "rgba(1,1,1,1)",
          tooltipCaretSize: 0,
          tooltipFontSize: 14,
          tooltipFontStyle: "thick",
         
         // tooltipEvents: [],
         // onAnimationComplete: function () { this.showTooltip(this.segments, true); }
      }

      var ctx1 = document.getElementById("myChartPie").getContext("2d");
      var myPieChart = new Chart(ctx1).Doughnut(dataDonut, options1);
    
      $('#Legend1').html('<br/>'+myPieChart.generateLegend());

      $('.rlist').fadeIn('slow');
     // $('.techitem').fadeIn('slow');

});


function GetStockValueSummary() {
    var ds = {};
    var data = { "value": "" };
    ds = GetDataFromServer("DynamicUI/GetStockValueSummary/", data);
   
    if (ds != '') {
        ds = JSON.parse(ds);
    }
    if (ds.Result == "OK") {
        if (ds.Records != null) {
            $('#TotalStockValue').html(ds.Records[0].totalValueConverted);
        }
        return ds.Records;
    }
    if (ds.Result == "ERROR") {
        return null;
    }


}



function getDonutAmount(l, v) {
    for (i = 0; i < dataDonut.length; i++) {
        if (dataDonut[i].label == l && dataDonut[i].value == v) {
            return v + '% ' + l + " :" + dataDonut[i].AmountConverted;
        }
    }
    return l + ' ' + v + '%';

}