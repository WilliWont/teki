$("#btn-compose").on("click", function () {
    //$("#modal-container").load("/Article/UserDrafts?handler=Partial", function () {
    //    $("#draft-modal").modal('show');
    //});
    $(".vertical").addClass("d-none");
    $(".horizontal").addClass("d-none");
    $(".loader-compose").removeClass("d-none");


    $.ajax({
        url: "/Article/UserDrafts",
        timeout: 30000,
        type: "GET",
        success: function (msg) {
            $("#modal-container").append(msg);
            $("#draft-modal").modal('show');

            $(".loader-compose").addClass("d-none");
            $(".vertical").removeClass("d-none");
            $(".horizontal").removeClass("d-none");

            $(".draft-row").on("click", function () {
                $(".draft-row").removeClass("selected");
                $(this).addClass("selected");
                $("#editor-id").val($(this).attr("editor-id"));
            });
        },
        error: function () {
            displayNotification("Service not available, please try again later");

            $(".loader-compose").addClass("d-none");
            $(".vertical").removeClass("d-none");
            $(".horizontal").removeClass("d-none");
        }
    });

});
