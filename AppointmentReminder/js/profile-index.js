var profileIndexController = function ($scope, $http) {

	//$scope.isBusy = true;

	//$http.get("/api/ProfileWeb")
	//	.then(
	//		function (result) {
	//			angular.copy(result.data, $scope.profile);
	//		}
	//	)
	//	.then(function () {
	//		$scope.isBusy = false;
	//	});

	//;

	var resultPromise = $http.get("/api/ProfileWeb");
	resultPromise.success(function (data) {
		$scope.profile = data;
	});
}