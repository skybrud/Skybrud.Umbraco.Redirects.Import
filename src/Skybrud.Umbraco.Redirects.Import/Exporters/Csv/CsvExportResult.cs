using System;
using Newtonsoft.Json;
using Skybrud.Csv;

namespace Skybrud.Umbraco.Redirects.Import.Exporters.Csv;

/// <summary>
/// Class representing the result of a CSV export.
/// </summary>
public class CsvExportResult : IExportResult {

    #region Properties

    /// <summary>
    /// Gets the unique key identifying the result.
    /// </summary>
    public Guid Key { get; }

    /// <summary>
    /// Gets the content type of the exported file. This will always be <see cref="RedirectsImportConstants.ContentTypes.Csv"/>.
    /// </summary>
    public string ContentType => RedirectsImportConstants.ContentTypes.Csv;

    /// <summary>
    /// Gets the filename of the exported CSV file.
    /// </summary>
    public string FileName { get; } = $"Redirects_{DateTime.UtcNow:yyyyMMddHHmmss}.csv";

    /// <summary>
    /// Gets a reference to the generated <see cref="CsvFile"/>.
    /// </summary>
    [JsonIgnore]
    public CsvFile File { get; }

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance based on the specified <paramref name="key"/> and <paramref name="file"/>.
    /// </summary>
    /// <param name="key">The unique key identifying the result.</param>
    /// <param name="file">The <see cref="CsvFile"/> generated for the result.</param>
    public CsvExportResult(Guid key, CsvFile file) {
        Key = key;
        File = file;
    }

    #endregion

    #region Member methods

    /// <summary>
    /// Returns the bytes representing the contents of the CSV file.
    /// </summary>
    /// <param name="options">The options for the export.</param>
    /// <returns>An array of <see cref="byte"/> representing the CSV file contents.</returns>
    public byte[] GetBytes(IExportOptions options) {
        return File.Encoding.GetBytes(File.ToString(File.Separator));
    }

    #endregion

}