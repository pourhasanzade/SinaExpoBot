using Newtonsoft.Json;

namespace SinaExpoBot.Domain.Model
{
    public class ButtonAlertModel
    {
        [JsonProperty("message")] public string Message { get; set; }

    }
}