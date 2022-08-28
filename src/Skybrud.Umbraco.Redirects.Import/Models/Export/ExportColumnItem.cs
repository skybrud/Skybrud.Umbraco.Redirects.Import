using Newtonsoft.Json;

namespace Skybrud.Umbraco.Redirects.Import.Models.Export {
    public class ExportColumnItem {

        [JsonProperty("alias")]
        public string Alias { get; set; }

        [JsonProperty("selected")]
        public bool IsSelected { get; set; }

    }
}