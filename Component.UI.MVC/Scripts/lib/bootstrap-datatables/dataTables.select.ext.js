; define("dataTables.select.ext", ["jquery", "dataTables.bootstrap", "datatables.net-select"], function ($) {
    return {
        initSelectAll: function (dataTable) {
            $(dataTable.table().container()).on("click", "th.select-checkbox", function () {
                if ($("th.select-checkbox").hasClass("selected")) {
                    dataTable.rows().deselect();
                    $("th.select-checkbox").removeClass("selected");
                } else {
                    dataTable.rows().select();
                    $("th.select-checkbox").addClass("selected");
                }
            });
            dataTable.on("select deselect", function () {
                if (dataTable.rows({
                    selected: true
                }).count() !== dataTable.rows().count()) {
                    $("th.select-checkbox").removeClass("selected");
                } else {
                    $("th.select-checkbox").addClass("selected");
                }
            });
        }
    }
});