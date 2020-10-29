using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Skybrud.Umbraco.Redirects.Import.Models {
   
    public interface IRedirectsImportExportProvider
    {

        #region Properties

        /// <summary>
        /// Gets the name of the provider.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the description of the provider.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Gets whether this provider supports importing redirects.
        /// </summary>
        bool CanImport { get; }

        /// <summary>
        /// Gets whether this provider supports exporting redirects.
        /// </summary>
        bool CanExport { get; }

        #endregion

        #region Methods

        IImportOptions ParseImportSettings(JObject obj);

        IImportOptions ParseImportSettings(Dictionary<string, string> dictionary);

        ImportDataSet Import(RedirectsProviderFile file, IImportOptions options);

        #endregion

    }

}