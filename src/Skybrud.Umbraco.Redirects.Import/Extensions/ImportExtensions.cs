using System.Data;

namespace Skybrud.Umbraco.Redirects.Import.Extensions;

internal static class ImportExtensions {

    /// <summary>
    /// Gets the string value of the cell identified by the specified <paramref name="row"/> and <paramref name="column"/>.
    /// </summary>
    /// <param name="row">The row.</param>
    /// <param name="column">The column.</param>
    /// <returns>The string value of the cell if successful; otherwise, <see langword="null"/>.</returns>
    public static string? GetString(this DataRow? row, DataColumn? column) {
        return row == null || column == null ? null : row[column]?.ToString();
    }

}