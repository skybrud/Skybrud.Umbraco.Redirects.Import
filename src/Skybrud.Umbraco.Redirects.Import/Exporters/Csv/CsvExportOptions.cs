using Newtonsoft.Json;
using Skybrud.Csv;
using Skybrud.Umbraco.Redirects.Import.Json.Newtonsoft.Converters;
using Skybrud.Umbraco.Redirects.Import.Models.Export;

namespace Skybrud.Umbraco.Redirects.Import.Exporters.Csv {

    public class CsvExportOptions : IExportColumnOptions {

        public CsvExportEncoding Encoding { get; set; }

        public CsvSeparator Separator { get; set; }

        [JsonConverter(typeof(BooleanJsonConverter))]
        public bool IncludeSeparator { get; set; }

        public ExportColumnList Columns { get; set; }

    }

}