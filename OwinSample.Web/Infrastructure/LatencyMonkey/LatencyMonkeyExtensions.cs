using System;
using Owin;

namespace OwinSample.Web.Infrastructure.LatencyMonkey
{
    public static class LatencyMonkeyExtensions
    {
        public static IAppBuilder UseLatencyMonkey(this IAppBuilder builder)
        {
            return UseLatencyMonkey(builder, new LatencyMonkeyOptions());
        }

        public static IAppBuilder UseLatencyMonkey(this IAppBuilder builder, Action<LatencyMonkeyOptions> configure)
        {
            var options = new LatencyMonkeyOptions();
            configure(options);
            return UseLatencyMonkey(builder, options);
        }

        public static IAppBuilder UseLatencyMonkey(this IAppBuilder builder, LatencyMonkeyOptions options)
        {
            return builder.Use(typeof(LatencyMonkeyMiddleware), options);
        }
    }
}