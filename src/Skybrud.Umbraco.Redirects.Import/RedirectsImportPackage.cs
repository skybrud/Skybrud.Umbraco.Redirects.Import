using System;
using Skybrud.Essentials.Reflection;
using Umbraco.Cms.Core.Semver;

namespace Skybrud.Umbraco.Redirects.Import;

/// <summary>
/// Static class with various constants about this package.
/// </summary>
public static class RedirectsImportPackage {

    /// <summary>
    /// Gets the alias of this package.
    /// </summary>
    public const string Alias = "Skybrud.Umbraco.Redirects.Import";
    /// <summary>
    /// Gets the friendly name of the package.
    /// </summary>
    public const string Name = "Skybrud Redirects Import";

    /// <summary>
    /// Gets the URL of this package's folder inside Umbraco's <c>App_Plugins</c> folder.
    /// </summary>
    public const string AppPlugins = $"/App_Plugins/{Alias}/";

    /// <summary>
    /// Gets the version of the package.
    /// </summary>
    public static readonly Version Version = typeof(RedirectsPackage).Assembly.GetName().Version!;

    /// <summary>
    /// Gets the informational version of the package.
    /// </summary>
    public static readonly string InformationalVersion = ReflectionUtils.GetInformationalVersion(typeof(RedirectsImportPackage));

    /// <summary>
    /// Gets the semantic version of the package.
    /// </summary>
    public static readonly SemVersion SemVersion = SemVersion.Parse(InformationalVersion);

}