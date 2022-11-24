using ClosedXML.Excel;
using System.Data;
using Skybrud.Umbraco.Redirects.Exceptions;
using System.Linq;

namespace Skybrud.Umbraco.Redirects.Import {

    public partial class RedirectsImportService {

        /// <summary>
        /// Converts the specified <paramref name="workbook"/> into a corresponding <see cref="DataTable"/>.
        /// </summary>
        /// <param name="workbook">The Excel workbook to be converted.</param>
        /// <returns>An instance of <see cref="DataTable"/>.</returns>
        public virtual DataTable ToDataTable(XLWorkbook workbook) {

            // Get the first worksheet from the workbook
            IXLWorksheet worksheet = workbook.Worksheets.FirstOrDefault();
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

                // Add a new row to the data table
                table.Rows.Add();

                // Iterate through the columns of the row
                int c = 0;
                foreach (IXLCell cell in row.Cells(row.FirstCellUsed().Address.ColumnNumber, row.LastCellUsed().Address.ColumnNumber)) {
                    table.Rows[^1][cell.WorksheetColumn().ColumnNumber() - 1] = cell.Value?.ToString() ?? string.Empty;
                    c++;
                }

            }

            return table;

        }

    }

}