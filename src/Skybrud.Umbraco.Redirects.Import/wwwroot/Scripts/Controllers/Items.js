angular.module("umbraco").controller("SkybrudUmbracoRedirects.ItemsController", function ($scope) {

    const vm = this;

    vm.single = true;

    vm.items = $scope.model.config.items;

    vm.select = function (current) {

        vm.items.forEach(function (item) {
            item.selected = item.alias === current.alias;
        });

        $scope.model.value = current.alias;

    };

    if ($scope.model.value) {
        const item = vm.items.find(x => x.alias === $scope.model.value);
        if (item) {
            item.selected = true;
        } else {
            vm.items[0].selected = true;
            $scope.model.value = vm.items[0].alias;
        }
    } else {
        vm.items[0].selected = true;
        $scope.model.value = vm.items[0].alias;
    }

});