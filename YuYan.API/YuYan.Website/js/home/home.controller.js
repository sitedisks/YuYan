(function () {
    'use strict';
    angular.module('yuyanApp').controller('surveyCtrl',
    ['$scope', '$rootScope', '$uibModal', '$translate', 'localStorageService', 'yuyanAPISvc',
        function ($scope, $rootScope, $uibModal, $translate, localStorageService, yuyanAPISvc) {

            $scope.dtoQuestions;
            $scope.DefaultQuestionType;
            $scope.QID;
            $scope.IID;
            $scope.showAddItem;
            $scope.disableNext;
            $scope.disableController;
            $rootScope.progressing;

            // functions
            $scope.toggleType = toggleType;
            $scope.addQuestion = addQuestion;
            $scope.addItem = addItem;
            $scope.nextQuestion = nextQuestion;
            $scope.previewSurvey = previewSurvey;
            $scope.saveSurvey = saveSurvey;
            $scope.reset = reset;

            // initial call
            reset();

            // broadcast on
            $scope.$on('reset', function (event, args) {
                reset();
            });

            // function implements:
            function toggleType() {
                if ($scope.DefaultQuestionType == 1) {
                    $scope.DefaultQuestionType = 2;
             
                }
                else {
                    $scope.DefaultQuestionType = 1;
                  
                }
            }

            function addQuestion() {
                $scope.QID++;
                var question = {
                    QuestionId: $scope.QID,
                    QuestionOrder: $scope.QID,
                    Question: $scope.question,
                    QuestionType: $scope.DefaultQuestionType,
                    dtoItems: []
                };

                $scope.dtoQuestions.push(question);
                $scope.question = "";
                $scope.item = "";
                $scope.showAddItem = true;
            }

            function addItem(QID) {
                $scope.IID++;
                var item = {
                    QuestionItemId: $scope.IID,
                    //ItemOrder: $scope.IID,
                    QuestionId: QID,
                    ItemDescription: $scope.item
                };

                angular.forEach($scope.dtoQuestions, function (question) {
                    if (question.QuestionId == QID) {
                        question.dtoItems.push(item);
                        if (question.dtoItems.length >= 6) {
                            $scope.showAddItem = false;
                        }
                        if (question.dtoItems.length >= 2) {
                            $scope.disableNext = false;
                        }
                    }
                });

                $scope.item = "";
                $scope.disableController = false;
            }

            function nextQuestion() {
                $scope.showAddItem = false;
                $scope.disableNext = true;
            }

            function previewSurvey() {
                var survey = {
                    Title: "Preview",
                    ShortDesc: "* Survey Preview",
                    LongDesc: "* Register to access full functions",
                    dtoQuestions: $scope.dtoQuestions
                };

                var modalInstance = $uibModal.open({
                    animation: true,
                    templateUrl: 'templates/previewModal.html',
                    controller: 'previewModalCtrl',
                    size: 'md',
                    resolve: {
                        survey: survey
                    }
                });

                modalInstance.result.then(function () {

                }, function () {
                    // dismissed log
                });
            }

            function saveSurvey() {

                var localSessionToken = localStorageService.get('authorizationData');

                if (localSessionToken) {
                    yuyanAPISvc.sessionCheckSvc().get({ sessionId: localSessionToken.token },
                        function (data) {
                            if (data.SessionId && data.IsActive) {
                                surveyTitle(data.UserId);
                            } else {
                                surveyTitle();
                            }
                        }, function (data) {
                            toastr.error('User Session Check failed. Please refresh.');
                        });
                }
                else {
                    // no session token, not login yet
                    surveyTitle();

                }

            }

            function reset() {
                $scope.dtoQuestions = [];
                $scope.DefaultQuestionType = 1;
                $scope.QID = 0;
                $scope.IID = 0;
                $scope.showAddItem = false;
                $scope.disableNext = true;
                $scope.disableController = true;
                $rootScope.progressing = false;
            }

            function surveyTitle(userId) {
                var modalInstance = $uibModal.open({
                    animation: true,
                    templateUrl: 'templates/surveyModal.html',
                    controller: 'surveyModalCtrl',
                    size: 'md',
                    resolve: {
                        loggedIn: function () {
                            if (userId)
                                return true;
                            else
                                return false;
                        }
                    }
                });

                modalInstance.result.then(function (surveyObj) {

                    if (userId) {
                        // save the survey
                        var survey = {
                            Title: surveyObj.Title,
                            ShortDesc: surveyObj.ShortDesc,
                            UserId: userId,
                            dtoQuestions: $scope.dtoQuestions
                        };

                        $rootScope.progressing = true;

                        yuyanAPISvc.surveyCrudSvc().save(survey,
                            function (data) {
                                toastr.success("Survey Saved!");
                                reset();
                            }, function (data) {
                                toastr.error("Save Survey Error");
                                reset();
                            });
                    }
                    else {
                        // user not login
                        var survey = {
                            Title: surveyObj.Title,
                            ShortDesc: surveyObj.ShortDesc,
                            dtoQuestions: $scope.dtoQuestions
                        };

                        $scope.$parent.userLogin('login', survey);
                    }


                }, function () {
                    // dismissed log
                });
            }

        }]);

})();
