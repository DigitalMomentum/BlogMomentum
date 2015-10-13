using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core;
using Umbraco.Core.Services;
using Umbraco.Web.Routing;

namespace BlogMomentum.Events {
	public class AppEvents : ApplicationEventHandler {

		protected override void ApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext) {
			base.ApplicationStarting(umbracoApplication, applicationContext);
			//For Each request, check if it is a Blog URL
			ContentFinderResolver.Current.InsertType<Routing.ContentFinder>();

		}

		protected override void ApplicationInitialized(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext) {
			
			base.ApplicationInitialized(umbracoApplication, applicationContext);
		}

		protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext) {
			ContentService.Created+=ContentService_Created;
			try {
				StartUp.CheckContentNodes.CreateContentNodesIfNotExist();
			} catch { }
		}

		void ContentService_Created(IContentService sender, Umbraco.Core.Events.NewEventArgs<Umbraco.Core.Models.IContent> e) {
			if (e.Entity.ContentType.Alias == "BlogPost") {
				//Set the current date to the "Entry Date" field for Posts
				if (e.Entity.HasProperty("entryDate")) {
					e.Entity.SetValue("entryDate", DateTime.Now);
				}
			} else if (e.Entity.ContentType.Alias == "BlogListing") {
				//Set the current date to the "Entry Date" field for Posts
				if (e.Entity.HasProperty("postsPerPage")) {
					e.Entity.SetValue("postsPerPage", 10);
				}
			}
		}
           



	}
}