(function () {
    'use strick';
    angular.module('choriceApp').constant("endpoint", {
        "ipaddress": "http://www.lowata.com.au/tohowapi/ipaddress",
        //"geoip": "http://freegeoip.net/json/",
        "geoip": "http://ip-api.com/json/",
        "localAPI": "http://localhost:5613/",
        "LiveAPI": "http://choriceapi.azurewebsites.net/"
    });

})();