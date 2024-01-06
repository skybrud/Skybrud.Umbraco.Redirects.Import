using System.Collections.Generic;
using System.Reflection;
using Umbraco.Cms.Core.Manifest;

namespace Skybrud.Umbraco.Redirects.Import;

/// <inheritdoc />
public class RedirectsImportManifestFilter : IManifestFilter {

    /// <inheritdoc />
    public void Filter(List<PackageManifest> manifests) {

        // Initialize a new manifest filter for this package
        PackageManifest manifest = new() {
            AllowPackageTelemetry = true,
            PackageName = RedirectsImportPackage.Name,
            Version = RedirectsImportPackage.InformationalVersion.Split('+')[0],
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
        };

        // The "PackageId" property isn't available prior to Umbraco 12, and since the package is build against
        // Umbraco 10, we need to use reflection for setting the property value for Umbraco 12+. Ideally this
        // shouldn't fail, but we might at least add a try/catch to be sure
        try {
            PropertyInfo? property = manifest.GetType().GetProperty("PackageId");
            property?.SetValue(manifest, RedirectsPackage.Alias);
        } catch {
            // We don't really care about the exception
        }

        // Append the manifest
        manifests.Add(manifest);

    }

}