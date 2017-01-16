using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CharityKitchen
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Create variable to keep track of user that is contained in the session
            CharityKitchenService.User user = Session["user"] as CharityKitchenService.User;

            // If the user in the session[user] is not a specific user
            if (user.ID == 0)
            {
                // Take back to Login page
                Response.Redirect("~/Pages/Login");
            }
        }
    }
}