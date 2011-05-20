using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Eq1.App1.Model;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
using NHibernate.Transform;

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

        private DetachedCriteria myQuery =
            DetachedCriteria.For<Team>()
                .CreateCriteria("Manager")
                .Add(Expression.Eq("FullName", "John Doe"));

        private QueryOver<Team> myQuery2 =
            QueryOver.Of<Team>()
                .JoinQueryOver<Manager>(x => x.Manager)
                .Where(m => m.FullName == "John Doe");
                

        public ActionResult ListPlayers(int id)
        {
            var s = MvcApplication.NHibernateSession;
            using (var tx = s.BeginTransaction())
            {
                var avgSalary = DetachedCriteria.For<Player>()
                    .SetProjection(Projections.Avg("Salary"));
                var actualQuery = DetachedCriteria.For<Player>()
                    .Add(Subqueries.PropertyGt("Salary", avgSalary));
                var tq = s.QueryOver<League>()
                    .Fetch(x => x.Teams).Eager
                    .WhereRestrictionOn(l => l.Name).IsLike("%Something%");
                
                var t = from league in s.Query<League>()
                            //.FetchMany(x => x.Teams).ThenFetch(x => x.Manager)
                        from team in league.Teams
                        where league.Name.Contains("Something")// && team.Name.Contains("Hello")
                        && team.Manager != null
                        select team;
                var players = s.Query<Player>();

                var player = from p in players
                             where p.Salary > (players.Average(x => x.Salary))
                             select p;
                //PlayerDTO dto = null;
                //Team team = null;
                //Manager manager = null;
                var averageSalary = QueryOver.Of<Player>()
                    .SelectList(x => x.SelectAvg(p => p.Salary));

                

                return J(""
                    //s.QueryOver<Team>()
                    //    .Where(Restrictions.Gt(Projections.SubQuery())
                    //myQuery2.GetExecutableQueryOver(s).List<Team>()
                    //t.ToList<Team>()

                    // using queryover for subquery
                    //s.QueryOver<Player>()
                    // .WithSubquery.WhereSome(x=>x.Salary > averageSalary.As<int>())
                    // .List()
                    
                     // attaching detached criteria
                    //actualQuery.GetExecutableCriteria(s).List<Player>()

                    // build DTO from criteria API
                    //s.CreateCriteria<Player>()
                    // .SetProjection(
                    //    //Projections.Property("FullName")
                    // Projections.ProjectionList()
                    //    .Add(Projections.Property("FullName"))
                    //    .Add(Projections.Property("Salary"))
                    //    )
                    // .List<object[]>()
                    // .Select(x=>new PlayerDTO
                    //                {
                    //                    FullName = (string)x[0],
                    //                    Salary = (int)x[1]
                    //                })

                    // build DTO using Result Transformer in Criteria API
                    
                    //s.CreateCriteria<Player>()
                    // .SetProjection(
                    // Projections.ProjectionList()
                    //    .Add(Projections.Property("FullName"), "FullName")
                    //    .Add(Projections.Property("Salary"), "Salary")
                    //    )
                    // .SetResultTransformer(Transformers.AliasToBean(typeof(PlayerDTO)))
                    // .List<PlayerDTO>()

                    // in Linq

                    //s.Query<Player>().Where(p=>p.FullName == null)
                    //.Select(x => new PlayerDTO
                    //                                {
                    //                                    FullName = x.FullName,
                    //                                    Salary = x.Salary
                    //                                })
                    
                    // in QueryOver

                    //s.QueryOver<Player>()
                    //    .SelectList(x=>
                    //        x.Select(p=>p.FullName).WithAlias(()=>dto.FullName)
                    //         .Select(p=>p.Salary).WithAlias(()=>dto.Salary))
                    //    .TransformUsing(Transformers.AliasToBean<PlayerDTO>())
                    //    .List<PlayerDTO>()

                    // Following associations in QueryOver

                    //s.QueryOver<League>()
                    // .JoinAlias(x=>x.Teams, ()=> team)
                    // .JoinAlias(()=>team.Manager, ()=> manager)
                    // .Where(()=> 
                    //     team.PlayerCount > 0 &&
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


                    //s.QueryOver<League>()
                    //    .JoinQueryOver<Team>(x => x.Teams)
                    //    .Where(t => t.PlayerCount > 0)
                    //        .JoinQueryOver<Manager>(x => x.Manager)
                    //        .Where(m => m.FullName == "JohnDoe")
                        
                    // .List<League>()

                    
                    //s.CreateCriteria<League>()
                    //.SetFetchMode("Teams", FetchMode.Select)

                    //(
                    //    from league in s.Query<League>()
                    //      .FetchMany(x => x.Teams).ThenFetch(x => x.Manager)
                          
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
                    //s.QueryOver<Player>()
                    // .Where(
                    //    //x=>x.FullName == "Rooney" && x.Salary >20
                    //    Restrictions.Or(
                    //    Restrictions.On<Player>(player => player.FullName).IsLike("%1"),
                    //    Restrictions.On<Player>(player => player.FullName).IsLike("%2"))
                    // )
                    // //.WhereRestrictionOn(x=>x.FullName).IsLike("%1%")
                    // //.Where(x=>x.Salary > 20)
                    // .List()
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

    public class PlayerDTO
    {
        public int Salary { get; set; }

        public string FullName { get; set; }
    }

    public class HumanViewModel
    {
        public string Name { get; set; }
    }
}
