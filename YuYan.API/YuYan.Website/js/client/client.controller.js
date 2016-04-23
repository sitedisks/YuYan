(function () {
    'use strict';
    angular.module('yuyanApp').controller('clientCtrl', ['$scope', '$stateParams',
        function ($scope, $stateParams) {


            $scope.url = $stateParams.tokenUrl;

        }]);
})();