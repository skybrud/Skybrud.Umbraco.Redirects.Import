using Skybrud.Umbraco.Redirects.Import.Models.Export;

namespace Skybrud.Umbraco.Redirects.Import.Exporters.Xlsx;

/// <summary>
/// Class representing the options for exporting redirects to an <strong>XLSX</strong> file.
/// </summary>
public class XlsxExportOptions : IExportColumnOptions {

    /// <summary>
    /// Gets or sets the columns that should be included in the exported XLSX file.
    /// </summary>
    public ExportColumnList? Columns { get; set; }

}