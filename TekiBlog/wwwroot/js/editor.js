

$(document).ready(function () {
    // fix title formatting on load
    let title = $('#form-title').text();
    if (title != null)
        $('#form-title').text(title.trim());

    // setup validation for forms and for submit button
    setupValidation('#btnSubmit');

    setupCharacterCount();

    toggleSubmit($('#input-id').val()!=null);
});

// initializing editor
tinymce.init({
    selector: "#form-content",
    setup: function (editor) {

        editor.on('init', function (e) {
            editor.setContent($("#input-content").val());
        });

        editor.on('WordCountUpdate', function (e) {
            let charcount = tinymce.activeEditor.plugins.wordcount.body.getCharacterCount();
            if(charcount != 0)
            $('.charcount-input[count-for="input-raw"]').text(charcount+' characters');
            else
            $('.charcount-input[count-for="input-raw"]').text('');
        });

    },
    placeholder: "article content here",
    inline: true,
    plugins: 'codesample autoresize link paste lists image paste wordcount',
    menubar: false,
    contextmenu: false,
    toolbar: 'undo redo | styleselect | bold italic underline | numlist bullist | strikethrough subscript superscript | codesample image link',
    link_assume_external_targets: true,
    paste_as_text: true,
    paste_data_images: true,
    branding: false, // remove this in real production for legal issues
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

//$("#form").submit(function (event) {
//    alert('submit');
//    bindData();
//    $.ajax({
//        type: "POST",
//        url: "/Article/AjaxTest",
//        data: $("#form").serialize(),
//        dataType: "text",
//        success: function (msg) {
//            //let response = JSON.parse(msg);

//            //if (response.isValid == true) {

//            //} else {
//            //    for (let i = 0; i < response.validationError.length; i++) {
//            //        let field = validationFields.get(response.validationError[0]);
//            //        field.text(response.validationError.msg);
//            //    }
//            //}
//        },
//        error: function (req, status, error) {
//            console.log(msg);
//        }
//    }); 

//    event.preventDefault();
//});

function previewFile(input) {
    var file = $("input[type=file]").get(0).files[0];

    if (file) {
        var reader = new FileReader();

        reader.onload = function () {
            $("#article-img-cover").attr("src", reader.result);
        }
        //$("#input-img-update").prop("checked", true);
        reader.readAsDataURL(file);
    }
}