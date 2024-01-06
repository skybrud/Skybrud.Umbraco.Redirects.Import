using ClosedXML.Excel;
using System.Data;
using Skybrud.Umbraco.Redirects.Exceptions;
using System.Linq;

namespace Skybrud.Umbraco.Redirects.Import;

public partial class RedirectsImportService {

    /// <summary>
    /// Converts the specified <paramref name="workbook"/> into a corresponding <see cref="DataTable"/>.
    /// </summary>
    /// <param name="workbook">The Excel workbook to be converted.</param>
    /// <returns>An instance of <see cref="DataTable"/>.</returns>
    public virtual DataTable ToDataTable(XLWorkbook workbook) {

        // Get the first worksheet from the workbook
        IXLWorksheet? worksheet = workbook.Worksheets.FirstOrDefault();
        if (worksheet == null) throw new RedirectsException("Workbook does not contain any worksheets.");

        // Initialize a new data table
        DataTable table = new(worksheet.Name);

        // Iterate through the rows of the worksheet
        foreach (IXLRow row in worksheet.Rows()) {

            // Add columns based on the first row
            if (row.RowNumber() == 1) {
                foreach (IXLCell cell in row.Cells()) {
                    table.Columns.Add(cell.Value.ToString());
                }
                continue;
            }

            // Get the first and last column of the row (might be null)
            IXLCell? firstCell = row.FirstCellUsed();
            IXLCell? lastCell = row.LastCellUsed();
            if (firstCell is null || lastCell is null) continue;

            // Add a new row to the data table
            table.Rows.Add();

            // Iterate through the columns of the row
            foreach (IXLCell cell in row.Cells(firstCell.Address.ColumnNumber, lastCell.Address.ColumnNumber)) {
                table.Rows[^1][cell.WorksheetColumn().ColumnNumber() - 1] = cell.Value.ToString();
            }

        }

        return table;

    }

}