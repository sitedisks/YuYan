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
                        yuyanAPISvc.surveyClientAnswerDicSvc().get({ surveyId: survey.SurveyId },
                           function (data) {
                               $scope.checkedHashtb = data;
                               $scope.APIResolved++;
                           }, function () {
                               toastr.error("Error please refresh the page.");
                           });

                    }, function (error) {
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