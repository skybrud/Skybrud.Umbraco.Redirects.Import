using System;
using System.Collections.Generic;
using Umbraco.Cms.Core.Composing;

namespace Skybrud.Umbraco.Redirects.Import.Exporters {

    public class ExporterCollection : BuilderCollectionBase<IExporter> {

        private readonly Dictionary<string, IExporter> _lookup;

        public ExporterCollection(Func<IEnumerable<IExporter>> items) : base(items) {

            _lookup = new Dictionary<string, IExporter>(StringComparer.OrdinalIgnoreCase);

            foreach (IExporter item in this) {

                string typeName = item.GetType().AssemblyQualifiedName;
                if (typeName != null && _lookup.ContainsKey(typeName) == false) {
                    _lookup.Add(typeName, item);
                }

            }

        }

        public bool TryGet<TExporter>(out TExporter result) where TExporter : IExporter {
            if (_lookup.TryGetValue(typeof(TExporter).AssemblyQualifiedName!, out IExporter importer)) {
                result = (TExporter)importer;
                return true;
            } else {
                result = default;
                return false;
            }
        }

        public bool TryGet(string typeName, out IExporter result) {
            return _lookup.TryGetValue(typeName, out result);
        }

    }

}