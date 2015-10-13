using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace BlogMomentum.StartUp {
	class CheckContentNodes {
		public static void CreateContentNodesIfNotExist() {
			IContentService contentService = ApplicationContext.Current.Services.ContentService;

			IContent blogExtras = contentService.GetRootContent().Where(r => r.ContentType.Alias == "BlogExtras").FirstOrDefault();
			if (blogExtras == null) {
				blogExtras = contentService.CreateContent("Blog Extras", -1, "BlogExtras");
				contentService.SaveAndPublishWithStatus(blogExtras);
			}

			IContent blogAuthors = blogExtras.Children().Where(r => r.ContentType.Alias == "BlogAuthors").FirstOrDefault();
			if (blogAuthors == null) {
				blogAuthors = contentService.CreateContent("Authors", blogExtras, "BlogAuthors");
				contentService.SaveAndPublishWithStatus(blogAuthors);
			}

			IContent blogCategories = blogExtras.Children().Where(r => r.ContentType.Alias == "BlogCategories").FirstOrDefault();
			if (blogCategories == null) {
				blogCategories = contentService.CreateContent("Categories", blogExtras, "BlogCategories");
				contentService.SaveAndPublishWithStatus(blogCategories);
			}
		}
	}
}
