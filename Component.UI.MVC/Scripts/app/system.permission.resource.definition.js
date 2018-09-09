define("system.permission.resource.definition", ["jquery"], function (jQuery) {
    var definition = {};
 
    definition.RESOURCE_TYPE = { Button: 1, Grid: 2, CellEvent: 3, SingleEvent: 4 };
 
    definition.COMMNON_CSS = { Btn: ".icon-pencil, .icon-clear, .icon-edit, .icon-up, .icon-down, .icon-delete, .icon-download" };
 
    definition.Configuration = [
        {
            moduleId: 3,
            permissions: [
                {
                    permissionId: "ZF_Supplier_WF",
                    resources: [
                        {
                            selectExpr: '.btn-supplier-apply',
                        },
                        {
                            selectExpr: '.btn-rename',
                        },
                        //{
                        //    selectExpr: '...',
                        //    prefixSelector: '...'
                        //} // 
                    ]
                }
            ],
            referencePermissions: [
                //{
                //    referencePermission: {
                //        permissionId: ""
                //    },
                //    resources: [
                //        {
                //            //selectExpr: "span.icon-link.checklist",
                //            //prefixSelector: '[data-module="CoCManagement"] #RightPanel #tab_checklist'
                //        }// ccoc mapping open icon
                //    ]
                //}
            ],
            prefixSelector: '[data-module="supplier-view"]'
        },
    ];
    return definition;
});