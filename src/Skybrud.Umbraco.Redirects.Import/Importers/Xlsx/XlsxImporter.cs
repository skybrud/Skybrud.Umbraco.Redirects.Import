using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Skybrud.Umbraco.Redirects.Import.Exporters.Xlsx;
using Skybrud.Umbraco.Redirects.Import.Models;

namespace Skybrud.Umbraco.Redirects.Import.Importers.Xlsx {

    public class XlsxImporter : ImporterBase<XlsxImportOptions, XlsxImportResult> {

        private readonly RedirectsImportService _redirectsImportService;

        #region Constructors

        public XlsxImporter(RedirectsImportService redirectsImportService) {
            _redirectsImportService = redirectsImportService;
            Icon = "icon-redirects-excel";
            Name = "XLSX";
            Description = "Lets you import redirects from an XLSX file.";
        }

        #endregion

        #region Member methods

        public XlsxExportResult Export(XlsxExportOptions options) {

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

        public override IEnumerable<Option> GetOptions(HttpRequest request) {

            return new [] {
                RedirectsImportUtils.GetOverwriteOption(),
                RedirectsImportUtils.GetFileOption("Select the XSLT file containing the redirects.")
            };

        }

        public override XlsxImportResult Import(XlsxImportOptions options) {

            if (options == null) throw new ArgumentNullException(nameof(options));

            List<string> errors = new();

            if (options.File == null) {
                errors.Add("No file was uploaded.");
                return XlsxImportResult.Failed(errors);
            }

            if (options.File.ContentType != RedirectsImportConstants.ContentTypes.Xlsx || Path.GetExtension(options.File.FileName).ToLowerInvariant() != ".xlsx") {
                errors.Add("Uploaded file doesn't look like a XLSX file.");
                return XlsxImportResult.Failed(errors);
            }

            // Return if we have encountered any errors this far
            if (errors.Any()) return XlsxImportResult.Failed(errors);

            // Create a new stream for the uploaded file
            using Stream stream = options.File.OpenReadStream();

            using XLWorkbook wb = new(stream);
            DataTable table = _redirectsImportService.ToDataTable(wb);

            // Start a new import and return the result
            return new XlsxImportResult(_redirectsImportService.Import(options, table));


        }

        #endregion

    }

}