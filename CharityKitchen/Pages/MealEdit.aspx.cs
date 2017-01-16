using CharityKitchen.CharityKitchenService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CharityKitchen.Pages
{
    public partial class MealEdit : System.Web.UI.Page
    {
        #region vars

        // Create variables for storing the necessary values
        static Meal meal;
        static List<Ingredient> ingredients = new List<Ingredient>();
        static List<MealIngredientLine> recipe = new List<MealIngredientLine>();

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
            DisplayIngredients(svc); // Populate the combo-box wil all ingredients
            if (DisplayMeal(svc)) // If false, it means that this is new record, no need to get recipe from database
                DisplayRecipe(svc); // Display the MealIngredientLines
        }

        #endregion init

        #region events

        // When button to Save everything is clicked
        protected void btnSave_Click(object sender, EventArgs e)
        {
            // NB: Only save the MealIngredientLine if Meals is saved successfully
            if (SaveMeal()) // Run method to save the Meal details
                SaveRecipe(); // Run method to save the Recipe
        }

        // When button to go back to all Meals page is clicked
        protected void Back_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Pages/Meals"); // Go to the indicated page
        }

        // When button clicked to add an Ingredient to GridView displaying Ingredients for this meal
        protected void btnAddIngredient_Click(object sender, EventArgs e)
        {
            // Create variable to store Ingredient-Quantity value
            double ingredQuantity;

            // Try Parse Ingredient-Quatity to double
            try
            {
                ingredQuantity = double.Parse(txtIngredintQuantity.Text);
            }
            // If could not Parse, display error message & return from this method
            catch (Exception)
            {
                lblInfo.ForeColor = System.Drawing.Color.Red;
                lblInfo.Text = "Could not Parse Ingredient-Quantity to a double. Please enter  anumber (e.g. 0.1)";
                return; 
            }

            // Create valiable to contain value of selected index in cboIngredients
            int index = cboIngredients.SelectedIndex;

            // For determining if the ingredient has already been ordered before (wether to edit MealIngredientLine-quantity or add new MealOrderLine)
            bool found = false;
            // Go thorough List<MealIngredientLine> to see if selected ingredient matches any meals already ordered
            foreach (var item in recipe)
            {
                if (item.IngredientName == cboIngredients.SelectedValue)
                {
                    // Just add +1 to meal quantity if it matches the meals selected to be added
                    item.IngredientQuantity = ingredQuantity;
                    found = true;
                    break;
                }
            }

            // If ingredient not found in mealsOrdered
            if (!found)
            {
                // Create new MealIngredientLine & set it's values
                MealIngredientLine newMealIngredientLine = new MealIngredientLine();
                newMealIngredientLine.MealID = meal.MealID;
                newMealIngredientLine.IngredientID = ingredients[index].ID;
                newMealIngredientLine.IngredientName = ingredients[index].Name;
                newMealIngredientLine.IngredientQuantity = ingredQuantity;
                newMealIngredientLine.IngredientCost = ingredients[index].Cost;
                newMealIngredientLine.IngredientUnit = ingredients[index].Unit;

                // Add the new MealIngredientLine to Recipe list
                recipe.Add(newMealIngredientLine);
            }

            // Display the changes in Grid-View
            gvRecipe.DataSource = recipe;
            gvRecipe.DataBind();

            // Display the AccessLevel in the lblInfo
            lblInfo.ForeColor = System.Drawing.Color.LightGray;
            ImplementAccessLevel();
        }

        // When Delete button pressed to delete an Ingredient from gvIngredients
        protected void gvIngredients_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            // Get the index of the row selected (will equal the index of the MealIngredientLine)
            int index = e.RowIndex;

            // Remove that Ingredient from the recipe (list of MealIngredientLine)
            recipe.RemoveAt(index);

            // Display the changes in Grid-View
            gvRecipe.DataSource = recipe;
            gvRecipe.DataBind();
        }

        #endregion events

        #region methods

        // Gets list of Ingredients from service & displays on screen
        private void DisplayIngredients(CharityKitchenServiceSoapClient _svc)
        {
            // operation will get data from database
            var operation = _svc.Ingredient_GetAll();

            // Create array for storing ingredient names (To be able to put into combo-box)
            String[] ingredientNames = new String[operation.Data.Count];

            // If data service operation is successful
            if (operation.Success)
            {
                // Add the data from database to list
                for (int i = 0; i < operation.Data.Count; i++)
                {
                    ingredients.Add((Ingredient)operation.Data[i]); // Update List of ingredients
                    ingredientNames[i] = ingredients[i].Name; // Update array of ingredientNames
                }

                // Add the list of ingredients to the combo-box & get it to display in cbo
                cboIngredients.DataSource = ingredientNames;
                cboIngredients.DataBind();
            }
            else
            {
                // If data service unsuccessfull, then error message will be displayed
                lblInfo.ForeColor = System.Drawing.Color.Red;
                lblInfo.Text = operation.Message;
            }
        }

        // Gets Meal from data-service & displays on screen
        private bool DisplayMeal(CharityKitchenServiceSoapClient _svc)
        {
            // Create id integer to store the id of the record
            int id = 0;
            // Try to get the id of the record from session
            try { id = (int)Session["meal_edit"]; }
            catch { }

            if (id != 0)
            {
                // Set service operation
                var operation = _svc.Meal_GetById(id);

                // If data service operation is successful
                if (operation.Success)
                {
                    // Display the values from service on screen
                    meal = (Meal)operation.Data[0];
                    lblID.Text = meal.MealID.ToString();
                    txtMealName.Text = meal.MealName;
                    txtDescription.Text = meal.MealDescription;
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
                meal = new Meal();
                recipe.Clear(); // Clear the static list of MealIngredientLine
                lblID.Text = meal.MealID.ToString();
                return false;
            }
        }

        // Updates & Displays the recipe DisplayIngredients
        private void DisplayRecipe(CharityKitchenServiceSoapClient _svc)
        {
            // In case recipe contains data, clear it, so data won't be duplicated
            recipe.Clear();

            // operation will get the data from database
            var operation = _svc.MealIngredientLine_ByMealId(meal.MealID);

            // If data service operation is successful
            if (operation.Success)
            {
                // Add the data from database to list
                for (int i = 0; i < operation.Data.Count; i++)
                    recipe.Add((MealIngredientLine)operation.Data[i]);

                // Add recipe to GridView & display on screen
                gvRecipe.DataSource = recipe;
                gvRecipe.DataBind();
            }
            else
            {
                // If data service unsuccessfull, then error message will be displayed
                lblInfo.ForeColor = System.Drawing.Color.Red;
                lblInfo.Text = operation.Message;
            }
        }

        // Saves the details of the Meal to the database through DataService
        private bool SaveMeal()
        {
            var svc = new CharityKitchenServiceSoapClient(); // Create new ServiceSoapClient
            ServiceOperation operation; // Create new ServiceOperation

            // Set the values of the static record to values from the screen
            meal.MealName = txtMealName.Text;
            meal.MealDescription = txtDescription.Text;

            // Save meal to database
            operation = svc.Meal_Save(meal);

            // If data service successfull
            if (operation.Success)
            {
                // Display success message on screen
                lblInfo.ForeColor = System.Drawing.Color.DarkGreen;
                lblInfo.Text = operation.Message;
                return true; // Indicate the Meal is saved successfully
            }
            else
            {
                // If data service unsuccessfull, display error message on screen
                lblInfo.ForeColor = System.Drawing.Color.Red;
                lblInfo.Text = operation.Message;
                return false; // Indicate the Meal is not saved successfully
            }
        }

        // Saves the details of the Recipe to the database through DataService
        private void SaveRecipe()
        {
            var svc = new CharityKitchenServiceSoapClient(); // Create new ServiceSoapClient
            ServiceOperation operation; // Create new ServiceOperation

            // If this is a new Meal, then the static record (which is static Meal object) has id of 0,
            // it has already been saved to database but has not been updated in RAM
            // Therefore if MealID of static order is 0, get updated MealID from database
            if (meal.MealID == 0)
            {
                // Get all the Meal objects from database, in ASC (ascending) order
                operation = svc.Meal_GetAll();

                // Set record to the last meal (since they all ordered ASC by MealID, therefore new record is saved under the lates record)
                meal = (Meal)operation.Data[operation.Data.Count - 1];

                // Update the MealID in Recipe in the RAM (update each MealRecipeLine)
                foreach (var item in recipe)
                    item.MealID = meal.MealID;
            }

            // Run operation to update the all the MealRecipeLine at the database
            operation = svc.MealIngredientLine_Update(recipe.ToArray(), meal.MealID);

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
                gvRecipe.Columns[5].Visible = false;
                // Disable the btnAddIngredient button
                btnAddIngredient.Enabled = false;
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