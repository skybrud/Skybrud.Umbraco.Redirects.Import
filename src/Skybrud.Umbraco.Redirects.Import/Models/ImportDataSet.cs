namespace Skybrud.Umbraco.Redirects.Import.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Skybrud.Umbraco.Redirects.Models;
    using Skybrud.Umbraco.Redirects.Models.Options;

    public class ImportDataSet
    {
        public IEnumerable<string> ImportErrors { get; set; }
        public IEnumerable<ImportDataItem> Items { get; set; }

        public ImportDataSet()
        {
            Items = new List<ImportDataItem>();
            ImportErrors = new List<string>();
        }
    }

    public class ImportDataItem
    {
        //TODO: Update with all possible fields
        
        //Required
        public string OldUrl { get; set; }

        //Required
        public string NewUrl { get; set; }

        public string Domain { get; set; }
        public int RootNodeId { get; set; }
        public Guid RootNodeKey { get; set; }
        public int DestinationId { get; set; }
        public RedirectDestinationType DestinationType { get; set; }
        public bool DataInvalidForImport { get; set; }
        public IEnumerable<string> Messages { get; set; }
      
    }


}
