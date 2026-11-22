using System.Diagnostics.CodeAnalysis;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace Skybrud.Umbraco.Redirects.Import.Helpers;

/// <summary>
/// Represents the result of resolving a destination URL to published content using
/// <see cref="RedirectsImportHelper.GetDestinationContent"/> or
/// <see cref="RedirectsImportHelper.TryGetDestinationContent"/>.
/// </summary>
public class DestinationContentResult {

    /// <summary>
    /// Gets the resolved published content.
    /// </summary>
    public required IPublishedContent Content { get; init; }

    /// <summary>
    /// Gets the culture associated with the resolved content, or <see langword="null"/>
    /// if the content was resolved without a specific culture.
    /// </summary>
    public required string? Culture { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DestinationContentResult"/> class.
    /// Both <see cref="Content"/> and <see cref="Culture"/> must be initialized.
    /// </summary>
    public DestinationContentResult() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="DestinationContentResult"/> class
    /// with the specified published content and culture.
    /// </summary>
    /// <param name="content">The resolved published content.</param>
    /// <param name="culture">
    /// The culture associated with the resolved content, or <see langword="null"/>
    /// if no specific culture was used during resolution.
    /// </param>
    [SetsRequiredMembers]
    public DestinationContentResult(IPublishedContent content, string? culture) {
        Content = content;
        Culture = culture;
    }

}