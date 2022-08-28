using System;

namespace Skybrud.Umbraco.Redirects.Import.Exporters.Xlsx {

    public class XlsxExportResult : IExportResult {

        private readonly byte[] _bytes;

        #region Properties

        public Guid Key { get; }

        public string ContentType { get; }

        public string FileName { get; }

        #endregion

        public XlsxExportResult(Guid key, byte[] bytes) {
            Key = key;
            _bytes = bytes;
            ContentType = RedirectsImportConstants.ContentTypes.Xlsx;
            FileName = $"Redirects_{DateTime.UtcNow:yyyyMMddHHmmss}.xlsx";
        }

        public byte[] GetBytes(IExportOptions options) {
            return _bytes;
        }

    }

}