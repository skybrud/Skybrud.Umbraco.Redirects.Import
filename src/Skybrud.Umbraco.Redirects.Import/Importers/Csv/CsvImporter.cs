using Microsoft.AspNetCore.Http;
using Skybrud.Csv;
using Skybrud.Umbraco.Redirects.Exceptions;
using Skybrud.Umbraco.Redirects.Import.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace Skybrud.Umbraco.Redirects.Import.Importers.Csv {

    public class CsvImporter : ImporterBase<CsvImportOptions, CsvImportResult> {

        private readonly RedirectsImportService _redirectsImportService;

        #region Constructors

        public CsvImporter(RedirectsImportService redirectsImportService) {

            _redirectsImportService = redirectsImportService;

            Icon = "icon-redirects-csv";
            Name = "CSV";
            Description = "Lets you import redirects from a CSV file.";

        }

        #endregion

        #region Member methods

        public override IEnumerable<Option> GetOptions(HttpRequest request) {

            return new Option[] {
                new () {
                    Alias = "overwriteExisting",
                    Label = "Overwrite existing",
                    Description = "Indicates whether existing redirects should be overwritten for matching inbound URLs.",
                    View = "boolean"
                },
                new () {
                    Alias = "encoding",
                    Label = "Encoding",
                    Description = "Select the encoding of the uploaded CSV file.",
                    View = "/App_Plugins/Skybrud.Umbraco.Redirects.Import/Views/Editors/Items.html",
                    Config = new Dictionary<string, object> {
                        {"items", GetEncodings()}
                    }
                },
                new () {
                    Alias = "separator",
                    Label = "Separator",
                    Description = "Select the separator used in the uploaded CSV file.",
                    View = "/App_Plugins/Skybrud.Umbraco.Redirects.Import/Views/Editors/Items.html",
                    Config = new Dictionary<string, object> {
                        {"items", new [] {
                            new Item { Alias = "Auto", Name = "Auto" },
                            new Item { Alias = "Colon", Name = "Colon" },
                            new Item { Alias = "Comma", Name = "Comma" },
                            new Item { Alias = "SemiColon", Name = "Semi colon" },
                            new Item { Alias = "Space", Name = "Space" },
                            new Item { Alias = "Tab", Name = "Tab" }
                        }}
                    }
                },
                new () {
                    Alias = "file",
                    Label = "File",
                    Description = "Select the CSV file.",
                    View = "/App_Plugins/Skybrud.Umbraco.Redirects.Import/Views/Editors/File.html",
                    Config = new Dictionary<string, object> {
                        {"multiple", false}
                    }
                }
            };

        }

        public override CsvImportResult Import(CsvImportOptions options) {

            if (options == null) throw new ArgumentNullException(nameof(options));

            List<string> errors = new();

            if (options.File == null) {
                errors.Add("No file was uploaded.");
                return CsvImportResult.Failed(errors);
            }

            if (options.File.ContentType != "text/csv" || Path.GetExtension(options.File.FileName).ToLowerInvariant() != ".csv") {
                errors.Add("Uploaded file doesn't look like a CSV file.");
                return CsvImportResult.Failed(errors);
            }

            // Return if we have encountered any errors this far
            if (errors.Any()) return CsvImportResult.Failed(errors);

            // Create a new stream for the uploaded file
            using Stream stream = options.File.OpenReadStream();

            // Determine the encoding
            Encoding encoding = GetEncoding(options);

            // Determine the separator
            CsvSeparator separator = GetSeparator(options);

            // Load the CSV file
            CsvFile file = CsvFile.Load(stream, separator, encoding);

            // TODO: Validate the CSV file a bit

            // Convert the CSV file to a data table
            DataTable dataTable = file.ToDataTable();

            // Start a new import based on the data table
            var result = _redirectsImportService.Import(options, dataTable);

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

        private Encoding GetEncoding(CsvImportOptions options) {
            return options.Encoding switch {
                CsvImportEncoding.Ascii => Encoding.ASCII,
                CsvImportEncoding.Utf8 => Encoding.UTF8,
                CsvImportEncoding.Windows1252 => Encoding.GetEncoding(1252),
                CsvImportEncoding.Auto => null,
                _ => throw new RedirectsException($"Unsupported encoding: {options.Encoding}")
            };
        }

        protected virtual IReadOnlyList<CsvImportEncodingItem> GetEncodings() {

            var temp = new List<CsvImportEncodingItem> {
                new ("Auto", "Auto"),
                new ("Ascii", "Ascii")
            };

            if (TryGetEncoding("utf-8", out Encoding _)) {
                temp.Add(new CsvImportEncodingItem("utf-8", "Unicode (UTF-8)"));
            }

            if (TryGetEncoding("Windows-1252", out Encoding _)) {
                temp.Add(new CsvImportEncodingItem("windows1252", "Windows 1252"));
            }

            if (TryGetEncoding("iso-8859-1", out Encoding _)) {
                temp.Add(new CsvImportEncodingItem("iso-8859-1", "Western European (ISO)"));
            }

            return temp;

        }

        protected bool TryGetEncoding(string alias, out Encoding result) {

            try {
                result = Encoding.GetEncoding(alias);
                return result != null;
            } catch {
                result = null;
                return false;
            }

        }

        #endregion

    }

}