window.registerRefreshKey = function (dotnetHelper) {

    document.addEventListener("keydown", function (e) {

        if (e.key === "F5") {

            e.preventDefault();

            dotnetHelper.invokeMethodAsync("RefreshData");
        }
    });
}