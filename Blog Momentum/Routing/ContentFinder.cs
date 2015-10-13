using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using umbraco;
using Umbraco.Web;
using Umbraco.Web.Routing;

namespace BlogMomentum.Routing {
	public class ContentFinder : IContentFinder {

		/// <summary>
		/// For each request, this function checks to see if its a virtual Blog URL targeted to 
		/// Authors, Categories, Tags & RSS and routes accordingly
		/// </summary>
		/// <param name="contentRequest"></param>
		/// <returns>True for a virtual URL or false for an exsting CMS Page</returns>
		public bool TryFindContent(PublishedContentRequest contentRequest) {
			
			string fullUri = contentRequest.Uri.AbsolutePath;
			Regex reg = new Regex("author|category|tag|rss");
			if (reg.IsMatch(fullUri)) { //Simple regex match to see if it's a possible url to redirect. If not, lets not worry about the overhead and just return false
				if (uQuery.GetNodesByType("BlogEntry").Where(r => r.Url.StartsWith(fullUri, StringComparison.InvariantCultureIgnoreCase)).Count() > 0) {
					//We've found a match to a blog entry, so no need to redirect
				
					return false;
				}

				//Get All Blog listings. This should only be a handful at Max
				var allNodes = uQuery.GetNodesByType("BlogListing").OrderByDescending(n => n.Level);

				foreach (var node in allNodes) {

					string parentUri = node.Url;
					bool isChild = fullUri.StartsWith(parentUri, StringComparison.InvariantCultureIgnoreCase);

					if (isChild) {
						//Its a virtual URL that is a child of a Blog listing.
						contentRequest.PublishedContent = new UmbracoHelper(UmbracoContext.Current).TypedContent(node.Id);
						
						return true;
					}
				}
			}
			
			return false;
		}

	}
}