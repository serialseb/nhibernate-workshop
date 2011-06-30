using System;
using System.Collections.Generic;

namespace ShoppingCartWeb.Models
{
    public class Store
    {
        public Store()
        {
            TopSellers = new List<Product>();

        }
        public virtual int Id { get; set; }

        public virtual IList<Product> TopSellers { get; set; }
        public virtual IEnumerable<Order> Orders { get; set; }
        public virtual string Name { get; set; }
    }
}