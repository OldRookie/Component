define("system.permission", ["jquery", "system.permission.resource.definition"], function (jQuery, definition) {
    var permission = {}

    permission.setAllModuleAccessControl = function (userPermissionIds, moduleId) {
        $('style[id*="sheet_"]').remove();
        var configuration = definition.Configuration;

        for (var i = 0; i < configuration.length; i++) {
            var itemConfiguration = configuration[i];
            if (moduleId) {
                if (moduleId == configuration.moduleId) {
                    itemConfiguration.userPermissionIds = userPermissionIds

                    permission.setModuleAccessControl(itemConfiguration);
                    break;
                }
            }
            else {
                itemConfiguration.userPermissionIds = userPermissionIds

                permission.setModuleAccessControl(itemConfiguration);
            }
        }
    };

    permission.setModuleAccessControl = function (options) {
        var setting = {
            prefixSelector: null,
            moduleId: null,
            defaultType: definition.RESOURCE_TYPE.Button,
            permissions: [],
            referencePermissions: [],
            userPermissionIds: []
        };
        var sheet;

        $.extend(setting, options);

        var getStyleSheet = function (id) {
            var head = document.head || document.getElementsByTagName('head')[0];
            var idRenamed = 'sheet_' + id.replace(/[=|-]/ig, '_').replace(/[\"|\'|\]|\[]/ig, '');
            var sheetEle = null;
            if ($('style[id="' + idRenamed + '"]').length > 0) {
                sheetEle = $('style#' + idRenamed).get(0);
            }
            else {
                var style = document.createElement('style');
                style.type = 'text/css';
                style.id = idRenamed;
                head.appendChild(style);
                sheetEle = style.sheet || style.styleSheet;
            }

            return sheetEle;
        };

        var addRule = function (selector, style, index) {
            index = index || 0;
            var rules = sheet.rules || sheet.cssRules;
            if (sheet.addRule) {
                sheet.addRule(selector, style, rules.length);
            }
            else if (sheet.insertRule) {
                sheet.insertRule(selector + "{" + style + "}", rules.length);
            }
        };

        var setResourcesAccessControl = function (resources, isEnbaled) {
            for (var iResource = 0; resources && iResource < resources.length; iResource++) {
                var resource = resources[iResource];

                var type = resource.type || setting.defaultType;
                var prefixSelector = resource.prefixSelector || setting.prefixSelector;

                var selectExpr = resource.selectExpr ? (" " + resource.selectExpr) : "";

                var selector = (prefixSelector + " " + selectExpr).replace(/,/ig, "," + prefixSelector + " ");

                switch (type) {
                    case definition.RESOURCE_TYPE.Button:
                        if (!isEnbaled) {
                            var rules = "display:none !important;";
                            addRule(selector, rules);
                        }
                        break;
                    case definition.RESOURCE_TYPE.CellEvent:
                        if (!isEnbaled) {
                            var extendData = eval('(' + resource.extendData + ')');
                            SMART.UserPermission.DisabledGridCellData[selector] = extendData;
                            addRule(prefixSelector + " td[field] .fav-checkBox", "background-color:#ccc!important;opacity:0.5!important;border-color:#999!important;box-shadow:none!important;");
                        }
                        break;
                    case definition.RESOURCE_TYPE.SingleEvent:
                        //apply for single checkbox, kenny
                        if (!isEnbaled) {
                            var shadeRules = "background-color:#ccc!important;opacity:0.5!important;border-color:#999!important;box-shadow:none!important;z-index:2!important";
                            addRule(selector + ' u', shadeRules);
                        }
                        break;
                    default:
                }
            }
        };

        var isInArr = function (arr, value) {
            return new RegExp("(^|-)" + value.toString() + "(-|$)").test(arr.join("-"));
        };

        sheet = getStyleSheet(setting.prefixSelector);

        //set self Permission
        for (var i = 0; setting.permissions && i < setting.permissions.length; i++) {
            var permission = setting.permissions[i];
            //set global Permission style
            if (!setting.permissionIds || !isInArr(setting.userPermissionIds, permission.permissionId)) {
                setResourcesAccessControl(permission.resources, false);
            }
        }

        //set reference Permission
        for (var i = 0; setting.referencePermissions && i < setting.referencePermissions.length; i++) {
            var permission = setting.referencePermissions[i];
            if (!permissionIds || !isInArr(userPermissionIds, permission.referencePermission.permissionId)) {
                setResourcesAccessControl(permission.resources, false);
            }
        }
    };

    return permission;
});