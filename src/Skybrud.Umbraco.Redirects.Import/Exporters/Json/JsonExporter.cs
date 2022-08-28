using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using Skybrud.Essentials.Reflection;
using Skybrud.Umbraco.Redirects.Import.Models;
using Skybrud.Umbraco.Redirects.Services;

namespace Skybrud.Umbraco.Redirects.Import.Exporters.Json {

    public class JsonExporter : ExporterBase<JsonExportOptions, JsonExportResult> {

        private readonly RedirectsImportService _redirectsImportService;

        #region Constructors

        public JsonExporter(RedirectsImportService redirectsImportService) {
            _redirectsImportService = redirectsImportService;
            Icon = "icon-redirects-json";
            Name = "JSON";
            Description = "Lets you export redirects to a JSON.";
        }

        #endregion

        #region Member methods

        public override IEnumerable<Option> GetOptions(HttpRequest request) {

            return new List<Option> {
                new () {
                    Alias = "formatting",
                    Label = "Encoding",
                    Description = "Select the formatting of the JSON file.",
                    View = $"{RedirectsImportPackage.AppPlugins}Views/Editors/Items.html?v={RedirectsPackage.Version}",
                    Value = "None",
                    Config = new Dictionary<string, object> {
                        {"items", new [] {
                            new Item { Alias = "None", Name = "None" },
                            new Item { Alias = "Indented", Name = "Indented" }
                        }}
                    }
                }
            };

        }

        public override JsonExportResult Export(JsonExportOptions options) {

            if (options == null) throw new ArgumentNullException(nameof(options));

            // We include the version numbers for future reference
            JObject versions = new();
            Assembly a1 = typeof(IRedirectsService).Assembly;
            Assembly a2 = typeof(RedirectsImportService).Assembly;
            versions.Add(a1.GetName().Name!, ReflectionUtils.GetInformationalVersion(a1));
            versions.Add(a2.GetName().Name!, ReflectionUtils.GetInformationalVersion(a2));

            // Generate the JSON
            JObject json = new() {
                {"versions", versions},
                {"redirects", JArray.FromObject(_redirectsImportService.GetRedirects(options))}
            };

            return new JsonExportResult(Guid.NewGuid(), json);

        }

        #endregion

    }

}