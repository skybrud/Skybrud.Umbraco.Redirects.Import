import { UmbElementMixin } from "@umbraco-cms/backoffice/element-api";
import { LitElement, html, css, repeat, when } from "@umbraco-cms/backoffice/external/lit";

export class RedirectsImportFileElement extends UmbElementMixin(LitElement) {

    static properties = {
        property: { type: Object }
    };

    get isValid() {
        return this.property?.value instanceof File;
    }

    constructor() {
        super();
    }

    _onFileChange(event) {
        this.property.value = event.target.files[0];
        this.requestUpdate();
    }

    render() {
        return html`
            <input type="file" @change=${(e) => this._onFileChange(e)} />
        `;
    }

}

customElements.define("skybrud-redirects-import-file", RedirectsImportFileElement);

export default RedirectsImportFileElement;