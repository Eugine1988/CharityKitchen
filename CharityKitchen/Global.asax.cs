﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace CharityKitchen
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        // Code below maunally added, runs when a session starts
        void Session_Start(object sender, EventArgs e)
        {
            // Set the session[user] to a user object
            Session["user"] = new CharityKitchenService.User();
        }
    }
}