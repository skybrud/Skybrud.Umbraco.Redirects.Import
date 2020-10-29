using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Skybrud.Umbraco.Redirects.Import.Models;
using Skybrud.Umbraco.Redirects.Import.Models.Csv;
using Skybrud.Umbraco.Redirects.Models;
using Umbraco.Core;
using Umbraco.Web.Composing;

namespace Skybrud.Umbraco.Redirects.Import.Services
{
    using System.Linq;
    using global::Umbraco.Core.Models.PublishedContent;
    using global::Umbraco.Web;
    using Microsoft.Owin.Security.Provider;
    using Skybrud.Umbraco.Redirects.Import.Utilities;
    using Skybrud.Umbraco.Redirects.Models.Options;

    public class RedirectsImportService
    {

        private IRedirectsService _redirectsService;
        private UmbracoHelper _umbracoHelper;

        public RedirectsImportService(IRedirectsService RedirectsService, UmbracoHelper UmbHelper)
        {
            _redirectsService = RedirectsService;
            _umbracoHelper = UmbHelper;
        }

        #region Public Methods
        public ImportResultSet Import(RedirectsProviderFile File, DefaultImportOptions ImportOptions, IRedirectsImportExportProvider ImportExportProvider)
        {
            //Setup
            var resultsSet = new ImportResultSet();
            resultsSet.Filename = File.FileName;
            resultsSet.DefaultOptions = ImportOptions;

            //Use provider to import the data to a standard model
            var importsDataModel = ImportExportProvider.Import(File, ImportOptions.ImportExportProviderOptions);

            //check for Provider data errors, add to results
            if (importsDataModel.ImportErrors.Any())
            {
                resultsSet.ErrorMessages.AddRange(importsDataModel.ImportErrors);
            }

            if (!importsDataModel.Items.Any())
            {
                resultsSet.ErrorMessages.Add("No Items were imported successfully for processing.");
                return resultsSet;
            }

            //Import returned data items
            var errorItems = new List<ImportErrorItem>();
            var newRedirects = new List<RedirectItem>();

            var options = new AddRedirectOptions();
            options.ForwardQueryString = ImportOptions.ForwardQueryString;
            options.IsPermanent = ImportOptions.Type == Constants.RedirectType.Permanent;
            options.RootNodeId = ImportOptions.SiteRootNode;

            //Setup
            var siteRoot = ImportOptions.SiteRootNode;
            var allContentNodes = siteRoot > 0 ? NodesHelper.AllContentNodes(_umbracoHelper, siteRoot).ToList() : NodesHelper.AllContentNodes(_umbracoHelper).ToList();
            var allMediaNodes = NodesHelper.AllMediaNodes(_umbracoHelper).ToList();

            foreach (var import in importsDataModel.Items)
            {
                bool isError = false;

                if (string.IsNullOrWhiteSpace(import.OldUrl))
                {
                    isError = true;
                    var errItem = new ImportErrorItem(import, "No Old Url provided.");
                    errorItems.Add(errItem);
                }

                //TODO: Add support for Destination ID
                if (string.IsNullOrWhiteSpace(import.NewUrl))
                {
                    isError = true;
                    var errItem = new ImportErrorItem(import, "No New Url provided.");
                    errorItems.Add(errItem);
                }

                if (!isError)
                {
                    // Split the Old URL and query string
                    string[] oldUrlParts = import.OldUrl.Split('?');
                    string oldUrl = oldUrlParts[0].TrimEnd('/');
                    string oldQuery = oldUrlParts.Length == 2 ? oldUrlParts[1] : string.Empty;

                    string[] newUrlParts = import.NewUrl.Split('?');
                    string newQuery = newUrlParts.Length == 2 ? newUrlParts[1] : string.Empty;
                    string newUrl = newUrlParts[0].TrimEnd('/');
                    string newAnchor = "";
                    if (newUrl.Contains("#"))
                    {
                        var x = newUrl.Split('#');
                        newUrl = x[0].TrimEnd('/');
                        newAnchor = "#" + x[1];
                    }


                    //Update Options
                    options.OriginalUrl = import.OldUrl;

                    //TODO: Add support for Destination ID & Type

                    //Try to determine type
                    if (newUrl.Contains("://"))
                    {
                        //TODO: Check for internal domain? 
                        //full url, assume external
                        RedirectDestination destination = new RedirectDestination(0, Guid.Empty, import.NewUrl, RedirectDestinationType.Url);
                        options.Destination = destination;

                        // Add the External redirect
                        try
                        {
                            RedirectItem redirect = _redirectsService.AddRedirect(options);
                            newRedirects.Add(redirect);
                        }
                        catch (Exception e)
                        {
                            isError = true;
                            var errItem = new ImportErrorItem(import, e.Message);
                            errorItems.Add(errItem);
                        }
                    }
                    else
                    {
                        //Try to lookup node
                        RedirectDestination destinationNode = null;

                        var matchingContent = allContentNodes.Where(n => n.Url.TrimEnd('/') == newUrl);
                        if (matchingContent.Any())
                        {
                            // Content Link
                            var node = matchingContent.First();
                            destinationNode = new RedirectDestination(node.Id, node.Key, node.Url, import.NewUrl, RedirectDestinationType.Content);
                        }
                        else
                        {
                            var matchingMedia = allMediaNodes.Where(n => n.Url == newUrl);
                            if (matchingMedia.Any())
                            {
                                // Media Link
                                var node = matchingMedia.First();
                                destinationNode = new RedirectDestination(node.Id, node.Key, node.Url, RedirectDestinationType.Media);
                            }
                            else
                            {
                                isError = true;
                                var errItem = new ImportErrorItem(import,
                                    "Unable to locate a published page matching the New Url");
                                errorItems.Add(errItem);
                            }
                        }

                        if (!isError)
                        {
                            // Add the redirect
                            try
                            {
                                options.Destination = destinationNode;
                                RedirectItem redirect = _redirectsService.AddRedirect(options);
                                newRedirects.Add(redirect);
                            }
                            catch (Exception e)
                            {
                                isError = true;
                                var errItem = new ImportErrorItem(import, e.Message);
                                errorItems.Add(errItem);
                            }
                        }
                    }
                }
            }

            //All imports handled, update return data
            resultsSet.ErrorItems = errorItems;
            resultsSet.SuccessItems = newRedirects;
            if (errorItems.Any())
            {
                resultsSet.HasErrors = true;
                resultsSet.ErrorMessages.Add("Some Imports failed.");
            }

            return resultsSet;
        }


        //TODO: Update to return ImportResultSet
        public object Import(RedirectsProviderFile file)
        {
            return Import(file, default(CsvImportOptions));
        }

        //TODO: Update to return ImportResultSet
        public object Import(RedirectsProviderFile file, Dictionary<string, string> options)
        {
            return Import(file, CsvImportOptions.FromDictionary(options));
        }

        //TODO: Update to return ImportResultSet
        public object Import(RedirectsProviderFile file, JObject options)
        {
            return Import(file, CsvImportOptions.Parse(options));
        }

        //TODO: Update to return ImportResultSet
        public object Import(RedirectsProviderFile file, CsvImportOptions options)
        {
            return new CsvRedirectsProvider().Import(file, options);
        }

        #endregion

        #region Private/Internal functions

        private RedirectDestination GetDestinationById(int DestinationNodeId, RedirectDestinationType DestinationMode)
        {
            string destinatioName = "";
            Guid destinationKey = Guid.Empty;
            string destinationUrl = "";

            if (DestinationMode == RedirectDestinationType.Content)
            {
                IPublishedContent content = Current.UmbracoContext.Content.GetById(DestinationNodeId);
                if (content != null)
                {
                    destinatioName = content.Name;
                    destinationKey = content.Key;
                    destinationUrl = content.Url;
                }
            }
            else if (DestinationMode == RedirectDestinationType.Media)
            {
                IPublishedContent media = Current.UmbracoContext.Media.GetById(DestinationNodeId);
                if (media != null)
                {
                    destinatioName = media.Name;
                    destinationKey = media.Key;
                    destinationUrl = media.Url;
                }
            }

            return new RedirectDestination(DestinationNodeId, destinationKey, destinationUrl, DestinationMode);
        }


        //TODO: Obsolete?
        internal void Import(RedirectImportItem redirect, CsvImportOptions options)
        {

            // TODO: Use dependency injection
            var service = _redirectsService; //Current.Factory.GetInstance<IRedirectsService>();

            // TODO: Check whether redirect exists

            try
            {
                service.AddRedirect(redirect.AddOptions);
                redirect.Status = RedirectImportStatus.Success;
            }
            catch (Exception ex)
            {
                redirect.Errors.Add(ex.Message);
                redirect.Status = RedirectImportStatus.Failed;
            }

        }

        #endregion

    }

}