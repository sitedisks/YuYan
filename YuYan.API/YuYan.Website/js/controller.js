(function () {
    'use strict';
    angular.module('yuyanApp').controller('masterCtrl', ['$scope', '$rootScope', '$state', '$translate', '$uibModal', 'localStorageService', 'yuyanAPISvc', 'yuyanAuthSvc',
        function ($scope, $rootScope, $state, $translate, $uibModal, localStorageService, yuyanAPISvc, yuyanAuthSvc) {
            $scope.userLogin = userLogin;
            $scope.userLogout = userLogout;
            $scope.goHome = goHome;
            $scope.goManagement = goManagement;
            $scope.changeLanguage = changeLanguage;
            $rootScope.isLogin = null;
            $rootScope.sessionChecking = false;

            checkSession();

            function userLogin(mode, survey) {
                var modalInstance = $uibModal.open({
                    animation: true,
                    templateUrl: 'templates/userModal.html',
                    controller: 'userModalCtrl',
                    size: 'md',
                    resolve: {
                        mode: function () {
                            return mode;
                        }
                    }
                });

                modalInstance.result.then(function (user) {

                    localStorageService.set('authorizationData', { token: user.CurrentSession.SessionId });
                    if (mode == 'login')
                        toastr.success('Welcome back!', user.Email);
                    else
                        toastr.success('Welcome to Chorice!', user.Email);

                    $rootScope.isLogin = true;
                    yuyanAuthSvc.isLogin = true;

                    if (survey) {
                        $rootScope.progressing = true;
                        survey.UserId = user.UserId;
                        saveSurvey(survey);
                    } else {
                        $state.go('survey', {}, { location: false });
                    }

                }, function () {
                    // dismissed log
                });
            }

            function userLogout() {
                var localSessionToken = localStorageService.get('authorizationData');
                if (localSessionToken) {

                    $rootScope.sessionChecking = true;

                    yuyanAPISvc.userLogoutSvc().remove({ sessionId: localSessionToken.token },
                        function (data) {

                            $rootScope.sessionChecking = false;

                            localStorageService.remove('authorizationData');
                            toastr.success('See ya later!');
                            $rootScope.isLogin = false;
                            yuyanAuthSvc.isLogin = false;

                            $state.go('home');

                        }, function (data) {
                            $rootScope.sessionChecking = false;
                            toastr.error('Failed logout.', data.statusText);
                        });
                }
            }

            function goHome() {
                $state.go('home');
            }

            function goManagement() {
                $state.go('survey', {}, { location: false });
            }

            function saveSurvey(survey) {
                yuyanAPISvc.surveyCrudSvc().save(survey,
                           function (data) {
                               toastr.success("Survey Saved!");
                               $scope.$broadcast('reset');
                               $state.go('survey', {}, { location: false });
                               //reset();
                           }, function (data) {
                               toastr.error("Save Survey Error");
                               $scope.$broadcast('reset');
                           });
            }

            function checkSession() {

                var localSessionToken = localStorageService.get('authorizationData');
                if (localSessionToken) {

                    $rootScope.sessionChecking = true;

                    yuyanAPISvc.sessionCheckSvc().get({ sessionId: localSessionToken.token },
                        function (data) {
                            $rootScope.sessionChecking = false;
                            if (data.SessionId && data.IsActive) {
                                $rootScope.isLogin = true;
                                yuyanAuthSvc.isLogin = true;
                            } else {
                                // session exipred
                                $rootScope.isLogin = false;
                                yuyanAuthSvc.isLogin = false;
                                localStorageService.remove('authorizationData');
                            }
                        }, function (data) {

                            $rootScope.sessionChecking = false;

                            toastr.error('User Session Check failed. Please refresh.');
                        });
                }
            }

            function changeLanguage(langKey) {
                $translate.use(langKey);
            };

        }]);
})();