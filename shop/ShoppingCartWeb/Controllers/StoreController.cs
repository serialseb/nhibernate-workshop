using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NHibernate.Linq;
using ShoppingCartWeb.Models;

namespace ShoppingCartWeb.Controllers
{
    public class StoreController : DefaultController
    {
        public ActionResult Create(string name)
        {
            return Json(Do(session => session.Save(new Store {Name = name})));
        }
        public ActionResult Products()
        {
            return Json(Do(s => s.Query<ProductItem>().ToList()));
        }
        public ActionResult SetProductDetails(int id, string name, string value)
        {
            return Json(Do(session => session.Get<ProductItem>(id),
                           prod    => prod.TechnicalAttributes[name] = value));
        }
        public ActionResult Promote(int id, int productId)
        {
            return Json(Do(
                session => new
                               {
                                   store = session.Get<Store>(id),
                                   product = session.Get<ProductItem>(productId)
                               },
                val => val.store.TopSellers.Insert(0, val.product)));
        }

    }
}
