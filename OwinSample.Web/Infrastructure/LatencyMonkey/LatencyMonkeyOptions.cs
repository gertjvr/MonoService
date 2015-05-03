using System;

namespace OwinSample.Web.Infrastructure.LatencyMonkey
{
    public class LatencyMonkeyOptions
    {
        static readonly Random Rand = new Random();
        public LatencyMonkeyOptions()
        {
            MinimumLatency = TimeSpan.FromMilliseconds(100);
            MaximumLatency = TimeSpan.FromMilliseconds(2000);
            HowLatentIsThisMonkey = () => TimeSpan.FromMilliseconds(Rand.Next((int)MinimumLatency.TotalMilliseconds, (int)MaximumLatency.TotalMilliseconds));
        }

        public Func<TimeSpan> HowLatentIsThisMonkey { get; set; }
        public TimeSpan MinimumLatency { get; set; }
        public TimeSpan MaximumLatency { get; set; }
    }
}