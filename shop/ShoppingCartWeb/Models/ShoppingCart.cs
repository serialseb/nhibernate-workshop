using System;
using System.Collections.Generic;

namespace ShoppingCartWeb.Models
{
    public class ShoppingCart
    {
        public ShoppingCart()
        {
            Products = new List<OrderLine>();
        }
        public virtual int Id { get; set; }

        public virtual DateTime? StartTime { get; set; }
        [System.Web.Script.Serialization.ScriptIgnore]

        public  virtual ICollection<OrderLine> Products { get; set; }

        public virtual DateTime? LastModified { get; set; }

        public virtual string Title { get; set; }
        public virtual Guid ExternalSyncId { get; set; }
        [System.Web.Script.Serialization.ScriptIgnore]

        public virtual Customer Owner { get; set; }
    }
}