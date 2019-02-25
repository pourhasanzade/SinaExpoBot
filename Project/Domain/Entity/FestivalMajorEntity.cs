using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using SinaExpoBot.Domain.Enum;

namespace SinaExpoBot.Domain.Entity
{
    public class FestivalMajorEntity
    {
        [Key]
        public long Id { get; set; } 
        public long? FieldId { get; set; }
        public string Title { get; set; }

        public FestivalFieldEntity Field { get; set; }
    }

    public class MajorEntityConfiguration : EntityTypeConfiguration<FestivalMajorEntity>
    {
        public MajorEntityConfiguration()
        {
            Property(x => x.Title).HasMaxLength(50);            

            ToTable("FestivalMajors");
        }
    }
}
