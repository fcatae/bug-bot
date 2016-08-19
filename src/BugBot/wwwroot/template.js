function init(user,feedback,form,url) {
    //var user = document.getElementById('user');
    //var feedback = document.getElementById('feedback');

    user.value = getUserId();
    form.action = url;

    alert(user.value)
}


function submitForm(feedback, user) {
    setUserId();

    return true;
}

function getUserId() {
    var hash = document.location.hash;    
    var username = (hash) ? hash.substring(1) : '';

    return encodeURIComponent(username);
}
