using CharityKitchen.CharityKitchenService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CharityKitchen.Pages
{
    public partial class Order_GetIngredients : System.Web.UI.Page
    {
        // When the page loads
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            // Create new ServiceSoapClient
            var svc = new CharityKitchenServiceSoapClient();
            // Get the list of all IngredientsOrdered from database
            var ingredients_Unprocessed = GetIngredientsOrdered(svc);
            // Process all ingredients ordered (quantity values) & return list ingredients
            var ingredients_Processed = GetIngredients(ingredients_Unprocessed);
            // Add _records to GridView & display on screen
            DisplayRecord(ingredients_Processed);
        }

        // When button to go back to Order page is clicked
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Pages/OrderEdit"); // Go to the indicated page
        }

        // Puts all the order & ingredient information on screen
        private void DisplayRecord(List<IngredientsOrdered> _record)
        {
            // Display the Order details on screen
            lblOrderID.Text = _record[0].OrderID.ToString();
            lblCustomerName.Text = _record[0].CustomerName;
            lblOrderDate.Text = _record[0].OrderDate.ToString();

            // Following bloack of code creates string with all meals ordered
            // Get first meal name
            string mealsOrdered = _record[0].MealName + "(" + _record[0].MealQuantity + ")";
            int count = _record[0].MealID; // Create count to keep track of meal that already recorded
            // Go through the entire list of preocessed ingredients
            for (int i = 1; i < _record.Count; i++)
            {
                // If curernt mealID doesn't match the previusly recorded mealID
                if (_record[i].MealID != count)
                {
                    // The records in _record are ordered ASC by mealID,
                    // therefore only need to know if current mealID equals prev count or not
                    count = _record[i].MealID;
                    // Add new meal name to string
                    mealsOrdered += ", " + _record[i].MealName + "(" + _record[i].MealQuantity + ")";
                }
            }
            // Display meals ordered
            lblMealsOrdered.Text = mealsOrdered;
            // Call a method to calculate the total cost & display result on screen
            lblTotalCost.Text += CalculateTotalCost(_record).ToString();

            // Add _records to GridView & display on screen
            gvIngredients.DataSource = _record;
            gvIngredients.DataBind();
        }

        // Processes the list of Ingredients & returns new list with easily readable data
        private List<IngredientsOrdered> GetIngredients(List<IngredientsOrdered> _ingredientsUnpr)
        {
            // Create new list of ingredients
            List<IngredientsOrdered> ingredients = new List<IngredientsOrdered>();

            // Add the first record to the list of processed ingredients
            ingredients.Add(_ingredientsUnpr[0]);
            // Set the ingredient quantity based on meal quantity
            ingredients[0].IngredientQuantity *= ingredients[0].MealQuantity;

            // Go through entire list of uprocessed ingredients
            for (int i = 1; i < _ingredientsUnpr.Count; i++)
            {
                // Create boolean to determine if matching ingredient has been found
                bool found = false;
                // Go through sorted ingredients list
                for (int j = 0; j < ingredients.Count; j++)
                {
                    // If the ingredientID is the same in both lists
                    if (_ingredientsUnpr[i].IngredientID == ingredients[j].IngredientID)
                    {
                        found = true; // Indicate that matching inredient has been found
                        // Add the appropriate quantity to the Quantity variable in processed ingredients list
                        ingredients[j].IngredientQuantity += _ingredientsUnpr[i].MealQuantity
                                                             * _ingredientsUnpr[i].IngredientQuantity;
                    }
                }
                // If matching ingredient hasn't been fouid, add it from unprocessed to processed list
                if (!found)
                {
                    // Add new ingredient to processed list
                    ingredients.Add(_ingredientsUnpr[i]);
                    // Set the ingredient quantity based on meal quantity
                    ingredients[ingredients.Count - 1].IngredientQuantity *= ingredients[ingredients.Count - 1].MealQuantity;
                }
            }
            // Return the processed ingredients list
            return ingredients;
        }

        // Get all the ingredients ordered in current order (still needs to be processed before displaying on screen)
        private List<IngredientsOrdered> GetIngredientsOrdered(CharityKitchenServiceSoapClient _svc)
        {
            // Create the list of ingredientsUnpr
            List<IngredientsOrdered> ingredientsUnpr = new List<IngredientsOrdered>();
            // Create id integer to store the id of the order
            int orderID = 0;
            // Try to get the id of the order from session
            try { orderID = (int)Session["order_edit"]; }
            catch { }

            // Set service operation
            var operation = _svc.Order_GetIngredients(orderID);

            // If data service operation is successful
            if (operation.Success)
            {
                // Insert data from operation to list of ingredientsUnpr
                for (int i = 0; i < operation.Data.Count; i++)
                    ingredientsUnpr.Add((IngredientsOrdered)operation.Data[i]);
            }
            else
            {
                // If data service unsuccessfull, then error message will be displayed
                lblInfo.ForeColor = System.Drawing.Color.Red;
                lblInfo.Text = operation.Message;
            }

            // Return the list of ingredientsUnpr
            return ingredientsUnpr;
        }

        // Calculates the total cost for this order & returns as double
        private double CalculateTotalCost(List<IngredientsOrdered> _records)
        {
            // Create variable to store the answer
            double answer = 0.0;

            // Add to answer (cost * quantity) of each ingredient
            foreach (var item in _records)
                answer += ( Convert.ToDouble(item.Cost) * item.IngredientQuantity );

            // Return the answer
            return answer;
        }
    }
}