(function () {
    'use strict';

    angular.module('yuyanApp').service('yuyanAPISvc', ['$resource', 'endpoint',
        function ($resource, endpoint) {

            var userAPI = endpoint.localAPI + 'users';
            var surveyAPI = endpoint.localAPI + 'surveys';
            var reportAPI = endpoint.localAPI + 'report';

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
                surveyClientAnswerDicSvc: surveyClientAnswerDicSvc
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
        }]);

})();