using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Skybrud.Umbraco.Redirects.Import.Models;
using Skybrud.Umbraco.Redirects.Import.Models.Csv;
using Skybrud.Umbraco.Redirects.Models;
using Umbraco.Core;
using Umbraco.Web.Composing;

namespace Skybrud.Umbraco.Redirects.Import.Services {
    
    public class RedirectsImportService {

        public object Import(RedirectsProviderFile file) {
            return Import(file, default(CsvImportOptions));
        }

        public object Import(RedirectsProviderFile file, Dictionary<string, string> options) {
            return Import(file, CsvImportOptions.FromDictionary(options));
        }

        public object Import(RedirectsProviderFile file, JObject options) {
            return Import(file, CsvImportOptions.Parse(options));
        }

        public object Import(RedirectsProviderFile file, CsvImportOptions options) {
            return new CsvRedirectsProvider().Import(file, options);
        }

        internal void Import(RedirectImportItem redirect, CsvImportOptions options) {

            // TODO: Use dependency injection
            var service = Current.Factory.GetInstance<IRedirectsService>();

            // TODO: Check whether redirect exists

            try {
                service.AddRedirect(redirect.AddOptions);
                redirect.Status = RedirectImportStatus.Success;
            } catch (Exception ex) {
                redirect.Errors.Add(ex.Message);
                redirect.Status = RedirectImportStatus.Failed;
            }

        }

    }

}