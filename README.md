# Skybrud Redirects Import [![GitHub license](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE.md) [![NuGet](https://img.shields.io/nuget/vpre/Skybrud.Umbraco.Redirects.Import.svg)](https://www.nuget.org/packages/Skybrud.Umbraco.Redirects.Import) [![NuGet](https://img.shields.io/nuget/dt/Skybrud.Umbraco.Redirects.Import.svg)](https://www.nuget.org/packages/Skybrud.Umbraco.Redirects.Import) [![Our Umbraco](https://img.shields.io/badge/our-umbraco-%233544B1)](https://our.umbraco.com/packages/website-utilities/skybrud-redirects-import/)

Small package that adds an endpoint for exporting all redirects to either a CSV or a JSON file. Although suggested by the name, the Umbraco 7 does not support importing. The purpose is instead to provide a CSV or JSON file that can be imported by the [Umbraco 10 version](https://github.com/skybrud/Skybrud.Umbraco.Redirects.Import/tree/v4/main) of the package.

<table>
  <tr>
    <td><strong>License:</strong></td>
    <td><a href="./LICENSE.md"><strong>MIT License</strong></a></td>
  </tr>
  <tr>
    <td><strong>Umbraco:</strong></td>
    <td>
      Umbraco 7
    </td>
  </tr>
  <tr>
    <td><strong>Target Framework:</strong></td>
    <td>
      .NET Framework 4.5
    </td>
  </tr>
</table>



<br /><br />

## Installation

The package is only available via [**NuGet**](https://www.nuget.org/packages/Skybrud.Umbraco.Redirects.Import). To install the package, you can use either .NET CLI:

```
dotnet add package Skybrud.Umbraco.Redirects.Import --version 1.0.0
```

or the older NuGet Package Manager:

```
Install-Package Skybrud.Umbraco.Redirects.Import -Version 1.0.0
```


<br /><br />

## Usage

When the package is installed, and you're logged into the Umbraco backoffice, you can access the URL below to download a CSV file with all redirects:

```
/umbraco/backoffice/Skybrud/RedirectsExport/Csv
```

or as JSON:

```
/umbraco/backoffice/Skybrud/RedirectsExport/Json
```
