using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Skybrud.Csv;
using Skybrud.Essentials.Enums;
using Skybrud.Umbraco.Redirects.Exceptions;
using Skybrud.Umbraco.Redirects.Import.Services;
using Skybrud.Umbraco.Redirects.Models;
using Skybrud.Umbraco.Redirects.Models.Options;
using Umbraco.Core.IO;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.PropertyEditors;
using Umbraco.Web.Composing;

namespace Skybrud.Umbraco.Redirects.Import.Models.Csv {

    public class CsvRedirectsProvider : IRedirectsProvider {

        #region Properties

        /// <summary>
        /// Gets the name of the provider.
        /// </summary>
        [JsonProperty("name")]
        public string Name => "CSV";

        /// <summary>
        /// Gets the description of the provider.
        /// </summary>
        [JsonProperty("description")]
        public string Description => "Lets you import from and export to CSV files.";

        /// <summary>
        /// Gets whether this provider supports importing redirects.
        /// </summary>
        [JsonProperty("canImport")]
        public bool CanImport => true;

        /// <summary>
        /// Gets whether this provider supports exporting redirects.
        /// </summary>
        [JsonProperty("canExport")]
        public bool CanExport => true;

        [JsonProperty("fields")]
        public IEnumerable<ConfigurationField> Fields => new[] {
            new ConfigurationField {
                Key = "overwriteExisting",
                Name = "Overwrite existing",
                Description = "Indidates whether existing redirects should be overwritten for matching inbound URLs.",
                View = "boolean"
            },
            new ConfigurationField {
                Key = "encoding",
                Name = "Encoding",
                Description = "Select the encoding of the uploaded CSV file.",
                View = "/App_Plugins/Skybrud.Umbraco.Redirects.Import/Views/Editors/Items.html",
                Config = new Dictionary<string, object> {
                    {"items", new [] {
                        new Item { Alias = "auto", Name = "Auto" },
                        new Item { Alias = "ascii", Name = "Ascii" },
                        new Item { Alias = "utf8", Name = "UTF-8" },
                        new Item { Alias = "windows1252", Name = "Windows 1252" }
                    }}
                }
            },
            new ConfigurationField {
                Key = "separator",
                Name = "Separator",
                Description = "Select the separator used in the uploaded CSV file.",
                View = "/App_Plugins/Skybrud.Umbraco.Redirects.Import/Views/Editors/Items.html",
                Config = new Dictionary<string, object> {
                    {"items", new [] {
                        new Item { Alias = "auto", Name = "Auto" },
                        new Item { Alias = "colon", Name = "Colon" },
                        new Item { Alias = "semicolon", Name = "Semi colon" },
                        new Item { Alias = "space", Name = "Space" },
                        new Item { Alias = "Tab", Name = "Tab" }
                    }}
                }
            },
            new ConfigurationField {
                Key = "file",
                Name = "File",
                Description = "Select the CSV file.",
                View = "/App_Plugins/Skybrud.Umbraco.Redirects.Import/Views/Editors/File.html",
                Config = new Dictionary<string, object> {
                    {"multiple", false}
                }
            }
        };

        public class Item {



            [JsonProperty("alias")]
            public string Alias { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

        }

        #endregion

        public IImportOptions ParseImportSettings(JObject obj) {
            return CsvImportOptions.Parse(obj);
        }

        IImportOptions IRedirectsProvider.ParseImportSettings(JObject obj) {
            return ParseImportSettings(obj);
        }

        public CsvImportOptions ParseImportSettings(Dictionary<string, string> dictionary) {
            return CsvImportOptions.FromDictionary(dictionary);
        }

        IImportOptions IRedirectsProvider.ParseImportSettings(Dictionary<string, string> dictionary) {
            return ParseImportSettings(dictionary);
        }

        public object Import(RedirectsProviderFile file, IImportOptions options) {

            if (file == null) throw new ArgumentNullException(nameof(file));
            if (options == null) throw new ArgumentNullException(nameof(options));

            if (!(options is CsvImportOptions)) throw new ArgumentException("Must be an instance of CsvImportOptions", nameof(options));

            return Import(file, (CsvImportOptions) options);

        }

        public object Import(RedirectsProviderFile file, CsvImportOptions options) {

            if (file == null) throw new ArgumentNullException(nameof(file));
            if (options == null) throw new ArgumentNullException(nameof(options));
            
            // Determine the encoding
            Encoding encoding;
            switch (options.Encoding) {

                case CsvImportEncoding.Ascii:
                    encoding = Encoding.ASCII;
                    break;
                
                case CsvImportEncoding.Utf8:
                    encoding = Encoding.UTF8;
                    break;
                
                case CsvImportEncoding.Windows1252:
                    encoding = Encoding.GetEncoding(1252);
                    break;

                default:
                    encoding = Encoding.GetEncoding(1252);
                    //using (var reader = new StreamReader(file.InputStream, Encoding.Default, true)) {
                    //    reader.Peek(); // you need this!
                    //    encoding = reader.CurrentEncoding;
                    //}
                    break;

            }
            
            // Load the CSV file from the stream
            CsvFile csv;
            using (Stream stream = file.InputStream) {
                csv = CsvFile.Load(stream, encoding);
            }

            CsvInternalImportOptions io = new CsvInternalImportOptions(csv, options);

            // Determine the columns
            MapCsvColumns(io);

            // Parse the rows
            List<RedirectImportItem> redirects = ParseCsvRows(io);


            var service = new RedirectsImportService();


            foreach (var redirect in redirects) {

                service.Import(redirect, options);

            }

            return redirects;

        }

        private void MapCsvColumns(CsvInternalImportOptions options) {
            
            foreach (CsvColumn column in options.File.Columns) {

                switch (column.Name.Replace(" ", "").ToLowerInvariant()) {

                    case "rootnodeid":
                    case "domain":
                        if (options.ColumnRootNodeId != null) break;
                        options.ColumnRootNodeId = column;
                        break;

                    case "url":
                    case "inboundurl":
                        if (options.ColumnInboundUrl != null) break;
                        options.ColumnInboundUrl = column;
                        break;

                    case "linkid":
                    case "destinationid":
                        if (options.ColumnDestinationId != null) break;
                        options.ColumnDestinationId = column;
                        break;

                    case "linktype":
                    case "linkmode":
                    case "destinationtype":
                    case "destinationmode":
                        if (options.ColumnDestinationType != null) break;
                        options.ColumnDestinationType = column;
                        break;

                    case "linkurl":
                    case "destinationurl":
                        if (options.ColumnDestinationUrl != null) break;
                        options.ColumnDestinationUrl = column;
                        break;

                }

            }
            
            if (options.ColumnRootNodeId == null) throw new RedirectsException("No column found for *root node ID*");
            if (options.ColumnInboundUrl == null) throw new RedirectsException("No column found for *inbound URL*");
            if (options.ColumnDestinationId == null) throw new RedirectsException("No column found for *destination ID*");
            if (options.ColumnDestinationType == null) throw new RedirectsException("No column found for *destination type*");
            if (options.ColumnDestinationUrl == null) throw new RedirectsException("No column found for *destination URL*");

        }

        private List<RedirectImportItem> ParseCsvRows(CsvInternalImportOptions options) {
            
            List<RedirectImportItem> redirects = new List<RedirectImportItem>();

            foreach (CsvRow row in options.File.Rows) {

                RedirectImportItem item = new RedirectImportItem {
                    AddOptions = new AddRedirectOptions()
                };

                string valueRootNodeId = row.GetCellValue(options.ColumnRootNodeId.Index);
                if (int.TryParse(valueRootNodeId, out int rootNodeId)) {
                    item.AddOptions.RootNodeId = rootNodeId;


                    IPublishedContent content = Current.UmbracoContext.Content.GetById(rootNodeId);

                    if (content != null) item.AddOptions.RootNodeKey = content.Key;

                } else {

                    // TODO: Should we validate the domain? Any security concerns about using the input value?

                    IDomain domain = Current.Services.DomainService.GetByName(valueRootNodeId);
                    if (domain == null) {
                        item.Errors.Add("Unknown root node ID or domain: " + valueRootNodeId);
                    } else if (domain.RootContentId == null) {
                        item.Errors.Add("Domain doesn't have a root node ID: " + valueRootNodeId);
                    } else {
                        item.AddOptions.RootNodeId = domain.RootContentId.Value;
                        IPublishedContent content = Current.UmbracoContext.Content.GetById(domain.RootContentId.Value);
                        if (content != null) item.AddOptions.RootNodeKey = content.Key;
                    }

                }

                string valueInboundUrl = row.GetCellValue(options.ColumnInboundUrl.Index);
                try {

                    // TODO: Should we validate the domain? Any security concerns about using the input value?

                    string testUrl = "http://hest.dk" + valueInboundUrl;
                    Uri uri = new Uri(testUrl);
                    item.AddOptions.OriginalUrl = valueInboundUrl;


                } catch (Exception) {
                    item.Errors.Add("Invalid inbound URL specified: " + valueInboundUrl);
                }





                string valueDestinationId = row.GetCellValue(options.ColumnDestinationId.Index);
                if (!int.TryParse(valueDestinationId, out int destinationId)) {
                    item.Errors.Add("Invalid destination ID: " + valueDestinationId);
                }



                string destinationUrl = row.GetCellValue(options.ColumnDestinationUrl.Index);
                if (string.IsNullOrWhiteSpace(destinationUrl)) {
                    item.Errors.Add("Invalid destination URL: " + destinationUrl);
                }




                string valueLinkMode = row.GetCellValue(options.ColumnDestinationType.Index);
                if (!EnumUtils.TryParseEnum(valueLinkMode, out RedirectDestinationType destinationMode)) {
                    item.Errors.Add("Invalid destination type: " + valueLinkMode);
                }


                string destinatioName = "";
                Guid destinationKey = Guid.Empty;
                if (destinationMode == RedirectDestinationType.Content) {
                    IPublishedContent content = Current.UmbracoContext.Content.GetById(destinationId);
                    if (content != null) {
                        destinatioName = content.Name;
                        destinationKey = content.Key;
                        destinationUrl = content.Url;
                    }
                } else if (destinationMode == RedirectDestinationType.Media) {
                    IPublishedContent media = Current.UmbracoContext.Media.GetById(destinationId);
                    if (media != null) {
                        destinatioName = media.Name;
                        destinationKey = media.Key;
                        destinationUrl = media.Url;
                    }
                }

                item.AddOptions.Destination = new RedirectDestination(destinationId, destinationKey, destinationUrl, destinationMode) { /*Name = destinatioName*/ };
                //item.AddOptions.Overwrite = options.Options.OverwriteExisting;
                
                redirects.Add(item);

            }

            return redirects;

        }

    }

    class CsvInternalImportOptions {

        public CsvFile File { get; }

        public CsvImportOptions Options { get; }

        public CsvColumn ColumnRootNodeId { get; set; }

        public CsvColumn ColumnInboundUrl { get; set; }

        public CsvColumn ColumnDestinationId { get; set; }

        public CsvColumn ColumnDestinationType { get; set; }

        public CsvColumn ColumnDestinationUrl { get; set; }

        public CsvInternalImportOptions(CsvFile file, CsvImportOptions options) {
            File = file;
            Options = options;
        }

    }

}