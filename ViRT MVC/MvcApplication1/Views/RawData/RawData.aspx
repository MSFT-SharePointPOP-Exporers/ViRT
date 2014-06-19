<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <link href="../../Content/Raw%20Data.css" rel="stylesheet" />
             <style>
        .amChartsLegend g text {
            text-decoration: underline;
        }

        #chartdiv .newchart {
            width: 100%;
            height: 500px;
        }

        rect {
            text-decoration: none;
        }
    </style>
    <script type="text/javascript" src="http://www.amcharts.com/lib/3/amcharts.js"></script>
    <script type="text/javascript" src="http://www.amcharts.com/lib/3/serial.js"></script>
    <script type="text/javascript" src="http://www.amcharts.com/lib/3/themes/none.js"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="TitleContent" runat="server">
    RawData
</asp:Content>

<asp:Content ID="Content" ContentPlaceHolderID="FeaturedContent" runat="server">
<h1>RawData</h1>
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

<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">
    <div id="chartdiv" style="width: 100%; height: 70%;"></div>
    <script>
        var bullets = ["round", "square", "triangleUp", "triangleDown", "triangleLeft", "triangleRight", "diamond", "xError", "yError"];
        var data = <%= Html.Raw(ViewBag.RawData)%>;//generateChartData();
        createCharts(data);

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
                    "id": "v1",
                    "position": "left",
                    "axisColor": "blue",
                    "axisThickness": 2,
                    "gridAlpha": 0,
                    "axisAlpha": 1

                }, {
                    "id": "v2",
                    "position": "right",
                    "axisColor": "green",
                    "axisThickness": 2,
                    "gridAlpha": 0,
                    "axisAlpha": 1
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

                    if( i ==0)
                    {
                        graph1.valueAxis = "v1";
                        graph1.lineColor = "blue";
                    }
                        
                    if( i ==1)
                    {
                        graph1.valueAxis = "v2";
                        graph1.lineColor = "green";
                    }
                        

                    chart.addGraph(graph1);
                    i++;
                }
            }
            return chart;
        }

    </script>
</asp:Content>


