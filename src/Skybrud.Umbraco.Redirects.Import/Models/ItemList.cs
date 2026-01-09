using System.Collections.Generic;
using System.Text.Json.Serialization;
using Skybrud.Umbraco.Redirects.Import.Json.Newtonsoft.Converters;

namespace Skybrud.Umbraco.Redirects.Import.Models;

/// <summary>
/// Class representing a list of <see cref="Item"/> objects.
/// </summary>
[JsonConverter(typeof(ItemListJsonConverter))]
public class ItemList : List<Item> {

    /// <summary>
    /// Initializes a new, empty instance of <see cref="ItemList"/>.
    /// </summary>
    public ItemList() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="ItemList"/> class that contains elements copied from the specified collection.
    /// </summary>
    /// <param name="source">The collection of <see cref="Item"/> objects to copy into the list. Cannot be <see langword="null"/>.</param>
    public ItemList(IEnumerable<Item> source) : base(source) { }

}