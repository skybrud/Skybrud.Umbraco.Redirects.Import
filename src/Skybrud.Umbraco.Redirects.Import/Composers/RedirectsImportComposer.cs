using Microsoft.Extensions.DependencyInjection;
using Skybrud.Umbraco.Redirects.Import.Exporters;
using Skybrud.Umbraco.Redirects.Import.Importers;
using Skybrud.Umbraco.Redirects.Import.Manifests;
using Skybrud.Umbraco.Redirects.Import.Models.Config;
using Skybrud.Umbraco.Redirects.Import.Services;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Infrastructure.Manifest;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Skybrud.Umbraco.Redirects.Import.Composers;

public class RedirectsImportComposer : IComposer {

    public void Compose(IUmbracoBuilder builder) {

        builder.Services.AddOptions<RedirectsImportSettings>()
            .Bind(builder.Config.GetSection("Skybrud:Redirects:Import"), o => o.BindNonPublicProperties = true)
            .ValidateDataAnnotations();

        builder.Services.AddSingleton<RedirectsImportService>();
        builder.Services.AddSingleton<RedirectsImportServiceDependencies>();

        // TODO: Should importers/exporters be registered manually as auto discovery may be expensive?
        builder.WithCollectionBuilder<ImporterCollectionBuilder>().Add(() => builder.TypeLoader.GetTypes<IImporter>());
        builder.WithCollectionBuilder<ExporterCollectionBuilder>().Add(() => builder.TypeLoader.GetTypes<IExporter>());

        builder.Services.AddSingleton<IPackageManifestReader, RedirectsImportManifestReader>();

    }

}