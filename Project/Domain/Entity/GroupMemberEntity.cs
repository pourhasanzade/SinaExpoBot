using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using SinaExpoBot.Domain.Enum;

namespace SinaExpoBot.Domain.Entity
{
    public class GroupMemberEntity:BaseEntity
    {
        public string ChatId { get; set; }
        public int MemberNumber { get; set; }//From 1
        public string Name { get; set; }
        public string NationalCode { get; set; }
        public bool IsDeleted { get; set; }
        public long GroupId { get; set; }
        public GroupEntity Group { get; set; }
    }

    public class GroupMemberEntityConfiguration : EntityTypeConfiguration<GroupMemberEntity>
    {
        public GroupMemberEntityConfiguration()
        {
            Property(x => x.ChatId).HasMaxLength(50);
            Property(x => x.Name).HasMaxLength(20);
            Property(x => x.NationalCode).HasMaxLength(20);

            HasIndex(x => x.ChatId).IsUnique(false);
            HasIndex(x => x.MemberNumber).IsUnique(false);

            ToTable("GroupMembers");
        }
    }
}
