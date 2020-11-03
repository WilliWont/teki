let $lastA;

let removeHref = $("#href-remove-template").attr('href');
let addHref = $("#href-add-template").attr('href');


$('.read-later-link').click(function (event) {
    event.preventDefault();
    $lastA = $(this);
    $lastA.children(":first").removeClass('fa-bookmark-o');
    $lastA.children(":first").removeClass('fa-bookmark');
    $lastA.children(":first").addClass('loader');


    $.ajax({
        url: $(this).attr('href'),
        success: function (response) {
            if (response != 0 && response != 1) {
                alert('service not available right now, please try again later');
            } else {
                let prevHref = $lastA.attr("href");
                let i = prevHref.lastIndexOf("/");
                let id = prevHref.substring(i, prevHref.length);
                let newHref = null;

                if (response == 0) {
                    $lastA.children(":first").addClass('fa-bookmark');
                    $lastA.children(":first").removeClass('loader');
                    newHref = removeHref + id;
                } else if (response == 1) {
                    $lastA.children(":first").addClass('fa-bookmark-o');
                    $lastA.children(":first").removeClass('loader');
                    newHref = addHref + id;
                } else {
                    alert('service not available right now, please try again later');
                }

                if(newHref != null)
                    $lastA.attr("href", newHref);

            }
        },
        error: function(req, status, error) {
            $lastA.children(":first").removeClass('loader');
            console.log('fail: ' + re + ', ' + status + ', ' + error);
        }
    });
    return false; // for good measure
});