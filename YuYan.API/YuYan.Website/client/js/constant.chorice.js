(function () {
    'use strick';
    angular.module('choriceApp').constant("endpoint", {
        "ipaddress": "http://www.lowata.com.au/tohowapi/ipaddress",
        //"geoip": "http://freegeoip.net/json/",
        "geoip": "http://ip-api.com/json/",
        "LocalAPI": "http://localhost:5613/",
        "LiveAPI": "http://choriceapi.azurewebsites.net/"
    }).constant("chartColor", {
        "info": "#5bc0de",
        "success": "#5cb85c",
        "warning": "#f0ad4e",
        "danger": "#d9534f",
        "primary": "#337ab7",
        "well": "#f5f5f5"
    }).constant("imageSize", {
        "survey": 760,
        "question": 760,
        "questionItem": 360,
        "result": 760
    });

})();