using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Web;

namespace CharityKitchenDataService.Models
{
    public class User
    {
        // Declare the variables that will hold values of this class
        public int ID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int AccessLevel { get; set; }

        // Database Field Names
        private const string FIELD_ID = "ID";
        private const string FIELD_USERNAME = "UserName";
        private const string FIELD_PASSWORD = "Password";
        private const string FIELD_ACCESS_LEVEL = "AccessLevel";

        public User() { } // Parameterless constructor needed from DataService to work

        // Constrictor sets all the User values to values from query in database
        public User(OleDbDataReader reader)
        {
            ID = (int)reader[FIELD_ID];
            UserName = (string)reader[FIELD_USERNAME];
            Password = (string)reader[FIELD_PASSWORD];
            AccessLevel = (int)reader[FIELD_ACCESS_LEVEL];
        }
    }
}