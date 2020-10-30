using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Skybrud.Csv;
using Skybrud.Essentials.Enums;
using Skybrud.Umbraco.Redirects.Exceptions;
using Skybrud.Umbraco.Redirects.Import.Models.Csv;
using Skybrud.Umbraco.Redirects.Import.Services;
using Skybrud.Umbraco.Redirects.Models;
using Skybrud.Umbraco.Redirects.Models.Options;
using Umbraco.Core.IO;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.PropertyEditors;
using Umbraco.Web.Composing;

namespace Skybrud.Umbraco.Redirects.Import.Models.Csv
{
    using System.Linq;
    using LINQtoCSV;

    public class CsvRedirectsProvider : IRedirectsImportExportProvider
    {

        #region Properties

        /// <summary>
        /// Gets the name of the provider.
        /// </summary>
        [JsonProperty("name")]
        public string Name => "CSV";

        /// <summary>
        /// Gets the description of the provider.
        /// </summary>
        [JsonProperty("description")]
        public string Description => "Lets you import from and export to CSV files.";

        /// <summary>
        /// Gets whether this provider supports importing redirects.
        /// </summary>
        [JsonProperty("canImport")]
        public bool CanImport => true;

        /// <summary>
        /// Gets whether this provider supports exporting redirects.
        /// </summary>
        [JsonProperty("canExport")]
        public bool CanExport => true;

        [JsonProperty("fields")]
        public IEnumerable<ConfigurationField> Fields => new[] {
            new ConfigurationField {
                Key = "overwriteExisting",
                Name = "Overwrite existing",
                Description = "Indidates whether existing redirects should be overwritten for matching inbound URLs.",
                View = "boolean"
            },
            new ConfigurationField {
                Key = "encoding",
                Name = "Encoding",
                Description = "Select the encoding of the uploaded CSV file.",
                View = "/App_Plugins/Skybrud.Umbraco.Redirects.Import/Views/Editors/Items.html",
                Config = new Dictionary<string, object> {
                    {"items", new [] {
                        new Item { Alias = "auto", Name = "Auto" },
                        new Item { Alias = "ascii", Name = "Ascii" },
                        new Item { Alias = "utf8", Name = "UTF-8" },
                        new Item { Alias = "windows1252", Name = "Windows 1252" }
                    }}
                }
            },
            new ConfigurationField {
                Key = "separator",
                Name = "Separator",
                Description = "Select the separator used in the uploaded CSV file.",
                View = "/App_Plugins/Skybrud.Umbraco.Redirects.Import/Views/Editors/Items.html",
                Config = new Dictionary<string, object> {
                    {"items", new [] {
                        new Item { Alias = "auto", Name = "Auto" },
                        new Item { Alias = "colon", Name = "Colon" },
                        new Item { Alias = "semicolon", Name = "Semi colon" },
                        new Item { Alias = "space", Name = "Space" },
                        new Item { Alias = "Tab", Name = "Tab" }
                    }}
                }
            },
            new ConfigurationField {
                Key = "file",
                Name = "File",
                Description = "Select the CSV file.",
                View = "/App_Plugins/Skybrud.Umbraco.Redirects.Import/Views/Editors/File.html",
                Config = new Dictionary<string, object> {
                    {"multiple", false}
                }
            }
        };

        public class Item
        {



            [JsonProperty("alias")]
            public string Alias { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

        }

        #endregion

        public IImportOptions ParseImportSettings(JObject obj)
        {
            return CsvImportOptions.Parse(obj);
        }

        IImportOptions IRedirectsImportExportProvider.ParseImportSettings(Dictionary<string, string> dictionary)
        {
            return ParseImportSettings(dictionary);
        }

        public CsvImportOptions ParseImportSettings(Dictionary<string, string> dictionary)
        {
            return CsvImportOptions.FromDictionary(dictionary);
        }


        public ImportDataSet Import(RedirectsProviderFile file, IImportOptions options)
        {

            if (file == null) throw new ArgumentNullException(nameof(file));
            if (options == null) throw new ArgumentNullException(nameof(options));

            if (!(options is CsvImportOptions)) throw new ArgumentException("Must be an instance of CsvImportOptions", nameof(options));

            return Import(file, (CsvImportOptions)options);

        }

        public ImportDataSet Import(RedirectsProviderFile file, CsvImportOptions options)
        {
            if (file == null) throw new ArgumentNullException(nameof(file));
            if (options == null) throw new ArgumentNullException(nameof(options));

            // Determine the encoding
            Encoding encoding;
            switch (options.Encoding)
            {
                case CsvImportEncoding.Ascii:
                    encoding = Encoding.ASCII;
                    break;

                case CsvImportEncoding.Utf8:
                    encoding = Encoding.UTF8;
                    break;

                case CsvImportEncoding.Windows1252:
                    encoding = Encoding.GetEncoding(1252);
                    break;

                default:
                    //encoding = Encoding.GetEncoding(1252);
                    using (var reader = new StreamReader(file.InputStream, Encoding.Default, true))
                    {
                        reader.Peek(); // you need this!
                        encoding = reader.CurrentEncoding;
                    }
                    break;
            }


            //Load CSV file using LinqToCSV
            var csvData = ReadCSV(file);

            //Map Columns
            CsvMap map = new CsvMap(csvData, options);
            MapCsvDataColumns(map);

            // Parse the rows
            var redirects = ParseCsvRows(map);

            return redirects;


            //// Load the CSV file from the stream using Skybrud.Csv
            //CsvFile csv;
            //using (Stream stream = file.InputStream)
            //{
            //    csv = CsvFile.Load(stream, encoding);
            //}

            //CsvInternalImportOptions io = new CsvInternalImportOptions(csv, options);

            //// Determine the columns
            //MapCsvColumns(io);

            //// Parse the rows
            //var redirects = ParseCsvRows(io);
            //return redirects;

        }

        #region Parsing Using LinqToCsv

        private CsvDataSet ReadCSV(RedirectsProviderFile File)
        {
            var dataSet = new CsvDataSet();

            //Load CSV file using LinqToCSV
            CsvFileDescription inputFileDescription = new CsvFileDescription
            {
                SeparatorChar = ',',
                FirstLineHasColumnNames = false //because we need to read them
            };
            CsvContext cc = new CsvContext();
            
            List<CsvDataRow> rows = new List<CsvDataRow>();
            var headers = new Dictionary<int, string>();

            using (var reader = new StreamReader(File.InputStream))
            {
                IEnumerable<CsvDataRow> rowsRead = cc.Read<CsvDataRow>(reader, inputFileDescription);
                var currRow = 0;
                
                foreach (var dataRow in rowsRead)
                {
                    currRow++;

                    var colCount = dataRow.Count;

                    if (currRow == 1)
                    {
                        //Header row
                        for (int i = 0; i < colCount ; i++)
                        {
                            var colNum = i + 1;
                            headers.Add(colNum, dataRow[i].Value);
                        }
                    }
                    else
                    {
                        //Data row - loop columns adding to list
                        var columnData = new List<CsvDataColumn>();
                        for (int i = 0; i < colCount ; i++)
                        {
                            var colNum = i + 1;
                            var colHeaderName = headers[colNum];
                            var col = new CsvDataColumn()
                            {
                                Header = colHeaderName,
                                ColNum = colNum,
                                Value = dataRow[i].Value
                            };
                            columnData.Add(col);
                        }

                        //Create row and add to collection
                        var newRow = new CsvDataRow() { Columns = columnData };
                        rows.Add(newRow);
                    }
                }
            }

            dataSet.Headers = headers;
            dataSet.Rows = rows;
            return dataSet;
        }

        private void MapCsvDataColumns(CsvMap Map)
        {
            var headers = Map.DataSet.Headers;
            var headersQty = headers.Count;

            //for (int i = 0; i < headersQty - 1; i++)
            //{
            //    var headerValue = headerRow[0].Value;
            //    var colName = headerValue.Replace(" ", "").ToLowerInvariant();
            //    fileColumns.Add(colName, i);
            //}

            foreach (var header in headers)
            {
                //test the name and get the matching index
                var colNum = header.Key;
                var colName = header.Value.Replace(" ", "").ToLowerInvariant();
                switch (colName)
                {
                    case "rootnodeid":
                    case "domain":
                        if (Map.ColumnRootNodeId > 0) break;
                        Map.ColumnRootNodeId = colNum;
                        break;

                    case "url":
                    case "inboundurl":
                    case "oldurl":
                    case "old":
                        if (Map.ColumnInboundUrl > 0) break;
                        Map.ColumnInboundUrl = colNum;
                        break;

                    case "linkid":
                    case "destinationid":
                        if (Map.ColumnDestinationId > 0) break;
                        Map.ColumnDestinationId = colNum;
                        break;

                    case "linktype":
                    case "linkmode":
                    case "destinationtype":
                    case "destinationmode":
                        if (Map.ColumnDestinationType > 0) break;
                        Map.ColumnDestinationType = colNum;
                        break;

                    case "linkurl":
                    case "destinationurl":
                    case "newurl":
                    case "new":
                        if (Map.ColumnDestinationUrl > 0) break;
                        Map.ColumnDestinationUrl = colNum;
                        break;
                }
            }

            var colsMsg = $" Columns present in imported file: {string.Join(", ", headers)}";
            //if (options.ColumnRootNodeId == null) throw new RedirectsException("No column found for *root node ID*");
            if (Map.ColumnInboundUrl == 0) throw new RedirectsException("No column found for *inbound URL*." + colsMsg);
            if (Map.ColumnDestinationUrl == 0)
            {
                //check for Dest ID & Type
                if (Map.ColumnDestinationId == 0 || Map.ColumnDestinationType == 0)
                {
                    throw new RedirectsException("You need to either provide the *destination URL* column OR both the*destination ID* and *destination type* columns." + colsMsg);
                }
            }
        }

        private ImportDataSet ParseCsvRows(CsvMap Map)
        {
            var importData = new ImportDataSet();
            var redirects = new List<ImportDataItem>();
            var rowIndex = 0;

            foreach (var row in Map.DataSet.Rows)
            {
                var errors = new List<string>();
                ImportDataItem item = new ImportDataItem();

                //RootNode Id / Key
                if(Map.ColumnRootNodeId>0)
                {
                    var dataVal = GetColumnValue(row, Map.ColumnRootNodeId);
                    string valueRootNodeId =  dataVal != null ? dataVal.ToString() : "";
                    if (int.TryParse(valueRootNodeId, out int rootNodeId))
                    {
                        item.RootNodeId = rootNodeId;
                        IPublishedContent content = Current.UmbracoContext.Content.GetById(rootNodeId);
                        if (content != null) item.RootNodeKey = content.Key;
                    }
                    else
                    {
                        // TODO: Should we validate the domain? Any security concerns about using the input value?
                        IDomain domain = Current.Services.DomainService.GetByName(dataVal.ToString());
                        if (domain == null)
                        {
                            errors.Add($"Row {rowIndex}: Unknown root node ID or domain: " + dataVal.ToString());
                        }
                        else if (domain.RootContentId == null)
                        {
                            errors.Add($"Row {rowIndex}: Domain doesn't have a root node ID: " + dataVal.ToString());
                        }
                        else
                        {
                            item.RootNodeId = domain.RootContentId.Value;
                            IPublishedContent content = Current.UmbracoContext.Content.GetById(domain.RootContentId.Value);
                            if (content != null) item.RootNodeKey = content.Key;
                        }

                    }
                }

                //Old Url - REQUIRED
                if (Map.ColumnInboundUrl > 0)
                {
                    var dataVal = GetColumnValue(row, Map.ColumnInboundUrl);
                    string valueInboundUrl = dataVal != null? dataVal.ToString():"";
                    try
                    {
                        // TODO: Should we validate the domain? Any security concerns about using the input value?

                        string testUrl = "http://hest.dk" + valueInboundUrl;
                        Uri uri = new Uri(testUrl);
                        // item.AddOptions.OriginalUrl = valueInboundUrl;
                        item.OldUrl = valueInboundUrl;
                    }
                    catch (Exception)
                    {
                        errors.Add($"Row {rowIndex}: Invalid inbound URL specified: " + valueInboundUrl);
                        item.DataInvalidForImport = true;
                    }
                }


                //Destination Id
                if (Map.ColumnDestinationId > 0)
                {
                    var dataVal = GetColumnValue(row, Map.ColumnDestinationId);
                    string valueDestinationId = dataVal != null ? dataVal.ToString() : "";
                    if (!int.TryParse(valueDestinationId, out int destinationId))
                    {
                        errors.Add($"WARN: Row {rowIndex}: Invalid destination ID: " + valueDestinationId);
                    }
                    else
                    {
                        item.DestinationId = destinationId;
                    }
                }

                //New Url - REQUIRED
                if (Map.ColumnDestinationUrl > 0)
                {
                    var dataVal = GetColumnValue(row, Map.ColumnDestinationUrl);
                    string destinationUrl = dataVal != null ? dataVal.ToString() : "";
                    if (string.IsNullOrWhiteSpace(destinationUrl))
                    {
                        errors.Add($"ERROR: Row {rowIndex}: Invalid destination URL: " + destinationUrl);
                        item.DataInvalidForImport = true;
                    }
                    else
                    {
                        item.NewUrl = destinationUrl;
                    }
                }

                //Link Mode (RedirectDestinationType)
                if (Map.ColumnDestinationType > 0)
                {
                    var dataVal = GetColumnValue(row, Map.ColumnDestinationType);
                    string valueLinkMode = dataVal != null ? dataVal.ToString() : "";
                    if (!EnumUtils.TryParseEnum(valueLinkMode, out RedirectDestinationType destinationMode))
                    {
                        errors.Add($"WARN: Row {rowIndex}: Invalid destination type: " + valueLinkMode);
                    }
                    else
                    {
                        item.DestinationType = destinationMode;
                    }
                }

                //Finish up Row
                item.Messages = errors;
                redirects.Add(item);

                rowIndex++;
            }

            importData.Items = redirects;

            //Compile failed rows ERROR messages
            var failures = redirects.Where(n => n.DataInvalidForImport);
            importData.ImportErrors = failures.SelectMany(n => n.Messages.Where(m => m.StartsWith("ERROR")));

            return importData;
        }

        private object GetColumnValue(CsvDataRow DataRow, int ColumnNumberToGet)
        {
            var dataMatches = DataRow.Columns.Where(n => n.ColNum == ColumnNumberToGet).ToList();
            if (dataMatches.Any())
            {
              return  dataMatches.First().Value;
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region Parsing using Skybrud.Csv

        private void MapCsvColumns(CsvInternalImportOptions options)
        {
            var fileColumns = new List<string>();

            foreach (CsvColumn column in options.File.Columns)
            {
                var colName = column.Name.Replace(" ", "").ToLowerInvariant();

                //strip BOM //TODO: Handle this better in the file reading stage?
                FixBOMIfNeeded(ref colName);

                fileColumns.Add(colName);

                switch (colName)
                {
                    case "rootnodeid":
                    case "domain":
                        if (options.ColumnRootNodeId != null) break;
                        options.ColumnRootNodeId = column;
                        break;

                    case "url":
                    case "inboundurl":
                    case "oldurl":
                    case "old":
                        if (options.ColumnInboundUrl != null) break;
                        options.ColumnInboundUrl = column;
                        break;

                    case "linkid":
                    case "destinationid":
                        if (options.ColumnDestinationId != null) break;
                        options.ColumnDestinationId = column;
                        break;

                    case "linktype":
                    case "linkmode":
                    case "destinationtype":
                    case "destinationmode":
                        if (options.ColumnDestinationType != null) break;
                        options.ColumnDestinationType = column;
                        break;

                    case "linkurl":
                    case "destinationurl":
                    case "newurl":
                    case "new":
                        if (options.ColumnDestinationUrl != null) break;
                        options.ColumnDestinationUrl = column;
                        break;
                }
            }

            var colsMsg = $" Columns present in imported file: {string.Join(", ", fileColumns)}";
            //if (options.ColumnRootNodeId == null) throw new RedirectsException("No column found for *root node ID*");
            if (options.ColumnInboundUrl == null) throw new RedirectsException("No column found for *inbound URL*." + colsMsg);
            if (options.ColumnDestinationUrl == null)
            {
                //check for Dest ID & Type
                if (options.ColumnDestinationId == null || options.ColumnDestinationType == null)
                {
                    throw new RedirectsException("You need to either provide the *destination URL* column OR both the*destination ID* and *destination type* columns." + colsMsg);
                }
            }

        }

        private static bool FixBOMIfNeeded(ref string str)
        {
            const char BOMChar = (char)65279;

            if (string.IsNullOrEmpty(str))
                return false;

            bool hasBom = str[0] == BOMChar;
            if (hasBom)
                str = str.Substring(1);

            return hasBom;
        }

        private ImportDataSet ParseCsvRows(CsvInternalImportOptions options)
        {
            var importData = new ImportDataSet();
            var redirects = new List<ImportDataItem>();

            foreach (CsvRow row in options.File.Rows)
            {
                var errors = new List<string>();
                ImportDataItem item = new ImportDataItem();
                //{
                //    AddOptions = new AddRedirectOptions()
                //};

                //RootNode Id / Key
                string valueRootNodeId = row.GetCellValue(options.ColumnRootNodeId.Index);
                if (int.TryParse(valueRootNodeId, out int rootNodeId))
                {
                    item.RootNodeId = rootNodeId;

                    IPublishedContent content = Current.UmbracoContext.Content.GetById(rootNodeId);

                    if (content != null) item.RootNodeKey = content.Key;

                }
                else
                {
                    // TODO: Should we validate the domain? Any security concerns about using the input value?

                    IDomain domain = Current.Services.DomainService.GetByName(valueRootNodeId);
                    if (domain == null)
                    {
                        errors.Add($"Row {row.Index}: Unknown root node ID or domain: " + valueRootNodeId);
                    }
                    else if (domain.RootContentId == null)
                    {
                        errors.Add($"Row {row.Index}: Domain doesn't have a root node ID: " + valueRootNodeId);
                    }
                    else
                    {
                        item.RootNodeId = domain.RootContentId.Value;
                        IPublishedContent content = Current.UmbracoContext.Content.GetById(domain.RootContentId.Value);
                        if (content != null) item.RootNodeKey = content.Key;
                    }

                }

                //Old Url - REQUIRED
                string valueInboundUrl = row.GetCellValue(options.ColumnInboundUrl.Index);
                try
                {
                    // TODO: Should we validate the domain? Any security concerns about using the input value?

                    string testUrl = "http://hest.dk" + valueInboundUrl;
                    Uri uri = new Uri(testUrl);
                    // item.AddOptions.OriginalUrl = valueInboundUrl;
                    item.OldUrl = valueInboundUrl;
                }
                catch (Exception)
                {
                    errors.Add($"Row {row.Index}: Invalid inbound URL specified: " + valueInboundUrl);
                    item.DataInvalidForImport = true;
                }

                //Destination Id
                string valueDestinationId = row.GetCellValue(options.ColumnDestinationId.Index);
                if (!int.TryParse(valueDestinationId, out int destinationId))
                {
                    errors.Add($"WARN: Row {row.Index}: Invalid destination ID: " + valueDestinationId);
                }
                else
                {
                    item.DestinationId = destinationId;
                }


                //New Url - REQUIRED
                string destinationUrl = row.GetCellValue(options.ColumnDestinationUrl.Index);
                if (string.IsNullOrWhiteSpace(destinationUrl))
                {
                    errors.Add($"ERROR: Row {row.Index}: Invalid destination URL: " + destinationUrl);
                    item.DataInvalidForImport = true;
                }

                //Link Mode (RedirectDestinationType)
                string valueLinkMode = row.GetCellValue(options.ColumnDestinationType.Index);
                if (!EnumUtils.TryParseEnum(valueLinkMode, out RedirectDestinationType destinationMode))
                {
                    errors.Add($"WARN: Row {row.Index}: Invalid destination type: " + valueLinkMode);
                }
                else
                {
                    item.DestinationType = destinationMode;
                }

                //item.AddOptions.Destination = new RedirectDestination(destinationId, destinationKey, destinationUrl, destinationMode) { /*Name = destinatioName*/ };
                //item.AddOptions.Overwrite = options.Options.OverwriteExisting;

                //Finish up Row
                item.Messages = errors;
                redirects.Add(item);
            }

            importData.Items = redirects;

            //Compile failed rows ERROR messages
            var failures = redirects.Where(n => n.DataInvalidForImport);
            importData.ImportErrors = failures.SelectMany(n => n.Messages.Where(m => m.StartsWith("ERROR")));

            return importData;
        }

        #endregion
    }

    class CsvInternalImportOptions
    {
        public CsvFile File { get; }

        public IEnumerable<CsvDataRow> Rows { get; set; }

        public CsvImportOptions Options { get; }

        public CsvColumn ColumnRootNodeId { get; set; }

        public CsvColumn ColumnInboundUrl { get; set; }

        public CsvColumn ColumnDestinationId { get; set; }

        public CsvColumn ColumnDestinationType { get; set; }

        public CsvColumn ColumnDestinationUrl { get; set; }

        public CsvInternalImportOptions(CsvFile file, CsvImportOptions options)
        {
            File = file;
            Options = options;
        }

        public CsvInternalImportOptions(IEnumerable<CsvDataRow> DataRows, CsvImportOptions options)
        {
            Rows = DataRows;
            Options = options;
        }


    }

    class CsvMap
    {
        public CsvDataSet DataSet { get; internal set; }

        public CsvImportOptions Options { get; internal set; }

        /// <summary>1-based Column Number of 'Root Node Id' data </summary>
        public int ColumnRootNodeId { get; set; }

        /// <summary>1-based Column Number of 'Inbound Url' data </summary>
        public int ColumnInboundUrl { get; set; }

        /// <summary>1-based Column Number of 'Destination Node Id' data </summary>
        public int ColumnDestinationId { get; set; }

        /// <summary>1-based Column Number of 'Destination Type' data </summary>
        public int ColumnDestinationType { get; set; }

        /// <summary>1-based Column Number of 'Destination Url' data </summary>
        public int ColumnDestinationUrl { get; set; }

        public CsvMap(CsvDataSet CsvData, CsvImportOptions ImportOptions)
        {
            DataSet = CsvData;
            Options = ImportOptions;
        }

    }

    class CsvDataSet
    {
        public IEnumerable<CsvDataRow> Rows { get; set; }

        public Dictionary<int, string> Headers { get; set; }

        public CsvDataSet()
        {
            Rows = new List<CsvDataRow>();
            Headers = new Dictionary<int, string>();
        }
    }
    class CsvDataRow : List<DataRowItem>, IDataRow
    {
        public IEnumerable<CsvDataColumn> Columns { get; set; }
    }

    class CsvDataColumn
    {
        public int ColNum { get; set; }
        public string Header { get; set; }
        public object Value { get; set; }

    }
}