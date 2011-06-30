using System;
using System.Collections.Generic;

namespace ShoppingCartWeb.Models
{
    public class ProductReservation
    {
        public ProductReservation()
        {
            RegularBuyers = new List<Customer>();
        }
        public virtual int Id { get; set; }

        //[System.Web.Script.Serialization.ScriptIgnore]
        //public virtual ShoppingCart Cart { get; set; }

        public virtual int Count { get; set; }

        public virtual ICollection<Customer> RegularBuyers { get; set; }
        
        [System.Web.Script.Serialization.ScriptIgnore]
        public virtual ProductItem Product { get; set; }
    }
}