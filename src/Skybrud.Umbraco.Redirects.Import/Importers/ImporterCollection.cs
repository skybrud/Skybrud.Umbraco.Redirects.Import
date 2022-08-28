using System;
using System.Collections.Generic;
using Umbraco.Cms.Core.Composing;

namespace Skybrud.Umbraco.Redirects.Import.Importers {

    public class ImporterCollection : BuilderCollectionBase<IImporter> {

        private readonly Dictionary<string, IImporter> _lookup;

        public ImporterCollection(Func<IEnumerable<IImporter>> items) : base(items) {

            _lookup = new Dictionary<string, IImporter>(StringComparer.OrdinalIgnoreCase);

            foreach (IImporter item in this) {

                string typeName = item.GetType().AssemblyQualifiedName;
                if (typeName != null && _lookup.ContainsKey(typeName) == false) {
                    _lookup.Add(typeName, item);
                }

            }

        }

        public bool TryGet<TImporter>(out TImporter result) where TImporter : IImporter {
            if (_lookup.TryGetValue(typeof(TImporter).AssemblyQualifiedName!, out IImporter importer)) {
                result = (TImporter)importer;
                return true;
            }
            result = default;
            return false;
        }

        public bool TryGet(string typeName, out IImporter result) {
            return _lookup.TryGetValue(typeName, out result);
        }

    }

}