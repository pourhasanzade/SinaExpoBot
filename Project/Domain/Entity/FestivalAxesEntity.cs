using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using SinaExpoBot.Domain.Enum;

namespace SinaExpoBot.Domain.Entity
{
    public class FestivalAxesEntity
    {
        [Key]
        public long Id { get; set; }       
        public string Title { get; set; }
    }

    public class FestivalAxesEntityConfiguration : EntityTypeConfiguration<FestivalAxesEntity>
    {
        public FestivalAxesEntityConfiguration()
        {
            Property(x => x.Title).HasMaxLength(50);            

            ToTable("FestivalAxes");
        }
    }
}
