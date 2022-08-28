using System.Data;

namespace Skybrud.Umbraco.Redirects.Import.Extensions {

    public static class ImportExtensions {

        public static string GetString(this DataRow row, DataColumn column) {
            return row == null || column == null ? null : row[column]?.ToString();
        }

    }

}