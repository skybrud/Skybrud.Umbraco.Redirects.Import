namespace Skybrud.Umbraco.Redirects.Import
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    public partial class Constants
    {
        public const string AppPluginsPath = "~/App_Plugins/Skybrud.Umbraco.Redirects.Import/";
        public const string FileUploadPath = "~/App_Data/TEMP/RedirectsImporter/";

        #region LinkMode
        public enum LinkMode
        {
            Content,
            Media,
            Url
        }

        public static Dictionary<LinkMode, string> LinkModesWithDisplayText()
        {
            var dict = new Dictionary<LinkMode, string>();

            dict.Add(LinkMode.Content, "Content");
            dict.Add(LinkMode.Media, "Media");
            dict.Add(LinkMode.Url, "Url");

            return dict;
        }

    
        #endregion

        //#region NodeType
        //public enum NodeType
        //{
        //    Content,
        //    Media,
        //    Unknown
        //}

        //public static NodeType GetNodeType(string TypeString)
        //{
        //    switch (TypeString)
        //    {
        //        case "Content":
        //            return NodeType.Content;

        //        case "Media":
        //            return NodeType.Media;

        //        default:
        //            return NodeType.Unknown;
        //    }
        //}

        //#endregion

    }

}
