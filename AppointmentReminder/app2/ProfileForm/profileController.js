appointmentReminderApp.controller('profileController',
	function ProfileFormController($scope, $window, $location, $routeParams, profileService) {

		$scope.$on('PROFILE_LOADED_EVENT', function () {
			 $scope.loading = false;
		});
		
		$scope.$on('PROFILE_LOADING_EVENT', function () {
			$scope.loading = true;
		});
		
		$scope.$broadcast('PROFILE_LOADING_EVENT');

		profileService.getProfile().then(
			function (results) {
				$scope.profile = results.data;
				$scope.$broadcast('PROFILE_LOADED_EVENT');
			},
			function(results) {
				// on error
				var data = results.data;
			}
		);

		$scope.isProfileIdGreaterThanZero = function () {
			var returnVal = false;
			if ($scope.profile) {
				returnVal = ($scope.profile.Id > 0);
			}
			return returnVal;
		};

		$scope.showCreateProfileForm = function () {
			$location.path('/newProfileForm');

		};

		$scope.showUpdateProfileForm = function (id) {
			$location.path('/updateProfileForm/' + id);

		};

		$scope.submitForm = function () {
			if ($scope.profile.Id > 0) {
				profileService.updateProfile($scope.profile).then(
					function (results) {
						$scope.profile = results.data;
						$window.history.back();
						},
						function (results) {
						// on error
						var data = results.data;
						}
					);
			} else {
				
				profileService.insertProfile($scope.profile).then(
					function (results) {
						$scope.profile = results.data;
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