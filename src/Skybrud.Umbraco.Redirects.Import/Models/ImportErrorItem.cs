using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skybrud.Umbraco.Redirects.Import.Models
{
    public class ImportErrorItem
    {
        public ImportErrorItem(ImportDataItem Item, string ErrorText)
        {
            this.ImportItem = Item;
            this.ErrorMessage = ErrorText;
        }

        public ImportDataItem ImportItem { get; set; }
        public string ErrorMessage { get; set; }
    }
}
