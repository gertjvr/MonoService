using System;

namespace OwinSample.Web.Infrastructure.Clock
{
    public interface IClock
    {
        DateTimeOffset UtcNow { get; }
    }
}