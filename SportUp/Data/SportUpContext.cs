using Microsoft.EntityFrameworkCore;
using SportUp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportUp.Data
{
    public class SportUpContext : DbContext
    {
        

        public SportUpContext(DbContextOptions<SportUpContext> options) : base(options)
        {

        }
    }
}
