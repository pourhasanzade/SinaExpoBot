using System.Data.Entity.ModelConfiguration;
using SinaExpoBot.Domain.Enum;

namespace SinaExpoBot.Domain.Entity
{
    public class OrderEntity : BaseEntity
    {
        public string ChatId { get; set; }
        public string PaymentToken { get; set; }
        public PaymentTypeEnum Type { get; set; }
        public PayemtnStatusEnum Status { get; set; }
        public SettleStatusEnum SettleStatus { get; set; }
        public OrderStatusEnum OrderStatus { get; set; }
        public string Amount { get; set; }
        public int? PlanId { get; set; }
    }

    public class OrderEntityConfiguration : EntityTypeConfiguration<OrderEntity>
    {
        public OrderEntityConfiguration()
        {
            Property(x => x.ChatId).HasMaxLength(50);
            Property(x => x.PaymentToken).HasMaxLength(250);
            Property(x => x.Amount).HasMaxLength(20);

            ToTable("Orders");
        }
    }
}
