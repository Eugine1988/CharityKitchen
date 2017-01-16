using CharityKitchen.CharityKitchenService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CharityKitchen
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Get the user from session
            User user = (User)Session["user"];

            // If user's access level is 1 (Read)
            if (user.AccessLevel == 1)
            {
                // Indicate the access level
                lblInfo.Text = "AccessLevel: Write";
            }
            // If user's access level is 2 (Write)
            else if (user.AccessLevel == 2)
            {
                // Indicate the access level
                lblInfo.Text = "AccessLevel: Read";
            }
            // If user's access level is not 1 or 2 (Deny)
            else
            {
                // Set the user's id to 0 so that use unable to login unless they provide
                // adequate login information
                user.ID = 0;
                // Set the changed user object to session[user]
                Session["user"] = user;
                // Redirect back to login page
                Response.Redirect("~/Pages/Login");
            }
        }

        protected void btnOrders_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Pages/Orders");
        }

        protected void btnMeals_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Pages/Meals");
        }

        protected void btnIngredients_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Pages/Ingredients");
        }

        protected void btnLogOut_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Pages/Login");
        }
    }
}