using System;
using Microsoft.Owin.Extensions;
using Owin;

namespace OwinSample.Web.Infrastructure.JwtTokenAuthentication
{
    public static class JwtTokenAuthenticationExtensions
    {
        public static IAppBuilder UseJwtTokenAuthentication(this IAppBuilder app, JwtTokenAuthenticationOptions options)
        {
            if (app == null)
            {
                throw new ArgumentNullException("app");
            }
            if (options == null)
            {
                throw new ArgumentNullException("options");
            }

            app.Use(typeof(JwtTokenAuthenticationMiddleware), app, options);
            app.UseStageMarker(PipelineStage.Authenticate);
            return app;
        }

        public static IAppBuilder UseJwtTokenAuthentication(this IAppBuilder app, string issuer, string audience, string clientSecret)
        {
            return
                app.UseJwtTokenAuthentication(
                    new JwtTokenAuthenticationOptions(issuer, audience, clientSecret));
        }
    }
}