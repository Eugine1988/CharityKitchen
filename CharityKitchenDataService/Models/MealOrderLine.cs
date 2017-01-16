using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Web;

namespace CharityKitchenDataService.Models
{
    public class MealOrderLine
    {
        // Declare the variables that will hold values of this class
        public int OrderLineID { get; set; }
        public int OrderID { get; set; }
        public int MealID { get; set; }
        public string MealName { get; set; }
        public int MealQuantity { get; set; }

        // Database Field Names
        private const string FIELD_ORDER_LINE_ID = "OrderLineID";
        private const string FIELD_ORDER_ID = "OrderID";
        private const string FIELD_MEAL_ID = "MealID";
        private const string FIELD_MEAL_NAME = "MealName";
        private const string FIELD_MEAL_QUANTITY = "MealQuantity";

        public MealOrderLine() { } // Parameterless constructor needed from DataService to work

        // Constrictor sets all the MealOrderLine values to values from query in database
        public MealOrderLine(OleDbDataReader reader)
        {
            OrderLineID = (int)reader[FIELD_ORDER_LINE_ID];
            OrderID = (int)reader[FIELD_ORDER_ID];
            MealID = (int)reader[FIELD_MEAL_ID];
            MealName = (string)reader[FIELD_MEAL_NAME];
            MealQuantity = (int)reader[FIELD_MEAL_QUANTITY];
        }
    }
}