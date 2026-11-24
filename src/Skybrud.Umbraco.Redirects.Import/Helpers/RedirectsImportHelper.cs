using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using Skybrud.Essentials.Common;
using Skybrud.Essentials.Enums;
using Skybrud.Essentials.Strings;
using Skybrud.Umbraco.Redirects.Import.Exceptions;
using Skybrud.Umbraco.Redirects.Import.Extensions;
using Skybrud.Umbraco.Redirects.Import.Importers;
using Skybrud.Umbraco.Redirects.Import.Models.Import;
using Skybrud.Umbraco.Redirects.Import.Services;
using Skybrud.Umbraco.Redirects.Models;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Extensions;

// ReSharper disable StringLiteralTypo

namespace Skybrud.Umbraco.Redirects.Import.Helpers;

/// <summary>
/// Helper class used for parsing redirects to be imported.
/// </summary>
public class RedirectsImportHelper {

    private readonly ILanguageService _languageService;
    private readonly IMediaService _mediaService;
    private readonly RedirectsImportServiceDependencies _dependencies;

    #region Properties

    /// <summary>
    /// Gets a reference to the current <see cref="IDomainService"/>.
    /// </summary>
    public IDomainService DomainService { get; }

    /// <summary>
    /// Gets a reference to the current <see cref="IUmbracoContext"/>.
    /// </summary>
    public IUmbracoContext Umbraco { get; }

    /// <summary>
    /// Gets a reference to the <see cref="DataTable"/> holding the redirects to be imported.
    /// </summary>
    public DataTable DataTable { get; set; }

    /// <summary>
    /// Gets a reference to the options for importing the redirects.
    /// </summary>
    public IImportOptions Options { get; set; }

    /// <summary>
    /// Gets a reference to the <see cref="ImportColumnList"/> with the mapped columns.
    /// </summary>
    public ImportColumnList? Columns { get; set; }

    /// <summary>
    /// Gets a reference to Umbraco's <see cref="IDocumentUrlService"/>.
    /// </summary>
    public IDocumentUrlService DocumentUrlService => _dependencies.DocumentUrlService;

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance based on the specified dependencies.
    /// </summary>
    public RedirectsImportHelper(RedirectsImportServiceDependencies dependencies, DataTable table, IImportOptions options) {

        _dependencies = dependencies;
        _languageService = dependencies.LanguageService;
        _mediaService = dependencies.MediaService;

        DomainService = dependencies.DomainService;
        Umbraco = dependencies.UmbracoContextAccessor.GetRequiredUmbracoContext();
        DataTable = table;
        Options = options;

    }

    #endregion

    #region Member methods

    /// <summary>
    /// Attempts to get a <see cref="IPublishedContent"/> instance representing the content with the specified <paramref name="id"/>.
    /// </summary>
    /// <param name="id">The numeric ID of the content.</param>
    /// <param name="result">When this method returns, holds the matching <see cref="IPublishedContent"/> if successful; otherwise, <see langword="null"/>.</param>
    /// <returns><see langword="true"/> if successful; otherwise, <see langword="false"/>.</returns>
    public bool TryGetContent(int id, [NotNullWhen(true)] out IPublishedContent? result) {
        result = Umbraco.Content.GetById(id);
        return result != null;
    }

    /// <summary>
    /// Attempts to get a <see cref="IPublishedContent"/> instance representing the content with the specified <paramref name="key"/>.
    /// </summary>
    /// <param name="key">The GUID key of the content.</param>
    /// <param name="result">When this method returns, holds the matching <see cref="IPublishedContent"/> if successful; otherwise, <see langword="null"/>.</param>
    /// <returns><see langword="true"/> if successful; otherwise, <see langword="false"/>.</returns>
    public bool TryGetContent(Guid key, [NotNullWhen(true)] out IPublishedContent? result) {
        result = Umbraco.Content.GetById(key);
        return result != null;
    }

    /// <summary>
    /// Attempts to resolve published content by its route, relative to a specified start node.
    /// </summary>
    /// <param name="startNodeId">The ID of the node from which to resolve the route.</param>
    /// <param name="route">The route of the content to resolve.</param>
    /// <param name="result">
    /// When this method returns <see langword="true"/>, contains the resolved <see cref="IPublishedContent"/>;
    /// otherwise, <see langword="null"/>.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if matching published content was found; otherwise, <see langword="false"/>.
    /// </returns>
    public bool TryGetContent(int startNodeId, string route, [NotNullWhen(true)] out IPublishedContent? result) {
        return TryGetContent(startNodeId, null, route, out result);
    }

    /// <summary>
    /// Attempts to resolve published content by its route and culture, relative to a specified start node.
    /// </summary>
    /// <param name="startNodeId">The ID of the node from which to resolve the route.</param>
    /// <param name="culture">
    /// The culture to use when resolving the route, or <see langword="null"/> to resolve without specifying a culture.
    /// </param>
    /// <param name="route">The route of the content to resolve.</param>
    /// <param name="result">
    /// When this method returns <see langword="true"/>, contains the resolved <see cref="IPublishedContent"/>;
    /// otherwise, <see langword="null"/>.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if matching published content was found; otherwise, <see langword="false"/>.
    /// </returns>
    public bool TryGetContent(int startNodeId, string? culture, string route, [NotNullWhen(true)] out IPublishedContent? result) {

        if (DocumentUrlService.GetDocumentKeyByRoute(route, culture, startNodeId, false) is not { } key) {
            result = null;
            return false;
        }

        result = Umbraco.Content.GetById(key);
        return result != null;

    }

    /// <summary>
    /// Attempts to get a <see cref="IPublishedContent"/> instance representing the media with the specified <paramref name="id"/>.
    /// </summary>
    /// <param name="id">The numeric ID the media.</param>
    /// <param name="result">When this method returns, holds the matching <see cref="IPublishedContent"/> if successful; otherwise, <see langword="null"/>.</param>
    /// <returns><see langword="true"/> if successful; otherwise, <see langword="false"/>.</returns>
    public bool TryGetMedia(int id, [NotNullWhen(true)] out IPublishedContent? result) {
        result = Umbraco.Media.GetById(id);
        return result != null;
    }

    /// <summary>
    /// Attempts to get a <see cref="IPublishedContent"/> instance representing the media with the specified <paramref name="key"/>.
    /// </summary>
    /// <param name="key">The GUID key the media.</param>
    /// <param name="result">When this method returns, holds the matching <see cref="IPublishedContent"/> if successful; otherwise, <see langword="null"/>.</param>
    /// <returns><see langword="true"/> if successful; otherwise, <see langword="false"/>.</returns>
    public bool TryGetMedia(Guid key, [NotNullWhen(true)] out IPublishedContent? result) {
        result = Umbraco.Media.GetById(key);
        return result != null;
    }

    /// <summary>
    /// Attempts to get a <see cref="IPublishedContent"/> instance representing the media at the specified <paramref name="route"/>.
    /// </summary>
    /// <param name="route">The route of the media.</param>
    /// <param name="result">When this method returns, holds the matching <see cref="IPublishedContent"/> if successful; otherwise, <see langword="null"/>.</param>
    /// <returns><see langword="true"/> if successful; otherwise, <see langword="false"/>.</returns>
    public bool TryGetMedia(string route, [NotNullWhen(true)] out IPublishedContent? result) {

        IMedia? media = _mediaService.GetMediaByPath(route);
        if (media != null) return TryGetMedia(media.Key, out result);

        result = null;
        return false;

    }

    /// <summary>
    /// Maps the various columns of the based on the redirects of the specified redirects import <paramref name="helper"/>.
    /// </summary>
    /// <param name="helper">A reference to the redirects import helper.</param>
    /// <returns>An instance of <see cref="ImportColumnList"/> with the column mapping.</returns>
    /// <exception cref="RedirectsImportException">If one or more required columns are not found.</exception>
    public virtual ImportColumnList MapColumns(RedirectsImportHelper helper) {

        ImportColumnList columns = new();

        foreach (DataColumn column in helper.DataTable.Columns) {

            switch (column.ColumnName.Replace(" ", "").ToLowerInvariant()) {

                // Root Node / Domain
                case "site":
                case "siteid":
                case "rootid":
                case "rootnode":
                case "rootnodeid":
                case "sitekey":
                case "rootkey":
                case "rootnodekey":
                case "domain":
                    if (columns.RootNode != null) break;
                    columns.RootNode = column;
                    break;

                // Original URL
                case "url":
                case "old":
                case "from":
                case "inboundurl":
                case "originalurl":
                case "oldurl":
                    if (columns.InboundUrl != null) break;
                    columns.InboundUrl = column;
                    break;

                case "query":
                case "querystring":
                case "inboundquery":
                case "inboundquerystring":
                case "originalquery":
                case "originalquerystring":
                    if (columns.InboundQuery != null) break;
                    columns.InboundQuery = column;
                    break;

                case "destinationid":
                case "redirectnodeid":
                    if (columns.DestinationId != null) break;
                    columns.DestinationId = column;
                    break;

                case "destinationkey":
                case "redirectnodekey":
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
                case "redirecturl":
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
                case "redirecthttpcode":
                    if (columns.RedirectType != null) break;
                    columns.RedirectType = column;
                    break;

                case "forward":
                case "forwardquery":
                case "forwardquerystring":
                    if (columns.ForwardQueryString != null) break;
                    columns.ForwardQueryString = column;
                    break;

                case "culture":
                case "destinationculture":
                    if (columns.DestinationCulture != null) break;
                    columns.DestinationCulture = column;
                    break;

            }

        }

        if (columns.InboundUrl == null) throw new RedirectsImportException("No column found for 'inbound URL'");
        if (columns.DestinationId == null && columns.DestinationUrl == null) throw new RedirectsImportException("No column found for destination (ID, key or URL)");

        return columns;

    }

    /// <summary>
    /// Parses the redirects.
    /// </summary>
    /// <returns>A list of <see cref="RedirectImportItem"/> representing the parsed redirects.</returns>
    public virtual List<RedirectImportItem> ParseRedirects() {

        List<RedirectImportItem> redirects = new();

        foreach (DataRow row in DataTable.Rows) {

            RedirectImportItem item = new() {
                Options = {
                    Overwrite = Options.OverwriteExisting
                }
            };

            ParseRootNode(row, item);
            ParseInboundUrl(row, item);
            ParseDestination(row, item);
            ParseRedirectType(row, item);
            ParseForwardQueryString(row, item);

            redirects.Add(item);

        }

        return redirects;

    }

    /// <summary>
    /// Parses the value for the root node option of the specified <paramref name="row"/>.
    /// </summary>
    /// <param name="row">The row being parsed.</param>
    /// <param name="item">An instance of <see cref="RedirectImportItem"/> representing the parsed row.</param>
    public virtual void ParseRootNode(DataRow row, RedirectImportItem item) {

        if (Columns is null) throw new PropertyNotSetException(nameof(Columns));

        string? valueRootNode = row.GetString(Columns.RootNode);
        if (string.IsNullOrWhiteSpace(valueRootNode)) return;

        // Does the value match a numeric ID?
        if (int.TryParse(valueRootNode, out int rootNodeId)) {
            if (TryGetContent(rootNodeId, out IPublishedContent? content)) {
                item.Options.RootNodeKey = content.Key;
                return;
            }
            item.Errors.Add($"Root node with ID '{valueRootNode}' not found.");
            return;
        }

        // Does the value match a GUID key?
        if (Guid.TryParse(valueRootNode, out Guid rootNodeKey)) {
            item.Options.RootNodeKey = rootNodeKey;
            if (rootNodeKey == Guid.Empty) return;
            if (TryGetContent(rootNodeKey, out IPublishedContent? _)) return;
            item.Errors.Add($"Root node with key '{rootNodeKey}' not found.");
            return;
        }

        // TODO: Should we validate the domain? Any security concerns about using the input value?

        IDomain? domain = DomainService.GetByName(valueRootNode);

        if (domain == null) {
            item.Errors.Add($"Unknown root node or domain: '{valueRootNode}'");
        } else if (domain.RootContentId == null) {
            item.Errors.Add($"Domain doesn't have a root node ID: '{valueRootNode}'");
        } else {
            if (TryGetContent(domain.RootContentId.Value, out IPublishedContent? content)) {
                item.Options.RootNodeKey = content.Key;
                return;
            }
            item.Errors.Add($"Root node with ID '{domain.RootContentId.Value}' not found.");
        }

    }

    /// <summary>
    /// Parses the inbound URL of the specified <paramref name="row"/>.
    /// </summary>
    /// <param name="row">The row being parsed.</param>
    /// <param name="item">An instance of <see cref="RedirectImportItem"/> representing the parsed row.</param>
    public virtual void ParseInboundUrl(DataRow row, RedirectImportItem item) {

        if (Columns is null) throw new PropertyNotSetException(nameof(Columns));

        string? url = row.GetString(Columns.InboundUrl);
        string? query = row.GetString(Columns.InboundQuery);

        try {
            // TODO: Should we validate the URL?
            string testUrl = "http://hest.dk" + url;
            Uri _ = new(testUrl);
            item.Options.OriginalUrl = url;
        } catch (Exception) {
            item.Errors.Add($"Invalid inbound URL specified: {url}");
        }

        // Return now if the query string column doesn't have a value
        if (!string.IsNullOrWhiteSpace(query)) return;

        // TODO: Seems like we're missing something here

    }

    /// <summary>
    /// Parses the destination of the specified <paramref name="row"/>.
    /// </summary>
    /// <param name="row">The row being parsed.</param>
    /// <param name="item">An instance of <see cref="RedirectImportItem"/> representing the parsed row.</param>
    public virtual void ParseDestination(DataRow row, RedirectImportItem item) {

        if (Columns is null) throw new PropertyNotSetException(nameof(Columns));

        RedirectDestinationType destinationType = RedirectDestinationType.Url;

        if (Columns.DestinationType is not null) {
            string? value = row.GetString(Columns.DestinationType);
            if (!EnumUtils.TryParseEnum(value, out destinationType)) {
                item.Errors.Add($"Invalid destination type: {value}");
            }
        }

        string? destinationUrl = row.GetString(Columns.DestinationUrl);
        string? destinationQuery = null;
        string? destinationFragment = null;
        string? destinationCulture = null;

        IPublishedContent? destination = null;

        // If a destination culture is specified, we validate that culture. If
        // the culture doesn't match a language in Umbraco, we trigger an error
        if (Columns.DestinationCulture is not null) {
            string? value = row.GetString(Columns.DestinationCulture);
            if (!string.IsNullOrWhiteSpace(value)) {
                if (TryGetLanguage(value, out ILanguage? language)) {
                    destinationCulture = language.IsoCode;
                } else {
                    item.Errors.Add($"The value '{value}' specified for the '{Columns.DestinationCulture.ColumnName}' column does not match a configured language in Umbraco.");
                }
            }
        }

        if (Columns.DestinationKey is not null) {
            string? value = row.GetString(Columns.DestinationKey);
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
            string? value = row.GetString(Columns.DestinationId);
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
                if (TryGetContent(id, out IPublishedContent? content)) {
                    destination = content;
                    destinationUrl = content.Url();
                    destinationType = RedirectDestinationType.Content;
                } else if (TryGetMedia(id, out IPublishedContent? media)) {
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
                if (TryGetContent(key, out IPublishedContent? content)) {
                    destination = content;
                    destinationUrl = content.Url();
                    destinationType = RedirectDestinationType.Content;
                } else if (TryGetMedia(key, out IPublishedContent? media)) {
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
                if (TryGetMedia(destinationUrl, out IPublishedContent? media)) {
                    destination = media;
                    destinationUrl = media.Url();
                    destinationType = RedirectDestinationType.Media;
                } else {
                    // TODO: Should this trigger an error, or just create a 'URL' redirect?
                    item.Errors.Add($"No destination found with URL '{destinationUrl}'.");
                    return;
                }
            }
        } else if (destinationUrl.StartsWith('/')) {
            if (destination == null && TryGetDestinationContent(item, destinationUrl, out DestinationContentResult? result)) {
                destination = result.Content;
                destinationUrl = destination.Url(result.Culture);
                destinationType = RedirectDestinationType.Content;
                destinationCulture = result.Culture;
            }
        } else if (!destinationUrl.StartsWith("http://") && !destinationUrl.StartsWith("https://")) {
            item.Errors.Add($"Destination URL is not a valid URL: {destinationUrl}");
            return;
        }

        if (Columns.DestinationQuery is not null) {
            string? value = row.GetString(Columns.DestinationQuery);
            if (!string.IsNullOrWhiteSpace(value)) destinationQuery = value;
        }

        if (Columns.DestinationFragment is not null) {
            string? value = row.GetString(Columns.DestinationFragment);
            if (!string.IsNullOrWhiteSpace(value)) destinationFragment = value;
        }

        item.Options.Destination = new RedirectDestination {
            Id = destination?.Id ?? 0,
            Key = destination?.Key ?? Guid.Empty,
            Url = destinationUrl,
            Query = destinationQuery ?? string.Empty,
            Fragment = destinationFragment ?? string.Empty,
            Type = destinationType,
            Name = destination?.Name ?? string.Empty,
            Culture = destinationCulture ?? string.Empty
        };

    }

    /// <summary>
    /// Attempts to resolve a destination URL to published content using the root node
    /// and assigned domains configured for the specified import item.
    /// </summary>
    /// <param name="item">The redirect import item containing the resolution options and error collection.</param>
    /// <param name="destinationUrl">The destination URL to resolve.</param>
    /// <param name="result">
    /// When this method returns <see langword="true"/>, contains the resolved destination content
    /// and its optional culture; otherwise, <see langword="null"/>.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the destination content was resolved successfully;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    /// <remarks>
    /// Any resolution errors are added to the error collection of <paramref name="item"/>.
    /// </remarks>
    protected bool TryGetDestinationContent(RedirectImportItem item, string destinationUrl, [NotNullWhen(true)] out DestinationContentResult? result) {
        result = GetDestinationContent(item, destinationUrl);
        return result is not null;
    }

    /// <summary>
    /// Resolves a destination URL to published content, optionally using the domains
    /// assigned to the configured root node to determine the route and culture.
    /// </summary>
    /// <param name="item">The redirect import item containing the root node configuration and error collection.</param>
    /// <param name="destinationUrl">The destination URL to resolve relative to the configured root node.</param>
    /// <returns>
    /// A <see cref="DestinationContentResult"/> containing the resolved published content
    /// and its optional culture, or <see langword="null"/> if the destination could not be resolved.
    /// </returns>
    /// <remarks>
    /// Resolution first attempts to locate the content directly beneath the configured root node.
    /// If unsuccessful, the method examines the assigned domains and attempts to resolve the
    /// destination URL after removing any matching domain path prefix.
    /// When a domain-based match is found, its language ISO code is included in the result.
    /// Resolution failures are recorded in the error collection of <paramref name="item"/>.
    /// </remarks>
    protected virtual DestinationContentResult? GetDestinationContent(RedirectImportItem item, string destinationUrl) {

        if (item.Options.RootNodeKey == null || item.Options.RootNodeKey == Guid.Empty) {
            item.Errors.Add("Cannot resolve relative destination URL without a root node being specified.");
            return null;
        }

        if (!TryGetContent(item.Options.RootNodeKey.Value, out IPublishedContent? rootNode)) {
            item.Errors.Add($"Cannot resolve relative destination URL. Root node with key '{item.Options.RootNodeKey.Value}' not found.");
            return null;
        }

        if (TryGetContent(rootNode.Id, destinationUrl, out IPublishedContent? content)) {
            return new DestinationContentResult(content, null);
        }

        // If we haven't found a content node as this point, we get the domains
        // assigned to the specified root node, and try to match the destination
        // URL against each domain and culture
        foreach (IDomain domain in _dependencies.DomainService.GetAssignedDomainsAsync(rootNode.Key, false).Result) {
            if (!domain.DomainName.Contains('/')) continue;
            string route = GetRoute(domain, destinationUrl);
            if (TryGetContent(rootNode.Id, domain.LanguageIsoCode, route, out IPublishedContent? result)) {
                return new DestinationContentResult(result, domain.LanguageIsoCode);
            }
        }

        item.Errors.Add($"No destination found with URL '{destinationUrl}' starting from root node '{rootNode.Name}' ({rootNode.Id}).");

        return null;

        static string GetRoute(IDomain domain, string destinationUrl) {

            string domainName = domain.DomainName;
            bool hasScheme = domainName.Contains("://", StringComparison.Ordinal);

            Uri uri = new(hasScheme ? domainName : $"https://{domainName}");

            string domainPath = uri.AbsolutePath.TrimEnd('/');
            string destinationPath = "/" + destinationUrl.TrimStart('/');

            if (string.IsNullOrEmpty(domainPath)) return destinationPath;

            if (destinationPath.Equals(domainPath, StringComparison.OrdinalIgnoreCase)) return "/";

            if (destinationPath.StartsWith(domainPath + "/", StringComparison.OrdinalIgnoreCase)) return destinationPath[domainPath.Length..];

            return destinationPath;

        }

    }
    /// <summary>
    /// Attempts to resolve a language by ISO code.
    /// </summary>
    /// <param name="input">The ISO code to resolve.</param>
    /// <param name="result">
    /// When this method returns <see langword="true"/>, contains the resolved <see cref="ILanguage"/>;
    /// otherwise, <see langword="null"/>.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if a matching language was found; otherwise, <see langword="false"/>.
    /// </returns>
    public virtual bool TryGetLanguage(string input, [NotNullWhen(true)] out ILanguage? result) {
        result = _languageService.GetAsync(input).Result;
        return result is not null;
    }

    /// <summary>
    /// Parses the redirect type of the specified <paramref name="row"/>.
    /// </summary>
    /// <param name="row">The row being parsed.</param>
    /// <param name="item">An instance of <see cref="RedirectImportItem"/> representing the parsed row.</param>
    public virtual void ParseRedirectType(DataRow row, RedirectImportItem item) {

        if (Columns is null) throw new PropertyNotSetException(nameof(Columns));

        // Use the fallback value if we can't tell from the imported file
        if (Columns.RedirectType is null) {
            item.Options.Type = Options.DefaultRedirectType;
            return;
        }

        // Get the value from the cell (if found)
        string? value = row.GetString(Columns.RedirectType);

        // Does the column specify an explicit type?
        if (EnumUtils.TryParseEnum(value, out RedirectType type)) {
            item.Options.Type = type;
            return;
        }

        // Or is the value a boolean value?
        if (StringUtils.TryParseBoolean(value, out bool isPermanent)) {
            item.Options.Type = isPermanent ? RedirectType.Permanent : RedirectType.Temporary;
            return;
        }

        // Or is the value an HTTP status code?
        if (EnumUtils.TryParseEnum(value, out HttpStatusCode statusCode)) {
            switch (statusCode) {
                case HttpStatusCode.MovedPermanently:
                    item.Options.Type = RedirectType.Permanent;
                    return;
                case HttpStatusCode.TemporaryRedirect:
                    item.Options.Type = RedirectType.Temporary;
                    return;
                case HttpStatusCode.Found:
                    item.Options.Type = RedirectType.Temporary;
                    item.Warnings.Add($"The '{Columns.RedirectType.ColumnName}' column specifies a '{(int) statusCode}' HTTP status code. Skybrud Redirects doesn't support this, and the redirect will be imported with the '307' status code instead.");
                    return;
                default:
                    item.Errors.Add($"The '{Columns.RedirectType.ColumnName}' column specifies an supported HTTP status code: {(int) statusCode}");
                    return;
            }
        }

        // Use the fallback value if we can't tell from the imported file
        item.Options.Type = Options.DefaultRedirectType;

    }

    /// <summary>
    /// Parses the query string option of the specified <paramref name="row"/>.
    /// </summary>
    /// <param name="row">The row being parsed.</param>
    /// <param name="item">An instance of <see cref="RedirectImportItem"/> representing the parsed row.</param>
    public virtual void ParseForwardQueryString(DataRow row, RedirectImportItem item) {

        if (Columns is null) throw new PropertyNotSetException(nameof(Columns));

        // Get the value from the cell (if found)
        string? value = row.GetString(Columns.ForwardQueryString);

        // Parse the value into a boolean
        item.Options.ForwardQueryString = StringUtils.ParseBoolean(value);

    }

    #endregion

}