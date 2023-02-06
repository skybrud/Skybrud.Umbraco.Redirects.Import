using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Skybrud.Umbraco.Redirects.Import.Json.Newtonsoft.Converters;
using Skybrud.Umbraco.Redirects.Models;

namespace Skybrud.Umbraco.Redirects.Import.Importers {

    /// <summary>
    /// Interface describing common options for importing one or more redirects.
    /// </summary>
    public interface IImportOptions {

        /// <summary>
        /// Gets whether existing redirects should be overwritten if the inbound URL is the same.
        /// </summary>
        [JsonConverter(typeof(BooleanJsonConverter))]
        bool OverwriteExisting { get; set; }

        /// <summary>
        /// Gets or sets the default redirect type to be used when imported files doesn't explicitly specify a type.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        RedirectType DefaultRedirectType { get; set; }

        /// <summary>
        /// Gets or sets the uploaded file.
        /// </summary>
        IFormFile? File { get; set; }

    }

}