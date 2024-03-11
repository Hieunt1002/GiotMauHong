using Azure.Core;
using BusinessObject.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace BusinessObject.Context
{
    public class ConnectDB : DbContext
    {
        public ConnectDB() { }
        public ConnectDB(DbContextOptions<ConnectDB> options) : base(options) { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfigurationRoot configuration = builder.Build();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("GiotMauHong"));
        }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<Volunteers> Volunteers { get; set; }
        public virtual DbSet<Hospitals> Hospitals { get; set; }
        public virtual DbSet<Bloodbank> Bloodbank { get; set; }
        public virtual DbSet<Registers> Registers { get; set; }
        public virtual DbSet<Requests> Requests { get; set; }
        public virtual DbSet<Takebloods> Takebloods { get; set; }
        public virtual DbSet<SendBlood> SendBlood { get; set; }
        public virtual DbSet<Notification> Notification { get; set; }
        public virtual DbSet<Activate> Activate { get; set; }
        public virtual DbSet<Images> Images { get; set; }
        public virtual DbSet<Bloodtypes> Bloodtypes { get; set; }
        public virtual DbSet<NumberBlood> NumberBlood { get; set; }
        public virtual DbSet<QuantitySend> QuantitySend { get; set; }
        public virtual DbSet<QuantityTake> QuantityTake { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Registers>()
                .HasOne(r => r.Volunteers)
                .WithMany(v => v.Registers)
                .HasForeignKey(r => r.Volunteerid)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<SendBlood>()
                .HasOne(sb => sb.Hospitals)
                .WithMany(h => h.SendBloods)
                .HasForeignKey(sb => sb.Hospitalid) 
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Takebloods>()
                .HasOne(tb => tb.Hospitals)
                .WithMany(h => h.Takebloods)
                .HasForeignKey(tb => tb.Hospitalid) 
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Hospitals>()
                .HasOne(tb => tb.Bloodbank)
                .WithMany(h => h.Hospitals)
                .HasForeignKey(tb => tb.Bloodbankid)
                .OnDelete(DeleteBehavior.NoAction);

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<NumberBlood>().HasData(
                new NumberBlood { numberbloodid = 1, quantity = 250},
                new NumberBlood { numberbloodid = 2, quantity = 350},
                new NumberBlood { numberbloodid = 3, quantity = 450}
                );
            modelBuilder.Entity<Bloodtypes>().HasData(
                new Bloodtypes { Bloodtypeid = 1, NameBlood = "A" },
                new Bloodtypes { Bloodtypeid = 2, NameBlood = "B" },
                new Bloodtypes { Bloodtypeid = 3, NameBlood = "AB" },
                new Bloodtypes { Bloodtypeid = 4, NameBlood = "O" }
                );
        }


    }
}
