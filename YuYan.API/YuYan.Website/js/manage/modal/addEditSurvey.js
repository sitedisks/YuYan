(function () {
    'use strick';

    angular.module('yuyanApp')
        .controller('addEditSurveyCtrl', ['$scope', '$uibModalInstance', 'survey', 'localStorageService', 'yuyanAPISvc',
            function ($scope, $uibModalInstance, survey, localStorageService, yuyanAPISvc) {

                $scope.saving = false;

                $scope.survey = survey;

                $scope.ok = function () {
                    $scope.saving = true;

                    if ($scope.survey.SurveyId) {
                        //update
                        yuyanAPISvc.surveyCrudSvc().update({ surveyId: $scope.survey.SurveyId }, $scope.survey,
                            function (data) {
                                $scope.saving = false;
                                toastr.success("Survey Updated!");
                                $uibModalInstance.close(data);
                            }, function (data) {
                                $scope.saving = false;
                                toastr.error("Update Survey Error, please try again.");
                            });
                    }
                    else {
                        //save
                        yuyanAPISvc.surveyCrudSvc().save($scope.survey,
                           function (data) {
                               $scope.saving = false;
                               toastr.success("Survey Added!");
                               $uibModalInstance.close(data);
                           },
                           function (data) {
                               $scope.saving = false;
                               toastr.error("Save Survey Error, please try again.");
                           });
                    }

                };

                $scope.cancel = function () {
                    $uibModalInstance.dismiss('cancel');
                };

            }]);
})();