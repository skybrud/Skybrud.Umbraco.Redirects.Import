angular.module("umbraco").controller("SkybrudUmbracoRedirects.FileController", function ($scope) {

    if (!$scope.model.value) $scope.model.value = [];

    $scope.handleFiles = function (files) {
        files.forEach(function (file) {
            $scope.model.value.push(file);
            console.log(file);
        });
        if ($scope.model.changed) $scope.model.changed($scope.model.value);
    };

});