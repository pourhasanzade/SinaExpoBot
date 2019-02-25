using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using SinaExpoBot.DAL;
using SinaExpoBot.Domain.Entity;
using SinaExpoBot.Domain.Enum;
using SinaExpoBot.Service.Interface;

namespace SinaExpoBot.Service
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;

        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<OrderEntity> GetOrder(long orderId)
        {
            return await _context.Orders.FirstOrDefaultAsync(x => x.Id == orderId);
        }

        public async Task<OrderEntity> UpdateOrderStatus(long orderId, OrderStatusEnum status)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == orderId);
            if (order == null) return null;

            order.OrderStatus = status;
            await _context.SaveChangesAsync();

            return order;
        }

        public async Task<OrderEntity> UpdateSettleStatus(long orderId, SettleStatusEnum status)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == orderId);
            if (order == null) return null;

            order.SettleStatus = status;
            await _context.SaveChangesAsync();

            return order;
        }

        public async Task<OrderEntity> AddPayment(string chatId, PaymentTypeEnum paymentType, string amount,
            int planId)
        {
            var order = new OrderEntity
            {
                ChatId = chatId,
                Status = PayemtnStatusEnum.NotStarted,
                Amount = amount,
                Type = paymentType,
                PlanId = planId,
            };
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<OrderEntity> UpdatePaymentToken(long orderId, string paymentToken)
        {
            var order = await GetOrder(orderId);
            if (order == null) return null;

            order.PaymentToken = paymentToken;
            await _context.SaveChangesAsync();
            return order;
        }
    }
}