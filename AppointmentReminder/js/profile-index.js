
var ProfileIndexController = function($scope, $http) {
	$http.get("/api/ProfileWeb")
		.then(function(response) {
			$scope.profile = response.data;
		})
};


var ProfileEditController = function ($scope, $http) {
	$http.put("/api/ProfileWeb")
		.then(function (response) {
			$scope.profile = response.data;
		})
};

var ProfileCreateController = function ($scope, $http) {
	$http.post("/api/ProfileWeb")
		.then(function (response) {
			$scope.profile = response.data;
		})
};