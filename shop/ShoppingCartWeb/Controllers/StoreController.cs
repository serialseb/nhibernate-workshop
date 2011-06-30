using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NHibernate.Criterion;
using NHibernate.Impl;
using NHibernate.Linq;
using ShoppingCartWeb.Models;

namespace ShoppingCartWeb.Controllers
{
    public class StoreController : DefaultController
    {
        private QueryOver<Product, Product> _myquery = QueryOver.Of<Product>().Where(x => x.Name != null);

        public ActionResult Index(int id)
        {
            return Json(Do(session =>
            {
                var store = session.Load<Store>(id);
                return store.Name;
            }));
        }

        public ActionResult Create(string name)
        {
            return Json(Do(session => session.Save(new Store {Name = name})));
        }

        
        public ActionResult Products()
        {
            return Json(Do(x =>x.Query<Product>()
                .WithName()
                .StartingWith("computer")
                ));
            
        }

        public ActionResult SetProductDetails(int id, string name, string value)
        {
            return Json(Do(session => session.Get<Product>(id),
                           prod    => prod.TechnicalAttributes[name] = value));
        }

        public ActionResult Promote(int id, int productId)
        {
            return Json(Do(
                session => new
                               {
                                   store = session.Get<Store>(id),
                                   product = session.Load<Product>(productId)
                               },
                val => val.store.TopSellers.Insert(0, val.product)));
        }

        public ActionResult Count()
        {
            return Json(Do(session=>

                //session.Query<Store>().Count()

                session.QueryOver<Store>().RowCount()
            ));
        }

        public ActionResult Tops()
        {
            return Json(Do(session =>

                
                session.CreateCriteria<Store>()
                       .CreateCriteria("TopSellers")
                       .SetProjection(Projections.RowCount())
                       .UniqueResult<int>()

            ));
        }

        public ActionResult Names()
        {
            return Json(Do(session=>

                from store in session.Query<Store>()
                select new { name = store.Name, id = store.Id }

                //session.QueryOver<Store>()
                //  .Select(x=>x.Name, x=>x.Id)
                //  .List<object[]>()
                //  .Select(x=>new{name = x[0], value=x[1]})

                ));

        }

        public ActionResult WithPromotions()
        {
            Product product = null;
            Store store = null;
            return Json(Do(session=>


                /* Linq */
                //from store in session.Query<Store>()
                //where store.TopSellers.Count() > 0
                //select store

                /* HQL */
                //session.CreateQuery(
                // /* find out online */
                //)

                /* QueryOver */
                // session.

                //session.CreateCriteria<Store>()
                //       .CreateCriteria("TopSellers")
                //       .Add(Restrictions.Eq("Name", "banana"))
                //       .List<Store>()

                //from store in session.Query<Store>()
                //from p in store.TopSellers
                //where p.Name == store.Name
                //select store


                session.QueryOver(()=> store)
                       .JoinAlias(x => x.TopSellers, ()=> product)
                       .Where(() => store.Name == product.Name )
                       .List()


                ));

        }
    }
}
