using System;
using Serilog;

namespace OwinSample.Web.Infrastructure
{
    public class RequestLogContext : IDisposable
    {
        private readonly IDisposable _operation;
        private readonly Guid _requestId = Guid.NewGuid();

        public RequestLogContext(ILogger logger)
        {
            _operation = logger.BeginTimedOperation("HTTP request (" + RequestId + ")", warnIfExceeds: TimeSpan.FromSeconds(1));
        }

        public Guid RequestId
        {
            get { return _requestId; }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing) return;

            _operation.Dispose();
        }
    }
}