(function () {
    'use strick';

    angular.module('yuyanApp')
        .controller('reportSurveyCtrl', ['$scope', '$uibModalInstance', '$timeout', 'survey', 'localStorageService', 'yuyanAPISvc', 'uiGmapGoogleMapApi',
            function ($scope, $uibModalInstance, $timeout, survey, localStorageService, yuyanAPISvc, uiGmapGoogleMapApi) {

                $scope.survey = survey;
                $scope.geoStatus = {};
                $scope.checkedHashtb;
                $scope.APIMini = 2;
                $scope.APIResolved = 0;


                // --- google chart start

                $scope.charType = "BarChart";
                $scope.chartGroup = [];


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

                //$scope.myChartObject.type = "PieChart";
                //$scope.myChartObject.type = "ColumnChart";

                // --- google chart end

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

                        $scope.APIResolved++;
                    }, function (error) {
                        toastr.error("Error please refresh the page.");
                    });

                // question result 
                /*
                yuyanAPISvc.surveyClientAnswerDicSvc().get({ surveyId: survey.SurveyId },
                    function (data) {
                        $scope.checkedHashtb = data;

                        //preparing the chart data


                        $scope.APIResolved++;
                    }, function () {
                        toastr.error("Error please refresh the page.");
                    });
                    */


                $scope.ok = function () {
                    $uibModalInstance.close(surveyClient);
                };

                $scope.cancel = function () {
                    $uibModalInstance.dismiss('cancel');
                };

            }]);
})();