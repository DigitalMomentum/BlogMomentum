using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Models;
using BlogMomentum;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace BlogMomentum.Models {
	public class BlogPost : RenderModel {
		public BlogPost() : base(UmbracoContext.Current.PublishedContentRequest.PublishedContent) { }
		public BlogPost(IPublishedContent content)
			: base(content) {
			Load();
		}

		public void Load() {
			var umbracoHelper = new Umbraco.Web.UmbracoHelper(Umbraco.Web.UmbracoContext.Current);
			BlogRoot = Content.Parent;
			EntryDate = (Content.HasValue("entryDate")) ? DateTime.Parse(Content.GetPropertyValue<string>("entryDate")) : Content.CreateDate;
			BlogContent = Content.GetPropertyValue<HtmlString>("content");
            //Categories = (Content.HasValue("categories")) ? umbracoHelper.Content(Content.GetPropertyValue<string>("categories").Split(',')) : new List<IPublishedContent>().AsEnumerable();
            Categories = (Content.HasValue("categories")) ? Content.GetPropertyValue<IEnumerable<IPublishedContent>>("categories") : new List<IPublishedContent>().AsEnumerable();
            Tags = (Content.HasValue("tags")) ? Content.GetPropertyValue<string>("tags").Split(',') : new string[0];
			MainImage = (Content.HasValue("mainImage")) ? JsonConvert.DeserializeObject<ImageCropDataSet>(Content.GetPropertyValue <string>("mainImage") ): null;
			
			Blurb = (Content.HasValue("blurb")) ? Content.GetPropertyValue<HtmlString>("blurb") : (HtmlString)umbracoHelper.Truncate(BlogContent, 800);
			Title = Content.Name;
			CommentsEnabled = (Content.HasValue("commentsEnabled", true)) ? Content.GetPropertyValue<bool>("commentsEnabled", true) : false;
			DisqusShortname = (Content.HasValue("disqusShortname", true)) ? Content.GetPropertyValue<string>("disqusShortname", true) : null;
			TwitterUsername = Content.GetPropertyValue<string>("twitterUsername", true);
			ShareDescription = Content.GetPropertyValue<string>("shareDescription", true);
			FullUrl = Content.UrlWithDomain();
			//var authorId = Content.GetProperty("author");
			//Author = (authorId != null && authorId.HasValue) ? new BlogAuthor(
			//	Convert.ToInt32( authorId.Value)
			//	) : null;
            Author = (Content.HasValue("author"))? new BlogAuthor(Content.GetPropertyValue<IEnumerable<IPublishedContent>>("author").FirstOrDefault().Id) : null;
            EnableShareIcons = (Content.HasValue("enableShareIcons", true)) ? Content.GetPropertyValue<bool>("enableShareIcons", true) : false;
		}

		public IPublishedContent BlogRoot { get; set; }
		public BlogAuthor Author { get; set; }
		public DateTime EntryDate { get; set; }
		public HtmlString BlogContent { get; set; }
		public string Title { get; set; }
		public ImageCropDataSet MainImage { get; set; }
		public HtmlString Blurb { get; set; }
		public IEnumerable<IPublishedContent> Categories { get; set; }
		public bool CommentsEnabled { get; set; }
		public string DisqusShortname { get; set; }
		public string TwitterUsername { get; set; }
		public string ShareDescription { get; set; }
		public string[] Tags { get; set; }
		public string GetAuthorUrl() {
			if (Author != null) {
				return BlogRoot.Url + "author/" + Author.UrlName;
			}
			return null;
		}

		public string FullUrl { get; set; }

		public bool EnableShareIcons { get; set; }
	}



}
