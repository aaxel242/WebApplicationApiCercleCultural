using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net.Http.Formatting;

public static class Formatters
{
    public static JsonMediaTypeFormatter SnakeCaseFormatter()
    {
        return new JsonMediaTypeFormatter
        {
            SerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver(), // respeta exacto PascalCase/underscore
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc
            }
        };
    }
}
