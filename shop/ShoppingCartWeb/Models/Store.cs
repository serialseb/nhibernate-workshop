using System;
using System.Collections.Generic;

namespace ShoppingCartWeb.Models
{
    public class Store
    {
        public Store()
        {
            TopSellers = new List<ProductItem>();

        }
        public virtual int Id { get; set; }

        public virtual IList<ProductItem> TopSellers { get; set; }

        public virtual string Name { get; set; }
    }
}