import { nothing } from "@umbraco-cms/backoffice/external/lit";
import { UmbModalBaseElement } from "@umbraco-cms/backoffice/modal";
import { UMB_MODAL_MANAGER_CONTEXT } from "@umbraco-cms/backoffice/modal";
import { UMB_NOTIFICATION_CONTEXT } from '@umbraco-cms/backoffice/notification';

import { REDIRECTS_IMPORT_MODAL } from "../Modals/Import.js";
import { REDIRECTS_EXPORT_MODAL } from "../Modals/Export.js";

export class RedirectsImportDashboardElement extends UmbModalBaseElement {

    constructor() {

        super();

        const self = this;

        this.consumeContext(UMB_MODAL_MANAGER_CONTEXT, (instance) => {
            self._modalManagerContext = instance;
        });

        this.consumeContext(UMB_NOTIFICATION_CONTEXT, (instance) => {
            self._notificationContext = instance;
        });

    }

    openImportModal() {
        this._modalManagerContext.open(this, REDIRECTS_IMPORT_MODAL);
    }

    openExportModal() {
        this._modalManagerContext.open(this, REDIRECTS_EXPORT_MODAL);
    }

    render() {
        return nothing;
    }

}

customElements.define("skybrud-redirects-import-dashboard", RedirectsImportDashboardElement);

export default RedirectsImportDashboardElement;