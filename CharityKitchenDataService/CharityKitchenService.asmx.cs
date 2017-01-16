using CharityKitchenDataService.Models;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;

namespace CharityKitchenDataService
{
    /// <summary>
    /// Summary description for CharityKitchenService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class CharityKitchenService : System.Web.Services.WebService
    {
        // Create enum to help with the RunOperation() method under Common-Methods region
        private enum obj { Orders, MealsOrdered, Meals, MealIngredientLine, Ingredient, User, IngredientsOrdered }

        #region Fields & Queries

        // Connection string
        const string CONNECTION_STRING = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|CharityKitchen.accdb;Persist Security Info=True";

        // Orders Queries
        const string ORDER_GET_ALL = "SELECT * FROM tblOrders ORDER BY OrderID ASC";
        const string ORDER_GET_BY_ID = "SELECT * FROM tblOrders WHERE OrderID = {0}";
        const string ORDER_INSERT = "INSERT INTO tblOrders (CustomerName,CustomerAddress1,CustomerAddress2,OrderDate) " +
                                    "VALUES (\"{0}\",\"{1}\",\"{2}\",#{3}#)";
        const string ORDER_UPDATE = "UPDATE tblOrders SET CustomerName=\"{0}\",CustomerAddress1=\"{1}\",CustomerAddress2=\"{2}\",OrderDate=#{3}# WHERE OrderID={4}";
        const string ORDER_DELETE = "DELETE FROM tblOrders WHERE OrderID={0}";
        const string ORDER_GET_INGREDIENTS = "SELECT * FROM qryIngredientsInAnOrder WHERE OrderID={0}";

        // MealOrderLine Queries
        const string MEALS_ORDERED_GET_BY_ORDER_ID = "SELECT * FROM qryMealOrders WHERE OrderID = " +
                                             "{0} ORDER BY MealID ASC";
        const string MEALS_ORDERED_INSERT = "INSERT INTO tblOrderLine (OrderID,MealID,MealQuantity) " +
                                    "VALUES ({0},{1},{2})";
        const string MEALS_ORDERED_UPDATE = "UPDATE tblOrderLine SET MealQuantity={0} WHERE OrderLineID={1}";
        const string MEALS_ORDERED_DELETE = "DELETE FROM tblOrderLine WHERE OrderLineID={0}";

        // Meals Queries
        const string MEALS_GET_ALL = "SELECT * FROM tblMeals ORDER BY MealID ASC";
        const string MEALS_GET_BY_ID = "SELECT * FROM tblMeals WHERE MealID = {0}";
        const string MEALS_INSERT = "INSERT INTO tblMeals (MealName,Description) " +
                                    "VALUES (\"{0}\",\"{1}\")";
        const string MEALS_UPDATE = "UPDATE tblMeals SET MealName=\"{0}\",Description=\"{1}\" WHERE MealID={2}";
        const string MEALS_DELETE = "DELETE FROM tblMeals WHERE MealID={0}";

        // MealIngredientLine (Recipe) Queries
        const string RECIPE_GET_BY_MEAL_ID = "SELECT * FROM qryMealIngredientLine WHERE MealID = " +
                                             "{0} ORDER BY IngredientID ASC";
        const string RECIPE_INSERT = "INSERT INTO tblRecipes (MealID,IngredientID,IngredientQuantity) " +
                                    "VALUES ({0},{1},{2})";
        const string RECIPE_UPDATE = "UPDATE tblRecipes SET IngredientQuantity={0} WHERE RecipeID={1}";
        const string RECIPE_DELETE = "DELETE FROM tblRecipes WHERE RecipeID={0}";

        // Ingredient Queries
        const string INGREDIENTS_GET_ALL = "SELECT * FROM tblIngredients ORDER BY IngredientID ASC";
        const string INGREDIENTS_GET_BY_ID = "SELECT * FROM tblIngredients WHERE IngredientID = {0}";
        const string INGREDIENTS_INSERT = "INSERT INTO tblIngredients (IngredientName,Cost,Unit) " +
                                    "VALUES (\"{0}\",{1},\"{2}\")";
        const string INGREDIENTS_UPDATE = "UPDATE tblIngredients SET IngredientName=\"{0}\",Cost={1},Unit=\"{2}\" WHERE IngredientID={3}";
        const string INGREDIENTS_DELETE = "DELETE FROM tblIngredients WHERE IngredientID={0}";

        // User queries
        const string USERS_LOGIN = "SELECT * FROM tblUsers WHERE UserName=\"{0}\" AND Password=\"{1}\"";

        #endregion Fields & Queries

        #region Orders

        [WebMethod] // Returns ServiceOperation with all the Orders in ASC order
        public ServiceOperation Order_GetAll()
        {
            return RunOperation(ORDER_GET_ALL, obj.Orders); // Run method to return the ServiceOperation
        }

        [WebMethod] // Takes OrderID as parameter& returns ServiceOperation containing the Order
        public ServiceOperation Order_GetById(int _id)
        {
            string query = String.Format(ORDER_GET_BY_ID, _id.ToString()); // Create the query
            return RunOperation(query, obj.Orders); // Run method to return the ServiceOperation
        }

        [WebMethod] // Takes Order object as parameter & saves or updates the database
        public ServiceOperation Order_Save(Order _record)
        {
            string query; // Create a variable to store the query
            // If OrderID is 0, that means this is a new record, therefore run INSERT query
            if (_record.OrderID == 0)
            {
                // Create the query
                query = String.Format(ORDER_INSERT, _record.CustomerName, _record.CustomerAddress1, _record.CustomerAddress2, _record.OrderDate.ToString());
                return RunOperation(query, obj.Orders); // Run the operation & return ServiceOperation which contains result of operation
            }
            // If OrderID is not 0, then the record exists in database, therefore run UPDATE query
            else
            {
                // Create the query
                query = String.Format(ORDER_UPDATE, _record.CustomerName, _record.CustomerAddress1, _record.CustomerAddress2, _record.OrderDate.ToString(), _record.OrderID.ToString());
                return RunOperation(query, obj.Orders); // Run the operation & return ServiceOperation which contains result of operation
            }
        }

        [WebMethod] // Takes OrderID as parameter & deletes a record from database based on parameter
        public ServiceOperation Order_Delete(int _orderID)
        {
            string query = string.Format(ORDER_DELETE, _orderID); // Create query
            return RunOperation(query, obj.Orders); // Run the operation & return results
        }

        [WebMethod] // Returns ServiceOperation with all the IngredientsOrdered in ASC order
        public ServiceOperation Order_GetIngredients(int _orderID)
        {
            string query = String.Format(ORDER_GET_INGREDIENTS, _orderID.ToString());
            return RunOperation(query, obj.IngredientsOrdered); // Run method to return the ServiceOperation
        }

        #endregion Orders

        #region MealsOrdered

        [WebMethod] // Takes OrderID as parameter & returns ServiceOperation with list of MealOrderLines
        public ServiceOperation MealsOrdered_ByOrderId(int _orderID)
        {
            string query = string.Format(MEALS_ORDERED_GET_BY_ORDER_ID, _orderID); // Creat the query
            return RunOperation(query, obj.MealsOrdered); // Return operation by running a RunOperation() method
        }

        [WebMethod] // Updates MealOrderLines to the database (If any been deleted in app, methods will delete from database)
        public ServiceOperation MealsOrdered_Update(List<MealOrderLine> _records, int _recordID)
        {
            // Create operation to determine if operation will be successful or not
            ServiceOperation op;

            // Delete any OrderLines that are not in the given list (from Parameter)
            op = MealsOrdered_Delete(_records, _recordID);
            // If any errors occur, return the operation with error message
            if (!op.Success)
                return op;

            // If _records has records, Save or Update the list to database
            if (_records.Count > 0)
                op = MealsOrdered_Save(_records);

            // Return the operation (either with success or error message)
            return op;
        }

        // Goes through the list given & inserts or updates the OrderLines in database
        private ServiceOperation MealsOrdered_Save(List<MealOrderLine> _records)
        {
            // Instantiate operation here, because many operations will need to be performed & kept some track of
            ServiceOperation operation = new ServiceOperation();
            string query; // Create a string for the query

            // Add or Update OrderLines, go through _records
            for (int i = 0; i < _records.Count; i++)
            {
                // If OrderLineID is 0, INSERT new record into table
                if (_records[i].OrderLineID == 0)
                {
                    query = String.Format(MEALS_ORDERED_INSERT, _records[i].OrderID.ToString(), _records[i].MealID.ToString(), _records[i].MealQuantity.ToString());
                    operation = RunOperation(query, obj.MealsOrdered);

                    // If error occurs, the operation with & error will return & indicate error
                    if (!operation.Success)
                        return operation;
                }
                // If MealOrderLine already exists, just modify/edit the MealQuanitity entry
                else
                {
                    query = String.Format(MEALS_ORDERED_UPDATE, _records[i].MealQuantity.ToString(), _records[i].OrderLineID.ToString());
                    operation = RunOperation(query, obj.MealsOrdered);

                    // If error occurs, the operation with & error will return & indicate error
                    if (!operation.Success)
                        return operation;
                }
            }

            // If everything goes successful, the last operation will return to be able to give operation.success message
            return operation;
        }

        // Compares OrderLines from database to the list given, & if match not found, deletes the OL from DB
        private ServiceOperation MealsOrdered_Delete(List<MealOrderLine> _records, int _orderID)
        {
            // Create a query to get all OrderLines with OrderID from _records
            string query = string.Format(MEALS_ORDERED_GET_BY_ORDER_ID, _orderID);
            // Get all the OrderLines from database
            ServiceOperation operation = RunOperation(query, obj.MealsOrdered);
            // Create dbRecords to store the OrderLines from database
            List<MealOrderLine> dbRecords = new List<MealOrderLine>();

            // Create the List of OrderLines that consists of database OrderLines
            for (int i = 0; i < operation.Data.Count; i++)
                dbRecords.Add((MealOrderLine)operation.Data[i]);

            // For each OrderLine in database
            for (int i = 0; i < dbRecords.Count; i++)
            {
                // Create boolean to determine if the OrderLine exists in list given (in parameter, _records)
                bool found = false;
                // Go through each record in given list (go trhough _records)
                for (int j = 0; j < _records.Count; j++)
                {
                    // If a matching record found
                    if (dbRecords[i].OrderLineID == _records[j].OrderLineID)
                    {
                        found = true; // Indicate that match has been found
                        break; // Stop going through loop
                    }
                }
                // If database-record has not been found in _records, delete that record
                if (!found)
                {
                    // Create the query to delete the OrderLine
                    query = string.Format(MEALS_ORDERED_DELETE, dbRecords[i].OrderLineID.ToString());
                    // Run the operation
                    operation = RunOperation(query, obj.MealsOrdered);
                    // If error occurs, return the operation with the error message
                    if (!operation.Success)
                        return operation;
                }
            }
            // If no errors occur, return operation with success message
            return operation;
        }

        #endregion MealOrders

        #region Meals

        [WebMethod] // Returns ServiceOperation with all the Meals in ASC order
        public ServiceOperation Meal_GetAll()
        {
            return RunOperation(MEALS_GET_ALL, obj.Meals); // Run method to return the ServiceOperation
        }

        [WebMethod] // Takes MealID as parameter& returns ServiceOperation containing the Meal
        public ServiceOperation Meal_GetById(int _id)
        {
            string query = String.Format(MEALS_GET_BY_ID, _id.ToString()); // Create the query
            return RunOperation(query, obj.Meals); // Run method to return the ServiceOperation
        }

        [WebMethod] // Takes Meal object as parameter & saves or updates the database
        public ServiceOperation Meal_Save(Meal _record)
        {
            string query; // Create a variable to store the query
            // If MealID is 0, that means this is a new record, therefore run INSERT query
            if (_record.MealID == 0)
            {
                // Create the query
                query = String.Format(MEALS_INSERT, _record.MealName, _record.MealDescription);
                return RunOperation(query, obj.Meals); // Run the operation & return ServiceOperation which contains result of operation
            }
            // If OrderID is not 0, then the record exists in database, therefore run UPDATE query
            else
            {
                // Create the query
                query = String.Format(MEALS_UPDATE, _record.MealName, _record.MealDescription, _record.MealID.ToString());
                return RunOperation(query, obj.Meals); // Run the operation & return ServiceOperation which contains result of operation
            }
        }

        [WebMethod] // Takes MealID as parameter & deletes a record from database based on parameter
        public ServiceOperation Meal_Delete(int _mealID)
        {
            string query = string.Format(MEALS_DELETE, _mealID); // Create query
            return RunOperation(query, obj.Meals); // Run the operation & return results
        }

        #endregion Meals

        #region MealIngredientLine

        [WebMethod] // Takes MealID as parameter & returns ServiceOperation with list of MealIngredientLines
        public ServiceOperation MealIngredientLine_ByMealId(int _mealID)
        {
            string query = string.Format(RECIPE_GET_BY_MEAL_ID, _mealID); // Creat the query
            return RunOperation(query, obj.MealIngredientLine); // Return operation by running a RunOperation() method
        }

        [WebMethod] // Updates MealIngredientLines to the database (If any been deleted in app, methods will delete from database)
        public ServiceOperation MealIngredientLine_Update(List<MealIngredientLine> _records, int _recordID)
        {
            // Create operation to determine if operation will be successful or not
            ServiceOperation op;

            // Delete any MealIngredientLines that are not in the given list (from Parameter)
            op = MealIngredientLine_Delete(_records, _recordID);
            // If any errors occur, return the operation with error message
            if (!op.Success)
                return op;

            // If _records has records, Save or Update the list to database
            if (_records.Count > 0)
                op = MealIngredientLine_Save(_records);

            // Return the operation (either with success or error message)
            return op;
        }

        // Goes through the list given & inserts or updates the MealIngredientLines in database
        private ServiceOperation MealIngredientLine_Save(List<MealIngredientLine> _records)
        {
            // Instantiate operation here, because many operations will need to be performed & kept some track of
            ServiceOperation operation = new ServiceOperation();
            string query; // Create a string for the query

            // Add or Update OrderLines, go through _records
            for (int i = 0; i < _records.Count; i++)
            {
                // If OrderLineID is 0, INSERT new record into table
                if (_records[i].MealIngredientLineID == 0)
                {
                    query = String.Format(RECIPE_INSERT, _records[i].MealID.ToString(), _records[i].IngredientID.ToString(), _records[i].IngredientQuantity.ToString());
                    operation = RunOperation(query, obj.MealIngredientLine);

                    // If error occurs, the operation with & error will return & indicate error
                    if (!operation.Success)
                        return operation;
                }
                // If MealIngredientLine already exists, just modify/edit the IngredientQuanitity entry
                else
                {
                    query = String.Format(RECIPE_UPDATE, _records[i].IngredientQuantity.ToString(), _records[i].MealIngredientLineID.ToString());
                    operation = RunOperation(query, obj.MealIngredientLine);

                    // If error occurs, the operation with & error will return & indicate error
                    if (!operation.Success)
                        return operation;
                }
            }

            // If everything goes successful, the last operation will return to be able to give operation.success message
            return operation;
        }

        // Compares MealIngredientLines from database to the list given, & if match not found, deletes the record from DB
        private ServiceOperation MealIngredientLine_Delete(List<MealIngredientLine> _records, int _mealID)
        {
            // Create a query to get all MealIngredientLines with MealID from _records
            string query = string.Format(RECIPE_GET_BY_MEAL_ID, _mealID);
            // Get all the MealIngredientLines from database
            ServiceOperation operation = RunOperation(query, obj.MealIngredientLine);
            // Create dbRecords to store the MealIngredientLines from database
            List<MealIngredientLine> dbRecords = new List<MealIngredientLine>();

            // Create the List of MealIngredientLines that consists of database MealIngredientLines
            for (int i = 0; i < operation.Data.Count; i++)
                dbRecords.Add((MealIngredientLine)operation.Data[i]);

            // For each MealIngredientLine in database
            for (int i = 0; i < dbRecords.Count; i++)
            {
                // Create boolean to determine if the MealIngredientLine exists in list given (in parameter, _records)
                bool found = false;
                // Go through each record in given list (go trhough _records)
                for (int j = 0; j < _records.Count; j++)
                {
                    // If a matching record found
                    if (dbRecords[i].MealIngredientLineID == _records[j].MealIngredientLineID)
                    {
                        found = true; // Indicate that match has been found
                        break; // Stop going through loop
                    }
                }
                // If database-record has not been found in _records, delete that record
                if (!found)
                {
                    // Create the query to delete the MealIngredientLine
                    query = string.Format(RECIPE_DELETE, dbRecords[i].MealIngredientLineID.ToString());
                    // Run the operation
                    operation = RunOperation(query, obj.MealIngredientLine);
                    // If error occurs, return the operation with the error message
                    if (!operation.Success)
                        return operation;
                }
            }
            // If no errors occur, return operation with success message
            return operation;
        }

        #endregion MealIngredientLine

        #region Ingredients

        [WebMethod] // Returns ServiceOperation with all the Ingredients in ASC order
        public ServiceOperation Ingredient_GetAll()
        {
            return RunOperation(INGREDIENTS_GET_ALL, obj.Ingredient); // Run method to return the ServiceOperation
        }

        [WebMethod] // Takes IngredientID as parameter& returns ServiceOperation containing the Ingredient
        public ServiceOperation Ingredient_GetById(int _id)
        {
            string query = String.Format(INGREDIENTS_GET_BY_ID, _id.ToString()); // Create the query
            return RunOperation(query, obj.Ingredient); // Run method to return the ServiceOperation
        }

        [WebMethod] // Takes Ingredient object as parameter & saves or updates the database
        public ServiceOperation Ingredient_Save(Ingredient _record)
        {
            string query; // Create a variable to store the query
            // If IngredientID is 0, that means this is a new record, therefore run INSERT query
            if (_record.ID == 0)
            {
                // Create the query
                query = String.Format(INGREDIENTS_INSERT, _record.Name, _record.Cost.ToString(), _record.Unit);
                return RunOperation(query, obj.Ingredient); // Run the operation & return ServiceOperation which contains result of operation
            }
            // If IngredientID is not 0, then the record exists in database, therefore run UPDATE query
            else
            {
                // Create the query
                query = String.Format(INGREDIENTS_UPDATE, _record.Name, _record.Cost.ToString(), _record.Unit, _record.ID.ToString());
                return RunOperation(query, obj.Ingredient); // Run the operation & return ServiceOperation which contains result of operation
            }
        }

        [WebMethod] // Takes IngredientID as parameter & deletes a record from database based on parameter
        public ServiceOperation Ingredient_Delete(int _ingredientID)
        {
            string query = string.Format(INGREDIENTS_DELETE, _ingredientID); // Create query
            return RunOperation(query, obj.Ingredient); // Run the operation & return results
        }

        #endregion Ingredients

        #region Users

        [WebMethod] // Takes UserName & Password as argument & returns operation with User object
        public ServiceOperation UserLogin(string username, string password)
        {
            string query = String.Format(USERS_LOGIN, username, password); // Create the query
            return RunOperation(query, obj.User); // Run & return the operation
        }

        #endregion Users

        #region Common-Methods

        // Runs a query given in parameter based on the object given in parameter
        private ServiceOperation RunOperation(string query, obj _obj)
        {
            // Instantiate a new ServiceOperation
            ServiceOperation operation = new ServiceOperation();
            // Create a connection to the database
            OleDbConnection dbConn = new OleDbConnection(CONNECTION_STRING);

            // Surround the follwing code in try-catch in order to catch potential errors
            try
            {
                // Create the command for the database
                OleDbCommand dbCmd = new OleDbCommand(query, dbConn);
                dbConn.Open(); // Open the connection to the database
                OleDbDataReader reader = dbCmd.ExecuteReader(); // Create a reader object

                // Based on the object given in parameter, run the appropriate method
                // that gets the appropriate objects from database & returns it in the operation
                switch (_obj)
                {
                    case obj.Orders:
                        GetOrders(operation, reader);
                        break;
                    case obj.MealsOrdered:
                        GetMealsOrdered(operation, reader);
                        break;
                    case obj.Meals:
                        GetMeals(operation, reader);
                        break;
                    case obj.MealIngredientLine:
                        GetMealIngredientLines(operation, reader);
                        break;
                    case obj.Ingredient:
                        GetIngredients(operation, reader);
                        break;
                    case obj.User:
                        GetUsers(operation, reader);
                        break;
                    case obj.IngredientsOrdered:
                        GetIngredientsOrdered(operation, reader);
                        break;
                    default:
                        break;
                }
            }
            // Catch 3 different types of exceptions
            catch (FormatException ex)
            {
                operation.Success = false; // Indicate that error occured
                operation.Exception = ex.Message; // Record the exception
                operation.Message = "Invalid data supplied: " + ex.Message; // Set intro to the error message
            }
            catch (OleDbException ex)
            {
                operation.Success = false; // Indicate that error occured
                operation.Exception = ex.Message; // Record the exception
                operation.Message = "Database error: " + ex.Message; // Set intro to the error message
            }
            catch (Exception ex)
            {
                operation.Success = false; // Indicate that error occured
                operation.Exception = ex.Message; // Record the exception
                operation.Message = "Unspecified error: " + ex.Message; // Set intro to the error message
            }
            finally // The finally block will execute even if error occured
            {
                // If the connection to the database is open
                if (dbConn.State == System.Data.ConnectionState.Open)
                {
                    dbConn.Close(); // Close the connection to the database
                }
            }
            // Return the operation that contains appropriate objects & success/error message
            return operation;
        }

        #region methods used by RunOperation()
        // The methods within this region are called by RunOperation() method,
        // in order to get the appropriate objects from the database

        private void GetOrders(ServiceOperation _op, OleDbDataReader _reader)
        {
            // Run a loop to get all the appropriate Order objects
            while (_reader.Read())
                _op.Data.Add(new Order(_reader));
        }

        private void GetMealsOrdered(ServiceOperation _op, OleDbDataReader _reader)
        {
            // Run a loop to get all the appropriate MealOrderLine objects
            while (_reader.Read())
                _op.Data.Add(new MealOrderLine(_reader));
        }

        private void GetMeals(ServiceOperation _op, OleDbDataReader _reader)
        {
            // Run a loop to get all the appropriate Meal objects
            while (_reader.Read())
                _op.Data.Add(new Meal(_reader));
        }

        private void GetMealIngredientLines(ServiceOperation _op, OleDbDataReader _reader)
        {
            // Run a loop to get all the appropriate MealIngredientLine objects
            while (_reader.Read())
                _op.Data.Add(new MealIngredientLine(_reader));
        }

        private void GetIngredients(ServiceOperation _op, OleDbDataReader _reader)
        {
            // Run a loop to get all the appropriate Ingredient objects
            while (_reader.Read())
                _op.Data.Add(new Ingredient(_reader));
        }

        private void GetUsers(ServiceOperation _op, OleDbDataReader _reader)
        {
            // Run a loop to get all the appropriate Ingredient objects
            while (_reader.Read())
                _op.Data.Add(new User(_reader));
        }

        private void GetIngredientsOrdered(ServiceOperation _op, OleDbDataReader _reader)
        {
            // Run a loop to get all the appropriate Ingredient objects
            while (_reader.Read())
                _op.Data.Add(new IngredientsOrdered(_reader));
        }

        #endregion methods used by GetObjects()

        #endregion Common-Methods
    }
}
