using System;

namespace Skybrud.Umbraco.Redirects.Import.Importers.Json {

    /// <summary>
    /// Class representing an importer based on an <strong>JSON</strong> file.
    /// </summary>
    public class JsonImporter : ImporterBase<JsonImportOptions, JsonImportResult> {

        private readonly RedirectsImportService _redirectsImportService;

        #region Constructors

        /// <summary>
        /// Initializes a new instance based on the specified dependencies.
        /// </summary>
        public JsonImporter(RedirectsImportService redirectsImportService) {
            _redirectsImportService = redirectsImportService;
            Icon = "icon-redirects-json";
            Name = "JSON";
            Description = "Lets you import redirects from a JSON file.";
        }

        #endregion

        #region Member methods

        /// <summary>
        /// Performs a new import based on the specified <paramref name="options"/>.
        /// </summary>
        /// <param name="options">The options describing the import.</param>
        /// <returns>An instance of <see cref="JsonImportResult"/> representing the result of the import.</returns>
        public override JsonImportResult Import(JsonImportOptions options) {
            throw new NotImplementedException();
        }

        #endregion

    }

}