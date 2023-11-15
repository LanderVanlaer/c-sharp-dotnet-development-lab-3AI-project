﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using c_sharp_dotnet_development_lab_3AI_project.database;

#nullable disable

namespace c_sharp_dotnet_development_lab_3AI_project.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    partial class DatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("c_sharp_dotnet_development_lab_3AI_project.database.entities.Group", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("c_sharp_dotnet_development_lab_3AI_project.database.entities.Payment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<Guid>("GroupId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("c_sharp_dotnet_development_lab_3AI_project.database.entities.PaymentRecord", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime(6)");

                    b.Property<Guid>("PaymentId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("PaymentId");

                    b.HasIndex("UserId");

                    b.ToTable("PaymentRecords");
                });

            modelBuilder.Entity("c_sharp_dotnet_development_lab_3AI_project.database.entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime(6)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("c_sharp_dotnet_development_lab_3AI_project.database.entities.UserGroup", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("GroupId")
                        .HasColumnType("char(36)");

                    b.HasKey("UserId", "GroupId");

                    b.HasIndex("GroupId");

                    b.ToTable("UserGroups");
                });

            modelBuilder.Entity("c_sharp_dotnet_development_lab_3AI_project.database.entities.Payment", b =>
                {
                    b.HasOne("c_sharp_dotnet_development_lab_3AI_project.database.entities.Group", "Group")
                        .WithMany("Payments")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Group");
                });

            modelBuilder.Entity("c_sharp_dotnet_development_lab_3AI_project.database.entities.PaymentRecord", b =>
                {
                    b.HasOne("c_sharp_dotnet_development_lab_3AI_project.database.entities.Payment", "Payment")
                        .WithMany("PaymentRecords")
                        .HasForeignKey("PaymentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("c_sharp_dotnet_development_lab_3AI_project.database.entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Payment");

                    b.Navigation("User");
                });

            modelBuilder.Entity("c_sharp_dotnet_development_lab_3AI_project.database.entities.UserGroup", b =>
                {
                    b.HasOne("c_sharp_dotnet_development_lab_3AI_project.database.entities.Group", "Group")
                        .WithMany("UserGroups")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("c_sharp_dotnet_development_lab_3AI_project.database.entities.User", "User")
                        .WithMany("UserGroups")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Group");

                    b.Navigation("User");
                });

            modelBuilder.Entity("c_sharp_dotnet_development_lab_3AI_project.database.entities.Group", b =>
                {
                    b.Navigation("Payments");

                    b.Navigation("UserGroups");
                });

            modelBuilder.Entity("c_sharp_dotnet_development_lab_3AI_project.database.entities.Payment", b =>
                {
                    b.Navigation("PaymentRecords");
                });

            modelBuilder.Entity("c_sharp_dotnet_development_lab_3AI_project.database.entities.User", b =>
                {
                    b.Navigation("UserGroups");
                });
#pragma warning restore 612, 618
        }
    }
}
