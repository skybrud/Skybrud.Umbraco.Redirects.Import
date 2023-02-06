using Microsoft.AspNetCore.Http;
using Skybrud.Umbraco.Redirects.Models;

namespace Skybrud.Umbraco.Redirects.Import.Importers.Csv {

    /// <summary>
    /// Class representing the options for import redirects from a <strong>CSV</strong> file.
    /// </summary>
    public class CsvImportOptions : IImportOptions {

        /// <summary>
        /// Gets or sets whether existing redirects should be overwritten if the root node, path and query matches.
        /// </summary>
        public bool OverwriteExisting { get; set; }

        /// <summary>
        /// Gets or sets the default redirect type.
        /// </summary>
        public RedirectType DefaultRedirectType { get; set; }

        /// <summary>
        /// Gets or sets the uploaded file.
        /// </summary>
        public IFormFile? File { get; set; }

        /// <summary>
        /// Gets or sets the separator to be used when importing the CSV file.
        /// </summary>
        public CsvImportSeparator Separator { get; set; }

        /// <summary>
        /// Gets or sets the encoding to be used when importing the CSV file.
        /// </summary>
        public CsvImportEncoding Encoding { get; set; }

    }

}