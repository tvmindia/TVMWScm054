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
    var dataDonut = [
{
    value: 38,
    color: "rgba(151,250,205,0.6)",
    label: "TL Spares"
},
{
    value: 27,
    color: "rgba(151,220,205,0.6)",
    label: "FL Spares"
},
{
    value: 10,
    color: "rgba(151,180,205,0.6)",
    label: "Additives"
},
{
    value:25,
    color: "rgba(151,160,205,0.6)",
    label: "Accessories"
},
  
      ]

      var options1 =
      {
          animation: true,
          tooltipTemplate: "<%= label%>-<%= value%>%",
          tooltipFillColor: "rgba(0,0,0,0)",
          tooltipFontColor: "rgba(1,1,1,.7)",
          tooltipCaretSize: 0,
          tooltipFontStyle: "thin",

          tooltipEvents: [],
          onAnimationComplete: function () { this.showTooltip(this.segments, true); }
      }

      var ctx1 = document.getElementById("myChartPie").getContext("2d");
      var myPieChart = new Chart(ctx1).Doughnut(dataDonut, options1);
    
  
});