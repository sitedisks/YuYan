(function () {

    'use strict';

    angular.module('yuyanApp').factory('authInterceptorSvc', ['$q', '$location', 'localStorageService',
        function ($q, $location, localStorageService) {
            var authInterceptorServiceFactory = {};

            var _request = function (config) {

                config.headers = config.headers || {};

                var localSessionToken = localStorageService.get('authorizationData');
                if (localSessionToken) {

                    config.headers['yuyan.header.token'] = localSessionToken.token;
                }

                return config;
            }

            var _requestError = function (rejection) {

                return $q.reject(rejection);
            }

            var _response = function (response) {

                return response;
            }

            // Intercept 401s and redirect you to login
            var _responseError = function (rejection) {
                if (rejection.status === 401)
                {
                    //$location.path('/');
                }
                else if (rejection.status === 404) {
                    //$location.path('/');
                }
                    

                return $q.reject(rejection);
            }

            authInterceptorServiceFactory.request = _request;
            authInterceptorServiceFactory.requestError = _requestError;
            authInterceptorServiceFactory.response = _response;
            authInterceptorServiceFactory.responseError = _responseError;

            return authInterceptorServiceFactory;
        }]);
})();