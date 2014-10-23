(function () {

	var app = angular.module("ProfileModule", []);

	var ProfileIndexController = function($scope, $http) {
		$http.get("/api/ProfileWeb")
			.then(function(response) {
				$scope.profile = response.data;
			});
	};


	var ProfileEditController = function ($scope, $http) {

		$http.get("/api/ProfileWeb")
			.then(function(response) {
				$scope.profile = response.data;
			});

		$scope.saveEditProfile = function () {
			$http.put("/api/ProfileWeb", $scope.profile)
				.then(function(response) {
					$scope.profile = response.data;
				});
		};
	};

	var ProfileCreateController = function($scope, $http) {
		$http.post("/api/ProfileWeb")
			.then(function(response) {
				$scope.profile = response.data;
			})
	};

	app.controller("ProfileIndexController", ["$scope", "$http", ProfileIndexController]);
	app.controller("ProfileEditController", ["$scope", "$http", ProfileEditController]);

}());