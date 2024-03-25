﻿// <auto-generated />
using System;
using BusinessObject.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BusinessObject.Migrations
{
    [DbContext(typeof(ConnectDB))]
    [Migration("20240325131441_Indit")]
    partial class Indit
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.15")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BusinessObject.Model.Activate", b =>
                {
                    b.Property<int>("Activateid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Activateid"));

                    b.Property<DateTime>("Datepost")
                        .HasColumnType("datetime2");

                    b.Property<string>("NameActivate")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Activateid");

                    b.ToTable("Activate");
                });

            modelBuilder.Entity("BusinessObject.Model.Bloodbank", b =>
                {
                    b.Property<int>("Bloodbankid")
                        .HasColumnType("int");

                    b.Property<string>("NameBloodbank")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Bloodbankid");

                    b.ToTable("Bloodbank");
                });

            modelBuilder.Entity("BusinessObject.Model.Bloodtypes", b =>
                {
                    b.Property<int>("Bloodtypeid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Bloodtypeid"));

                    b.Property<string>("NameBlood")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Bloodtypeid");

                    b.ToTable("Bloodtypes");

                    b.HasData(
                        new
                        {
                            Bloodtypeid = 1,
                            NameBlood = "A"
                        },
                        new
                        {
                            Bloodtypeid = 2,
                            NameBlood = "B"
                        },
                        new
                        {
                            Bloodtypeid = 3,
                            NameBlood = "AB"
                        },
                        new
                        {
                            Bloodtypeid = 4,
                            NameBlood = "O"
                        });
                });

            modelBuilder.Entity("BusinessObject.Model.Hospitals", b =>
                {
                    b.Property<int>("Hospitalid")
                        .HasColumnType("int");

                    b.Property<int>("Bloodbankid")
                        .HasColumnType("int");

                    b.Property<string>("NameHospital")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Hospitalid");

                    b.HasIndex("Bloodbankid");

                    b.ToTable("Hospitals");
                });

            modelBuilder.Entity("BusinessObject.Model.Images", b =>
                {
                    b.Property<int>("ImgId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ImgId"));

                    b.Property<int>("Activateid")
                        .HasColumnType("int");

                    b.Property<string>("Img")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ImgId");

                    b.HasIndex("Activateid");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("BusinessObject.Model.Notification", b =>
                {
                    b.Property<int>("NotificationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("NotificationId"));

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Datepost")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Userid")
                        .HasColumnType("int");

                    b.Property<int>("status")
                        .HasColumnType("int");

                    b.HasKey("NotificationId");

                    b.HasIndex("Userid");

                    b.ToTable("Notification");
                });

            modelBuilder.Entity("BusinessObject.Model.NumberBlood", b =>
                {
                    b.Property<int>("numberbloodid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("numberbloodid"));

                    b.Property<int>("quantity")
                        .HasColumnType("int");

                    b.HasKey("numberbloodid");

                    b.ToTable("NumberBlood");

                    b.HasData(
                        new
                        {
                            numberbloodid = 1,
                            quantity = 250
                        },
                        new
                        {
                            numberbloodid = 2,
                            quantity = 350
                        },
                        new
                        {
                            numberbloodid = 3,
                            quantity = 450
                        });
                });

            modelBuilder.Entity("BusinessObject.Model.QuantitySend", b =>
                {
                    b.Property<int>("quantitysendid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("quantitysendid"));

                    b.Property<int>("Bloodtypeid")
                        .HasColumnType("int");

                    b.Property<int>("SendBloodid")
                        .HasColumnType("int");

                    b.Property<int>("numberbloodid")
                        .HasColumnType("int");

                    b.Property<int>("quantity")
                        .HasColumnType("int");

                    b.HasKey("quantitysendid");

                    b.HasIndex("Bloodtypeid");

                    b.HasIndex("SendBloodid");

                    b.HasIndex("numberbloodid");

                    b.ToTable("QuantitySend");
                });

            modelBuilder.Entity("BusinessObject.Model.QuantityTake", b =>
                {
                    b.Property<int>("quantitytakeid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("quantitytakeid"));

                    b.Property<int>("Bloodtypeid")
                        .HasColumnType("int");

                    b.Property<int>("Takebloodid")
                        .HasColumnType("int");

                    b.Property<int>("numberbloodid")
                        .HasColumnType("int");

                    b.Property<int>("quantity")
                        .HasColumnType("int");

                    b.HasKey("quantitytakeid");

                    b.HasIndex("Bloodtypeid");

                    b.HasIndex("Takebloodid");

                    b.HasIndex("numberbloodid");

                    b.ToTable("QuantityTake");
                });

            modelBuilder.Entity("BusinessObject.Model.Registers", b =>
                {
                    b.Property<int>("RegisterId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RegisterId"));

                    b.Property<int>("Bloodtypeid")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<int>("Requestid")
                        .HasColumnType("int");

                    b.Property<int>("Volunteerid")
                        .HasColumnType("int");

                    b.HasKey("RegisterId");

                    b.HasIndex("Bloodtypeid");

                    b.HasIndex("Requestid");

                    b.HasIndex("Volunteerid");

                    b.ToTable("Registers");
                });

            modelBuilder.Entity("BusinessObject.Model.Requests", b =>
                {
                    b.Property<int>("Requestid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Requestid"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Contact")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("District")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Endtime")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Hospitalid")
                        .HasColumnType("int");

                    b.Property<DateTime>("RequestDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Starttime")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Ward")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("img")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("quantity")
                        .HasColumnType("int");

                    b.Property<int>("status")
                        .HasColumnType("int");

                    b.HasKey("Requestid");

                    b.HasIndex("Hospitalid");

                    b.ToTable("Requests");
                });

            modelBuilder.Entity("BusinessObject.Model.SendBlood", b =>
                {
                    b.Property<int>("SendBloodid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SendBloodid"));

                    b.Property<int>("Bloodbankid")
                        .HasColumnType("int");

                    b.Property<DateTime>("Datesend")
                        .HasColumnType("datetime2");

                    b.Property<int>("Hospitalid")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("SendBloodid");

                    b.HasIndex("Bloodbankid");

                    b.HasIndex("Hospitalid");

                    b.ToTable("SendBlood");
                });

            modelBuilder.Entity("BusinessObject.Model.Takebloods", b =>
                {
                    b.Property<int>("Takebloodid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Takebloodid"));

                    b.Property<int>("Bloodbankid")
                        .HasColumnType("int");

                    b.Property<DateTime>("Datetake")
                        .HasColumnType("datetime2");

                    b.Property<int>("Hospitalid")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Takebloodid");

                    b.HasIndex("Bloodbankid");

                    b.HasIndex("Hospitalid");

                    b.ToTable("Takebloods");
                });

            modelBuilder.Entity("BusinessObject.Model.Users", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("District")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Img")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<string>("Ward")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("deactive")
                        .HasColumnType("int");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("BusinessObject.Model.Volunteers", b =>
                {
                    b.Property<int>("Volunteerid")
                        .HasColumnType("int");

                    b.Property<DateTime>("Birthdate")
                        .HasColumnType("datetime2");

                    b.Property<string>("CCCD")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Fullname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Gender")
                        .HasColumnType("int");

                    b.HasKey("Volunteerid");

                    b.ToTable("Volunteers");
                });

            modelBuilder.Entity("BusinessObject.Model.Bloodbank", b =>
                {
                    b.HasOne("BusinessObject.Model.Users", "Users")
                        .WithOne("Bloodbank")
                        .HasForeignKey("BusinessObject.Model.Bloodbank", "Bloodbankid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Users");
                });

            modelBuilder.Entity("BusinessObject.Model.Hospitals", b =>
                {
                    b.HasOne("BusinessObject.Model.Bloodbank", "Bloodbank")
                        .WithMany("Hospitals")
                        .HasForeignKey("Bloodbankid")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("BusinessObject.Model.Users", "Users")
                        .WithOne("Hospitals")
                        .HasForeignKey("BusinessObject.Model.Hospitals", "Hospitalid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Bloodbank");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("BusinessObject.Model.Images", b =>
                {
                    b.HasOne("BusinessObject.Model.Activate", "Activate")
                        .WithMany("Images")
                        .HasForeignKey("Activateid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Activate");
                });

            modelBuilder.Entity("BusinessObject.Model.Notification", b =>
                {
                    b.HasOne("BusinessObject.Model.Users", "Users")
                        .WithMany("Notifications")
                        .HasForeignKey("Userid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Users");
                });

            modelBuilder.Entity("BusinessObject.Model.QuantitySend", b =>
                {
                    b.HasOne("BusinessObject.Model.Bloodtypes", "Bloodtypes")
                        .WithMany("QuantitySend")
                        .HasForeignKey("Bloodtypeid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BusinessObject.Model.SendBlood", "SendBlood")
                        .WithMany("QuantitySends")
                        .HasForeignKey("SendBloodid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BusinessObject.Model.NumberBlood", "NumberBlood")
                        .WithMany("QuantitySends")
                        .HasForeignKey("numberbloodid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Bloodtypes");

                    b.Navigation("NumberBlood");

                    b.Navigation("SendBlood");
                });

            modelBuilder.Entity("BusinessObject.Model.QuantityTake", b =>
                {
                    b.HasOne("BusinessObject.Model.Bloodtypes", "Bloodtypes")
                        .WithMany("QuantityTake")
                        .HasForeignKey("Bloodtypeid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BusinessObject.Model.Takebloods", "Takebloods")
                        .WithMany("QuantityTake")
                        .HasForeignKey("Takebloodid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BusinessObject.Model.NumberBlood", "NumberBlood")
                        .WithMany("QuantityTake")
                        .HasForeignKey("numberbloodid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Bloodtypes");

                    b.Navigation("NumberBlood");

                    b.Navigation("Takebloods");
                });

            modelBuilder.Entity("BusinessObject.Model.Registers", b =>
                {
                    b.HasOne("BusinessObject.Model.Bloodtypes", "Bloodtypes")
                        .WithMany("Registers")
                        .HasForeignKey("Bloodtypeid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BusinessObject.Model.Requests", "Requests")
                        .WithMany("Registers")
                        .HasForeignKey("Requestid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BusinessObject.Model.Volunteers", "Volunteers")
                        .WithMany("Registers")
                        .HasForeignKey("Volunteerid")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Bloodtypes");

                    b.Navigation("Requests");

                    b.Navigation("Volunteers");
                });

            modelBuilder.Entity("BusinessObject.Model.Requests", b =>
                {
                    b.HasOne("BusinessObject.Model.Hospitals", "Hospitals")
                        .WithMany("Requests")
                        .HasForeignKey("Hospitalid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Hospitals");
                });

            modelBuilder.Entity("BusinessObject.Model.SendBlood", b =>
                {
                    b.HasOne("BusinessObject.Model.Bloodbank", "Bloodbank")
                        .WithMany("SendBloods")
                        .HasForeignKey("Bloodbankid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BusinessObject.Model.Hospitals", "Hospitals")
                        .WithMany("SendBloods")
                        .HasForeignKey("Hospitalid")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Bloodbank");

                    b.Navigation("Hospitals");
                });

            modelBuilder.Entity("BusinessObject.Model.Takebloods", b =>
                {
                    b.HasOne("BusinessObject.Model.Bloodbank", "Bloodbank")
                        .WithMany("Takebloods")
                        .HasForeignKey("Bloodbankid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BusinessObject.Model.Hospitals", "Hospitals")
                        .WithMany("Takebloods")
                        .HasForeignKey("Hospitalid")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Bloodbank");

                    b.Navigation("Hospitals");
                });

            modelBuilder.Entity("BusinessObject.Model.Volunteers", b =>
                {
                    b.HasOne("BusinessObject.Model.Users", "Users")
                        .WithOne("Volunteers")
                        .HasForeignKey("BusinessObject.Model.Volunteers", "Volunteerid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Users");
                });

            modelBuilder.Entity("BusinessObject.Model.Activate", b =>
                {
                    b.Navigation("Images");
                });

            modelBuilder.Entity("BusinessObject.Model.Bloodbank", b =>
                {
                    b.Navigation("Hospitals");

                    b.Navigation("SendBloods");

                    b.Navigation("Takebloods");
                });

            modelBuilder.Entity("BusinessObject.Model.Bloodtypes", b =>
                {
                    b.Navigation("QuantitySend");

                    b.Navigation("QuantityTake");

                    b.Navigation("Registers");
                });

            modelBuilder.Entity("BusinessObject.Model.Hospitals", b =>
                {
                    b.Navigation("Requests");

                    b.Navigation("SendBloods");

                    b.Navigation("Takebloods");
                });

            modelBuilder.Entity("BusinessObject.Model.NumberBlood", b =>
                {
                    b.Navigation("QuantitySends");

                    b.Navigation("QuantityTake");
                });

            modelBuilder.Entity("BusinessObject.Model.Requests", b =>
                {
                    b.Navigation("Registers");
                });

            modelBuilder.Entity("BusinessObject.Model.SendBlood", b =>
                {
                    b.Navigation("QuantitySends");
                });

            modelBuilder.Entity("BusinessObject.Model.Takebloods", b =>
                {
                    b.Navigation("QuantityTake");
                });

            modelBuilder.Entity("BusinessObject.Model.Users", b =>
                {
                    b.Navigation("Bloodbank")
                        .IsRequired();

                    b.Navigation("Hospitals")
                        .IsRequired();

                    b.Navigation("Notifications");

                    b.Navigation("Volunteers")
                        .IsRequired();
                });

            modelBuilder.Entity("BusinessObject.Model.Volunteers", b =>
                {
                    b.Navigation("Registers");
                });
#pragma warning restore 612, 618
        }
    }
}