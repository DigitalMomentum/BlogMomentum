<%@ Control Language="C#" AutoEventWireup="true"  %>
<%-- NOTE: YOU CAN DELETE THIS FILE AFTER INSTALLLING BLOG MOMENTUM --%>
<a href="http://blogmomentum.digitalmomentum.com.au/">
	<img src="/UmbracoBookshelf/Blog%20Momentum/images/logo.png" /></a>
<h1>Blog Momentum succesfully installed</h1>
<p>
	Blog Momentum is a Blog plugin for Umbraco with minimal Bootstrap 3 styling. This package has been designed for easy integration with an existing Umbraco website. 
</p>
<p>
	This plugin is not a starter package, so you will need to set-up your main template, navigation and non Blog related DocTypes yourself.
</p>
<p>
	This is a new open source project that you can find on GitHub managed by Digital Momentum.
</p>
<h2>
	Integration Instructions:
</h2>
<h3>
	1. Templates
</h3>
<p>
	You will need to add your master page template to the blog. 
Go To Settings->Templates->Blog Page.
Click the Properties tab and select your Master Template.
Click Save
</p>
<h3>
	2. [Optional] DocumentTypes - Inherit Properties from your Master DocType
</h3>
<p>
	If you have custom properties in your Master Document Type, that you would like included in you blog pages (e.g. Meta Title / Description), you will need to make a couple of changes.
Go To Settings->Document types->Blog Pages->Blog Listing.
Select the Document Type Compositions that you would like to apply to your document type. 
Click Save.
Do the same for the Blog Post Document Type.
</p>
<h3>
	3. Document Type - Allow the creating of the Blog Listing.
</h3>
<p>
	To allow content editors to add the Blog to the site, you will have to specify where they will be allowed to add the listing of blog posts. This usually will be under the Home or Textpage nodes.
Pick the document type that you would like to allow the creation of Blog posts under (e.g. Home) 
Under the Structure Tab tick the Allowed child node type of "Blog Listing"
Click Save
</p>
<h3>
	4. Content - Add to your website
</h3>
<p>
	Under the root of your Content section you should find an un-piblished Node called "Blog Extras".
Right Click on "Blog Extras and click Publish.
Select both tick Boxes ("Publish Blog Extras and all its subpages" & "Include unpublished child pages").
Click Publish
This is where you Authors and Categories are managed. These pages will not show directly on your site.
</p>
<p>
	Next you will need to add the Blog Listing Page.
Right click on the node where you want to add your blog
Click Create and add the name of your Blog
Make any adjustments to the properties and click Save & Publish
</p>
<h3>
	5. Create your first Blog Post
</h3>
<p>
	Right click on the blog listing page and select "Create"
Enter the title of the blog post, the main content and any other options and click "Save & Publish". If all goes well, you'll have a new blog post on your site.
</p>
<p>

	<%
			try {
				System.Net.WebRequest request = System.Net.WebRequest.Create("http://blogmomentum.digitalmomentum.com.au//Ajax/logInstall.aspx");
				System.Net.WebResponse response = request.GetResponse();
			} catch { }
		}
		 %>
