(function () {
    'use strick';

    angular.module('yuyanApp')
        .controller('deleteResultCtrl', ['$scope', '$uibModalInstance', 'result',
            function ($scope, $uibModalInstance, result) {

                $scope.result = result;

                $scope.ok = function () {
                    $uibModalInstance.close(result);
                };

                $scope.cancel = function () {
                    $uibModalInstance.dismiss('cancel');
                };
            }]);

})();