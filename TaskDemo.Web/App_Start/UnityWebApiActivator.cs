using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.WebApi;
using TaskDemo.Services.Web.Unity;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(TaskDemo.Web.App_Start.UnityWebApiActivator), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethod(typeof(TaskDemo.Web.App_Start.UnityWebApiActivator), "Shutdown")]

namespace TaskDemo.Web.App_Start
{
    /// <summary>Provides the bootstrapping for integrating Unity with WebApi when it is hosted in ASP.NET</summary>
    public static class UnityWebApiActivator
    {
        /// <summary>Integrates Unity when the application starts.</summary>
        public static void Start() 
        {
            var config = GlobalConfiguration.Configuration;
            var container = UnityConfig.GetConfiguredContainer();

            container.RegisterInstance(config);

            // web api setup
            container.RegisterInstance<IHttpControllerActivator>(new WebApiUnityControllerActivator(config.Services.GetHttpControllerActivator()));
            container.RegisterType<IControllerActivator, SiteUnityControllerActivator>();

            // Set MVC controller factory
            ControllerBuilder.Current.SetControllerFactory(new UnityMvcControllerFactory(container));

            // Use UnityHierarchicalDependencyResolver if you want to use a new child container for each IHttpController resolution.
            var resolver = new UnityHierarchicalDependencyResolver(UnityConfig.GetConfiguredContainer());
            //var resolver = new UnityDependencyResolver(UnityConfig.GetConfiguredContainer());

            GlobalConfiguration.Configuration.DependencyResolver = resolver;
        }

        /// <summary>Disposes the Unity container when the application is shut down.</summary>
        public static void Shutdown()
        {
            var container = UnityConfig.GetConfiguredContainer();
            container.Dispose();
        }
    }
}
