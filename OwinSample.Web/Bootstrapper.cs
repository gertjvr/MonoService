using System;
using System.IO;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Bootstrappers.Autofac;
using Nancy.IO;
using Nancy.Responses;
using OwinSample.Web.Infrastructure;
using OwinSample.Web.Infrastructure.Authentication;
using OwinSample.Web.Infrastructure.Environments;
using Serilog;

namespace OwinSample.Web
{
    public class Bootstrapper : AutofacNancyBootstrapper
    {
        private IContainer _container;
        private bool _enforceHttps;
        private Func<NancyContext, ClaimsPrincipal> _claimsPrincipalResolver;

        public Bootstrapper UseContainer(IContainer containerToUse)
        {
            if (ApplicationContainer != null)
                throw new Exception("The ApplicationContainer already exists! This method should be called before the ApplicationContainer is initialized by Nancy.");

            _container = containerToUse;
            return this;
        }

        public Bootstrapper EnforceHttps(bool enforce = true)
        {
            _enforceHttps = enforce;
            return this;
        }

        public Bootstrapper ResolveClaimsPrincipal(Func<NancyContext, ClaimsPrincipal> resolver)
        {
            _claimsPrincipalResolver = resolver;
            return this;
        }

        protected override IRootPathProvider RootPathProvider
        {
            get { return new PathProvider(); }
        }

        protected override ILifetimeScope GetApplicationContainer()
        {
            return _container ?? base.GetApplicationContainer();
        }

        protected override void RequestStartup(ILifetimeScope container, IPipelines pipelines, NancyContext context)
        {
            pipelines.BeforeRequest += async (nancyContext, token) =>
            {
                await LogRequestStart(container, nancyContext, token);

                // Require SSL
                if (_enforceHttps && nancyContext.Request.Url.IsSecure == false)
                {
                    var secureUrl = nancyContext.Request.Url.Clone();
                    secureUrl.Scheme = "https";
                    return new RedirectResponse(secureUrl.ToString(), RedirectResponse.RedirectType.Temporary);
                }

                // Resolve and assign the CurrentUser
                if (_claimsPrincipalResolver != null)
                {
                    var principal = _claimsPrincipalResolver(nancyContext);
                    if (principal != null && principal.Identity.IsAuthenticated)
                    {
                        var claimsIdentity = (ClaimsIdentity)principal.Identity;
                        nancyContext.CurrentUser = new AuthenticatedUser(claimsIdentity);
                    }
                }

                // Otherwise continue with processing this request
                return null;
            };

            pipelines.AfterRequest +=
                (nancyContext, token) => LogRequestComplete(container, nancyContext, token);

            pipelines.OnError +=
                (nancyContext, exception) => LogRequestError(container, nancyContext, exception);
        }

        private Task<Response> LogRequestStart(ILifetimeScope lifetimeScope, NancyContext context, CancellationToken ct)
        {
            return Task.Run(() =>
                            {
                                var logger = lifetimeScope.Resolve<ILogger>();
                                var requestLogContext = lifetimeScope.Resolve<RequestLogContext>();
                                var requestBody = context.Request.Body.ReadAsString();
                                logger.Debug("Begin {HttpMethod} to {RequestUrl} ({RequestId}) {@RequestBody}",
                                             context.Request.Method,
                                             context.Request.Url,
                                             requestLogContext.RequestId,
                                             requestBody);
                                return context.Response;
                            },
                            ct);
        }

        private Task LogRequestComplete(ILifetimeScope lifetimeScope, NancyContext context, CancellationToken ct)
        {
            return Task.Run(() =>
                            {
                                var logger = lifetimeScope.Resolve<ILogger>();
                                logger.Debug("End {HttpMethod} to {RequestUrl}", context.Request.Method, context.Request.Url);
                            },
                            ct);
        }

        private Response LogRequestError(ILifetimeScope lifetimeScope, NancyContext context, Exception exception)
        {
            var logger = lifetimeScope.Resolve<ILogger>();
            logger.Error(exception, "Error on {HttpMethod} to {RequestUrl}: {Message}", context.Request.Method, context.Request.Url, exception.Message);
            return context.Response;
        }
    
        //private byte[] _favicon;
        //protected override byte[] FavIcon
        //{
        //    get { return _favicon ?? (_favicon = LoadFavIcon()); }
        //}

        //private byte[] LoadFavIcon()
        //{
        //    using (var resourceStream = GetType().Assembly.GetManifestResourceStream("OwinSample.Web.favicon.ico"))
        //    {
        //        var tempFavicon = new byte[resourceStream.Length];
        //        resourceStream.Read(tempFavicon, 0, (int)resourceStream.Length);
        //        return tempFavicon;
        //    }
        //}
    }


    public class PathProvider : IRootPathProvider
    {
        public string GetRootPath()
        {
            var x = new DefaultRootPathProvider();
            var orignal = x.GetRootPath();
            
            var fullPath = AppDomain.CurrentDomain.BaseDirectory;
            return Path.GetFullPath(fullPath);
        }
    }

    /// <see cref="http://www.heikura.me/tip-nancyfx-read-request-body-as-string" />
    public static class RequestBodyExtensions
    {
        public static string ReadAsString(this RequestStream requestStream)
        {
            using (var reader = new StreamReader(requestStream))
            {
                var result = reader.ReadToEnd();
                requestStream.Seek(0, SeekOrigin.Begin);
                return result;
            }
        }
    }
}