<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="scripts" ContentPlaceHolderID="Head" runat="server">
        <link href="../../Content/amcharts/ammap.css" rel="stylesheet" />
        <script src="../../Scripts/amcharts/ammap.js"></script>
        <!-- map file should be included after ammap.js -->
        <script src="../../Scripts/amcharts/worldLow.js"></script>
        <script src="../../Scripts/amcharts/themes/black.js"></script>
     <script type="text/javascript">
         /*
             although ammap has methos like getAreaCenterLatitude and getAreaCenterLongitude,
             they are not suitable in quite a lot of cases as the center of some countries
             is even outside the country itself (like US, because of Alaska and Hawaii)
             That's why wehave the coordinates stored here
         */
		 
     	var dataDCLatLong = <%= Html.Raw(ViewBag.Index)%>;
     	var latlong = {};
         latlong["ZW"] = { "latitude": -20, "longitude": 30 };

         var mapData = [
             { "code": "AF", "name": "Afghanistan", "value": 32358260, "color": "#eea638", "SVGTextElement": "Test!" },
             { "code": "AL", "name": "Albania", "value": 3215988, "color": "#d8854f", "text": "Test!" },
             { "code": "DZ", "name": "Algeria", "value": 35980193, "color": "#de4c4f", "text": "Test!" },
             { "code": "AO", "name": "Angola", "value": 19618432, "color": "#de4c4f", "text": "Test!" },
             { "code": "AR", "name": "Argentina", "value": 40764561, "color": "#86a965", "text": "Test!" },
             { "code": "AM", "name": "Armenia", "value": 3100236, "color": "#d8854f", "text": "Test!" },
             { "code": "US", "name": "United States", "value": 22605732, "color": "#8aabb0", "text": "Test!" },
             { "code": "VG", "name": "Somewhere!", "value": 2, "color": "#8aabb0", "text": "Test!" } ];


         var map;
         var min = Infinity;
         var max = -Infinity;

         AmCharts.theme = AmCharts.themes.black;

         // build map
         AmCharts.ready(function () {
             map = new AmCharts.AmMap();
             map.pathToImages = "../../Images/images/";

             map.areasSettings = {
                 unlistedAreasColor: "#FFFFFF",
                 unlistedAreasAlpha: 0.1
             };
             map.imagesSettings = {
                 balloonText: "<span style='font-size:14px;'><b>[[title]]</b>: [[value]]</span>",
                 alpha: 0.6
             }

             var dataProvider = {
                 mapVar: AmCharts.maps.worldLow,
                 images: []
             }

             // create circle for each country
             for (var i = 0; i < mapData.length; i++) {
                 var dataItem = mapData[i];
                 var value = dataItem.value;
                 // calculate size of a bubble
                 var id = dataItem.code;

                 dataProvider.images.push({
                     type: "circle",
                     width: 50,
                     height: 50,
                     color: dataItem.color,
                     longitude: latlong[id].longitude,
                     latitude: latlong[id].latitude,
                     title: dataItem.name,
                     value: value,
                     text: SVGTextElement
                 });
             }

             map.dataProvider = dataProvider;

             map.write("mapdiv");
         });
		</script>
</asp:Content>

<asp:Content ID="aboutTitle" ContentPlaceHolderID="TitleContent" runat="server">
World Heat Map
</asp:Content>

<asp:Content ID="aboutContent" ContentPlaceHolderID="MainContent" runat="server">
    <h1>World Heat Map</h1>
    <div id="mapdiv" style="width: 100%; height: 72%;"></div>
    <div id ="legendBar" class="small-12 medium-12 large-12 columns">
        <h1>Legend Bar</h1>
    </div>
</asp:Content>