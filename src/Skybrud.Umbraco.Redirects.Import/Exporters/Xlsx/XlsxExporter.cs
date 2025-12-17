using System;
using System.Collections.Generic;
using System.IO;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Skybrud.Umbraco.Redirects.Import.Models;
using Skybrud.Umbraco.Redirects.Import.Services;

namespace Skybrud.Umbraco.Redirects.Import.Exporters.Xlsx;

/// <summary>
/// Class serving as an exporter for exporting redirects to an <strong>XLSX</strong> file.
/// </summary>
public class XlsxExporter : ExporterBase<XlsxExportOptions, XlsxExportResult> {

    private readonly RedirectsImportService _redirectsImportService;

    #region Constructors

    /// <summary>
    /// Initializes a new instance based on the specified dependencies.
    /// </summary>
    public XlsxExporter(RedirectsImportService redirectsImportService) {
        _redirectsImportService = redirectsImportService;
        Icon = "redirects-xslt";
        Name = "XLSX";
        Description = "Lets you export redirects to an XLSX file.";
    }

    #endregion

    #region Member methods

    /// <summary>
    /// Returns a collection of <see cref="Option"/> representing the configurable options for the exporter.
    /// </summary>
    /// <param name="request">A reference to the current request.</param>
    /// <returns>A collection of <see cref="Option"/></returns>
    public override IEnumerable<Option> GetOptions(HttpRequest request) {
        return [
            new Option() {
                Alias = "columns",
                Label = "Columns",
                Description = "Select the columns that should be included in the exported file.",
                Element = "skybrud-redirects-export-columns"
            }
        ];
    }

    /// <summary>
    /// Triggers a new export of redirects based on the specified <paramref name="options"/>.
    /// </summary>
    /// <param name="options">The options for the export.</param>
    /// <returns>An instance of <see cref="XlsxExportResult"/> representing the result of the export.</returns>
    public override XlsxExportResult Export(XlsxExportOptions options) {

        ArgumentNullException.ThrowIfNull(options, nameof(options));

        byte[] bytes;

        // Initialize a new workbook
        using (XLWorkbook workbook = new()) {

            // Add a new sheet to the workbook based on the data table
            IXLWorksheet worksheet = workbook.Worksheets.Add(_redirectsImportService.ExportAsDataTable(options));

            // Adjust column sizes
            worksheet.Columns().AdjustToContents();

            // Convert the workbook to a byte array
            using (MemoryStream ms = new()) {
                workbook.SaveAs(ms);
                bytes = ms.ToArray();
            }

        }

        // Return the result
        return new XlsxExportResult(Guid.NewGuid(), bytes);

    }

    #endregion

}