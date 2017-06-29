$(document).ready(function () {
    $('#filterSearch').click(function () {
        
        var filterURL = "/UsedStock/FilterSearch/?";
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

        $("#listingStock").load(filterURL, function (response, status) {
            if (status == "error") {
                console.log("error");
            }
        });
        //window.location.href = filterURL;
    });
});

