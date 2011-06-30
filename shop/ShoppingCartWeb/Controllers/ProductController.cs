using System.Linq;
using System.Web.Mvc;
using NHibernate.Criterion;
using NHibernate.Linq;
using ShoppingCartWeb.Models;

namespace ShoppingCartWeb.Controllers
{
    public class ProductController
        : DefaultController
    {
        public ActionResult ReservationsFor(string productName)
        {

            return Json(Do(session =>
                           from reservation in session.Query<ProductReservation>()
                           let products = (from product in session.Query<Product>()
                                           where product.Name == productName
                                           select product)
                           where products.Contains(reservation.Product)
                           select reservation
                            ));
        }
        public ActionResult Index()
        {
            return Json(Do(session => 
                /* Linq */
                //from product in session.Query<Product>()
                //where product.Name != null
                //select product

                /* HQL */
                //session.CreateQuery("from Product where name != null")
                //       .List<Product>()

                /* Criteria */
                //session.CreateCriteria<Product>()
                //    .Add(Restrictions.IsNotNull("Name"))
                //    .List<Product>()

                /* QueryOver */
                //session.QueryOver<Product>()
                //       .Where(x=>x.Name != null)
                //       .List<Product>()
                (
                    from product in session.QueryOver<Product>()
                    where product.Name != null
                    select product
                )
                .List()
            ));
        }
        public ActionResult StartingWith(string start)
        {
            return Json(Do(session => 
                
                /* Linq */
                //from product in session.Query<Product>()
                //where product.Name.StartsWith(start)
                //select product

                /* HQL */
                //session.CreateQuery("from Product where Name like :input")
                //       .SetString("input", start + "%")
                //       .List()

                /* Criteria */
                //session.CreateCriteria<Product>()
                //       .Add(Restrictions.Like("Name", start, MatchMode.Start))
                //       .List()

                /* QueryOver */
                session.QueryOver<Product>()
                       .WhereRestrictionOn(x=>x.Name)
                       .IsLike(start, MatchMode.Start)
                       .List()

                ));
        }

    }
}