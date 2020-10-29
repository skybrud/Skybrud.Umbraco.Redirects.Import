namespace Skybrud.Umbraco.Redirects.Import.Controllers.WebApi
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;
    using Dragonfly.NetModels;
    using Skybrud.Umbraco.Redirects.Import.Models;
    using Skybrud.Umbraco.Redirects.Import.Utilities;
    using global::Umbraco.Web.WebApi;
    using Newtonsoft.Json;
    using Skybrud.Umbraco.Redirects.Import.Models.Csv;
    using Skybrud.Umbraco.Redirects.Import.Services;
    using Skybrud.Umbraco.Redirects.Models;
    using Skybrud.Umbraco.Redirects.Models.Options;
    using Constants = Skybrud.Umbraco.Redirects.Import.Constants;


    // [IsBackOffice]
    // GET: /Umbraco/backoffice/Api/ImportWebApi <-- UmbracoAuthorizedApiController

    [IsBackOffice]
    public partial class ImportWebApiController : UmbracoAuthorizedApiController
    {
        private readonly IRedirectsService _redirectsService;

        public ImportWebApiController(IRedirectsService SkybrudRedirectsService)
        {
            _redirectsService = SkybrudRedirectsService;
        }


        /// /Umbraco/backoffice/Api/ImportWebApi/ImportRedirects
        [System.Web.Http.HttpGet]
        public HttpResponseMessage ImportRedirectsTest()
        {
            var returnStatusMsg = new StatusMessage(true); //assume success

            //Default Redirect Settings
            var defaults = new DefaultImportOptions();
            defaults.SiteRootNode = 0;
            defaults.ForwardQueryString = false;
            defaults.Type = Constants.RedirectType.Temporary;

            //Set CSV Provider + options
            IRedirectsImportExportProvider csvProvider = new CsvRedirectsProvider();
            var csvOptions = new CsvImportOptions();
            csvOptions.Encoding = CsvImportEncoding.Auto;
            csvOptions.OverwriteExisting = false;

            //Test Data File
            var testImportFile = $"{Constants.FileUploadPath}Demo1.csv";
            RedirectsProviderFile importFile = null;
            try
            {
                importFile = new RedirectsProviderFile(testImportFile);
            }
            catch (Exception e)
            {
                returnStatusMsg.Success = false;
                returnStatusMsg.RelatedException = e;
                returnStatusMsg.Message = $"Unable to get a valid file from '{testImportFile}'.";
            }

            if (importFile != null)
            {
                //Call Service for Results
                var importerService = new RedirectsImportService(_redirectsService, Umbraco);
                var results = importerService.Import(importFile, defaults, csvProvider);
                returnStatusMsg.RelatedObject = results;

                if (results.HasErrors)
                {
                    returnStatusMsg.Message = "Some Imports failed.";
                }
            }



            //RETURN AS JSON
            string json = JsonConvert.SerializeObject(returnStatusMsg);
            
            return new HttpResponseMessage()
            {
                Content = new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json"
                )
            };
        }

        
        }
}
