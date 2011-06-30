using System.Linq;
using System.Web.Mvc;
using NHibernate.Criterion;
using NHibernate.Linq;
using ShoppingCartWeb.Models;
using Order = ShoppingCartWeb.Models.Order;

namespace ShoppingCartWeb.Controllers
{
    public class QueryOverController : DefaultController
    {
        public ActionResult OrdersFromCustomersNamed(string name)
        {
            return Json(s => s.QueryOver<Order>()
                              .JoinQueryOver(x=>x.Customer)
                              .Where(x=>x.Name == name)
                              .List());
        }
        public ActionResult OrdersPerShop()
        {
            Store storeAlias= null;
            return Json(s => s.QueryOver(()=> storeAlias)
                              .SelectList(store => store
                                  .Select(_=>_.Name)
                                  .SelectSubQuery(
                                        QueryOver.Of<Order>()
                                                 .Where(x=>x.Store.Id == storeAlias.Id)
                                                 .ToRowCountQuery()))
                              .List<object[]>()
                              .Select(x=>new { store = x[0], orders = x[1]}));
        }
        public ActionResult ProductsWithOrders()
        {
            return null;
        }
    }
    public class LinqOverController : DefaultController
    {
        public ActionResult OrdersFromCustomersNamed(string name)
        {
            return Json(s => from order in s.Query<Order>()
                             where order.Customer.Name == name
                             select order);
        }
        public ActionResult OrdersPerShop()
        {
            return Json(s => from store in  s.Query<Store>()
                             let orderCount = store.Orders.Count()
                             select new { store = store.Name, orders = orderCount});
        }

        public ActionResult ProductsWithOrders()
        {
            return null;
        }
    }
}