$('.read-later-link').click(function (event) {
    alert("clicked");
    event.preventDefault();
    $.ajax({
        url: $(this).attr('href'),
        success: function (response) {
            alert(response);
        },
        error: function(req, status, error) {
            console.log('fail: ' + re + ', ' + status + ', ' + error);
        }
    });
    return false; // for good measure
});