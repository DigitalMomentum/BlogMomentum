using BlogMomentum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Core.Models;
using Umbraco.Web.Mvc;

namespace BlogMomentum.Controllers {
	public class BlogController : SurfaceController {
		[ChildActionOnly]
		public ActionResult BlogPostSummary(IPublishedContent con) {
			return PartialView("Blog/PostSummary", new BlogPost(con));
		}
	}
}