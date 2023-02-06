using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Skybrud.Umbraco.Redirects.Import.Models;

namespace Skybrud.Umbraco.Redirects.Import.Exporters {

    /// <summary>
    /// Interface describing an exporter.
    /// </summary>
    public interface IExporter {

        #region Properties

        /// <summary>
        /// Gets the type of the exporter.
        /// </summary>
        [JsonProperty("type", Order = -99)]
        public string Type => GetType().AssemblyQualifiedName;

        /// <summary>
        /// Gets the icon of the exporter.
        /// </summary>
        [JsonProperty("icon", Order = -100)]
        string Icon { get; }

        /// <summary>
        /// Gets the name of the exporter.
        /// </summary>
        [JsonProperty("name", Order = -98)]
        string Name { get; }

        /// <summary>
        /// Gets the description of the exporter.
        /// </summary>
        [JsonProperty("description", Order = -97)]
        string Description { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Returns a collection with the options for the <strong>Options</strong> step in the export process.
        /// </summary>
        /// <param name="request">A reference to the current request.</param>
        /// <returns>A collection of <see cref="Option"/> representing the options.</returns>
        IEnumerable<Option> GetOptions(HttpRequest request);

        /// <summary>
        /// Parses the specified JSON <paramref name="config"/> object into an instance of <see cref="IExportOptions"/>.
        /// </summary>
        /// <param name="config">The <see cref="JObject"/> representing the configuration/options.</param>
        /// <returns>An instance of <see cref="IExportOptions"/>.</returns>
        IExportOptions ParseOptions(JObject config);

        /// <summary>
        /// Performs a new export based on the specified <paramref name="options"/>.
        /// </summary>
        /// <param name="options">The options describing the export.</param>
        /// <returns>An instance of <see cref="IExportResult"/> representing the result of the export.</returns>
        IExportResult Export(IExportOptions options);

        #endregion

    }

    /// <summary>
    /// Interface describing a generic exporter.
    /// </summary>
    public interface IExporter<TOptions, out TResult> : IExporter where TOptions : IExportOptions where TResult : IExportResult {

        /// <summary>
        /// Parses the specified JSON <paramref name="config"/> object into an instance of <typeparamref name="TOptions"/>.
        /// </summary>
        /// <param name="config">The <see cref="JObject"/> representing the configuration/options.</param>
        /// <returns>An instance of <typeparamref name="TOptions"/>.</returns>
        new TOptions ParseOptions(JObject config);

        /// <summary>
        /// Performs a new import based on the specified <paramref name="options"/>.
        /// </summary>
        /// <param name="options">The options describing the export.</param>
        /// <returns>An instance of <typeparamref name="TResult"/>.</returns>
        TResult Export(TOptions options);

    }

}