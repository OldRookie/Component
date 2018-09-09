
; require(["jquery", "bootbox"], function (jQuery, bootbox) {
    jQuery.each(["get", "post"], function (i, method) {
        jQuery[method + "Request"] = function (url, data, callback, type) {
            if (jQuery.isFunction(data)) {
                type = type || callback;
                callback = data;
                data = undefined;
            }
            if (method == 'get') {
                if (data == undefined) {
                    data = {};
                }
                data.rnd = Math.random();
            }

            return jQuery.request({
                url: url,
                type: method,
                dataType: type,
                data: data,
                success: callback
            });
        };
    });

    $.extend({
        request: function (options) {
            var successCallback = options.success;
            options.success = function (rep) {
                try {
                    if (successCallback) {
                        successCallback(rep);
                    }
                } catch (e) {
                    $('.window-overlay').hide();
                }

                if (!options.notLoadOverlay) {
                    $('.window-overlay').hide();
                }
            }
            var errorCallback = options.error;
            options.error = function (rep) {
                try {
                    if (errorCallback) {
                        errorCallback(rep);
                    }
                    else {
                        bootbox.alert("服务器错误!")
                    }
                } catch (e) {
                    $('.window-overlay').hide();
                }

                if (!options.notLoadOverlay) {
                    $('.window-overlay').hide();
                }
            }

            if (!options.notLoadOverlay) {
                $('.window-overlay').show();
            }

            return jQuery.ajax(options);
        },
        showInvalidError: function (msgs, selector) {
            if (!selector) {
                selector = "[data-valmsg-summary]";
            }

            var container = $(selector);
            var list = container.find("ul");

            if (list && msgs.length) {
                list.empty();
                container.addClass("validation-summary-errors").removeClass("validation-summary-valid");

                $.each(msgs, function (i, msg) {
                    $("<li />").html(msg).appendTo(list);
                });
                container.show();
            }
        }
    });

    $.fn.serializeObject = function () {
        var o = {};
        var a = this.serializeArray();
        $.each(a, function () {
            if (o[this.name]) {
                if (!o[this.name].push) {
                    o[this.name] = [o[this.name]];
                }
                o[this.name].push(this.value || '');
            } else {
                o[this.name] = this.value || '';
            }
        });
        return o;
    };
});