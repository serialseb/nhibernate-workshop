using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Eq1.App1.Model;
using NHibernate;
using NHibernate.Linq;

namespace Ex_App1.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        public ActionResult Blah()
        {
            using (var tx = MvcApplication.NHibernateSession.BeginTransaction())
            {
                var first = MvcApplication.NHibernateSession.Load<Team>(2);
                var two = MvcApplication.NHibernateSession.Load<Team>(2);
                Debug.Assert(ReferenceEquals(first, two));
                tx.Commit();
            }
            return View("Index");
        }
        public ActionResult Index()
        {
            using (var tx = MvcApplication.NHibernateSession.BeginTransaction())
            {

                var realMadrid = new Team("Real Madrid")
                                     {
                                         Players = GeneratePlayers()
                                     };
                var barcelona = new Team("Barcelona")
                                    {
                                        Players = GeneratePlayers(),
                                        Manager = new Manager
                                                      {
                                                          FullName = "John Doe"
                                                      }
                                    };

                MvcApplication.NHibernateSession.Save(new League
                {
                    Name = "La ligua",
                    Teams = { realMadrid, barcelona }
                });
                tx.Commit();
            }
            return View();
        }

        private ICollection<Player> GeneratePlayers()
        {
            return Enumerable.Range(0, 13).Select(x => new Player { FullName = "Well Paid " + x }).ToList();
        }
        public ActionResult Everything()
        {
            using(var tx = MvcApplication.NHibernateSession.BeginTransaction())
            {
                foreach (var league in MvcApplication.NHibernateSession.QueryOver<League>().List<League>())
                    Console.WriteLine(league.Name);
                        //foreach (var player in team.Players)
                        //    Debug.WriteLine(player.FullName);
            }
            return View("Index");
        }
        public ActionResult FirePlayer(int id)
        {
            using(var tx = MvcApplication.NHibernateSession.BeginTransaction())
            {
                var team = MvcApplication.NHibernateSession.Get<Team>(id);
                team.Players.Clear();
                tx.Commit();
                return View("TeamsWithPlayers", new List<Team> {team});
            }
        }
        public ActionResult TeamsWithPlayers()
        {
            using (var tx = MvcApplication.NHibernateSession.BeginTransaction())
            {
                var teams = MvcApplication.NHibernateSession.Query<Team>().ToList();

                tx.Commit();
                return View(teams);
            }
        }
        public ActionResult About()
        {
            return View();
        }
    }
}
