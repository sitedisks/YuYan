﻿(function () {
    'use strict';

    angular.module('yuyanApp')
		.controller('previewModalCtrl', ['$scope', '$uibModalInstance', 'yuyanAPISvc', 'survey', 'imageSize',
			function ($scope, $uibModalInstance, yuyanAPISvc, survey, imageSize) {

			    $scope.survey = survey;
			    $scope.openLink = openLink;

			    $scope.getImageUrl = getImageUrl;

			    if (!isNullOrEmpty(survey.BannerId))
			        $scope.bannerUrl = yuyanAPISvc.imageGetUrl(survey.BannerId, imageSize.survey);
			    if (!isNullOrEmpty(survey.LogoId))
			        $scope.logoUrl = yuyanAPISvc.imageGetUrl(survey.LogoId, imageSize.survey);

			    $scope.shareLink = 'http://' + location.host + '/client/#/' + survey.URLToken;
			    $scope.sharePageLink = 'http://' + location.host + '/client/#/page/' + survey.URLToken;
			    $scope.shareLiveLink = 'http://www.chorice.net/client/#/' + survey.URLToken;
			    $scope.shareLivePageLink = 'http://www.chorice.net/client/#/page/' + survey.URLToken;

			    function openLink(url) {
			        window.open('http://' + url, '_blank');
			    }

			    function getImageUrl(imageId) {
			        if (imageId != null)
			            return yuyanAPISvc.imageGetUrl(imageId, imageSize.questionItem);
			        else
			            return "";
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