using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using Skybrud.Umbraco.Redirects.Import.Models;
using Skybrud.Umbraco.Redirects.Import.Models.Csv;
using Skybrud.Umbraco.Redirects.Import.Services;
using Skybrud.WebApi.Json;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;

namespace Skybrud.Umbraco.Redirects.Import.Controllers
{
    using Skybrud.Umbraco.Redirects.Models;

    [JsonOnlyConfiguration]
    [PluginController("Skybrud")]
    public class RedirectsImportController : UmbracoAuthorizedApiController
    {

        private readonly IRedirectsService _redirectsService;

        public RedirectsImportController(IRedirectsService RedirectsService)
        {
            _redirectsService = RedirectsService;
        }

        [HttpGet]
        public object GetProviders()
        {

            return new[] {
                new CsvRedirectsProvider()
            };

        }


        [HttpPost]
        public object Upload()
        {
            HttpPostedFile file = HttpContext.Current.Request.Files["file"];

            if (file == null) return Request.CreateResponse(HttpStatusCode.BadRequest, "No file specified.");

            if (file.ContentType != "application/octet-stream" || !file.FileName.ToLower().EndsWith(".csv")) return Request.CreateResponse(HttpStatusCode.BadRequest, "Specified file must be a CSV file.");

            RedirectsProviderFile pfile = new RedirectsProviderFile(new HttpPostedFileWrapper(file));

            Dictionary<string, string> body = new Dictionary<string, string>();
            foreach (string key in HttpContext.Current.Request.Form.Keys)
            {
                body[key] = HttpContext.Current.Request.Form[key];
            }

            return new RedirectsImportService(_redirectsService, Umbraco).Import(pfile, CsvImportOptions.FromDictionary(body));
        }

    }

}