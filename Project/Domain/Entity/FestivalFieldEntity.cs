using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using SinaExpoBot.Domain.Enum;

namespace SinaExpoBot.Domain.Entity
{
    public class FestivalFieldEntity
    {
        [Key]
        public long Id { get; set; } 
        public long? FestivalAxesId { get; set; }
        public string Title { get; set; }

        public FestivalAxesEntity FestivalAxes { get; set; }
    }

    public class FieldEntityConfiguration : EntityTypeConfiguration<FestivalFieldEntity>
    {
        public FieldEntityConfiguration()
        {
            Property(x => x.Title).HasMaxLength(50);            

            ToTable("FestivalFields");
        }
    }
}
