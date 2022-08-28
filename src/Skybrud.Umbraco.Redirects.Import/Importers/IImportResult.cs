using Newtonsoft.Json;
using Skybrud.Umbraco.Redirects.Import.Models.Import;
using System.Collections.Generic;

namespace Skybrud.Umbraco.Redirects.Import.Importers {

    public interface IImportResult {

        #region Properties

        [JsonProperty("success")]
        bool IsSuccessful { get; }

        [JsonProperty("errors", NullValueHandling = NullValueHandling.Ignore)]
        IReadOnlyList<string> Errors { get; }

        [JsonProperty("redirects", NullValueHandling = NullValueHandling.Ignore)]
        IReadOnlyList<RedirectImportItem> Redirects { get; }

        #endregion

    }

}