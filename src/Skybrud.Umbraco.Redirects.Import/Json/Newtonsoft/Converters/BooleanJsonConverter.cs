using System;
using Newtonsoft.Json;
using Skybrud.Essentials.Strings;

namespace Skybrud.Umbraco.Redirects.Import.Json.Newtonsoft.Converters;

internal class BooleanJsonConverter : JsonConverter {

    // TODO: Consider moving to Skybrud.Essentials?

    public override bool CanWrite => false;

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer) {
        throw new NotImplementedException();
    }

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer) {
        return reader.TokenType switch {
            JsonToken.Boolean => (bool) reader.Value!,
            JsonToken.String => StringUtils.ParseBoolean((string) reader.Value!),
            JsonToken.Integer => ((int) reader.Value!) == 1,
            _ => throw new Exception($"Unsupported token type: {reader.TokenType}."),
        };
    }

    public override bool CanConvert(Type objectType) {
        return false;
    }

}