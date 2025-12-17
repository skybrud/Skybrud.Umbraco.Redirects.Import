import { html, css, repeat, when, nothing } from "@umbraco-cms/backoffice/external/lit";
import { UmbModalBaseElement } from "@umbraco-cms/backoffice/modal";

export class RedirectsExportPropertyElement extends UmbModalBaseElement {

    static properties = {
        property: { type: Object }
    };

    get property() {
        return this._property;
    }

    set property(value) {
        this._property = value;
        if (!this._element) {
            this._element = document.createElement(value.element);
            this._element.property = value;
        }
        this.requestUpdate();
    }

    constructor() {
        super();
    }

    render() {
        if (!this.property) return nothing;
        return html`
            <div class="property">
                <strong>${this.property.label}</strong><br />
                <small>${this.property.description}</small>
                <div class="property-content">
                    ${this._element}
                </div>
            </div>
        `;
    }

    static styles = css`

        .property-content {
            margin-top: 10px;
        }

    `;

}

customElements.define("skybrud-redirects-import-property", RedirectsExportPropertyElement);

export default RedirectsExportPropertyElement;