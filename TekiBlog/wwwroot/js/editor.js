

$(document).ready(function () {
    // initializing editor
    tinymce.init({
        selector: "#form-content",
        setup: function (editor) {

            editor.on('init', function (e) {
                editor.setContent($("#input-content").val());
                firstValidation = false;
                let input = $("#input-id").val();
                if (input != null && input.length > 0) {
                    fireAllValidation();
                }
                validateSubmit();
            });

            editor.on('WordCountUpdate', function (e) {
                let charcount = tinymce.activeEditor.plugins.wordcount.body.getCharacterCount();
                if (charcount != 0)
                    $('.charcount-input[count-for="input-raw"]').text(charcount + ' characters');
                else
                    $('.charcount-input[count-for="input-raw"]').text('');
            });

        },
        placeholder: "article content here",
        inline: true,
        plugins: 'codesample autoresize link paste lists image wordcount',
        menubar: false,
        contextmenu: false,
        toolbar: 'undo redo | styleselect | bold italic underline | numlist bullist | strikethrough subscript superscript | codesample image link',
        link_assume_external_targets: true,
        paste_as_text: true,
        paste_data_images: true,
        branding: false, // remove this in real production for legal issues
    });

    // fix title formatting on load
    let title = $('#form-title').text();
    if (title != null)
        $('#form-title').text(title.trim());

    // setup validation for forms and for submit button
    setupValidation('#btnSubmit');

    setupCharacterCount();
});

// bind data to form before posting to server
function bindData() {
    $("#input-raw").val(tinymce.activeEditor.getContent({ format: "text" }));
    $("#input-content").val(tinymce.activeEditor.getContent());
    $("#input-tldr").val($("#form-tldr").text());
    $("#input-title").val($("#form-title").text());
}

// to convert any pasting into plain text
document.querySelector('#form-title').addEventListener('paste', function (event) {
    event.preventDefault();
    document.execCommand('inserttext', false, event.clipboardData.getData('text/plain').trim());
});
document.querySelector('#form-tldr').addEventListener('paste', function (event) {
    event.preventDefault();
    document.execCommand('inserttext', false, event.clipboardData.getData('text/plain').trim());
});


function previewFile(input) {
    var file = $("input[type=file]").get(0).files[0];

    if (file) {
        var reader = new FileReader();

        reader.onload = function () {
            $("#article-img-cover").removeClass("d-none");
            $("#article-img-cover").attr("src", reader.result);
        }
        //$("#input-img-update").prop("checked", true);
        reader.readAsDataURL(file);
    }
}
var $navbar = $("#site-header");
jQuery(window).on('scroll', function () {
    var top = jQuery(window).scrollTop(),
        divBottom = jQuery('.article-header').offset().top + jQuery('.article-header').outerHeight();
    if (divBottom * 6 / 9 > top) {
        $navbar.removeClass('hide-nav');
    } else {
        $navbar.addClass('hide-nav');
    }
});

// terrible code, do not use in production
// but it works, so i'll leave it here
function hideField(fieldName) {
    $(fieldName).addClass('d-none');
}
// Write your JavaScript code.
$("img").on("error", function () {
    $(this).addClass("d-none");
});