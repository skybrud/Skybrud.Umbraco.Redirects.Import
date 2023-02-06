using System;
using Newtonsoft.Json;
using Skybrud.Essentials.Strings;

namespace Skybrud.Umbraco.Redirects.Import.Json.Newtonsoft.Converters {

    internal class BooleanJsonConverter : JsonConverter {

        // TODO: COnsider moving to Skybrud.Essentials?

        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {

            switch (reader.TokenType) {

                case JsonToken.Boolean:
                    return (bool) reader.Value!;

                case JsonToken.String:
                    return StringUtils.ParseBoolean((string)reader.Value);

                case JsonToken.Integer:
                    return ((int) reader.Value!) == 1;

                default:
                    throw new Exception($"Unsupported token type: {reader.TokenType}.");

            }

        }

        public override bool CanConvert(Type objectType) {
            return false;
        }

    }

}