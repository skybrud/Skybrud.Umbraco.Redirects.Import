# Skybrud Redirects Import

[![GitHub license](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE.md)
[![NuGet](https://img.shields.io/nuget/vpre/Skybrud.Umbraco.Redirects.Import.svg)](https://www.nuget.org/packages/Skybrud.Umbraco.Redirects.Import)
[![NuGet](https://img.shields.io/nuget/dt/Skybrud.Umbraco.Redirects.Import.svg)](https://www.nuget.org/packages/Skybrud.Umbraco.Redirects.Import)
[![Our Umbraco](https://img.shields.io/badge/our-umbraco-%233544B1)](https://our.umbraco.com/packages/website-utilities/skybrud-redirects-import/)
[![Umbraco Marketplace](https://img.shields.io/badge/umbraco-marketplace-%233544B1)](https://marketplace.umbraco.com/package/skybrud.umbraco.redirects.import)

Import and export addon for [**Skybrud.Umbraco.Redirects**](https://github.com/skybrud/Skybrud.Umbraco.Redirects). The package features an extensible set of importers and exporters, and by default it supports importing from and exporting to CSV, XLSX and JSON files.


<br /><br />

## Installation

The package is only available via [**NuGet**](https://www.nuget.org/packages/Skybrud.Umbraco.Redirects.Import). To install the package, you can use either .NET CLI:

```
dotnet add package Skybrud.Umbraco.Redirects.Import --version 4.0.0
```

or the older NuGet Package Manager:

```
Install-Package Skybrud.Umbraco.Redirects.Import -Version 4.0.0
```



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
