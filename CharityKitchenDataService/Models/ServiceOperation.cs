using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace CharityKitchenDataService.Models
{
    [XmlInclude(typeof(Order))]
    [XmlInclude(typeof(MealOrderLine))]
    [XmlInclude(typeof(Meal))]
    [XmlInclude(typeof(MealIngredientLine))]
    [XmlInclude(typeof(Ingredient))]
    [XmlInclude(typeof(User))]
    [XmlInclude(typeof(IngredientsOrdered))]

    public class ServiceOperation
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Exception { get; set; }
        public List<object> Data { get; set; }

        public ServiceOperation()
        {
            Success = true;
            Message = "Operation successful";
            Exception = "N/A";
            Data = new List<object>();
        }
    }
}