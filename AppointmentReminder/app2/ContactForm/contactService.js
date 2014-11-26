appointmentReminderApp.factory('contactService',
	function ($http) {
		
		var getContact = function (id) {
			return $http.get("/api/ContactWeb/" + id);
		};

		var getContacts = function() {
			return $http.get("/api/ContactWeb");
		};

		var insertContact = function (contact) {
			return $http.post("/api/ContactWeb", contact);
		};

		var updateContact = function (contact) {
			return $http.put("/api/ContactWeb", contact);
		};
		
		var deleteContact = function (id) {
			return $http.delete("/api/ContactWeb/" + id);
		};

		return {
			insertContact: insertContact,
			deleteContact: deleteContact,
			updateContact: updateContact,
			getContact: getContact,
			getContacts : getContacts
		};
	}
);


