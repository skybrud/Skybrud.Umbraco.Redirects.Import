@echo off
dotnet build src/Skybrud.Umbraco.Redirects.Import --configuration Release /t:rebuild /t:pack -p:PackageOutputPath=../../releases/nuget