//redirect to login
$(document).ajaxError(function (e, xhr) {
    if (xhr.status == 403) {
        var response = $.parseJSON(xhr.responseText);
        window.location = response.LoginUrl;
    }
});

//check user
const checkAccount = function (callback) {
    $.ajax({
        type: "GET",
        url: "/Account/CheckAuthorize",
    }).done(function () {
        if (callback) callback();
    });
};