function setFields() {
    $(".from").val($.QueryString("start"));
    $(".to").val($.QueryString("end"));
    $("#FeaturedContent_Datacenter").val($.QueryString("datacen").substring(0,3));
    $("#FeaturedContent_Network").val($.QueryString("network"));
    $("#FeaturedContent_Farm").val($.QueryString("farm"));
}

function setDefaults() {
    sessionStorage["start"] = new Date.parse("t - 8 d").toString("yyyy-MM-dd");
    sessionStorage["end"] = new Date.parse("t - 1 d").toString("yyyy-MM-dd");
    sessionStorage["pipeline"] = "overview";
    sessionStorage["datacen"] = "all";
    sessionStorage["network"] = -1;
    sessionStorage["farm"] = -1;
    sessionStorage["query"] = "?start=" + sessionStorage["start"] + "&end=" + sessionStorage["end"] + "&pipeline=" + sessionStorage["pipeline"] + "&datacen=" + sessionStorage["datacen"] + "&network=" + sessionStorage["network"] + "&farm=" + sessionStorage["farm"];
}

function updateQueryString() {
    if (sessionStorage["start"] == undefined) {
        setDefaults();
    }
    sessionStorage["query"] = "?start=" + sessionStorage["start"] + "&end=" + sessionStorage["end"] + "&pipeline=" + sessionStorage["pipeline"] + "&datacen=" + sessionStorage["datacen"] + "&network=" + sessionStorage["network"] + "&farm=" + sessionStorage["farm"];
    window.location.search = sessionStorage["query"];
}

$(document).ready(function () {
    $(document).foundation();
    if (window.location.search == "") {
        updateQueryString();
    }
    setFields();
    $(".button").click(function () {
        sessionStorage["start"] = $(".from").val();
        sessionStorage["end"] = $(".to").val();
        updateQueryString();
    });
    $("#FeaturedContent_Datacenter").change(function () {
        sessionStorage["datacen"] = $("#FeaturedContent_Datacenter").val().toString();
        updateQueryString();
    });
    $("#FeaturedContent_Network").change(function () {
        sessionStorage["network"] = $("#FeaturedContent_Network").val();
        updateQueryString();
    });
    $("#FeaturedContent_Farm").change(function () {
        sessionStorage["farm"] = $("#FeaturedContent_Farm").val();
        updateQueryString();
    });
});


$(document).ready(function () {
    $(".from").datepicker({
        defaultDate: sessionStorage["start"],
        changeMonth: true,
        changeYear: true,
        dateFormat:'yy-mm-dd',
        numberOfMonths: 1,
        onClose: function (selectedDate) {
            $(".to").datepicker("option", "minDate", selectedDate);
            $('.from').val(selectedDate);
        }
    });
    $(".to").datepicker({
        defaultDate: sessionStorage["end"],
        changeMonth: true,
        changeYear: true,
        maxDate: "+0D",
        dateFormat:'yy-mm-dd',
        numberOfMonths: 1,
        onClose: function (selectedDate) {
            $(".from").datepicker("option", "maxDate", selectedDate);
            $('.to').val(selectedDate);
        }
    });
});