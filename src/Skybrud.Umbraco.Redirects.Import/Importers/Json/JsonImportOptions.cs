using Microsoft.AspNetCore.Http;
using Skybrud.Umbraco.Redirects.Models;

namespace Skybrud.Umbraco.Redirects.Import.Importers.Json {

    public class JsonImportOptions : IImportOptions {

        public bool OverwriteExisting { get; set; }

        public RedirectType DefaultRedirectType { get; set; }

        /// <summary>
        /// Gets or sets the uploaded file.
        /// </summary>
        public IFormFile File { get; set; }

    }

}