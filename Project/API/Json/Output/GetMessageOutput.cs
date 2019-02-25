using System.Collections.Generic;
using Newtonsoft.Json;
using SinaExpoBot.Domain.Model;

namespace SinaExpoBot.API.Json.Output
{
    public class GetMessageOutput
    {
        [JsonProperty("data")] public GetMessageOutputData Data { get; set; }

    }

    public class GetMessageOutputData
    {
        [JsonProperty("messages")] public List<MessageModel> MessageList { get; set; }

    }
}