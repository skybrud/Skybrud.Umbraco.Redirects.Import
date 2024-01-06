using System.Collections.Generic;

namespace Skybrud.Umbraco.Redirects.Import.Config;

/// <summary>
/// Class with settings for the CSV importer.
/// </summary>
public class RedirectsCsvSettings {

    /// <summary>
    /// Gets or sets the allowed content types for the CSV importer.
    /// </summary>
    public List<string> AllowedContentTypes { get; set; } = new() {
        "text/plain",
        "text/x-csv",
        "application/vnd.ms-excel",
        "application/csv",
        "application/x-csv",
        "text/csv",
        "text/comma-separated-values",
        "text/x-comma-separated-values",
        "text/tab-separated-values"
    };

}