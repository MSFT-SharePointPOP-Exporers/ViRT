function setDefaults() {
    sessionStorage["start"] = new Date.parse("t - 8 d").toString("yyyy-MM-dd");
    sessionStorage["end"] = new Date.parse("t - 1 d").toString("yyyy-MM-dd");
    sessionStorage["pipeline"] = "Overview";
    sessionStorage["datacen"] = "All";
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

function setSessionStorage() {
    sessionStorage["start"] = $.QueryString("start");
    sessionStorage["end"] = $.QueryString("end");
    sessionStorage["pipeline"] = $.QueryString("pipeline");
    sessionStorage["datacen"] = $.QueryString("datacen");
    sessionStorage["network"] = $.QueryString("network");
    sessionStorage["farm"] = $.QueryString("farm");
}

function setFields() {
    $(".from").val(sessionStorage["start"]);
    $(".to").val(sessionStorage["end"]);
    $("#FeaturedContent_Datacenter").val(sessionStorage["datacen"]);
    $("#FeaturedContent_Network").val(sessionStorage["network"]);
    $("#FeaturedContent_Farm").val(sessionStorage["farm"]);
}

function setActivePipeline(id) {
    if (id == sessionStorage["pipeline"]) {
        $("#" + id).addClass("selected");
    }
}

function setPipeline(id) {
    sessionStorage["pipeline"] = id;
    updateQueryString();
}


/*
*Set a farm and it's network is found and set in the url too!
*/
function setDatacenter(id) {
    sessionStorage["datacen"] = id;
    sessionStorage["network"] = -1;
    sessionStorage["farm"] = -1;
    updateQueryString();
}

/*
*Set a farm and it's network is found and set in the url too!
*/
function setFarm(id) {
    sessionStorage["farm"] = id;
    sessionStorage["network"] = $("#" + id).parent().attr('id');
    updateQueryString();
}

/*
*Set a network and it'll reset the farm!
*/
function setNetwork(id) {
    sessionStorage["network"] = id;
    sessionStorage["farm"] = -1;
    updateQueryString();
}
                                                

$(document).ready(function () {
    $(document).foundation();
    if (window.location.search == "") {
        updateQueryString();
    } else {
        setSessionStorage();
        setFields();
    }
});

//Move Elsewhere!
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