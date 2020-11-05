$('#btnSave').click(function (event) {
    event.preventDefault();
    bindData();

    let form = $('#form')[0];
    let data = new FormData(form);

    $('#btnReturn').prop('disabled', true);
    $('#btnSave').prop('disabled', true);
    $('#btnSubmit').prop('disabled', true);

    $('#btnSave').children(":first").removeClass('fa-save');
    $('#btnSave').children(":first").addClass('loader');

    $.ajax({
        type: "POST",
        enctype: 'multipart/form-data',
        url: DRAFT_ADDR,
        data: data,
        processData: false,
        contentType: false,
        cache: false,
        timeout: 600000,
        success: function (response) {
            if (response != 0 && response != 1) {
                displayNotification('Service not available right now, please try again later');
            } else {
                displayNotification('Drafted successfully');
            }

            $('#btnReturn').prop('disabled', false);
            $('#btnSave').prop('disabled', false);
            $('#btnSave').children(":first").addClass('fa-save');
            $('#btnSave').children(":first").removeClass('loader');
            fireAllValidation();
            validateSubmit();
        },
        error: function (req, status, error) {
            //$lastA.children(":first").removeClass('loader');
            console.log('fail: ' + req + ', ' + status + ', ' + error);
            displayNotification('Service not available right now, please try again later');

            $('#btnReturn').prop('disabled', false);
            $('#btnSave').prop('disabled', false);
            $('#btnSave').children(":first").addClass('fa-save');
            $('#btnSave').children(":first").removeClass('loader');
            fireAllValidation();
            validateSubmit();
        }
    });
    return false; // for good measure
});