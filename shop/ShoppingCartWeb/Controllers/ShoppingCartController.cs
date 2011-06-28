using System;
using System.Web.Mvc;
using ShoppingCartWeb.Models;

namespace ShoppingCartWeb.Controllers
{
    // http://myblahblah:85834938484/ShoppingCart/
    public class ShoppingCartController : Controller
    {
        public ActionResult Index()
        {
            var session = MvcApplication.NHibernateSession;
            using (var tx = session.BeginTransaction())
            {
                var carts = session.QueryOver<ShoppingCart>()
                                   .List<ShoppingCart>();
                tx.Commit();
                return Json(carts, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult CreateWithProduct(string productName)
        {
            var s = MvcApplication.NHibernateSession;
            using (var tx = s.BeginTransaction())
            {
                var cart = new ShoppingCart
                                        {
                                            StartTime = DateTime.Now
                                        };
                var product = new ProductReservation
                                  {
                                      Cart = cart,
                                      ProductName = productName
                                  };
                s.SaveOrUpdate(cart);
                s.SaveOrUpdate(product);
                tx.Commit();
                return Json(cart, JsonRequestBehavior.AllowGet);
            }
        }
        // GET http://[::1]:44995/ShoppingCart/Create
        public ActionResult Create()
        {
            var s = MvcApplication.NHibernateSession;
            using (var tx = s.BeginTransaction())
            {
                var shoppingCart = new ShoppingCart
                                       {
                                           StartTime = DateTime.Now
                                       };
                s.Save(shoppingCart);
                tx.Commit();
                return Json(shoppingCart, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
