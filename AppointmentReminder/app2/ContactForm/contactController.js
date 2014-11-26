appointmentReminderApp.controller('contactController',
	function ContactFormController($scope, $window, $location, $routeParams, contactService) {
		
		if ($routeParams.id) {
		
			contactService.getContact($routeParams.id).then(
				function (results) {
					$scope.contact = results.data;
				},
				function (results) {
					// on error
					var data = results.data;
				}
			);

		} else {
			$scope.contact = { id: -1 };
			contactService.getContacts().then(
				function (results) {
					$scope.contacts = results.data;
					$scope.$broadcast('CONTACTS_LOADED_EVENT');
				},
				function (results) {
					// on error
					var data = results.data;
				}
			);
		}
		
		$scope.$on('CONTACTS_LOADED_EVENT', function () {
			$scope.loadingcontacts = false;
		});

		$scope.$on('CONTACTS_LOADING_EVENT', function () {
			$scope.loadingcontacts = true;
		});

		$scope.$broadcast('CONTACTS_LOADING_EVENT');
		
		$scope.showCreateContactForm = function () {
			$location.path('/newContactForm');
		};
		
		$scope.showUpdateContactForm = function (id) {
			$location.path('/updateContactForm/' + id);
		};
		
		$scope.showDeleteContactForm = function (id) {
			$location.path('/deleteContactForm/' + id);
		};

		$scope.submitForm = function() {
			if ($scope.contact.Id > 0) {
				contactService.updateContact($scope.contact).then(
					function(results) {
						$scope.contact = results.data;
						$window.history.back();
					},
					function(results) {
						// on error
						var data = results.data;
					}
				);
			} else {

				contactService.insertContact($scope.contact).then(
					function (results) {
						$scope.contact = results.data;
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
			if ($scope.contact.Id > 0) {
				contactService.deleteContact($routeParams.id).then(
					function (results) {
						$scope.contact = results.data;
						$window.history.back();
					},
					function (results) {
						// on error
						var data = results.data;
					}
				);
			} 
		};

	});

