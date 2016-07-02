(function () {
    angular.module('choriceApp').controller('headCtrl', ['$scope', '$http', '$log', 'choriceAPISvc', 'endpoint',
        function ($scope, $http, $log, choriceAPISvc, endpoint) {

            var tokenUrl = window.location.hash.split('/')[2];

            $scope.title = "Chorice - Easy Survey Online";

            choriceAPISvc.surveyTitleSvc().get({ urltoken: tokenUrl },
                function (data) {
                    $scope.title = data.title;
                },
                function (error) {
                    toastr.error('Error load Survey');
                });

        }]);
})();