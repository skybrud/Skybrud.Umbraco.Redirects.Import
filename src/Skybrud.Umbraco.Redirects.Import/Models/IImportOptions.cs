namespace Skybrud.Umbraco.Redirects.Import.Models {
    
    /// <summary>
    /// Interface describing common options for importing one or more redirects.
    /// </summary>
    public interface IImportOptions {
        
        /// <summary>
        /// Gets whether existing redirects should be overwritten if the inbound URL is the same.
        /// </summary>
        bool OverwriteExisting { get; }

    }

}