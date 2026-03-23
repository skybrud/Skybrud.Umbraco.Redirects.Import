# Skybrud Redirects Import

[![GitHub license](https://img.shields.io/badge/license-MIT-blue.svg)](https://github.com/skybrud/Skybrud.Umbraco.Redirects.Import/blob/v17/main/LICENSE.md)
[![NuGet](https://img.shields.io/nuget/vpre/Skybrud.Umbraco.Redirects.Import.svg)](https://www.nuget.org/packages/Skybrud.Umbraco.Redirects.Import)
[![NuGet](https://img.shields.io/nuget/dt/Skybrud.Umbraco.Redirects.Import.svg)](https://www.nuget.org/packages/Skybrud.Umbraco.Redirects.Import)
[![Umbraco Marketplace](https://img.shields.io/badge/umbraco-marketplace-%233544B1)](https://marketplace.umbraco.com/package/skybrud.umbraco.redirects.import)

Import and export addon for [**Skybrud.Umbraco.Redirects**](https://github.com/skybrud/Skybrud.Umbraco.Redirects). The package features an extensible set of importers and exporters, and by default supports importing from and exporting to **CSV**, **XLSX**, and **JSON** files.

<table>
  <tr>
    <td><strong>License:</strong></td>
    <td><a href="https://github.com/skybrud/Skybrud.Umbraco.Redirects.Import/blob/v17/main/LICENSE.md"><strong>MIT License</strong></a></td>
  </tr>
  <tr>
    <td><strong>Umbraco:</strong></td>
    <td>Umbraco 17</td>
  </tr>
  <tr>
    <td><strong>Target Framework:</strong></td>
    <td>.NET 10</td>
  </tr>
</table>

<br />

## Table of Contents

- [Installation](#installation)
- [Configuration](#configuration)
- [Usage](#usage)
- [File Formats](#file-formats)
  - [CSV](#csv)
  - [XLSX](#xlsx)
  - [JSON](#json)
- [Column Reference](#column-reference)
- [Extending](#extending)
  - [Custom Importers](#custom-importers)
  - [Custom Exporters](#custom-exporters)
- [Screenshots](#screenshots)

<br /><br />

## Installation

### Umbraco 17

The package is only available via [**NuGet**](https://www.nuget.org/packages/Skybrud.Umbraco.Redirects.Import). To install the package, you can use either .NET CLI:

```
dotnet add package Skybrud.Umbraco.Redirects.Import --version 17.0.0-beta001
```

or the NuGet Package Manager:

```
Install-Package Skybrud.Umbraco.Redirects.Import -Version 17.0.0-beta001
```

No further setup is required. The package registers its services and discovers importers/exporters automatically via Umbraco's composition system.

### Other versions of Umbraco

- [**`v13/main`**](https://github.com/skybrud/Skybrud.Umbraco.Redirects.Import/tree/v13/main) Umbraco 13
- ~~[**`v4/main`**](https://github.com/skybrud/Skybrud.Umbraco.Redirects.Import/tree/v4/main) Umbraco 10, 11 and 12~~ <sub title="Umbraco 10, 11 and 12 have reached end-of-life"><sup>(EOL)</sup></sub>

<br /><br />

## Configuration

The package can be configured via `appsettings.json` under the `Skybrud:Redirects:Import` key. All settings are optional.

```json
{
  "Skybrud": {
    "Redirects": {
      "Import": {
        "Csv": {
          "AllowedContentTypes": [
            "text/csv",
            "application/csv"
          ]
        }
      }
    }
  }
}
```

| Setting | Default | Description |
|---|---|---|
| `Csv:AllowedContentTypes` | `["text/csv", "application/csv"]` | MIME types accepted when uploading a CSV file. |

<br /><br />

## Usage

Once installed, the package adds **Import** and **Export** options to the redirects back-office section.

1. Open the **Redirects** section in the Umbraco back-office.
2. Click the **Add redirect** button — you will now see **Import** and **Export** options alongside the default option.
3. Select a format (CSV, XLSX, or JSON).
4. Configure the available options (encoding, separator, column selection, etc.).
5. Upload a file (import) or download the generated file (export).

During import, each redirect row is processed individually and assigned one of these statuses:

| Status | Description |
|---|---|
| `Added` | Redirect was created successfully. |
| `Updated` | An existing redirect was overwritten with new values. |
| `NotModified` | An existing redirect was found but had no changes. |
| `AlreadyExists` | A redirect with the same URL already exists and **Overwrite** was not enabled. |
| `Failed` | Import failed for this row (error message is shown). |

<br /><br />

## File Formats

### CSV

CSV files are imported and exported with configurable encoding and column separator.

#### Import options

| Option | Values | Default | Description |
|---|---|---|---|
| Overwrite existing | `true` / `false` | `false` | Whether to overwrite redirects that already exist. |
| Encoding | Auto, ASCII, UTF-8, Windows-1252, ISO-8859-1 | Auto | Character encoding of the uploaded file. |
| Separator | Auto, Colon, Comma, SemiColon, Space, Tab | Auto | Column delimiter used in the file. |

#### Export options

| Option | Values | Default | Description |
|---|---|---|---|
| Encoding | ASCII, UTF-8, Windows-1252 | UTF-8 | Character encoding of the downloaded file. |
| Separator | Colon, Comma, SemiColon, Space, Tab | Comma | Column delimiter to use. |
| Columns | Selectable list | All | Which redirect properties to include in the export. |

---

### XLSX

Excel files follow the same column structure as CSV. The importer reads the first worksheet and treats the first row as column headers.

#### Import options

| Option | Values | Default | Description |
|---|---|---|---|
| Overwrite existing | `true` / `false` | `false` | Whether to overwrite redirects that already exist. |

#### Export options

| Option | Description |
|---|---|
| Columns | Which redirect properties to include in the exported sheet. |

---

### JSON

JSON files are the native format for backing up and restoring redirects. Exported files include a package version marker which is validated on import.

#### Import options

| Option | Values | Default | Description |
|---|---|---|---|
| Overwrite existing | `true` / `false` | `false` | Whether to overwrite redirects that already exist. |

#### Export options

| Option | Description |
|---|---|
| Columns | Which redirect properties to include in the exported file. |

<br /><br />

## Column Reference

The CSV and XLSX importers detect columns by their header names. Matching is **case-insensitive** and spaces are ignored, so `Root Key`, `rootkey`, and `ROOTKEY` all resolve to the same column. The JSON importer uses a fixed schema described [below](#json-1).

### Inbound URL *(required)*

The original URL that should trigger the redirect.

| Accepted header | Notes |
|---|---|
| `Url` | Recommended |
| `InboundUrl` | |
| `OriginalUrl` | |
| `OldUrl` | |
| `Old` | Short alias |
| `From` | Short alias |

### Destination URL *(required unless `DestinationId` is provided)*

The URL, path, or content reference to redirect to.

| Accepted header | Notes |
|---|---|
| `DestinationUrl` | Recommended |
| `To` | Short alias |
| `New` | Short alias |
| `NewUrl` | |
| `RedirectUrl` | |

Accepted values: a relative path (`/new-page`), an absolute URL (`https://example.com`), or a numeric node ID / GUID key (resolved to a content node).

### Destination ID *(required unless `DestinationUrl` is provided)*

Numeric Umbraco node ID of the destination content node.

| Accepted header | Notes |
|---|---|
| `DestinationId` | Recommended |
| `RedirectNodeId` | Legacy alias |

### Destination Key *(optional)*

GUID key of the destination content or media node. Supplementary to `DestinationId` / `DestinationUrl` — note that a `DestinationKey` column alone will not pass import validation; at least one of `DestinationId` or `DestinationUrl` must also be present.

| Accepted header | Notes |
|---|---|
| `DestinationKey` | Recommended |
| `RedirectNodeKey` | Legacy alias |

### Destination Type *(optional)*

Whether the destination is a content node, media item, or plain URL.

| Accepted header | Accepted values |
|---|---|
| `DestinationType` | `Content`, `Media`, `Url` |

### Root Node / Site *(optional)*

Scopes the redirect to a specific Umbraco site. Omit for global redirects.

| Accepted header | Accepted value type | Notes |
|---|---|---|
| `RootKey` | GUID | Recommended |
| `RootNodeKey` | GUID | |
| `RootNode` | GUID or numeric ID | |
| `RootNodeId` | Numeric ID | |
| `RootId` | Numeric ID | |
| `SiteId` | Numeric ID | |
| `SiteKey` | GUID | |
| `Domain` | Domain name, e.g. `example.com` | |

### Redirect Type *(optional)*

Controls the HTTP status code used for the redirect. Defaults to **Permanent** (301) when omitted.

| Accepted header | Accepted values |
|---|---|
| `Type` | `Permanent`, `Temporary` |
| `RedirectType` | `Permanent`, `Temporary` |
| `Permanent` | `true` (301), `false` (307) |
| `IsPermanent` | `true` (301), `false` (307) |
| `RedirectHttpCode` | `301` (Permanent), `302` / `307` (Temporary) |

### Forward Query String *(optional)*

Whether the query string from the inbound request should be appended to the destination URL.

| Accepted header | Accepted values |
|---|---|
| `ForwardQueryString` | `true`, `false` |
| `ForwardQuery` | `true`, `false` |
| `Forward` | `true`, `false` |

### Inbound Query String *(optional)*

A query string that must be present on the inbound URL for this redirect to match (e.g. `?id=123`).

| Accepted header | Notes |
|---|---|
| `QueryString` | Recommended |
| `Query` | Short alias |
| `InboundQuery` | |
| `InboundQueryString` | |
| `OriginalQuery` | |
| `OriginalQueryString` | |

### Destination Query String *(optional)*

A query string to append to the destination URL.

| Accepted header |
|---|
| `DestinationQuery` |

### Destination Fragment *(optional)*

A URL fragment (hash anchor) to append to the destination URL.

| Accepted header |
|---|
| `DestinationFragment` |

### Destination Culture *(optional)*

The culture variant of the destination content node.

| Accepted header | Accepted values |
|---|---|
| `Culture` | ISO language code, e.g. `en-US`, or numeric language ID |
| `DestinationCulture` | ISO language code, e.g. `en-US`, or numeric language ID |

---

### Example CSV

A minimal import file:

```csv
Url,DestinationUrl
/old-page,/new-page
/another-old-url,https://example.com/target
```

A more complete example using optional columns:

```csv
Url,DestinationUrl,Type,ForwardQueryString,RootKey
/old-page,/new-page,Permanent,false,
/promo,https://example.com/campaign,Temporary,true,
/da/gammel-side,/da/ny-side,Permanent,false,11fb1e9f-4f15-44f3-9ad5-e1a432dde48c
```

<br /><br />

## Extending

The package is built around two interfaces — `IImporter` and `IExporter` — and their generic variants. All implementations are auto-discovered at startup via Umbraco's `TypeLoader`, so registering a custom importer or exporter only requires implementing the interface; no additional DI registration is needed.

### Custom Importers

Inherit from `ImporterBase<TOptions, TResult>` and implement the `Import` method.

```csharp
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using Skybrud.Umbraco.Redirects.Import.Importers;
using Skybrud.Umbraco.Redirects.Import.Models;

public class MyImportOptions : IImportOptions {
    public bool OverwriteExisting { get; set; }
    public RedirectType DefaultRedirectType { get; set; }
    public IFormFile? File { get; set; }
}

public class MyImportResult : IImportResult {
    public bool IsSuccessful { get; }
    public IReadOnlyList<string> Errors { get; }
    public IReadOnlyList<RedirectImportItem> Redirects { get; }
    // ...
}

public class MyImporter : ImporterBase<MyImportOptions, MyImportResult> {

    public MyImporter() {
        Icon = "icon-doc";
        Name = "My Format";
        Description = "Imports redirects from my custom format.";
    }

    public override IEnumerable<Option> GetOptions(HttpRequest request) {
        return [
            Option.Overwrite(),
            Option.File(description: "Select the file to import.")
        ];
    }

    public override MyImportResult Import(MyImportOptions options) {
        // Parse options.File and return a result
        throw new NotImplementedException();
    }

}
```

`GetOptions` controls what is shown in the import dialog. Use the built-in factory methods (`Option.File()`, `Option.Overwrite()`) for standard fields, or construct an `Option` directly for custom dropdowns and inputs.

---

### Custom Exporters

Inherit from `ExporterBase<TOptions, TResult>` and implement the `Export` method.

```csharp
using Skybrud.Umbraco.Redirects.Import.Exporters;
using Skybrud.Umbraco.Redirects.Import.Models;

public class MyExportOptions : IExportOptions { }

public class MyExportResult : IExportResult {
    public Guid Key { get; }
    public string ContentType => "application/octet-stream";
    public string FileName => "redirects.myformat";
    public byte[] GetBytes() => throw new NotImplementedException();
}

public class MyExporter : ExporterBase<MyExportOptions, MyExportResult> {

    public MyExporter() {
        Icon = "icon-download";
        Name = "My Format";
        Description = "Exports redirects as my custom format.";
    }

    public override MyExportResult Export(MyExportOptions options) {
        // Build and return the file
        throw new NotImplementedException();
    }

}
```

The exported file is temporarily stored on disk and served to the browser via a signed download URL. Implement `GetBytes()` to return the raw file content.

<br /><br />

## Screenshots

![image](https://user-images.githubusercontent.com/3634580/187294337-f95fc44c-a058-4e0f-8c31-aed876115ed5.png)
*The package adds an **Import** option as well as an **Export** option to the existing **Add redirect** button.*

![image](https://user-images.githubusercontent.com/3634580/187294360-428ed84e-a0ac-4c56-a2be-85e76fe53e25.png)
*Package includes default importers and exporters for CSV, JSON and XLSX.*

![image](https://user-images.githubusercontent.com/3634580/187294375-eabce1bb-a220-48af-bbb5-63b79a44d08e.png)
*Each importer may have different options — here are the options for the CSV importer.*

![image](https://user-images.githubusercontent.com/3634580/187294383-a702e0af-7c94-4d6c-8987-4bfe111bcc31.png)
*When uploading the CSV file, a status is shown for each redirect. If not set to overwrite existing redirects, the importer will report any duplicates.*
