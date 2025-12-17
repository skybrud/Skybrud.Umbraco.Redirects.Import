import { UMB_AUTH_CONTEXT } from "@umbraco-cms/backoffice/auth";

import { RedirectsImportAuth } from "@skybrud-redirects/import/auth";
import { RedirectsImportPackage } from "@skybrud-redirects/import/package";
import { RedirectsImportService } from "@skybrud-redirects/import/service";

import { RedirectsImportDashboardElement } from "./Elements/Dashboard.js";

export const onInit = (_host, extensionRegistry) => {

    _host.consumeContext(UMB_AUTH_CONTEXT, (authContext) => {

        const config = authContext.getOpenApiConfiguration();
        RedirectsImportAuth.TOKEN = config.token;

        RedirectsImportService.getServerVariables().then(function (serverVariables) {

            RedirectsImportPackage.serverVariables = serverVariables;

            extensionRegistry.register({
                "type": "modal",
                "alias": "Skybrud.Umbraco.Redirects.Import.ImportRedirectsModal",
                "name": "Import Redirects Modal",
                "element": "/App_Plugins/Skybrud.Umbraco.Redirects.Import/Elements/ImportModal.js?v=" + RedirectsImportPackage.cacheBuster,
            });

            extensionRegistry.register({
                "type": "modal",
                "alias": "Skybrud.Umbraco.Redirects.Import.ExportRedirectsModal",
                "name": "Export Redirects Modal",
                "element": "/App_Plugins/Skybrud.Umbraco.Redirects.Import/Elements/ExportModal.js?v=" + RedirectsImportPackage.cacheBuster,
            });

            extensionRegistry.register({
                "type": "icons",
                "alias": "Skybrud.Umbraco.Redirects.Import.Icons",
                "name": "Skybrud Redirects Import Icons",
                "js": "/App_Plugins/Skybrud.Umbraco.Redirects.Import/Icons.js?v=" + RedirectsImportPackage.cacheBuster,
            });

        });

        window.addEventListener("redirects.onDashboardLoad", function (event) {

            const addButton = event.dashboard?.addButton;

            if (!addButton) return;

            const module = document.createElement("skybrud-redirects-import-dashboard");

            setTimeout(function () {
                event.dashboard.element.parentElement.insertBefore(module, event.dashboard.element);
            }, 250);

            addButton.subButtons.push({
                label: "Import",
                look: "secondary",
                color: "default",
                action: function () {
                    module.openImportModal();
                }
            });

            addButton.subButtons.push({
                label: "Export",
                look: "secondary",
                color: "default",
                action: function () {
                    module.openExportModal();
                }
            });

        });

    });

};