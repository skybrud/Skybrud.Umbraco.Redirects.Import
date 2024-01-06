using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Skybrud.Umbraco.Redirects.Import.Models.Export;

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
            new("Id", true),
            new("Key", true),
            new("RootKey", true),
            new("Url", true),
            new("QueryString", true),
            new("DestinationType", true),
            new("DestinationId", true),
            new("DestionationUrl", true),
            new("DestionationQuery", true),
            new("DestionationFragment", true),
            new("DestionationName", true),
            new("Type", true),
            new("IsPermanent", false),
            new("ForwardQueryString", true),
            new("CreateDate", true),
            new("UpdateDate", true)
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