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
        _columns = [
            new ExportColumnItem("Id", true),
            new ExportColumnItem("Key", true),
            new ExportColumnItem("RootKey", true),
            new ExportColumnItem("Url", true),
            new ExportColumnItem("QueryString", true),
            new ExportColumnItem("DestinationType", true),
            new ExportColumnItem("DestinationId", false),
            new ExportColumnItem("DestinationKey", true),
            new ExportColumnItem("DestinationUrl", true),
            new ExportColumnItem("DestinationQuery", true),
            new ExportColumnItem("DestinationFragment", true),
            new ExportColumnItem("DestinationName", true),
            new ExportColumnItem("Type", true),
            new ExportColumnItem("IsPermanent", false),
            new ExportColumnItem("ForwardQueryString", true),
            new ExportColumnItem("CreateDate", true),
            new ExportColumnItem("UpdateDate", true)
        ];
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