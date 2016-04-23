(function () {
    'use strick';

    angular.module('yuyanApp')
        .controller('deleteQuestionCtrl', ['$scope', '$uibModalInstance', 'question',
            function ($scope, $uibModalInstance, question) {

                $scope.question = question;

                $scope.ok = function () {
                    $uibModalInstance.close(question);
                };

                $scope.cancel = function () {
                    $uibModalInstance.dismiss('cancel');
                };
            }]);

})();