using Umbraco.Cms.Core.Composing;

namespace Skybrud.Umbraco.Redirects.Import.Exporters;

internal sealed class ExporterCollectionBuilder : LazyCollectionBuilderBase<ExporterCollectionBuilder, ExporterCollection, IExporter> {

    protected override ExporterCollectionBuilder This => this;

}