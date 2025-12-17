using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Skybrud.Csv;
using Skybrud.Umbraco.Redirects.Import.Models;
using Skybrud.Umbraco.Redirects.Import.Services;

namespace Skybrud.Umbraco.Redirects.Import.Exporters.Csv;

/// <summary>
/// A CSV specific implementation of the <see cref="ExporterBase{TOptions,TResult}"/> class.
/// </summary>
public class CsvExporter : ExporterBase<CsvExportOptions, CsvExportResult> {

    private readonly RedirectsImportService _redirectsImportService;

    #region Constructors

    /// <summary>
    /// Initializes a new instance based on the specified <paramref name="redirectsImportService"/>.
    /// </summary>
    /// <param name="redirectsImportService">The current instance of <see cref="RedirectsImportService"/>.</param>
    public CsvExporter(RedirectsImportService redirectsImportService) {
        _redirectsImportService = redirectsImportService;
        Icon = "redirects-csv";
        Name = "CSV";
        Description = "Lets you export redirects to a CSV file.";
    }

    #endregion

    #region Member methods

    /// <summary>
    /// Returns a list of options based on the specified <paramref name="request"/>.
    /// </summary>
    /// <param name="request">The request.</param>
    /// <returns>A collection of <see cref="Option"/>.</returns>
    public override IEnumerable<Option> GetOptions(HttpRequest request) {

        ItemList encodings = [
            new Item("ascii", "Ascii"),
            new Item("utf8", "UTF-8"),
            new Item("windows1252", "Windows 1252")
        ];

        ItemList separators = [
            new Item("colon", "Colon"),
            new Item("comma", "Comma"),
            new Item("semicolon", "Semi colon"),
            new Item("space", "Space"),
            new Item("tab", "Tab")
        ];

        return [
            new Option() {
                Alias = "encoding",
                Label = "Encoding",
                Description = "Select the encoding of the CSV file.",
                Element = "skybrud-redirects-import-items",
                Value = "utf8",
                Config = encodings
            },
            new Option() {
                Alias = "separator",
                Label = "Separator",
                Description = "Select the separator to be used in the exported CSV file.",
                Element = "skybrud-redirects-import-items",
                Value = "semicolon",
                Config = separators
            },
            new Option() {
                Alias = "columns",
                Label = "Columns",
                Description = "Select the columns that should be included in the exported file.",
                Element = "skybrud-redirects-export-columns"
            }
        ];

    }

    /// <summary>
    /// Initiates a new export based on the specified <paramref name="options"/>.
    /// </summary>
    /// <param name="options"></param>
    /// <returns>An instance of <see cref="CsvExportResult"/> representing the result of the export.</returns>
    public override CsvExportResult Export(CsvExportOptions options) {

        ArgumentNullException.ThrowIfNull(options, nameof(options));

        CsvFile file = _redirectsImportService.ExportAsCsv(options);

        return new CsvExportResult(Guid.NewGuid(), file);

    }

    #endregion

}