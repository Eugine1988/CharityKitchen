using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Web;

namespace CharityKitchenDataService.Models
{
    public class MealIngredientLine
    {
        // Declare the variables that will hold values of this class
        public int MealIngredientLineID { get; set; }
        public int MealID { get; set; }
        public int IngredientID { get; set; }
        public string IngredientName { get; set; }
        public double IngredientQuantity { get; set; }
        public decimal IngredientCost { get; set; }
        public string IngredientUnit { get; set; }

        // Database Field Names
        private const string FIELD_MEAL_INGREDIENT_LINE_ID = "RecipeID";
        private const string FIELD_MEAL_ID = "MealID";
        private const string FIELD_INGREDIENT_ID = "IngredientID";
        private const string FIELD_INGREDIENT_NAME = "IngredientName";
        private const string FIELD_INGREDIENT_QUANTITY = "IngredientQuantity";
        private const string FIELD_INGREDIENT_COST = "Cost";
        private const string FIELD_INGREDIENT_UNIT = "Unit";

        public MealIngredientLine() { } // Parameterless constructor needed from DataService to work

        // Constrictor sets all the MealIngredientLine values to values from query in database
        public MealIngredientLine(OleDbDataReader reader)
        {
            MealIngredientLineID = (int)reader[FIELD_MEAL_INGREDIENT_LINE_ID];
            MealID = (int)reader[FIELD_MEAL_ID];
            IngredientID = (int)reader[FIELD_INGREDIENT_ID];
            IngredientName = (string)reader[FIELD_INGREDIENT_NAME];
            IngredientQuantity = (double)reader[FIELD_INGREDIENT_QUANTITY];
            IngredientCost = (decimal)reader[FIELD_INGREDIENT_COST];
            IngredientUnit = (string)reader[FIELD_INGREDIENT_UNIT];
        }
    }
}