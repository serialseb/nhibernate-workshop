using System.Web.Mvc;
using ShoppingCartWeb.Models;

namespace ShoppingCartWeb.Controllers
{
    public class PartyController : DefaultController
    {
        public ActionResult CreatePerson(string name)
        {
            return Json(Do(session => session.Save(new Person
                                                       {
                                                           FirstName = name,
                                                           Location = "London"
                                                       })));
        }
        public ActionResult CreateCompany(string name)
        {
            return Json(Do(session => session.Save(new Company
            {
                CompanyName = name,
                Location = "London"
            })));
        }
        public ActionResult Search(string location)
        {
            return Json(Do(s => s.QueryOver<Party>()
                .Where(x => x.Location == location)
                .List()));
        }
        public ActionResult SearchCompany(string location)
        {
            return Json(Do(s => s.QueryOver<Company>()
                .Where(x => x.Location == location)
                .List()));
        }
        public ActionResult Index(int id)
        {
            return Json(Do(session => session.Get<Party>(id)));
        }
    }
}