using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NGTI.Models;

namespace NGTI.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public ApplicationDbContext()
        {
        }

        // tabllen die hij gaat aanmaken in de database dbset pakt de models
        //public DbSet<Table> Tables { get; set; }
        public DbSet<ApplicationUser> AspNetUsers { get; set; }
        public DbSet<TeamMember> TeamMembers { get; set; }
        public DbSet<GroupReservation> GroupReservations { get; set; }
        public DbSet<SoloReservation> SoloReservations { get; set; }
        public DbSet<Teams> Teams { get; set; }

        public DbSet<Limit> Limit { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<Teams>().HasOne(team => team.TeamMembers).WithMany(teammembers => teammembers.Teams).OnDelete(DeleteBehavior.Cascade);
            //modelBuilder.Entity<Teams>().HasOne(t => t.TeamMembers).WithMany(tm => tm.Teams).HasForeignKey(t => t.TeamName).IsRequired(true).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<TeamMember>().HasKey(tm => new { tm.TeamName, tm.UserId });
            modelBuilder.Entity<TeamMember>().HasOne(t => t.Teams).WithMany(tm => tm.TeamMembers);
            modelBuilder.Entity<TeamMember>().HasOne(a => a.ApplicationUser).WithMany(tm => tm.TeamMembers);
            //modelBuilder.Entity<GroupReservation>().HasOne(groupreservation => groupreservation.Table).WithMany(table => table.GroupReservations);
            //modelBuilder.Entity<GroupReservation>().HasOne(groupreservation => groupreservation.TeamName).WithMany(Teams => Teams.GroupReservations);
            //modelBuilder.Entity<SoloReservation>().HasOne(soloreservation => soloreservation.Table).WithMany(table => table.SoloReservations);

            //modelBuilder.Entity<TeamMembers>().HasKey(tm => new { tm.TeamName, tm.UserId });
        }
        public DbSet<NGTI.Models.Team> Team { get; set; }


        // dit is voor de zekerheid dat de verbinding vast staat
    }
}