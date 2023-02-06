using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Skybrud.Umbraco.Redirects.Import.Models.Export {

    /// <summary>
    /// Class representing a list of columns to be exported.
    /// </summary>
    [JsonConverter(typeof(ExportColumnListJsonConverter))]
    public class ExportColumnList : IEnumerable<ExportColumnItem> {

        private readonly IReadOnlyList<ExportColumnItem> _columns;

        /// <summary>
        /// Initializes a new instanced with default options.
        /// </summary>
        public ExportColumnList() {
            _columns = new List<ExportColumnItem> {
                new() { Alias = "Id", IsSelected = true },
                new() { Alias = "Key", IsSelected = true },
                new() { Alias = "RootKey", IsSelected = true },
                new() { Alias = "Url", IsSelected = true },
                new() { Alias = "QueryString", IsSelected = true },
                new() { Alias = "DestinationType", IsSelected = true },
                new() { Alias = "DestinationId", IsSelected = true },
                new() { Alias = "DestionationUrl", IsSelected = true },
                new() { Alias = "DestionationQuery", IsSelected = true },
                new() { Alias = "DestionationFragment", IsSelected = true },
                new() { Alias = "DestionationName", IsSelected = true },
                new() { Alias = "Type", IsSelected = true },
                new() { Alias = "IsPermanent", IsSelected = false },
                new() { Alias = "ForwardQueryString", IsSelected = true },
                new() { Alias = "CreateDate", IsSelected = true },
                new() { Alias = "UpdateDate", IsSelected = true }
            };
        }

        /// <summary>
        /// Initializes a new instance based on the specified <paramref name="columns"/>.
        /// </summary>
        /// <param name="columns">A list of columns the new instance should be based on.</param>
        public ExportColumnList(IReadOnlyList<ExportColumnItem> columns) {
            _columns = columns;
        }

        /// <inheritdoc />
        public IEnumerator<ExportColumnItem> GetEnumerator() {
            return _columns.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

    }

}