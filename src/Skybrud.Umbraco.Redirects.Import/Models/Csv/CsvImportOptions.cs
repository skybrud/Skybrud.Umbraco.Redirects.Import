using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Skybrud.Essentials.Json.Extensions;
using Skybrud.Essentials.Strings;

namespace Skybrud.Umbraco.Redirects.Import.Models.Csv {

    /// <summary>
    /// Class with options for import one or more redirects from a CSV file.
    /// </summary>
    public class CsvImportOptions : IImportOptions {

        #region Properties
        
        /// <summary>
        /// Gets or sets whether existing redirects should be overwritten for matching inbound URLs.
        /// </summary>
        public bool OverwriteExisting { get; set; }

        /// <summary>
        /// Gets or sets the encoding to be used.
        /// </summary>
        public CsvImportEncoding Encoding { get; set; }

        #endregion

        public static CsvImportOptions Parse(JObject obj) {

            CsvImportOptions options = new CsvImportOptions();

            string encodingName = (obj.GetString("encoding") ?? "").ToLowerInvariant();

            switch (encodingName) {
                case "utf8":
                case "utf-8":
                    options.Encoding = CsvImportEncoding.Utf8;
                    break;
                case "windows1252":
                case "windows-1252":
                    options.Encoding = CsvImportEncoding.Windows1252;
                    break;
                case "ascii":
                    options.Encoding = CsvImportEncoding.Ascii;
                    break;
                default:
                    options.Encoding = CsvImportEncoding.Auto;
                    break;
            }

            options.OverwriteExisting = obj.GetBoolean("overwrite");

            return options;

        }

        public static CsvImportOptions FromDictionary(Dictionary<string, string> dictionary) {

            CsvImportOptions options = new CsvImportOptions();

            if (dictionary.TryGetValue("encoding", out string encodingName)) {
                switch (encodingName.ToLowerInvariant()) {
                    case "utf8":
                    case "utf-8":
                        options.Encoding = CsvImportEncoding.Utf8;
                        break;
                    case "windows1252":
                    case "windows-1252":
                        options.Encoding = CsvImportEncoding.Windows1252;
                        break;
                    case "ascii":
                        options.Encoding = CsvImportEncoding.Ascii;
                        break;
                    default:
                        options.Encoding = CsvImportEncoding.Auto;
                        break;
                }
            }

            if (dictionary.TryGetValue("overwrite", out string overwrite)) {
                options.OverwriteExisting = StringUtils.ParseBoolean(overwrite);
            }

            return options;

        }

    }

}