app.run(function (eventsService, editorService) {

    eventsService.on("skybrud.umbraco.redirects.dashboard.init", function (_, args) {

        const add = args.buttonGroups.find(x => x.alias === "add");
        if (!add) return;

        add.subButtons.push({
            label: "Import",
            labelKey: "redirects_import",
            handler: function() {

                editorService.open({
                    title: "Import redirects",
                    size: "medium",
                    view: "/App_Plugins/Skybrud.Umbraco.Redirects.Import/Views/Dialogs/Import.html",
                    submit: function () {
                        editorService.close();
                    },
                    close: function () {
                        editorService.close();
                    }
                });

            }
        });

        add.subButtons.push({
            label: "Export",
            labelKey: "redirects_export",
            handler: function () {
                // open export dialog
            }
        });

    });

});