using Newtonsoft.Json;
using Skybrud.Umbraco.Redirects.Exceptions;
using Skybrud.Umbraco.Redirects.Import.Models.Import;
using System;
using System.Collections.Generic;

namespace Skybrud.Umbraco.Redirects.Import.Importers.Csv {

    /// <summary>
    /// Class representing the result of an import of redirects from a <strong>CSV</strong> file.
    /// </summary>
    public class CsvImportResult : IImportResult {

        #region Properties

        /// <summary>
        /// Gets whether the import was successful.
        /// </summary>
        public bool IsSuccessful { get; }

        /// <summary>
        /// Gets a list of errors triggered by the import.
        /// </summary>
        public IReadOnlyList<string> Errors { get; }

        /// <summary>
        /// Gets a list of the imported redirects.
        /// </summary>
        public IReadOnlyList<RedirectImportItem> Redirects { get; }

        /// <summary>
        /// Gets a reference to an <see cref="Exception"/> if the import failed at a global level.
        /// </summary>
        [JsonIgnore]
        public Exception? Exception { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance from the specified <paramref name="result"/>.
        /// </summary>
        /// <param name="result">The import result to wrap.</param>
        public CsvImportResult(ImportResult result) {
            IsSuccessful = result.IsSuccessful;
            Errors = result.Errors;
            Redirects = result.Redirects;
            Exception = result.Exception;
        }

        private CsvImportResult(IReadOnlyList<RedirectImportItem> redirects) {
            IsSuccessful = true;
            Errors = Array.Empty<string>();
            Redirects = redirects;
        }

        private CsvImportResult(IReadOnlyList<string> errors) {
            IsSuccessful = false;
            Errors = errors;
            Redirects = Array.Empty<RedirectImportItem>();
        }

        private CsvImportResult(Exception exception) {
            IsSuccessful = false;
            Exception = exception;
            Errors = new[] {
                exception is RedirectsException rex ? rex.Message : "Import failed on the server."
            };
            Redirects = Array.Empty<RedirectImportItem>();
        }

        #endregion

        #region Static methods

        /// <summary>
        /// Initializes a new, failed import result based on the specified <paramref name="exception"/>.
        /// </summary>
        /// <param name="exception">The exception triggered by the import.</param>
        /// <returns>An instance of <see cref="ImportResult"/> representing the import result.</returns>
        public static CsvImportResult Failed(Exception exception) {
            return new CsvImportResult(exception);
        }

        /// <summary>
        /// Initializes a new, failed import result based on the specified list of <paramref name="errors"/>.
        /// </summary>
        /// <param name="errors">The list of errors triggered by the import.</param>
        /// <returns>An instance of <see cref="ImportResult"/> representing the import result.</returns>
        public static CsvImportResult Failed(List<string> errors) {
            return new CsvImportResult(errors);
        }

        /// <summary>
        /// Initializes a new, successful import result based on the specified <paramref name="redirects"/>.
        /// </summary>
        /// <param name="redirects">The imported redirects.</param>
        /// <returns>An instance of <see cref="ImportResult"/> representing the import result.</returns>
        public static CsvImportResult Success(List<RedirectImportItem> redirects) {
            return new CsvImportResult(redirects);
        }

        #endregion

    }

}