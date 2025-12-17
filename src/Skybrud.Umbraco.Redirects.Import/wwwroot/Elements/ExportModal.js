import { html, css, repeat, when, nothing } from "@umbraco-cms/backoffice/external/lit";
import { UmbModalBaseElement } from "@umbraco-cms/backoffice/modal";

import { RedirectsImportService } from "@skybrud-redirects/import/service";
import { } from "@skybrud-redirects/import/elements/properties";

export class ExportRedirectsModalElement extends UmbModalBaseElement {

    constructor() {

        super();

        const self = this;

        this.submitButtonDisabled = true;

        RedirectsImportService.getExporters().then(function (exporters) {
            self.exporters = exporters;
            self.requestUpdate();
        });

    }

    handleCancel() {
        this.modalContext?.reject();
    }

    selectExporter(exporter) {
        this.exporter = JSON.parse(JSON.stringify(exporter));
        this.submitButtonDisabled = false;
        this.requestUpdate();
    }

    renderSelectExporter() {
        if (!this.exporters) return nothing;
        return html`
            <uui-box>
                <div class="exporter-list">
                    ${repeat(this.exporters, exporter => exporter.name, exporter => html`
                        <button type="button" class="exporter-item" @click=${() => this.selectExporter(exporter)}>
                            <uui-icon name="${exporter.icon}"></uui-icon>
                            <div>
                                <div class="exporter-name">${exporter.name}</div>
                                <div class="exporter-description">${exporter.description}</div>
                            </div>
                        </button>
                    `)}
                </div>
            </uui-box>
        `;

    }

    renderConfigureExporter() {

        if (this.exporter.config.length === 0) {
            this.export();
            return html``;
        }

        return html`
            <uui-box>
                ${repeat(this.exporter.config, property => property.alias, property => html`
                    <div class="property">
                        <skybrud-redirects-import-property .property=${property}></skybrud-redirects-import-property>
                    </div>
                `)}
            </uui-box>
        `;

    }

    export() {

        const self = this;

        // Initialize an object for the request body
        const body = {
            type: this.exporter.type,
            config: {}
        };

        // Update the request body configuration
        this.exporter.config.forEach(function (property) {
            body.config[property.alias] = property.value;
        });

        // Set the overlay (submit button) as busy
        this.submitButtonState = "waiting";
        this.requestUpdate();

        RedirectsImportService.export(body).then(function (response) {

            const key = response.data.key;
            const filename = response.data.fileName;

            // Generate the download URL from the 'key' and 'filename'
            const downloadUrl = `/umbraco/skybrud/redirects/import/export/${key}/${filename}`;

            // Create a fake <a> element
            const link = document.createElement("a");
            link.setAttribute("href", downloadUrl);
            link.setAttribute("download", filename);
            link.setAttribute("target", "_blank");

            // Click the link
            link.click();

            // Submit the modal (so it closes)
            self.modalContext?.submit();

        });

    }

    render() {
        return html`
            <umb-body-layout headline="Export redirects">
                ${when(!this.exporter, () => this.renderSelectExporter())}
                ${when(this.exporter && !this.config, () => this.renderConfigureExporter())}
                <div slot="actions">
                        <uui-button id="cancel" label="${this.localize.term("general_cancel")}" @click="${this.handleCancel}">${this.localize.term("general_cancel")}</uui-button>
                        <uui-button
                            id="submit"
                            color='positive'
                            look="primary"
                            label="Export"
                            state="${this.submitButtonState}"
                            ?disabled=${this.submitButtonDisabled}
                            @click="${this.export}"></uui-button>
                </div>
            </umb-body-layout>
        `;
    }

    static styles = css`

        .property + .property {
            margin-top: 30px;
        }

        .exporter-list {
            display: flex;
            flex-wrap: wrap;
            gap: 10px;
        }

        .exporter-item {
            display: flex;
            align-items: center;
            gap: 20px;
            border: 0;
            width: 100%;
            background: none;
            text-align: left;
            cursor: pointer;
            padding: 10px;
            font-size: 14px;
            &:hover {
                background-color: rgba(216,215,217, .3);
            }
            uui-icon {
                font-size: 48px;
            }
        }

        .exporter-name {
            font-weight: bold;
        }

        .exporter-description {
            margin-top: 5px;
            font-sise: smaller;
        }

    `;

}

customElements.define("skybrud-redirects-export-modal", ExportRedirectsModalElement);

export default ExportRedirectsModalElement;