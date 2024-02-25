---
order: 5
---

# Import

The package ships with three different importer, where each importer is responsible for handling the import from a specific file format. These formats are CSV, XLSX and JSON. You can read more about these importers in the sections below.

If you wish to add support for importing from another file format, you can also do this by implementing the `IImporter` - or extend the `ImporterBase<TOptions, TResult>` base class to get a bit of extra logic out of the box.




## CSV

CSV files (Comma Separated Values) are essentially simple text files with a bit of extra rules to the formatting. While the name suggests that a comma is used as a seperator, other separators are typically used as well, where using a semicolon as the separator might even be more widely used than a colon.

When using the package's UI to import a CSV file, you're prompted with the following options:

- **Overwrite existing**  
By default, if you try to import a redirect where the root node, URL and query string already matches an existing redirect, the import will fail. When enabling this option, the package will instead update the existing redirect rather than creating a new redirect.

- **Encoding**  
Encodings and special characters are often difficult to get right. By default, the package will try to detect the encoding of the uploaded CSV file, but this isn't 100% fail proof, so you also have the option to select a specific encoding.

- **Separator**  
As mentioned earlier, not all CSV files use a comma as separator. By default, the package will try to detect the separator used in the file, but like with the encoding, this this isn't 100% fail proof. As such you'll be able to select between the most typical separators.

- **File**  
This is where you pick the CSV file. Only files with the `.csv` extension are allowed, so if your CSV file has another extension (it happens), you'll have to rename it first.

When processing the uploaded CSV file, the package will look for a number of different column names. You can see the [**Columns** page](./Columns.md) for more information on this.







## XLSX

XLSX files works similar to CSV files, but XLSX is a much more strict format, meaning we don't have to guess or configure things like separators or encodings. This therefore also makes a user experience a bit easier, as there are less options to consider. The XLSX importer currently has the following options:

- **Overwrite existing**  
By default, if you try to import a redirect where the root node, URL and query string already matches an existing redirect, the import will fail. When enabling this option, the package will instead update the existing redirect rather than creating a new redirect.

- **File**  
This is where you pick the XLSX file. Only files with the `.xlsx` extension are allowed, so if your ELSX file has another extension, you'll have to rename it first.





## JSON

The JSON import uses an internal format, so it can primarily be used for importing redirects from a JSON file exported from another site using this package - eg. when doing a migration from an older solution to a newer solution.

Since the JSON import only supports this internal format, and since JSON files per definition are encoding using UTF-8, the import doesn't provide additional option exception selecting the file to upload.
