using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Autofac;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Serilog;

namespace OwinSample.Web.Infrastructure.SignalR
{
    /// <summary>
    /// Autofac implementation of the <see cref="Nimbus.DependencyResolution.IDependencyResolver"/> interface.
    /// </summary>
    public class AutofacDependencyResolver : DefaultDependencyResolver
    {
        readonly ILifetimeScope _lifetimeScope;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutofacDependencyResolver" /> class.
        /// </summary>
        /// <param name="lifetimeScope">The lifetime scope that services will be resolved from.</param>
        /// <exception cref="System.ArgumentNullException">
        /// Thrown if <paramref name="lifetimeScope" /> is <see langword="null" />.
        /// </exception>
        public AutofacDependencyResolver(ILifetimeScope lifetimeScope)
        {
            if (lifetimeScope == null)
                throw new ArgumentNullException("lifetimeScope");

            _lifetimeScope = lifetimeScope;
        }

        /// <summary>
        /// Gets the Autofac implementation of the dependency resolver.
        /// </summary>
        public static AutofacDependencyResolver Current
        {
            get { return GlobalHost.DependencyResolver as AutofacDependencyResolver; }
        }

        /// <summary>
        /// Gets the <see cref="ILifetimeScope"/> that was provided to the constructor.
        /// </summary>
        public ILifetimeScope LifetimeScope
        {
            get { return _lifetimeScope; }
        }

        /// <summary>
        /// Get a single instance of a service.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <returns>The single instance if resolved; otherwise, <c>null</c>.</returns>
        public override object GetService(Type serviceType)
        {
            Exception autofacException;
            object service;
            if (TryResolve(serviceType, out autofacException, out service)) return service;

            var fallback = base.GetService(serviceType);
            if (fallback != null || IsIgnored(serviceType)) return fallback;

            if (Debugger.IsAttached) Debugger.Break();
            Log.Error(autofacException, "Autofac and SignalR Cannot resolve type {ServiceType}", serviceType);

            return null;
        }

        [DebuggerStepThrough]
        bool TryResolve(Type serviceType, out Exception autofacException, out object service)
        {
            autofacException = null;
            service = null;

            try
            {
                service = _lifetimeScope.Resolve(serviceType);
                return true;
            }
            catch (Exception e)
            {
                autofacException = e;
            }
            return false;
        }

        /// <summary>
        /// Gets all available instances of a services.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <returns>The list of instances if any were resolved; otherwise, an empty list.</returns>
        public override IEnumerable<object> GetServices(Type serviceType)
        {
            var enumerableServiceType = typeof(IEnumerable<>).MakeGenericType(serviceType);
            var instance = (IEnumerable<object>)_lifetimeScope.Resolve(enumerableServiceType);
            var resolved = instance.Any() ? instance : base.GetServices(serviceType);
            if (resolved == null || resolved.Any() == false)
            {
                if (Debugger.IsAttached) Debugger.Break();
                throw new Exception(string.Format("Autofac and SignalR Cannot resolve enumerable of type '{0}'", serviceType));
            }

            return resolved;
        }

        bool IsIgnored(Type serviceType)
        {
            return new[]
            {
                typeof (IJavaScriptMinifier)
            }
                .Contains(serviceType);
        }
    }
}