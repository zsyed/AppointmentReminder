(function () {

	var app = angular.module("ReminderModule", ['ui.bootstrap']);
	
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
	
	var ReminderEditController = function ($scope,$filter, $http, $location) {
		var reminderId = $location.absUrl().match(/\/Edit\/(.*)/)[1];

		var contactId = null;
		var recurrence = null;
		var weekday = null;
		
		$http.get("/api/ReminderWeb/" + reminderId)
			.then(function (response) {
				$scope.reminder = response.data;
				contactId = $scope.reminder.ContactId;
				recurrence = $scope.reminder.Recurrence;
				weekday = $scope.reminder.WeekDay;
				
				$scope.ReminderDate = $filter('date')($scope.reminder.ReminderDateTime, 'MM/dd/yyyy');
				$scope.ReminderTime = $filter('date')($scope.reminder.ReminderDateTime, 'MM/dd/yyyy hh:mm a');
				var keepGoing = true;
				for (var i = 0; i <= $scope.contacts.length && keepGoing; i++) {
					if ($scope.contacts[i].Id == contactId) {
						$scope.selectedContact = $scope.contacts[i];
						keepGoing = false;
					}
				}
				
				
				keepGoing = true;
				for (var i = 0; i <= $scope.recurrences.length && keepGoing; i++) {
					if ($scope.recurrences[i].idrecur == recurrence) {
						$scope.selectedRecurrence = $scope.recurrences[i];
						keepGoing = false;
					}
				}

				if (weekday != null) {
					keepGoing = true;
					for (var i = 0; i <= $scope.weekdays.length && keepGoing; i++) {
						if ($scope.weekdays[i].idweek == weekday) {
							$scope.selectedWeekday = $scope.weekdays[i];
							keepGoing = false;
						}
					}
				}

				//copy paste code must be refactored.

				if (recurrence == 'Once') {
					$scope.calendarshow = true;
					$scope.weekdayshow = false;
				}

				if (recurrence == 'Daily') {
					$scope.calendarshow = false;
					$scope.weekdayshow = false;
				}

				if (recurrence == 'Weekly') {
					$scope.calendarshow = false;
					$scope.weekdayshow = true;
				}

				//copy paste code

			
			});
		
		$http.get("/api/ReminderContactWeb/")
			.then(function (response) {
				$scope.contacts = response.data;
			});
		
		$scope.recurrences = [
		  { idrecur: 'Once' },
		  { idrecur: 'Daily' },
		  { idrecur: 'Weekly' }
		];
		
		$scope.weekdays = [
			{ idweek: 'Monday' },
			{ idweek: 'Tuesday' },
			{ idweek: 'Wednesday' },
			{ idweek: 'Thursday' },
			{ idweek: 'Friday' },
			{ idweek: 'Saturday' },
			{ idweek: 'Sunday' }
		];
		
		$scope.calendarshow = false;
		$scope.weekdayshow = false;

		$("#ddlRecurrence").on('change', function() {
			var recurrence = $('#ddlRecurrence option:selected').text();
			if (recurrence == 'Once') {
				$scope.calendarshow = true;
				$scope.weekdayshow = false;
			}
			
			if (recurrence == 'Daily') {
				$scope.calendarshow = false;
				$scope.weekdayshow = false;
			}
			
			if (recurrence == 'Weekly') {
				$scope.calendarshow = false;
				$scope.weekdayshow = true;
			}
			
		});

		$scope.saveEditReminder = function () {
			var contactId = $scope.selectedContact.Id;
			$scope.reminder.ContactId = contactId;
			
			if ($scope.calendarshow) {
				$scope.ReminderDate = $filter('date')($scope.ReminderDate, 'MM/dd/yyyy');
			} else {
				$scope.ReminderDate = '01/01/1901';
			}
			
			$scope.ReminderTime = $filter('date')($scope.ReminderTime, 'hh:mm a');
			$scope.ReminderTime = $scope.ReminderTime.substr($scope.ReminderTime.length - 8);
			$scope.reminder.ReminderDateTime = $scope.ReminderDate + " " + $scope.ReminderTime;
			$scope.reminder.Recurrence = $scope.selectedRecurrence.idrecur;
			if ($scope.weekdayshow) {
				$scope.reminder.WeekDay = $scope.selectedWeekday.idweek;
			}

			$http.put("/api/ReminderWeb", $scope.reminder)
				.then(function (response) {
					$scope.reminder = response.data;
				});
			window.location = "/Reminder/Index";
		};
		
		/* Date */
		$scope.toggleMin = function () {
			$scope.minDate = $scope.minDate ? null : new Date();
		};
		$scope.toggleMin();
		/* Date */
		
		/* Time */
		$scope.hstep = 1;
		$scope.mstep = 15;

		$scope.options = {
			hstep: [1, 2, 3],
			mstep: [1, 5, 10, 15, 25, 30]
		};

		$scope.ismeridian = true;
		/* Time */
	};
	
	var ReminderCreateController = function ($scope, $filter, $http) {
		$http.get("/api/ReminderContactWeb/")
			.then(
				function (response) {
					$scope.contacts = response.data;
				}
			);
		
		$scope.recurrences = [
  { idrecur: 'Once' },
  { idrecur: 'Daily' },
  { idrecur: 'Weekly' }
		];

		$scope.weekdays = [
			{ idweek: 'Mon' },
			{ idweek: 'Tue' },
			{ idweek: 'Wed' },
			{ idweek: 'Thur' },
			{ idweek: 'Fri' },
			{ idweek: 'Sat' },
			{ idweek: 'Sun' }
		];

		$scope.calendarshow = false;
		$scope.weekdayshow = false;

		$("#ddlRecurrence").on('change', function () {
			var recurrence = $('#ddlRecurrence option:selected').text();
			if (recurrence == 'Once') {
				$scope.calendarshow = true;
				$scope.weekdayshow = false;
			}

			if (recurrence == 'Daily') {
				$scope.calendarshow = false;
				$scope.weekdayshow = false;
			}

			if (recurrence == 'Weekly') {
				$scope.calendarshow = false;
				$scope.weekdayshow = true;
			}

		});

		$scope.saveCreateReminder = function () {
			$scope.reminder.ContactId = $scope.selectedContact.Id;
			
			if ($scope.calendarshow) {
				$scope.ReminderDate = $filter('date')($scope.ReminderDate, 'MM/dd/yyyy');
			} else {
				$scope.ReminderDate = '01/01/1901';
			}

			$scope.ReminderTime = $filter('date')($scope.ReminderTime, 'hh:mm a');
			$scope.ReminderTime = $scope.ReminderTime.substr($scope.ReminderTime.length - 8);
			$scope.reminder.ReminderDateTime = $scope.ReminderDate + " " + $scope.ReminderTime;
			$scope.reminder.Recurrence = $scope.selectedRecurrence.idrecur;
			if ($scope.weekdayshow) {
				$scope.reminder.WeekDay = $scope.selectedWeekday.idweek;
			}

			//$scope.ReminderDate = $filter('date')($scope.ReminderDate, 'MM/dd/yyyy');
			//$scope.ReminderTime = $filter('date')($scope.ReminderTime, 'hh:mm a');
			//$scope.reminder.ReminderDateTime = $scope.ReminderDate + " " + $scope.ReminderTime;
			$http.post("/api/ReminderWeb", $scope.reminder)
				.then(function (response) {
					$scope.reminder = response.data;
				});
			window.location = "/Reminder/Index";
		};
		/* Date */
		$scope.toggleMin = function () {
			$scope.minDate = $scope.minDate ? null : new Date();
		};
		$scope.toggleMin();
		/* Date */

		/* Time */
		$scope.hstep = 1;
		$scope.mstep = 15;

		$scope.options = {
			hstep: [1, 2, 3],
			mstep: [1, 5, 10, 15, 25, 30]
		};

		$scope.ismeridian = true;
		
		$scope.toggleMode = function () {
			$scope.ismeridian = !$scope.ismeridian;
		};
		/* Time */
	};


	app.controller("ReminderHistoryController", ["$scope", "$http", ReminderHistoryController]);
	app.controller("ReminderIndexController", ["$scope", "$http", ReminderIndexController]);
	app.controller("ReminderCreateController", ["$scope", "$filter", "$http", ReminderCreateController]);
	app.controller("ReminderEditController", ["$scope","$filter", "$http", "$location", ReminderEditController]);
	app.controller("ReminderDeleteController", ["$scope", "$http", "$location", ReminderDeleteController]);
	
}());