angular.module("umbraco").controller("SkybrudUmbracoRedirects.ItemsController", function ($scope) {

    $scope.single = true;

    $scope.items = $scope.model.config.items;

    $scope.select = function(current) {

        $scope.items.forEach(function(item) {
            item.selected = item.alias === current.alias;
        });

        $scope.model.value = current.alias;

    };

    if ($scope.model.value) {
        const item = $scope.items.find(x => x.alias === $scope.model.value);
        if (item) {
            item.selected = true;
        } else {
            $scope.items[0].selected = true;
            $scope.model.value = $scope.items[0].alias;
        }
    } else {
        $scope.items[0].selected = true;
        $scope.model.value = $scope.items[0].alias;
    }

});