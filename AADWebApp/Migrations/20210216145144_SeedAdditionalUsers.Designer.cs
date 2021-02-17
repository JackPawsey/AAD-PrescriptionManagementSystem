﻿// <auto-generated />
using System;
using AADWebApp.Areas.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AADWebApp.Migrations
{
    [DbContext(typeof(AuthDbContext))]
    [Migration("20210216145144_SeedAdditionalUsers")]
    partial class SeedAdditionalUsers
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("AADWebApp.Areas.Identity.Data.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("GeneralPractioner")
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NHSNumber")
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");

                    b.HasData(
                        new
                        {
                            Id = "be2497f5-ab1f-4824-9a94-a14747bcccd7",
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "2a4673d9-4280-4f31-abf5-5c61c83129c8",
                            Email = "cloudcrusaderssystems@gmail.com",
                            EmailConfirmed = false,
                            LockoutEnabled = false,
                            NormalizedEmail = "CLOUDCRUSADERSSYSTEMS@GMAIL.COM",
                            NormalizedUserName = "CLOUDCRUSADERSSYSTEMS@GMAIL.COM",
                            PasswordHash = "AQAAAAEAACcQAAAAEN/QGFXZopgiLSihLbM0His3Tv2Vkue1+jt0D87bDjpKqA2HHBAKQjZApvamvROaVA==",
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "ec71e725-02cb-4dcf-9127-d95ba13e0a6d",
                            TwoFactorEnabled = false,
                            UserName = "cloudcrusaderssystems@gmail.com"
                        },
                        new
                        {
                            Id = "fd064d4e-7457-4287-a3f4-5b99580ef2ab",
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "e986e8e6-f748-4524-ae50-7c6360086395",
                            Email = "cloudcrusaderssystems+pharmacist@gmail.com",
                            EmailConfirmed = false,
                            LockoutEnabled = false,
                            NormalizedEmail = "CLOUDCRUSADERSSYSTEMS+PHARMACIST@GMAIL.COM",
                            NormalizedUserName = "CLOUDCRUSADERSSYSTEMS+PHARMACIST@GMAIL.COM",
                            PasswordHash = "AQAAAAEAACcQAAAAECjNQIdDn0uDxl97ci6jgJRc4an4ZrM2FbxNs/fab4Pwh2aUu0QRU+S/Wmu/4VAUKw==",
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "f3940753-4cdc-4c05-8ae1-6966ba93d934",
                            TwoFactorEnabled = false,
                            UserName = "cloudcrusaderssystems+pharmacist@gmail.com"
                        },
                        new
                        {
                            Id = "01734a51-05b1-4c95-8d21-6820014332e9",
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "706765a7-28ac-4ad4-8935-a760d8eeb9a9",
                            Email = "cloudcrusaderssystems+technician@gmail.com",
                            EmailConfirmed = false,
                            LockoutEnabled = false,
                            NormalizedEmail = "CLOUDCRUSADERSSYSTEMS+TECHNICIAN@GMAIL.COM",
                            NormalizedUserName = "CLOUDCRUSADERSSYSTEMS+TECHNICIAN@GMAIL.COM",
                            PasswordHash = "AQAAAAEAACcQAAAAEEblYKMALLLQZcwA8o3PGBvAb7J427YkLmH/DLhtDGHxFUj/PzCIhepUYk6s+FvrxA==",
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "93f594c0-4bd9-4555-8c72-18de913cda5a",
                            TwoFactorEnabled = false,
                            UserName = "cloudcrusaderssystems+technician@gmail.com"
                        },
                        new
                        {
                            Id = "c299b237-a197-454d-b474-587e7fe61656",
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "13e0da14-6a03-475b-9cb5-7de13d6e1295",
                            Email = "cloudcrusaderssystems+general.practitioner@gmail.com",
                            EmailConfirmed = false,
                            LockoutEnabled = false,
                            NormalizedEmail = "CLOUDCRUSADERSSYSTEMS+GENERAL.PRACTITIONER@GMAIL.COM",
                            NormalizedUserName = "CLOUDCRUSADERSSYSTEMS+GENERAL.PRACTITIONER@GMAIL.COM",
                            PasswordHash = "AQAAAAEAACcQAAAAECwvOvvOOOcwIN6kmx5e+V1ue1V7Zc+shUUqU0MlWLn0ARIu3m+6DLLq+3sqTGOdlg==",
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "fcdee77f-0cd1-4fe0-82a5-77fb2bfb0bbe",
                            TwoFactorEnabled = false,
                            UserName = "cloudcrusaderssystems+general.practitioner@gmail.com"
                        },
                        new
                        {
                            Id = "250f3fea-59bd-4f65-ba6a-a08b7afad55a",
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "302af0a2-7197-4c26-9fa8-14ab10061f51",
                            Email = "cloudcrusaderssystems+patient@gmail.com",
                            EmailConfirmed = false,
                            LockoutEnabled = false,
                            NormalizedEmail = "CLOUDCRUSADERSSYSTEMS+PATIENT@GMAIL.COM",
                            NormalizedUserName = "CLOUDCRUSADERSSYSTEMS+PATIENT@GMAIL.COM",
                            PasswordHash = "AQAAAAEAACcQAAAAEBea/EhS/KCwmHrs/I0N9XocNVZEhd/wzeJB8tR4PHc94AldxleS08vaJxVy/RaKoA==",
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "379e1c18-8f4a-45f5-b47d-04b01c0af6d4",
                            TwoFactorEnabled = false,
                            UserName = "cloudcrusaderssystems+patient@gmail.com"
                        },
                        new
                        {
                            Id = "33a728ad-f9f0-414b-a0d7-4d3cda8dbd6b",
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "7a8266bc-c78e-461c-867d-adc4e1bc073c",
                            Email = "cloudcrusaderssystems+authorised.carer@gmail.com",
                            EmailConfirmed = false,
                            LockoutEnabled = false,
                            NormalizedEmail = "CLOUDCRUSADERSSYSTEMS+AUTHORISED.CARER@GMAIL.COM",
                            NormalizedUserName = "CLOUDCRUSADERSSYSTEMS+AUTHORISED.CARER@GMAIL.COM",
                            PasswordHash = "AQAAAAEAACcQAAAAEHXkN1vsFuhRDXz4TEW+rv+b+Cs1enulWo6enKtPIlyzrUdILVn9ia8Xj5+BX4KNYw==",
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "945befd3-2621-44f7-834f-a3b6389f2ea6",
                            TwoFactorEnabled = false,
                            UserName = "cloudcrusaderssystems+authorised.carer@gmail.com"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");

                    b.HasData(
                        new
                        {
                            Id = "a0abf262-1a77-4b9d-bac5-ec293928f9ae",
                            ConcurrencyStamp = "d33f1ace-4e33-474d-aca9-874a9b145c32",
                            Name = "Pharmacist",
                            NormalizedName = "PHARMACIST"
                        },
                        new
                        {
                            Id = "5cf92bcd-61c7-40be-bf40-857cd7e94679",
                            ConcurrencyStamp = "945881ad-1491-4a5c-b92a-ad5941fbb8b7",
                            Name = "Technician",
                            NormalizedName = "TECHNICIAN"
                        },
                        new
                        {
                            Id = "dac4ae7a-4b01-4865-8f3d-66e4cb0bdb42",
                            ConcurrencyStamp = "4c35b654-2e49-439e-b2c5-e2bfead9cc23",
                            Name = "General Practitioner",
                            NormalizedName = "GENERAL PRACTITIONER"
                        },
                        new
                        {
                            Id = "7bdb12d3-caf8-4d43-a2e9-ef6ebe8f4b31",
                            ConcurrencyStamp = "4b73f4f6-8e56-41fe-bca9-43c5c932dac1",
                            Name = "Admin",
                            NormalizedName = "ADMIN"
                        },
                        new
                        {
                            Id = "89363d4b-e187-4c02-8959-c3fa597d0846",
                            ConcurrencyStamp = "7687c2be-3203-462f-9174-45b8f3ac33b8",
                            Name = "Patient",
                            NormalizedName = "PATIENT"
                        },
                        new
                        {
                            Id = "4d2715ee-88a0-4631-8339-cf24311bafbc",
                            ConcurrencyStamp = "7d6f205b-86e1-4d1f-a1e6-2741a27d84a5",
                            Name = "Authorised Carer",
                            NormalizedName = "AUTHORISED CARER"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");

                    b.HasData(
                        new
                        {
                            UserId = "be2497f5-ab1f-4824-9a94-a14747bcccd7",
                            RoleId = "7bdb12d3-caf8-4d43-a2e9-ef6ebe8f4b31"
                        },
                        new
                        {
                            UserId = "fd064d4e-7457-4287-a3f4-5b99580ef2ab",
                            RoleId = "a0abf262-1a77-4b9d-bac5-ec293928f9ae"
                        },
                        new
                        {
                            UserId = "01734a51-05b1-4c95-8d21-6820014332e9",
                            RoleId = "5cf92bcd-61c7-40be-bf40-857cd7e94679"
                        },
                        new
                        {
                            UserId = "c299b237-a197-454d-b474-587e7fe61656",
                            RoleId = "dac4ae7a-4b01-4865-8f3d-66e4cb0bdb42"
                        },
                        new
                        {
                            UserId = "250f3fea-59bd-4f65-ba6a-a08b7afad55a",
                            RoleId = "89363d4b-e187-4c02-8959-c3fa597d0846"
                        },
                        new
                        {
                            UserId = "33a728ad-f9f0-414b-a0d7-4d3cda8dbd6b",
                            RoleId = "4d2715ee-88a0-4631-8339-cf24311bafbc"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("AADWebApp.Areas.Identity.Data.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("AADWebApp.Areas.Identity.Data.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AADWebApp.Areas.Identity.Data.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("AADWebApp.Areas.Identity.Data.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}