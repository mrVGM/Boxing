function makePrediction(form, id, success) {
    form.submit(function (e) {
        $.ajax({
            type: "POST",
            url: "/Predictions/" + id.toString(),
            data: form.serialize(),
            success: success
        });
        e.preventDefeult();
    });
}