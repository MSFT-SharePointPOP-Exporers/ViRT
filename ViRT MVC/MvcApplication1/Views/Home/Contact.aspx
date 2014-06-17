<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Head" ContentPlaceHolderID="Head" runat="server">
    <link href="../../Content/DCHM.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="contactTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Data Center Heat Map
</asp:Content>

<asp:Content ID="Content" ContentPlaceHolderID="FeaturedContent" runat="server">
    <h1>AM1</h1> <!-- Data Center will go here -->
    <h2>Select a Network of Farm to filter reliability data.</h2>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <div id="DCHM">
    <ul class="small-block-grid-2 medium-block-grid-3 large-block-grid-3">
        <li class="network_box">
            <ul class="small-block-grid-4 medium-block-grid-4 large-block-grid-4">
                <li class="farm_box">
                    <p>Farm 24</p>
                    <p>99.55%</p>
                </li>
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
        <li class="network_box">
             <ul class="small-block-grid-4 medium-block-grid-4 large-block-grid-4">
                <li class="farm_box"><!-- Your content goes here --></li>
                <li class="farm_box"><!-- Your content goes here --></li>
                <li class="farm_box"><!-- Your content goes here --></li>
                <li class="farm_box"><!-- Your content goes here --></li>
            </ul>
        </li>
    </ul>
    </div>
    <div id ="legendBar" class="small-12 medium-12 large-12 columns">
        <h1>Legend Bar</h1>
    </div>
</asp:Content>