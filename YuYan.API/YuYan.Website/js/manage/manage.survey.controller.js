(function () {
    'use strict';

    angular.module('yuyanApp').controller('manageCtrl', ['$scope', '$rootScope', '$state', '$uibModal', 'yuyanAPISvc', 'yuyanAuthSvc', 'uiGmapGoogleMapApi',
        function ($scope, $rootScope, $state, $uibModal, yuyanAPISvc, yuyanAuthSvc, uiGmapGoogleMapApi) {

            $scope.APIMini = 2;
            $scope.APIResolved = 0;

            // functions
            $scope.goHome = goHome;
            $scope.goQuestion = goQuestion;
            $scope.goResult = goResult;
            $scope.previewSurvey = previewSurvey;
            $scope.deleteSurvey = deleteSurvey;
            $scope.addEditSurvey = addEditSurvey;
            $scope.report = report;

            // pagination 
            $scope.row = 10;
            $scope.totalItems = 0;
            $scope.currentPage = 1;
            $scope.maxSize = 3;


            suveryListInit(true);
            //doMap();

            $scope.$watch("currentPage", function (newValue, oldValue) {

                if (newValue != oldValue)
                {
                    //$scope.APIResolved--;
                    surveyRetreive(newValue, false);
                }
                    
            });

            function suveryListInit(isInit) {

                if (!yuyanAuthSvc.isLogin) {
                    $state.go('home');
                } else {

                    yuyanAPISvc.surveyCountBySession().get({},
                        function (data) {
                            if (isInit)
                                $scope.APIResolved++;
                            $scope.totalItems = data.SurveyCount;

                            surveyRetreive($scope.currentPage, isInit);
                        },
                        function (data) {
                            toastr.error('Failed to retreive survey. Please refresh.');
                        });
                }
            }

            function surveyRetreive(page, isInit) {
                yuyanAPISvc.surveyCrudSvc().query({ page: page, row: $scope.row },
                      function (data) {
                          if(isInit)
                            $scope.APIResolved++;
                          $scope.surveys = data;
                      }, function (data) {
                          toastr.error('Failed to retreive survey. Please refresh.');
                      });
            }

            function goHome() {
                $state.go('home');
            }

            function goQuestion(survey) {
                $state.go('question', { survey: survey }, { location: false });
            }

            function goResult(survey) {
                $state.go('result', { survey: survey }, { location: false });
            }

            function previewSurvey(survey) {
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

            function deleteSurvey(survey) {
                var modalInstance = $uibModal.open({
                    animation: true,
                    templateUrl: 'components/manage/modal/deleteSurvey.html',
                    controller: 'deleteSurveyCtrl',
                    size: 'md',
                    resolve: {
                        survey: survey
                    }
                });

                modalInstance.result.then(function (survey) {
                    yuyanAPISvc.surveyCrudSvc().remove({ surveyId: survey.SurveyId },
                        function (data) {
                            toastr.success("Survey Deleted!");
                            //$scope.APIResolved--;
                            suveryListInit(false);
                        }, function (data) {
                            toastr.error("Remove Survey Error, please try again.");
                        });
                }, function () {
                    // dismissed log
                });
            }

            function addEditSurvey(survey) {

                if (!survey) {
                    // empty survey then
                    survey = {
                        Title: '',
                        ShortDesc: '',
                        LongDesc: ''
                    };
                }

                var modalInstance = $uibModal.open({
                    animation: true,
                    templateUrl: 'components/manage/modal/addEditSurvey.html',
                    controller: 'addEditSurveyCtrl',
                    size: 'md',
                    resolve: {
                        survey: angular.copy(survey)
                    }
                });

                modalInstance.result.then(function (data) {
                    //$scope.APIResolved--;
                    suveryListInit(false);
                }, function () {
                    // dismissed log
                });
            }

            function report(survey) {
                var modalInstance = $uibModal.open({
                    animation: true,
                    templateUrl: 'components/manage/modal/reportSurvey.html',
                    controller: 'reportSurveyCtrl',
                    size: 'lg',
                    resolve: {
                        survey: angular.copy(survey)
                    }
                });

                modalInstance.result.then(function (data) {
              
                }, function () {
                    // dismissed log
                });
            }

            // google map Test
            /*
            function doMap() {
                uiGmapGoogleMapApi.then(function (maps) {
 
                    var lat = -37.8140000, lng = 144.9633200; // default melbourne 
                    $scope.map = {
                        center: { latitude: lat, longitude: lng },
                        zoom: 12,
                        options: { scrollwheel: true },
                        control: {}
                    };

                });
            }*/

        }]);

})();