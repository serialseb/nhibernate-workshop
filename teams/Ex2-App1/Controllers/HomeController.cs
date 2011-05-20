using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Eq1.App1.Model;
using NHibernate.Criterion;
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
            return View();
        }
        public ActionResult CreateLeague()
        {
            using (var tx = MvcApplication.NHibernateSession.BeginTransaction())
            {

                var realMadrid = new Team("Real Madrid");
                realMadrid.SignPlayers(GeneratePlayers());

                var barcelona = new Team("Barcelona")
                {
                    Fixtures =
                        {
                            { DateTime.Today, "Kingston" },
                            { DateTime.Today.AddDays(1), "Dublin" }
                        },
                    Manager = new Manager
                                    {
                                        FullName = "John Doe"
                                    }
                };
                barcelona.SignPlayers(GeneratePlayers());
                MvcApplication.NHibernateSession.Save(new League
                {
                    Name = "La ligua",
                    Teams = { realMadrid, barcelona },
                    TopScorers = { realMadrid.Players.First(), barcelona.Players.First() },
                    //AuthorizedPersonel =
                    //    {
                    //        new Manager{FullName="Agassi", Salary = 20000},
                    //        new Player{FullName="Roonie wearing heels"}
                    //    }
                });
                tx.Commit();
            }
            return View("Index");
        }

        private ICollection<Player> GeneratePlayers()
        {
            return Enumerable.Range(0, 13).Select(x => new Player { FullName = "Well Paid " + x }).ToList();
        }
        public ActionResult Everything()
        {
            var s = MvcApplication.NHibernateSession;
            using (var tx = s.BeginTransaction())
            {
                var q = s.Query<Team>().Fetch(x => x.Manager)
                    .FetchMany(x => x.Players).ThenFetch(p => p.Team);

                var result = (
                    from human in s.Query<Human>()
                    where human.FullName.StartsWith("W")
                    select new HumanViewModel
                    {
                        Name = human.FullName
                    }).ToList();
                Debug.WriteLine(result.Count());
                tx.Commit();
            }
            return View("Index");
        }
        public ActionResult Leagues()
        {
            var s = MvcApplication.NHibernateSession;
            using (var tx = s.BeginTransaction())
            {
                //var allAuthorized = from league in s.Query<League>()
                //                    from personel in league.AuthorizedPersonel
                //                    select personel.FullName;
                //Console.WriteLine(allAuthorized.ToList().Count());
                tx.Commit();
            }
            return View("Index");
        }

        public ActionResult HirePlayer(int id)
        {

            using (var tx = MvcApplication.NHibernateSession.BeginTransaction())
            {
                var team = MvcApplication.NHibernateSession.Get<Team>(id);
                team.Hire(new Player
                              {
                                  FullName = "Jean Dupont"
                              });

                tx.Commit();
            }
            return View("Index");
        }

        public ActionResult ListPlayers(int id)
        {
            var s = MvcApplication.NHibernateSession;
            using (var tx = s.BeginTransaction())
            {
                Team team = null;
                Manager manager = null;
                return J(
                    
                    //s.QueryOver<League>()
                    // .JoinAlias(x=>x.Teams, ()=> team)
                    // .JoinAlias(()=>team.Manager, ()=> manager)
                    // .Where(()=> 
                    //     team.PlayerCount > 0 ||
                    //     manager.FullName == "John Doe"
                    //     )
                    // .List<League>()
                    // Criteria old style
                    //s.CreateCriteria<League>()
                    // .CreateAlias("Teams", "team")
                    // .CreateAlias("team.Manager", "manager")
                    // .Add(
                    //    Restrictions.And(
                    //    Restrictions.Gt("team.PlayerCount", 0),
                    //    Restrictions.Eq("manager.FullName", "John doe")
                    //    )
                    //    )
                    // .List<League>()

                    //(
                    //    from league in s.Query<League>()
                    //      .FetchMany(x=>x.Teams).ThenFetch(x=>x.Manager)
                    //    from team in league.Teams
                    //    where team.Manager.FullName == "John Doe" &&
                    //     team.PlayerCount > 0
                    //    select league
                    //  ).ToList()
                    
                    //s.CreateCriteria<Player>()
                    //.Add(
                    //    Restrictions.And(
                    //        Restrictions.Gt("Salary", 20),
                    //        Restrictions.Like("FullName", "%1")
                    //    )
                    //)
                    //.List<Player>()

                    // Linq
                    //(from player in s.Query<Player>()
                    // where player.FullName.Contains("1") &&
                    //    player.Salary > 20
                    // select player).ToList()

                    // QueryOver
                    s.QueryOver<Player>()
                     .Where(
                        //x=>x.FullName == "Rooney" && x.Salary >20
                        Restrictions.Or(
                        Restrictions.On<Player>(player => player.FullName).IsLike("%1"),
                        Restrictions.On<Player>(player => player.FullName).IsLike("%2"))
                     )
                     //.WhereRestrictionOn(x=>x.FullName).IsLike("%1%")
                     //.Where(x=>x.Salary > 20)
                     .List()
                    );


            }
        }

        private JsonResult J(object entity)
        {

            return Json(
                entity,
                JsonRequestBehavior.AllowGet);
        }

        public ActionResult SackPlayers(int id)
        {
            using (var tx = MvcApplication.NHibernateSession.BeginTransaction())
            {
                var team = MvcApplication.NHibernateSession.Get<Team>(id);

                team.Sack(team.Players.Last());
                tx.Commit();
                return View("TeamsWithPlayers", new List<Team> { team });
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

    public class HumanViewModel
    {
        public string Name { get; set; }
    }
}
