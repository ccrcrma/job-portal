window.addEventListener("load", function (event) {
    $("form[name='password-reset']").validate({
        rules: {
            password: {
                required: true,
                minlength: 5,
                messages: {
                    required: "Password is Required",
                    minlength: jQuery.validator.format(
                        "Please, at least {0} characters are necessary"
                    ),
                },
            },
            "confirm-password": {
                required: true,
                minlength: 5,
                equalTo: $("input[name='password']"),
            },
        },
    });
});
