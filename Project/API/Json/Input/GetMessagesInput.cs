using Newtonsoft.Json;
using SinaExpoBot.Domain.Enum;
using SinaExpoBot.Domain.Model;

namespace SinaExpoBot.API.Json.Input
{
    public class GetMessagesInput
    {
        [JsonProperty("message")] public MessageModel Mesage { get; set; }
        [JsonProperty("type")] public MessageBehaviourTypeEnum Type { get; set; }

    }
}