using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using Umbraco.Web.Mvc;
using BlogMomentum.Models;
using System.Web;

namespace BlogMomentum.Controllers {
	public class BlogPostController : RenderMvcController {
		public ActionResult BlogPost(BlogPost model) {

			model.Load();

			return CurrentTemplate(model);
		}
	}


}
