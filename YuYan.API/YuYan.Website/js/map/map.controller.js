(function () {
    'use strict';

    angular.module('yuyanApp')

		.controller('controlCtrl', ['$scope', function ($scope) {
		    $scope.controlText = 'I\'m a custom control';
		    $scope.danger = false;
		    $scope.controlClick = function () {
		        $scope.danger = !$scope.danger;
		        alert('custom control clicked!');
		    };

		}])


		.controller('mapCtrl', ['$scope', '$http', '$timeout', '$uibModal', 'uiGmapGoogleMapApi', 'endpoint',
		function ($scope, $http, $timeout, $uibModal, uiGmapGoogleMapApi, endpoint) {

		    $scope.clocation = null;

		    $scope.clickMe = clickMe;

		    doMap();
		    getLocation();

		    doDom(); // resolve dom/promise
		    function doDom() {
		        $timeout(function () {
		            if ($scope.clocation) {
		                $timeout(function () {
		                    doMarker();
		                }, 1000);

		            } else {
		                doDom();
		            }
		        });
		    }

		    function doMarker() {
		        //single marker
		        $scope.marker = { id: 6, coords: { latitude: $scope.clocation.latitude, longitude: $scope.clocation.longitude } };

		        //multiple markers
		        $scope.markers = [
					{ id: 0, coords: { latitude: 45, longitude: -73 }, info: "marker1" },
					{ id: 1, coords: { latitude: 46, longitude: -72 }, info: "marker2" },
					{ id: 2, coords: { latitude: 47, longitude: -71 }, info: "marker3" }
		        ];
		    }

		    function doMap() {

		        uiGmapGoogleMapApi.then(function (maps) {

		            lodashFix();
		            //geocoder.geocode({});
		            var lat = -37.8140000, lng = 144.9633200; // default melbourne 
		            $scope.map = {
		                center: { latitude: lat, longitude: lng },
		                zoom: 12,
		                options: { scrollwheel: true },
		                control: {}
		            };

		        });
		    }

		    function getLocation() {
		        $http.get(endpoint.ipaddress).then(
					function (response) {
					    // toastr.success("Your IP address: " + ipaddress);
					    $http.get(endpoint.geoip + response.data).then(function (response) {
					        $scope.clocation = response.data;
					        var fulladd = $scope.clocation.city + ", " + $scope.clocation.region_code + " " + $scope.clocation.country_name;
					        toastr.success("You are at <b>" + fulladd + "</b>");
					    }, function (response) {
					        toastr.error("Cannot get Location.");
					    });
					},
					function (response) {
					    //error
					    toastr.error("Cannot get your Ip Address.");
					}
				);

		    }

		    // functions
		    function clickMe() {
		        var modalInstance = $uibModal.open({
		            animation: true,
		            templateUrl: 'templates/modal.html',
		            controller: 'modalCtrl',
		            size: 'lg',
		            resolve: {}
		        });

		        modalInstance.result.then(function () {

		        }, function () {
		            // dismissed log
		        });
		    }

		    function lodashFix() {
		        //fix of lodash.js
		        if (typeof _.contains === 'undefined')
		            _.contains = _.includes;
		        if (typeof _.object === 'undefined')
		            _.object = _.zipObject;
		    }

		}]);

})();