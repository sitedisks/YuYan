(function () {
    'use strict';

    angular.module('yuyanApp').controller('manageQuestionCtrl', ['$scope', '$stateParams', '$state', '$uibModal', 'yuyanAPISvc',
        function ($scope, $stateParams, $state, $uibModal, yuyanAPISvc) {

            $scope.survey = $stateParams.survey;
            $scope.APIMini = 1;
            $scope.APIResolved = 1;
            var survey = $scope.survey;

            // functions
            $scope.goHome = goHome;
            $scope.goSurvey = goSurvey;
            $scope.deleteQuestion = deleteQuestion;
            $scope.addEditQuestion = addEditQuestion;
            $scope.getImageUrl = getImageUrl;

            function getImageUrl(imageId) {
                if (imageId != null)
                    return yuyanAPISvc.imageGetUrl(imageId, 400);
                else
                    return "";
            }

            function goHome() {
                $state.go('home');
            }

            function goSurvey() {
                $state.go('survey', {}, { location: false });
            }

            function deleteQuestion(question) {
                //$scope.deleteQuestion = null;
                var modalInstance = $uibModal.open({
                    animation: true,
                    templateUrl: 'components/manage/modal/deleteQuestion.html',
                    controller: 'deleteQuestionCtrl',
                    size: 'md',
                    resolve: {
                        question: question
                    }
                });

                modalInstance.result.then(function (question) {
                 
                    yuyanAPISvc.questionCrudSvc().remove({ surveyId: question.SurveyId, questionId: question.QuestionId },
                        function (data) {
                         
                            var index = 0;
                            angular.forEach($scope.survey.dtoQuestions, function (q, key) {
                                if (q.QuestionId == question.QuestionId)
                                    index = key;
                            });

                            $scope.survey.dtoQuestions.splice(index, 1);

                            toastr.success("Survey Deleted!");
                        }, function (data) {

                        });
                }, function () {
                    // dismissed log
                });
            }

            function addEditQuestion(question) {
                if (!question)
                {
                    question = {
                        SurveyId: $scope.survey.SurveyId,
                        QuestionType: 1,
                        Question: '',
                        dtoItems: []
                    };
                }

                var modalInstance = $uibModal.open({
                    animation: true,
                    templateUrl: 'components/manage/modal/addEditQuestion.html',
                    controller: 'addEditQuestionCtrl',
                    size: 'md',
                    resolve: {
                        question: angular.copy(question)
                    }
                });

                modalInstance.result.then(function (data) {
                    // renew UI dom

                    var isNew = true;
                    // refresh the survey dtoQuestions
                    angular.forEach($scope.survey.dtoQuestions, function (q) {
                        if (q.QuestionId == data.QuestionId) {
                            q.Question = data.Question;
                            q.IsActive = data.IsActive;
                            q.IsDeleted = data.IsDeleted;
                            q.QuestionId = data.QuestionId;
                            q.QuestionOrder = data.QuestionOrder;
                            q.QuestionType = data.QuestionType;
                            q.dtoItems = data.dtoItems;
                            q.SurveyId = data.SurveyId;

                            isNew = false;
                        }
                    });

                    if (isNew) {
                        $scope.survey.dtoQuestions.push(data);
                    }


                }, function () {
                    // dismissed log
                });
            }


        }]);
})();