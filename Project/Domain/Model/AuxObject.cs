using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SinaExpoBot.Domain.Enum;

namespace SinaExpoBot.Domain.Model
{
    public class AuxObject
    {
        [JsonProperty("button_id")] public string ButtonId { get; set; }
        [JsonProperty("order_id")] public string OrderId { get; set; }
        [JsonProperty("order_status")] [JsonConverter(typeof(StringEnumConverter))] public OrderStatusEnum OrderStatus { get; set; }

    }
}