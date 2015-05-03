using System;
using Owin;

namespace OwinSample.Web.Infrastructure.ErrorHandling
{
    public static class SerilogErrorHandlingExtensions
    {
        public static IAppBuilder UseSerilogErrorHandling(this IAppBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException("app");
            }

            app.Use(typeof(SerilogErrorHandlingMiddleware), app);
            return app;
        }
    }
}