using Microsoft.AspNetCore.Http;
using Skybrud.Umbraco.Redirects.Models;

namespace Skybrud.Umbraco.Redirects.Import.Importers.Csv {

    public class CsvImportOptions : IImportOptions {

        public bool OverwriteExisting { get; set; }

        public RedirectType DefaultRedirectType { get; set; }

        public IFormFile File { get; set; }

        public CsvImportSeparator Separator { get; set; }

        public CsvImportEncoding Encoding { get; set; }

    }

}