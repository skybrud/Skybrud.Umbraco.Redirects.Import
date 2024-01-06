using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Skybrud.Csv;
using Skybrud.Umbraco.Redirects.Models;
using Skybrud.Umbraco.Redirects.Services;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Extensions;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Skybrud.Umbraco.Redirects.Import.Models.Export;
using Skybrud.Umbraco.Redirects.Import.Exporters.Csv;
using Skybrud.Umbraco.Redirects.Import.Exporters;

namespace Skybrud.Umbraco.Redirects.Import;

/// <summary>
/// Service class for handling imports and exports.
/// </summary>
public partial class RedirectsImportService {

    private readonly ILogger<RedirectsImportService> _logger;
    private readonly RedirectsImportDependencies _dependencies;
    private readonly IDomainService _domainService;
    private readonly ILocalizationService _localizationService;
    private readonly IMediaService _mediaService;
    private readonly IUmbracoContextAccessor _umbracoContextAccessor;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IRedirectsService _redirectsService;

    #region Constructors

    /// <summary>
    /// Initializes a new instance based on the specified dependencies.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="dependencies"></param>
    public RedirectsImportService(ILogger<RedirectsImportService> logger, RedirectsImportDependencies dependencies) {
        _logger = logger;
        _dependencies = dependencies;
        _domainService = dependencies.DomainService;
        _localizationService = dependencies.LocalizationService;
        _mediaService = dependencies.MediaService;
        _umbracoContextAccessor = dependencies.UmbracoContextAccessor;
        _webHostEnvironment = dependencies.WebHostEnvironment;
        _redirectsService = dependencies.RedirectsService;
    }

    #endregion

    #region Member methods

    /// <summary>
    /// Returns the path to the temp directory for this package.
    /// </summary>
    /// <returns>The path to the temp directory.</returns>
    public string GetTempDirectoryPath() {
        return _webHostEnvironment.MapPathContentRoot($"{Constants.SystemDirectories.TempData}/{RedirectsPackage.Alias}");
    }

    /// <summary>
    /// Creates the temp directory for this package if it doesn't already exist.
    /// </summary>
    /// <returns>The path to the temp directory.</returns>
    public string EnsureTempDirectory() {
        string path = _webHostEnvironment.MapPathContentRoot($"{Constants.SystemDirectories.TempData}/{RedirectsPackage.Alias}");
        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        return path;
    }

    /// <summary>
    /// Returns a new <see cref="DataTable"/> containing the redirects matching the specified <paramref name="options"/>.
    /// </summary>
    /// <param name="options">The options describing the export.</param>
    /// <returns>An instance of <see cref="DataTable"/>.</returns>
    public virtual DataTable ExportAsDataTable(IExportOptions options) {

        Dictionary<string, Func<IRedirect, string>> hej = new () {
            {"Id", x => x.Id.ToString()},
            {"Key", x => x.Key.ToString()},
            {"RootKey", x => x.RootKey.ToString()},
            {"Url", x => x.Url.ToString()},
            {"QueryString", x => x.QueryString.ToString()},
            {"DestinationType", x => x.Destination.Type.ToString()},
            {"DestinationId", x => x.Destination.Id.ToString()},
            {"DestinationKey", x => x.Destination.Key.ToString()},
            {"DestinationUrl", x => x.Destination.Url.ToString()},
            {"DestinationQuery", x => x.Destination.Query.ToString()},
            {"DestinationFragment", x => x.Destination.Fragment.ToString()},
            {"DestinationName", x => x.Destination.Name.ToString()},
            {"DestinationCulture", x => x.Destination.Culture ?? string.Empty},
            {"Type", x => x.Type.ToString()},
            {"IsPermanent", x => x.IsPermanent.ToString()},
            {"ForwardQueryString", x => x.ForwardQueryString.ToString()},
            {"CreateDate", x => x.CreateDate.ToString()},
            {"UpdateDate", x => x.UpdateDate.ToString()}
        };

        // Get the value of the "Columns" option (or fallback to the default value)
        ExportColumnList columns = (options as IExportColumnOptions)?.Columns ?? new ExportColumnList();

        // Initialize a new data table
        DataTable table = new ("Redirects");

        // Append select columns to the data table
        foreach (ExportColumnItem column in columns) {
            if (!column.IsSelected) continue;
            if (!hej.ContainsKey(column.Alias)) continue;
            table.Columns.Add(column.Alias);
        }

        // Append each redirect as individual rows to the data table
        foreach (IRedirect redirect in GetRedirects(options)) {
            DataRow row = table.Rows.Add();
            foreach (ExportColumnItem column in columns) {
                if (!column.IsSelected) continue;
                if (!hej.TryGetValue(column.Alias, out var hest)) continue;
                row[column.Alias] = hest(redirect);
            }
        }

        return table;

    }

    /// <summary>
    /// Returns a list of all redirects matching the specified <paramref name="options"/>.
    /// </summary>
    /// <returns>A collection of <see cref="IRedirect"/> representing the matched redirects.</returns>
    public IEnumerable<IRedirect> GetRedirects(IExportOptions options) {
        return _redirectsService.GetAllRedirects();
    }

    /// <summary>
    /// Returns a new <see cref="CsvFile"/> containing the redirects identified by the specified <paramref name="options"/>.
    /// </summary>
    /// <param name="options">The options describing the export.</param>
    /// <returns>An instance of <see cref="CsvFile"/>.</returns>
    public virtual CsvFile ExportAsCsv(IExportOptions options) {

        // Set default options for the CSV file
        CsvSeparator separator = CsvSeparator.SemiColon;
        Encoding encoding = Encoding.UTF8;

        // Get the options from the 'CsvExportOptions' if the type matches
        if (options is CsvExportOptions o) {
            separator = o.Separator;
            encoding = GetEncoding(o.Encoding);
        }

        // Export all redirects to a new data table intermediary
        DataTable table = ExportAsDataTable(options);

        // Initialize a new CSV file
        CsvFile file = new(separator, encoding);

        // Append all columns from the data table
        foreach (DataColumn column in table.Columns) {
            file.AddColumn(column.ColumnName);
        }

        // Append the rows and cells from the data table
        foreach (DataRow dr in table.Rows) {
            CsvRow row = file.AddRow();
            foreach (DataColumn column in table.Columns) {
                row.AddCell((string)dr[column.ColumnName]);
            }
        }

        return file;

    }

    /// <summary>
    /// Returns the content type based on the specified file <paramref name="extension"/>.
    /// </summary>
    /// <param name="extension">The file extension.</param>
    /// <returns>The content type.</returns>
    public virtual string GetContentType(string? extension) {
        return extension?.Trim('.') switch {
            "csv" => "text/csv",
            "json" => "application/json",
            "xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            _ => throw new Exception($"Unsupported extension '{extension}'.")
        };
    }

    /// <summary>
    /// Returns the <see cref="Encoding"/> matching the specified <paramref name="encoding"/>.
    /// </summary>
    /// <param name="encoding">The enum value representing the encoding.</param>
    /// <returns>An instance of <see cref="Encoding"/>.</returns>
    protected virtual Encoding GetEncoding(CsvExportEncoding encoding) {
        return encoding switch {
            CsvExportEncoding.Ascii => Encoding.ASCII,
            CsvExportEncoding.Utf8 => Encoding.UTF8,
            CsvExportEncoding.Windows1252 => Encoding.GetEncoding(1252),
            _ => throw new Exception($"Unsupported encoding '{encoding}'.")
        };
    }

    #endregion

}