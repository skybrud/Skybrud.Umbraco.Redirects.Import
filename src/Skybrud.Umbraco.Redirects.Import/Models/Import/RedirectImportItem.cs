using System.Collections.Generic;
using Newtonsoft.Json;
using Skybrud.Umbraco.Redirects.Models.Options;

namespace Skybrud.Umbraco.Redirects.Import.Models.Import {

    public class RedirectImportItem {

        /// <summary>
        /// The options describing the redirect to be added.
        /// </summary>
        [JsonProperty("options")]
        public AddRedirectOptions AddOptions { get; set; }

        /// <summary>
        /// Gets or sets the import status of the redirect.
        /// </summary>
        [JsonProperty("status")]
        public RedirectImportStatus Status { get; set; }

        /// <summary>
        /// Gets a list of errors triggered by the redirect.
        /// </summary>
        [JsonProperty("errors")]
        public List<string> Errors = new();

        /// <summary>
        /// Gets a list of warnings triggered by the redirect.
        /// </summary>
        [JsonProperty("warnings")]
        public List<string> Warnings = new();

    }

}