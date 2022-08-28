using System;
using Newtonsoft.Json;
using Skybrud.Csv;

namespace Skybrud.Umbraco.Redirects.Import.Exporters.Csv {

    public class CsvExportResult : IExportResult {

        #region Properties

        public Guid Key { get; }

        public string ContentType => RedirectsImportConstants.ContentTypes.Csv;

        public string FileName { get; } = $"Redirects_{DateTime.UtcNow:yyyyMMddHHmmss}.csv";

        [JsonIgnore]
        public CsvFile File { get; }

        #endregion

        #region Constructors

        public CsvExportResult(Guid key, CsvFile file) {
            Key = key;
            File = file;
        }

        #endregion

        #region Member methods

        public byte[] GetBytes(IExportOptions options) {
            return File.Encoding.GetBytes(File.ToString(File.Separator));
        }

        #endregion

    }

}