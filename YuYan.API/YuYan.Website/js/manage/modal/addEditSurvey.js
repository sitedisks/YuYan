(function () {
    'use strick';

    angular.module('yuyanApp')
        .controller('addEditSurveyCtrl', ['$scope', '$uibModalInstance', 'survey', 'yuyanAPISvc', 'imageType',
            function ($scope, $uibModalInstance, survey, yuyanAPISvc, imageType) {

                $scope.saving = false;

                $scope.survey = survey;
                $scope.bannerProgress = 0;
                $scope.logoProgress = 0;

                //banner
                $scope.uploadBanner = function (file) {
                    $scope.bannerUploading = true;
                    yuyanAPISvc
                      .imageUploadSvc(file, imageType.SurveyBanner, $scope.survey.SurveyId)
                    .then(function (resp) {
                        $scope.bannerUploading = false;
                        $scope.bannerUrl = yuyanAPISvc.imageGetUrl(resp.data.ImageId, 760);
                        console.log('Success [' + resp.config.data.file.name + '] uploaded. Response: ' + resp.data.ImageId);
                    }, function (resp) {
                        console.log('Error status: ' + resp.status);
                    }, function (evt) {
                        $scope.bannerProgress = parseInt(100.0 * evt.loaded / evt.total);
                        console.log('progress: ' + $scope.bannerProgress + '% ' + evt.config.data.file.name);
                    });
                }

                //logo
                $scope.uploadLogo = function (file) {
                    $scope.logoUploading = true;
                    yuyanAPISvc
                      .imageUploadSvc(file, imageType.SurveyLogo, $scope.survey.SurveyId)
                    .then(function (resp) {
                        $scope.logoUploading = false;
                        $scope.logoUrl = yuyanAPISvc.imageGetUrl(resp.data.ImageId, 760);
                        console.log('Success [' + resp.config.data.file.name + '] uploaded. Response: ' + resp.data.ImageId);
                    }, function (resp) {
                        console.log('Error status: ' + resp.status);
                    }, function (evt) {
                        $scope.logoProgress = parseInt(100.0 * evt.loaded / evt.total);
                        console.log('progress: ' + $scope.logoProgress + '% ' + evt.config.data.file.name);
                    });
                }

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