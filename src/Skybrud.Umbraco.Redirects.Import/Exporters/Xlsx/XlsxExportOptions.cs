using Skybrud.Umbraco.Redirects.Import.Models.Export;

namespace Skybrud.Umbraco.Redirects.Import.Exporters.Xlsx {

    public class XlsxExportOptions : IExportColumnOptions {

        public ExportColumnList Columns { get; set; }

    }

}