using Newtonsoft.Json;

namespace SinaExpoBot.Domain.Model
{
    public class ButtonCallModel
    {
        [JsonProperty("phone_number")] public string PhoneNumber { get; set; }

    }
}