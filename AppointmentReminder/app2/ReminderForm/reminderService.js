appointmentReminderApp.factory('reminderService',
	function ($http) {

		var getReminder = function (id) {
			return $http.get("/api/ReminderWeb/" + id);
		};
		
		var getReminderForUpdate = function (id) {
			return $http.get("/api/ReminderWeb/" + id);
		};

		var getReminders = function () {
			return $http.get("/api/ReminderWeb");
		};
		
		var getReminderForUpdateContacts = function () {
			return $http.get("/api/ReminderContactWeb");
		};

		var getReminderContacts = function () {
			return $http.get("/api/ReminderContactWeb");
		};


		var insertReminder = function (reminder) {
			return $http.post("/api/ReminderWeb", reminder);
		};

		var updateReminder = function (reminder) {
			return $http.put("/api/ReminderWeb", reminder);
		};

		var deleteReminder = function (id) {
			return $http.delete("/api/ReminderWeb/" + id);
		};

		var getReminderHistories = function () {
			return $http.get("/api/ReminderHistoryWeb");
		};

		return {
			getReminderForUpdateContacts: getReminderForUpdateContacts,
			getReminderForUpdate: getReminderForUpdate,
			getReminderContacts: getReminderContacts,
			getReminderHistories: getReminderHistories,
			insertReminder: insertReminder,
			deleteReminder: deleteReminder,
			updateReminder: updateReminder,
			getReminder: getReminder,
			getReminders: getReminders
		};
	}
);


