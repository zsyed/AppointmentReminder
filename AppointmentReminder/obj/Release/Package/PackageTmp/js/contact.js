(function () {

	var app = angular.module("ContactModule", []);

	var ContactIndexController = function ($scope, $http) {
		$http.get("/api/ContactWeb")
			.then(function(response) {
				$scope.contacts = response.data;
			});
	};
	
	var ContactEditController = function ($scope, $http, $location) {
		var contactId = $location.absUrl().match(/\/Edit\/(.*)/)[1];
		$http.get("/api/ContactWeb/" + contactId)
			.then(function (response) {
				$scope.contact = response.data;
			});

		$scope.saveEditContact = function () {
			$http.put("/api/ContactWeb", $scope.contact)
				.then(function (response) {
					$scope.contact = response.data;
				});
			window.location = "/Contact/Index";
		};
	};
	
	var ContactDeleteController = function ($scope, $http, $location) {
		var contactId = $location.absUrl().match(/\/Delete\/(.*)/)[1];
		$http.get("/api/ContactWeb/" + contactId)
			.then(function (response) {
				$scope.contact = response.data;
			});

		$scope.saveDeleteContact = function () {
			$http.delete("/api/ContactWeb/" + contactId)
				.then(function (response) {
					$scope.contact = response.data;
				});
			window.location = "/Contact/Index";
		};
	};
	
	var ContactCreateController = function ($scope, $http) {
		$scope.saveCreateContact = function () {
			$http.post("/api/ContactWeb", $scope.contact)
				.then(function (response) {
					$scope.contact = response.data;
				});
			window.location = "/Contact/Index";
		};
	};
	
	app.controller("ContactIndexController", ["$scope", "$http", ContactIndexController]);
	app.controller("ContactEditController", ["$scope", "$http", "$location", ContactEditController]);
	app.controller("ContactDeleteController", ["$scope", "$http", "$location", ContactDeleteController]);
	app.controller("ContactCreateController", ["$scope", "$http", ContactCreateController]);
	
}());