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

		var contactId = null;
		$http.get("/api/ReminderWeb/" + reminderId)
			.then(function (response) {
				$scope.reminder = response.data;
				contactId = $scope.reminder.ContactId;
				var keepGoing = true;
				for (var i = 0; i <= $scope.contacts.length && keepGoing; i++) {
					if ($scope.contacts[i].Id == contactId) {
						$scope.selectedContact = $scope.contacts[i];
						keepGoing = false;
					}
				}
			});
		
		$http.get("/api/ReminderContactWeb/")
			.then(function (response) {
				$scope.contacts = response.data;
		});

		$scope.saveEditReminder = function () {
			var contactId = $scope.selectedContact.Id;
			$scope.reminder.ContactId = contactId;
			$http.put("/api/ReminderWeb", $scope.reminder)
				.then(function (response) {
					$scope.reminder = response.data;
				});
			window.location = "/Reminder/Index";
		};
	};
	
	var ReminderCreateController = function ($scope, $http) {
		$http.get("/api/ReminderContactWeb/")
			.then(
				function (response) {
					$scope.contacts = response.data;
				}
			);

		$scope.saveCreateReminder = function () {
			$scope.reminder.ContactId = $scope.selectedContact.Id;
			$http.post("/api/ReminderWeb", $scope.reminder)
				.then(function (response) {
					$scope.reminder = response.data;
				});
			window.location = "/Reminder/Index";
		};
	};

	app.controller("ReminderHistoryController", ["$scope", "$http", ReminderHistoryController]);
	app.controller("ReminderIndexController", ["$scope", "$http", ReminderIndexController]);
	app.controller("ReminderCreateController", ["$scope", "$http", ReminderCreateController]);
	app.controller("ReminderEditController", ["$scope", "$http", "$location", ReminderEditController]);
	app.controller("ReminderDeleteController", ["$scope", "$http", "$location", ReminderDeleteController]);
}());