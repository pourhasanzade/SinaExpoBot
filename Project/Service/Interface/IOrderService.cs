using System.Threading.Tasks;
using SinaExpoBot.Domain.Entity;
using SinaExpoBot.Domain.Enum;

namespace SinaExpoBot.Service.Interface
{
    public interface IOrderService
    {
        Task<OrderEntity> GetOrder(long orderId);
        Task<OrderEntity> UpdateOrderStatus(long orderId, OrderStatusEnum status);
        Task<OrderEntity> UpdateSettleStatus(long orderId, SettleStatusEnum status);
        Task<OrderEntity> AddPayment(string chatId, PaymentTypeEnum paymentType, string amount, int planId);
        Task<OrderEntity> UpdatePaymentToken(long orderId, string paymentToken);
    }
}