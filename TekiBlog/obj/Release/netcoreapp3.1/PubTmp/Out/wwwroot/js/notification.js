function toggleNotification(status) {
    if (status) {
        $("#page-notification").removeClass("toast-hide");
    } else {
        $("#page-notification").addClass("toast-hide");
    }
}

function setNotificationContent(content) {
    $("#page-notification-body").text(content);
}

function displayNotification(content) {
    setNotificationContent(content);
    toggleNotification(true);
    setTimeout(() => {
        toggleNotification(false);
    },
        3000);
}