using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Web;

namespace CharityKitchenDataService.Models
{
    public class Ingredient
    {
        // Declare the variables that will hold values of this class
        public int ID { get; set; }
        public string Name { get; set; }
        public decimal Cost { get; set; }
        public string Unit { get; set; }

        // Database Field Names
        private const string FIELD_ID = "IngredientID";
        private const string FIELD_NAME = "IngredientName";
        private const string FIELD_COST = "Cost";
        private const string FIELD_UNIT = "Unit";

        public Ingredient() { } // Parameterless constructor needed from DataService to work

        // Constrictor sets all the Ingredient values to values from query in database
        public Ingredient(OleDbDataReader reader)
        {
            ID = (int)reader[FIELD_ID];
            Name = (string)reader[FIELD_NAME];
            Cost = (decimal)reader[FIELD_COST];
            Unit = (string)reader[FIELD_UNIT];
        }
    }
}