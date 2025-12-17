import { UmbElementMixin } from "@umbraco-cms/backoffice/element-api";
import { LitElement, html, css, repeat, when } from "@umbraco-cms/backoffice/external/lit";

export class RedirectsImportBooleanElement extends UmbElementMixin(LitElement) {

    static properties = {
        property: { type: Object }
    };

    get property() {
        return this._property;
    }

    set property(value) {
        this._property = value;
        if (value.value === null) value.value = false;
        this.requestUpdate();
    }

    constructor() {
        super();
    }

    toggle(event) {
        this.property.value = event.target.checked;
        this.requestUpdate();
    }

    onChange(event) {
        this.property.value = !this.property.value;
        this.requestUpdate();
    }

    render() {
        return html`
            <uui-toggle pristine="" ?checked=${this.property.value} @change=${(e) => this.onChange(e)}></uui-toggle>
        `;
    }

}

customElements.define("skybrud-redirects-import-boolean", RedirectsImportBooleanElement);

export default RedirectsImportBooleanElement;