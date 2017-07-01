$(document).ready(function () {
    $('#previousResult').prop('disabled', true);
    $('#nextResult').prop('disabled', false);

    var city = $("select#location option:checked").val();
    var minBudget = $("select#minBudget option:checked").val();
    var maxBudget = $("select#maxBudget option:checked").val();
    var page = 1;

    function loadData(filterURL) {
        if (city != "all") {
            filterURL += "&city=" + city;
        }

        if (minBudget != 0 || maxBudget != 2147483647) {
            filterURL += "&minBudget=" + minBudget;
            filterURL += "&maxBudget=" + maxBudget;
        }

        $("#listingStock").load(filterURL, function (response, status) {
            if (status == "error") {
                console.log("error occured");
            }
        });

    }

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
    }
     )

});