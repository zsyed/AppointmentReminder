(function () {

	var app = angular.module("ReminderModule", []);
	
	var ReminderHistoryController = function ($scope, $http) {
		$http.get("/api/ReminderHistoryWeb")
			.then(function (response) {
				$scope.reminderhistories = response.data;
			});
	};
	
	var ReminderIndexController = function ($scope, $http) {
		$http.get("/api/ReminderWeb")
			.then(function (response) {
				$scope.reminders = response.data;
			});
	};
	
	var ReminderDeleteController = function ($scope, $http, $location) {
		var reminderId = $location.absUrl().match(/\/Delete\/(.*)/)[1];
		$http.get("/api/ReminderWeb/" + reminderId)
			.then(function (response) {
				$scope.reminder = response.data;
			});

		$scope.saveDeleteReminder = function () {
			$http.delete("/api/ReminderWeb/" + reminderId)
				.then(function (response) {
					$scope.reminder = response.data;
				});
			window.location = "/Reminder/Index";
		};
	};
	
	var ReminderEditController = function ($scope, $http, $location) {
		var reminderId = $location.absUrl().match(/\/Edit\/(.*)/)[1];
		$http.get("/api/ReminderWeb/" + reminderId)
			.then(function (response) {
				$scope.reminder = response.data;
			});

		$scope.saveEditReminder = function () {
			$http.put("/api/ReminderWeb", $scope.reminder)
				.then(function (response) {
					$scope.reminder = response.data;
				});
			window.location = "/Reminder/Index";
		};
	};

	app.controller("ReminderHistoryController", ["$scope", "$http", ReminderHistoryController]);
	app.controller("ReminderIndexController", ["$scope", "$http", ReminderIndexController]);
	app.controller("ReminderEditController", ["$scope", "$http", "$location", ReminderEditController]);
	app.controller("ReminderDeleteController", ["$scope", "$http", "$location", ReminderDeleteController]);
}());