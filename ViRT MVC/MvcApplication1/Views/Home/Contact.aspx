<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Head" ContentPlaceHolderID="Head" runat="server">
    <link href="../../Content/DCHM.css" rel="stylesheet" />
    <script>
        $(document).ready(function () {
            $("#rendering h1").append($.QueryString("datacen").substring(0,3));
            $.ajax({
                contentType: "application/json",
                data: sessionStorage["query"],
                url: '<%= Url.Action("getNetworkFarm", "Query") %>',
                dataType: "json",
                success: function (data) {
                    console.log(data);
                }
            });
      });
    </script>
</asp:Content>

<asp:Content ID="contactTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Data Center Heat Map
</asp:Content>

<asp:Content ID="Content" ContentPlaceHolderID="FeaturedContent" runat="server">
    <h1></h1> <!-- Data Center will go here -->
    <h2>Select a Network of Farm to filter reliability data.</h2>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <ul class="small-block-grid-2 medium-block-grid-3 large-block-grid-3">
        <div id ="DHCM">
        <li class="network_box">
            <ul class="small-block-grid-4 medium-block-grid-4 large-block-grid-4">
                <li class="farm_box"><p>Farm 24</p><p>99.55%</p></li>
                <li class="farm_box"><p>Farm 24</p><p>99.55%</p></li>
                <li class="farm_box"><p>Farm 24</p><p>99.55%</p></li>
                <li class="farm_box"><p>Farm 24</p><p>99.55%</p></li>
            </ul>
        </li>
        <li class="network_box">
            <ul class="small-block-grid-4 medium-block-grid-4 large-block-grid-4">
                <li class="farm_box"><!-- Your content goes here --></li>
                <li class="farm_box"><!-- Your content goes here --></li>
                <li class="farm_box"><!-- Your content goes here --></li>
                <li class="farm_box"><!-- Your content goes here --></li>

            </ul>
        </li>
        <li class="network_box">
            <ul class="small-block-grid-4 medium-block-grid-4 large-block-grid-4">
                <li class="farm_box"><!-- Your content goes here --></li>
                <li class="farm_box"><!-- Your content goes here --></li>
                <li class="farm_box"><!-- Your content goes here --></li>
                <li class="farm_box"><!-- Your content goes here --></li>
            </ul>
        </li>
        <li class="network_box">
            <ul class="small-block-grid-4 medium-block-grid-4 large-block-grid-4">
                <li class="farm_box"><!-- Your content goes here --></li>
                <li class="farm_box"><!-- Your content goes here --></li>
                <li class="farm_box"><!-- Your content goes here --></li>
                <li class="farm_box"><!-- Your content goes here --></li>
            </ul>
        </li>
        <li class="network_box">
             <ul class="small-block-grid-4 medium-block-grid-4 large-block-grid-4">
                <li class="farm_box"><!-- Your content goes here --></li>
                <li class="farm_box"><!-- Your content goes here --></li>
                <li class="farm_box"><!-- Your content goes here --></li>
                <li class="farm_box"><!-- Your content goes here --></li>
            </ul>
        </li>
    </ul>
    <div id ="legendBar" class="small-12 small-centered medium-12 large-12 columns large-centered">
        <h1>Legend Bar</h1>
                <ul class="button-group [radius round]">
          <li><a href="#" id="green" class="button [tiny small large]">100-99.9</a></li>
          <li><a href="#" id="yellow" class="button [tiny small large]">99.9-99.0</a></li>
          <li><a href="#" id="red1" class="button [tiny small large]">99.0-95.0</a></li>
            <li><a href="#" id="red2" class="button [tiny small large]">95.0-85.0</a></li>
            <li><a href="#" id="red3" class="button [tiny small large]">85.0-0.0</a></li>
        </ul>
    </div>
</asp:Content>