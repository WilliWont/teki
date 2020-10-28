
// initializing editor
tinymce.init({
    selector: "#form-content",
    setup: function (editor) {
        editor.on('init', function (e) {
            editor.setContent($("#input-content").val());
        });
    },
    placeholder: "article content here",
    inline: true,
    plugins: 'codesample autoresize link paste',
    menubar: false,
    contextmenu: false,
    toolbar: 'bold italic underline | strikethrough subscript superscript | codesample media link',
    link_assume_external_targets: true,
    paste_as_text: true
});

// bind data to form before posting to server
function bindData() {
    $("#input-raw").val(tinymce.activeEditor.getContent({ format: "text" }));
    $("#input-tldr").val($("#form-tldr").text());
    $("#input-title").val($("#form-title").text());
    $("#input-content").val(tinymce.activeEditor.getContent());
}

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
