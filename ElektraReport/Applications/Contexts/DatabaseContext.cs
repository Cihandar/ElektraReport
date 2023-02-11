using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElektraReport.Models;
using Microsoft.EntityFrameworkCore;

namespace ElektraReport.Applications.Context
{
    public class DatabaseContext : IdentityDbContext<AppUser>
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }
        public DatabaseContext()
        {



        }

        public DbSet<Company> Companys { get; set; }
        public DbSet<DepremKayit> DepremKayits{ get; set; }
    }
}
