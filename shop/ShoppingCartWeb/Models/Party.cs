namespace ShoppingCartWeb.Models
{
	public abstract class Party
	{
		public virtual int Id { get; set; }
        public virtual string Location { get; set; }
	}

	public class Person : Party
	{
		public virtual string FirstName { get; set; }
	}

	public class Company : Party
	{
		public virtual string CompanyName { get; set; }
	}
}