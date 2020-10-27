
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
    plugins: 'codesample autoresize link',
    menubar: false,
    contextmenu: false,
    toolbar: 'bold italic underline | strikethrough subscript superscript | codesample media link',
});

// bind data to form before posting to server
function bindData() {
    $("#input-raw").val(tinymce.activeEditor.getContent({ format: "text" }));
    $("#input-tldr").val($("#form-tldr").text());
    $("#input-title").val($("#form-title").text());
    $("#input-content").val(tinymce.activeEditor.getContent());
}

$("#form").submit(function (event) {
    alert('submit');

    $.post("/Article/PostArticle", $form.serialize(), function (response) {
        alert(response);
        //if (response.error) {
        //    alert("error: " + response.error);
        //} else {
        //    createComment(response);
        //}
    });

    event.preventDefault();
});