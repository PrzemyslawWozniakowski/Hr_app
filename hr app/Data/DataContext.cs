using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using hr_app.EntityFramework;
using hr_app.Models;
using Enums;
namespace hr_app.EntityFramework
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<JobApplication> JobApplications { get; set; }
        public DbSet<JobOffer> JobOffers { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<User> Users { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity("hr_app.Models.Company", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                b.HasKey("Id");

                b.ToTable("Companies");
            });

            modelBuilder.Entity("hr_app.Models.JobApplication", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                b.HasKey("Id");

                b.HasIndex("JobOfferId");

                b.ToTable("JobApplications");
            });

            modelBuilder.Entity("hr_app.Models.JobOffer", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                b.HasKey("Id");

                b.HasIndex("CompanyId");

                b.HasIndex("UserId");

                b.ToTable("JobOffers");
            });

            modelBuilder.Entity("hr_app.Models.User", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                b.HasKey("Id");

                b.ToTable("Users");
            });

            modelBuilder.Entity("hr_app.Models.JobApplication", b =>
            {
                b.HasOne("hr_app.Models.JobOffer")
                    .WithMany("JobApplications")
                    .HasForeignKey("JobOfferId")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity("hr_app.Models.JobOffer", b =>
            {
                b.HasOne("hr_app.Models.Company", "Company")
                    .WithMany()
                    .HasForeignKey("CompanyId")
                    .OnDelete(DeleteBehavior.Cascade);

                b.HasOne("hr_app.Models.User", "User")
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity("hr_app.Models.User", b =>
            {
                b.HasData(new User {Id=1, Name = "Przemek", Role = Enums.Role.Admin, Email = "przemek-98@wp.pl" },
                           new User { Id = 2, Name = "przemek", Role = Enums.Role.HR, Email = "przemek.wozniakowskiwaw@gmail.com" },
                           new User { Id = 3, Name = "przemek", Role = Enums.Role.User, Email = "piroman916@gmail.com" });
            });


        }
    }
}
