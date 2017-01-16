using CharityKitchen.CharityKitchenService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CharityKitchen.Pages
{
    public partial class Meals : System.Web.UI.Page
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
                gvMeals.Columns[3].Visible = false;
                // Disable the add new meal button
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
            // Set session to 0, to indicate the MealID
            Session["meal_edit"] = 0;
            // Redirect to MealEdit Page
            Response.Redirect("~/Pages/MealEdit");
        }

        // When Select-Row is clicked in Grid-View
        protected void gvMeals_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Set appropriate session, to indicate the MealID
            Session["meal_edit"] = int.Parse(gvMeals.SelectedRow.Cells[1].Text);
            // Redirect to MealEdit Page
            Response.Redirect("~/Pages/MealEdit");
        }

        // When Delete-Row is clicked in Grid-View
        protected void gvMeals_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            // Get the MealID of selected row
            int mealID = int.Parse(gvMeals.Rows[e.RowIndex].Cells[1].Text);

            // Instantiate a Service
            var svc = new CharityKitchenServiceSoapClient();
            // Create space in RAM for two service-operations
            ServiceOperation[] ops = new ServiceOperation[2];

            // Perform two operations, first to delete the MealIngredientLine then delete the Meal
            ops[0] = svc.MealIngredientLine_Update(new MealIngredientLine[0], mealID);
            ops[1] = svc.Meal_Delete(mealID);

            bool errorFound = false;
            // For both operations, display success/error messages
            foreach (var op in ops)
            {
                // If operation successful
                if (!op.Success)
                {
                    // Display error on screen & break out of loop
                    lblInfo.ForeColor = System.Drawing.Color.Red;

                    // If the Meal exists in an Order, display user friendly message
                    if (op.Message == "Database error: The record cannot be deleted or changed because table 'tblOrderLine' includes related records.")
                        lblInfo.Text = "Cannot Delete: This Meal is used in an Order. Please delete that Order first";
                    // If other message, display that message
                    else lblInfo.Text = op.Message;

                    // Indicate that an error has been found
                    errorFound = true;
                    break;
                }
            }
            // If an error has not been found
            if (!errorFound)
            {
                // Display success message on screen
                lblInfo.ForeColor = System.Drawing.Color.DarkGreen;
                lblInfo.Text = ops[1].Message;

                // Update the Grid-View
                UpdateGridView();
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
            var operation = svc.Meal_GetAll();

            // If operation is successful
            if (operation.Success)
            {
                // Add data from operation to the GridView
                gvMeals.DataSource = operation.Data;
                gvMeals.DataBind();
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