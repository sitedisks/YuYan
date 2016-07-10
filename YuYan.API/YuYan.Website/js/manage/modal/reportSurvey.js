(function () {
    'use strick';

    angular.module('yuyanApp')
        .controller('reportSurveyCtrl', ['$scope', '$uibModalInstance', 'survey', 'localStorageService', 'yuyanAPISvc',
            function ($scope, $uibModalInstance, survey, localStorageService, yuyanAPISvc) {

                $scope.survey = survey;
                $scope.geoStatus = {};
                $scope.checkedHashtb;
                $scope.APIMini = 2;
                $scope.APIResolved = 0;


                // --- google chart start
                $scope.chartGroup = [myChartObject1, myChartObject2];

                var myChartObject1 = {
                    type: "BarChart",
                    data: {
                        "cols": [
                                    { id: "t", label: "Items", type: "string" },
                                    { id: "s", label: "Value", type: "number" }
                        ],
                        "rows": [
                                    { c: [{ v: "Mushrooms" }, { v: 3 }, ] },
                                    { c: [{ v: "Onions" }, { v: 3 }, ] },
                                    { c: [{ v: "Olives" }, { v: 31 }] },
                                    { c: [{ v: "Zucchini" }, { v: 1 }, ] },
                                    { c: [{ v: "Pepperoni" }, { v: 2 }, ] }
                        ]
                    },
                    options: { 'title': 'How Much Pizza I Ate Last Night' }
                };

                var myChartObject2 = {
                    type: "BarChart",
                    data: {
                        "cols": [
                                    { id: "t", label: "Items", type: "string" },
                                    { id: "s", label: "Value", type: "number" }
                        ],
                        "rows": [
                                    { c: [{ v: "Mushrooms" }, { v: 3 }, ] },
                                    { c: [{ v: "Onions" }, { v: 3 }, ] },
                                    { c: [{ v: "Olives" }, { v: 31 }] },
                                    { c: [{ v: "Zucchini" }, { v: 1 }, ] },
                                    { c: [{ v: "Pepperoni" }, { v: 2 }, ] }
                        ]
                    },
                    options: { 'title': 'Second' }
                };


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
                yuyanAPISvc.surveyClientAnswerDicSvc().get({ surveyId: survey.SurveyId },
                    function (data) {
                        $scope.checkedHashtb = data;

                        //preparing the chart data


                        $scope.APIResolved++;
                    }, function () {
                        toastr.error("Error please refresh the page.");
                    });


                $scope.ok = function () {
                    $uibModalInstance.close(surveyClient);
                };

                $scope.cancel = function () {
                    $uibModalInstance.dismiss('cancel');
                };

            }]);
})();