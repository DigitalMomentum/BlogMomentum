using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Umbraco.Web.Models;

namespace BlogMomentum.Models {
	public class RssModel:RenderModel {

		public string[] Parameters { get; set; }



		public RssModel(RenderModel model, string[] parameters)
			: base(model.Content, model.CurrentCulture)

	{
		var umbracoHelper = new Umbraco.Web.UmbracoHelper(Umbraco.Web.UmbracoContext.Current);
		Title = Content.Name;
		IntroContent = (Content.GetProperty("content").HasValue)?StripHtml(Content.GetProperty("content").Value.ToString()):"";
		SiteURL = "http://" + HttpContext.Current.Request.Url.Host;

		AllPosts = Content.Children.OrderByDescending(r => r.GetProperty("entryDate").Value);
		var latestPost = AllPosts.OrderByDescending(r => r.UpdateDate).FirstOrDefault();
		//var latestPost = AllPosts.FirstOrDefault();
		LatestUpdate = (latestPost != null) ? latestPost.UpdateDate : DateTime.Now;
	}
		public string StripHtml(string Htmlstr) {
			return Regex.Replace(Htmlstr, @"<[^>]*>", String.Empty);
		}

		public string Title { get; set; }

		public string IntroContent { get; set; }

		public IOrderedEnumerable<Umbraco.Core.Models.IPublishedContent> AllPosts { get; set; }

		public string SiteURL { get; set; }

		public DateTime LatestUpdate { get; set; }
	}
}