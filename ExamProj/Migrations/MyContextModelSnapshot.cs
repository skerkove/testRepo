﻿// <auto-generated />
using System;
using ExamProj.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace LoginReg.Migrations
{
    [DbContext(typeof(MyContext))]
    partial class MyContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("ExamProj.Models.Attend", b =>
                {
                    b.Property<int>("AttendId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("OccasionId");

                    b.Property<int>("UserId");

                    b.HasKey("AttendId");

                    b.HasIndex("OccasionId");

                    b.HasIndex("UserId");

                    b.ToTable("Attends");
                });

            modelBuilder.Entity("ExamProj.Models.Occasion", b =>
                {
                    b.Property<int>("OccasionId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CoordinatorName");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<DateTime>("Date");

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<int>("Duration");

                    b.Property<string>("Title")
                        .IsRequired();

                    b.Property<DateTime>("UpdatedAt");

                    b.Property<int>("UserId");

                    b.HasKey("OccasionId");

                    b.HasIndex("UserId");

                    b.ToTable("Occasions");
                });

            modelBuilder.Entity("ExamProj.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(25);

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(25);

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<DateTime>("UpdatedAt");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ExamProj.Models.Attend", b =>
                {
                    b.HasOne("ExamProj.Models.Occasion", "Attending")
                        .WithMany("Participants")
                        .HasForeignKey("OccasionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ExamProj.Models.User", "Participant")
                        .WithMany("GoingTo")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ExamProj.Models.Occasion", b =>
                {
                    b.HasOne("ExamProj.Models.User", "Coordinator")
                        .WithMany("PlannedOccasion")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
