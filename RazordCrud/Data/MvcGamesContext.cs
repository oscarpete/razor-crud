using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RazordCrud.Models;

namespace RazordCrud.Data
{
    public class MvcGamesContext : DbContext
    {
        public MvcGamesContext(DbContextOptions<MvcGamesContext> options)
            : base(options)
        {
        }

        public DbSet<Games> Games { get; set; }
    }
}
