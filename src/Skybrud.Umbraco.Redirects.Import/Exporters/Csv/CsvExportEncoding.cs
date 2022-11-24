using static System.Net.WebRequestMethods;

namespace Skybrud.Umbraco.Redirects.Import.Exporters.Csv {

    /// <summary>
    /// Enum class indicating an encoding.
    /// </summary>
    public enum CsvExportEncoding {

        /// <summary>
        /// Indicates that encoding is or should be <strong>UTF-8</strong>.
        /// </summary>
        /// <see>
        ///     <cref>https://en.wikipedia.org/wiki/UTF-8</cref>
        /// </see>
        Utf8,

        /// <summary>
        /// Indicates that encoding is or should be <strong>ASCII</strong>.
        /// </summary>
        /// <see>
        ///     <cref>https://en.wikipedia.org/wiki/ASCII</cref>
        /// </see>
        Ascii,

        /// <summary>
        /// Indicates that encoding is or should be <strong>Windows-1252</strong>.
        /// </summary>
        /// <see>
        ///     <cref>https://en.wikipedia.org/wiki/Windows-1252</cref>
        /// </see>
        Windows1252

    }

}