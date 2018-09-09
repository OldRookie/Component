var require = {
    baseUrl: "/scripts/",
    paths: {
        "jquery": "jquery-3.3.1.min",
        "validator": "jquery.validate.js?v=1",
        "validate.unobtrusive": "jquery.validate.unobtrusive.js?v=1",
        "datatables.net": "lib/bootstrap-datatables/DataTables-1.10.18/js/jquery.dataTables",
        "dataTables.bootstrap": "lib/bootstrap-datatables/DataTables-1.10.18/js/dataTables.bootstrap",
        "dataTables.responsive": "lib/bootstrap-datatables/Responsive-2.2.2/js/dataTables.responsive",
        "datatables.net-select": "lib/bootstrap-datatables/Select-1.2.6/js/dataTables.select",
        "dataTables.select.ext": "lib/bootstrap-datatables/dataTables.select.ext",
        "select2": "lib/select2/js/select2",
        "ckeditor": "lib/ckeditor/ckeditor",
        "bootstrap-datetimepicker": "lib/bootstrap-datepicker/js/bootstrap-datetimepicker",
        "bootstrap": "lib/bootstrap/dist/js/bootstrap",
        "bootstrap.lte": "lib/lte/js/adminlte",
        "bootstrap-datetimepicker-locales": "lib/bootstrap-datepicker/js/locales/bootstrap-datetimepicker.zh-CN",
        "fileinput-zh": "lib/bootstrap-fileinput/js/locales/zh",
        "fileinput-theme": "lib/bootstrap-fileinput/themes/explorer-fa/theme",
        "fileinput": "lib/bootstrap-fileinput/js/fileinput",
        "jquery.ext": "jquery.ext",
        "bootbox": "bootbox",
        "toastr": "toastr",
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
        "bootbox": { "exports": "bootbox", deps: ["css!../Content/bootbox.css"] },
        "toastr": { "exports": "toastr", deps: ["css!../Content/toastr.css"] },
        "dataTables.bootstrap": ["bootstrap", "dataTables.responsive",
            "css!lib/bootstrap-datatables/DataTables-1.10.18/css/dataTables.bootstrap.css",
            "css!lib/bootstrap-datatables/Responsive-2.2.2/css/responsive.dataTables.css"],
        "datatables.net-select": ["css!lib/bootstrap-datatables/Select-1.2.6/css/select.bootstrap.css"],
        "bootstrap-datetimepicker": ["bootstrap"],
        "bootstrap.lte": ["bootstrap"],
        "bootstrap-datetimepicker-locales": ["jquery", "bootstrap-datetimepicker"],
        "fileinput-zh": ["fileinput", "fileinput-theme"],
        "fileinput-theme": ["fileinput"],
        "jquery.ext": ["jquery"],
        "validate.unobtrusive": ["validator"],
        "validator": ["jquery"]
        //...
    }
};