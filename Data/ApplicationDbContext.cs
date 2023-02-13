using BoardGamePickerWithAngularJS.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoardGamePickerWithAngularJS.Data
{
    public class ApplicationDbContext : DbContext
    {
        private readonly DbContextOptions<ApplicationDbContext> _options;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            _options = options;
        }

        public DbSet<Game> Games { get; set; }
        public DbSet<User> Users { get; set; }

    }
}
