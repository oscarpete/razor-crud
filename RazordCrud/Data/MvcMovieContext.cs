using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RazordCrud.Models;
using Microsoft.EntityFrameworkCore;

namespace RazordCrud.Data
{
    public class MvcMovieContext : DbContext
    {
        public MvcMovieContext(DbContextOptions<MvcMovieContext> options)
            : base(options)
        {
        }

        public DbSet<Movie> Movie { get; set; }

        public DbSet<RazordCrud.Models.Games> Games { get; set; }

        public DbSet<RazordCrud.Models.Musics> Musics { get; set; }
    }
}
