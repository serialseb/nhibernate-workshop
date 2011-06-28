namespace ShoppingCartWeb.Models
{
    public class ProductReservation
    {
        public virtual int Id { get; set; }
        public virtual ShoppingCart Cart { get; set; }
        public virtual string ProductName { get; set; }
    }
}