using System;
using Skybrud.Umbraco.Redirects.Models;

namespace Skybrud.Umbraco.Redirects.Import.Csv {

    public class CsvRedirect {

        public int Id { get; set; }

        public Guid Key { get; set; }

        public Guid RootNodeKey { get; set; }

        public string Url { get; set; }

        public string QueryString { get; set; }

        public RedirectDestinationType DestinationType { get; set; }

        public Guid DestinationKey { get; set; }

        public string DestinationName { get; set; }

        public string DestinationUrl { get; set; }

        public string DestinationQuery { get; set; }

        public string Type { get; set; }

        public bool ForwardQueryString { get; set; }

        public string CreateDate { get; set; }

        public string UpdateDate { get; set; }

    }

}