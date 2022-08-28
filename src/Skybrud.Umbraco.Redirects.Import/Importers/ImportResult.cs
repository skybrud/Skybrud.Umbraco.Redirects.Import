using Newtonsoft.Json;
using Skybrud.Umbraco.Redirects.Exceptions;
using Skybrud.Umbraco.Redirects.Import.Models.Import;
using System;
using System.Collections.Generic;

namespace Skybrud.Umbraco.Redirects.Import.Importers {

    public class ImportResult : IImportResult {

        #region Properties

        public bool IsSuccessful { get; }

        public IReadOnlyList<string> Errors { get; }

        public IReadOnlyList<RedirectImportItem> Redirects { get; }

        [JsonIgnore]
        public Exception Exception { get; }

        #endregion

        #region Constructors

        private ImportResult(List<RedirectImportItem> redirects) {
            IsSuccessful = true;
            Redirects = redirects;
        }

        private ImportResult(List<string> errors) {
            IsSuccessful = false;
            Errors = errors;
        }

        private ImportResult(Exception exception) {
            IsSuccessful = false;
            Exception = exception;
            Errors = new[] {
                exception is RedirectsException rex ? rex.Message : "Import failed on the server."
            };
        }

        #endregion

        #region Static methods

        public static ImportResult Failed(Exception exception) {
            return new ImportResult(exception);
        }

        public static ImportResult Failed(List<string> errors) {
            return new ImportResult(errors);
        }

        public static ImportResult Success(List<RedirectImportItem> redirects) {
            return new ImportResult(redirects);
        }

        #endregion

    }

}