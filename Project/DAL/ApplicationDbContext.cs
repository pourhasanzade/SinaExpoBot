using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using SinaExpoBot.Domain.Entity;

namespace SinaExpoBot.DAL
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext() : base("DefaultConnection")
        {
        }

        public DbSet<ButtonEntity> Buttons { get; set; }
        public DbSet<UserDataEntity> UserData { get; set; }
        public DbSet<OrderEntity> Orders { get; set; }
        public DbSet<ConfigEntity> Configs { get; set; }
        public DbSet<ExceptionLogEntity> ExceptionLogs { get; set; }
        public DbSet<GroupEntity> Groups { get; set; }
        public DbSet<GroupMemberEntity> GroupMembers { get; set; }
        public DbSet<FestivalAxesEntity> FestivalAxes { get; set; }
        public DbSet<FestivalFieldEntity> Fields { get; set; }
        public DbSet<FestivalMajorEntity> Majors { get; set; }
        public DbSet<ProvinceEntity> Provinces { get; set; }
        public DbSet<PriceEntity> Prices { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Configurations.Add(new ButtonEntityConfiguration());
            modelBuilder.Configurations.Add(new UserDataEntityConfiguration());
            modelBuilder.Configurations.Add(new OrderEntityConfiguration());
            modelBuilder.Configurations.Add(new ConfigEntityConfiguration());
            modelBuilder.Configurations.Add(new ExceptionLogEntityConfiguration());
            modelBuilder.Configurations.Add(new ProvinceEntityConfiguration());

            modelBuilder.Configurations.Add(new GroupEntityConfiguration());
            modelBuilder.Configurations.Add(new GroupMemberEntityConfiguration());
            modelBuilder.Configurations.Add(new FestivalAxesEntityConfiguration());
            modelBuilder.Configurations.Add(new FieldEntityConfiguration());
            modelBuilder.Configurations.Add(new MajorEntityConfiguration());

            modelBuilder.Configurations.Add(new PriceEntityConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}