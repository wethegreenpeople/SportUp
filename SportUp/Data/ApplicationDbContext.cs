using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SportUp.Data.Models;

namespace SportUp.Data
{
    public class ApplicationDbContext : IdentityDbContext<SportUpUser>
    {
        public DbSet<Sport> Sports { get; set; }
        public DbSet<Team> Teams { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Join table between SportUpUsers and Sports
            builder.Entity<UserSport>()
            .HasKey(us => new { us.SportId, us.SportUpUserId });
            builder.Entity<UserSport>()
                .HasOne(s => s.Sport)
                .WithMany(u => u.UserSports)
                .HasForeignKey(s => s.SportId);
            builder.Entity<UserSport>()
                .HasOne(s => s.SportUpUser)
                .WithMany(s => s.UserSports)
                .HasForeignKey(s => s.SportUpUserId);

            // Join table between SportUpUsers and Teams
            builder.Entity<UserTeam>()
            .HasKey(s => new { s.SportUpUserId, s.TeamId });
            builder.Entity<UserTeam>()
                .HasOne(s => s.Team)
                .WithMany(u => u.UserTeams)
                .HasForeignKey(s => s.TeamId);
            builder.Entity<UserTeam>()
                .HasOne(s => s.SportUpUser)
                .WithMany(s => s.UserTeams)
                .HasForeignKey(s => s.SportUpUserId);

            // One to many relationship between a team and a sport
            builder.Entity<Team>()
                .HasOne(s => s.TeamSportType)
                .WithMany(s => s.Teams);
        }
    }
}
