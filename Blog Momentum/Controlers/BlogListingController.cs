using BlogMomentum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using umbraco;
using Umbraco.Core.Models;
using Umbraco.Web.Mvc;

namespace BlogMomentum.Controllers {
	public class BlogListingController : RenderMvcController {

		public ActionResult BlogListing(BlogListing model) {

			string nodeUrl = model.Content.Url;
			string currentUrl = Request.Url.AbsolutePath;
			currentUrl = currentUrl.TrimStart('/');

			string[] urlSegments = Uri.UnescapeDataString(currentUrl).ToLower().Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

			//if (urlSegments.Length > 1) {
			string type = null;
				for (int i = 0; i < urlSegments.Length; i++) {
					switch (urlSegments[i]) {
						case "rss":
						//return View("BlogRss", new RssModel(model, urlSegments));
						type = "rss";
							return View("BlogRss", new RssModel(model, urlSegments));
						case "author":
						//model.BlogEntries = GetPagedBlogPostsByAuthor(model, urlSegments[i + 1]);
						//return CurrentTemplate(model);
						type = "author";
						model.Load(Models.BlogListing.PageType.Author, Uri.UnescapeDataString(urlSegments[i + 1]));
							break;
						case "tag":
						//model.BlogEntries = GetPagedBlogPostsByTag(model, urlSegments[i + 1]);
						//return CurrentTemplate(model);
						type = "tag";
						model.Load(Models.BlogListing.PageType.Tag, Uri.UnescapeDataString(urlSegments[i + 1]));
							break;
						case "category":
						//model.BlogEntries = GetPagedBlogPostsByCategory(model, urlSegments[i + 1]);
						//return CurrentTemplate(model);
						type = "category";
						model.Load(Models.BlogListing.PageType.Category, Uri.UnescapeDataString(urlSegments[i + 1]));
							break;
					}
				}
			//if (urlSegments[1] == "rss") {

			//} else {
			//	return View("BlogEntry", new BlogEntry(model));
			//}
			//} else {
			if (type == null) {
				//Could not find a type so assume that its the landing page
				model.Load(Models.BlogListing.PageType.Landing);
			}
			//}
			//return base.Index(model);

			//model.BlogEntries = GetPagedBlogPosts(model);
			//model.Load();
			return CurrentTemplate(model);
		}

		//private static IEnumerable<IPublishedContent> GetPagedBlogPosts(BlogListing model) {
		//	IEnumerable<IPublishedContent> allPosts = model.Content.Children;
			
		//	return GetPagedPosts(model, model.Content.Children.ToList());
		//}

		//private static IEnumerable<IPublishedContent> GetPagedBlogPostsByAuthor(BlogListing model, string author) {
		//	var authorNode = uQuery.GetNodesByType("BlogAuthor").Where(r => r.UrlName == author).FirstOrDefault();
		//	if (authorNode != null) {
		//		return GetPagedPosts(model, model.Content.Children.Where(r => r.GetProperty("author").HasValue && Convert.ToInt32(r.GetProperty("author").Value) == authorNode.Id).ToList());
		//	}
		//	return new List<IPublishedContent>().AsEnumerable();
		//}


		//private static IEnumerable<IPublishedContent> GetPagedBlogPostsByCategory(BlogListing model, string category) {
		//	var categoryrNode = uQuery.GetNodesByType("BlogCategory").Where(r => r.UrlName == category).FirstOrDefault();
		//	if (categoryrNode != null) {
		//		return GetPagedPosts(model, model.Content.Children.Where(r => r.GetProperty("categories").HasValue && r.GetProperty("categories").Value.ToString().Contains(categoryrNode.Id.ToString())).ToList());
		//	}
		//	return new List<IPublishedContent>().AsEnumerable();
		//}

		//private static IEnumerable<IPublishedContent> GetPagedBlogPostsByTag(BlogListing model, string tag) {
			
		//		return GetPagedPosts(model, model.Content.Children.Where(r => r.GetProperty("tags").HasValue && r.GetProperty("tags").Value.ToString().Contains(tag)).ToList());
	

		//}


		//private static IEnumerable<IPublishedContent> GetPagedPosts(BlogListing model, List<IPublishedContent> allPosts) {
		//	string bob = model.Search;
		//	if (model.Page == default(int))
		//		model.Page = 1;

		//	const int pageSize = 5;
		//	var skipItems = (pageSize * model.Page) - pageSize;

		//	//var posts = model.Content.Children.ToList();
		//	model.TotalPages = Convert.ToInt32(Math.Ceiling((double)allPosts.Count() / pageSize));

		//	model.PreviousPage = model.Page - 1;
		//	model.NextPage = model.Page + 1;

		//	model.IsFirstPage = model.Page <= 1;
		//	model.IsLastPage = model.Page >= model.TotalPages;

		//	return allPosts.OrderByDescending(x => x.CreateDate).Skip(skipItems).Take(pageSize);
		//}
	}


}

