using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using Microsoft.Practices.Unity;

namespace TaskDemo.Services.Web.Unity
{
    /// <inheritdoc />
    /// <summary>
    /// Unity controller activator for Web API controllers.
    /// </summary>
    /// <remarks>Based on this article: http://www.nesterovsky-bros.com/weblog/2014/12/15/DependencyInjectionInASPNETWebAPI2.aspx </remarks>
    public class WebApiUnityControllerActivator : IHttpControllerActivator
    {
        #region Nested types

        /// <inheritdoc cref="IHttpController" />
        /// <summary>
        /// A controller wrapper.
        /// </summary>
        private class Controller : IHttpController, IDisposable
        {
            #region Public properties

            /// <summary>
            /// Base controller activator.
            /// </summary>
            public IHttpControllerActivator Activator;

            /// <summary>
            /// Controller type.
            /// </summary>
            public Type ControllerType;

            #endregion Public properties

            /// <summary>
            /// A controller instance.
            /// </summary>
            private IHttpController _controller;

            /// <inheritdoc />
            /// <summary>
            /// Disposes controller.
            /// </summary>
            public void Dispose()
            {
                if (_controller is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }

            /// <inheritdoc />
            /// <summary>
            /// Executes an action.
            /// </summary>
            /// <param name="controllerContext">Controller context.</param>
            /// <param name="cancellationToken">Cancellation token.</param>
            /// <returns>Response message.</returns>
            public Task<HttpResponseMessage> ExecuteAsync(
                HttpControllerContext controllerContext,
                CancellationToken cancellationToken)
            {
                if (_controller == null)
                {
                    var request = controllerContext.Request;

                    if (request
                        .GetDependencyScope()
                        .GetService(typeof(IUnityContainer)) is IUnityContainer container)
                    {
                        container.RegisterInstance(controllerContext);
                        container.RegisterInstance(request);
                        container.RegisterInstance(cancellationToken);

                        UnityTypesRegistrator.RegisterTypes(container);
                    }

                    _controller = Activator.Create(
                        request,
                        controllerContext.ControllerDescriptor,
                        ControllerType);
                }

                controllerContext.Controller = _controller;

                return _controller.ExecuteAsync(controllerContext, cancellationToken);
            }
        }

        #endregion Nested types

        #region Public methods

        /// <summary>
        /// Creates an UnityControllerActivator instance.
        /// </summary>
        /// <param name="activator">Base activator.</param>
        public WebApiUnityControllerActivator(IHttpControllerActivator activator)
        {
            _activator = activator ?? throw new ArgumentException("activator");
        }

        /// <inheritdoc />
        /// <summary>
        /// Creates a controller wrapper.
        /// </summary>
        /// <param name="request">A http request.</param>
        /// <param name="controllerDescriptor">Controller descriptor.</param>
        /// <param name="controllerType">Controller type.</param>
        /// <returns>A controller wrapper.</returns>
        public IHttpController Create(
            HttpRequestMessage request,
            HttpControllerDescriptor controllerDescriptor,
            Type controllerType)
        {
            return new Controller
            {
                Activator = _activator,
                ControllerType = controllerType
            };
        }

        #endregion Public methods

        #region Fields

        /// <summary>
        /// Base controller activator.
        /// </summary>
        private readonly IHttpControllerActivator _activator;

        #endregion Fields
    }
}