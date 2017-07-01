function loadData() {
    var filterURL = "/UsedStock/FilterSearch/?page=" + page;
    if (city != "all") {
        filterURL += "&city=" + city;
    }

    if (minBudget != 0 || maxBudget != 2147483647) {
        filterURL += "&minBudget=" + minBudget;
        filterURL += "&maxBudget=" + maxBudget;
    }

    $("#listingStock").load(filterURL, function (response, status) {
        if (status == "error") {
            console.log("gadbad ho rahi");
            console.log("error occured");
        }
    });
}

var city = "all";
var minBudget = 0;
var maxBudget = 2147483647;
var page = 1;

$(document).ready(function () {
    console.log("ready ready");
    $('#previousResult').prop('disabled', true);
    $('#nextResult').prop('disabled', false);



    $('#previousResult').click(function () {
        page -= 1;
        if ($('#nextResult').is(':disabled')) {
            $('#nextResult').prop('disabled', false);
        }
        if (page == 1) {
            $('#previousResult').prop('disabled', true);
        }
        loadData("/UsedStock/FilterSearch/?page=" + page);
    });

    $('#nextResult').click(function () {
        page += 1;
        $('#previousResult').prop('disabled', false);
        loadData("/UsedStock/FilterSearch/?page=" + page);
    });

    $('#filterSearch').click(function () {
        city = $("select#location option:checked").val();
        minBudget = $("select#minBudget option:checked").val();
        maxBudget = $("select#maxBudget option:checked").val();
        page = 1;
        $('#previousResult').prop('disabled', true);
        $('#nextResult').prop('disabled', false);
        loadData("/UsedStock/FilterSearch/?page=" + page);
    });

    $('.search').click(function () {
        var id = $(this).parent().find(".car_id").text();
        window.location.href = "/StockDetail/Info/" + id;
    });

});