$(document).ready(function () {
    $('#filterSearch').click(function () {
        
        var filterURL = "/UsedStock/Index/?";
        var city = $("select#location option:checked").val();
        var minBudget = $("select#minBudget option:checked").val();
        var maxBudget = $("select#maxBudget option:checked").val();
        var flag = 0;

        if (city != "all") {
            filterURL += "city=" + city;
            if (minBudget != 0 || maxBudget != 2147483647) {
                filterURL += "&minBudget=" + minBudget;
                filterURL += "&maxBudget=" + maxBudget;
            }
        } else if (minBudget != 0 || maxBudget != 2147483647) {
            filterURL += "minBudget=" + minBudget;
            filterURL += "&maxBudget=" + maxBudget;
        }
        window.location.href = filterURL;
    });
    $('.search').click(function() {
        var id = $(this).parent().find(".car_id").text();
        window.location.href = "/StockDetail/Info/" + id;
        }
     )

});

