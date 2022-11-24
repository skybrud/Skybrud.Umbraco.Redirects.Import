using Newtonsoft.Json;
using Skybrud.Csv;
using Skybrud.Umbraco.Redirects.Import.Json.Newtonsoft.Converters;
using Skybrud.Umbraco.Redirects.Import.Models.Export;

namespace Skybrud.Umbraco.Redirects.Import.Exporters.Csv {

    /// <summary>
    /// Class reprenting the options for an export to a CSV file.
    /// </summary>
    public class CsvExportOptions : IExportColumnOptions {

        /// <summary>
        /// Gets or sets the encoding to be used for the export.
        /// </summary>
        public CsvExportEncoding Encoding { get; set; }

        /// <summary>
        /// Gets or sets the separator to be used for the exported CSV file.
        /// </summary>
        public CsvSeparator Separator { get; set; }

        /// <summary>
        /// Gets or sets whether a separator declaring should be added to the first line of the exported CSV file.
        /// </summary>
        [JsonConverter(typeof(BooleanJsonConverter))]
        public bool IncludeSeparator { get; set; }

        /// <summary>
        /// Gets or sets the columns that should be included in the exported CSV file.
        /// </summary>
        public ExportColumnList Columns { get; set; }

    }

}