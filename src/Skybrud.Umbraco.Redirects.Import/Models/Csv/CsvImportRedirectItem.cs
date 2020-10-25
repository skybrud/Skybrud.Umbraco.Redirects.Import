using System.Collections.Generic;
using Skybrud.Umbraco.Redirects.Models.Options;

namespace Skybrud.Umbraco.Redirects.Import.Models.Csv {

    public class CsvImportRedirectItem {

        public AddRedirectOptions AddOptions { get; set; }

        public List<string> Errors = new List<string>();

        public object Hmm { get; set; }

    }

}