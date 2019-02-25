using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Core;
using System.Data.Entity.ModelConfiguration;
using SinaExpoBot.Domain.Enum;

namespace SinaExpoBot.Domain.Entity
{
    public class GroupEntity : BaseEntity
    {
        public string ChatId { get; set; }

        //--------- Center Info -----------//
        public string CenterName { get; set; }
        public CenterTypeEnum? CenterType { get; set; }
        public CenterGenderEnum? CenterGenderType { get; set; }
        public long? ProvinceId { get; set; }
        public string ProvinceName { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string Phone { get; set; }
        public string ManagerName { get; set; }

        //--------- Supervisor info ---------//
        public string SupervisorName { get; set; }
        public string SupervisorMobile { get; set; }
        public string SupervisorEmail { get; set; }

        //-------- Project Info -----------------//
        public string ProjectSubject { get; set; }// موضوع

        public long? FestivalAxId { get; set; }
        public string AxTitle { get; set; }// محور جشنواره
        public long? FestivalFieldId { get; set; }
        public string FieldTitle { get; set; }// رشته تحصیلی
        public long? FestivalMajorId { get; set; }
        public string MajorTitle { get; set; } //گرایش 
        //--------- Members ----------------//
        public int? MembersCount { get; set; }
        public MembersGradeEnum? MembersGrade { get; set; }

        //-------- Pay Info ----------------//
        public DateTime? PayDate { get; set; }
        public string Price { get; set; }
        public long? OrderId { get; set; }
        public bool IsFinished { get; set; }

        public FestivalAxesEntity FestivalAxes { get; set; }
        public FestivalFieldEntity FestivalField { get; set; }
        public FestivalMajorEntity FestivalMajor { get; set; }
        public OrderEntity Order { get; set; }
        public ProvinceEntity Province { get; set; }
    }

    public class GroupEntityConfiguration : EntityTypeConfiguration<GroupEntity>
    {
        public GroupEntityConfiguration()
        {
            Property(x => x.ChatId).HasMaxLength(50);
            Property(x => x.CenterName).HasMaxLength(100);
            Property(x => x.ProvinceName).HasMaxLength(50);
            Property(x => x.Address).HasMaxLength(2000);
            Property(x => x.PostalCode).HasMaxLength(10);
            Property(x => x.Phone).HasMaxLength(20);
            Property(x => x.ManagerName).HasMaxLength(100);
            Property(x => x.SupervisorName).HasMaxLength(100);
            Property(x => x.SupervisorMobile).HasMaxLength(11);
            Property(x => x.SupervisorEmail).HasMaxLength(50);
            Property(x => x.AxTitle).HasMaxLength(50);
            Property(x => x.FieldTitle).HasMaxLength(50);
            Property(x => x.MajorTitle).HasMaxLength(50);

            Property(x => x.Price).HasMaxLength(20);

            HasIndex(x => x.ChatId).IsUnique(false);
            HasIndex(x => x.IsFinished).IsUnique(false);

            ToTable("Groups");
        }
    }
}
