var require = {
    baseUrl: "/scripts/",
    paths: {
        "jquery": "jquery-1.10.2.min",
        "validator": "jquery.validate",
        "validate.unobtrusive": "jquery.validate.unobtrusive",
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
        "dataTables.bootstrap": ["bootstrap", "dataTables.responsive",
            "css!../content/datatables/css/dataTables.bootstrap.css",
            "css!../content/datatables-responsive/css/responsive.dataTables.css"],
        "datepicker": ["css!../content/bootstrap/css/bootstrap-datepicker.min.css"],
        "bootstrap-datetimepicker": ["bootstrap"],
        //...
    }
};