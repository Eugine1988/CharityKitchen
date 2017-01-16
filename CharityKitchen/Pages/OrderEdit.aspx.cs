using CharityKitchen.CharityKitchenService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CharityKitchen.Pages
{
    public partial class MealOrder : System.Web.UI.Page
    {
        #region vars

        // Create variables for storing the necessary values
        static Order order;
        static List<Meal> meals = new List<Meal>();
        static List<MealOrderLine> mealsOrdered = new List<MealOrderLine>();

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
            DisplayMeals(svc); // Populate the combo-box wil all meals
            if(DisplayOrder(svc)) // If false, it means that this is new record, no need to get mealsOrdered from database
                DisplayMealsOrdered(svc); // Display the MealOrderLines
        }

        #endregion init

        #region events

        // When button to Save everything is clicked
        protected void btnSave_Click(object sender, EventArgs e)
        {
            // NB: Only save the OrderLine if Order is saved successfully
            if(SaveOrder()) // Run method to save the Order details
                SaveMealsOrdered(); // Run method to save the MealOrderLines
        }

        // When button to go back to all Orders page is clicked
        protected void Back_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Pages/Orders"); // Go to the indicated page
        }

        // When button clicked to add a meal to GridView displaying meals ordered
        protected void btnAddMeal_Click(object sender, EventArgs e)
        {
            // Create valiable to contain value of selected index in cboMeal
            int index = cboMeal.SelectedIndex;

            // For determining if the meal already been ordered before (wether to edit MealOrderLine-quantity or add new MealOrderLine)
            bool found = false;
            // Go thorough List<MealOrderLine> to see if selected meal matches any meals already ordered
            foreach (var item in mealsOrdered)
            {
                if (item.MealName == cboMeal.SelectedValue)
                {
                    // Just add +1 to meal quantity if it matches the meals selected to be added
                    item.MealQuantity++;
                    found = true;
                    break;
                }
            }

            // If meal not found in mealsOrdered
            if (!found)
            {
                // Create new MealOrderLine & set it's values
                MealOrderLine neaMealOrderLine = new MealOrderLine();
                neaMealOrderLine.OrderID = order.OrderID;
                neaMealOrderLine.MealID = meals[index].MealID;
                neaMealOrderLine.MealName = meals[index].MealName;
                neaMealOrderLine.MealQuantity = 1;

                // Add the new MealOrderLine to mealsOrdered list
                mealsOrdered.Add(neaMealOrderLine);
            }

            // Display the changes in Grid-View
            gvMeals.DataSource = mealsOrdered;
            gvMeals.DataBind();
        }

        // When Delete button pressed to delete a meal from gvMeals
        protected void gvMeals_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            // Get the index of the row selected (will equal the index of the MealOrderLine)
            int index = e.RowIndex;

            // If the Quantity of the meal to delete is more than 1
            if (mealsOrdered[index].MealQuantity > 1)
            {
                // Just reduce quantity -1
                mealsOrdered[index].MealQuantity--;
            }
            // If only one of this meals been ordered
            else
            {
                // Remove that meal from the list of MealOrderLines (mealsOrdered)
                mealsOrdered.RemoveAt(index);
            }

            // Display the changes in Grid-View
            gvMeals.DataSource = mealsOrdered;
            gvMeals.DataBind();
        }

        // When button to display/get all ingredients for this order is clicked
        protected void btnGetIngredients_Click(object sender, EventArgs e)
        {
            // Redirect to Order_GetIngredients Page
            Response.Redirect("~/Pages/Order_GetIngredients");
        }

        #endregion events

        #region methods

        // Gets list of meals from service & displays on screen
        private void DisplayMeals(CharityKitchenServiceSoapClient _svc)
        {
            // operation will get data from database
            var operation = _svc.Meal_GetAll();

            // Create array for storing meal names (To be able to put into combo-box)
            String[] mealNames = new String[operation.Data.Count];

            // If data service operation is successful
            if (operation.Success)
            {
                // Add the data from database to list
                for (int i = 0; i < operation.Data.Count; i++)
                {
                    meals.Add((Meal)operation.Data[i]); // Update List of meals
                    mealNames[i] = meals[i].MealName; // Update array of mealNames
                }

                // Add the list of meals to the combo-box & get it to display in cbo
                cboMeal.DataSource = mealNames;
                cboMeal.DataBind();
            }
            else
            {
                // If data service unsuccessfull, then error message will be displayed
                lblInfo.ForeColor = System.Drawing.Color.Red;
                lblInfo.Text = operation.Message;
            }
        }

        // Gets Order from data-service & displays on screen
        private bool DisplayOrder(CharityKitchenServiceSoapClient _svc)
        {
            // Create id integer to store the id of the record
            int id = 0;
            // Try to get the id of the record from session
            try { id = (int)Session["order_edit"]; }
            catch { }

            if (id != 0)
            {
                // Set service operation
                var operation = _svc.Order_GetById(id);

                // If data service operation is successful
                if (operation.Success)
                {
                    // Display the values from service on screen
                    order = (Order)operation.Data[0];
                    lblID.Text = order.OrderID.ToString();
                    txtCustomerName.Text = order.CustomerName;
                    txtC_Address1.Text = order.CustomerAddress1;
                    txtC_Address2.Text = order.CustomerAddress2;
                    txtOrderDate.Text = order.OrderDate.ToString();
                }
                else
                {
                    // If data service unsuccessfull, then error message will be displayed
                    btnSave.Enabled = false;
                    lblInfo.ForeColor = System.Drawing.Color.Red;
                    lblInfo.Text = operation.Message;
                }
                return true;
            }
            else
            {
                // If id is 0, then this is new record
                order = new Order();
                mealsOrdered.Clear(); // Clear the static list of meal-order-lines
                lblID.Text = order.OrderID.ToString();
                return false;
            }
        }

        // Updates & Displays the meals ordered in this order
        private void DisplayMealsOrdered(CharityKitchenServiceSoapClient _svc)
        {
            // In case mealsOrdered contains data, clear it, so data won't be duplicated
            mealsOrdered.Clear();

            // operation will get & hold the data from database
            var operation = _svc.MealsOrdered_ByOrderId(order.OrderID);

            // If data service operation is successful
            if (operation.Success)
            {
                // Add the data from database to list
                for (int i = 0; i < operation.Data.Count; i++)
                    mealsOrdered.Add((MealOrderLine)operation.Data[i]);

                // Add meals from MealOrderLines to GridView & display on screen
                gvMeals.DataSource = mealsOrdered;
                gvMeals.DataBind();
            }
            else
            {
                // If data service unsuccessfull, then error message will be displayed
                lblInfo.ForeColor = System.Drawing.Color.Red;
                lblInfo.Text = operation.Message;
            }
        }

        // Saves the details of the Order to the database through DataService
        private bool SaveOrder()
        {
            var svc = new CharityKitchenServiceSoapClient(); // Create new ServiceSoapClient
            ServiceOperation operation; // Create new ServiceOperation

            // Set the values of the static record to values from the screen
            try
            {
                // The date may not be entered correctly by user,
                // therefore put in in try-catch block
                order.OrderDate = DateTime.Parse(txtOrderDate.Text);
                order.CustomerName = txtCustomerName.Text;
                order.CustomerAddress1 = txtC_Address1.Text;
                order.CustomerAddress2 = txtC_Address2.Text;
            }
            catch (Exception)
            {
                // If DateTime could not parse
                // Display message to used that they did not put date correctly
                lblInfo.ForeColor = System.Drawing.Color.Red;
                lblInfo.Text = "Error: Date format is incorrect, please enter Order-Date in this format mm/dd/yyyy";
                return false; // Indicate that the error occured
            }

            operation = svc.Order_Save(order);

            // If data service successfull
            if (operation.Success)
            {
                // Display success message on screen
                lblInfo.ForeColor = System.Drawing.Color.DarkGreen;
                lblInfo.Text = operation.Message;
                return true; // Indicate the Order is saved successfully
            }
            else
            {
                // If data service unsuccessfull, display error message on screen
                lblInfo.ForeColor = System.Drawing.Color.Red;
                lblInfo.Text = operation.Message;
                return false; // Indicate the Order is not saved successfully
            }
        }

        // Saves the details of the MealOrderLine to the database through DataService
        private void SaveMealsOrdered()
        {
            var svc = new CharityKitchenServiceSoapClient(); // Create new ServiceSoapClient
            ServiceOperation operation; // Create new ServiceOperation

            // If this is a new Order, then the static record (which is static Order object) has id of 0,
            // it has already been saved to database but has not been updated in RAM
            // Therefore if OrderID of static order is 0, get updated OrderID from database
            if (order.OrderID == 0)
            {
                // Get all the Order objects from database, in ASC (ascending) order
                operation = svc.Order_GetAll();

                // Set record to the last order (since they all ordered ASC by OrderID, therefore new record is saved under the lates record)
                order = (Order)operation.Data[operation.Data.Count - 1];

                // Update the OrderID in the list of MeaOrderLine in the RAM (update each MealOrderLine)
                foreach (var item in mealsOrdered)
                    item.OrderID = order.OrderID;
            }

            // Run operation to update the all the MealOrderLine at the database
            operation = svc.MealsOrdered_Update(mealsOrdered.ToArray(), order.OrderID);

            // If the operation returns successful
            if (operation.Success)
            {
                // Display the success message on the screen
                lblInfo.ForeColor = System.Drawing.Color.DarkGreen;
                lblInfo.Text = operation.Message;
            }
            else
            {
                // If unsuccessful, display the error message on the screen
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
                // Disable Delete column in the Grid-View
                gvMeals.Columns[3].Visible = false;
                // Disable the btnAddMeal button
                btnAddMeal.Enabled = false;
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