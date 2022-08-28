angular.module("umbraco").controller("SkybrudUmbracoRedirects.ExportController", function ($scope, $http) {

    $scope.model.submitButtonDisabled = true;

    const vm = this;

    vm.exporters = [];

    vm.exporter = null;

    vm.selectExporter = function (exporter) {

        vm.exporter = exporter;
        $scope.model.submitButtonDisabled = false;
        $scope.model.title += `: ${exporter.name}`;

        // If the exporter doesn't specify an array with at least one configuration option, it means we don't need any
        // further input from the user, and we can therefore trigger an export right away
        if (!Array.isArray(exporter.config) || exporter.config.length === 0) {
            vm.save();
        }

    };

    vm.save = function() {

        // Initialize an object for the request body
        const body = {
            type: vm.exporter.type,
            config: {}
        };

        // Update the request body configuration
        vm.exporter.config.forEach(function(option) {
            body.config[option.alias] = option.value;
        });

        // Set the overlay (submit button) as busy
        $scope.model.submitButtonState = "busy";

        // Trigger a new export
        $http.post("/umbraco/backoffice/Skybrud/RedirectsImport/Export", body).then(function (response) {

            // TODO: Handle error scenarios

            const key = response.data.key;
            const filename = response.data.fileName;

            // Generate the download URL from the 'key' and 'filename'
            const downloadUrl = `/umbraco/backoffice/Skybrud/RedirectsImport/GetExportedFile?key=${key}&filename=${filename}`;

            // Create a fake <a> element
            const link = document.createElement("a");
            link.setAttribute("href", downloadUrl);
            link.setAttribute("download", filename);
            link.setAttribute("target", "_blank");

            // Click the link
            link.click();

            // Submit the overlay (so it closes)
            $scope.model.submit();

        });

    };

    function init() {
        $http.get("/umbraco/backoffice/Skybrud/RedirectsImport/GetExporters").then(function (response) {
            vm.exporters = response.data;
        });
    };

    init();

});