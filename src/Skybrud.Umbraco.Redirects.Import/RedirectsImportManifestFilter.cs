using System.Collections.Generic;
using Skybrud.Essentials.Strings.Extensions;
using Umbraco.Cms.Core.Manifest;

namespace Skybrud.Umbraco.Redirects.Import {

    /// <inheritdoc />
    public class RedirectsImportManifestFilter : IManifestFilter {

        /// <inheritdoc />
        public void Filter(List<PackageManifest> manifests) {
            manifests.Add(new PackageManifest {
                PackageName = RedirectsImportPackage.Alias.ToKebabCase(),
                BundleOptions = BundleOptions.Independent,
                Scripts = new[] {
                    $"/App_Plugins/{RedirectsImportPackage.Alias}/Scripts/App.js",
                    $"/App_Plugins/{RedirectsImportPackage.Alias}/Scripts/Controllers/Export.js",
                    $"/App_Plugins/{RedirectsImportPackage.Alias}/Scripts/Controllers/Import.js",
                    $"/App_Plugins/{RedirectsImportPackage.Alias}/Scripts/Controllers/File.js",
                    $"/App_Plugins/{RedirectsImportPackage.Alias}/Scripts/Controllers/Items.js",
                    $"/App_Plugins/{RedirectsImportPackage.Alias}/Scripts/Controllers/Columns.js"
                },
                Stylesheets = new[] {
                    $"/App_Plugins/{RedirectsImportPackage.Alias}/Styles/Default.css"
                }
            });
        }

    }

}