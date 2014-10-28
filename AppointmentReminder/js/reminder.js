(function () {

	var app = angular.module("ReminderHistoryModule", []);
	
	var ReminderHistoryController = function ($scope, $http) {
		$http.get("/api/ReminderHistoryWeb")
			.then(function (response) {
				$scope.reminderhistories = response.data;
			});
	};
	
	app.controller("ReminderHistoryController", ["$scope", "$http", ReminderHistoryController]);
}());