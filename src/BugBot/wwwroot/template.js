function init(user, feedback, form, url) {

    var parameters = document.location.search;

    // user.value = getUserId();
    form.action = url + parameters;

}

function getUserId() {
    var hash = document.location.hash;    
    var username = (hash) ? hash.substring(1) : '';

    return encodeURIComponent(username);
}
