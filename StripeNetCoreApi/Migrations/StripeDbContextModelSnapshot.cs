﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StripeNetCoreApi.Model;

namespace StripeNetCoreApi.Migrations
{
    [DbContext(typeof(StripeDbContext))]
    partial class StripeDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("StripeNetCoreApi.Entity.Address", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("City")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Country")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("DateCreated")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("DateDeleted")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("DateModified")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Line1")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Line2")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("PostalCode")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("State")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("StripeNetCoreApi.Entity.Card", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("DateCreated")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("DateDeleted")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("DateModified")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("StripeCardId")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Cards");
                });

            modelBuilder.Entity("StripeNetCoreApi.Entity.Roles", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("NormalizedName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("Roles");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Administrator",
                            NormalizedName = "Admin"
                        },
                        new
                        {
                            Id = 2,
                            Name = "ServiceProvider",
                            NormalizedName = "ServiceProvider"
                        },
                        new
                        {
                            Id = 3,
                            Name = "User",
                            NormalizedName = "User"
                        });
                });

            modelBuilder.Entity("StripeNetCoreApi.Entity.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int?>("AddressId")
                        .HasColumnType("int");

                    b.Property<string>("Avatar")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Birthdate")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("DateCreated")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("DateDeleted")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("DateModified")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Email")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("ForgetPasswordCode")
                        .HasColumnType("int");

                    b.Property<bool>("IsForget")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Password")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Phone_Number")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int?>("RoleId")
                        .HasColumnType("int");

                    b.Property<string>("Stripe_CustomerId")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("UserName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.HasIndex("AddressId");

                    b.HasIndex("RoleId");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Email = "admin@localhost",
                            ForgetPasswordCode = 0,
                            IsForget = false,
                            Name = "Admin",
                            Password = "TeRqx0VKPwHDAzmprkFpC/LHHVjulk77pScSe/qpAac=",
                            RoleId = 1
                        });
                });

            modelBuilder.Entity("StripeNetCoreApi.Entity.UserSession", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("AccessToken")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("CreationTime")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("DateCreated")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("DateDeleted")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("DateModified")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("LastModificationTime")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserSessions");
                });

            modelBuilder.Entity("StripeNetCoreApi.Entity.Card", b =>
                {
                    b.HasOne("StripeNetCoreApi.Entity.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("StripeNetCoreApi.Entity.User", b =>
                {
                    b.HasOne("StripeNetCoreApi.Entity.Address", "Address")
                        .WithMany()
                        .HasForeignKey("AddressId");

                    b.HasOne("StripeNetCoreApi.Entity.Roles", "Roles")
                        .WithMany()
                        .HasForeignKey("RoleId");
                });

            modelBuilder.Entity("StripeNetCoreApi.Entity.UserSession", b =>
                {
                    b.HasOne("StripeNetCoreApi.Entity.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
