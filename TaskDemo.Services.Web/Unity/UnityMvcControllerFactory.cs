using System;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.Practices.Unity;

namespace TaskDemo.Services.Web.Unity
{
    /// <inheritdoc />
    /// <summary>
    /// Controller factory that uses Unity
    /// </summary>
    /// <seealso cref="T:System.Web.Mvc.DefaultControllerFactory" />
    public class UnityMvcControllerFactory : DefaultControllerFactory
    {
        #region Constructors

        public UnityMvcControllerFactory(IUnityContainer container)
        {
            _container = container;
        }

        #endregion

        #region Overrides of DefaultControllerFactory

        /// <inheritdoc />
        /// <summary>Retrieves the controller instance for the specified request context and controller type.</summary>
        /// <returns>The controller instance.</returns>
        /// <param name="requestContext">The context of the HTTP request, which includes the HTTP context and route data.</param>
        /// <param name="controllerType">The type of the controller.</param>
        /// <exception cref="T:System.Web.HttpException">
        /// <paramref name="controllerType" /> is null.</exception>
        /// <exception cref="T:System.ArgumentException">
        /// <paramref name="controllerType" /> cannot be assigned.</exception>
        /// <exception cref="T:System.InvalidOperationException">An instance of <paramref name="controllerType" /> cannot be created.</exception>
        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            if (controllerType == null)
            {
                throw new ArgumentNullException(nameof(controllerType));
            }

            if (!typeof(IController).IsAssignableFrom(controllerType))
            {
                throw new ArgumentException($"Type requested is not a controller: {controllerType.Name}", nameof(controllerType));
            }

            UnityTypesRegistrator.RegisterTypes(_container);

            return _container.Resolve(controllerType) as IController;
        }

        #endregion

        #region Fields

        readonly IUnityContainer _container;

        #endregion
    }
}
