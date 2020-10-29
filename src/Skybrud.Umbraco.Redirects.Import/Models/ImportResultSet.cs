namespace Skybrud.Umbraco.Redirects.Import.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Skybrud.Umbraco.Redirects.Import.Utilities;
    using Skybrud.Umbraco.Redirects.Models;


    public class ImportResultSet
    {
        public DefaultImportOptions DefaultOptions { get; set; }
        public string Filename { get; set; }
        public List<RedirectItem> SuccessItems { get; set; }
        public List<ImportErrorItem> ErrorItems { get; set; }
        public bool HasErrors { get; set; }
        public List<string> ErrorMessages { get; set; }

        public ImportResultSet()
        {
            SuccessItems = new List<RedirectItem>();
            ErrorItems = new List<ImportErrorItem>();
            ErrorMessages = new List<string>();
        }
    }
    
}
