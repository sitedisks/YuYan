(function () {
    'use strict';
    angular.module('choriceApp').service('choriceAPISvc', ['$resource', 'endpoint',
        function ($resource, endpoint) {

            //var useEndpoint = endpoint.LiveAPI;
            var useEndpoint = endpoint.LocalAPI;

            var clientAPI = useEndpoint + 'client';
            var reportAPI = useEndpoint + 'report';
         
            var service = {
                // survey
                surveyRetreiveSvc: surveyRetreiveSvc,
                surveyTitleSvc: surveyTitleSvc, 
                surveySaveSvc: surveySaveSvc,
                surveyResultSvc: surveyResultSvc,
                // report
                surveyClientReportSvc: surveyClientReportSvc,
                surveyClientAnswerDicSvc: surveyClientAnswerDicSvc
            };

            return service;

            function surveyRetreiveSvc() {
                return $resource(clientAPI + '/:urltoken', { urltoken: '@url' });
            }

            function surveyTitleSvc() {
                return $resource(clientAPI + '/:urltoken/title', { urltoken: '@url' }, {
                    get: {
                        method: 'GET',
                        isArray: false,
                        transformResponse: function (data, headers) {
                            if (data == '') 
                                return;
                            return { title: angular.fromJson(data) };
                        }
                    }
                });
            }

            function surveySaveSvc() {
                return $resource(clientAPI);
            }

            function surveyResultSvc() {
                return $resource(clientAPI + '/:surveyId/:score',
                    { surveyId: '@surveyId', score: '@score' });
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