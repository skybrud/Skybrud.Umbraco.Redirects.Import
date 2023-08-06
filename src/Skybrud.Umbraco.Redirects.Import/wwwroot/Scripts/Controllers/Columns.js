angular.module("umbraco").controller("SkybrudUmbracoRedirects.ColumnsController", function ($scope) {

    const vm = this;

    vm.columns = [
        { alias: "Id", selected: true },
        { alias: "Key", selected: true },
        { alias: "RootKey", selected: true },
        { alias: "Url", selected: true },
        { alias: "QueryString", selected: true },
        { alias: "DestinationType", selected: true },
        { alias: "DestinationId", selected: false },
        { alias: "DestinationKey", selected: true },
        { alias: "DestinationUrl", selected: true },
        { alias: "DestinationQuery", selected: true },
        { alias: "DestinationFragment", selected: true },
        { alias: "Type", selected: true },
        { alias: "IsPermanent", selected: false },
        { alias: "ForwardQueryString", selected: true },
        { alias: "CreateDate", selected: true },
        { alias: "UpdateDate", selected: true }
    ];

    $scope.model.value = vm.columns;

});