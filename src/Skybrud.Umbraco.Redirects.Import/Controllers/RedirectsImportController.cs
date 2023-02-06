using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Skybrud.Essentials.Json.Newtonsoft;
using Skybrud.Essentials.Json.Newtonsoft.Extensions;
using Skybrud.Umbraco.Redirects.Import.Exporters;
using Skybrud.Umbraco.Redirects.Import.Importers;
using Umbraco.Cms.Web.BackOffice.Controllers;
using Umbraco.Cms.Web.Common.Attributes;

#pragma warning disable CS1591

namespace Skybrud.Umbraco.Redirects.Import.Controllers {

    [PluginController("Skybrud")]
    public class RedirectsImportController : UmbracoAuthorizedApiController {

        private readonly RedirectsImportService _redirectsImportService;
        private readonly ImporterCollection _importers;
        private readonly ExporterCollection _exporters;

        #region Constructors

        public RedirectsImportController(RedirectsImportService redirectsImportService, ImporterCollection importers, ExporterCollection exporters) {
            _redirectsImportService = redirectsImportService;
            _importers = importers;
            _exporters = exporters;
        }

        #endregion

        [HttpGet]
        public object GetImporters() {

            List<object> importers = new();

            foreach (IImporter importer in _importers) {

                JObject json = JObject.FromObject(importer);

                json["config"] = JArray.FromObject(importer.GetOptions(HttpContext.Request));

                importers.Add(json);

            }

            return importers;

        }

        [HttpGet]
        public object GetExporters() {

            List<object> exporters = new();

            foreach (IExporter exporter in _exporters) {

                JObject json = JObject.FromObject(exporter);

                json["config"] = JArray.FromObject(exporter.GetOptions(HttpContext.Request));

                exporters.Add(json);

            }

            return exporters;

        }

        [HttpPost]
        public object Export(JObject body) {

            // Get a reference to the selected exporter
            string type = body.GetString("type")!;
            if (string.IsNullOrWhiteSpace(type)) return BadRequest("No type specified for selected exporter.");
            if (!_exporters.TryGet(type, out IExporter? exporter)) return BadRequest($"Selected exporter {type} not found.");

            JObject? config = body.GetObject("config");
            if (config == null) return BadRequest("Failed parsing JSON data!!!");

            IExportOptions options = exporter.ParseOptions(config);

            IExportResult result = exporter.Export(options);

            string tempDir = _redirectsImportService.EnsureTempDirectory();
            string tempPath = Path.Combine(tempDir, result.Key + Path.GetExtension(result.FileName));
            System.IO.File.WriteAllBytes(tempPath, result.GetBytes(options));

            return Ok(result);

        }

        [HttpGet]
        public object GetExportedFile(Guid key, string filename) {

            string dir = _redirectsImportService.GetTempDirectoryPath();

            string extension = Path.GetExtension(filename).Trim('.');

            string contentType = _redirectsImportService.GetContentType(extension);

            string path = Path.Combine(dir, $"{key}.{extension}");

            if (!System.IO.File.Exists(path)) return BadRequest("File not found.");

            byte[] bytes = System.IO.File.ReadAllBytes(path);

            System.IO.File.Delete(path);

            return File(bytes, contentType, filename);

        }

        [HttpPost]
        public object Import() {

            if (!TryGetJsonBody(out JObject? body)) return BadRequest("Failed parsing JSON data!!!");

            // Get a reference to the selected importer
            string type = body.GetString("type")!;
            if (!_importers.TryGet(type, out IImporter? importer)) return BadRequest($"Importer '{type}' not found.");

            JObject? config = body.GetObject("config");
            if (config == null) return BadRequest("Configuration object not found in JSON data.");

            IImportOptions options = importer.ParseOptions(config);

            options.File = HttpContext.Request.Form.Files.FirstOrDefault();

            IImportResult result = importer.Import(options);

            return result.IsSuccessful ? Ok(result) : InternalServerError(result);

        }

        private static new ActionResult Ok(object data)  {
            return new ContentResult {
                StatusCode = (int)HttpStatusCode.OK,
                ContentType = "application/json",
                Content = JsonConvert.SerializeObject(data, new JsonSerializerSettings {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                })
            };
        }

        private static new ActionResult BadRequest(object data) {
            return new ContentResult {
                StatusCode = (int)HttpStatusCode.BadRequest,
                ContentType = "application/json",
                Content = JsonConvert.SerializeObject(data, new JsonSerializerSettings {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                })
            };
        }

        private static ActionResult InternalServerError(object data) {
            return new ContentResult {
                StatusCode = (int)HttpStatusCode.InternalServerError,
                ContentType = "application/json",
                Content = JsonConvert.SerializeObject(data, new JsonSerializerSettings {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                })
            };
        }

        private bool TryGetJsonBody([NotNullWhen(true)] out JObject? result) {
            return JsonUtils.TryParseJsonObject(Request.Form["body"].FirstOrDefault()!, out result);
        }

    }

}