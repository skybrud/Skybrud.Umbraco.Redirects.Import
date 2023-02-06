using Newtonsoft.Json;

namespace Skybrud.Umbraco.Redirects.Import.Models.Export {

    /// <summary>
    /// Class describing a column in an export.
    /// </summary>
    public class ExportColumnItem {

        /// <summary>
        /// Gets the alias of the column.
        /// </summary>
        [JsonProperty("alias")]
        public string Alias { get; set; }

        /// <summary>
        /// Gets whether the column has been selected.
        /// </summary>
        [JsonProperty("selected")]
        public bool IsSelected { get; set; }

    }

}