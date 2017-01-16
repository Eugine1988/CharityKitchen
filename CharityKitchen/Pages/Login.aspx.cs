using CharityKitchen.CharityKitchenService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CharityKitchen.Pages
{
    public partial class Login1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["user"] = new User();
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            // Display message
            lblInfo.Text = "Logging in...";

            // Create new Data-Service
            var svc = new CharityKitchenServiceSoapClient();
            // Get Operation with User from database
            var operation = svc.UserLogin(txtUserName.Text, txtPassword.Text);

            // If operation successful
            if (operation.Success)
            {
                // If the operation returned a User
                if (operation.Data.Count > 0)
                {
                    // Store the user in session (do be able to establish the access level of the user later)
                    Session["user"] = (User)operation.Data[0];
                    // Redirect to the main page
                    Response.Redirect("~/Default");
                }
            }
            else
            {
                // If data service unsuccessfull, then error message will be displayed
                lblInfo.ForeColor = System.Drawing.Color.Red;
                lblInfo.Text = operation.Message;
            }
        }
    }
}