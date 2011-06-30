using System;
using System.Linq;
using System.Web.Mvc;
using NHibernate;
using ShoppingCartWeb.Models;

namespace ShoppingCartWeb.Controllers
{
    // http://myblahblah:85834938484/ShoppingCart/
    public class DefaultController : Controller
    {
        protected new ActionResult Json(object cart)
        {
            return Json(cart, JsonRequestBehavior.AllowGet);
        }

        protected static T Do<T>(Func<ISession, T> @do, params Action<T>[] thingsToDo)
        {
            
            var s = MvcApplication.NHibernateSession;
            using (var tx = s.BeginTransaction())
            {
                T result = @do(s);
                foreach(var a in thingsToDo)
                    a(result);
                tx.Commit();
                return result;
            }
        }
    }

    public class ShoppingCartController : DefaultController
    {
        public ActionResult UpdateExternalId(int id, Guid newId)
        {
            return Json(
                Do(
                session => session.Get<ShoppingCart>(id),
                cart => cart.ExternalSyncId = newId),
                JsonRequestBehavior.AllowGet);
        }
        public ActionResult Rename(int id, string name)
        {
            
            var session = MvcApplication.NHibernateSession;
            using (var tx = session.BeginTransaction())
            {
                var shoppingCart = session.Get<ShoppingCart>(id);
                shoppingCart.Title = name;
                tx.Commit();
                return Json(shoppingCart, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Delete(int id)
        {
            Do(session =>
                   {
                       session.Delete(session.Load<ShoppingCart>(id));
                       return true;
                   });
            return Json(true);
        }

        public ActionResult View(int id)
        {
            return Json(Do(session => session.Get<ShoppingCart>(id)));
        }
        public ActionResult ProductNames(int id)
        {
            return Json(Do(s => (from res in s.Get<ShoppingCart>(id).Products
                                 where res.Product != null
                                 select res.Product.Name).ToList()));
        }
        public ActionResult RemoveProduct(int id, string productName)
        {
            return Json(Do(
                s => s.Get<ShoppingCart>(id),
                cart => cart.Products.Remove(cart.Products.First(
                    x=>x.Product != null && x.Product.Name==productName))
                            ));
        }
        public ActionResult ProductCount(int id)
        {
            return Json(Do(s => s.Get<ShoppingCart>(id).Products.Count));
        }
        //public ActionResult EmptyProducts(int id)
        //{
        //    return Json(Do(
        //    session=>session.Get<ShoppingCart>(id)
        //        .Products.Clear()
        //                    ));
        //}
            
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
        public ActionResult AddProduct(int id, string productName)
        {
            var session = MvcApplication.NHibernateSession;
            using (var tx = session.BeginTransaction())
            {
                var cart = session.Get<ShoppingCart>(id);
                
                cart.LastModified = DateTime.Now;
                var product = session.QueryOver<Product>()
                    .Where(x=>x.Name == productName)
                    .SingleOrDefault()
                    ?? new Product { Name = productName };

                var prod = new ProductReservation
                                             {
                                                 //Cart = cart,
                                                 Product = product
                                             };
                cart.Products.Add(prod);
                cart.Products.Add(prod);

                tx.Commit();
                return Json(cart);
            }            
        }

        private delegate object MyMethod(ISession session);
        public ActionResult Reservation(int id)
        {
            // Func<ISession, object>
            // T MyMethod(ISession session)
            // delegate(ISession session) { return new T() }
            
            return Json(Do(
                  session => session.Get<ProductReservation>(id)
                ));
        }

        public ActionResult Regulars()
        {
            return Json(Do(
                s => (from cart in s.QueryOver<ShoppingCart>().List<ShoppingCart>()
                      from prod in cart.Products
                      from regular in prod.RegularBuyers
                      select regular.Name
                     )));
        }
        public ActionResult CreateWithProduct(string productName)
        {
            var s = MvcApplication.NHibernateSession;
            var product = new ProductReservation
            {
                //Cart = cart,
                Product = new Product
                            {
                                Name = productName
                            },
            RegularBuyers =
            {
                new Customer{Name = "John Doe"},
                new Customer{Name="Jeanne D'arc"}
            }
                              };
            using (var tx = s.BeginTransaction())
            {
                var cart = new ShoppingCart
                                        {
                                            StartTime = DateTime.Now,
                                            Products = {product}
                                        };

                s.SaveOrUpdate(cart);
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
                    StartTime = DateTime.Now,
                    LastModified = DateTime.Now - TimeSpan.FromDays(2),
                    Owner = new Customer
                                {
                                    Name = "@serialseb"
                                }
                };
                s.Save(shoppingCart);
                tx.Commit();
                return Json(shoppingCart, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
