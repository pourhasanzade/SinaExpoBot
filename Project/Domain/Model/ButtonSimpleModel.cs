using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SinaExpoBot.Domain.Enum;

namespace SinaExpoBot.Domain.Model
{
    public class ButtonSimpleModel
    {
        [JsonProperty("text")] public string Text { get; set; }
        [JsonProperty("image_url")] public string ImageUrl { get; set; }
        [JsonProperty("type")] [JsonConverter(typeof(StringEnumConverter))] public ButtonSimpleTypeEnum Type { get; set; }
    }
}