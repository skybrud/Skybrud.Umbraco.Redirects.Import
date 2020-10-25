angular.module("umbraco").controller("SkybrudUmbracoRedirects.ImportController", function ($scope, $http, Upload) {

    $scope.model.submitButtonDisabled = true;

    $scope.providers = [];

    $http.get("/umbraco/backoffice/Skybrud/RedirectsImport/GetProviders").then(function(r) {
        $scope.providers = r.data;
    });


    $scope.selectProvider = function(provider) {

        $scope.provider = provider;

        var f = provider.fields.find(x => x.key === "file");

        if (f) {
            f.changed = function (value) {
                $scope.model.submitButtonDisabled = value.length === 0;
            };
        }

    };

    $scope.import = function () {

        var options = {
            url: "/umbraco/backoffice/Skybrud/RedirectsImport/Upload",
            fields: {}
        };

        $scope.provider.fields.forEach(function(f) {
            if (f.key === "file") {
                options.file = f.value[0];
            } else {
                options.fields[f.key] = f.value;
            }
        });

        $scope.model.submitButtonState = "busy";

        Upload.upload(options).progress(function (evt) {

            if (file.uploadStat !== "done" && file.uploadStat !== "error") {
                var progressPercentage = parseInt(100 * evt.loaded / evt.total, 10);
                file.uploadProgress = progressPercentage;
                file.uploadStatus = "uploading";
            }

        }).then(function (res) {

            $scope.model.submitButtonState = "success";
            $scope.model.submitButtonDisabled = true;

            $scope.model.size = "large";

            //$scope.model.submit();

            $scope.result = res.data;

        }, function (evt, status, headers, config) {

            //file.uploadStatus = "error";

            //$scope.done.push(file);
            //$scope.currentFile = null;

            //if (evt.Message) {
            //    file.serverErrorMessage = evt.Message;
            //}

                //processQueueItem();

                $scope.model.submitButtonState = "error";


        });

    };


    return;










    $scope.queue = [];
    $scope.done = [];
    $scope.currentFile = null;

    var processQueueItem = function () {

        console.log("processQueueItem");

        if ($scope.queue.length > 0) {
            $scope.currentFile = $scope.queue.shift();
            upload($scope.currentFile);
            return;
        }

        $scope.model.size = "large";

    };

    function upload(file) {

        console.log("upload");

        Upload.upload({
            url: "/umbraco/backoffice/Skybrud/RedirectsImport/Upload",
            fields: {
                path: file.path
            },
            file: file
        }).progress(function (evt) {

            if (file.uploadStat !== "done" && file.uploadStat !== "error") {
                var progressPercentage = parseInt(100 * evt.loaded / evt.total, 10);
                file.uploadProgress = progressPercentage;
                file.uploadStatus = "uploading";
            }

        }).then(function (res) {

            $scope.hest = res.data;

            $scope.currentFile = null;

            file.uploadStatus = "done";

            $scope.done.push(file);

            processQueueItem();

        }, function (evt, status, headers, config) {

            file.uploadStatus = "error";

            $scope.done.push(file);
            $scope.currentFile = null;

            if (evt.Message) {
                file.serverErrorMessage = evt.Message;
            }

            processQueueItem();


        });

    }

    $scope.handleFiles = function (files, event) {

        console.log("handleFiles");

        files.forEach(function (file) {
            $scope.queue.push(file);
        });

        processQueueItem();

    };


});