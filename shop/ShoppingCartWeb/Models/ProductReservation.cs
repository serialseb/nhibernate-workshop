using System;

namespace ShoppingCartWeb.Models
{
    public class ProductReservation
    {
        public virtual int Id { get; set; }
        [System.Web.Script.Serialization.ScriptIgnore]
        public virtual ShoppingCart Cart { get; set; }

        public virtual int Count { get; set; }

        public virtual Product Product { get; set; }
    }
}