import { html, css, repeat, when, nothing } from "@umbraco-cms/backoffice/external/lit";
import { UmbModalBaseElement } from "@umbraco-cms/backoffice/modal";

export class RedirectsImportItemListOptionElement extends UmbModalBaseElement {

    static properties = {
        property: { type: Object }
    };

    get property() {
        return this._property;
    }

    set property(value) {
        this._property = value;
        if (!Array.isArray(value?.config)) return;
        value.config.forEach(function (item) {
            item.selected = item.alias === value.value;
        });
        this.requestUpdate();
    }

    constructor() {
        super();
    }

    select(item) {
        this.property.value = item.alias;
        this.property.config.forEach(function (i) {
            i.selected = i == item;
        });
        this.requestUpdate();
    }

    render() {
        return html`
            ${when(this.property.config?.length > 0, () => html`
                <div class="items">
                ${repeat(this.property.config, (item) => item.alias, (item) => html`
                    <button type="button" class="item ${item.selected ? "active" : null}" @click=${() => this.select(item)}>
                        ${item.name}
                    </button>
                `)}
                </div>
            `)}
        `;
    }

    static styles = css`

        pre {
            font-size: 11px;
            line-height: 13px;
        }

        .items {
            display: flex;
            flex-wrap: wrap;
            gap: 7px;
        }

        .item {
            appearance: none;
            display: inline-block;
            -webkit-appearance: none;
            border: 0;
            font-weight: bold;
            color: #1A2650;
            line-height: 1;
            background-color: rgba(216,215,217, .5);
            font-size: 13px;
            padding: 10px 20px;
            border-radius: 4px;
            transition: all .2s ease;
            position: relative;
            cursor: pointer;
            &:hover {
                background-color: rgba(216,215,217, .3);
                color: #2152a3;
            }
        }

        .item.active {
            background-color: #F5C1BC;
            color: #1A2650;
            &:hover {
                background-color: #f2aca6;
            }
        }

    `;

}

customElements.define("skybrud-redirects-import-items", RedirectsImportItemListOptionElement);

export default RedirectsImportItemListOptionElement;