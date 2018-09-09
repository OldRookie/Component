; require(["jquery", "bootbox", "validator"], function (jQuery, bootbox) {
    $.validator.addMethod("userFullNameValidator", function (value, element, params) {
        return value == params;
    }, '格式不对');
});