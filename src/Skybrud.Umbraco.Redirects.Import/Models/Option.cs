using Newtonsoft.Json;

namespace Skybrud.Umbraco.Redirects.Import.Models {

    public class Option {

        [JsonProperty("alias")]
        public string Alias { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("view")]
        public string View { get; set; }

        [JsonProperty("value")]
        public object Value { get; set; }

        [JsonProperty("config", NullValueHandling = NullValueHandling.Ignore)]
        public object Config { get; set; }

        [JsonProperty("validation", NullValueHandling = NullValueHandling.Ignore)]
        public OptionValidation Validation { get; set; }

    }

}