using LRDT.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LRDT.Database
{
    public class LRDTContext : DbContext
    {
        public DbSet<Rendeles> Rendeles { get; set; }
        public DbSet<Asztal> Asztal { get; set; }
        public DbSet<Tetel> Tetel { get; set; }
        public DbSet<Pincer> Pincer { get; set; }
        public DbSet<FizetesiMod> FizetesiMod { get; set; }
        public DbSet<RendelesTetel> RendelesTetel { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RendelesTetel>()
                .HasKey(rt => new { rt.TetelId, rt.RendelesId });

            modelBuilder.Entity<RendelesTetel>()
                .HasOne(rt => rt.Rendeles)
                .WithMany(r => r.RendelesTetels)
                .HasForeignKey(rt => rt.RendelesId);

            modelBuilder.Entity<RendelesTetel>()
                .HasOne(rt => rt.Tetel)
                .WithMany()
                .HasForeignKey(rt => rt.TetelId);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL("server=localhost;port=3307;username=root;password=;database=lrdt");
        }
    }
}
