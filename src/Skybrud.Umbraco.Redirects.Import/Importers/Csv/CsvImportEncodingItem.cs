using Skybrud.Umbraco.Redirects.Import.Models;

namespace Skybrud.Umbraco.Redirects.Import.Importers.Csv;

/// <summary>
/// Class representing an item describing an encoding.
/// </summary>
public class CsvImportEncodingItem : Item {

    /// <summary>
    /// Initializes a new instance based on the specified <paramref name="alias"/> and <paramref name="name"/>.
    /// </summary>
    /// <param name="alias">The alias of the encoding.</param>
    /// <param name="name">The friendly name of the encoding.</param>
    public CsvImportEncodingItem(string alias, string name) : base(alias, name) { }

}