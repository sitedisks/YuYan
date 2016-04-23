(function () {
    'use strick';

    angular.module('yuyanApp')
        .controller('deleteSurveyCtrl', ['$scope', '$uibModalInstance', 'survey',
            function ($scope, $uibModalInstance, survey) {

                $scope.survey = survey;

                $scope.ok = function () {
                    $uibModalInstance.close(survey);
                };

                $scope.cancel = function () {
                    $uibModalInstance.dismiss('cancel');
                };
            }]
        );
})();