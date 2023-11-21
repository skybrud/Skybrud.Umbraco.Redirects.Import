﻿using System.Collections.Generic;

namespace Skybrud.Umbraco.Redirects.Import {

    /// <summary>
    /// Static class with various constants used throughout this package.
    /// </summary>
    public static class RedirectsImportConstants {

        /// <summary>
        /// Static class with constants for various content types.
        /// </summary>
        public static class ContentTypes {


            /// <summary>
            /// Gets a list of the allowed content types for CSV files
            /// </summary>
            public static readonly IList<string> AllowedCsvContentTypes = new[] { "application/vnd.ms-excel", "text/csv" };

            /// <summary>
            /// Gets the content type for a <strong>CSV</strong> file.
            /// </summary>
            public const string Csv = "text/csv";

            /// <summary>
            /// Gets the content type for a <strong>JSON</strong> file.
            /// </summary>
            public const string Json = "application/json";

            /// <summary>
            /// Gets the content type for a <strong>XLSX</strong> file.
            /// </summary>
            public const string Xlsx = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        }

    }

}