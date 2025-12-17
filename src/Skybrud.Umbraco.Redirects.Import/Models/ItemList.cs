using System.Collections.Generic;
using System.Text.Json.Serialization;
using Skybrud.Umbraco.Redirects.Import.Json.Newtonsoft.Converters;

namespace Skybrud.Umbraco.Redirects.Import.Models;

[JsonConverter(typeof(ItemListJsonConverter))]
public class ItemList : List<Item> {

    public ItemList() { }

    public ItemList(IEnumerable<Item> source) : base(source) { }

}