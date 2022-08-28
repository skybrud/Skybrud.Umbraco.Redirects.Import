using System;

namespace Skybrud.Umbraco.Redirects.Import.Importers.Json {

    public class JsonImporter : ImporterBase<JsonImportOptions, JsonImportResult> {

        private readonly RedirectsImportService _redirectsImportService;

        #region Constructors

        public JsonImporter(RedirectsImportService redirectsImportService) {
            _redirectsImportService = redirectsImportService;
            Icon = "icon-redirects-json";
            Name = "JSON";
            Description = "Lets you import redirects from a JSON file.";
        }

        #endregion

        #region Member methods

        public override JsonImportResult Import(JsonImportOptions options) {
            throw new NotImplementedException();
        }

        #endregion

    }

}