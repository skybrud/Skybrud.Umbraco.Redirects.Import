using Newtonsoft.Json;
using Skybrud.Essentials.Json.Converters.Enums;

namespace Skybrud.Umbraco.Redirects.Import.Importers.Csv {

    [JsonConverter(typeof(EnumCamelCaseConverter))]
    public enum CsvImportEncoding {
        Auto,
        Utf8,
        Ascii,
        Windows1252
    }

}