<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FetchCustomer.aspx.cs" Inherits="FetchCustomer" %>

<!DOCTYPE html> 
<html>
<head runat="server">
    <title></title>
    <link href="Content/style.css" rel="stylesheet" />
    <script src="Scripts/amcharts/amcharts.js"></script>
    <script src="Scripts/amcharts/serial.js"></script>
    <script type="text/javascript">
     var chart;
     var chartData = <%=sjson%>;
     var average = 90.4;

     AmCharts.ready(function () {

         // SERIAL CHART
         chart = new AmCharts.AmSerialChart();
         chart.pathToImages = "App_LocalResources/";
         chart.dataProvider = chartData;
         chart.categoryField = "Date";
         chart.dataDateFormat = "YYYY-MM-DD";

         // AXES
         // category
         var categoryAxis = chart.categoryAxis;
         categoryAxis.parseDates = true; // as our data is date-based, we set parseDates to true
         categoryAxis.minPeriod = "DD"; // our data is daily, so we set minPeriod to DD
         categoryAxis.dashLength = 1;
         categoryAxis.gridAlpha = 0.15;
         categoryAxis.axisColor = "#DADADA";

         // value
         var valueAxis = new AmCharts.ValueAxis();
         valueAxis.axisColor = "#DADADA";
         valueAxis.dashLength = 1;
         valueAxis.logarithmic = true; // this line makes axis logarithmic
         chart.addValueAxis(valueAxis);

         // GUIDE for average
         var guide = new AmCharts.Guide();
         guide.value = average;
         guide.lineColor = "#CC0000";
         guide.dashLength = 4;
         guide.label = "average";
         guide.inside = true;
         guide.lineAlpha = 1;
         valueAxis.addGuide(guide);


         // GRAPH
         var graph = new AmCharts.AmGraph();
         graph.type = "smoothedLine";
         graph.bullet = "round";
         graph.bulletColor = "#FFFFFF";
         graph.useLineColorForBulletBorder = true;
         graph.bulletBorderAlpha = 1;
         graph.bulletBorderThickness = 2;
         graph.bulletSize = 7;
         graph.title = "Hits";
         graph.valueField = "NumberOfHits";
         graph.lineThickness = 2;
         graph.lineColor = "#00BBCC";
         chart.addGraph(graph);

         // CURSOR
         var chartCursor = new AmCharts.ChartCursor();
         chartCursor.cursorPosition = "mouse";
         chart.addChartCursor(chartCursor);

         // SCROLLBAR
         var chartScrollbar = new AmCharts.ChartScrollbar();
         chartScrollbar.graph = graph;
         chartScrollbar.scrollbarHeight = 30;
         chart.addChartScrollbar(chartScrollbar);

         chart.creditsPosition = "bottom-right";

         // WRITE
         chart.write("chartdiv");
     });
    </script>
</head>
<body>
    <form id="form1" runat="server">
      
          >
    </form>
</body>
</html>
