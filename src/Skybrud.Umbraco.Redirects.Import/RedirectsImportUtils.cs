using System.Collections.Generic;
using Skybrud.Umbraco.Redirects.Import.Models;

namespace Skybrud.Umbraco.Redirects.Import;

internal class RedirectsImportUtils {

    public static Option GetColumnsOption() {
        string view = $"{RedirectsImportPackage.AppPlugins}Views/Editors/Columns.html?v={RedirectsPackage.Version}";
        return new Option("columns", "Columns", view, "Select the columns that should be included in the exported file.");
    }

    public static Option GetOverwriteOption() {
        return new Option("overwriteExisting", "Overwrite existing", "boolean", "Indicates whether existing redirects should be overwritten for matching inbound URLs.");
    }

    public static Option GetFileOption(string? description = null) {

        string view = $"{RedirectsImportPackage.AppPlugins}Views/Editors/File.html?v={RedirectsPackage.Version}";

        return new Option("file", "File", view, description ?? "Select the file containing the redirects.") {
            Config = new Dictionary<string, object> {
                {"multiple", false}
            }
        };

    }

}