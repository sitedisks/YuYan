(function () {
    'use strict';

    angular.module('yuyanApp')
        .controller('addEditResultCtrl', ['$scope', '$uibModalInstance', '$timeout', 'Upload', 'result', 'localStorageService', 'yuyanAPISvc', 'endpoint',
            function ($scope, $uibModalInstance, $timeout, Upload, result, localStorageService, yuyanAPISvc, endpoint) {

                $scope.saving = false;

                $scope.result = result;
                $scope.progressPercentage = 0;
                $scope.imageId = null;

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
        
                $scope.upload = function (file) {
                    yuyanAPISvc
                        .imageUploadSvc(file, 'survey', $scope.result.SurveyId)
                    .then(function (resp) {
                        $scope.imageId = resp.data.ImageId;
                        console.log('Success [' + resp.config.data.file.name + '] uploaded. Response: ' + resp.data.ImageId);
                    }, function (resp) {
                        console.log('Error status: ' + resp.status);
                    }, function (evt) {
                        $scope.progressPercentage = parseInt(100.0 * evt.loaded / evt.total);
                        console.log('progress: ' + $scope.progressPercentage + '% ' + evt.config.data.file.name);
                    });
                };

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