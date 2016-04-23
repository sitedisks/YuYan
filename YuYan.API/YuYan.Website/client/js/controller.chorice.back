(function () {
    'use strict';
    angular.module('choriceApp').controller('choriceCtrl', ['$scope', '$http', '$log', '$stateParams', 'choriceAPISvc', 'endpoint',
        function ($scope, $http, $log, $stateParams, choriceAPISvc, endpoint) {

            var tokenUrl = $stateParams.tokenUrl;
            var location;

            $scope.APIMini = 1;
            $scope.APIResolved = 0;
            $scope.submitSuccess = false;
            $scope.ip;
            $scope.geo_city = null;
            $scope.geo_state = null;
            $scope.geo_country = null;

            $scope.radioChecked = radioChecked;
            $scope.submitSurvey = submitSurvey;
            $scope.backHome = backHome;

            // --------- start use geolocation API ------------
            if (navigator && navigator.geolocation) {
                navigator.geolocation.getCurrentPosition(geoSuccess, geoError);
            }

            function geoSuccess(position) {
                var lat = position.coords.latitude;
                var lng = position.coords.longitude;
                codeLatLng(lat, lng);
            }

            function geoError(error) {
                $log.warn(error.message);
            }

            function codeLatLng(lat, lng) {
                var geocoder = new google.maps.Geocoder();
                var latlng = new google.maps.LatLng(lat, lng);
                geocoder.geocode({ latLng: latlng }, function (results, status) {
                    if (status == google.maps.GeocoderStatus.OK) {
                        if (results[1]) {
                            $scope.geo_city = results[1].address_components[0].long_name;
                            $scope.geo_state = results[1].address_components[1].long_name;
                            $scope.geo_country = results[1].address_components[2].long_name;

                        } else {
                            $log.warn("No results found");
                        }
                    } else {
                        $log.warn("Geocoder failed due to: " + status);
                    }
                });
            }
            // ----------- end geolocation API ----------------
            /*
            $http.get(endpoint.ipaddress).then(
                function (response) {
                    $scope.APIResolved++;
                    $scope.ip = response.data;

                  
                    $http.get(endpoint.geoip + $scope.ip).then(
                        function (response) {
                            $scope.APIResolved++;
                            location = response.data;
                        },
                        function (response) {
                            $scope.APIResolved++;
                            toastr.error("Cannot get location.");
                        });
                        


                }, function (response) {
                    $scope.APIResolved++;
                    toastr.error("Cannot get your Ip Address.");
                });
            */

            choriceAPISvc.surveyRetreiveSvc().get({ urltoken: tokenUrl },
                function (data) {
                    $scope.APIResolved++;

                    // store the visited cookie

                    if (data) {
                        angular.forEach(data.dtoQuestions, function (q) {

                            if (q.dtoItems.length > 0) {
                                angular.forEach(q.dtoItems, function (i) {
                                    i.IsChecked = false;
                                });
                            }
                        });
                        $scope.survey = data;
                    }
                    //toastr.success('Enjoy!');
                },
                function (data) {
                    $scope.APIResolved++;
                    toastr.error('Error load Survey');

                });


            function radioChecked(question, item) {
                angular.forEach(question.dtoItems, function (i) {
                    i.IsChecked = false;
                });
                item.IsChecked = true;
            }

            function submitSurvey() {
                $scope.APIMini = 1;
                $scope.APIResolved = 0;
                var dtoClientAnswers = [];

                angular.forEach($scope.survey.dtoQuestions, function (q) {
                    angular.forEach(q.dtoItems, function (i) {
                        dtoClientAnswers.push({ QuestionId: i.QuestionId, QuestionItemId: i.QuestionItemId, IsChecked: i.IsChecked });
                    });
                });

                // do we need to get geoIP?
                /*
                $http.get(endpoint.geoip + $scope.ip).then(
                    function (response) {
                        $scope.APIResolved++;
                        location = response.data;
                    },
                    function (response) {
                        $scope.APIResolved++;
                        toastr.error("Cannot get location.");
                    });
                    */
                /*
                {"as":"AS18250 Pacific Wireless Pty Ltd","city":"Mulgrave","country":"Australia","countryCode":"AU","isp":"Pacific Wireless Pty","lat":-37.9167,"lon":145.2,"org":"Pacific Wireless Pty","query":"202.134.40.162","region":"VIC","regionName":"Victoria","status":"success","timezone":"Australia/Melbourne","zip":"3170"}
                */

                var surveyClient = {
                    IPAddress: $scope.ip,
                    SurveyId: $scope.survey.SurveyId,
                    //City: $scope.geo_city ? $scope.geo_city : location.city,
                    //State: $scope.geo_state ? $scope.geo_state : location.regionName,
                    //Country: $scope.geo_country ? $scope.geo_country : location.country,
                    City: $scope.geo_city,
                    State: $scope.geo_state,
                    Country: $scope.geo_country,
                    dtoClientAnswers: dtoClientAnswers
                };

                choriceAPISvc.surveySaveSvc().save(surveyClient,
                    function (data) {
                        // data is the survey dto
                        $scope.submitSuccess = true;
                        $scope.APIResolved++;
                    },
                    function (data) {
                        toastr.error('Suvry Submit Failed. Please fresh the page.');
                    });

            }

            function backHome() {
                window.location = '/';
            }
        }]);

})();


/*  location:
      {city: "Balwyn"
      country_code: "AU"
      country_name: "Australia"
      ip: "115.64.103.98"
      latitude: -37.8117
      longitude: 145.081
      metro_code: 0
      region_code: "VIC"
      region_name: "Victoria"
      time_zone: "Australia/Melbourne"
      zip_code: "3103"}
  */
/* surveyClient:
{
      "Email": "string",
      "IPAddress": "string",
      "SurveyId": 0,
      "City": "string",
      "State": "string",
      "Country": "string",
      "dtoClientAnswers": [
        {
          "QuestionId": 0,
          "QuestionItemId": 0,
          "IsChecked": true
        }
      ]
    }
*/