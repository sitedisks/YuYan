(function () {
    'use strict';

    angular.module('yuyanApp')
		.controller('previewModalCtrl', ['$scope', '$uibModalInstance', 'yuyanAPISvc', 'survey',
			function ($scope, $uibModalInstance, yuyanAPISvc, survey) {

			    $scope.survey = survey;
			    $scope.openLink = openLink;

			    if (!isNullOrEmpty(survey.BannerId))
			        $scope.bannerUrl = yuyanAPISvc.imageGetUrl(survey.BannerId, 760);
			    if (!isNullOrEmpty(survey.LogoId))
			        $scope.logoUrl = yuyanAPISvc.imageGetUrl(survey.LogoId, 760);

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