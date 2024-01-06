using Newtonsoft.Json;

namespace Skybrud.Umbraco.Redirects.Import.Models.Export;

/// <summary>
/// Class describing a column in an export.
/// </summary>
public class ExportColumnItem {

    /// <summary>
    /// Gets the alias of the column.
    /// </summary>
    [JsonProperty("alias")]
    public string Alias { get; set; }

    /// <summary>
    /// Gets whether the column has been selected.
    /// </summary>
    [JsonProperty("selected")]
    public bool IsSelected { get; set; }

    /// <summary>
    /// Initializes a new instance based on the specified <paramref name="alias"/>.
    /// </summary>
    /// <param name="alias">The alias of the item.</param>
    /// <param name="selected">Whether the item is selected.</param>
    public ExportColumnItem(string alias, bool selected) {
        Alias = alias;
        IsSelected = selected;
    }

}