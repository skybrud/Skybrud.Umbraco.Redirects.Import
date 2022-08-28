using System;

namespace Skybrud.Umbraco.Redirects.Import.Exporters {

    public interface IExportResult {

        Guid Key { get; }

        string ContentType { get; }

        string FileName { get; }

        byte[] GetBytes(IExportOptions options);

    }

}