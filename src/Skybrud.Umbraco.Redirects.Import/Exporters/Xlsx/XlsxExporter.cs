using System;
using System.Collections.Generic;
using System.IO;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Skybrud.Umbraco.Redirects.Import.Models;

namespace Skybrud.Umbraco.Redirects.Import.Exporters.Xlsx {

    public class XlsxExporter : ExporterBase<XlsxExportOptions, XlsxExportResult> {

        private readonly RedirectsImportService _redirectsImportService;

        #region Constructors

        public XlsxExporter(RedirectsImportService redirectsImportService) {
            _redirectsImportService = redirectsImportService;
            Icon = "icon-redirects-excel";
            Name = "XLSX";
            Description = "Lets you export redirects to an XLSX file.";
        }

        #endregion

        #region Member methods

        public override IEnumerable<Option> GetOptions(HttpRequest request) {
            return new[] {
                new Option {
                    Alias = "columns",
                    Label = "Columns",
                    Description = "Select the columns that should be included in the exported file.",
                    View = $"{RedirectsImportPackage.AppPlugins}?v={RedirectsPackage.Version}"
                }
            };
        }

        public override XlsxExportResult Export(XlsxExportOptions options) {

            byte[] bytes;

            // Initialize a new workbook
            using (XLWorkbook workbook = new()) {

                // Add a new sheet to the workbook based on the data table
                IXLWorksheet worksheet = workbook.Worksheets.Add(_redirectsImportService.ExportAsDataTable(options));

                // Adjust column sizes
                worksheet.Columns().AdjustToContents();

                // Convert the workbook to a byte array
                using (MemoryStream ms = new()) {
                    workbook.SaveAs(ms);
                    bytes = ms.ToArray();
                }

            }

            // Return the result
            return new XlsxExportResult(Guid.NewGuid(), bytes);

        }

        #endregion

    }

}