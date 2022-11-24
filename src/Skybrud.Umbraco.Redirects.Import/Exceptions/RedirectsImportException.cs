using Skybrud.Umbraco.Redirects.Exceptions;

namespace Skybrud.Umbraco.Redirects.Import.Exceptions {

    /// <summary>
    /// Class representing an exception triggered by an import or export.
    /// </summary>
    public class RedirectsImportException : RedirectsException {

        /// <summary>
        /// Initializes a new exception with the specified <paramref name="message"/>.
        /// </summary>
        /// <param name="message">The message of the exception.</param>
        public RedirectsImportException(string message) : base(message) { }

    }

}