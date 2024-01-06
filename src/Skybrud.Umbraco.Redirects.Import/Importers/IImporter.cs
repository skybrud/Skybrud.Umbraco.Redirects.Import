using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Skybrud.Umbraco.Redirects.Import.Models;
using System.Collections.Generic;

namespace Skybrud.Umbraco.Redirects.Import.Importers;

/// <summary>
/// Interface describing an importer.
/// </summary>
public interface IImporter {

    #region Properties

    /// <summary>
    /// Gets the type of the importer.
    /// </summary>
    [JsonProperty("type", Order = -99)]
    public string Type => GetType().AssemblyQualifiedName!;

    /// <summary>
    /// Gets the icon of the importer.
    /// </summary>
    [JsonProperty("icon", Order = -100)]
    string Icon { get; }

    /// <summary>
    /// Gets the name of the importer.
    /// </summary>
    [JsonProperty("name", Order = -98)]
    string Name { get; }

    /// <summary>
    /// Gets the description of the importer.
    /// </summary>
    [JsonProperty("description", Order = -97)]
    string? Description { get; }

    #endregion

    #region Member methods

    /// <summary>
    /// Returns a collection with the options for the <strong>Options</strong> step in the import process.
    /// </summary>
    /// <param name="request">A reference to the current request.</param>
    /// <returns>A collection of <see cref="Option"/> representing the options.</returns>
    IEnumerable<Option> GetOptions(HttpRequest request);

    /// <summary>
    /// Parses the specified JSON <paramref name="config"/> object into an instance of <see cref="IImportOptions"/>.
    /// </summary>
    /// <param name="config">The <see cref="JObject"/> representing the configuration/options.</param>
    /// <returns>An instance of <see cref="IImportOptions"/>.</returns>
    IImportOptions ParseOptions(JObject config);

    /// <summary>
    /// Performs a new import based on the specified <paramref name="options"/>.
    /// </summary>
    /// <param name="options">The options describing the import.</param>
    /// <returns>An instance of <see cref="IImportResult"/> representing the result of the import.</returns>
    IImportResult Import(IImportOptions options);

    #endregion

}

/// <summary>
/// Interface describing a generic importer.
/// </summary>
public interface IImporter<TOptions, out TResult> : IImporter where TOptions : IImportOptions where TResult : IImportResult {

    /// <summary>
    /// Parses the specified JSON <paramref name="config"/> object into an instance of <typeparamref name="TOptions"/>.
    /// </summary>
    /// <param name="config">The <see cref="JObject"/> representing the configuration/options.</param>
    /// <returns>An instance of <typeparamref name="TOptions"/>.</returns>
    new TOptions ParseOptions(JObject config);

    /// <summary>
    /// Performs a new import based on the specified <paramref name="options"/>.
    /// </summary>
    /// <param name="options">The options describing the import.</param>
    /// <returns>An instance of <typeparamref name="TResult"/>.</returns>
    TResult Import(TOptions options);

}