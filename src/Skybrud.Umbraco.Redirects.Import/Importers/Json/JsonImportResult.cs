using Skybrud.Umbraco.Redirects.Import.Models.Import;
using System.Collections.Generic;

namespace Skybrud.Umbraco.Redirects.Import.Importers.Json {

    public class JsonImportResult : IImportResult {

        #region Properties

        public bool IsSuccessful { get; }

        public IReadOnlyList<string> Errors { get; }

        public IReadOnlyList<RedirectImportItem> Redirects { get; }

        #endregion

    }

}