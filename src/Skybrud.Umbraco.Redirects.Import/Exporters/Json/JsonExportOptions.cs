using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Skybrud.Umbraco.Redirects.Import.Exporters.Json;

/// <summary>
/// Class representing the options for exporting redirects to a <strong>JSON</strong> file.
/// </summary>
public class JsonExportOptions : IExportOptions {

    /// <summary>
    /// Gets or sets the formatting to be used when saving the JSON file.
    /// </summary>
    [JsonProperty("formatting")]
    [JsonConverter(typeof(StringEnumConverter))]
    public Formatting Formatting { get; set; }

}