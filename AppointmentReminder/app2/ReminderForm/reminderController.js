appointmentReminderApp.controller('reminderController',
	function ReminderFormController($scope, $filter, $window, $location, $routeParams, reminderService) {

		reminderService.getReminderContacts().then(
			function (results) {
				$scope.remindercontacts = results.data;
			},
			function (results) {
				// on error
				var data = results.data;
			}
		);

		if ($routeParams.id) {

			reminderService.getReminder($routeParams.id).then(
				function (response) {
					$scope.reminder = response.data;
					

					$scope.reminder = response.data;
					var contactId = $scope.reminder.ContactId;
					var recurrence = $scope.reminder.Recurrence;
					var weekday = $scope.reminder.WeekDay;

					$scope.ReminderDate = $filter('date')($scope.reminder.ReminderDateTime, 'MM/dd/yyyy');
					$scope.ReminderTime = $filter('date')($scope.reminder.ReminderDateTime, 'MM/dd/yyyy hh:mm a');
					var keepGoing = true;
					for (i = 0; i <= $scope.remindercontacts.length && keepGoing; i++) {
						if ($scope.remindercontacts[i].Id == contactId) {
							$scope.selectedContact = $scope.remindercontacts[i];
							keepGoing = false;
						}
					}


					keepGoing = true;
					for (i = 0; i <= $scope.recurrences.length && keepGoing; i++) {
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



				},
				function (results) {
					// on error
					var data = results.data;
				}
			);

		} else {
			$scope.reminder = { id: -1 };
			reminderService.getReminders().then(
				function (results) {
					$scope.reminders = results.data;
					$scope.$broadcast('REMINDERS_LOADED_EVENT');
				},
				function (results) {
					// on error
					var data = results.data;
				}
			);

			reminderService.getReminderHistories().then(
				function (results) {
					$scope.reminderhistories = results.data;
					$scope.$broadcast('REMINDER_HISTORIES_LOADED_EVENT');
				},
				function (results) {
					// on error
					var data = results.data;
				}
			);


		}

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

		$scope.$on('REMINDERS_LOADED_EVENT', function () {
			$scope.loadingreminders = false;
		});

		$scope.$on('REMINDERS_LOADING_EVENT', function () {
			$scope.loadingreminders = true;
		});

		$scope.$on('REMINDER_HISTORIES_LOADED_EVENT', function () {
			$scope.loadingreminderhistories = false;
		});

		$scope.$on('REMINDER_HISTORIES_LOADING_EVENT', function () {
			$scope.loadingreminderhistories = true;
		});

		$scope.$broadcast('REMINDER_HISTORIES_LOADING_EVENT');
		
		$scope.$broadcast('REMINDERS_LOADING_EVENT');
		
		$scope.showCreateReminderForm = function () {
			$location.path('/newReminderForm');
		};

		$scope.showHistoryReminderForm = function () {
			$location.path('/historyReminderForm');
		};

		$scope.showUpdateReminderForm = function (id) {
			$location.path('/updateReminderForm/' + id);
		};

		$scope.showDeleteReminderForm = function (id) {
			$location.path('/deleteReminderForm/' + id);
		};

		$scope.submitForm = function () {
			if ($scope.reminder.Id > 0) {
				
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


				reminderService.updateReminder($scope.reminder).then(
					function (results) {
						$scope.reminder = results.data;
						$window.history.back();
					},
					function (results) {
						// on error
						var data = results.data;
					}
				);
			} else {
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
				
				reminderService.insertReminder($scope.reminder).then(
					function (results) {
						$scope.reminder = results.data;
						$window.history.back();
					},
						function (results) {
							// on error
							var data = results.data;
						}
					);
			}
		};

		$scope.submitDeleteForm = function () {
			if ($scope.reminder.Id > 0) {
				reminderService.deleteReminder($routeParams.id).then(
					function (results) {
						$scope.reminder = results.data;
						$window.history.back();
					},
					function (results) {
						// on error
						var data = results.data;
					}
				);
			}
		};
		
		/* Time */
		$scope.hstep = 1;
		$scope.mstep = 15;
		$scope.ismeridian = true;
		
		$scope.options = {
			hstep: [1, 2, 3],
			mstep: [1, 5, 10, 15, 25, 30]
		};

		$scope.toggleMode = function () {
			$scope.ismeridian = !$scope.ismeridian;
		};
		/* Time */
		
		/* manage controls */
		$scope.calendarshow = false;
		$scope.weekdayshow = false;
		
		$scope.reminderRecurrenceTimeChanged = function () {
			var recurrence = $scope.selectedRecurrence.idrecur; // $('#ddlRecurrence option:selected').text();
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
		};

		/* manage controls */
	});

