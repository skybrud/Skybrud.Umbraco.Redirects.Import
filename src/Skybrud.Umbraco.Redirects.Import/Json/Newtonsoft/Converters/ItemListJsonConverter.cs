using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Skybrud.Umbraco.Redirects.Import.Models;

namespace Skybrud.Umbraco.Redirects.Import.Json.Newtonsoft.Converters;

internal class ItemListJsonConverter : JsonConverter {

    public override bool CanConvert(Type objectType) {
        return false;
    }

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer) {
        throw new NotImplementedException();
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer) {

        if (value is not ItemList list) {
            writer.WriteNull();
            return;
        }

        JArray array = [];

        foreach (Item item in list) {
            array.Add(JObject.FromObject(item, serializer));
        }

        array.WriteTo(writer);

    }

}