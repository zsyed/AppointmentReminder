appointmentReminderApp.factory('profileService',
	function ($http) {
		var getProfile = function () {
			return $http.get("/api/ProfileWeb");
		};
		
		var insertProfile = function (profile) {
			return $http.post("/api/ProfileWeb", profile);
		};

		var updateProfile = function (profile) {
			
			return $http.put("/api/ProfileWeb", profile);
		};

		return {
			insertProfile: insertProfile,
			updateProfile: updateProfile,
			getProfile: getProfile
		};
	}
);


//$scope.saveEditProfile = function () {
//	$http.put("/api/ProfileWeb", $scope.profile)
//		.then(function (response) {
//			$scope.profile = response.data;
//		});
//	window.location = "/Profile/Index";
//};