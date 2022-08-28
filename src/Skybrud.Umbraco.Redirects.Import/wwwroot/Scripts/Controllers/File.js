angular.module("umbraco").controller("SkybrudUmbracoRedirects.FileController", function ($scope) {

    if (!$scope.model.value) $scope.model.value = [];

    const vm = this;

    vm.handleFiles = function (files) {
        files.forEach(function (file) {
            $scope.model.value.push(file);
        });
        if ($scope.model.changed) $scope.model.changed($scope.model.value);
    };

    vm.reset = function() {
        $scope.model.value = [];
    };

});