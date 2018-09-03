; require(['jquery', "bootstrap.lte", "bootstrap-datetimepicker"], function ($) {
    $(function () {
        if ($.fn.dataTable) {
            $.extend(true, $.fn.dataTable.defaults, {
                "searching": false,
                "ordering": false,
                "language": {
                    "decimal": "",
                    "emptyTable": "空数据",
                    "info": "显示 _START_ 到 _END_ 中 _TOTAL_ 记录",
                    "infoEmpty": "显示 0 到 0 中 0 记录",
                    "infoFiltered": "(过滤 从 _MAX_ 总记录)",
                    "infoPostFix": "",
                    "thousands": ",",
                    "lengthMenu": "显示 _MENU_ 记录",
                    "loadingRecords": "Loading...",
                    "processing": "Processing...",
                    "search": "Search:",
                    "zeroRecords": "No matching records found",
                    "paginate": {
                        "first": "<<",
                        "last": ">>",
                        "next": ">",
                        "previous": "<"
                    },
                    "aria": {
                        "sortAscending": ": activate to sort column ascending",
                        "sortDescending": ": activate to sort column descending"
                    }
                }
            });
        }
    });
});