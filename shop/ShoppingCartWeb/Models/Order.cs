using System;
using System.Collections.Generic;
using NHibernate.Criterion.Lambda;

namespace ShoppingCartWeb.Models
{
    public class Order
    {
        public Order()
        {
            Lines = new List<OrderLine>();
        }
        public virtual int Id { get; set; }
        public virtual ICollection<OrderLine> Lines { get; set; }
        public virtual Customer Customer { get; set; }

        public virtual Store Store { get; set; }
    }
}