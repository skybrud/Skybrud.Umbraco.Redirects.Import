using Umbraco.Cms.Core.Composing;

namespace Skybrud.Umbraco.Redirects.Import.Importers;

internal sealed class ImporterCollectionBuilder : LazyCollectionBuilderBase<ImporterCollectionBuilder, ImporterCollection, IImporter> {

    protected override ImporterCollectionBuilder This => this;

}