using System;
using Skybrud.Umbraco.Redirects.Models;

namespace Skybrud.Umbraco.Redirects.Import.Models.Import;

/// <summary>
/// Class with options for importing a single redirect.
/// </summary>
public class RedirectImportOptions {

    /// <summary>
    /// Gets or sets whether an existing redirect should be overwritten should there already be a redirect with
    /// the same <see cref="RootNodeKey"/> and <see cref="OriginalUrl"/>.
    /// </summary>
    public bool Overwrite { get; set; }

    /// <summary>
    /// Gets or set the root node key of the redirect.
    /// </summary>
    public Guid? RootNodeKey { get; set; }

    /// <summary>
    /// Gets or set the original URL the redirect.
    /// </summary>
    public string? OriginalUrl { get; set; }

    /// <summary>
    /// Gets or set the destination of the redirect.
    /// </summary>
    public RedirectDestination? Destination { get; set; }

    /// <summary>
    /// Gets or sets the type of the redirect - e.g. <see cref="RedirectType.Permanent"/> or <see cref="RedirectType.Temporary"/>.
    /// </summary>
    public RedirectType? Type { get; set; }

    /// <summary>
    /// Gets or sets whether query string forwarding should be enabled for the redirect.
    /// </summary>
    public bool? ForwardQueryString { get; set; }

}