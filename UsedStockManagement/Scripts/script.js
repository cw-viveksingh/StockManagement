/*var city = "all";
var minBudget = 0;
var maxBudget = 2147483647;
var page = 1;
*/
function loadData(page, city, minBudget, maxBudget) {
    var filterURL = "/UsedStock/Index/?page=" + page;
    if (city != "all") {
        filterURL += "&city=" + city;
    }

    if (minBudget != 0 || maxBudget != 2147483647) {
        filterURL += "&minBudget=" + minBudget;
        filterURL += "&maxBudget=" + maxBudget;
    }

    /*$("#listingStock").load(filterURL, function (response, status) {
        if (status == "error") {
            console.log("gadbad ho rahi");
            console.log("error occured");
        }
    });*/

    window.location.href = filterURL;
}

$(document).ready(function () {
    console.log("ready ready");
    $('#previousResult').prop('disabled', true);
    $('#nextResult').prop('disabled', false);



    $('#previousResult').click(function () {
        var url = new URL(window.location.href);
        var page = url.searchParams.get("page");
        if (page == null) {
            page = 1;
        }
        page -= 1;
        if ($('#nextResult').is(':disabled')) {
            $('#nextResult').prop('disabled', false);
        }
        if (page == 1) {
            $('#previousResult').prop('disabled', true);
        }
        loadData(page, url.searchParams.get("city"), url.searchParams.get("minBudget"), url.searchParams.get("maxBudget"));
    });

    $('#nextResult').click(function () {
        var url = new URL(window.location.href);
        var page = url.searchParams.get("page");
        if (page == null) {
            page = 1;
        }
        page += 1;
        $('#previousResult').prop('disabled', false);
        loadData(page, url.searchParams.get("city"), url.searchParams.get("minBudget"), url.searchParams.get("maxBudget"));
    });

    $('#filterSearch').click(function () {
        city = $("select#location option:checked").val();
        minBudget = $("select#minBudget option:checked").val();
        maxBudget = $("select#maxBudget option:checked").val();
        page = 1;
        $('#previousResult').prop('disabled', true);
        $('#nextResult').prop('disabled', false);
        loadData(1, city, minBudget, maxBudget);
    });

    /*$('.search').on('click', 'button', function () {
        var id = $(this).parent().find(".car_id").text();
        window.location.href = "/StockDetail/Info/" + id;
    });*/

    $('.search').click(function () {
        var id = $(this).parent().find(".car_id").text();
        window.location.href = "/StockDetail/Info/" + id;
    });

});