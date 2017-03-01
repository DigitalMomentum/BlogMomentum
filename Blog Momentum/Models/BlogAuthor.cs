using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Umbraco.Core.Models;
using Umbraco.Web.Models;

namespace BlogMomentum.Models {

	/// <summary>
	/// Model of an Author
	/// </summary>
	public class BlogAuthor {
		/// <summary>
		/// Initialises the BlogAuthor Object
		/// </summary>
		/// <param name="NodeId">Umbraco NodeId of the Author</param>
		public BlogAuthor(int NodeId) {

			var umbracoHelper = new Umbraco.Web.UmbracoHelper(Umbraco.Web.UmbracoContext.Current);
			IPublishedContent authorNode = umbracoHelper.Content(NodeId);
			Name = authorNode.Name;
			PhotoUrl = null;
			if (authorNode.GetProperty(("photo")).Value != null){
				PhotoUrl = JsonConvert.DeserializeObject<ImageCropDataSet>(authorNode.GetProperty("photo").Value.ToString());
			}
			Blurb = (authorNode.GetProperty("blurb").Value != null)? (HtmlString)authorNode.GetProperty("blurb").Value : new HtmlString("");
			UrlName = authorNode.UrlName;
		}

		/// <summary>
		/// Name Of the Author
		/// </summary>
		public string Name { get; set; }
		
		/// <summary>
		/// Photo of the Author
		/// </summary>
		public ImageCropDataSet PhotoUrl { get; set; }
		
		/// <summary>
		/// Small Description of the Author
		/// </summary>
		public HtmlString Blurb { get; set; }

		/// <summary>
		/// The Author Name Converted to URL (e.g. "John Smith" will be "John-Smith")
		/// </summary>
		public string UrlName { get; set; }
	}
}
