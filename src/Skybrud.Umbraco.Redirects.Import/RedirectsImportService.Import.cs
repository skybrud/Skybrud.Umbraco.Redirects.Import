using Microsoft.Extensions.Logging;
using Skybrud.Umbraco.Redirects.Import.Importers;
using Skybrud.Umbraco.Redirects.Import.Models.Import;
using Skybrud.Umbraco.Redirects.Models;
using Skybrud.Umbraco.Redirects.Models.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Skybrud.Umbraco.Redirects.Import {

    public partial class RedirectsImportService {

        public virtual void Import(RedirectImportItem item) {

            try {

                IRedirect existing = _redirectsService.GetRedirectByUrl(item.AddOptions.RootNodeKey, item.AddOptions.OriginalUrl);

                if (!item.AddOptions.Overwrite && existing != null) {
                    item.Errors.Add("Redirect with same root node, URL and query string already exist.");
                    item.Status = RedirectImportStatus.AlreadyExists;
                    return;
                }

                if (existing == null) {
                    _redirectsService.AddRedirect(item.AddOptions);
                    item.Status = RedirectImportStatus.Added;
                    return;
                }

                if (HasChanges(existing, item.AddOptions)) {
                    existing.Destination = item.AddOptions.Destination;
                    existing.Type = item.AddOptions.Type;
                    existing.ForwardQueryString = item.AddOptions.ForwardQueryString;
                    _redirectsService.SaveRedirect(existing);
                    item.Status = RedirectImportStatus.Updated;
                } else {
                    item.Status = RedirectImportStatus.NotModified;
                }

            } catch (Exception ex) {
                item.Errors.Add(ex.Message); // TODO: Should not expose exception message if the exception is not 'RedirectsExcerption'
                item.Status = RedirectImportStatus.Failed;
                _logger.LogError(ex, "Failed importing redirect.");
            }

        }

        public virtual ImportResult Import(IImportOptions options, DataTable dataTable) {

            // Initialize a new helper instance
            RedirectsImportHelper helper = new(_domainService, _mediaService, _umbracoContextAccessor, dataTable, options);

            try {

                // Map the columns
                helper.Columns = helper.MapColumns(helper);

                // Parse the rows into new redirect add options
                List<RedirectImportItem> redirects = helper.ParseRedirects(helper);

                foreach (var redirect in redirects) {
                    Import(redirect);
                }

                return ImportResult.Success(redirects);

            } catch (Exception ex) {

                _logger.LogError(ex, "Import from CSV file failed.");

                return ImportResult.Failed(ex);

            }

        }

        protected virtual bool HasChanges(IRedirect a, AddRedirectOptions b) {

            StringBuilder sb1 = new();
            StringBuilder sb2 = new();

            sb1.AppendLine($"DestionationId: {a.Destination.Id}");
            sb1.AppendLine($"DestionationKey: {a.Destination.Key}");
            sb1.AppendLine($"DestionationUrl: {a.Destination.Url}");
            sb1.AppendLine($"DestionationQuery: {a.Destination.Query}");
            sb1.AppendLine($"DestionationFragment: {a.Destination.Fragment}");
            //sb1.AppendLine($"DestionationName: {a.Destination.Name}");
            sb1.AppendLine($"DestionationType: {a.Destination.Type}");
            sb1.AppendLine($"Type: {a.Type}");
            sb1.AppendLine($"ForwardQueryString: {a.ForwardQueryString}");

            sb2.AppendLine($"DestionationId: {b.Destination.Id}");
            sb2.AppendLine($"DestionationKey: {b.Destination.Key}");
            sb2.AppendLine($"DestionationUrl: {b.Destination.Url}");
            sb2.AppendLine($"DestionationQuery: {b.Destination.Query}");
            sb2.AppendLine($"DestionationFragment: {b.Destination.Fragment}");
            //sb2.AppendLine($"DestionationName: {b.Destination.Name}");
            sb2.AppendLine($"DestionationType: {b.Destination.Type}");
            sb2.AppendLine($"Type: {b.Type}");
            sb2.AppendLine($"ForwardQueryString: {b.ForwardQueryString}");

            return sb1.ToString() != sb2.ToString();

            throw new Exception((sb1.ToString() == sb2.ToString()) + "\r\n\r\n" + sb1 + "\r\n\r\nvs\r\n\r\n" + sb2);

        }

    }
}