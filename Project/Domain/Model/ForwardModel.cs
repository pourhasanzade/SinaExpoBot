using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SinaExpoBot.Domain.Enum;

namespace SinaExpoBot.Domain.Model
{
    public class ForwardModel
    {
        [JsonProperty("type_from")] [JsonConverter(typeof(StringEnumConverter))] public ForwardTypeFromEnum TypeFrom { get; set; }
        [JsonProperty("user_id")] public string UserId { get; set; }
        [JsonProperty("chat_id")] public string ChatId { get; set; }
        [JsonProperty("message_id")] public string MessageId { get; set; }
    }
}