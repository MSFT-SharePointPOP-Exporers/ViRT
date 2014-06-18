var start;
var end;
var pipeline;
var datacen;
var network;
var farm;

function setFields() {
    $(".from").val($.QueryString("start"));
    $(".to").val($.QueryString("end"));
}

function setDefaults() {
    start = new Date.parse("t - 8 d").toString("yyyy-MM-dd");
    end = new Date.parse("t - 1 d").toString("yyyy-MM-dd");
    pipeline = "overview";
    datacen = "all";
    network = -1;
    farm = -1;
}

function updateQueryString() {
    document.cookie = "?start=" + start + "&end=" + end + "&pipeline=" + pipeline + "&datacen=" + datacen + "&network=" + network + "&farm=" + farm;
    window.location.search = document.cookie;
}

$(document).ready(function () {
    $(document).foundation();
    if (window.location.search == "") {
        setDefaults();
        updateQueryString();
    }
    setFields();
    $(".button").click(function () {
        start = $(".from").val();
        end = $(".to").val();
        updateQueryString();
    });
});


$(document).ready(function () {
    $(".from").datepicker({
        defaultDate: start,
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
        defaultDate: end,
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