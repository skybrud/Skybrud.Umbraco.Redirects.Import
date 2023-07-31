using Newtonsoft.Json.Linq;
using Skybrud.Csv;
using Skybrud.Essentials.Reflection;
using Skybrud.Umbraco.Redirects.Models;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System;
using System.Linq;
using System.Web.Http;
using Skybrud.Umbraco.Redirects.Import.Csv;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;

namespace Skybrud.Umbraco.Redirects.Import {

    [PluginController("Skybrud")]
    [AngularJsonOnlyConfiguration]
    public class RedirectsExportController : UmbracoAuthorizedApiController {

        private readonly IRedirectsService _redirectsService;

        public RedirectsExportController(IRedirectsService redirectsService) {
            _redirectsService = redirectsService;
        }

        [HttpGet]
        public object Csv() {

            // Get all redirects from the database
            RedirectsSearchResult all = _redirectsService.GetRedirects(limit: int.MaxValue);

            // Create the CSV file
            CsvFile csv = all.Items
                .OrderBy(x => x.Id)
                .Select(ToCsvDto)
                .ToCsv(CsvSeparator.SemiColon, Encoding.UTF8);

            // Convert it to a string
            string contents = csv.ToString();

            HttpResponseMessage response = new HttpResponseMessage {
                Content = new StringContent(contents, Encoding.Default, "text/csv") {
                    Headers = {
                        ContentDisposition = new ContentDispositionHeaderValue("attachment") {
                            FileName = $"Redirects_{DateTime.UtcNow:yyyyMMddHHmmss}.csv"
                        }
                    }
                }
            };

            return response;

        }

        [HttpGet]
        public object Json() {

            // Get all redirects from the database
            RedirectsSearchResult all = _redirectsService.GetRedirects(limit: int.MaxValue);

            // Initialize a dictionary with relevant versions
            Dictionary<string, string> versions = new Dictionary<string, string> {
                { "UmbracoCms.Core", ReflectionUtils.GetInformationalVersion(typeof(UrlHelperExtensions)) },
                { "Skybrud.Umbraco.Redirects", ReflectionUtils.GetInformationalVersion<RedirectsService>() },
                { "Skybrud.Umbraco.Redirects.Import", ReflectionUtils.GetInformationalVersion<RedirectsExportController>() }
            };

            // Initialize a new JSON array for the redirects
            JArray redirects = new JArray();

            // Iterate through the redirects
            foreach (RedirectItem redirect in all.Items.OrderBy(x => x.Id)) {

                JArray warnings = new JArray();

                // Does the redirect have a root node?
                Guid rootNodeKey = Guid.Empty;
                if (redirect.RootId > 0) {
                    IPublishedContent rootNode = Umbraco.Content(redirect.RootId);
                    if (rootNode == null) {
                        warnings.Add($"Root node with ID '{redirect.RootId}' not found.'");
                    } else {
                        rootNodeKey = rootNode.Key;
                    }
                }

                JObject json = new JObject {
                    {"id", redirect.Id},
                    {"key", redirect.Key},
                    {"rootNode", rootNodeKey},
                    {"path", redirect.Url.Split('?')[0]},
                    {"queryString", redirect.Url.Split('#').Skip(1).FirstOrDefault() ?? string.Empty},
                    {"url", redirect.Url},
                    {"createDate", redirect.Created.Iso8601},
                    {"updateDate", redirect.Updated.Iso8601},
                    {"type", redirect.IsPermanent ? "permanent" : "temporary"},
                    {"permanent", redirect.IsPermanent},
                    {"forward", redirect.ForwardQueryString}
                };

                switch (redirect.Link.Type) {

                    case RedirectDestinationType.Content: {

                            IPublishedContent content = Umbraco.Content(redirect.LinkId);

                            if (content == null) {
                                warnings.Add($"Content node with ID '{redirect.LinkId}' not found.'");
                            }

                            json.Add("destination", new JObject {
                            {"id", redirect.LinkId},
                            {"key", content?.Key ?? Guid.Empty},
                            {"name", content?.Name},
                            {"url", redirect.LinkUrl},
                            {"type", "content"}
                        });

                            break;

                        }

                    case RedirectDestinationType.Media: {

                            IPublishedContent media = Umbraco.Media(redirect.LinkId);

                            if (media == null) {
                                warnings.Add($"Media node with ID '{redirect.LinkId}' not found.'");
                            }

                            json.Add("destination", new JObject {
                                {"id", redirect.LinkId},
                                {"key", media?.Key ?? Guid.Empty},
                                {"name", media?.Name},
                                {"url", redirect.LinkUrl},
                                {"type", "media"}
                            });

                            break;

                        }

                    default:

                        json.Add("destination", new JObject {
                            {"url", redirect.LinkUrl},
                            {"type", "url"}
                        });

                        break;

                }

                if (warnings.Any()) json.Add("warnings", warnings);

                redirects.Add(json);

            }

            return new { versions, redirects };

        }

        private CsvRedirect ToCsvDto(RedirectItem redirect) {

            // Does the redirect have a root node?
            Guid rootNodeKey = Guid.Empty;
            if (redirect.RootId > 0) {
                IPublishedContent rootNode = Umbraco.Content(redirect.RootId);
                if (rootNode != null) rootNodeKey = rootNode.Key;
            }

            Guid destinationKey = Guid.Empty;
            string destinationName = string.Empty;
            string destinationUrl = redirect.LinkUrl.Split('?')[0];
            string destionationQuery = redirect.LinkUrl.Split('?').Skip(1).FirstOrDefault() ?? string.Empty;

            switch (redirect.LinkMode) {

                case RedirectDestinationType.Content:
                    IPublishedContent content = Umbraco.Content(redirect.LinkId);
                    if (content != null) {
                        destinationKey = content.Key;
                        destinationName = content.Name;
                    }
                    break;

                case RedirectDestinationType.Media:
                    IPublishedContent media = Umbraco.Media(redirect.LinkId);
                    if (media != null) {
                        destinationKey = media.Key;
                        destinationName = media.Name;
                    }
                    break;

            }

            return new CsvRedirect {
                Id = redirect.Id,
                Key = redirect.Key,
                RootNodeKey = rootNodeKey,
                Url = redirect.Url,
                QueryString = redirect.QueryString,
                DestinationType = redirect.LinkMode,
                DestinationKey = destinationKey,
                DestinationName = destinationName,
                DestinationUrl = destinationUrl,
                DestinationQuery = destionationQuery,
                Type = redirect.IsPermanent ? "Permanent" : "Temporary",
                ForwardQueryString = redirect.ForwardQueryString,
                CreateDate = redirect.Created.Iso8601,
                UpdateDate = redirect.Updated.Iso8601
            };

        }

    }

}