﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    PercentData
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="chartdiv"></div>
    <script>
        var bullets = ["round", "square", "triangleUp", "triangleDown", "triangleLeft", "triangleRight", "diamond", "xError", "yError"];
        var data = <%= Html.Raw(ViewBag.PercentData)%>;//generateChartData();
        //var second = generateChartData2();
        //var datasets = [data];
        //var chart = create(data);
        //createCharts(datasets);

        create(data);

        function create(chartData) {
            var chart = AmCharts.makeChart("chartdiv", {
                "type": "serial",
                "theme": "dark",
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
                    "useGraphSettings": true,
                    "color": "#0000FF",
                    "position": "left",
                    "width": 320,
                    "valueText": ""
                },
                "chartScrollbar": {
                    "autoGridCount": true,
                    "scrollbarHeight": 40
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

<asp:Content ID="Content" ContentPlaceHolderID="FeaturedContent" runat="server">
<h1>Component Reliability</h1>

    <a href="#" id="RawDataLink">View Raw Data</a>

    <div id="selectors" class="small-12 small-centered medium-12 medium-centered large-centered large-12">
        <form id="form1" runat="server">
            <div id="SelectDatacenter">
                <p>Datacenter</p>
       
                <asp:DropDownList ID="Datacenter" runat="server" DataSourceID="SqlDataSource4" DataTextField="DataCenter" DataValueField="DataCenter">
                    <asp:ListItem Selected="True">All  </asp:ListItem>
                </asp:DropDownList>
                <asp:SqlDataSource runat="server" ID="SqlDataSource4" ConnectionString="Data Source=FIDEL3127;Initial Catalog=VisDataTestCOSMOS;User ID=dataUser;Password=userData!" ProviderName="System.Data.SqlClient" SelectCommand="SELECT DISTINCT [DataCenter] FROM [DataCenter]"></asp:SqlDataSource>
            </div>
            <div id="SelectNetwork">
                <p>Network ID</p>
       
                <asp:DropDownList ID="Network" runat="server" DataSourceID="SqlDataSource2" DataTextField="NetworkId" DataValueField="NetworkId">
                    <asp:ListItem Selected="False" Value="-1">5</asp:ListItem>
                </asp:DropDownList>
                <asp:SqlDataSource runat="server" ID="SqlDataSource2" ConnectionString="Data Source=FIDEL3127;Initial Catalog=VisDataTestCOSMOS;User ID=dataUser;Password=userData!" ProviderName="System.Data.SqlClient" SelectCommand="SELECT DISTINCT [NetworkId] FROM [DataCenterNetworkId]"></asp:SqlDataSource>
            </div>
            <div id="SelectFarm">
                <p>Farm ID</p>
       
                <asp:DropDownList ID="Farm" runat="server" DataSourceID="SqlDataSource1" DataTextField="FarmId" DataValueField="FarmId">
                    <asp:ListItem Selected="False" Value="-1">5</asp:ListItem>
                </asp:DropDownList>
                <asp:SqlDataSource runat="server" ID="SqlDataSource1" ConnectionString="Data Source=FIDEL3127;Initial Catalog=VisDataTestCOSMOS;User ID=dataUser;Password=userData!" ProviderName="System.Data.SqlClient" SelectCommand="SELECT DISTINCT [FarmID] FROM [ProdDollar_TagAggregation]"></asp:SqlDataSource>
            </div>
            <div id="Entry">
            </div>
        </form>
    </div>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="Head" runat="server">
    <style>
        .amChartsLegend g text {
            text-decoration: underline;
        }

        #chartdiv {
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

        #RawDataLink {
            padding: 0;
        }
    </style>
    <script type="text/javascript" src="http://www.amcharts.com/lib/3/amcharts.js"></script>
    <script type="text/javascript" src="http://www.amcharts.com/lib/3/serial.js"></script>
    <script type="text/javascript" src="http://www.amcharts.com/lib/3/themes/dark.js"></script>
</asp:Content>
