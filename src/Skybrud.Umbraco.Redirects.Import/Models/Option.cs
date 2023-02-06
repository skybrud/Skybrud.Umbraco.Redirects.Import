using Newtonsoft.Json;

namespace Skybrud.Umbraco.Redirects.Import.Models {

    /// <summary>
    /// Class representing a configurable option presented to the user.
    /// </summary>
    public class Option {

        /// <summary>
        /// Gets or sets the alias of the option.
        /// </summary>
        [JsonProperty("alias")]
        public string Alias { get; set; }

        /// <summary>
        /// Gets or sets the label of the option.
        /// </summary>
        [JsonProperty("label")]
        public string Label { get; set; }

        /// <summary>
        /// Gets or sets the description of the option.
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the URL of the view of the option.
        /// </summary>
        [JsonProperty("view")]
        public string View { get; set; }

        /// <summary>
        /// Gets or sets the value of the option.
        /// </summary>
        [JsonProperty("value")]
        public object Value { get; set; }

        /// <summary>
        /// Gets or sets the configuration of the option.
        /// </summary>
        [JsonProperty("config", NullValueHandling = NullValueHandling.Ignore)]
        public object Config { get; set; }

        /// <summary>
        /// Gets or sets an object describing the validation of the option.
        /// </summary>
        [JsonProperty("validation", NullValueHandling = NullValueHandling.Ignore)]
        public OptionValidation Validation { get; set; }

    }

}