using System;
using System.Collections.Generic;
using System.IO;
using System.Web;

namespace Skybrud.Umbraco.Redirects.Import.Models
{

    public class RedirectsProviderFile
    {

        public string FileName { get; set; }

        public long ContentLength { get; set; }

        public Stream InputStream { get; private set; }

        public RedirectsProviderFile(HttpPostedFileBase file)
        {
            FileName = file.FileName;
            ContentLength = file.ContentLength;
            InputStream = file.InputStream;
        }

        public RedirectsProviderFile(string FilePath)
        {
            var mappedPath = "";
            var canMap = Dragonfly.NetHelpers.Files.TryGetMappedPath(FilePath, out mappedPath);

            if (canMap)
            {
                FileName = mappedPath;

                var fileInfo = new FileInfo(mappedPath);
                ContentLength = fileInfo.Length;
                InputStream = fileInfo.OpenRead();
            }
            else
            {
                throw new ArgumentException($"Unable to map a valid path for '{FilePath}'", nameof(FilePath));
            }

        }

        public RedirectsProviderFile(FileInfo fileInfo)
        {
            FileName = fileInfo.Name;
            ContentLength = fileInfo.Length;
            InputStream = fileInfo.OpenRead();
        }

    }

}