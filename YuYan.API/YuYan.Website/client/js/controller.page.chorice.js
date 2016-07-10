(function () {
    'use strict';
    angular.module('choriceApp').controller('choricePageCtrl', ['$scope', '$http', '$log', '$stateParams', 'choriceAPISvc', 'endpoint',
        function ($scope, $http, $log, $stateParams, choriceAPISvc, endpoint) {

            var tokenUrl = $stateParams.tokenUrl;

            $scope.APIMini = 1;
            $scope.APIResolved = 0;
            $scope.submitSuccess = false;
            $scope.googleGeo = {
                geo_city: null,
                geo_state: null,
                geo_country: null
            };
            $scope.result = null;
            $scope.page = 1;
            $scope.questionCount = 0;

            //$scope.geo_city = null;
            //$scope.geo_state = null;
            //$scope.geo_country = null;

            // function register
            $scope.radioChecked = radioChecked;
            $scope.submitSurvey = submitSurvey;
            $scope.backHome = backHome;
            $scope.pageOrder = pageOrder;
            $scope.prePage = prePage;
            $scope.nextPage = nextPage;

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
                            $scope.googleGeo.geo_city = results[1].address_components[0].long_name;
                            $scope.googleGeo.geo_state = results[1].address_components[1].long_name;
                            $scope.googleGeo.geo_country = results[1].address_components[2].long_name;

                        } else {
                            $log.warn("No results found");
                        }
                    } else {
                        $log.warn("Geocoder failed due to: " + status);
                    }
                });
            }
            // ----------- end geolocation API ----------------

            choriceAPISvc.surveyRetreiveSvc().get({ urltoken: tokenUrl },
                function (data) {
                    $scope.APIResolved++;

                    // store the visited cookie

                    if (data) {
                        $scope.questionCount = data.dtoQuestions.length;
                        angular.forEach(data.dtoQuestions, function (q) {

                            if (q.dtoItems.length > 0) {
                                angular.forEach(q.dtoItems, function (i) {
                                    i.IsChecked = false;
                                });
                            }
                        });
                        $scope.survey = data;

                        if (!isNullOrEmpty(data.BannerId))
                            $scope.bannerUrl = choriceAPISvc.imageGetUrl(data.BannerId, 760);
                        if (!isNullOrEmpty(data.LogoId))
                            $scope.logoUrl = choriceAPISvc.imageGetUrl(data.LogoId, 760);
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
                $scope.APIMini = 2;
                $scope.APIResolved = 0;
                var dtoClientAnswers = [];
                var totalScore = 0;

                angular.forEach($scope.survey.dtoQuestions, function (q) {
                    angular.forEach(q.dtoItems, function (i) {
                        dtoClientAnswers.push({ QuestionId: i.QuestionId, QuestionItemId: i.QuestionItemId, IsChecked: i.IsChecked });
                        if (i.IsChecked && i.Score != null) {
                            totalScore += i.Score
                        }
                    });
                });

                var surveyClient = {
                    IPAddress: $scope.ip,
                    SurveyId: $scope.survey.SurveyId,
                    TotalScore: totalScore,
                    City: $scope.googleGeo.geo_city,
                    State: $scope.googleGeo.geo_state,
                    Country: $scope.googleGeo.geo_country,
                    dtoClientAnswers: dtoClientAnswers
                };

                choriceAPISvc.surveySaveSvc().save(surveyClient,
                    function (data) {
                        // data is the survey dto
                        $scope.APIResolved++;
                        choriceAPISvc.surveyResultSvc().get(
                            { surveyId: surveyClient.SurveyId, score: surveyClient.TotalScore },
                            function (result) {

                                $scope.APIResolved++;
                                $scope.submitSuccess = true;
                                $scope.result = result;

                                if ($scope.survey.ShowReport) {
                                    // load the answer status 
                                    $scope.APIMini++;
                                    loadAnswerDic();
                                }

                            },
                            function (error) {
                                toastr.error('ZZZ sleep');
                            });
                    },
                    function (data) {
                        toastr.error('Suvry Submit Failed. Please fresh the page.');
                    });

            }

            function loadAnswerDic() {
                choriceAPISvc.surveyClientAnswerDicSvc().get({ surveyId: $scope.survey.SurveyId },
                    function (data) {
                        $scope.checkedHashtb = data;
                        $scope.APIResolved++;
                    }, function () {
                        toastr.error("Error please refresh the page.");
                    });
            }

            function pageOrder(question) {
                if (question.QuestionOrder == $scope.page)
                    return true;
                else
                    return false;
            }

            function prePage() {
                $scope.page--;
            }

            function nextPage() {
                $scope.page++;
            }

            function backHome() {
                window.location = '/';
            }
        }]);

})();