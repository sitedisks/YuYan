angular.module('choriceApp', [
    'ui.bootstrap',
    'ui.router',
    'ngResource'
]);

// global function
function isNullOrEmpty(s) {
    return (s == null || s === "");
}