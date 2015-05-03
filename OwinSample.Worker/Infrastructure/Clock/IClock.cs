using System;

namespace OwinSample.Worker.Infrastructure.Clock
{
    public interface IClock
    {
        DateTimeOffset UtcNow { get; }
    }
}