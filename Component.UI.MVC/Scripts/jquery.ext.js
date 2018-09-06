jQuery(function () {
    jQuery.each(["get", "post"], function (i, method) {
        jQuery[method + "Request"] = function (url, data, callback, type) {
            // shift arguments if data argument was omitted
            if (method == 'get') {
                if (url.indexOf('?') != -1) {
                    url = url + "&rnd=" + Math.random();
                }
                else {
                    url = url + "?rnd=" + Math.random();
                }
            }

            if (jQuery.isFunction(data)) {
                type = type || callback;
                callback = data;
                data = undefined;
            }

            if (callback) {
                var originalCallBack = callback;
                callback = function (data) {
                    //$.loading(false);
                    originalCallBack(data);
                };
            }
            else {
                callback = function (data) {
                    //$.loading(false);
                };
            }

            //$.loading(true);
            return jQuery.ajax({
                url: url,
                type: method,
                dataType: type,
                data: data,
                success: callback,
                error: function () {
                    //$.loading(false);
                }
            });
        };
    });
});