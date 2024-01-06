using Newtonsoft.Json;

namespace Skybrud.Umbraco.Redirects.Import.Models;

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

    /// <summary>
    /// Initializes a new instance based on the specified <paramref name="alias"/> and <paramref name="name"/>.
    /// </summary>
    /// <param name="alias">The alias of the item.</param>
    /// <param name="name">The name of the item.</param>
    public Item(string alias, string name) {
        Alias = alias;
        Name = name;
    }

}