namespace DataTransferService.DataAccessLayer.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class PolitermUsers : DbContext
    {
        public PolitermUsers()
            : base("name=PolitermUsers")
        {
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }

        public DbSet<City> Cities { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Mail> Mails { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<City>()
                .HasMany(e => e.Organizations)
                .WithOptional(e => e.City)
                .HasForeignKey(e => e.City_Id);

            modelBuilder.Entity<Mail>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<Organization>()
                .HasMany(e => e.Contacts)
                .WithOptional(e => e.Organization)
                .HasForeignKey(e => e.Organization_Id);

            modelBuilder.Entity<Organization>()
                .HasMany(e => e.Tasks)
                .WithOptional(e => e.Organization)
                .HasForeignKey(e => e.Organization_Id);

            modelBuilder.Entity<Region>()
                .HasMany(e => e.Cities)
                .WithOptional(e => e.Region)
                .HasForeignKey(e => e.Region_Id);

            modelBuilder.Entity<Task>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<Task>()
                .Property(e => e.ProjectCost)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Task>()
                .HasMany(e => e.Mails)
                .WithOptional(e => e.Tasks)
                .HasForeignKey(e => e.Tasks_Id);

            modelBuilder.Entity<User>()
                .Property(e => e.FIO)
                .IsUnicode(false);
        }
    }
}
