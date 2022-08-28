using System.Collections.Generic;
using Skybrud.Umbraco.Redirects.Import.Models;

namespace Skybrud.Umbraco.Redirects.Import {

    internal class RedirectsImportUtils {

        public static Option GetColumnsOption() {
            return new Option {
                Alias = "columns",
                Label = "Columns",
                Description = "Select the columns that should be included in the exported file.",
                View = $"{RedirectsImportPackage.AppPlugins}Views/Editors/Columns.html?v={RedirectsPackage.Version}"
            };
        }

        public static Option GetOverwriteOption() {
            return new Option {
                Alias = "overwriteExisting",
                Label = "Overwrite existing",
                Description = "Indicates whether existing redirects should be overwritten for matching inbound URLs.",
                View = "boolean"
            };
        }

        public static Option GetFileOption(string description = null) {
            return new Option {
                Alias = "file",
                Label = "File",
                Description = description ?? "Select the file containing the redirects.",
                View = $"{RedirectsImportPackage.AppPlugins}Views/Editors/File.html?v={RedirectsPackage.Version}",
                Config = new Dictionary<string, object> {
                    {"multiple", false}
                }
            };
        }

    }

}