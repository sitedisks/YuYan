(function () {
    'use strict';

    angular.module('yuyanApp').service('yuyanAuthSvc', [function () {

            var authService = {
                isLogin: false 
            };

            return authService;
        }]);
})();