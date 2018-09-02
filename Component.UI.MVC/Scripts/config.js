var require = {
    baseUrl: "/scripts/",
    paths: {
        "jquery": "jquery-3.3.1.min",
        "validator": "jquery.validate",
        "validate.unobtrusive": "jquery.validate.unobtrusive",
        "datatables.net": "lib/bootstrap-datatables/datatables",
        "dataTables.bootstrap": "lib/bootstrap-datatables/DataTables-1.10.18/js/dataTables.bootstrap",
        "datatables.net-buttons": "lib/bootstrap-datatables/Buttons-1.5.2/js/dataTables.buttons",
        "dataTables.responsive": "lib/bootstrap-datatables/Responsive-2.2.2/js/dataTables.responsive",
        "ckeditor": "lib/ckeditor/ckeditor",
        "bootstrap-datetimepicker": "lib/bootstrap-datepicker/js/bootstrap-datetimepicker",
        "bootstrap": "lib/bootstrap/dist/js/bootstrap",
        "bootstrap.lte": "lib/lte/js/adminlte",
        "bootstrap-datetimepicker-locales": "lib/bootstrap-datepicker/js/locales/bootstrap-datetimepicker.zh-CN",
    },
    waitSeconds: 15,
    map: {
        '*': {
            'css': 'css'
        }
    },
    shim: {
        'bootstrap': ["jquery"],
        "ckeditor": { "exports": "CKEDITOR" },
        "dataTables.bootstrap": ["bootstrap", "datatables.net", "dataTables.responsive",
            "css!lib/bootstrap-datatables/DataTables-1.10.18/css/dataTables.bootstrap.css",
            "css!lib/bootstrap-datatables/Responsive-2.2.2/css/responsive.dataTables.css"],
        "bootstrap-datetimepicker": ["bootstrap"],
        "bootstrap.lte": ["bootstrap"],
        "bootstrap-datetimepicker-locales": ["jquery","bootstrap-datetimepicker"]
        //...
    }
};