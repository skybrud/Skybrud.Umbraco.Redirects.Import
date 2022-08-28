angular.module("umbraco").controller("SkybrudUmbracoRedirects.ImportController", function ($scope, $http, Upload) {

    $scope.model.submitButtonDisabled = true;

    const vm = this;

    vm.title = $scope.model.title;
    vm.importers = [];

    vm.importer = null;
    vm.config = null;

    const tabImporter = {
        alias: "importer",
        label: "Importer",
        active: true
    };

    const tabOptions = {
        alias: "options",
        label: "Options",
        disabled: true
    };

    const tabResult = {
        alias: "result",
        label: "Result",
        disabled: true
    };

    vm.selectImporter = function (importer) {

        vm.importer = importer;
        vm.config = Utilities.copy(importer.config);

        $scope.model.submitButtonDisabled = false;
        $scope.model.title = `${vm.title}: ${importer.name}`;

        vm.changeTab(tabOptions);

        // If the importer doesn't specify an array with at least one configuration option, it means we don't need any
        // further input from the user, and we can therefore trigger an export right away
        if (!Array.isArray(importer.config) || importer.config.length === 0) {
            vm.save();
        }

    };

    vm.changeTab = function (selectedTab) {

        if (selectedTab === tabImporter) {
            tabOptions.disabled = true;
            tabResult.disabled = true;
            vm.result = null;
            vm.importer = null;
            vm.config = null;
            $scope.model.submitButtonDisabled = true;
        } else if (selectedTab === tabOptions) {
            tabOptions.disabled = false;
            tabResult.disabled = true;
            vm.result = null;
            $scope.model.submitButtonDisabled = false;
        } else if (selectedTab === tabResult) {
            tabResult.disabled = false;
            $scope.model.submitButtonDisabled = true;
        }

        vm.tabs.forEach(function (tab) {
            tab.active = false;
        });

        selectedTab.active = true;

    };

    vm.tabs = [tabImporter,tabOptions,tabResult];

    vm.save = function () {

        const body = {
            type: vm.importer.type,
            config: {}
        };

        // Update the request body configuration
        //vm.importer.config.forEach(function(option) {
        //    body.config[option.alias] = option.value;
        //});

        // Set the overlay (submit button) as busy
        $scope.model.submitButtonState = "busy";

        const options = {
            url: "/umbraco/backoffice/Skybrud/RedirectsImport/Import",
            data: {}
        };

        vm.config.forEach(function (f) {
            if (f.alias === "file") {
                options.file = f.value[0];
            } else {
                body.config[f.alias] = f.value;
            }
        });

        options.data.body = JSON.stringify(body);

        Upload.upload(options).progress(function (evt) {

            if (!options.file) return;

            if (options.file.uploadStat !== "done" && options.file.uploadStat !== "error") {
                const progressPercentage = parseInt(100 * evt.loaded / evt.total, 10);
                options.file.uploadProgress = progressPercentage;
                options.file.uploadStatus = "uploading";
            }

        }).then(function (response) {

            $scope.model.submitButtonState = "success";
            //$scope.model.submitButtonDisabled = true;

            //$scope.model.size = "large";



            //$scope.model.submit();

            vm.result = updateResult(response.data);
            vm.changeTab(tabResult);

        }, function (response) {

            $scope.model.submitButtonState = "error";

            vm.result = updateResult(response.data);
            vm.changeTab(tabResult);

            if (!options.file) return;
            options.file.uploadStatus = "error";
            options.file.errors = response.data.errors || [];

        });

    };

    function updateResult(result) {

        if (result.ExceptionMessage) {
            result.errors = [result.ExceptionMessage];
            return result;
        }

        if (Array.isArray(result.redirects)) {

            result.redirects.forEach(function (r) {

                if (!r.options.originalUrl) r.options.originalUrl = r.options.originalurl;

                r.messages = [];

                r.errors.forEach(function (e) {
                    r.messages.push({ class: "color-red", text: e });
                });

                r.warnings.forEach(function (w) {
                    r.messages.push({ class: "color-orange", text: w });
                });

                switch (r.status) {

                    case "Added":
                        r.messages.push({ class: "color-green", text: "New redirect was successfully added." });
                        r.icon = "icon-check color-green";
                        break;

                    case "Updated":
                        r.messages.push({ class: "color-green", text: "Existing redirect was successfully updated." });
                        r.icon = "icon-check color-green";
                        break;

                    case "NotModified":
                        r.icon = "icon-check color-grey";
                        r.messages.push({ class: "color-grey", text: "Redirect matches existing redirect, but no new changes were found." });
                        break;

                    case "AlreadyExists":
                        r.icon = "icon-stop-hand color-red";
                        break;

                    case "Failed":
                        r.icon = "icon-delete color-red";
                        break;

                    default:
                        r.icon = "icon-check color-green";
                        break;

                }

            });
        }

        return result;

    }

    function init() {
        $http.get("/umbraco/backoffice/Skybrud/RedirectsImport/GetImporters").then(function (response) {
            vm.importers = response.data;
        });
    };

    init();

});