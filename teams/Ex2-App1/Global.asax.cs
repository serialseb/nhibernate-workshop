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

namespace Ex_App1
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static ISessionFactory _sessionFactory;

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default",                                              // Route name
                "{controller}/{action}/{id}",                           // URL with parameters
                new { controller = "Home", action = "Index", id = "" }  // Parameter defaults
            );

        }

        private const string NHIBERNATE_SESSION = "NHIBERNATE_SESSION";
        public static ISession NHibernateSession
        {
            get
            {
                return (ISession)HttpContext.Current.Items[NHIBERNATE_SESSION];
            }
            set { HttpContext.Current.Items[NHIBERNATE_SESSION] = value; }
        }

        protected void Application_BeginRequest()
        {
            NHibernateSession = _sessionFactory.OpenSession();
        }
        protected void Application_EndRequest()
        {
            NHibernateSession.Close();
        }
        protected void Application_Start()
        {
            RegisterRoutes(RouteTable.Routes);
            NHibernateProfiler.Initialize();
            var cfg = new Configuration();
            _sessionFactory = cfg
                .Configure(
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                    "hibernate.cfg.xml"))
                .BuildSessionFactory();

            //new SchemaExport(cfg).Execute(true, true, true);
            new SchemaUpdate(cfg).Execute(true, true);

        }
    }
}