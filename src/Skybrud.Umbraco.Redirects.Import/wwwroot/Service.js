import { RedirectsImportAuth } from "@skybrud-redirects/import/auth";

function hi(url, config) {

    if (!config) config = {};
    if (!config.method) config.method = "GET";
    if (!config.headers) config.headers = {};

    return new Promise((resolve, reject) => {

        RedirectsImportAuth.TOKEN().then(function (token) {

            config.headers.Authorization = "Bearer " + token;

            //console.log(config.method + " " + url);

            const response = fetch(url, config);

            response.then(function (res) {

                res.json().then(function (json) {
                    res.data = json;
                    if (res.status < 400) {
                        resolve(res);
                    } else {
                        reject(res);
                    }
                });

            }, function (res) {

                // sending the request failed (before actually calling the URL)

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

};

export default RedirectsImportService;