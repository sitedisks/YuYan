angular.module('choriceApp', [
    'ui.bootstrap',
    'ui.router',
    'ngResource',
    'googlechart'
]);

// global function
function isNullOrEmpty(s) {
    return (s == null || s === "");
}