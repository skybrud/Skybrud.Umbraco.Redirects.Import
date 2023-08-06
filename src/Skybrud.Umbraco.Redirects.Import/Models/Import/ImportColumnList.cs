using System.Data;

#pragma warning disable CS1591

namespace Skybrud.Umbraco.Redirects.Import.Models.Import {

    /// <summary>
    /// Class representing the column mapping of a redriects import.
    /// </summary>
    public class ImportColumnList {

        public DataColumn? RootNode { get; set; }

        public DataColumn? InboundUrl { get; set; }

        public DataColumn? InboundQuery { get; set; }

        public DataColumn? DestinationId { get; set; }

        public DataColumn? DestinationKey { get; set; }

        public DataColumn? DestinationType { get; set; }

        public DataColumn? DestinationUrl { get; set; }

        public DataColumn? DestinationQuery { get; set; }

        public DataColumn? DestinationFragment { get; set; }

        public DataColumn? DestinationCulture { get; set; }

        public DataColumn? RedirectType { get; set; }

        public DataColumn? ForwardQueryString { get; set; }

    }

}