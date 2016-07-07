(function () {
    'use strict';

    angular.module('yuyanApp').service('yuyanAPISvc', ['$resource', 'Upload', 'endpoint',
        function ($resource, Upload, endpoint) {

            //var useEndpoint = endpoint.LiveAPI;
            var useEndpoint = endpoint.LocalAPI;

            var userAPI = useEndpoint + 'users';
            var surveyAPI = useEndpoint + 'surveys';
            var reportAPI = useEndpoint + 'report';
            var imageAPI = useEndpoint + 'images';

            var service = {
                // user
                sessionCheckSvc: sessionCheckSvc,
                isAuthenticated: isAuthenticated,
                userCheckSvc: userCheckSvc,
                userLoginSvc: userLoginSvc,
                userLogoutSvc: userLogoutSvc,
                userRegisterSvc: userRegisterSvc,
                // survey
                surveyCrudSvc: surveyCrudSvc,
                surveyCountBySession: surveyCountBySession,
                // question
                questionCrudSvc: questionCrudSvc,
                // result
                surveyResultCrudSvc: surveyResultCrudSvc,
                // report
                surveyClientReportSvc: surveyClientReportSvc,
                surveyClientAnswerDicSvc: surveyClientAnswerDicSvc,
                // image
                imageUploadSvc: imageUploadSvc
            };

            return service;

            // user
            function sessionCheckSvc() {
                return $resource(userAPI + '/status');
            }

            function isAuthenticated() { }

            function userCheckSvc() {
                return $resource(userAPI + '/check');
            }

            function userLoginSvc() {
                return $resource(userAPI + '/login');
            }

            function userLogoutSvc() {
                return $resource(userAPI + '/logout');
            }

            function userRegisterSvc() {
                return $resource(userAPI + '/register');
            }

            // survey
            function surveyCountBySession() {
                return $resource(surveyAPI + '/count');
            }

            function surveyCrudSvc() {
                return $resource(surveyAPI + '/:surveyId',
                    { surveyId: '@sid' },
                    { update: { method: 'PUT' } });
            }

            // survey queston 
            function questionCrudSvc() {
                return $resource(surveyAPI + '/:surveyId/questions/:questionId',
                    { surveyId: '@sid', questionId: '@qid' },
                    { update: { method: 'PUT' } });
            }

            // survey result
            function surveyResultCrudSvc() {
                return $resource(surveyAPI + '/:surveyId/results/:resultId',
                    { surveyId: '@sid', resultId: '@rid' },
                    { update: { method: 'PUT' } });
            }

            // survey item
            function itemCrudSvc() {
                return $resource(surveyAPI + '/:surveyId/questions/:questionId/items/:itemId',
                    { surveyId: '@sid', questionId: '@qid', itemId: '@iid' },
                    { update: { method: 'PUT' } });
            }

            // report
            function surveyClientReportSvc() {
                return $resource(reportAPI + '/:surveyId',
                    { surveyId: '@sid' });
            }

            function surveyClientAnswerDicSvc() {
                return $resource(reportAPI + '/:surveyId/answerdic',
                     { surveyId: '@sid' });
            }

            // image
            function imageUploadSvc(file, imageGroup, refId) {
                return Upload.upload({
                    url: imageAPI + '/upload/' + imageGroup,
                    method: 'POST',
                    data: { file: file, 'refId': refId }
                });
            }
        }]);

})();