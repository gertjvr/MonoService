using System;
using System.Threading;
using Owin;

namespace OwinSample.Web.Infrastructure.Extentions
{
    public static class OwinExtensions
    {
        private const string AppDisposingKey = "host.OnAppDisposing";

        public static void HookDisposal(this IAppBuilder builder, Action callback)
        {
            if (!builder.Properties.ContainsKey(AppDisposingKey))
            {
                return;
            }

            var appDisposing = builder.Properties[AppDisposingKey] as CancellationToken?;

            if (appDisposing.HasValue)
            {
                appDisposing.Value.Register(callback);
            }
        }
    }
}