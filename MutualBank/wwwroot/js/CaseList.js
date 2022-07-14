postCaseBtn.addEventListener('click', function () {
    dynamicLoading.css("/css/PostCase.css");
    $.ajax({
        type: "POST",
        url: "/Case/GetPostCase",
        success: function (res) {
            $('#ajaxSection').html(res);
        },
        error: function (res) {
            console.log(res);
        },
        complete: function () {
            $.getScript("/js/PostCaseReady.js", function () {
            });
            $.getScript("/js/PostCase.js", function () {
            });
        }
    });
});