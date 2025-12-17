import { html, css, repeat, when } from "@umbraco-cms/backoffice/external/lit";
import { UmbModalBaseElement } from "@umbraco-cms/backoffice/modal";

export class RedirectsExportColumnsElement extends UmbModalBaseElement {

    static properties = {
        property: { type: Object }
    };

    get isValid() {
        return this.property?.value?.length > 0;
    }

    get property() {
        return this._property;
    }

    set property(value) {

        this._property = value;

        if (value.value === null) {
            value.value = this.columns.filter(c => c.selected).map(c => c.alias).join(",");
        }

        this.requestUpdate();

    }

    constructor() {

        super();

        this.columns = [
            { alias: "Id", selected: true },
            { alias: "Key", selected: true },
            { alias: "RootKey", selected: true },
            { alias: "Url", selected: true },
            { alias: "QueryString", selected: true },
            { alias: "DestinationType", selected: true },
            { alias: "DestinationId", selected: false },
            { alias: "DestinationKey", selected: true },
            { alias: "DestinationUrl", selected: true },
            { alias: "DestinationQuery", selected: true },
            { alias: "DestinationFragment", selected: true },
            { alias: "Type", selected: true },
            { alias: "IsPermanent", selected: false },
            { alias: "ForwardQueryString", selected: true },
            { alias: "CreateDate", selected: true },
            { alias: "UpdateDate", selected: true }
        ];

    }

    toggle(column) {
        column.selected = !column.selected;
        this.property.value = this.columns.filter(c => c.selected).map(c => c.alias).join(",");
        this.requestUpdate();
    }

    render() {
        return html`
            ${when(this.columns && this.columns.length > 0, () => html`

                <div>
                    ${repeat(this.columns, (column) => column.alias, (column) => html`
                        <div>
                            <input type="checkbox" id="column-${column.alias}" .checked=${column.selected} @change=${(e) => this.toggle(column, e)} />
                            <label for="column-${column.alias}">${column.alias}</label>
                        </div>
                    `)}
                </div>

            `)}
        `;
    }

}

customElements.define("skybrud-redirects-export-columns", RedirectsExportColumnsElement);

export default RedirectsExportColumnsElement;