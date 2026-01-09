# Skybrud Redirects Import

[![GitHub license](https://img.shields.io/badge/license-MIT-blue.svg)](https://github.com/skybrud/Skybrud.Umbraco.Redirects.Import/blob/v17/main/LICENSE.md)
[![NuGet](https://img.shields.io/nuget/vpre/Skybrud.Umbraco.Redirects.Import.svg)](https://www.nuget.org/packages/Skybrud.Umbraco.Redirects.Import)
[![NuGet](https://img.shields.io/nuget/dt/Skybrud.Umbraco.Redirects.Import.svg)](https://www.nuget.org/packages/Skybrud.Umbraco.Redirects.Import)
[![Umbraco Marketplace](https://img.shields.io/badge/umbraco-marketplace-%233544B1)](https://marketplace.umbraco.com/package/skybrud.umbraco.redirects.import)

Import and export addon for [**Skybrud.Umbraco.Redirects**](https://github.com/skybrud/Skybrud.Umbraco.Redirects). The package features an extensible set of importers and exporters, and by default it supports importing from and exporting to CSV, XLSX and JSON files.

<table>
  <tr>
    <td><strong>License:</strong></td>
    <td><a href="https://github.com/skybrud/Skybrud.Umbraco.Redirects.Import/blob/v17/main/LICENSE.md"><strong>MIT License</strong></a></td>
  </tr>
  <tr>
    <td><strong>Umbraco:</strong></td>
    <td>
      Umbraco 17
    </td>
  </tr>
  <tr>
    <td><strong>Target Framework:</strong></td>
    <td>
      .NET 10
    </td>
  </tr>
</table>


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

### Other versions of Umbraco

- [**`v13/main`**](https://github.com/skybrud/Skybrud.Umbraco.Redirects.Import/tree/v13/main) Umbraco 13
- ~~[**`v4/main`**](https://github.com/skybrud/Skybrud.Umbraco.Redirects.Import/tree/v4/main) Umbraco 10, 11 and 12~~ <sub title="Umbraco 10, 11 and 12 have reached end-of-life"><sup>(EOL)</sup></sub>



<br /><br />

## Screenshots

![image](https://user-images.githubusercontent.com/3634580/187294337-f95fc44c-a058-4e0f-8c31-aed876115ed5.png)
*The package adds an **Import** option as well as an **Export** option to the existing **Add redirect** button.*

![image](https://user-images.githubusercontent.com/3634580/187294360-428ed84e-a0ac-4c56-a2be-85e76fe53e25.png)
*Package includes default importers and exporters for CSV, JSON and XLSX.*

![image](https://user-images.githubusercontent.com/3634580/187294375-eabce1bb-a220-48af-bbb5-63b79a44d08e.png)
*Each import may have different options - here is the options for the CSV importer.*

![image](https://user-images.githubusercontent.com/3634580/187294383-a702e0af-7c94-4d6c-8987-4bfe111bcc31.png)
*When uploading the CSV file, a status is shown for each redirect. If not set to override existing redirects, the importer will fail for any existing redirects.*
