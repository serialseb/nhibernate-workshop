using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using HibernatingRhinos.Profiler.Appender.NHibernate;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace ShoppingCartWeb
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        private static ISessionFactory _sessionFactory;

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }


        private const string NHIBERNATE_SESSION
            = "NHIBERNATE_SESSION";
        public static ISession NHibernateSession
        {
            get
            {
                return
                    (ISession)
                    HttpContext.Current.Items
                        [NHIBERNATE_SESSION];
            }
            set
            {
                HttpContext.Current.Items
                    [NHIBERNATE_SESSION] = value;   
            }
        }




        protected void Application_BeginRequest()
        {
            NHibernateSession = _sessionFactory.OpenSession();
            NHibernateSession.SetBatchSize(20);
        }
        protected void Application_EndRequest()
        {
            NHibernateSession.Close();
        }





        protected void Application_Start()
        {

            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
            NHibernateProfiler.Initialize();
            var cfg = new Configuration();
            cfg.Configure(
                Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    "hibernate.cfg.xml"));
            _sessionFactory = cfg
                .BuildSessionFactory();

            new SchemaUpdate(cfg).Execute(true, true);
        }
    }
}