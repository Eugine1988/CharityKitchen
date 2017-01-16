using CharityKitchen.CharityKitchenService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CharityKitchen.Pages
{
    public partial class Orders : System.Web.UI.Page
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
                gvOrders.Columns[3].Visible = false;
                // Disable the add new order button
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
            // Set session to 0, to indicate the OrderID
            Session["order_edit"] = 0;
            // Redirect to OrderEdit Page
            Response.Redirect("~/Pages/OrderEdit");
        }

        // When Select-Row is clicked in Grid-View
        protected void gvOrders_SelectedIndexChanged(object sender, EventArgs ev)
        {
            // Set appropriate session, to indicate the OrderID
            Session["order_edit"] = int.Parse(gvOrders.SelectedRow.Cells[1].Text);
            // Redirect to OrderEdit Page
            Response.Redirect("~/Pages/OrderEdit");
        }

        // When Delete-Row is clicked in Grid-View
        protected void gvOrders_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            // Get the OrderID of selected row
            int orderID = int.Parse(gvOrders.Rows[e.RowIndex].Cells[1].Text);

            // Iinstantiate a Service
            var svc = new CharityKitchenServiceSoapClient();
            // Create space in RAM for two service-operations
            ServiceOperation[] ops = new ServiceOperation[2];

            // Perform two operations, first to delete the OrderLines then delete the Order
            ops[0] = svc.MealsOrdered_Update(new MealOrderLine[0], orderID);
            ops[1] = svc.Order_Delete(orderID);

            bool errorFound = false;
            // For both operations, display success/error messages
            foreach (var op in ops)
            {
                // If operation successful
                if (!op.Success)
                {
                    // Display error on screen & break out of loop
                    lblInfo.ForeColor = System.Drawing.Color.Red;
                    lblInfo.Text = op.Message;
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
            var operation = svc.Order_GetAll();

            // If operation is successful
            if (operation.Success)
            {
                // Add data from operation to the GridView
                gvOrders.DataSource = operation.Data;
                gvOrders.DataBind();
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