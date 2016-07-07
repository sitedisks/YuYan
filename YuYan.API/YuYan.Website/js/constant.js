(function () {
    'use strick';

    angular.module('yuyanApp').constant("endpoint", {
        "ipaddress": "http://www.lowata.com.au/tohowapi/ipaddress",
        "geoip": "http://freegeoip.net/json/",
        "LocalAPI": "http://localhost:5613/",
        "LiveAPI": "http://choriceapi.azurewebsites.net/"
    }).constant("imageType", {
        "UserAvatar": { id: 1, group: "User" },
        "SurveyBanner": { id: 2, group: "Survey" },
        "SurveyLogo": { id: 3, group: "Survey" },
        "SurveyRef": { id: 4, group: "Survey" },
        "QuestionBanner": { id: 5, group: "Question" },
        "QuestionRef": { id: 6, group: "Question" },
        "ItemRef": { id: 7, group: "QuestionItem" },

    });

    //http://www.lowata.com.au/tohowapi/ipaddress

})();