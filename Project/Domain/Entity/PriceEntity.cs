using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Core;
using System.Data.Entity.ModelConfiguration;
using SinaExpoBot.Domain.Enum;

namespace SinaExpoBot.Domain.Entity
{
    public class PriceEntity : BaseEntity
    {
        public CenterTypeEnum? CenterType { get; set; }        
        public long? FestivalAxesId { get; set; }
        public string PriceAmount { get; set; }
        public FestivalAxesEntity FestivalAxes { get; set; }
    }

    public class PriceEntityConfiguration : EntityTypeConfiguration<PriceEntity>
    {
        public PriceEntityConfiguration()
        {
            Property(x => x.PriceAmount).HasMaxLength(10);
            HasIndex(x => x.CenterType).IsUnique(false);

            ToTable("Prices");
        }
    }
}
