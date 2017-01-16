using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Web;

namespace CharityKitchenDataService.Models
{
    public class Meal
    {
        // Declare the variables that will hold values of this class
        public int MealID { get; set; }
        public string MealName { get; set; }
        public string MealDescription { get; set; }

        // Database Field Names
        private const string FIELD_ID = "MealID";
        private const string FIELD_NAME = "MealName";
        private const string FIELD_DESCRIPTION = "Description";

        public Meal() { } // Parameterless constructor needed from DataService to work

        // Constrictor sets all the Meal values to values from query in database
        public Meal(OleDbDataReader reader)
        {
            MealID = (int)reader[FIELD_ID];
            MealName = (string)reader[FIELD_NAME];
            MealDescription = (string)reader[FIELD_DESCRIPTION];
        }
    }
}