using System.Collections.Generic;
using System.Threading.Tasks;
using Skybrud.Essentials.Security.Extensions;
using Umbraco.Cms.Core.Manifest;
using Umbraco.Cms.Infrastructure.Manifest;

namespace Skybrud.Umbraco.Redirects.Import.Manifests;

public class RedirectsImportManifestReader : IPackageManifestReader {

    public async Task<IEnumerable<PackageManifest>> ReadPackageManifestsAsync() {

        const string alias = RedirectsImportPackage.Alias;
        string cacheBuster = RedirectsImportPackage.InformationalVersion.ToMd5Hash();

        List<PackageManifest> temp = [
            new() {
                Name = RedirectsImportPackage.Name,
                AllowTelemetry = true,
                Version = RedirectsImportPackage.InformationalVersion,
                Extensions = [
                    new {
                        name = "redirects.import.entrypoint",
                        alias = "Skybrud.Umbraco.Redirects.Import.EntryPoint",
                        type = "backofficeEntryPoint",
                        js = $"/App_Plugins/{alias}/EntryPoint.js?v={cacheBuster}"
                    }
                ],
                Importmap = new PackageManifestImportmap {
                    Imports = new Dictionary<string, string> {
                        {"@skybrud-redirects/import/auth", $"/App_Plugins/{alias}/Auth.js?v={cacheBuster}"},
                        {"@skybrud-redirects/import/package", $"/App_Plugins/{alias}/Package.js?{cacheBuster}"},
                        {"@skybrud-redirects/import/service", $"/App_Plugins/{alias}/Service.js?v={cacheBuster}"},
                        {"@skybrud-redirects/import/elements/properties", $"/App_Plugins/{alias}/Elements/Properties/_index.js?v={cacheBuster}"},
                        {"@skybrud-redirects/import/elements/properties/boolean", $"/App_Plugins/{alias}/Elements/Properties/Boolean.js?v={cacheBuster}"},
                        {"@skybrud-redirects/import/elements/properties/columns", $"/App_Plugins/{alias}/Elements/Properties/Columns.js?v={cacheBuster}"},
                        {"@skybrud-redirects/import/elements/properties/file", $"/App_Plugins/{alias}/Elements/Properties/File.js?v={cacheBuster}"},
                        {"@skybrud-redirects/import/elements/properties/item-list", $"/App_Plugins/{alias}/Elements/Properties/ItemList.js?v={cacheBuster}"},
                        {"@skybrud-redirects/import/elements/properties/property", $"/App_Plugins/{alias}/Elements/Properties/Property.js?v={cacheBuster}"}
                    }
                }
            }

        ];

        return await Task.FromResult(temp);

    }

}