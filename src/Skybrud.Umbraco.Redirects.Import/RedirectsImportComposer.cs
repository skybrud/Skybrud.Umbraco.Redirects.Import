using Microsoft.Extensions.DependencyInjection;
using Skybrud.Umbraco.Redirects.Import.Exporters;
using Skybrud.Umbraco.Redirects.Import.Importers;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;

#pragma warning disable CS1591

namespace Skybrud.Umbraco.Redirects.Import {

    public class RedirectsImportComposer : IComposer {

        public void Compose(IUmbracoBuilder builder) {

            builder.Services.AddSingleton<RedirectsImportService>();

            // TODO: Should importers/exporters be registered manually as auto discovery may be expensive?
            builder.WithCollectionBuilder<ImporterCollectionBuilder>().Add(() => builder.TypeLoader.GetTypes<IImporter>());
            builder.WithCollectionBuilder<ExporterCollectionBuilder>().Add(() => builder.TypeLoader.GetTypes<IExporter>());

            builder.ManifestFilters().Append<RedirectsImportManifestFilter>();

        }

    }

}