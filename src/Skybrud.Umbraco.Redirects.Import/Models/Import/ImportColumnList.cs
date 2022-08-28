using System.Data;

namespace Skybrud.Umbraco.Redirects.Import.Models.Import {

    public class ImportColumnList {

        public DataColumn RootNode { get; set; }

        public DataColumn InboundUrl { get; set; }

        public DataColumn InboundQuery { get; set; }

        public DataColumn DestinationId { get; set; }

        public DataColumn DestinationKey { get; set; }

        public DataColumn DestinationType { get; set; }

        public DataColumn DestinationUrl { get; set; }

        public DataColumn DestinationQuery { get; set; }

        public DataColumn DestinationFragment { get; set; }

        public DataColumn RedirectType { get; set; }

        public DataColumn ForwardQueryString { get; set; }

    }

}