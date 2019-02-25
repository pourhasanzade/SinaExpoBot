using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SinaExpoBot.Domain.Enum;

namespace SinaExpoBot.Domain.Model
{
    public class ButtonCalendarModel
    {
        [JsonProperty("default_value")] public string DefaultValue { get; set; }
        [JsonProperty("type")] [JsonConverter(typeof(StringEnumConverter))] public ButtonCalendarTypeEnum Type { get; set; }
        [JsonProperty("min_year")] public string MaxYear { get; set; }
        [JsonProperty("max_year")] public string MinYear { get; set; }
    }
}