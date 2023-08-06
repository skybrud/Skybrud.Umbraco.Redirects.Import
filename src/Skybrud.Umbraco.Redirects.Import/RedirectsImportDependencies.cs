using Microsoft.AspNetCore.Hosting;
using Skybrud.Umbraco.Redirects.Services;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;

#pragma warning disable CS1591

namespace Skybrud.Umbraco.Redirects.Import {

    public class RedirectsImportDependencies {

        public IWebHostEnvironment WebHostEnvironment { get; }

        public IDomainService DomainService { get; }

        public ILocalizationService LocalizationService { get; }

        public IMediaService MediaService { get; }

        public IRedirectsService RedirectsService { get; }

        public IUmbracoContextAccessor UmbracoContextAccessor { get; }

        public RedirectsImportDependencies(IWebHostEnvironment webHostEnvironment, IDomainService domainService, ILocalizationService localizationService, IMediaService mediaService, IRedirectsService redirectsService, IUmbracoContextAccessor umbracoContextAccessor) {
            WebHostEnvironment = webHostEnvironment;
            DomainService = domainService;
            LocalizationService = localizationService;
            MediaService = mediaService;
            RedirectsService = redirectsService;
            UmbracoContextAccessor = umbracoContextAccessor;
        }

    }

}