(function () {
    'use strict';

    angular.module('yuyanApp')
		.controller('previewModalCtrl', ['$scope', '$uibModalInstance', 'survey',
			function ($scope, $uibModalInstance, survey) {

			    $scope.survey = survey;
			    $scope.openLink = openLink;

			    $scope.shareLink = 'http://' + location.host + '/client/#/' + survey.URLToken;
			    $scope.sharePageLink = 'http://' + location.host + '/client/#/page/' + survey.URLToken;
			    $scope.shareLiveLink = 'http://www.chorice.net/client/#/' + survey.URLToken;
			    $scope.shareLivePageLink = 'http://www.chorice.net/client/#/page/' + survey.URLToken;

			    function openLink(url) {
			        window.open('http://' + url, '_blank');
			    }

			    $scope.ok = function () {
			        $uibModalInstance.close();
			    };

			    $scope.cancel = function () {
			        $uibModalInstance.dismiss('cancel');
			    };
			}]
		);

})();