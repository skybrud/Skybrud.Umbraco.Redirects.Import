app.run(function (eventsService) {

    eventsService.on("skybrud.umbraco.redirects.dashboard.init", function (_, args) {

        const add = args.buttonGroups.find(x => x.alias === "add");
        if (!add) return;

        add.subButtons.push({
            label: "Import",
            labelKey: "redirects_import",
            handler: function() {
                // open import dialog
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