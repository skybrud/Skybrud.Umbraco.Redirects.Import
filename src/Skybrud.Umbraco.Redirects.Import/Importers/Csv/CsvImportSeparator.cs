using Newtonsoft.Json;
using Skybrud.Essentials.Json.Converters.Enums;

namespace Skybrud.Umbraco.Redirects.Import.Importers.Csv {

    [JsonConverter(typeof(EnumCamelCaseConverter))]
    public enum CsvImportSeparator {
        Auto,
        Colon,
        Comma,
        SemiColon,
        Space,
        Tab
    }

}