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
    }
}
