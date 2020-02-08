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
        public DbSet<Sport> Sports { get; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

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
        }
    }
}
