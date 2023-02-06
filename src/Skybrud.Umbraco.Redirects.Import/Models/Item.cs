using Newtonsoft.Json;

namespace Skybrud.Umbraco.Redirects.Import.Models {

    /// <summary>
    /// Class describing an item with an <see cref="Alias"/> and a <see cref="Name"/>.
    /// </summary>
    public class Item {

        /// <summary>
        /// Gets or sets the alias of the item.
        /// </summary>
        [JsonProperty("alias")]
        public string Alias { get; set; }

        /// <summary>
        /// Gets or sets the name of the item.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

    }

}