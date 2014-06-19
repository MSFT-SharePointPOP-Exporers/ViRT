<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    PercentData
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<h2>PercentData</h2>
    <div id="chartdiv"></div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="Head" runat="server">
    <style>
        .amChartsLegend g text {
            text-decoration: underline;
        }

        #chartdiv .newchart{
            width: 100%;
            height: 500px;
        }

        body {
            margin: 0;
            padding: 0;
            width: 100%;
        }

        rect {
            text-decoration: none;
        }
    </style>
        <script type="text/javascript" src="http://www.amcharts.com/lib/3/amcharts.js"></script>
    <script type="text/javascript" src="http://www.amcharts.com/lib/3/serial.js"></script>
    <script type="text/javascript" src="http://www.amcharts.com/lib/3/themes/none.js"></script>
    <script>
        var bullets = ["round", "square", "triangleUp", "triangleDown", "triangleLeft", "triangleRight", "diamond", "xError", "yError"];
        var data = <%= Html.Raw(ViewBag.PercentData)%>;//generateChartData();
        //var second = generateChartData2();
        var datasets = [data];
        //var chart = create(data);
        createCharts(datasets);

        function createCharts(datasets) {
            for (var i = 0; i < datasets.length; i++) {
                var div = document.createElement("div");
                div.className = "newchart";
                document.getElementById("chartdiv").appendChild(div);
                var object = datasets[i];
                var chart = create(object, div);
            }
        }

        function create(chartData, newdiv) {
            var chart = AmCharts.makeChart(newdiv, {
                "type": "serial",
                "theme": "none",
                "pathToImages": "http://www.amcharts.com/lib/3/images/",
                "dataProvider": chartData,
                "valueAxes": [{
                    "dashLength": 1,
                    "position": "left"
                }],
                "legend": {
                    "title": "Select Component:",
                    "font": 40,
                    "labelText": "[[title]]",
                    "titlePosition": "top",
                    "useGraphSettings": true,
                    "color": "#0000FF",
                    "position": "left",
                    "width": 320,
                    "valueText": ""
                },
                "chartScrollbar": {
                    "autoGridCount": true,
                    "scrollbarHeight": 40,
                    "color": "#000000"
                },
                "chartCursor": {
                    "cursorPosition": "mouse",
                    "categoryBalloonDateFormat": "MMM DD, YYYY \nJJ:NN"
                },
                "dataDateFormat": "YYYY-MM-DDTJJ:NN:SS",
                "categoryField": "Date",
                "categoryAxis": {
                    "parseDates": true,
                    "minPeriod": "hh"
                }
            });

            var i = 0;
            for (var propertyName in chartData[0]) {
                if (propertyName != 'Date') {
                    if (i == 9)
                        i = 0;
                    var graph1 = new AmCharts.AmGraph();
                    graph1.type = "line";
                    graph1.valueField = propertyName;
                    graph1.balloonText = "<b><span style='font-size:14px;'>[[title]]</span></b><br />[[category]]<br /><span style='font-size:14px;'>Reliability: [[value]]</span>";
                    graph1.title = propertyName;
                    graph1.bullet = bullets[i];
                    graph1.bulletSize = 10;
                    graph1.hideBulletsCount = 30;
                    graph1.connect = false;
                    chart.addGraph(graph1);
                    i++;
                }
            }


            return chart;
        }

    </script>
</asp:Content>
