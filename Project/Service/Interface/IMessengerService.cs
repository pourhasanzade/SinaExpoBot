using System.Threading.Tasks;
using SinaExpoBot.API.Json.Input;
using SinaExpoBot.API.Json.Output;

namespace SinaExpoBot.Service.Interface
{
    public interface IMessengerService
    {
        Task<SendMessagesOutput> SendMessage(SendMessageInput model);
        Task<GetMessageOutput> GetMessages(long lastMessageId);
        Task<PaymentRequestOutput> PaymentRequest(PaymentRequestInput paymentRequestInput);

        Task<SettlePaymentOutput> SettlePayment(string chatId, string orderId, string paymentToken);
    }
}