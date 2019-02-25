using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using SinaExpoBot.Domain.Enum;

namespace SinaExpoBot.Domain.Entity
{
    public class ProvinceEntity
    {
        [Key]
        public long Id { get; set; } 
        public string Title { get; set; }
    }

    public class ProvinceEntityConfiguration : EntityTypeConfiguration<ProvinceEntity>
    {
        public ProvinceEntityConfiguration()
        {
            Property(x => x.Title).HasMaxLength(50);            

            ToTable("Provinces");
        }
    }
}
