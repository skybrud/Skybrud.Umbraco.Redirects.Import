using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;
using Skybrud.Csv;

namespace Skybrud.Umbraco.Redirects.Import.Csv {

    public static class CsvExtensions {

        // TODO: Move to CSV package

        internal static CsvCell AddCell(this CsvRow row, object value) {
            return row.AddCell(string.Format(CultureInfo.InvariantCulture, "{0}", value));
        }

        internal static CsvFile ToCsv<T>(this IEnumerable<T> items, CsvSeparator separator, Encoding encoding) where T : class {

            CsvFile csv = new CsvFile(separator, encoding);

            PropertyInfo[] properties = typeof(T).GetProperties();

            foreach (PropertyInfo property in properties) {
                csv.AddColumn(property.Name);
            }

            foreach (T item in items) {
                CsvRow row = csv.AddRow();
                foreach (PropertyInfo property in properties) {
                    row.AddCell(property.GetValue(item));
                }
            }

            return csv;

        }

    }

}