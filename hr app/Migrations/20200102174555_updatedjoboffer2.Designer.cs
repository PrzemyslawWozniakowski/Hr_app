﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using hr_app.EntityFramework;

namespace hr_app.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20200102174555_updatedjoboffer2")]
    partial class updatedjoboffer2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.11-servicing-32099")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("hr_app.Models.Company", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("hr_app.Models.JobApplication", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("ContactAgreement");

                    b.Property<string>("CvUrl");

                    b.Property<string>("EmailAddress");

                    b.Property<string>("FirstName");

                    b.Property<int>("JobOfferId");

                    b.Property<string>("LastName");

                    b.Property<string>("PhoneNumber");

                    b.HasKey("Id");

                    b.HasIndex("JobOfferId");

                    b.ToTable("JobApplications");
                });

            modelBuilder.Entity("hr_app.Models.JobOffer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CompanyId");

                    b.Property<DateTime>("Created");

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<string>("JobTitle")
                        .IsRequired();

                    b.Property<string>("Location");

                    b.Property<decimal?>("SalaryFrom");

                    b.Property<decimal?>("SalaryTo");

                    b.Property<int>("UserId");

                    b.Property<DateTime?>("ValidUntil");

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

                    b.Property<string>("Email");

                    b.Property<string>("Name");

                    b.Property<int>("Role");

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
#pragma warning restore 612, 618
        }
    }
}
