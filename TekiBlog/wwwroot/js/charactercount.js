let charCountListeners = [];

function setupCharacterCount() {
    $(".charcount-input[count-for]").each(function () {

        let field = $(this).attr("count-for");

        countCharacter(field);
        charCountListeners.push(document.getElementById(field).addEventListener("input", function () {
            countCharacter(field);
        }, false));
    });
}

function countCharacter(field) {
    let content = $("#" + field).text();
    content = (content != null) ? content.trim() : content;

    let len = content.length;


    if (len == 0) {
        $('.charcount-input[count-for=\'' + field + '\']').text('');
    } else {
        $('.charcount-input[count-for=\'' + field + '\']').text(len + ' characters');
    }
}