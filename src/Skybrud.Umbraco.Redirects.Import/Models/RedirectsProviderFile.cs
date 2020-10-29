using System;
using System.Collections.Generic;
using System.IO;
using System.Web;

namespace Skybrud.Umbraco.Redirects.Import.Models {
    
    public class RedirectsProviderFile {

        public string FileName { get; set; }

        public long ContentLength { get; set; }

        public Stream InputStream { get; private set; }

        public RedirectsProviderFile(HttpPostedFileBase file) {
            FileName = file.FileName;
            ContentLength = file.ContentLength;
            InputStream = file.InputStream;
        }

        public RedirectsProviderFile(string FilePath)
        {
            //TODO: Add error checking/handling for non-existent file?
            FileName = FilePath;

            var fileInfo = new FileInfo(FilePath);
            ContentLength = fileInfo.Length;
            InputStream = fileInfo.OpenRead();
        }

        public RedirectsProviderFile(FileInfo fileInfo)
        {
            FileName = fileInfo.Name;
            ContentLength = fileInfo.Length;
            InputStream = fileInfo.OpenRead();
        }

    }

}