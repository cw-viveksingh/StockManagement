function loadData(page, city, minBudget, maxBudget) {
    var filterURL = "/UsedStock/Index/?page=" + page;
    if (city != null && city != 'all') {
        filterURL += "&city=" + city;
    }

    if (minBudget != 0 || maxBudget != 2147483647) {
        filterURL += "&minBudget=" + minBudget;
        filterURL += "&maxBudget=" + maxBudget;
    }

    window.location.href = filterURL;
}

function Toggle(pageSize) {
    var url = new URL(window.location.href);
    var page = url.searchParams.get("page");
    if (page == null) {
        page = 1;
    }
    if (page == 1) {
        $('#previousResult').hide();
    }
    if (pageSize != 5) {
        $('#nextResult').hide();
    }
}

$(document).ready(function () {

    $('#previousResult').click(function () {
        var url = new URL(window.location.href);
        var page = url.searchParams.get("page");
        if (page == null) {
            page = 1;
        }
        page = parseInt(page) - 1;
        loadData(page, url.searchParams.get("city"), url.searchParams.get("minBudget"), url.searchParams.get("maxBudget"));
    });

    $('#nextResult').click(function () {
        var url = new URL(window.location.href);
        var page = url.searchParams.get("page");
        if (page == null) {
            page = 1;
        }

        page = parseInt(page) + 1;
        loadData(page, url.searchParams.get("city"), url.searchParams.get("minBudget"), url.searchParams.get("maxBudget"));
    });

    $('#filterSearch').click(function () {
        city = $("select#location option:checked").val();
        minBudget = $("select#minBudget option:checked").val();
        maxBudget = $("select#maxBudget option:checked").val();
        page = 1;
        loadData(1, city, minBudget, maxBudget);
    });

    $('.search').click(function () {
        var id = $(this).parent().find(".car_id").text();
        window.location.href = "/StockDetail/Info/" + id;
    });

});