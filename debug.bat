@echo off
dotnet build src/Skybrud.Umbraco.Redirects.Import --configuration Debug /t:rebuild /t:pack -p:PackageOutputPath=c:/nuget