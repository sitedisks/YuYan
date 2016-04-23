(function () {
    'use strict';

    angular.module('yuyanApp')
		.controller('modalCtrl', ['$scope', '$uibModalInstance',
			function ($scope, $uibModalInstance) {

			    $scope.ok = function () {
			        $uibModalInstance.close();
			    };

			    $scope.cancel = function () {
			        $uibModalInstance.dismiss('cancel');
			    };
			}]
		);

})();