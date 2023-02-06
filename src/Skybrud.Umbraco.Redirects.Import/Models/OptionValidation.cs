using Newtonsoft.Json;

namespace Skybrud.Umbraco.Redirects.Import.Models {

    /// <summary>
    /// Class describing the validation options of an <see cref="Option"/> instance.
    /// </summary>
    public class OptionValidation {

        /// <summary>
        /// Gets or sets whether the parent option is mandatory.
        /// </summary>
        [JsonProperty("mandatory")]
        public bool IsMandatory { get; set; }

    }

}