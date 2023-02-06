using System;
using System.Collections.Generic;
using Umbraco.Cms.Core.Composing;

namespace Skybrud.Umbraco.Redirects.Import.Exporters {

    /// <summary>
    /// Class representing a collection of <see cref="IExporter"/> instances.
    /// </summary>
    public class ExporterCollection : BuilderCollectionBase<IExporter> {

        private readonly Dictionary<string, IExporter> _lookup;

        /// <inheritdoc />
        public ExporterCollection(Func<IEnumerable<IExporter>> items) : base(items) {

            _lookup = new Dictionary<string, IExporter>(StringComparer.OrdinalIgnoreCase);

            foreach (IExporter item in this) {

                string typeName = item.GetType().AssemblyQualifiedName;
                if (typeName != null && _lookup.ContainsKey(typeName) == false) {
                    _lookup.Add(typeName, item);
                }

            }

        }

        /// <summary>
        /// Attempts to get the exporter of the specified type.
        /// </summary>
        /// <typeparam name="TExporter">The type of the exporter.</typeparam>
        /// <param name="result">When this method returns, holds an instance of <typeparamref name="TExporter"/> if successful; otherwise, <see langword="null"/>.</param>
        /// <returns><see langword="true"/> if successful; otherwise, <see langword="false"/>.</returns>
        public bool TryGet<TExporter>(out TExporter result) where TExporter : IExporter {
            if (_lookup.TryGetValue(typeof(TExporter).AssemblyQualifiedName!, out IExporter exporter)) {
                result = (TExporter)exporter;
                return true;
            } else {
                result = default;
                return false;
            }
        }

        /// <summary>
        /// Attempts to get the exporter matching the specified <paramref name="typeName"/>.
        /// </summary>
        /// <param name="typeName">The name of the type.</param>
        /// <param name="result">When this method returns, holds an instance of <see cref="IExporter"/> if successful; otherwise, <see langword="null"/>.</param>
        /// <returns><see langword="true"/> if successful; otherwise, <see langword="false"/>.</returns>
        public bool TryGet(string typeName, out IExporter result) {
            return _lookup.TryGetValue(typeName, out result);
        }

    }

}