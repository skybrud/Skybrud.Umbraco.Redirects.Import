using Newtonsoft.Json;
using Skybrud.Umbraco.Redirects.Import.Models.Import;
using System.Collections.Generic;

namespace Skybrud.Umbraco.Redirects.Import.Importers {

    /// <summary>
    /// Interface describing the result of an import.
    /// </summary>
    public interface IImportResult {

        #region Properties

        /// <summary>
        /// Gets whether the import was successful.
        /// </summary>
        [JsonProperty("success")]
        bool IsSuccessful { get; }

        /// <summary>
        /// Gets a list of errors triggered by the import.
        /// </summary>
        [JsonProperty("errors", NullValueHandling = NullValueHandling.Ignore)]
        IReadOnlyList<string> Errors { get; }

        /// <summary>
        /// Gets a list of the imported redirects.
        /// </summary>
        [JsonProperty("redirects", NullValueHandling = NullValueHandling.Ignore)]
        IReadOnlyList<RedirectImportItem> Redirects { get; }

        #endregion

    }

}