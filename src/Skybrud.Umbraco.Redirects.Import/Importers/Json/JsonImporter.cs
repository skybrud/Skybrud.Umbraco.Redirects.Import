using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using Skybrud.Essentials.Json.Newtonsoft;
using Skybrud.Essentials.Json.Newtonsoft.Extensions;
using Skybrud.Umbraco.Redirects.Import.Models;

namespace Skybrud.Umbraco.Redirects.Import.Importers.Json; 

/// <summary>
/// Class representing an importer based on an <strong>JSON</strong> file.
/// </summary>
public class JsonImporter : ImporterBase<JsonImportOptions, JsonImportResult> {

    private readonly RedirectsImportService _redirectsImportService;

    #region Constructors

    /// <summary>
    /// Initializes a new instance based on the specified dependencies.
    /// </summary>
    public JsonImporter(RedirectsImportService redirectsImportService) {
        _redirectsImportService = redirectsImportService;
        Icon = "icon-redirects-json";
        Name = "JSON";
        Description = "Lets you import redirects from a JSON file.";
    }

    #endregion

    #region Member methods

    /// <summary>
    /// Returns a collection with the options for the <strong>Options</strong> step in the import process.
    /// </summary>
    /// <param name="request">A reference to current request.</param>
    /// <returns>A collection of <see cref="Option"/> representing the options.</returns>
    public override IEnumerable<Option> GetOptions(HttpRequest request) {

        const string fileUrl = "/App_Plugins/Skybrud.Umbraco.Redirects.Import/Views/Editors/File.html";

        return new Option[] {
            new ("file", "File", fileUrl, "Select the JSON file.") {
                Config = new Dictionary<string, object> {
                    {"multiple", false}
                }
            }
        };

    }

    /// <summary>
    /// Performs a new import based on the specified <paramref name="options"/>.
    /// </summary>
    /// <param name="options">The options describing the import.</param>
    /// <returns>An instance of <see cref="JsonImportResult"/> representing the result of the import.</returns>
    public override JsonImportResult Import(JsonImportOptions options) {

        if (options == null) throw new ArgumentNullException(nameof(options));

        List<string> errors = new();

        if (options.File == null) {
            errors.Add("No file was uploaded.");
            return JsonImportResult.Failed(errors);
        }

        if (options.File.ContentType != "application/json" || Path.GetExtension(options.File.FileName).ToLowerInvariant() != ".json") {
            errors.Add("Uploaded file doesn't look like a JSON file.");
            return JsonImportResult.Failed(errors);
        }

        // Return if we have encountered any errors this far
        if (errors.Any()) return JsonImportResult.Failed(errors);

        // Create a new stream for the uploaded file
        using Stream stream = options.File.OpenReadStream();

        // Create a new stream reader
        using StreamReader reader = new(stream, Encoding.UTF8);

        // Read the raw file contents
        string contents = reader.ReadToEnd();

        // Try to parse the raw JSON string to a "JObject" instance
        if (!JsonUtils.TryParseJsonObject(contents, out JObject? json)) {
            errors.Add("Failed parsing JSON contents from uploaded file (1).");
            return JsonImportResult.Failed(errors);
        }

        // Validate the JSON file. Exported files should contain both the version for both the main redirects package and
        // the import package. Currently we don't really check the version - just that the value is present
        JObject? versions = json.GetObject("versions");
        string? v1 = versions.GetString("Skybrud.Umbraco.Redirects");
        string? v2 = versions.GetString("Skybrud.Umbraco.Redirects.Import");
        if (string.IsNullOrWhiteSpace(v1) || string.IsNullOrWhiteSpace(v2)) {
            errors.Add("Failed parsing JSON contents from uploaded file (2).");
            return JsonImportResult.Failed(errors);
        }

        DataTable dataTable = new();

        DataColumn keyColumn = dataTable.Columns.Add("Key");
        DataColumn rootNodeColumn = dataTable.Columns.Add("RootNode");
        DataColumn urlColumn = dataTable.Columns.Add("Url");
        DataColumn typeColumn = dataTable.Columns.Add("Type");
        DataColumn forwardColumn = dataTable.Columns.Add("Forward");
        DataColumn destinationKeyColumn = dataTable.Columns.Add("Destination Key");
        DataColumn destinationNameColumn = dataTable.Columns.Add("Destination Name");
        DataColumn destinationUrlColumn = dataTable.Columns.Add("Destination URL");
        DataColumn destinationQueryColumn = dataTable.Columns.Add("Destination Query");
        DataColumn destinationFragmentColumn = dataTable.Columns.Add("Destination Fragment");
        DataColumn destinationTypeColumn = dataTable.Columns.Add("Destination Type");
        DataColumn destinationCultureColumn = dataTable.Columns.Add("Destination Culture");

        foreach (JObject item in json.GetObjectArray("redirects")) {

            DataRow row = dataTable.NewRow();
            dataTable.Rows.Add(row);

            if (!item.TryGetGuid("key", out Guid key)) {
                errors.Add("Redirect doesn't have a 'key' property.");
                return JsonImportResult.Failed(errors);
            }

            if (!item.TryGetGuid("rootKey", out Guid rootKey)) {
                errors.Add("Redirect doesn't have a 'rootKey' property.");
                return JsonImportResult.Failed(errors);
            }

            if (!item.TryGetString("url", out string? url)) {
                errors.Add("Redirect doesn't have a 'url' property.");
                return JsonImportResult.Failed(errors);
            }

            if (!item.TryGetString("type", out string? type)) {
                errors.Add("Redirect doesn't have a 'type' property.");
                return JsonImportResult.Failed(errors);
            }

            if (!item.TryGetString("forward", out string? forward)) {
                errors.Add("Redirect doesn't have a 'forward' property.");
                return JsonImportResult.Failed(errors);
            }

            if (item.GetObject("destination") is not {} destination) {
                errors.Add("Redirect doesn't have a 'destination' property.");
                return JsonImportResult.Failed(errors);
            }

            if (!destination.TryGetGuid("key", out Guid destinationKey)) {
                errors.Add("Redirect doesn't have a 'destination.key' property.");
                return JsonImportResult.Failed(errors);
            }

            destination.TryGetString("name", out string? destinationName);

            if (!destination.TryGetString("url", out string? destinationUrl)) {
                errors.Add("Redirect doesn't have a 'destination.url' property.");
                return JsonImportResult.Failed(errors);
            }

            destination.TryGetString("query", out string? destinationQuery);
            destination.TryGetString("fragment", out string? destinationFragment);

            if (!destination.TryGetString("type", out string? destinationType)) {
                errors.Add("Redirect doesn't have a 'destination.type' property.");
                return JsonImportResult.Failed(errors);
            }

            destination.TryGetString("culture", out string? destinationCulture);

            row[keyColumn] = key;
            row[rootNodeColumn] = rootKey;
            row[urlColumn] = url;
            row[typeColumn] = type;
            row[forwardColumn] = forward;

            row[destinationKeyColumn] = destinationKey;
            row[destinationNameColumn] = destinationName;
            row[destinationUrlColumn] = destinationUrl;
            row[destinationQueryColumn] = destinationQuery;
            row[destinationFragmentColumn] = destinationFragment;
            row[destinationTypeColumn] = destinationType;
            row[destinationCultureColumn] = destinationCulture;

        }

        // Start a new import based on the data table
        var result = _redirectsImportService.Import(options, dataTable);

        // Wrap the result
        return new JsonImportResult(result);

    }

    #endregion

}