using System;
using System.Collections.Generic;

namespace ShoppingCartWeb.Models
{
    public class OrderLine
    {
        public OrderLine()
        {
        }
        public virtual int Id { get; set; }

        //[System.Web.Script.Serialization.ScriptIgnore]
        //public virtual ShoppingCart Cart { get; set; }

        public virtual int Count { get; set; }

        [System.Web.Script.Serialization.ScriptIgnore]
        public virtual Product Product { get; set; }
    }
}