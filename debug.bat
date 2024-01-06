@echo off
dotnet build src/Skybrud.Umbraco.Redirects.Import/Skybrud.Umbraco.Redirects.Import.csproj --configuration Debug /t:rebuild /t:pack -p:PackageOutputPath=c:\nuget\Umbraco13