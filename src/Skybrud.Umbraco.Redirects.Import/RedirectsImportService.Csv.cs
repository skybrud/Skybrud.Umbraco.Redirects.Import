using Skybrud.Csv;
using System.Data;

namespace Skybrud.Umbraco.Redirects.Import; 

public partial class RedirectsImportService {

    /// <summary>
    /// Converts the specified CSV <paramref name="file"/> into a corresponding <see cref="DataTable"/>.
    /// </summary>
    /// <param name="file">The CSV file to be converted.</param>
    /// <returns>An instance of <see cref="DataTable"/>.</returns>
    public virtual DataTable ToDataTable(CsvFile file) {
        return file.ToDataTable();
    }

}