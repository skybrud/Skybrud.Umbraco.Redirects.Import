using Skybrud.Umbraco.Redirects.Import.Models.Import;
using System.Collections.Generic;

namespace Skybrud.Umbraco.Redirects.Import.Importers.Json {

    /// <summary>
    /// Class representing the result of an import of redirects from a <strong>JSON</strong> file.
    /// </summary>
    public class JsonImportResult : IImportResult {

        #region Properties

        /// <summary>
        /// Gets whether the import was successful.
        /// </summary>
        public bool IsSuccessful { get; }

        /// <summary>
        /// Gets a list of errors triggered by the import.
        /// </summary>
        public IReadOnlyList<string> Errors { get; }

        /// <summary>
        /// Gets a list of the imported redirects.
        /// </summary>
        public IReadOnlyList<RedirectImportItem> Redirects { get; }

        #endregion

    }

}