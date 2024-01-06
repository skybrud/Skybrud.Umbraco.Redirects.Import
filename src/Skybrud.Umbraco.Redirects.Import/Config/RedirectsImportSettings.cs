namespace Skybrud.Umbraco.Redirects.Import.Config {

    /// <summary>
    /// Class with settings for the <strong>Skybrud Redirects Import</strong> package.
    /// </summary>
    public class RedirectsImportSettings {

        /// <summary>
        /// Gets the settings for the CSV importer.
        /// </summary>
        public RedirectsCsvSettings Csv = new();

    }
}
