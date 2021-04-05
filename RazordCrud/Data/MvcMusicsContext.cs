using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RazordCrud.Models;

namespace RazordCrud.Data
{
    public class MvcMusicsContext : DbContext
    {
        public MvcMusicsContext(DbContextOptions<MvcMusicsContext> options)
            : base(options)
        {
        }

        public DbSet<Musics> Musics { get; set; }
    }
}
