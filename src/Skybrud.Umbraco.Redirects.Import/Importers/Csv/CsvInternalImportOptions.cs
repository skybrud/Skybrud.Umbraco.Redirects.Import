using Skybrud.Csv;

namespace Skybrud.Umbraco.Redirects.Import.Importers.Csv {

    internal class CsvInternalImportOptions {

        public CsvFile File { get; }

        public CsvImportOptions Options { get; }

        public CsvColumn ColumnRootNode { get; set; }

        public CsvColumn ColumnInboundUrl { get; set; }

        public CsvColumn ColumnInboundQuery { get; set; }

        public CsvColumn ColumnDestinationId { get; set; }

        public CsvColumn ColumnDestinationKey { get; set; }

        public CsvColumn ColumnDestinationType { get; set; }

        public CsvColumn ColumnDestinationUrl { get; set; }

        public CsvColumn ColumnDestinationQuery { get; set; }

        public CsvColumn ColumnDestinationFragment { get; set; }

        public CsvInternalImportOptions(CsvFile file, CsvImportOptions options) {
            File = file;
            Options = options;
        }

    }

}