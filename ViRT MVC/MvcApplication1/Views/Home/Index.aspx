﻿<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="scripts" ContentPlaceHolderID="Head" runat="server">
        <link href="../../Content/amcharts/ammap.css" rel="stylesheet" />
        <script src="../../Scripts/amcharts/ammap.js"></script>
        <!-- map file should be included after ammap.js -->
        <script src="../../Scripts/amcharts/worldLow.js"></script>
        <script src="../../Scripts/amcharts/themes/black.js"></script>
     
</asp:Content>

<asp:Content ID="aboutTitle" ContentPlaceHolderID="TitleContent" runat="server">
World Heat Map
</asp:Content>

<asp:Content ID="aboutContent" ContentPlaceHolderID="MainContent" runat="server">
    <h1>World Heat Map</h1>
    <div id="mapdiv" style="width: 100%; height: 72%;">
		<script>
			/*
				although ammap has methos like getAreaCenterLatitude and getAreaCenterLongitude,
				they are not suitable in quite a lot of cases as the center of some countries
				is even outside the country itself (like US, because of Alaska and Hawaii)
				That's why wehave the coordinates stored here
			*/

			
			var latlong = {};
			var mapData = new Array();
			var dataDCLatLong = <%= Html.Raw(ViewBag.WorldMap)%>;
			var averageDCPer = <%= Html.Raw(ViewBag.AverageDCPercent)%>;
			//var pipe = <%= Html.Raw(ViewBag.Pipeline)%>;

			for(var i = 0; i < dataDCLatLong.length; i++)
			{
				var obj = dataDCLatLong[i];
				var objName = obj["DataCenter"];
				latlong[objName] = {"latitude":obj["latitude"], "longitude":obj["longitude"]};
				
				var col = "#000000";
				var objVal = averageDCPer[i];
				var per = objVal["Percent"];

				if(per > 99.90)	   {col = "$43ac6a";}
				else if(per > 99.0){col = "#ffff51";}
				else if(per > 95.0){col = "#c52e2e";}
				else if(per > 85.0){col = "#8f0202";}
				else			   {col = "#4e1010";}
				
				var toPush = { "code":objName, "color":col, "value": per};

				mapData.push(toPush)
			}	

			var map;

			AmCharts.theme = AmCharts.themes.black;

			// build map
			AmCharts.ready(function () {
				map = new AmCharts.AmMap();
				map.pathToImages = "../../Images/images/";
				map.panEventsEnabled = true;

				map.areasSettings = {
					unlistedAreasColor: "#FFFFFF",
					unlistedAreasAlpha: 0.1
				};
				map.imagesSettings = {
					alpha: 0.6
				}

				var dataProvider = {
					mapVar: AmCharts.maps.worldLow,
					images: []
				}

				// create circle for each country
				for (var i = 0; i < mapData.length; i++) {
					var dataItem = mapData[i];
					var val = dataItem.value;
					var id = dataItem.code;
					var des = "Reliability of Data Center: "
					des = des.concat(val);
					des = des.concat("</br></br><a href='../Home/DCHM' id='" + dataItem.code.substring(0,3) + "' onclick='setDatacenter(this.id)'>Detailed Data Center View</a>");
					dataProvider.images.push({
						label: id,
						labelPosition: "middle",
						labelShiftX: -15,
						labelShiftY: -17,
						type: "circle",
						percentWidth: 2,
						percentHeight: 2,
						color: dataItem.color,
						longitude: latlong[id].longitude,
						latitude: latlong[id].latitude,
						description: des,
						title: id
					});
				}


				map.dataProvider = dataProvider;

				map.write("mapdiv");
			});
		</script>
    </div>
    <div id ="legendBar" class="small-12 medium-12 large-12 columns">
        <h1>Legend Bar</h1>
          <a href="#" class="button [tiny small large] green">100-99.9</a>
          <a href="#" class="button [tiny small large] yellow">99.9-99.0</a>
          <a href="#" class="button [tiny small large] red1">99.0-95.0</a>
           <a href="#" class="button [tiny small large] red2">95.0-85.0</a>
           <a href="#" class="button [tiny small large] red3">85.0-0.0</a>
    </div>
</asp:Content>