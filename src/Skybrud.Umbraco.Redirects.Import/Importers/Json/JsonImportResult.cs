using System;
using Skybrud.Umbraco.Redirects.Import.Models.Import;
using System.Collections.Generic;
using Newtonsoft.Json;
using Skybrud.Umbraco.Redirects.Exceptions;

namespace Skybrud.Umbraco.Redirects.Import.Importers.Json; 

/// <summary>
/// Class representing the result of an import of redirects from a <strong>JSON</strong> file.
/// </summary>
public class JsonImportResult : IImportResult {

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
    /// Initializes a new instance with default options.
    /// </summary>
    public JsonImportResult() {
        IsSuccessful = false;
        Errors = null!;
        Redirects = null!;
    }

    /// <summary>
    /// Initializes a new instance from the specified <paramref name="result"/>.
    /// </summary>
    /// <param name="result">The import result to wrap.</param>
    public JsonImportResult(ImportResult result) {
        IsSuccessful = result.IsSuccessful;
        Errors = result.Errors;
        Redirects = result.Redirects;
        Exception = result.Exception;
    }

    private JsonImportResult(IReadOnlyList<RedirectImportItem> redirects) {
        IsSuccessful = true;
        Errors = Array.Empty<string>();
        Redirects = redirects;
    }

    private JsonImportResult(IReadOnlyList<string> errors) {
        IsSuccessful = false;
        Errors = errors;
        Redirects = Array.Empty<RedirectImportItem>();
    }

    private JsonImportResult(Exception exception) {
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
    /// <returns>An instance of <see cref="JsonImportResult"/> representing the import result.</returns>
    public static JsonImportResult Failed(Exception exception) {
        return new JsonImportResult(exception);
    }

    /// <summary>
    /// Initializes a new, failed import result based on the specified list of <paramref name="errors"/>.
    /// </summary>
    /// <param name="errors">The list of errors triggered by the import.</param>
    /// <returns>An instance of <see cref="JsonImportResult"/> representing the import result.</returns>
    public static JsonImportResult Failed(List<string> errors) {
        return new JsonImportResult(errors);
    }

    /// <summary>
    /// Initializes a new, successful import result based on the specified <paramref name="redirects"/>.
    /// </summary>
    /// <param name="redirects">The imported redirects.</param>
    /// <returns>An instance of <see cref="ImportResult"/> representing the import result.</returns>
    public static JsonImportResult Success(List<RedirectImportItem> redirects) {
        return new JsonImportResult(redirects);
    }

    #endregion

}