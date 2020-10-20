using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NGTI.Models;

namespace NGTI.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        // tabllen die hij gaat aanmaken in de database dbset pakt de models
        public DbSet<Table> Tables { get; set; }
        public DbSet<GroupReservation> GroupReservations { get; set; }
        public DbSet<SoloReservation> SoloReservations { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<GroupReservation>().HasOne(groupreservation => groupreservation.Table).WithMany(table => table.GroupReservations); }
        // dit is voor de zekerheid dat de verbinding vast staat
    }
}
