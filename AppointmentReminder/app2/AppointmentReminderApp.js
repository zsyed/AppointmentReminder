
var appointmentReminderApp = angular.module('appointmentReminderApp', ["ngRoute", "ui.bootstrap"]);

appointmentReminderApp.config(function ($routeProvider, $locationProvider) {
	
	$locationProvider.html5Mode(true);
	
	$routeProvider
		.when("/home", {
			templateUrl: "app2/Home.html",
			controller: "HomeController"
		})
		.when("/Profile", {
			templateUrl: "app2/ProfileForm/templates/profile.html",
			controller: "profileController"
		})
		.when("/Profile/:id", {
			templateUrl: "app2/ProfileForm/templates/profile.html",
			controller: "profileController"
		})
		.when("/newProfileForm", {
			templateUrl: "app2/ProfileForm/templates/profileUpdate.html",
			controller: "profileController"
		})
		.when("/updateProfileForm/:id", {
			templateUrl: "app2/ProfileForm/templates/profileUpdate.html",
			controller: "profileController"
		})
		.when("/Contacts", {
			templateUrl: "app2/ContactForm/templates/contacts.html",
			controller: "contactController"
		})
		.when("/newContactForm", {
			templateUrl: "app2/ContactForm/templates/contactUpdate.html",
			controller: "contactController"
		})
		.when("/updateContactForm/:id", {
			templateUrl: "app2/ContactForm/templates/contactUpdate.html",
			controller: "contactController"
		})
		.when("/deleteContactForm/:id", {
			templateUrl: "app2/ContactForm/templates/contactDelete.html",
			controller: "contactController"
		})
		.when("/Reminders", {
			templateUrl: "app2/ReminderForm/templates/reminders.html",
			controller: "reminderController"
		})
		.when("/historyReminderForm", {
			templateUrl: "app2/ReminderForm/templates/reminderHistory.html",
			controller: "reminderController"
		})
		.when("/newReminderForm", {
			templateUrl: "app2/ReminderForm/templates/reminderUpdate.html",
			controller: "reminderController"
		})
		.when("/updateReminderForm/:id", {
			templateUrl: "app2/ReminderForm/templates/reminderUpdate.html",
			controller: "reminderController"
		})
		.when("/deleteReminderForm/:id", {
			templateUrl: "app2/ReminderForm/templates/reminderDelete.html",
			controller: "reminderController"
		})
		.otherwise({ redirectTo: "/home" });
	
	//if (window.history && window.history.pushState) {
	//	$locationProvider.html5Mode(true);
	//}
});
