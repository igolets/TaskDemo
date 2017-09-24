using System;
using System.Web.Mvc;

namespace TaskDemo.Services.Web.Unity
{
    /// <inheritdoc />
    /// <summary>
    /// Unity controller activator for regular controllers.
    /// </summary>
    public class SiteUnityControllerActivator : IControllerActivator
    {
        #region Public methods

        public IController Create(System.Web.Routing.RequestContext requestContext, Type controllerType)
        {
            return DependencyResolver.Current.GetService(controllerType) as IController;
        }

        #endregion Public methods
    }
}