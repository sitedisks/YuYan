(function () {
    'use strict';

    angular.module('yuyanApp').config(['$translateProvider',
        function ($translateProvider) {

            $translateProvider
                .translations('en', {
                    "TEXT_CHORICE": "chorice"
                });

            $translateProvider.useStaticFilesLoader({
                'prefix': 'data/locale-',
                'suffix': '.json'
            });

            $translateProvider.preferredLanguage('en');
            $translateProvider.forceAsyncReload(true);
            $translateProvider.useSanitizeValueStrategy('escape');
        }]);
})();