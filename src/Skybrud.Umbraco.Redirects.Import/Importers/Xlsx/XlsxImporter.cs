using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Skybrud.Umbraco.Redirects.Import.Models;

namespace Skybrud.Umbraco.Redirects.Import.Importers.Xlsx {

    /// <summary>
    /// Class representing an importer based on an <strong>XLSX</strong> file.
    /// </summary>
    public class XlsxImporter : ImporterBase<XlsxImportOptions, XlsxImportResult> {

        private readonly RedirectsImportService _redirectsImportService;

        #region Constructors

        /// <summary>
        /// Initializes a new instance based on the specified dependencies.
        /// </summary>
        public XlsxImporter(RedirectsImportService redirectsImportService) {
            _redirectsImportService = redirectsImportService;
            Icon = "icon-redirects-excel";
            Name = "XLSX";
            Description = "Lets you import redirects from an XLSX file.";
        }

        #endregion

        #region Member methods

        /// <summary>
        /// Returns a collection with the options for the <strong>Options</strong> step in the import process.
        /// </summary>
        /// <param name="request">A reference to current request.</param>
        /// <returns>A collection of <see cref="Option"/> representing the options.</returns>
        public override IEnumerable<Option> GetOptions(HttpRequest request) {

            return new [] {
                RedirectsImportUtils.GetOverwriteOption(),
                RedirectsImportUtils.GetFileOption("Select the XSLT file containing the redirects.")
            };

        }

        /// <summary>
        /// Performs a new import based on the specified <paramref name="options"/>.
        /// </summary>
        /// <param name="options">The options describing the import.</param>
        /// <returns>An instance of <see cref="XlsxImportResult"/> representing the result of the import.</returns>
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