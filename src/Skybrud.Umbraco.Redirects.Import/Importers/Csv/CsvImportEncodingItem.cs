using Newtonsoft.Json;

namespace Skybrud.Umbraco.Redirects.Import.Importers.Csv {

    public class CsvImportEncodingItem {

        [JsonProperty("alias")]
        public string Alias { get; }

        [JsonProperty("name")]
        public string Name { get; }

        public CsvImportEncodingItem(string alias, string name) {
            Alias = alias;
            Name = name;
        }

    }

}