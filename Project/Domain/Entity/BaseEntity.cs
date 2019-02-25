using System;
using System.ComponentModel.DataAnnotations;

namespace SinaExpoBot.Domain.Entity
{
    public class BaseEntity
    {
        public BaseEntity()
        {
            CreateDateTime = DateTime.Now;
        }

        [Key]
        public long Id { get; set; }

        public DateTime CreateDateTime { get; set; }
    }
}
