(function () {
    'use strick';

    angular.module('yuyanApp').constant("endpoint", {
        "ipaddress": "http://www.lowata.com.au/tohowapi/ipaddress",
        "geoip": "http://freegeoip.net/json/",
        "localAPI": "http://localhost:5613/",
        "LiveAPI": "http://choriceapi.azurewebsites.net/"
    });

    //http://www.lowata.com.au/tohowapi/ipaddress

})();