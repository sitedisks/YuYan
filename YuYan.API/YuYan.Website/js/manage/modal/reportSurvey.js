(function () {
    'use strick';

    angular.module('yuyanApp')
        .controller('reportSurveyCtrl', ['$scope', '$uibModalInstance', '$timeout', 'survey', 'localStorageService', 'yuyanAPISvc', 'uiGmapGoogleMapApi', 'uiGmapIsReady',
            function ($scope, $uibModalInstance, $timeout, survey, localStorageService, yuyanAPISvc, uiGmapGoogleMapApi, uiGmapIsReady) {

                var geocoder = new google.maps.Geocoder();
                var lat = -37.8140000, lng = 144.9633200; // default melbourne 
                var i = 0;

                $scope.survey = survey;
                $scope.geoStatus = {};
                $scope.checkedHashtb;
                $scope.APIMini = 2;
                $scope.APIResolved = 0;

                // --- google chart start
                $scope.charType = "BarChart"; // "PieChart"; "ColumnChart";
                $scope.chartGroup = [];

                // --- google map markers
                $scope.markers = [];

                // --- functions
                $scope.clickMe = clickMe;
                $scope.ok = ok;
                $scope.cancel = cancel;



                yuyanAPISvc.surveyClientAnswerDicSvc().get({ surveyId: survey.SurveyId },
                    function (data) {
                        $scope.checkedHashtb = data;

                        $timeout(function () {
                            //preparing the chart data
                            angular.forEach($scope.survey.dtoQuestions, function (question) {

                                var rows = [];

                                angular.forEach(question.dtoItems, function (item) {
                                    var row = { c: [{ v: item.ItemDescription }, { v: data[item.QuestionItemId] == null ? 0 : data[item.QuestionItemId] }, ] };
                                    rows.push(row);
                                });

                                var questionChart = {
                                    type: $scope.charType,
                                    data: {
                                        "cols": [{ id: "t", label: "Items", type: "string" }, { id: "s", label: "Counts", type: "number" }],
                                        "rows": rows
                                    },
                                    options: { 'title': question.Question }
                                };

                                $scope.chartGroup.push(questionChart);
                            });

                        }, 300);


                        $scope.APIResolved++;
                    }, function () {
                        toastr.error("Error please refresh the page.");
                    });

                // geo location
                yuyanAPISvc.surveyClientReportSvc().query({ surveyId: survey.SurveyId },
                    function (data) {
                        $scope.surveyClient = data;

                        angular.forEach(data, function (client) {
                            var address = client.City + ' ' + client.State + ', ' + client.Country;
                            if ($scope.geoStatus[address] === undefined)
                                $scope.geoStatus[address] = { name: address, count: 1 };
                            else
                                $scope.geoStatus[address] = { name: address, count: $scope.geoStatus[address].count + 1 };
                        });

                        doMap();

                        $scope.APIResolved++;
                    }, function (error) {
                        toastr.error("Error please refresh the page.");
                    });


                function doMap() {

                    // set map
                    uiGmapGoogleMapApi.then(function (maps) {
                        //lodashFix();
                        //geocoder.geocode({});
                        $scope.map = {
                            center: { latitude: lat, longitude: lng },
                            zoom: 12,
                            options: { scrollwheel: true },
                            control: {},
                            events: {
                                tilesloaded: function (map) {
                                    $scope.$apply(function () {
                                        google.maps.event.trigger(map, 'resize');
                                    });
                                }
                            }
                        };

                    });

                    // set marker
                    angular.forEach($scope.geoStatus, function (geoMarker) {
                        geocoder.geocode({ 'address': geoMarker.name }, function (results, status) {
                            if (status == google.maps.GeocoderStatus.OK) {
                                var center = results[0].geometry.location;
                                var lat = center.lat();
                                var lng = center.lng();
                            }

                            $scope.markers.push({
                                id: i++,
                                coords: { latitude: lat, longitude: lng },
                                area: geoMarker.name,
                                count: geoMarker.count
                            });

                        });
                    });
                }

                
                function clickMe() {
                    toastr.success("Docker it!");
                }

                function ok () {
                    $uibModalInstance.close(surveyClient);
                };

                function cancel () {
                    $uibModalInstance.dismiss('cancel');
                };

            }]);
})();