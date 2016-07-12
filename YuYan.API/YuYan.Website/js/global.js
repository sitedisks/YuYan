//global setting
toastr.options = {
    "progressBar": false,
    "positionClass": "toast-top-left",
    "showDuration": "300",
    "hideDuration": "1000",
    "timeOut": "5000",
    "extendedTimeOut": "1000"
};

function isNullOrEmpty(s) {
    return (s == null || s === "");
}

// validate email
function validateEmail($email) {
    var emailReg = /^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/;
    return emailReg.test($email);
}

//fix of lodash.js
function lodashFix() {
    if (typeof _.contains === 'undefined')
        _.contains = _.includes;
    if (typeof _.object === 'undefined')
        _.object = _.zipObject;
}