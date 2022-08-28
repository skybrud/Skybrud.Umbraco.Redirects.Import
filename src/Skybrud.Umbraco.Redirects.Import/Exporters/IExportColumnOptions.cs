using Newtonsoft.Json;
using Skybrud.Umbraco.Redirects.Import.Models.Export;

namespace Skybrud.Umbraco.Redirects.Import.Exporters {

    public interface IExportColumnOptions : IExportOptions {

        [JsonProperty("columns", Order = 50)]
        ExportColumnList Columns { get; set; }

    }

}
