using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Umbraco.Cms.Core.Composing;

namespace Skybrud.Umbraco.Redirects.Import.Importers {

    /// <summary>
    /// Class representing a collection of <see cref="IImporter"/> instances.
    /// </summary>
    public class ImporterCollection : BuilderCollectionBase<IImporter> {

        private readonly Dictionary<string, IImporter> _lookup;

        /// <inheritdoc />
        public ImporterCollection(Func<IEnumerable<IImporter>> items) : base(items) {

            _lookup = new Dictionary<string, IImporter>(StringComparer.OrdinalIgnoreCase);

            foreach (IImporter item in this) {

                string? typeName = item.GetType().AssemblyQualifiedName;
                if (typeName != null && _lookup.ContainsKey(typeName) == false) {
                    _lookup.Add(typeName, item);
                }

            }

        }

        /// <summary>
        /// Attempts to get the importer of the specified type.
        /// </summary>
        /// <typeparam name="TImporter">The type of the importer.</typeparam>
        /// <param name="result">When this method returns, holds an instance of <typeparamref name="TImporter"/> if successful; otherwise, <see langword="null"/>.</param>
        /// <returns><see langword="true"/> if successful; otherwise, <see langword="false"/>.</returns>
        public bool TryGet<TImporter>([NotNullWhen(true)] out TImporter? result) where TImporter : IImporter {
            if (_lookup.TryGetValue(typeof(TImporter).AssemblyQualifiedName!, out IImporter? importer)) {
                result = (TImporter)importer;
                return true;
            }
            result = default;
            return false;
        }

        /// <summary>
        /// Attempts to get the importer matching the specified <paramref name="typeName"/>.
        /// </summary>
        /// <param name="typeName">The name of the type.</param>
        /// <param name="result">When this method returns, holds an instance of <see cref="IImporter"/> if successful; otherwise, <see langword="null"/>.</param>
        /// <returns><see langword="true"/> if successful; otherwise, <see langword="false"/>.</returns>
        public bool TryGet(string typeName, [NotNullWhen(true)] out IImporter? result) {
            return _lookup.TryGetValue(typeName, out result);
        }

    }

}