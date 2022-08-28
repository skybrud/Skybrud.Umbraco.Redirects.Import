using Newtonsoft.Json;

namespace Skybrud.Umbraco.Redirects.Import.Models {

    public class Item {

        [JsonProperty("alias")]
        public string Alias { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

    }

}