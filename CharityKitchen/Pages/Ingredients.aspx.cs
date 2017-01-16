using CharityKitchen.CharityKitchenService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CharityKitchen.Pages
{
    public partial class Ingredients : System.Web.UI.Page
    {
        #region init

        // When the page loads
        protected void Page_Load(object sender, EventArgs e)
        {
            // Get the user from session
            User user = (User)Session["user"];

            // If user's access level is 1 (Read)
            if (user.AccessLevel == 1)
            {
                // Get the data for Grid-View
                UpdateGridView();
                // Indicate the access level
                lblInfo.Text = "AccessLevel: Write";
            }
            // If user's access level is 2 (Write)
            else if (user.AccessLevel == 2)
            {
                // Get the data for Grid-View
                UpdateGridView();

                // Disable Delete column in the Grid-View
                gvIngredients.Columns[5].Visible = false;
                // Disable the add new ingredient button
                btnNew.Enabled = false;
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

        #endregion init

        #region events

        // When button btnNew clicked
        protected void btnNew_Click(object sender, EventArgs e)
        {
            // Set session to 0, to indicate the IngredientID
            Session["ingredient_edit"] = 0;
            // Redirect to IngredientEdit Page
            Response.Redirect("~/Pages/IngredientEdit");
        }

        // When Select-Row is clicked in Grid-View
        protected void gvIngredients_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Set appropriate session, to indicate the IngredientID
            Session["ingredient_edit"] = int.Parse(gvIngredients.SelectedRow.Cells[1].Text);
            // Redirect to IngredientEdit Page
            Response.Redirect("~/Pages/IngredientEdit");
        }

        // When Delete-Row is clicked in Grid-View
        protected void gvIngredients_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            // Get the MealID of selected row
            int ingredientID = int.Parse(gvIngredients.Rows[e.RowIndex].Cells[1].Text);

            // Instantiate a Service
            var svc = new CharityKitchenServiceSoapClient();
            
            // Perform operation to selete the selected ingredient from database
            var operation = svc.Ingredient_Delete(ingredientID);

            // If operation successful
            if (operation.Success)
            {
                // Display success message on screen
                lblInfo.ForeColor = System.Drawing.Color.DarkGreen;
                lblInfo.Text = operation.Message;

                // Update the Grid-View
                UpdateGridView();
            }
            else
            {
                // Display error on screen & break out of loop
                lblInfo.ForeColor = System.Drawing.Color.Red;

                // If the Meal exists in an Order, display user friendly message
                if (operation.Message == "Database error: The record cannot be deleted or changed because table 'tblRecipes' includes related records.")
                    lblInfo.Text = "Cannot Delete: This Ingredient is used in a Meal. Please delete that Meal first";
                // If other message, display that message
                else lblInfo.Text = operation.Message;
            }
        }
        
        #endregion events

        #region methods

        // Gets that data from data-service & binds it to GridView
        private void UpdateGridView()
        {
            // Instantiate Service
            var svc = new CharityKitchenServiceSoapClient();
            // Create new operation & get data from service
            var operation = svc.Ingredient_GetAll();

            // If operation is successful
            if (operation.Success)
            {
                // Add data from operation to the GridView
                gvIngredients.DataSource = operation.Data;
                gvIngredients.DataBind();
            }
            else
            {
                // If data service unsuccessfull, then error message will be displayed
                lblInfo.ForeColor = System.Drawing.Color.Red;
                lblInfo.Text = operation.Message;
            }
        }

        #endregion methods
    }
}