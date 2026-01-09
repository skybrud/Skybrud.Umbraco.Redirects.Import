using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Skybrud.Essentials.Json.Newtonsoft;
using Skybrud.Essentials.Json.Newtonsoft.Extensions;
using Skybrud.Essentials.Security.Extensions;
using Skybrud.Umbraco.Redirects.Import.Exporters;
using Skybrud.Umbraco.Redirects.Import.Importers;
using Skybrud.Umbraco.Redirects.Import.Services;
using Umbraco.Cms.Web.Common.Authorization;
using Umbraco.Cms.Web.Common.Routing;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Skybrud.Umbraco.Redirects.Import.Controllers;

[ApiController]
[BackOfficeRoute("skybrud/redirects/import")]
[Authorize(Policy = AuthorizationPolicies.SectionAccessContent)]
[ApiVersion("1.0")]
[ApiExplorerSettings(GroupName = "Skybrud Redirects Import")]
public class RedirectsImportController : Controller {

    private readonly ILogger<RedirectsImportController> _logger;
    private readonly RedirectsImportService _redirectsImportService;
    private readonly ImporterCollection _importerCollection;
    private readonly ExporterCollection _exporterCollection;

    public RedirectsImportController(ILogger<RedirectsImportController> logger, RedirectsImportService redirectsImportService, ImporterCollection importerCollection, ExporterCollection exporterCollection) {
        _logger = logger;
        _redirectsImportService = redirectsImportService;
        _importerCollection = importerCollection;
        _exporterCollection = exporterCollection;
    }

    [HttpGet]
    [Route("serverVariables")]
    public object GetServerVariables() {
        return new {
            version = RedirectsImportPackage.InformationalVersion,
            cacheBuster = RedirectsImportPackage.InformationalVersion.ToMd5Hash()
        };
    }

    [HttpGet("importers")]
    public object GetImporters() {

        List<object> importers = [];

        foreach (IImporter importer in _importerCollection) {

            JObject json = JObject.FromObject(importer);

            json["config"] = JArray.FromObject(importer.GetOptions(HttpContext.Request));

            importers.Add(json);

        }

        return Ok(importers);

    }

    [HttpGet("exporters")]
    public object GetExporters() {

        List<object> exporters = [];

        foreach (IExporter exporter in _exporterCollection) {

            JObject json = JObject.FromObject(exporter);

            json["config"] = JArray.FromObject(exporter.GetOptions(HttpContext.Request));

            exporters.Add(json);

        }

        return Ok(exporters);

    }

    [HttpPost("import")]
    public object Import() {

        if (!TryGetJsonBody(out JObject? body)) return BadRequest("Failed parsing JSON data!!!");

        try {

            // Get a reference to the selected importer
            string type = body.GetString("type")!;
            if (!_importerCollection.TryGet(type, out IImporter? importer)) return BadRequest($"Importer '{type}' not found.");

            JObject? config = body.GetObject("config");
            if (config == null) return BadRequest("Configuration object not found in JSON data.");

            IImportOptions options = importer.ParseOptions(config);

            options.File = HttpContext.Request.Form.Files.FirstOrDefault();

            IImportResult result = importer.Import(options);

            return result.IsSuccessful ? Ok(result) : InternalServerError(result);

        } catch (Exception ex) {

            _logger.LogError(ex, "Failed importing redirects.");

            return InternalServerError(ImportResult.Failed(ex, "Failed importing redirects. Check the Umbraco log for further information or contact your administrator if the problem persists."));

        }

    }

    [HttpPost("export")]
    public async Task<object> Export() {

        JObject? body = await _redirectsImportService.GetJsonBodyAsync(Request);

        try {

            // Get a reference to the selected exporter
            string? type = body.GetString("type");
            if (string.IsNullOrWhiteSpace(type)) return BadRequest("No type specified for selected exporter.");
            if (!_exporterCollection.TryGet(type, out IExporter? exporter)) return BadRequest($"Selected exporter {type} not found.");

            JObject? config = body.GetObject("config");
            if (config == null) return BadRequest("Failed parsing JSON data!!!");

            IExportOptions options = exporter.ParseOptions(config);

            IExportResult result = exporter.Export(options);

            string tempDir = _redirectsImportService.EnsureTempDirectory();
            string tempPath = Path.Combine(tempDir, result.Key + Path.GetExtension(result.FileName));
            await System.IO.File.WriteAllBytesAsync(tempPath, result.GetBytes(options));

            return Ok(result);

        } catch (Exception ex) {

            //_logger.LogError(ex, "Failed exporting redirects.");

            return BadRequest(ex.Message);

            //return InternalServerError(ImportResult.Failed(ex, "Failed exporting redirects. Check the Umbraco log for further information or contact your administrator if the problem persists."));

        }

    }

    [HttpGet("export/{key}/{filename}")]
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

    private static new ActionResult Ok(object data) {
        return new ContentResult {
            StatusCode = (int) HttpStatusCode.OK,
            ContentType = "application/json",
            Content = JsonConvert.SerializeObject(data, new JsonSerializerSettings {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            })
        };
    }

    private static new ActionResult BadRequest(object data) {
        return new ContentResult {
            StatusCode = (int) HttpStatusCode.BadRequest,
            ContentType = "application/json",
            Content = JsonConvert.SerializeObject(data, new JsonSerializerSettings {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            })
        };
    }

    private static ActionResult InternalServerError(object data) {
        return new ContentResult {
            StatusCode = (int) HttpStatusCode.InternalServerError,
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