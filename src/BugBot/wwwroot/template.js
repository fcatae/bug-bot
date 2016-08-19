function init(user, feedback, form, url) {
    user.value = getUserId();
    form.action = url;
}

function getUserId() {
    var hash = document.location.hash;    
    var username = (hash) ? hash.substring(1) : '';

    return encodeURIComponent(username);
}
