function showErrorNotification(title, message) {
    $.Notification.notify('error', 'top left', title, message);
}

function showSuccessNotification(title, message) {
    $.Notification.notify('success', 'top left', title, message);
}