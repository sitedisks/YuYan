(function () {
    'use strick';

    angular.module('yuyanApp')
        .controller('addEditQuestionCtrl', ['$scope', '$uibModalInstance', 'question', 'yuyanAPISvc',
            function ($scope, $uibModalInstance, question, yuyanAPISvc) {

                $scope.saving = false;

                $scope.isItemCountValid = false;

                $scope.question = question;

                $scope.addItem = addItem;
                $scope.removeItem = removeItem;


                if (question.dtoItems.length >= 2)
                    $scope.isItemCountValid = true;

                function addItem() {

                    var order = $scope.question.dtoItems.length + 1;
                    var item = {
                        QuestionId: $scope.question.QuestionId,
                        ItemDescription: $scope.item,
                        ItemOrder: order
                    }

                    $scope.question.dtoItems.push(item);
                    $scope.item = "";
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