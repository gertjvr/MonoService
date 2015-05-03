using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Autofac;
using Autofac.Builder;

namespace OwinSample.Worker
    {
        public static class IoC
        {
            [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Io")]
            public static IContainer LetThereBeIoC(ContainerBuildOptions containerBuildOptions = ContainerBuildOptions.None, Action<ContainerBuilder> preHooks = null)
            {
                var builder = new ContainerBuilder();
                builder.RegisterAssemblyModules(Assembly.GetExecutingAssembly());
                if (preHooks != null) preHooks(builder);

                return builder.Build(containerBuildOptions);
            }
        }
    }