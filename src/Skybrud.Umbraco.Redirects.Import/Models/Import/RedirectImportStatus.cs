using Newtonsoft.Json;
using Skybrud.Essentials.Json.Converters.Enums;

namespace Skybrud.Umbraco.Redirects.Import.Models.Import {

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