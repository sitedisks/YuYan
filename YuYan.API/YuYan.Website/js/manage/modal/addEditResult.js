(function () {
    'use strict';

    angular.module('yuyanApp')
        .controller('addEditResultCtrl', ['$scope', '$uibModalInstance', '$timeout', 'result', 'localStorageService', 'yuyanAPISvc',
            function ($scope, $uibModalInstance, $timeout, result, localStorageService, yuyanAPISvc) {

                $scope.saving = false;

                $scope.result = result;

                $timeout(function () {
                    $scope.slider = {
                        options: {
                            floor: 0,
                            ceil: 100,
                            step: 10,
                            showTicksValues: false
                        }
                    };
                }, 100);
        
                $scope.ok = function () {
                    $scope.saving = true;

                    if ($scope.result.SurveyResultId) {
                        // update
                        yuyanAPISvc.surveyResultCrudSvc().update({ surveyId: $scope.result.SurveyId, resultId: $scope.result.SurveyResultId }, $scope.result,
                            function (data) {
                                $scope.saving = false;
                                toastr.success("Result Updated!");
                                $uibModalInstance.close(data);
                            }, function (error) {
                                $scope.saving = false;
                                toastr.error("Save Result Error, please try again.");
                            });
                    }
                    else {
                        // save
                        yuyanAPISvc.surveyResultCrudSvc().save({ surveyId: $scope.result.SurveyId }, $scope.result,
                            function (data) {
                                $scope.saving = false;
                                toastr.success("Result Added!");
                                $uibModalInstance.close(data);
                            }, function (error) {
                                $scope.saving = false;
                                toastr.error("Save Result Error, please try again.");
                            });
                    }
                }

                $scope.cancel = function () {
                    $uibModalInstance.dismiss('cancel');
                }

            }]);
})();