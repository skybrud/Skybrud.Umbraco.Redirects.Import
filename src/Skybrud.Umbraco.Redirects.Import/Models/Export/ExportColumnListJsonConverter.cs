using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Skybrud.Umbraco.Redirects.Import.Models.Export {

    internal class ExportColumnListJsonConverter : JsonConverter {

        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
            if (reader.TokenType != JsonToken.StartArray) return null;
            JArray array = JArray.Load(reader);
            return new ExportColumnList(new List<ExportColumnItem>(array.Select(x => ((JObject) x).ToObject<ExportColumnItem>())));

        }

        public override bool CanConvert(Type objectType) {
            return false;
        }

    }

}