using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Web;

namespace CharityKitchenDataService.Models
{
    public class Order
    {
        // Declare the variables that will hold values of this class
        public int OrderID { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress1 { get; set; }
        public string CustomerAddress2 { get; set; }
        public DateTime OrderDate { get; set; }

        // Database Field Names
        private const string FIELD_ID = "OrderID";
        private const string FIELD_CUST_NAME = "CustomerName";
        private const string FIELD_ADDRESS1 = "CustomerAddress1";
        private const string FIELD_ADDRESS2 = "CustomerAddress2";
        private const string FIELD_ORDER_DATE = "OrderDate";

        public Order() { } // Parameterless constructor needed from DataService to work

        // Constrictor sets all the Order values to values from query in database
        public Order(OleDbDataReader reader)
        {
            OrderID = (int)reader[FIELD_ID];
            CustomerName = (string)reader[FIELD_CUST_NAME];
            CustomerAddress1 = (string)reader[FIELD_ADDRESS1];
            CustomerAddress2 = (string)reader[FIELD_ADDRESS2];
            OrderDate = (DateTime)reader[FIELD_ORDER_DATE];
        }
    }
}