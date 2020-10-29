let firstValidation = true;
let validationListeners = [];
let submit;

function toggleSubmit(bool) {
    $(submit).prop('disabled', !bool);
}

function setupValidation(btnSubmit) {
    submit = btnSubmit;
    $(".invalid-input[validate-for]").each(function () {

        let field = $(this).attr("validate-for");

        let displayName = $(this).attr('input-display-name');
        let required = $(this).attr('input-required');
        let minLen = $(this).attr('input-min-len');
        let maxLen = $(this).attr('input-max-len');


        validationListeners.push(document.getElementById(field).addEventListener("input", function () {

            let content = $("#" + field).text();

            content = (content != null) ? content.trim() : content; 

            let hasError = false;

            if (minLen != null)
                hasError |= validate(field, displayName + ' field must exceed ' + minLen + ' characters', isMinLen(content, minLen));

            if (maxLen != null)
                hasError |= validate(field, displayName + ' field cannot exceed ' + maxLen + ' characters', isMaxLen(content, maxLen));

            if (required != null)
                hasError |= validate(field, displayName + ' field is required', isEmpty(content));

            if (!hasError) {
                clearError(field);
            }

        }, false));
    });
}

function validate(field, errorMsg, validateFunc) {
    let hasError = false;
    if (validateFunc && !firstValidation) {
        $('.invalid-input[validate-for=\'' + field + '\']').text(errorMsg);
        $("#"+field).addClass("invalid-form");
        toggleSubmit(false);
        hasError = true;
    }
    firstValidation = false;
    return hasError;
}

function isEmpty(content) {
    return (content == null || content.length == 0);
}

function isMinLen(content, minLen) {
    return (content.length < minLen && content.length != 0);
}

function isMaxLen(content, maxLen) {
    return (content.length > maxLen);
}

function clearError(field) {
    $('.invalid-input[validate-for=\'' + field + '\']').text('');
    $("#"+field).removeClass("invalid-form");
    toggleSubmit(true);
}