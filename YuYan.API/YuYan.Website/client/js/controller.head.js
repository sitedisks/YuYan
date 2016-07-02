(function () {
    angular.module('choriceApp').controller('headCtrl', ['$scope', '$http', '$log', 'choriceAPISvc', 'endpoint',
        function ($scope, $http, $log, choriceAPISvc, endpoint) {

            var creditUrl = window.location.hash;
            if (creditUrl.indexOf('page') > -1)
                var tokenUrl = creditUrl.split('/')[2];
            else
                var tokenUrl = creditUrl.split('/')[1];

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