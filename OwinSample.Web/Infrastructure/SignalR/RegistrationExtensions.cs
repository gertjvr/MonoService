using System.Reflection;
using Autofac;
using Autofac.Builder;
using Autofac.Features.Scanning;
using Microsoft.AspNet.SignalR;

namespace OwinSample.Web.Infrastructure.SignalR
{
    public static class RegistrationExtensions
    {
        /// <summary>
        /// Registers SignalR hubs for a given assembly.
        /// </summary>
        /// <param name="builder"> The Autofac container builder. </param>
        /// <param name="assemblies"> The assemblies. </param>
        /// <returns> A registration builder. </returns>
        public static IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> RegisterHubs(this ContainerBuilder builder, params Assembly[] assemblies)
        {
            return builder.RegisterAssemblyTypes(assemblies).Where(x => typeof(Hub).IsAssignableFrom(x));
        }
    }
}