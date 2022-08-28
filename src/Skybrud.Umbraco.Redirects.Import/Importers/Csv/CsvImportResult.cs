using Newtonsoft.Json;
using Skybrud.Umbraco.Redirects.Exceptions;
using Skybrud.Umbraco.Redirects.Import.Models.Import;
using System;
using System.Collections.Generic;

namespace Skybrud.Umbraco.Redirects.Import.Importers.Csv {

    public class CsvImportResult : IImportResult {

        #region Properties

        public bool IsSuccessful { get; }

        public IReadOnlyList<string> Errors { get; }

        public IReadOnlyList<RedirectImportItem> Redirects { get; }

        [JsonIgnore]
        public Exception Exception { get; }

        #endregion

        #region Constructors

        public CsvImportResult(ImportResult result) {
            IsSuccessful = result.IsSuccessful;
            Errors = result.Errors;
            Redirects = result.Redirects;
            Exception = result.Exception;
        }

        private CsvImportResult(List<RedirectImportItem> redirects) {
            IsSuccessful = true;
            Redirects = redirects;
        }

        private CsvImportResult(List<string> errors) {
            IsSuccessful = false;
            Errors = errors;
        }

        private CsvImportResult(Exception exception) {
            IsSuccessful = false;
            Exception = exception;
            Errors = new[] {
                exception is RedirectsException rex ? rex.Message : "Import failed on the server."
            };
        }

        #endregion

        #region Static methods

        public static CsvImportResult Failed(Exception exception) {
            return new CsvImportResult(exception);
        }

        public static CsvImportResult Failed(List<string> errors) {
            return new CsvImportResult(errors);
        }

        public static CsvImportResult Success(CsvImportOptions options, List<RedirectImportItem> redirects) {
            return new CsvImportResult(redirects);
        }

        #endregion

    }

}