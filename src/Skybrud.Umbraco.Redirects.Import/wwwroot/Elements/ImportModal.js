import { html, css, repeat, when, nothing } from "@umbraco-cms/backoffice/external/lit";
import { UmbModalBaseElement } from "@umbraco-cms/backoffice/modal";

import { RedirectsImportService } from "@skybrud-redirects/import/service";
import { } from "@skybrud-redirects/import/elements/properties";

export class ImportRedirectsModalElement extends UmbModalBaseElement {

    constructor() {

        super();

        const self = this;

        this.goToImporter();

        RedirectsImportService.getImporters().then(function (importers) {
            self.importers = importers;
            self.requestUpdate();
        });

    }

    handleCancel() {
        this.modalContext?.reject();
    }

    import() {

        const self = this;

        // Initialize the request configuration
        const config = { body: new FormData() };

        // Initialize the JSON request body
        const body = {
            type: this.importer.type,
            config: {}
        };

        // Update the request body configuration
        this.importer.config.forEach(function (property) {
            if (property.value instanceof File) {
                config.body.append(property.alias, property.value);
            } else {
                body.config[property.alias] = property.value;
            }
        });

        // Append the JSON body as a blob to the form data
        config.body.append("body", JSON.stringify(body));

        // Set the overlay (submit button) as waiting
        this.submitButtonState = "waiting";
        this.requestUpdate();

        RedirectsImportService.import(config).then(function (response) {
            self.result = response.data;
            self.updateResult(self.result);
            self.goToResult();
        }, function (response) {
            self.result = response.data;
            self.updateResult(self.result);
            self.goToResult();
        });

    }

    selectImporter(importer) {
        this.importer = importer;
        this.submitButtonDisabled = false;
        this.requestUpdate();
    }

    renderSelectImporter() {
        if (!this.importers) return nothing;
        return html`
            <uui-box>
                <div class="importer-list">
                    ${repeat(this.importers, importer => importer.name, importer => html`
                        <button type="button" class="importer-item" @click=${() => this.selectImporter(importer)}>
                            <uui-icon name="${importer.icon}"></uui-icon>
                            <div>
                                <div class="importer-name">${importer.name}</div>
                                <div class="importer-description">${importer.description}</div>
                            </div>
                        </button>
                    `)}
                </div>
            </uui-box>
        `;

    }

    renderConfigureImporter() {

        if (this.importer.config.length === 0) {
            this.import();
            return html``;
        }

        return html`
            <uui-box>
                ${repeat(this.importer.config, property => property.alias, property => html`
                    <div class="property">
                        <skybrud-redirects-import-property .property=${property}></skybrud-redirects-import-property>
                    </div>
                `)}
            </uui-box>
        `;

    }

    updateResult(result) {

        if (result.ExceptionMessage) {
            result.errors = [result.ExceptionMessage];
            return;
        }

        if (Array.isArray(result.redirects)) {

            result.redirects.forEach(function (r) {

                // Why????
                if (!r.options.originalUrl) r.options.originalUrl = r.options.originalurl;

                r.messages = [];

                r.errors.forEach(function (e) {
                    r.messages.push({ type: "error", text: e });
                });

                r.warnings.forEach(function (w) {
                    r.messages.push({ type: "warning", text: w });
                });

                switch (r.status) {

                    case "Added":
                        r.messages.push({ type: "success", text: "New redirect was successfully added." });
                        r.icon = "icon-check color-green";
                        break;

                    case "Updated":
                        r.messages.push({ type: "success", text: "Existing redirect was successfully updated." });
                        r.icon = "icon-check color-green";
                        break;

                    case "NotModified":
                        r.icon = "icon-check color-grey";
                        r.messages.push({ type: "info", text: "Redirect matches existing redirect, but no new changes were found." });
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

        return;

    }

    renderResult() {
        return html`
            ${when(this.result.errors?.length > 0, () => html`
                <uui-alert color="danger" headline="Errors occurred during import" style="color: red;">
                    Meh, some errors occurred during the import:
                    <ul>
                        ${repeat(this.result.errors, error => html`<li>${error}</li>`)}
                    </ul>
                </uui-alert>
            `)}
            ${when(this.result.errors?.length > 0 == 0, () => html`
                <uui-box>
                    ${repeat(this.result.redirects, (redirect) => redirect, (redirect) => html`
                        <div class="redirect-import-result">
                            <h3>${redirect.options.originalUrl}</h3>
                            ${when(redirect.messages?.length > 0, () => html`
                                ${repeat(redirect.messages, (m) => m, (m) => html`
                                    <div class="${m.type}">${m.text}</div>
                                `)}
                            `)}
                        </div>
                    `)}
                </uui-box>
            `)}
        `;
    }

    goToImporter() {
        this.importer = null;
        this.result = null;
        this.cancelButtonLabel = this.localize.term("general_cancel");
        this.submitButtonLabel = "Import";
        this.submitButtonDisabled = true;
        this.submitButtonState = null;
        this.requestUpdate();
    }

    goToOptions() {
        this.result = null;
        this.cancelButtonLabel = this.localize.term("general_cancel");
        this.submitButtonLabel = "Import";
        this.submitButtonDisabled = false;
        this.submitButtonState = null;
        this.requestUpdate();
    }

    goToResult() {
        this.cancelButtonLabel = this.localize.term("general_close");
        this.submitButtonLabel = "Import";
        this.submitButtonDisabled = true;
        this.submitButtonState = "success";
        this.requestUpdate();
    }

    renderTabs() {

        if (!this.importer) {
            return html`
                <div class="tabs">
                    <uui-tab-group>
                        <uui-tab active="">Importer</uui-tab>
                        <uui-tab disabled>Options</uui-tab>
                        <uui-tab disabled>Result</uui-tab>
                    </uui-tab-group>
                </div>
            `;
        }

        if (this.result) {
            return html`
                <div class="tabs">
                    <uui-tab-group>
                        <uui-tab @click=${(e) => this.goToImporter()}>Importer</uui-tab>
                        <uui-tab @click=${(e) => this.goToOptions()}>Options</uui-tab>
                        <uui-tab active="">Result</uui-tab>
                    </uui-tab-group>
                </div>
            `;
        }

        return html`
            <div class="tabs">
                <uui-tab-group>
                    <uui-tab @click=${(e) => this.goToImporter()}>Importer</uui-tab>
                    <uui-tab active="">Options</uui-tab>
                    <uui-tab disabled>Result</uui-tab>
                </uui-tab-group>
            </div>
        `;


    }

    renderBody() {
        if (!this.importer) return this.renderSelectImporter();
        if (this.result) return this.renderResult();
        return this.renderConfigureImporter();
    }

    render() {
        return html`
            <umb-body-layout headline="Import redirects">
                ${this.renderTabs()}
                ${this.renderBody()}
                <div slot="actions">
                        <uui-button
                            id="cancel"
                            label="${this.cancelButtonLabel}"
                            @click="${this.handleCancel}"></uui-button>
                        <uui-button
                            id="submit"
                            color='positive'
                            look="primary"
                            label="${this.submitButtonLabel}"
                            state="${this.submitButtonState}"
                            ?disabled=${this.submitButtonDisabled}
                            @click="${this.import}"></uui-button>
                </div>
            </umb-body-layout>
        `;
    }

    static styles = css`

        .tabs {
            display: flex;
            margin-bottom: 20px;
        }

        .property + .property {
            margin-top: 30px;
        }

        .importer-list {
            display: flex;
            flex-wrap: wrap;
            gap: 10px;
        }

        .importer-item {
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

        .importer-name {
            font-weight: bold;
        }

        .importer-description {
            margin-top: 5px;
            font-sise: smaller;
        }

        .error {
            color: red;
        }

        .warning {
            color: orange;
        }

        .success {
            color: green;
        }

    `;

}

customElements.define("skybrud-redirects-import-modal", ImportRedirectsModalElement);

export default ImportRedirectsModalElement;