export class RedirectsImportPackage {

    static set serverVariables(value) {
        this._serverVariables = value;
    }

    static get version() {
        return this._serverVariables["version"];
    }

    static get cacheBuster() {
        return this._serverVariables["cacheBuster"];
    }

}

export default RedirectsImportPackage;