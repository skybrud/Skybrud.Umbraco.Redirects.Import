using Newtonsoft.Json;
using Skybrud.Essentials.Json.Converters.Enums;

namespace Skybrud.Umbraco.Redirects.Import.Models {
    
    [JsonConverter(typeof(EnumLowerCaseConverter))]
    public enum RedirectImportStatus {
        Pending,
        Adding,
        Failed,
        Success
    }

}