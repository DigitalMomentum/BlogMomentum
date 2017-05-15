using Examine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using umbraco;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Models;

namespace BlogMomentum.Models
{
    public class BlogListing : RenderModel {
		public BlogListing() : base(UmbracoContext.Current.PublishedContentRequest.PublishedContent) { }


		/// <summary>
		/// Type of blog listing
		/// </summary>
		public enum PageType {
			Landing,
			Author,
			Category,
			Tag,
			Archive
		}

		/// <summary>
		/// This only works for the landing page. Use the Page key for other Page Types
		/// </summary>
		/// <param name="pageType"></param>
		public void Load(PageType pageType) {
			Load(pageType, null);
		}

		/// <summary>
		/// Loads the Page Details and relivant Blog items into the BlogListing Model
		/// </summary>
		/// <param name="pageType">The type of listing</param>
		/// <param name="pageKey">They key used. Such as the tag name or category name</param>
		public void Load(PageType pageType, string pageKey) {
		
			Title = Content.Name;
			

			PageSize = (Content.HasValue("postsPerPage")) ? Content.GetPropertyValue<int>("postsPerPage") : 10;
			
			if (PageSize <= 0) { //Make sure that we dont get any accidental Div/0 Errors
				PageSize = 10;
			}

			IntroContent = (Content.HasValue("openingContent")) ? Content.GetPropertyValue<HtmlString>("openingContent") : null;

			PageListingType = pageType;
			PageKey = pageKey;

			switch (pageType) {
				case PageType.Landing:
					
					BlogEntries = GetPagedBlogPosts();
					break;
				case PageType.Author:
					BlogEntries = GetPagedBlogPostsByAuthor(pageKey);
					break;
				case PageType.Tag:
					BlogEntries = GetPagedBlogPostsByTag(pageKey);
					break;
				case PageType.Category:
					BlogEntries = GetPagedBlogPostsByCategory(pageKey);
					break;
				case PageType.Archive:
					string[] dateSplit = PageKey.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
					int? year = null;
					int? month = null;
					int? day = null;

					if (dateSplit.Length > 0) {
						year = int.Parse(dateSplit[0]);
					}

					if (dateSplit.Length > 1) {
						month = int.Parse(dateSplit[1]);
					}

					if (dateSplit.Length > 2) {
						day = int.Parse(dateSplit[2]);
					}
					BlogEntries = GetPagedBlogPostsByArchive(year, month, day);
					break;
			}
		}





		/// <summary>
		/// Title of the Blog
		/// </summary>
		public string Title { get; set; }

		/// <summary>
		/// Introduction Content that appears before the blog listing
		/// </summary>
		public HtmlString IntroContent { get; set; }

		public PageType PageListingType { get; set; }

		public string PageKey { get; set; }

		/// <summary>
		/// The Current Page of paged results
		/// </summary>
		public int Page { get; set; }


		public string Search { get; set; }

		/// <summary>
		/// Number of Blog posts per page
		/// </summary>
		public int PageSize { get; set; }

		/// <summary>
		/// The total number of Blog posts in this listing
		/// </summary>
		public int TotalPages { get; set; }

		/// <summary>
		/// The Previous page index. This will return 1 (not 0) when on the first page
		/// </summary>
		public int PreviousPage { get; set; }

		/// <summary>
		/// The next page index. This will return the TotalPages when on the last page
		/// </summary>
		public int NextPage { get; set; }

		/// <summary>
		/// Tells us if we are on the first page
		/// </summary>
		public bool IsFirstPage { get; set; }

		/// <summary>
		/// Tells us if we are on the last page
		/// </summary>
		public bool IsLastPage { get; set; }

		//public IEnumerable<IPublishedContent> BlogEntries { get; set; }
		public IEnumerable<IPublishedContent> BlogEntries { get; set; }

		/// <summary>
		/// Gets the unfiltered blog posts for the page
		/// </summary>
		/// <returns>All Blog Posts in desending Date Order</returns>
		private IEnumerable<IPublishedContent> GetPagedBlogPosts() {
			IEnumerable<IPublishedContent> AllPosts = Content.Children;
			if(!string.IsNullOrWhiteSpace(Search)){
				var searchProvider = ExamineManager.Instance.DefaultSearchProvider.Name;
				//var searchResults = ExamineManager.Instance.SearchProviderCollection[searchProvider].Search(Search, true);
				
				var umbracoHelper = new Umbraco.Web.UmbracoHelper(Umbraco.Web.UmbracoContext.Current);

				return GetPagedPosts(umbracoHelper.TypedSearch(Search, true, searchProvider).Where(r=>r.ContentType.Alias == "BlogPost").ToList());
			}

			return GetPagedPosts(AllPosts.ToList());
		}

		/// <summary>
		/// Gets the blog posts for the current page for the given Author
		/// </summary>
		/// <param name="author">Author UrlName of the Author</param>
		/// <returns>Authors Blog Posts</returns>
		private IEnumerable<IPublishedContent> GetPagedBlogPostsByAuthor(string author) {
			var authorNode = uQuery.GetNodesByType("BlogAuthor").Where(r => r.UrlName == author).FirstOrDefault();
			if (authorNode != null) {
				PageKey = authorNode.Name; //Change Pagekey to Authors name instead of URL, to use as labeling on the front end
				return GetPagedPosts(Content.Children.Where(r => r.GetProperty("author").HasValue && Convert.ToInt32(r.GetProperty("author").Value) == authorNode.Id).ToList());
			}
			return new List<IPublishedContent>().AsEnumerable();
		}

		/// <summary>
		/// Gets the blog posts for the current page for the given Category
		/// </summary>
		/// <param name="category">Category Name</param>
		/// <returns>Blog Posts in the given category</returns>
		private IEnumerable<IPublishedContent> GetPagedBlogPostsByCategory(string category) {
			category = category.ToLower();
			var categoryrNode = uQuery.GetNodesByType("BlogCategory").Where(r => r.UrlName.ToLower() == category).FirstOrDefault();
			if (categoryrNode != null) {
				return GetPagedPosts(Content.Children.Where(r => r.GetProperty("categories").HasValue && r.GetProperty("categories").Value.ToString().ToLower().Contains(categoryrNode.Id.ToString())).ToList());
			}
			return new List<IPublishedContent>().AsEnumerable();
		}

		/// <summary>
		/// Gets the blog posts for the current page for the given Tag
		/// </summary>
		/// <param name="category">tag Name</param>
		/// <returns>Blog Posts filtered by tag</returns>
		private IEnumerable<IPublishedContent> GetPagedBlogPostsByTag(string tag) {
			tag = tag.ToLower();
			return GetPagedPosts(Content.Children.Where(r => r.GetProperty("tags").HasValue && r.GetProperty("tags").Value.ToString().ToLower().Contains(tag)).ToList());
	

		}



		private IEnumerable<IPublishedContent> GetPagedBlogPostsByArchive(int? year, int? month, int? day) {

			if(day.HasValue && month.HasValue && year.HasValue) {
				return GetPagedPosts(Content.Children.Where(r => r.GetProperty("entryDate").HasValue
				&& r.GetPropertyValue<DateTime>("entryDate").Year == year.Value
				&& r.GetPropertyValue<DateTime>("entryDate").Month == month.Value
				&& r.GetPropertyValue<DateTime>("entryDate").Day == day.Value
				).ToList());
			} else if (month.HasValue && year.HasValue) {
				return GetPagedPosts(Content.Children.Where(r => r.GetProperty("entryDate").HasValue
					&& r.GetPropertyValue<DateTime>("entryDate").Year == year.Value
					&& r.GetPropertyValue<DateTime>("entryDate").Month == month.Value
					).ToList());
			} else if (year.HasValue) {
				return GetPagedPosts(Content.Children.Where(r => r.GetProperty("entryDate").HasValue
					&& r.GetPropertyValue<DateTime>("entryDate").Year == year.Value
					).ToList());
			}
			
			return GetPagedPosts(Content.Children.Where(r => r.GetProperty("entryDate").HasValue).ToList());
			

		
		}


		/// <summary>
		/// Wwn passed all Posts, it will filter out just the ones needed for the current page
		/// </summary>
		/// <param name="allPosts"></param>
		/// <returns>Filtered list of results for the current page</returns>
		private IEnumerable<IPublishedContent> GetPagedPosts(List<IPublishedContent> allPosts) {
			if (Page == default(int))
				Page = 1;

			//const int pageSize = 5;
			var skipItems = (PageSize * Page) - PageSize;

			//var posts = model.Content.Children.ToList();
			TotalPages = Convert.ToInt32(Math.Ceiling((double)allPosts.Count() / PageSize));

			PreviousPage = Page - 1;
			NextPage = Page + 1;
			if (PreviousPage < 1) {
				PreviousPage = 1;
			}
			if (NextPage > TotalPages) {
				NextPage = TotalPages;
			}

			IsFirstPage = Page <= 1;
			IsLastPage = Page >= TotalPages;

			return allPosts.OrderByDescending(x => x.GetPropertyValue<DateTime>("entryDate")).Skip(skipItems).Take(PageSize);
		}
    }
}
