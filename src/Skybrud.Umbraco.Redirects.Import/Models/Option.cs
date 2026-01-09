using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace Skybrud.Umbraco.Redirects.Import.Models;

/// <summary>
/// Class representing a configurable option presented to the user.
/// </summary>
public class Option {

    /// <summary>
    /// Gets or sets the alias of the option.
    /// </summary>
    [JsonProperty("alias")]
    public required string Alias { get; set; }

    /// <summary>
    /// Gets or sets the label of the option.
    /// </summary>
    [JsonProperty("label")]
    public required string Label { get; set; }

    /// <summary>
    /// Gets or sets the description of the option.
    /// </summary>
    [JsonProperty("description")]
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the name of the associated HTML element.
    /// </summary>
    [JsonProperty("element")]
    public required string Element { get; set; }

    /// <summary>
    /// Gets or sets the value of the option.
    /// </summary>
    [JsonProperty("value")]
    public object? Value { get; set; }

    /// <summary>
    /// Gets or sets the configuration of the option.
    /// </summary>
    [JsonProperty("config", NullValueHandling = NullValueHandling.Ignore)]
    public object? Config { get; set; }

    /// <summary>
    /// Gets or sets an object describing the validation of the option.
    /// </summary>
    [JsonProperty("validation", NullValueHandling = NullValueHandling.Ignore)]
    public OptionValidation? Validation { get; set; }

    #region Constructors

    /// <summary>
    /// Initializes a new instance. Notice that the required members must be set manually after using this constructor.
    /// </summary>
    public Option() { }

    /// <summary>
    /// Initializes a new instance based on the specified <paramref name="alias"/>, <paramref name="label"/> and <paramref name="element"/>.
    /// </summary>
    /// <param name="alias">The alias of the option.</param>
    /// <param name="label">The name of the option.</param>
    /// <param name="element">The name of the associated HTML element.</param>
    [SetsRequiredMembers]
    public Option(string alias, string label, string element) {
        Alias = alias;
        Label = label;
        Element = element;
    }

    /// <summary>
    /// Initializes a new instance based on the specified <paramref name="alias"/>, <paramref name="label"/>, <paramref name="element"/> and <paramref name="description"/>.
    /// </summary>
    /// <param name="alias">The alias of the option.</param>
    /// <param name="label">The name of the option.</param>
    /// <param name="element">The name of the associated HTML element.</param>
    /// <param name="description">The description of the option.</param>
    [SetsRequiredMembers]
    public Option(string alias, string label, string element, string? description) {
        Alias = alias;
        Label = label;
        Element = element;
        Description = description;
    }

    #endregion

    #region Static methods

    internal static Option File(string? alias = null, string? label = null, string? description = null, bool multiple = false) {
        return new Option {
            Alias = alias ?? "file",
            Label = label ?? "File",
            Element = "skybrud-redirects-import-file",
            Description = description,
            Config = new Dictionary<string, object> {
                {"multiple", multiple}
            }
        };
    }

    internal static Option Overwrite(string? alias = null, string? label = null, string? description = null) {
        return new Option {
            Alias = alias ?? "overwriteExisting",
            Label = label ?? "Overwrite existing",
            Element = "skybrud-redirects-import-boolean",
            Description = description ?? "Indicates whether existing redirects should be overwritten for matching inbound URLs.",
        };
    }

    #endregion

}