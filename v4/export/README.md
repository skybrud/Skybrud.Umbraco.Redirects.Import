---
order: 6
---

# Export

The package ships with three different exporters, thereby supporting to either a `.csv` file, an `.xlsx` file or a `.json` file.





### CSV

Since CSV files are text based, there are a few additional options to consider. 

#### Encoding

Encodings are always a bit tricky. If your redirects may have special characters, you should make sure to select the right encoding that matches the application or system for which the exported file is intended. **UTF-8** supports the widest set of characters, which often makes it a favorable encoding. But Excel will not automatically detect UTF-8 encoding files, so if the exported file is intended to be opened in Excel, you should select the **Windows 1252** encoding.

#### Separator

CSV stands for comma separator values, meaning a comma (`,`) is used for separating each column. Despite the name, a *semi colons* (`;`) do however seem to be more widely used. The default separator in Excel may depend both on the version, and the culture of the operating system.





#### Columns

This option let's you specify the columns that should be included in the CSV file. Most columns are selected by default, while these columns are not selected by default:

- **DestinationId**
When moving content across environments, a numeric ID may quickly lose it's meaning, as a numeric ID from one environment may refer to something different in another environment. Ideally you should use the `DestinationKey` column instead.

- **IsPermanent**
This column is obsolete as a `Type` column was introduced instead. Currently **Skybrud Redirects** only supports two reidrect types (`Permanent` and `Temporary`), but if more types are introduced in the future, the `IsPermanent` column will loose it's meaning.





#### XLSX

Available options:

#### Columns

This option let's you specify the columns that should be included in the XLSX file. Most columns are selected by default, while these columns are not selected by default:

- **DestinationId**
When moving content across environments, a numeric ID may quickly lose it's meaning, as a numeric ID from one environment may refer to something different in another environment. Ideally you should use the `DestinationKey` column instead.

- **IsPermanent**
This column is obsolete as a `Type` column was introduced instead. Currently **Skybrud Redirects** only supports two reidrect types (`Permanent` and `Temporary`), but if more types are introduced in the future, the `IsPermanent` column will loose it's meaning.




#### JSON

The JSON exporter uses an internal format, which can be used to re-import in another Umbraco installation that is also using this package.
