using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Web;

namespace CharityKitchenDataService.Models
{
    public class IngredientsOrdered
    {
        // Declare the variables that will hold values of this class
        public int OrderID { get; set; }
        public string CustomerName { get; set; }
        public DateTime OrderDate { get; set; }
        public int MealID { get; set; }
        public string MealName { get; set; }
        public int MealQuantity { get; set; }
        public int IngredientID { get; set; }
        public string IngredientName { get; set; }
        public double IngredientQuantity { get; set; }
        public decimal Cost { get; set; }
        public string Unit { get; set; }

        // Database Field Names
        private const string FIELD_ORDER_ID = "OrderID";
        private const string FIELD_CUSTOMER_NAME = "CustomerName";
        private const string FIELD_ORDER_DATE = "OrderDate";
        private const string FIELD_MEAL_ID = "MealID";
        private const string FIELD_MEAL_NAME = "MealName";
        private const string FIELD_MEAL_QUANTITY = "MealQuantity";
        private const string FIELD_INGREDIENT_ID = "IngredientID";
        private const string FIELD_INGREDIENT_NAME = "IngredientName";
        private const string FIELD_INGREDIENT_QUANTITY = "IngredientQuantity";
        private const string FIELD_COST = "Cost";
        private const string FIELD_UNIT = "Unit";

        public IngredientsOrdered() { } // Parameterless constructor needed from DataService to work

        // Constrictor sets all the IngredientsOrdered values to values from query in database
        public IngredientsOrdered(OleDbDataReader reader)
        {
            OrderID = (int)reader[FIELD_ORDER_ID];
            CustomerName = (string)reader[FIELD_CUSTOMER_NAME];
            OrderDate = (DateTime)reader[FIELD_ORDER_DATE];
            MealID = (int)reader[FIELD_MEAL_ID];
            MealName = (string)reader[FIELD_MEAL_NAME];
            MealQuantity = (int)reader[FIELD_MEAL_QUANTITY];
            IngredientID = (int)reader[FIELD_INGREDIENT_ID];
            IngredientName = (string)reader[FIELD_INGREDIENT_NAME];
            IngredientQuantity = (double)reader[FIELD_INGREDIENT_QUANTITY];
            Cost = (decimal)reader[FIELD_COST];
            Unit = (string)reader[FIELD_UNIT];
        }
    }
}