using Newtonsoft.Json;

namespace SinaExpoBot.Domain.Model
{
    public class ButtonNumberPickerModel
    {
        [JsonProperty("min_value")] public string MinValue { get; set; }
        [JsonProperty("max_value")] public string MaxValue { get; set; }
        [JsonProperty("default_value")] public string DefaultValue { get; set; }
    }
}