using Microsoft.AspNetCore.Http;
using Skybrud.Csv;
using Skybrud.Umbraco.Redirects.Exceptions;
using Skybrud.Umbraco.Redirects.Import.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Options;
using Skybrud.Umbraco.Redirects.Import.Config;

namespace Skybrud.Umbraco.Redirects.Import.Importers.Csv {

    /// <summary>
    /// Class representing an importer based on an <strong>CSV</strong> file.
    /// </summary>
    public class CsvImporter : ImporterBase<CsvImportOptions, CsvImportResult> {

        private readonly RedirectsImportSettings _redirectsImportSettings;
        private readonly RedirectsImportService _redirectsImportService;

        #region Constructors

        /// <summary>
        /// Initializes a new instance based on the specified dependencies.
        /// </summary>
        public CsvImporter(IOptions<RedirectsImportSettings> redirectsImportSettings, RedirectsImportService redirectsImportService) {
            _redirectsImportSettings = redirectsImportSettings.Value;
            _redirectsImportService = redirectsImportService;
            Icon = "icon-redirects-csv";
            Name = "CSV";
            Description = "Lets you import redirects from a CSV file.";
        }

        #endregion

        #region Member methods

        /// <summary>
        /// Returns a collection with the options for the <strong>Options</strong> step in the import process.
        /// </summary>
        /// <param name="request">A reference to current request.</param>
        /// <returns>A collection of <see cref="Option"/> representing the options.</returns>
        public override IEnumerable<Option> GetOptions(HttpRequest request) {

            const string itemsUrl = "/App_Plugins/Skybrud.Umbraco.Redirects.Import/Views/Editors/Items.html";
            const string fileUrl = "/App_Plugins/Skybrud.Umbraco.Redirects.Import/Views/Editors/File.html";

            return new Option[] {
                new ("overwriteExisting", "Overwrite existing", "boolean", "Indicates whether existing redirects should be overwritten for matching inbound URLs."),
                new ("encoding", "Encoding", itemsUrl, "Select the encoding of the uploaded CSV file.") {
                    Config = new Dictionary<string, object> {
                        {"items", GetEncodings()}
                    }
                },
                new ("separator", "Separator", itemsUrl, "Select the separator used in the uploaded CSV file.") {
                    Config = new Dictionary<string, object> {
                        {"items", new [] {
                            new Item("Auto", "Auto"),
                            new Item("Colon", "Colon"),
                            new Item("Comma", "Comma"),
                            new Item("SemiColon", "Semi colon"),
                            new Item("Space", "Space"),
                            new Item("Tab", "Tab")
                        }}
                    }
                },
                new ("file", "File", fileUrl, "Select the CSV file.") {
                    Config = new Dictionary<string, object> {
                        {"multiple", false}
                    }
                }
            };

        }

        /// <summary>
        /// Performs a new import based on the specified <paramref name="options"/>.
        /// </summary>
        /// <param name="options">The options describing the import.</param>
        /// <returns>An instance of <see cref="CsvImportResult"/> representing the result of the import.</returns>
        public override CsvImportResult Import(CsvImportOptions options) {

            if (options == null) throw new ArgumentNullException(nameof(options));

            List<string> errors = new();

            if (options.File == null) {
                errors.Add("No file was uploaded.");
                return CsvImportResult.Failed(errors);
            }

            if (!_redirectsImportSettings.Csv.AllowedContentTypes.Contains(options.File.ContentType) || Path.GetExtension(options.File.FileName).ToLowerInvariant() != ".csv") {
                errors.Add("Uploaded file doesn't look like a CSV file.");
                return CsvImportResult.Failed(errors);
            }

            // Return if we have encountered any errors this far
            if (errors.Any()) return CsvImportResult.Failed(errors);

            // Create a new stream for the uploaded file
            using Stream stream = options.File.OpenReadStream();

            // Determine the encoding
            Encoding? encoding = GetEncoding(options);

            // Determine the separator
            CsvSeparator separator = GetSeparator(options);

            // Load the CSV file
            CsvFile file = CsvFile.Load(stream, separator, encoding);

            // TODO: Validate the CSV file a bit

            // Convert the CSV file to a data table
            DataTable dataTable = file.ToDataTable();

            // Start a new import based on the data table
            ImportResult result = _redirectsImportService.Import(options, dataTable);

            // Wrap the result
            return new CsvImportResult(result);

        }

        private CsvSeparator GetSeparator(CsvImportOptions options) {
            return options.Separator switch {
                CsvImportSeparator.Auto => CsvSeparator.Auto,
                CsvImportSeparator.Colon => CsvSeparator.Colon,
                CsvImportSeparator.Comma => CsvSeparator.Comma,
                CsvImportSeparator.SemiColon => CsvSeparator.SemiColon,
                CsvImportSeparator.Space => CsvSeparator.Space,
                CsvImportSeparator.Tab => CsvSeparator.Tab,
                _ => throw new RedirectsException($"Unsupported separator: {options.Separator}")
            };
        }

        private Encoding? GetEncoding(CsvImportOptions options) {
            return options.Encoding switch {
                CsvImportEncoding.Ascii => Encoding.ASCII,
                CsvImportEncoding.Utf8 => Encoding.UTF8,
                CsvImportEncoding.Windows1252 => Encoding.GetEncoding(1252),
                CsvImportEncoding.Auto => null,
                _ => throw new RedirectsException($"Unsupported encoding: {options.Encoding}")
            };
        }

        /// <summary>
        /// Returns a list of available encodings.
        /// </summary>
        /// <returns>A list of <see cref="CsvImportEncodingItem"/>.</returns>
        protected virtual IReadOnlyList<CsvImportEncodingItem> GetEncodings() {

            var temp = new List<CsvImportEncodingItem> {
                new ("Auto", "Auto"),
                new ("Ascii", "Ascii")
            };

            if (TryGetEncoding("utf-8", out Encoding? _)) {
                temp.Add(new CsvImportEncodingItem("utf-8", "Unicode (UTF-8)"));
            }

            if (TryGetEncoding("Windows-1252", out Encoding? _)) {
                temp.Add(new CsvImportEncodingItem("windows1252", "Windows 1252"));
            }

            if (TryGetEncoding("iso-8859-1", out Encoding? _)) {
                temp.Add(new CsvImportEncodingItem("iso-8859-1", "Western European (ISO)"));
            }

            return temp;

        }

        /// <summary>
        /// Attempts to get the encoding with the specified <paramref name="alias"/>.
        /// </summary>
        /// <param name="alias">The alias of the encoding.</param>
        /// <param name="result">When this method returns, holds an instance <see cref="Encoding"/> representing the matched encoding if successful; otherwise, <see langword="null"/>.</param>
        /// <returns><see langword="true"/> if successful; otherwise, <see langword="false"/>.</returns>
        protected bool TryGetEncoding(string alias, [NotNullWhen(true)] out Encoding? result) {

            try {
                result = Encoding.GetEncoding(alias);
                return true;
            } catch {
                result = null;
                return false;
            }

        }

        #endregion

    }

}