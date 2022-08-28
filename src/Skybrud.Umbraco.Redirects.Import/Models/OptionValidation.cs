using Newtonsoft.Json;

namespace Skybrud.Umbraco.Redirects.Import.Models {

    public class OptionValidation {

        [JsonProperty("mandatory")]
        public bool IsMandatory { get; set; }

    }

}