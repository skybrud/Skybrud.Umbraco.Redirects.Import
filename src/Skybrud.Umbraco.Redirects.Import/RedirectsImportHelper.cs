using Skybrud.Essentials.Enums;
using Skybrud.Essentials.Strings;
using Skybrud.Umbraco.Redirects.Import.Exceptions;
using Skybrud.Umbraco.Redirects.Import.Extensions;
using Skybrud.Umbraco.Redirects.Import.Models.Import;
using Skybrud.Umbraco.Redirects.Models;
using Skybrud.Umbraco.Redirects.Models.Options;
using System;
using System.Collections.Generic;
using System.Data;
using Skybrud.Umbraco.Redirects.Import.Importers;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Extensions;

namespace Skybrud.Umbraco.Redirects.Import {

    public class RedirectsImportHelper {

        private readonly IMediaService _mediaService;

        #region Properties

        public IDomainService DomainService { get; }

        public IUmbracoContext Umbraco { get; }

        public DataTable DataTable { get; set; }

        public IImportOptions Options { get; set; }

        public ImportColumnList Columns { get; set; }

        #endregion

        #region Constructors

        public RedirectsImportHelper(IDomainService domainService, IMediaService mediaService, IUmbracoContextAccessor umbracoContextAccessor, DataTable table, IImportOptions options) {

            _mediaService = mediaService;

            DomainService = domainService;
            if (!umbracoContextAccessor.TryGetUmbracoContext(out IUmbracoContext umbraco)) throw new Exception("Unable to get current Umbraco context.");
            Umbraco = umbraco;

            DataTable = table;
            Options = options;

        }

        #endregion

        #region Member methods

        public bool TryGetContent(int id, out IPublishedContent result) {
            result = Umbraco?.Content?.GetById(id);
            return result != null;
        }

        public bool TryGetContent(Guid key, out IPublishedContent result) {
            result = Umbraco?.Content?.GetById(key);
            return result != null;
        }

        public bool TryGetContent(string route, out IPublishedContent result) {
            result = Umbraco?.Content?.GetByRoute(route);
            return result != null;
        }

        public bool TryGetMedia(int id, out IPublishedContent result) {
            result = Umbraco?.Media?.GetById(id);
            return result != null;
        }

        public bool TryGetMedia(Guid key, out IPublishedContent result) {
            result = Umbraco?.Media?.GetById(key);
            return result != null;
        }

        public bool TryGetMedia(string route, out IPublishedContent result) {

            IMedia media = _mediaService.GetMediaByPath(route);
            if (media != null) return TryGetMedia(media.Key, out result);

            result = null;
            return false;

        }

        public virtual ImportColumnList MapColumns(RedirectsImportHelper inter) {

            ImportColumnList columns = new();

            foreach (DataColumn column in inter.DataTable.Columns) {

                switch (column.ColumnName.Replace(" ", "").ToLowerInvariant()) {

                    case "siteid":
                    case "rootid":
                    case "rootnodeid":
                    case "sitekey":
                    case "rootkey":
                    case "rootnodekey":
                    case "domain":
                        if (columns.RootNode != null) break;
                        columns.RootNode = column;
                        break;

                    case "url":
                    case "old":
                    case "from":
                    case "inboundurl":
                    case "originalurl":
                        if (columns.InboundUrl != null) break;
                        columns.InboundUrl = column;
                        break;

                    case "query":
                    case "querystring":
                    case "inboundquery":
                    case "inboundquerystring":
                        if (columns.InboundQuery != null) break;
                        columns.InboundQuery = column;
                        break;

                    case "destinationid":
                        if (columns.DestinationId != null) break;
                        columns.DestinationId = column;
                        break;

                    case "destinationkey":
                        if (columns.DestinationKey != null) break;
                        columns.DestinationKey = column;
                        break;

                    case "destinationtype":
                        if (columns.DestinationType != null) break;
                        columns.DestinationType = column;
                        break;

                    case "to":
                    case "new":
                    case "newurl":
                    case "destinationurl":
                        if (columns.DestinationUrl != null) break;
                        columns.DestinationUrl = column;
                        break;

                    case "destinationquery":
                        if (columns.DestinationQuery != null) break;
                        columns.DestinationQuery = column;
                        break;

                    case "destinationfragment":
                        if (columns.DestinationFragment != null) break;
                        columns.DestinationFragment = column;
                        break;

                    case "type":
                    case "redirecttype":
                    case "permanent":
                    case "ispermanent":
                        if (columns.RedirectType != null) break;
                        columns.RedirectType = column;
                        break;

                    case "forward":
                    case "forwardquery":
                    case "forwardquerystring":
                        if (columns.ForwardQueryString != null) break;
                        columns.ForwardQueryString = column;
                        break;

                }

            }

            if (columns.InboundUrl == null) throw new RedirectsImportException("No column found for 'inbound URL'");
            if (columns.DestinationId == null && columns.DestinationUrl == null) throw new RedirectsImportException("No column found for destination (ID, key or URL");

            return columns;

        }

        public virtual List<RedirectImportItem> ParseRedirects(RedirectsImportHelper inter) {

            List<RedirectImportItem> redirects = new();

            foreach (DataRow row in inter.DataTable.Rows) {

                RedirectImportItem item = new() {
                    AddOptions = new AddRedirectOptions {
                        Overwrite = Options.OverwriteExisting
                    }
                };

                ParseRootNode(inter, row, item);
                ParseInboundUrl(inter, row, item);
                ParseDestination(row, item);
                ParseRedirectType(inter, row, item);

                redirects.Add(item);

            }

            return redirects;

        }

        public virtual void ParseRootNode(RedirectsImportHelper inter, DataRow row, RedirectImportItem item) {

            string valueRootNode = row.GetString(inter.Columns.RootNode);
            if (string.IsNullOrWhiteSpace(valueRootNode)) return;

            // Does the value match a numeric ID?
            if (int.TryParse(valueRootNode, out int rootNodeId)) {
                item.AddOptions.RootNodeId = rootNodeId;
                if (inter.TryGetContent(rootNodeId, out IPublishedContent content)) {
                    item.AddOptions.RootNodeKey = content.Key;
                    return;
                }
                item.Errors.Add($"Root node with ID '{valueRootNode}' not found.");
                return;
            }

            // Does the value match a GUID key?
            if (Guid.TryParse(valueRootNode, out Guid rootNodeKey)) {
                item.AddOptions.RootNodeKey = rootNodeKey;
                if (rootNodeKey == Guid.Empty) {
                    return;
                }
                if (inter.TryGetContent(rootNodeKey, out IPublishedContent content)) {
                    item.AddOptions.RootNodeId = content.Id;
                    return;
                }
                item.Errors.Add($"Root node with key '{rootNodeKey}' not found.");
                return;
            }

            // TODO: Should we validate the domain? Any security concerns about using the input value?

            IDomain domain = DomainService.GetByName(valueRootNode!);

            if (domain == null) {
                item.Errors.Add($"Unknown root node or domain: '{valueRootNode}'");
            } else if (domain.RootContentId == null) {
                item.Errors.Add($"Domain doesn't have a root node ID: '{valueRootNode}'");
            } else {
                item.AddOptions.RootNodeId = domain.RootContentId.Value;
                if (inter.TryGetContent(rootNodeId, out IPublishedContent content)) {
                    item.AddOptions.RootNodeKey = content.Key;
                    return;
                }
                item.Errors.Add($"Root node with ID '{domain.RootContentId.Value}' not found.");
            }

        }

        public virtual void ParseInboundUrl(RedirectsImportHelper inter, DataRow row, RedirectImportItem item) {

            string url = row.GetString(inter.Columns.InboundUrl);
            string query = row.GetString(inter.Columns.InboundQuery);

            try {
                // TODO: Should we validate the URL?
                string testUrl = "http://hest.dk" + url;
                Uri _ = new(testUrl);
                item.AddOptions.OriginalUrl = url;
            } catch (Exception) {
                item.Errors.Add($"Invalid inbound URL specified: {url}");
            }

            // Return now if the query string column doesn't have a value
            if (!string.IsNullOrWhiteSpace(query)) return;

            // TODO: Seems like we're missing something here

        }

        public virtual void ParseDestination(DataRow row, RedirectImportItem item) {

            RedirectDestinationType destinationType = RedirectDestinationType.Url;

            if (Columns.DestinationType is not null) {
                string value = row.GetString(Columns.DestinationType);
                if (!EnumUtils.TryParseEnum(value, out destinationType)) {
                    item.Errors.Add($"Invalid destination type: {value}");
                }
            }

            string destinationUrl = row.GetString(Columns.DestinationUrl);
            string destinationQuery = null;
            string destinationFragment = null;

            if (string.IsNullOrWhiteSpace(destinationUrl)) {
                item.Errors.Add($"Invalid destination URL: {destinationUrl}");
            }

            IPublishedContent destination = null;

            if (Columns.DestinationKey is not null) {
                string value = row.GetString(Columns.DestinationKey);
                if (!string.IsNullOrWhiteSpace(value)) {
                    if (!Guid.TryParse(value, out Guid destinationKey)) {
                        item.Errors.Add($"Invalid destination key: {value}");
                    } else if (destinationType == RedirectDestinationType.Content) {
                        if (!TryGetContent(destinationKey, out destination)) {
                            item.Errors.Add($"Destination content with key '{destinationKey}' not found.");
                        }
                    } else if (destinationType == RedirectDestinationType.Media) {
                        if (!TryGetMedia(destinationKey, out destination)) {
                            item.Errors.Add($"Destination media with key '{destinationKey}' not found.");
                        }
                    }
                }
            }

            if (destination == null && Columns.DestinationId is not null) {
                string value = row.GetString(Columns.DestinationId);
                if (!string.IsNullOrWhiteSpace(value)) {
                    if (!int.TryParse(value, out int destinationId)) {
                        item.Errors.Add($"Invalid destination ID: {value}");
                    } else if (destinationId != 0 && !TryGetContent(destinationId, out destination)) {
                        item.Errors.Add($"Destination node with ID '{destinationId}' not found.");
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(destinationUrl)) {

                string[] split = destinationUrl.Split('#');

                if (split.Length == 2 && split[1].Length > 0) {
                    destinationUrl = split[0];
                    destinationFragment = split[1];
                }

                split = destinationUrl.Split('?');

                if (split.Length == 2 && split[1].Length > 0) {
                    destinationUrl = split[0];
                    destinationQuery = split[1];
                }

            } else if (destination != null) {
                destinationUrl = destination.Url();
            }

            if (string.IsNullOrWhiteSpace(destinationUrl)) {
                item.Errors.Add("No destination URL found.");
                return;
            }

            if (int.TryParse(destinationUrl, out int id)) {
                if (destination == null) {
                    if (TryGetContent(id, out IPublishedContent content)) {
                        destination = content;
                        destinationUrl = content.Url();
                        destinationType = RedirectDestinationType.Content;
                    } else if (TryGetMedia(id, out IPublishedContent media)) {
                        destination = media;
                        destinationUrl = media.Url();
                        destinationType = RedirectDestinationType.Media;
                    } else {
                        item.Errors.Add($"No destination found with ID '{id}.");
                        return;
                    }
                }
            } else if (Guid.TryParse(destinationUrl, out Guid key)) {
                if (destination == null) {
                    if (TryGetContent(key, out IPublishedContent content)) {
                        destination = content;
                        destinationUrl = content.Url();
                        destinationType = RedirectDestinationType.Content;
                    } else if (TryGetMedia(key, out IPublishedContent media)) {
                        destination = media;
                        destinationUrl = media.Url();
                        destinationType = RedirectDestinationType.Media;
                    } else {
                        item.Errors.Add($"No destination found with key '{key}.");
                        return;
                    }
                }
            } else if (destinationUrl.StartsWith("/media")) {
                if (destination == null) {
                    if (TryGetMedia(destinationUrl, out IPublishedContent media)) {
                        destination = media;
                        destinationUrl = media.Url();
                        destinationType = RedirectDestinationType.Media;
                    } else {
                        // TODO: Should this trigger an error, or just create an 'URL' redirect?
                        item.Errors.Add($"No destination found with URL '{destinationUrl}.");
                        return;
                    }
                }
            } else if (destinationUrl.StartsWith("/")) {
                if (destination == null) {
                    if (TryGetContent(destinationUrl, out IPublishedContent content)) {
                        destination = content;
                        destinationUrl = content.Url();
                        destinationType = RedirectDestinationType.Content;
                    } else {
                        // TODO: Should this trigger an error, or just create an 'URL' redirect?
                        item.Errors.Add($"No destination found with URL '{destinationUrl}.");
                        return;
                    }
                }
            } else {
                item.Errors.Add($"Destination URL is not a valid URL: {destinationUrl}");
                return;
            }

            if (Columns.DestinationQuery is not null) {
                string value = row.GetString(Columns.DestinationQuery);
                if (!string.IsNullOrWhiteSpace(value)) destinationQuery = value;
            }

            if (Columns.DestinationFragment is not null) {
                string value = row.GetString(Columns.DestinationFragment);
                if (!string.IsNullOrWhiteSpace(value)) destinationFragment = value;
            }

            item.AddOptions.Destination = new RedirectDestination {
                Id = destination?.Id ?? default,
                Key = destination?.Key ?? default,
                Url = destinationUrl,
                Query = destinationQuery,
                Fragment = destinationFragment,
                Type = destinationType,
                Name = destination?.Name
            };

        }

        public virtual void ParseRedirectType(RedirectsImportHelper inter, DataRow row, RedirectImportItem item) {

            // Get the value from the cell (if found)
            string value = row.GetString(inter.Columns.RedirectType);

            // Does the column specify an explicit type?
            if (EnumUtils.TryParseEnum(value, out RedirectType type)) {
                item.AddOptions.Type = type;
                return;
            }

            // Or is the value a boolean value?
            if (StringUtils.TryParseBoolean(value, out bool isPermanent)) {
                item.AddOptions.Type = isPermanent ? RedirectType.Permanent : RedirectType.Temporary;
                return;
            }

            // Use the fallback value if we can't tell from the imported file
            item.AddOptions.Type = inter.Options.DefaultRedirectType;

        }

        public virtual void ParseForwardQueryString(RedirectsImportHelper inter, DataRow row, RedirectImportItem item) {

            // Get the value from the cell (if found)
            string value = row.GetString(inter.Columns.ForwardQueryString);

            // Parse the value into a boolean
            item.AddOptions.ForwardQueryString = StringUtils.ParseBoolean(value);

        }

        #endregion

    }

}