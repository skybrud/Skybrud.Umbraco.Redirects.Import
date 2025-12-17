using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using Skybrud.Essentials.Reflection;
using Skybrud.Umbraco.Redirects.Import.Models;
using Skybrud.Umbraco.Redirects.Import.Services;
using Skybrud.Umbraco.Redirects.Services;

namespace Skybrud.Umbraco.Redirects.Import.Exporters.Json;

/// <summary>
/// Class serving as an exporter for exporting redirects to a <strong>JSON</strong> file.
/// </summary>
public class JsonExporter : ExporterBase<JsonExportOptions, JsonExportResult> {

    private readonly RedirectsImportService _redirectsImportService;

    #region Constructors

    /// <summary>
    /// Initializes a new instance based on the specified dependencies.
    /// </summary>
    public JsonExporter(RedirectsImportService redirectsImportService) {
        _redirectsImportService = redirectsImportService;
        Icon = "redirects-json";
        Name = "JSON";
        Description = "Lets you export redirects to a JSON.";
    }

    #endregion

    #region Member methods

    /// <summary>
    /// Returns a collection of <see cref="Option"/> representing the configurable options for the exporter.
    /// </summary>
    /// <param name="request">A reference to the current request.</param>
    /// <returns>A collection of <see cref="Option"/></returns>
    public override IEnumerable<Option> GetOptions(HttpRequest request) {

        ItemList indentations = [
            new Item("None", "None"),
            new Item("Indented", "Indented")
        ];

        return [
            new Option() {
                Alias = "formatting",
                Label = "Formatting",
                Description = "Select the formatting of the JSON file.",
                Element = "skybrud-redirects-import-items",
                Value = indentations[0].Alias,
                Config = indentations
            }
        ];

    }

    /// <summary>
    /// Triggers a new export of redirects based on the specified <paramref name="options"/>.
    /// </summary>
    /// <param name="options">The options for the export.</param>
    /// <returns>An instance of <see cref="JsonExportResult"/> representing the result of the export.</returns>
    public override JsonExportResult Export(JsonExportOptions options) {

        ArgumentNullException.ThrowIfNull(options, nameof(options));

        // We include the version numbers for future reference
        JObject versions = new();
        Assembly a1 = typeof(IRedirectsService).Assembly;
        Assembly a2 = typeof(RedirectsImportService).Assembly;
        versions.Add(a1.GetName().Name!, ReflectionUtils.GetInformationalVersion(a1));
        versions.Add(a2.GetName().Name!, ReflectionUtils.GetInformationalVersion(a2));

        // Generate the JSON
        JObject json = new() {
            {"versions", versions},
            {"redirects", JArray.FromObject(_redirectsImportService.GetRedirects(options))}
        };

        return new JsonExportResult(Guid.NewGuid(), json);

    }

    #endregion

}