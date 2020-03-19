// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

    $("#btnPrevedeteTekst").submit(function (event) {
        grecaptcha.execute('6LdCFOAUAAAAADRsAUg-ZlmFgyXV1h0R7rsFUO8l', { action: 'homepage_submit_form' }).then(function (token) {
            $.ajax({
                method: "POST",
                url: "Home/VerifyReCaptchaV3",
                data: {
                    response: token,
                    action: 'homepage_submit_form'
                }
            })
                .done(function (msg) {
                });
        })
    });