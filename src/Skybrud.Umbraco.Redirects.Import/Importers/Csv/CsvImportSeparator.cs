using Newtonsoft.Json;
using Skybrud.Essentials.Json.Converters.Enums;

namespace Skybrud.Umbraco.Redirects.Import.Importers.Csv {

    /// <summary>
    /// Enum class representing the separator of a CSV file.
    /// </summary>
    [JsonConverter(typeof(EnumCamelCaseConverter))]
    public enum CsvImportSeparator {

        /// <summary>
        /// Indicates that importer should automatically try to detect the separator.
        /// </summary>
        Auto,

        /// <summary>
        /// Indicates that the importer should use a colon (<c>:</c>) as separator.
        /// </summary>
        Colon,

        /// <summary>
        /// Indicates that the importer should use a comma (<c>,</c>) as separator.
        /// </summary>
        Comma,

        /// <summary>
        /// Indicates that the importer should use a semi colon (<c>;</c>) as separator.
        /// </summary>
        SemiColon,

        /// <summary>
        /// Indicates that the importer should use a space (<c> </c>) as separator.
        /// </summary>
        Space,

        /// <summary>
        /// Indicates that the importer should use a tab (<c>	</c>) as separator.
        /// </summary>
        Tab

    }

}