using Newtonsoft.Json;
using Skybrud.Umbraco.Redirects.Import.Models.Export;

namespace Skybrud.Umbraco.Redirects.Import.Exporters {

    /// <summary>
    /// Interface describing export options with a <see cref="Columns"/> property identifying the columns that should be exported.
    /// </summary>
    public interface IExportColumnOptions : IExportOptions {

        /// <summary>
        /// Gets a list of the columns that should be exported.
        /// </summary>
        [JsonProperty("columns", Order = 50)]
        ExportColumnList Columns { get; set; }

    }

}