<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="scripts" ContentPlaceHolderID="Head" runat="server">
    <link href="../../Content/amcharts/style.css" rel="stylesheet" />
    <script src="../../Scripts/amcharts/amcharts.js"></script>
    <script src="../../Scripts/amcharts/serial.js"></script>
    <script type="text/javascript">
     var chart;
     var chartData = [
                {
                    "year": "1950",
                    "value": 100
                },
                {
                    "year": "1951",
                    "value": 584
                },
                {
                    "year": "1952",
                    "value": 252
                },
                {
                    "year": "1953",
                    "value": 484
                },
                {
                    "year": "1954",
                    "value": 784
                },
                {
                    "year": "1955",
                    "value": 278
                },
                {
                    "year": "1956",
                    "value": 151
                },
                {
                    "year": "1957",
                    "value": 594
                },
                {
                    "year": "1958",
                    "value": 999
                },
                {
                    "year": "1959",
                    "value": 218
                },
                {
                    "year": "1960",
                    "value": 148
                },
                {
                    "year": "1961",
                    "value": 364
                },
                {
                    "year": "1962",
                    "value": 221
                },
                {
                    "year": "1963",
                    "value": 498
                },
                {
                    "year": "1964",
                    "value": 845
                },
                {
                    "year": "1965",
                    "value": 259
                },
                {
                    "year": "1966",
                    "value": 184
                },
                {
                    "year": "1967",
                    "value": 1520
                },
                {
                    "year": "1968",
                    "value": 598
                }]; 
     var average = 90.4;

     AmCharts.ready(function () {

         // SERIAL CHART
         chart = new AmCharts.AmSerialChart();
         chart.pathToImages = "../../Images/images/";
         chart.dataProvider = chartData;
         chart.categoryField = "year";
         chart.dataDateFormat = "YYYY";

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
         graph.title = "Values";
         graph.valueField = "value";
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
</asp:Content>

<asp:Content ID="Title" ContentPlaceHolderID="TitleContent" runat="server">
Component Reliability Trend
</asp:Content>

<asp:Content ID="Content" ContentPlaceHolderID="FeaturedContent" runat="server">
    <h1>Component Reliability Trend</h1>
    <div id="selectors" class="small-12 small-centered medium-12 medium-centered large-centered large-12">
        <form id="form1" runat="server">
            <div id="SelectDatacenter">
                <p>Datacenter</p>
       
                <asp:DropDownList ID="Datacenter" runat="server" DataSourceID="SqlDataSource4" DataTextField="DataCenter" DataValueField="DataCenter">
                    <asp:ListItem Selected="True">All</asp:ListItem>
                </asp:DropDownList>
                <asp:SqlDataSource runat="server" ID="SqlDataSource4" ConnectionString="Data Source=FIDEL3127;Initial Catalog=VisDataTestCOSMOS;User ID=dataUser;Password=userData!" ProviderName="System.Data.SqlClient" SelectCommand="SELECT DISTINCT [DataCenter] FROM [DataCenter]"></asp:SqlDataSource>
            </div>
            <div id="SelectNetwork">
                <p>Network ID</p>
       
                <asp:DropDownList ID="Network" runat="server" DataSourceID="SqlDataSource2" DataTextField="NetworkId" DataValueField="NetworkId">
                    <asp:ListItem Selected="True" Value="0">All</asp:ListItem>
                </asp:DropDownList>
                <asp:SqlDataSource runat="server" ID="SqlDataSource2" ConnectionString="Data Source=FIDEL3127;Initial Catalog=VisDataTestCOSMOS;User ID=dataUser;Password=userData!" ProviderName="System.Data.SqlClient" SelectCommand="SELECT DISTINCT [NetworkId] FROM [DataCenterNetworkId]"></asp:SqlDataSource>
            </div>
            <div id="SelectFarm">
                <p>Farm ID</p>
       
                <asp:DropDownList ID="Farm" runat="server" DataSourceID="SqlDataSource1" DataTextField="FarmId" DataValueField="FarmId">
                    <asp:ListItem Selected="True" Value="0">All</asp:ListItem>
                </asp:DropDownList>
                <asp:SqlDataSource runat="server" ID="SqlDataSource1" ConnectionString="Data Source=FIDEL3127;Initial Catalog=VisDataTestCOSMOS;User ID=dataUser;Password=userData!" ProviderName="System.Data.SqlClient" SelectCommand="SELECT DISTINCT [FarmID] FROM [ProdDollar_TagAggregation]"></asp:SqlDataSource>
            </div>
            <div id="Entry">
            </div>
        </form>
    </div>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <div id="chartdiv" style="width: 95%; height: 70%;"></div>
</asp:Content>