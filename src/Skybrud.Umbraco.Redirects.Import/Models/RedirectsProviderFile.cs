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

        //public RedirectsProviderFile(FileInfo file) {
        //    FileName = file.Name;
        //    ContentLength = file.Length;
        //    InputStream = file.OpenRead();
        //}

    }

}