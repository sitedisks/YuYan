(function () {
    'use strick';

    angular.module('yuyanApp')
        .controller('addEditQuestionCtrl', ['$scope', '$uibModalInstance', 'question', 'yuyanAPISvc', 'imageType', 'imageSize',
            function ($scope, $uibModalInstance, question, yuyanAPISvc, imageType, imageSize) {

                $scope.saving = false;

                $scope.isItemCountValid = false;
                $scope.imageProgress = 0;
                $scope.refImageProgress = 0;
                $scope.tempImageId = null;

                $scope.question = question;

                // functions
                $scope.addItem = addItem;
                $scope.removeItem = removeItem;
                $scope.getImageUrl = getImageUrl;

                // if there is refImageUrl
                $scope.refImageUrl = getImageUrl(question.RefImageId, imageSize.question);

                $scope.uploadItemImage = function (file) {
                    $scope.ImageUploading = true;
                    yuyanAPISvc
                        .imageUploadSvc(file, imageType.ItemRef, -1)
                     .then(function (resp) {
                         $scope.ImageUploading = false;
                         $scope.imageProgress = 0;
                         $scope.imageUrl = yuyanAPISvc.imageGetUrl(resp.data.ImageId, imageSize.questionItem);
                         console.log('Success [' + resp.config.data.file.name + '] uploaded. Response: ' + resp.data.ImageId);
                         $scope.tempImageId = resp.data.ImageId;
                     }, function (resp) {
                         console.log('Error status: ' + resp.status);
                     }, function (evt) {
                         $scope.imageProgress = parseInt(100.0 * evt.loaded / evt.total);
                         //console.log('progress: ' + $scope.imageProcess + '% ' + evt.config.data.file.name);
                     });
                }

                $scope.uploadRefImage = function (file) {
                    $scope.RefImageUploading = true;
                    yuyanAPISvc
                      .imageUploadSvc(file, imageType.QuestionRef, -1)
                   .then(function (resp) {
                       $scope.RefImageUploading = false;
                       $scope.refImageProgress = 0;
                       $scope.refImageUrl = yuyanAPISvc.imageGetUrl(resp.data.ImageId, imageSize.question);
                       console.log('Success [' + resp.config.data.file.name + '] uploaded. Response: ' + resp.data.ImageId);
                       $scope.question.RefImageId = resp.data.ImageId;
                   }, function (resp) {
                       console.log('Error status: ' + resp.status);
                   }, function (evt) {
                       $scope.refImageProgress = parseInt(100.0 * evt.loaded / evt.total);
                       //console.log('progress: ' + $scope.imageProcess + '% ' + evt.config.data.file.name);
                   });
                }


                if (question.dtoItems.length >= 2)
                    $scope.isItemCountValid = true;

                function getImageUrl(imageId, size) {
                    if (!size)
                        size = imageSize.questionItem; // default
                    
                    if (!isNullOrEmpty(imageId))
                        return yuyanAPISvc.imageGetUrl(imageId, size);
                    else
                        return "";
                }

                function addItem() {

                    var order = $scope.question.dtoItems.length + 1;
                    var item = {
                        QuestionId: $scope.question.QuestionId,
                        ItemDescription: $scope.item,
                        ItemOrder: order,
                        ImageId: $scope.tempImageId
                    }

                    $scope.question.dtoItems.push(item);
                    $scope.tempImageId = null;
                    $scope.item = "";
                    $scope.imageUrl = null;
                    ItemCountValid();
                }

                function removeItem(i) {
                    var index = $scope.question.dtoItems.indexOf(i);

                    $scope.question.dtoItems.splice(index, 1);
                    var order = 1;
                    angular.forEach($scope.question.dtoItems, function (item) {
                        item.ItemOrder = order++;
                    });
                    ItemCountValid();
                }

                function ItemCountValid() {
                    if ($scope.question.dtoItems.length < 2 || $scope.question.dtoItems.length > 6)
                        $scope.isItemCountValid = false;
                    else
                        $scope.isItemCountValid = true;
                }

                $scope.ok = function () {
                    $scope.saving = true;

                    if ($scope.question.QuestionId) {
                        //update
                        yuyanAPISvc.questionCrudSvc().update({ surveyId: $scope.question.SurveyId, questionId: $scope.question.QuestionId }, $scope.question,
                            function (data) {
                                $scope.saving = false;
                                toastr.success("Question Updated!");
                                $uibModalInstance.close(data);
                            }, function (data) {
                                $scope.saving = false;
                                toastr.error("Save Question Error, please try again.");
                            });
                    } else {
                        //save
                        yuyanAPISvc.questionCrudSvc().save({ surveyId: $scope.question.SurveyId }, $scope.question,
                            function (data) {
                                $scope.saving = false;
                                toastr.success("Question Added!");
                                $uibModalInstance.close(data);
                            }, function (data) {
                                $scope.saving = false;
                                toastr.error("Save Question Error, please try again.");
                            });
                    }

                }


                $scope.cancel = function () {
                    $uibModalInstance.dismiss('cancel');
                };

            }]);
})();