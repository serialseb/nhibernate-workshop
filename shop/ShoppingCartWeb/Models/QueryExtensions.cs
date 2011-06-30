using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ShoppingCartWeb.Models
{
    public static class QueryExtensions
    {
        public static IQueryable<Product> WithName(this IQueryable<Product> product)
        {
            return product.Where(x => x.Name != null);
        }
        public static IQueryable<Product> StartingWith(this IQueryable<Product> product, string start)
        {
            return product.Where(x => x.Name.StartsWith(start));
        }
    }

    internal class MyClass
    {
        private static void Main(string[] args)
        {
            var list = new List<Product> {new Product {Name = "computer"}, new Product {Name = "banana"}};
            var result = list.AsQueryable().StartingWith("computer").Count();
            Debug.Assert(result == 1);

        }
    }
}