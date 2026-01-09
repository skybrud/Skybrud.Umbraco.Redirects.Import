import { RedirectsImportAuth } from "@skybrud-redirects/import/auth";

function _fetch(url, config) {

    if (!config) config = {};
    if (!config.method) config.method = "GET";
    if (!config.headers) config.headers = {};

    return new Promise((resolve, reject) => {

        RedirectsImportAuth.TOKEN().then(function (token) {

            config.headers.Authorization = "Bearer " + token;

            const response = fetch(url, config);

            response.then(function (res) {
                if (res.status < 400) {
                    resolve(res);
                } else {
                    reject(res);
                }
            }, function (res) {
                console.log("failed", arguments);
            });

        });

    });

}

function hi(url, config) {

    if (!config) config = {};
    if (!config.method) config.method = "GET";
    if (!config.headers) config.headers = {};

    return new Promise((resolve, reject) => {

        RedirectsImportAuth.TOKEN().then(function (token) {

            config.headers.Authorization = "Bearer " + token;

            const response = fetch(url, config);

            response.then(function (res) {

                const contentType = res.headers.get("content-type") || "";

                if (contentType.includes("application/json")) {
                    res.json().then(function (json) {
                        res.data = json;
                        if (res.status < 400) {
                            resolve(res);
                        } else {
                            reject(res);
                        }
                    });
                } else {
                    if (res.status < 400) {
                        resolve(res);
                    } else {
                        reject(res);
                    }
                }

            }, function (res) {

                console.log("failed", arguments);

            });

        });

    });

}

function get(url) {
    return hi(url);
}

function post(url, body, config) {
    if (!config) config = {};
    config.method = "POST";
    if (!config.headers) config.headers = {};
    config.headers["Content-Type"] = "application/json";
    config.body = JSON.stringify(body);
    return hi(url, config);
}

export class RedirectsImportService {

    static getServerVariables() {
        return get("/umbraco/skybrud/redirects/import/serverVariables").then(function (res) {
            return res.data;
        });
    }

    static getExporters() {
        return get("/umbraco/skybrud/redirects/import/exporters").then(function (res) {
            return res.data;
        });
    }

    static getImporters() {
        return get("/umbraco/skybrud/redirects/import/importers").then(function (res) {
            return res.data;
        });
    }

    static export(body) {
        return post("/umbraco/skybrud/redirects/import/export", body);
    }

    static import(config) {
        config.method = "POST";
        return hi("/umbraco/skybrud/redirects/import/import", config);
    }

    static async downloadFile(key, filename) {

        // Generate the download URL from the 'key' and 'filename'
        const url = `/umbraco/skybrud/redirects/import/export/${key}/${filename}`;

        // Perform the GET request to download the file
        return await _fetch(url);

    }

};

export default RedirectsImportService;