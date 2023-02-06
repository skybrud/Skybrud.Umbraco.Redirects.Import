using Microsoft.AspNetCore.Http;
using Skybrud.Umbraco.Redirects.Models;

namespace Skybrud.Umbraco.Redirects.Import.Importers.Json {

    /// <summary>
    /// Class representing the options for importing redirects from a <strong>JSON</strong> file.
    /// </summary>
    public class JsonImportOptions : IImportOptions {

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
        public IFormFile File { get; set; }

    }

}