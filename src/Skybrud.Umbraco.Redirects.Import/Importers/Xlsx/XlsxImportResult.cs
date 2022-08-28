using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Skybrud.Umbraco.Redirects.Exceptions;
using Skybrud.Umbraco.Redirects.Import.Models.Import;
using Skybrud.Umbraco.Redirects.Import.Importers.Csv;

namespace Skybrud.Umbraco.Redirects.Import.Importers.Xlsx {

    public class XlsxImportResult : IImportResult {

        #region Properties

        public bool IsSuccessful { get; }

        public IReadOnlyList<string> Errors { get; }

        public IReadOnlyList<RedirectImportItem> Redirects { get; }

        [JsonIgnore]
        public Exception Exception { get; }

        #endregion

        #region Constructors

        public XlsxImportResult(ImportResult result) {
            IsSuccessful = result.IsSuccessful;
            Errors = result.Errors;
            Redirects = result.Redirects;
            Exception = result.Exception;
        }

        private XlsxImportResult(List<RedirectImportItem> redirects) {
            IsSuccessful = true;
            Redirects = redirects;
        }

        private XlsxImportResult(List<string> errors) {
            IsSuccessful = false;
            Errors = errors;
        }

        private XlsxImportResult(Exception exception) {
            IsSuccessful = false;
            Exception = exception;
            Errors = new[] {
                exception is RedirectsException rex ? rex.Message : "Import failed on the server."
            };
        }

        #endregion

        #region Static methods

        public static XlsxImportResult Failed(Exception exception) {
            return new XlsxImportResult(exception);
        }

        public static XlsxImportResult Failed(List<string> errors) {
            return new XlsxImportResult(errors);
        }

        public static XlsxImportResult Success(CsvImportOptions options, List<RedirectImportItem> redirects) {
            return new XlsxImportResult(redirects);
        }

        #endregion

    }

}