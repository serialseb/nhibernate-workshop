using System;
using System.Collections.Generic;

namespace ShoppingCartWeb.Models
{
    public class ProductItem
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual IDictionary<string,string> TechnicalAttributes { get; set;}
        
    }
}