using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace OwinSample.Worker.Infrastructure
{
    public class CustomJsonSerializer : JsonSerializer
    {
        public CustomJsonSerializer()
        {
            ContractResolver = new DefaultContractResolver();
            Formatting = Formatting.Indented;
            DefaultValueHandling = DefaultValueHandling.Ignore;
            DateParseHandling = DateParseHandling.DateTimeOffset;
            DateFormatHandling = DateFormatHandling.MicrosoftDateFormat;
        }
    }
}