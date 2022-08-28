using System;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Skybrud.Umbraco.Redirects.Import.Exporters.Json {

    public class JsonExportResult : IExportResult {

        private readonly JObject _json;

        #region Properties

        public Guid Key { get; }

        public string ContentType => RedirectsImportConstants.ContentTypes.Json;

        public string FileName { get; } = $"Redirects_{DateTime.UtcNow:yyyyMMddHHmmss}.json";

        #endregion

        #region Constructors

        public JsonExportResult(Guid key, JObject json) {
            _json = json;
            Key = key;
        }

        #endregion

        #region Member methods

        public byte[] GetBytes(IExportOptions options) {
            return Encoding.UTF8.GetBytes(_json.ToString((options as JsonExportOptions)?.Formatting ?? Formatting.None));
        }

        #endregion

    }

}