using Skybrud.Umbraco.Redirects.Import.Models.Export;

namespace Skybrud.Umbraco.Redirects.Import.Exporters.Xlsx {

    public class ExcelExportOptions : IExportColumnOptions {

        public ExportColumnList Columns { get; set; }

    }

}