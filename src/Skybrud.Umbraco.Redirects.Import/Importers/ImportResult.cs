using Newtonsoft.Json;
using Skybrud.Umbraco.Redirects.Exceptions;
using Skybrud.Umbraco.Redirects.Import.Models.Import;
using System;
using System.Collections.Generic;

namespace Skybrud.Umbraco.Redirects.Import.Importers {

    /// <summary>
    /// Class representing the result of an import of redirects.
    /// </summary>
    public class ImportResult : IImportResult {

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

        private ImportResult(IReadOnlyList<RedirectImportItem> redirects) {
            IsSuccessful = true;
            Redirects = redirects;
            Errors = Array.Empty<string>();
        }

        private ImportResult(IReadOnlyList<string> errors) {
            IsSuccessful = false;
            Errors = errors;
            Redirects = Array.Empty<RedirectImportItem>();
        }

        private ImportResult(Exception exception) {
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
        public static ImportResult Failed(Exception exception) {
            return new ImportResult(exception);
        }

        /// <summary>
        /// Initializes a new, failed import result based on the specified list of <paramref name="errors"/>.
        /// </summary>
        /// <param name="errors">The list of errors triggered by the import.</param>
        /// <returns>An instance of <see cref="ImportResult"/> representing the import result.</returns>
        public static ImportResult Failed(IReadOnlyList<string> errors) {
            return new ImportResult(errors);
        }

        /// <summary>
        /// Initializes a new, successful import result based on the specified <paramref name="redirects"/>.
        /// </summary>
        /// <param name="redirects">The imported redirects.</param>
        /// <returns>An instance of <see cref="ImportResult"/> representing the import result.</returns>
        public static ImportResult Success(IReadOnlyList<RedirectImportItem> redirects) {
            return new ImportResult(redirects);
        }

        #endregion

    }

}