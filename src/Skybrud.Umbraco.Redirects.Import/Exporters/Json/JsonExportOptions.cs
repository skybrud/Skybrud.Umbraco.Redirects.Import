using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Skybrud.Umbraco.Redirects.Import.Exporters.Json {

    public class JsonExportOptions : IExportOptions {

        [JsonProperty("formatting")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Formatting Formatting { get; set; }

    }

}