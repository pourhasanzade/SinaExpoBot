using System.Collections.Generic;
using System.Threading.Tasks;
using SinaExpoBot.API.Json.Input;
using SinaExpoBot.API.Json.Output;
using SinaExpoBot.Service.Interface;
using SinaExpoBot.Utility;
using Exception = System.Exception;

namespace SinaExpoBot.Service
{
    public class MessengerService : IMessengerService
    {
        private readonly IApiService _apiService;
        private readonly IExceptionLogService _exceptionLogService;

        public MessengerService(IApiService apiService, IExceptionLogService exceptionLogService)
        {
            _apiService = apiService;
            _exceptionLogService = exceptionLogService;
        }
        private readonly Dictionary<string, string> _botHeader = new Dictionary<string, string> { { "bot_key", Variables.BotKey } };

        

        public async Task<SendMessagesOutput> SendMessage(SendMessageInput model)
        {
            try
            {
                return await _apiService.PostAsync<SendMessagesOutput>(Variables.BotApi, new { method = "sendMessage", data = model }, _botHeader);
            }
            catch (Exception e)
            {
                await _exceptionLogService.LogException(e, model.ChatId, "SendMessage");
                return null;
            }
        }

        public async Task<GetMessageOutput> GetMessages(long lastMessageId)
        {
            try
            {
                return await _apiService.PostAsync<GetMessageOutput>(Variables.BotApi,
                    new
                    {
                        method = "getMessages",
                        data = new
                        {
                            start_message_id = lastMessageId + 1,
                            limit = 10
                        }
                    }, _botHeader);
            }
            catch (Exception e)
            {
                await _exceptionLogService.LogException(e, "", "GetMessages");
                return null;
            }
            
        }

        public async Task<PaymentRequestOutput> PaymentRequest(PaymentRequestInput paymentRequestInput)
        {
            try
            {
                return await _apiService.PostAsync<PaymentRequestOutput>(Variables.BotApi,
                    new
                    {
                        method = "paymentRequest",
                        data = paymentRequestInput
                    }, _botHeader);
            }
            catch (Exception e)
            {
                await _exceptionLogService.LogException(e, "", "PaymentRequest");
                return null;
            }
            
        }

        public async Task<SettlePaymentOutput> SettlePayment(string chatId, string orderId, string paymentToken)
        {
            try
            {
                return await _apiService.PostAsync<SettlePaymentOutput>(Variables.BotApi, new
                {
                    method = "settlePayment",
                    data = new
                    {
                        order_id = orderId.ToString(),
                        chat_id = chatId,
                        payment_token = paymentToken
                    }
                }, _botHeader);
            }
            catch (Exception e)
            {
                await _exceptionLogService.LogException(e, "", "PaymentRequest");
                return null;
            }
            
        }

    }
}