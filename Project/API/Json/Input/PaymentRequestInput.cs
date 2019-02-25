using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SinaExpoBot.Domain.Enum;

namespace SinaExpoBot.API.Json.Input
{
    public class PaymentRequestInput
    {
        [JsonProperty("chat_id")] public string ChatId { get; set; }
        [JsonProperty("order_id")] public string OrderId { get; set; }
        [JsonProperty("type")] [JsonConverter(typeof(StringEnumConverter))] public PaymentTypeEnum Type { get; set; }
        [JsonProperty("payment_options")] public PaymentOptions Options { get; set; }
    }

    public class PaymentOptions
    {
        [JsonProperty("amount")] public string Amount { get; set; }
        [JsonProperty("plan_id")] public string PlanId { get; set; }

    }
}