using CharityKitchen.CharityKitchenService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CharityKitchen.Pages
{
    public partial class IngredientEdit : System.Web.UI.Page
    {
        #region vars

        // Create variables for storing the necessary values
        static Ingredient ingredient;

        #endregion vars

        #region init

        // When the page loads
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            // Run method to implemet user's access-level
            ImplementAccessLevel();
            // Create new ServiceSoapClient
            var svc = new CharityKitchenServiceSoapClient();
            DisplayIngredient(svc); // Display the record
        }

        #endregion init

        #region events

        // When button to Save everything is clicked
        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveIngredient(); // Save the record
        }

        // When button to go back to all Meals page is clicked
        protected void Back_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Pages/Ingredients"); // Go to the indicated page
        }

        #endregion events

        #region methods

        // Gets Ingredient from data-service & displays on screen
        private void DisplayIngredient(CharityKitchenServiceSoapClient _svc)
        {
            // Create id integer to store the id of the record
            int id = 0;
            // Try to get the id of the record from session
            try { id = (int)Session["ingredient_edit"]; }
            catch { }

            if (id != 0)
            {
                // Set service operation
                var operation = _svc.Ingredient_GetById(id);

                // If data service operation is successful
                if (operation.Success)
                {
                    // Display the values from service on screen
                    ingredient = (Ingredient)operation.Data[0];
                    lblID.Text = ingredient.ID.ToString();
                    txtName.Text = ingredient.Name;
                    txtCost.Text = ingredient.Cost.ToString();
                    txtUnit.Text = ingredient.Unit.ToString();
                }
                else
                {
                    // If data service unsuccessfull, then error message will be displayed
                    btnSave.Enabled = false;
                    lblInfo.ForeColor = System.Drawing.Color.Red;
                    lblInfo.Text = operation.Message;
                }
            }
            else
            {
                // If id is 0, then this is new record
                ingredient = new Ingredient();
                lblID.Text = ingredient.ID.ToString();
            }
        }

        // Saves the details of the Ingredient to the database through DataService
        private void SaveIngredient()
        {
            var svc = new CharityKitchenServiceSoapClient(); // Create new ServiceSoapClient
            ServiceOperation operation; // Create new ServiceOperation

            // Set the values of the static record to values from the screen
            ingredient.Name = txtName.Text;
            ingredient.Unit = txtUnit.Text;
            try
            {
                ingredient.Cost = decimal.Parse(txtCost.Text);
            }
            catch (Exception)
            {
                // If data service unsuccessfull, display error message on screen
                lblInfo.ForeColor = System.Drawing.Color.Red;
                lblInfo.Text = "Error: Cost value needs to be a Decimal";
            }

            // Save meal to database
            operation = svc.Ingredient_Save(ingredient);

            // If data service successfull
            if (operation.Success)
            {
                // Display success message on screen
                lblInfo.ForeColor = System.Drawing.Color.DarkGreen;
                lblInfo.Text = operation.Message;
            }
            else
            {
                // If data service unsuccessfull, display error message on screen
                lblInfo.ForeColor = System.Drawing.Color.Red;
                lblInfo.Text = operation.Message;
            }
        }

        // This method to implemet user's access-level
        private void ImplementAccessLevel()
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
                // Disable the btnSave button
                btnSave.Enabled = false;
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

        #endregion methods
    }
}