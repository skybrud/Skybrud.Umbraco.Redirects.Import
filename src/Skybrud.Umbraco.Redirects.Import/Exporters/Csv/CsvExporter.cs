using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Skybrud.Csv;
using Skybrud.Umbraco.Redirects.Import.Models;

namespace Skybrud.Umbraco.Redirects.Import.Exporters.Csv {

    public class CsvExporter : ExporterBase<CsvExportOptions, CsvExportResult> {

        private readonly RedirectsImportService _redirectsImportService;

        #region Constructors

        public CsvExporter(RedirectsImportService redirectsImportService) {
            _redirectsImportService = redirectsImportService;
            Icon = "icon-redirects-csv";
            Name = "CSV";
            Description = "Lets you export redirects to a CSV file.";
        }

        #endregion

        #region Member methods

        public override IEnumerable<Option> GetOptions(HttpRequest request) {

            return new List<Option> {
                new() {
                    Alias = "encoding",
                    Label = "Encoding",
                    Description = "Select the encoding of the CSV file.",
                    View = $"{RedirectsImportPackage.AppPlugins}Views/Editors/Items.html?v={RedirectsPackage.Version}",
                    Value = "utf8",
                    Config = new Dictionary<string, object> {
                        {"items", new [] {
                            new Item { Alias = "ascii", Name = "Ascii" },
                            new Item { Alias = "utf8", Name = "UTF-8" },
                            new Item { Alias = "windows1252", Name = "Windows 1252" }
                        }}
                    }
                },
                new() {
                    Alias = "separator",
                    Label = "Separator",
                    Description = "Select the separator to be used in the exported CSV file.",
                    View = $"{RedirectsImportPackage.AppPlugins}Views/Editors/Items.html?v={RedirectsPackage.Version}",
                    Value = "semicolon",
                    Config = new Dictionary<string, object> {
                        {"items", new [] {
                            new Item { Alias = "default", Name = "Default" },
                            new Item { Alias = "colon", Name = "Colon" },
                            new Item { Alias = "semicolon", Name = "Semi colon" },
                            new Item { Alias = "space", Name = "Space" },
                            new Item { Alias = "Tab", Name = "Tab" }
                        }}
                    }
                },
                RedirectsImportUtils.GetColumnsOption()
            };

        }

        public override CsvExportResult Export(CsvExportOptions options) {

            if (options == null) throw new ArgumentNullException(nameof(options));

            CsvFile file = _redirectsImportService.ExportAsCsv(options);

            return new CsvExportResult(Guid.NewGuid(), file);

        }

        #endregion

    }

}