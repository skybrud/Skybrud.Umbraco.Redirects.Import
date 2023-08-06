using Newtonsoft.Json;
using Skybrud.Essentials.Json.Newtonsoft.Converters.Enums;

#pragma warning disable CS1591

namespace Skybrud.Umbraco.Redirects.Import.Models.Import {

    /// <summary>
    /// Enum class indicating the import status of a single redirect.
    /// </summary>
    [JsonConverter(typeof(EnumPascalCaseConverter))]
    public enum RedirectImportStatus {

        Pending,

        Adding,

        Failed,

        Added,

        Updated,

        AlreadyExists,

        NotModified

    }

}