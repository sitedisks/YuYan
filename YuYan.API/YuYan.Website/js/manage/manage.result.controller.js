(function () {
    'use strict';

    angular.module('yuyanApp').controller('manageResultCtrl', ['$scope', '$stateParams', '$state', '$uibModal', 'yuyanAPISvc',
        function ($scope, $stateParams, $state, $uibModal, yuyanAPISvc) {

            $scope.survey = $stateParams.survey;
            $scope.APIMini = 1;
            $scope.APIResolved = 1;

            // functions
            $scope.goHome = goHome;
            $scope.goSurvey = goSurvey;
            $scope.deleteResult = deleteResult;
            $scope.addEditResult = addEditResult;

            yuyanAPISvc.surveyResultCrudSvc().query({ surveyId: $scope.survey.SurveyId },
                function (data) {
                    $scope.results = data;
                },
                function (error) {
                    toastr.error('Failed to retreive result. Please refresh.');
                });


            function goHome() {
                $state.go('home');
            }

            function goSurvey() {
                $state.go('survey', {}, { location: false });
            }

            function deleteResult(result) {
                var modalInstance = $uibModal.open({
                    animation: true,
                    templateUrl: 'components/manage/modal/deleteResult.html',
                    controller: 'deleteResultCtrl',
                    size: 'md',
                    resolve: {
                        result: result
                    }
                });

                modalInstance.result.then(function (result) {

                    yuyanAPISvc.surveyResultCrudSvc().remove({ surveyId: result.SurveyId, resultId: result.SurveyResultId },
                        function (data) {

                            var index = 0;
                            angular.forEach($scope.results, function (r, key) {
                                if (r.SurveyResultId == result.SurveyResultId)
                                    index = key;
                            });

                            $scope.results.splice(index, 1);

                            toastr.success("Result Deleted!");
                        }, function (data) {

                        });
                }, function () {
                    // dismissed log
                });
            }

            function addEditResult(result) {
                if (!result) {
                    result = {
                        MinScore: 0,
                        MaxScore: 100,
                        SurveyId: $scope.survey.SurveyId,
                        Title: '',
                        Description: ''
                    };
                }

                var modalInstance = $uibModal.open({
                    animation: true,
                    templateUrl: 'components/manage/modal/addEditResult.html',
                    controller: 'addEditResultCtrl',
                    size: 'md',
                    resolve: {
                        result: angular.copy(result)
                    }
                });
           
                modalInstance.result.then(function (data) {
                    // renew UI dom

                    var isNew = true;

                    angular.forEach($scope.results, function (r) {
                        if (r.SurveyResultId == data.SurveyResultId) {
                            r.MinScore = data.MinScore;
                            r.MaxScore = data.MaxScore;
                            r.SurveyId = data.SurveyId;
                            r.Title = data.Title;
                            r.Description = data.Description;
                            r.ShowStatistics = data.ShowStatistics;
                            r.ShowScore = data.ShowScore;
                            isNew = false;
                        }
                    });

                    if (isNew) {
                        $scope.results.push(data);
                    }

                }, function () {
                    // dismissed log
                });
            }

        }]);
})();
