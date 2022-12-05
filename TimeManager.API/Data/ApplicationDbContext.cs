#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TimeManager.API.Models;

namespace TimeManager.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext (DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<TimeManager.API.Models.Employee> Employee { get; set; }

        public DbSet<TimeManager.API.Models.Timeregistration> Timeregistration { get; set; }
    }
}
