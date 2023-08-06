using Newtonsoft.Json;
using Skybrud.Essentials.Json.Newtonsoft.Converters.Enums;

namespace Skybrud.Umbraco.Redirects.Import.Importers.Csv {

    /// <summary>
    /// Enum class representing the encoding of a CSV file.
    /// </summary>
    [JsonConverter(typeof(EnumCamelCaseConverter))]
    public enum CsvImportEncoding {

        /// <summary>
        /// Indicates that importer should automatically try to detect the encoding.
        /// </summary>
        Auto,

        /// <summary>
        /// Indicates that the encoding of a CSV file is <c>UTF-8</c>.
        /// </summary>
        Utf8,

        /// <summary>
        /// Indicates that the encoding of a CSV file is <c>ASCII</c>.
        /// </summary>
        Ascii,

        /// <summary>
        /// Indicates that the encoding of a CSV file is <c>Windows-1252</c>.
        /// </summary>
        Windows1252

    }

}